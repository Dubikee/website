using LiteDB;
using Server.DB.Models;
using Server.Shared.Options;

namespace Server.DB
{
    public class RinkDbContext : BaseDbContext<RinkDbModel>
    {
        public RinkDbContext(LiteDatabase db, DbOptions opt) : base(db, opt.RinkName)
        {
        }
    }
}
