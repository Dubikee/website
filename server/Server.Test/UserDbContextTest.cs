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
                Uid = "001",
                Email = null,
                Name = "a"
            };
            db.AddUser(u);
            Assert.True(db.Users.FirstOrDefault(x => x.Uid == u.Uid) != null);
            Assert.True(db.FindUser("001") != null);
            u.Name = "b";
            db.UpdateUser(u);
            Assert.True(db.FindUser("001").Name == "b");
            db.DeleteUser(u);
            Assert.True(db.FindUser("001") == null);
        }
    }
}
