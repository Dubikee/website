using Server.DB.UserDb;
using Server.Shared.Models;
using Server.Shared.Options;
using System.Linq;
using Xunit;

namespace Server.Test
{
    /// <summary>
    /// UseerDb测试
    /// </summary>
    public class UserDbContextTest
    {
        private static readonly UserDbContext Db = new UserDbContext(new DbOptions()
        {
            UserDbPath = "./user.db",
            CollectionName = "users"
        });

        [Fact]
        public void Test()
        {
            var u = new User()
            {
                Uid = "001",
                Email = null,
                Name = "a"
            };
            Db.AddUser(u);
            Assert.True(Db.Users.FirstOrDefault(x => x.Uid == u.Uid) != null);
            Assert.True(Db.FindUser("001") != null);
            u.Name = "b";
            Db.UpdateUser(u);
            Assert.True(Db.FindUser("001").Name == "b");
            Db.DeleteUser(u);
            Assert.True(Db.FindUser("001") == null);
        }
    }
}
