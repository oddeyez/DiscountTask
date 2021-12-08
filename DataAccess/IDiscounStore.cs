using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DiscountCodeAPI.DataAccess
{

    public interface IDiscountStore
    {
        IMongoCollection<T> GetCollection<T>(string name);
        Task CreateAsync<T>(string name, T item);
        Task<Data.DiscountCampaign> GetDiscountCampaignAsync(string collectionName, string campaignCode);
        Task<Data.Discount> GetDiscountAsync(string collectionName, string discountCode);
    }
   
}
