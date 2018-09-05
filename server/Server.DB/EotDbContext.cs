using LiteDB;
using Server.DB.Models;
using Server.Shared.Options;

namespace Server.DB
{
    public class EotDbContext : BaseDbContext<EotDbModel>
    {
        public EotDbContext(LiteDatabase db, DbOptions opt) : base(db, opt.EotName)
        {
        }
    }
}
