using LiteDB;
using Server.DB.Models;
using Server.Shared.Options;

namespace Server.DB
{
    public class ScoresDbContext:BaseDbContext<ScoresDbModel>
    {
        public ScoresDbContext(LiteDatabase db, DbOptions opt) : base(db, opt.ScoresName)
        {
        }
    }
}
