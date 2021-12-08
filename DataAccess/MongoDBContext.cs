using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Threading.Tasks;
using DiscountCodeAPI.Data;

namespace DiscountCodeAPI.DataAccess
{
    public class DiscountStore : IDiscountStore
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public IClientSessionHandle Session { get; set; }
        public DiscountStore(IOptions<DBSettings> configuration)
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

        public async Task<Data.DiscountCampaign> GetDiscountCampaignAsync(string collectionName, string campaignCode)
        {
            IMongoCollection<Data.DiscountCampaign> collection = _db.GetCollection<Data.DiscountCampaign>(collectionName);
            var filter = Builders<Data.DiscountCampaign>.Filter.Eq(x => x.CampaignCode, campaignCode);
            var campaign = await collection.Find(filter).SingleAsync();
            return campaign;
        }

        public async Task<Data.Discount> GetDiscountAsync(string collectionName, string discountCode)
        {
            IMongoCollection<Data.Discount> collection = _db.GetCollection<Data.Discount>(collectionName);
            var filter = Builders<Data.Discount>.Filter.Eq(x => x.DiscountCode, discountCode);
            var discount = await collection.Find(filter).SingleAsync();
            return discount;
        }
    }
}
