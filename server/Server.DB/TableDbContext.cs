using LiteDB;
using Server.DB.Models;
using Server.Shared.Options;

namespace Server.DB
{
    public class TableDbContext : BaseDbContext<TableDbModel>
    {
        public TableDbContext(LiteDatabase db, DbOptions opt) : base(db, opt.TableName)
        {
        }
    }
}
