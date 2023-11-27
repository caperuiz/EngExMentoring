using MongoDB.Bson;
using MongoDB.Driver;

namespace BasketService.API
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
            _database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
        }

        public IMongoCollection<MongoSavingModel> MyCollection => _database.GetCollection<MongoSavingModel>("your-collection-name");
    }

    public class MongoSavingModel
    {
        public ObjectId Id { get; set; }
        public string Message { get; set; }
    }

}
