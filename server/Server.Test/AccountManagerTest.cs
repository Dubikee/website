using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Server.Shared.Core;
using Server.Shared.Models;
using Server.Shared.Options;
using Server.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Server.Service.Auth;
using Xunit;

namespace Server.Test
{
    public class AccountManagerTest
    {
        private readonly List<User> _users = new List<User>
        {
            new User()
            {
                Uid = "00000001",
                Name = "a",
                Role = RoleTypes.Master,
                PwHash = User.MakePwdHash("abcd1234")
            },
            new User()
            {
                Uid = "00000002",
                Name = "b",
                Role = RoleTypes.Admin,
                PwHash = User.MakePwdHash("abcd1234")
            },
            new User()
            {
                Uid = "00000003",
                Name = "c",
                Role = RoleTypes.Vistor,
                PwHash = User.MakePwdHash("abcd1234")
            }
        };

        private readonly JwtOptions _opt = new JwtOptions()
        {
            Key = "w3ufr3vt7i08gustwyw9",
            Audience = "w3ufr3vt7i08g",
            Issuer = "w3ufr3vt7i08g",
            Expires = TimeSpan.FromDays(10)
        };

        private readonly TestUserDbContext _db;
        private readonly TestContextAccessor _masterctx;
        private readonly TestContextAccessor _adminctx;
        private readonly TestContextAccessor _vistorctx;
        private readonly TestContextAccessor _anoctx;

        public AccountManagerTest()
        {
            _db = new TestUserDbContext(_users);
            _masterctx = new TestContextAccessor(_users[0].Uid);
            _adminctx = new TestContextAccessor(_users[1].Uid);
            _vistorctx = new TestContextAccessor(_users[2].Uid);
            _anoctx = new TestContextAccessor("");

        }

        /// <summary>
        /// 登陆测试
        /// </summary>
        [Fact]
        public void LoginTest()
        {
            var m = new AccountManager(_db, _masterctx, _opt);

            Assert.Equal(_users[0], m.User);
            Assert.True(m.Login(null, null).status == AuthStatus.ParamsIsEmpty);
            Assert.True(m.Login(_users[1].Uid, "____").status == AuthStatus.PasswordWrong);
            Assert.True(m.Login("____", "____").status == AuthStatus.UIdNotFind);
            Assert.True(m.Login(_users[1].Uid, "abcd1234").status == AuthStatus.Ok);
            Assert.Equal(m.User, _users[1]);
        }

        /// <summary>
        /// 注册测试
        /// </summary>
        [Fact]
        public void RegisterTest()
        {
            var m = new AccountManager(_db, _anoctx, _opt);
            Assert.Null(m.User);
            Assert.True(m.Register(" ", " ", " ").status == AuthStatus.ParamsIsEmpty);
            Assert.True(m.Register("0000", "d", "abcd1234").status == AuthStatus.UidTooShort);
            Assert.True(m.Register("0000000a", "d", "abcd1234").status == AuthStatus.UidIsNotNumbers);
            Assert.True(m.Register("00000004", "d", "1234").status == AuthStatus.PasswordTooShort);
            Assert.True(m.Register("00000004", "d", "12345678").status == AuthStatus.PasswordNoLetters);
            Assert.True(m.Register("00000004", "d", "abcdefgh").status == AuthStatus.PasswordNoNumbers);
            Assert.True(m.Register("00000001", "d", "abcd1234").status == AuthStatus.UidHasExist);
            Assert.True(m.Register("00000004", "d", "abcd1234").status == AuthStatus.Ok);
            Assert.NotNull(_db.Users.FirstOrDefault(x => x.Uid == "00000004"));
        }

        /// <summary>
        /// 删除测试
        /// </summary>
        [Fact]
        public void DeleteUserTest()
        {
            var m = new AccountManager(_db, _anoctx, _opt);
            Assert.Null(m.User);
            Assert.True(m.DeleteUser() == AuthStatus.TokenExpired);
            m = new AccountManager(_db, _vistorctx, _opt);
            Assert.True(m.DeleteUser() == AuthStatus.Ok);
            Assert.DoesNotContain(_db.Users, x => x.Name == "c");
        }

        /// <summary>
        /// 更改测试
        /// </summary>
        [Fact]
        public void UpdateUserTest()
        {
            var m = new AccountManager(_db, _anoctx, _opt);
            Assert.True(m.UpdateUserInfo("a", null, null) == AuthStatus.TokenExpired);
            m = new AccountManager(_db, _adminctx, _opt);
        }

        /// <summary>
        /// 测试Context类
        /// </summary>
        private class TestHttpContext : HttpContext
        {
            public override void Abort()
            {
                throw new NotImplementedException();
            }

            public override IFeatureCollection Features { get; }
            public override HttpRequest Request { get; }
            public override HttpResponse Response { get; }
            public override ConnectionInfo Connection { get; }
            public override WebSocketManager WebSockets { get; }
            public override AuthenticationManager Authentication { get; }
            public override ClaimsPrincipal User { get; set; }
            public override IDictionary<object, object> Items { get; set; }
            public override IServiceProvider RequestServices { get; set; }
            public override CancellationToken RequestAborted { get; set; }
            public override string TraceIdentifier { get; set; }
            public override ISession Session { get; set; }
        }

        /// <summary>
        /// 测试数据库类
        /// </summary>
        private class TestUserDbContext : IUserDbContext<User>
        {
            private List<User> _users;

            public TestUserDbContext(List<User> users)
            {
                _users = users;
            }

            public IEnumerable<User> Users => _users;

            public void AddUser(User user)
            {
                _users.Add(user);
            }

            public User FindUser(string uid)
            {
                return _users.Find(x => x.Uid == uid);

            }

            public bool UpdateUser(User user)
            {
                return true;
            }

            public bool DeleteUser(User user)
            {
                return _users.Remove(user);
            }
        }

        /// <summary>
        /// 测试HttpContextAccessor类
        /// </summary>
        private class TestContextAccessor : IHttpContextAccessor
        {
            public HttpContext HttpContext { get; set; }

            public TestContextAccessor(string uid)
            {
                var claims = new[] {new Claim(AccountManager.UidClaimType, uid)};
                var identity = new ClaimsIdentity(claims);
                HttpContext = new TestHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                };
            }
        }
    }
}
