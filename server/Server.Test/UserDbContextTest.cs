using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Server.DB.UserDb;
using Server.Shared.Core;
using Server.Shared.Models;
using Server.Shared.Options;
using static Server.Host.Program;
using Xunit;

namespace Server.Test
{
    public class UserDbContextTest
    {
        private static UserDbContext db = new UserDbContext(new DbOptions()
        {
            UserDbPath = "./user.db",
            CollectionName = "users"
        });

        [Fact]
        public void Test()
        {
            var u = new User()
            {
                UId = "001",
                Email = null,
                Name = "a"
            };
            db.AddUser(u);
            Assert.True(db.Users.FirstOrDefault(x => x.UId == u.UId) != null);
            Assert.True(db.FindUser("001") != null);
            u.Name = "b";
            db.UpdateUser(u);
            Assert.True(db.FindUser("001").Name == "b");
            db.DeleteUser(u);
            Assert.True(db.FindUser("001") == null);
        }
    }
}
