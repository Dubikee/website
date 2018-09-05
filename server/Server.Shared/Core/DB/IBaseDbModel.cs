namespace Server.Shared.Core.DB
{
    public interface IBaseDbModel
    {
        int Id { get; set; }
        string Uid { get; set; }
    }
}
