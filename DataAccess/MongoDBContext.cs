using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Threading.Tasks;
using DiscountCodeAPI.Data;

namespace DiscountCodeAPI.DataAccess
{
    public class Temp
    {
        public string test { get; set; }
    }

    public interface IMongoDBContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
        Task CreateAsync<T>(string name, T item);
        Task<Data.DiscountCampaign> GetTaskAsync<DiscountCampaign>(string collectionName, string campaignCode);
    }

    public class MongoDBContext : IMongoDBContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public IClientSessionHandle Session { get; set; }
        public MongoDBContext(IOptions<DBSettings> configuration)
        {
            _mongoClient = new MongoClient(configuration.Value.Connection);
            _db = _mongoClient.GetDatabase(configuration.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }

        public async Task CreateAsync<T>(string collectionName, T item)
        {
            IMongoCollection<T> collection = _db.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(item);
        }

        public async Task<Data.DiscountCampaign> GetTaskAsync<DiscountCampaign>(string collectionName, string campaignCode)
        {
            IMongoCollection<Data.DiscountCampaign> collection = _db.GetCollection<Data.DiscountCampaign>(collectionName);
            var filter = Builders<Data.DiscountCampaign>.Filter.Eq(x => x.CampaignCode, campaignCode);
            var campaign = await collection.Find(filter).SingleAsync();
            return campaign;
        }
    }
}
