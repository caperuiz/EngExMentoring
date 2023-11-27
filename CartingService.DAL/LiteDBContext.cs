using CartingService.DAL.Models;
using LiteDB;

public class LiteDBContext
{
    private readonly LiteDatabase _database;

    public LiteDBContext(string connectionString)
    {
        _database = new LiteDatabase(connectionString);
    }

    public ILiteCollection<CartDBModel> Carts => _database.GetCollection<CartDBModel>("carts");
}
