using System;
using DiscountCodeAPI.DataAccess;
using DiscountCodeAPI.Data;
using MongoDB.Driver;
using System.Text.Json;

namespace DiscountCodeAPI.Services
{
   
    public class DiscountFactory
    {
        public static DiscountCampaign CreateDiscountCampaign(JsonElement s)
        {
            var json = s.GetRawText();
            DiscountCampaign c = JsonSerializer.Deserialize<DiscountCampaign>(json);
          
            if (s.GetProperty("DiscountType").GetString() == "FixedAmount")
            {
                var subElement = s.GetProperty("DiscountTemplate");
                json = subElement.GetRawText();
                c.DiscountTemplate = JsonSerializer.Deserialize<FixedAmountDiscount>(json);
            }

            return c;
        }
    }

    public class DiscountService : IDiscountService
    {
        IMongoDBContext _dbContext;

        private readonly IMongoCollection<DiscountCampaign> _discountCampaigns;

        public DiscountService(IMongoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CreateCampaign(DiscountCampaign campaign)
        {
            _dbContext.CreateAsync("Campaigns", campaign);
            return true;
        }

        public DiscountCampaign GetDiscountCampaign(string campaignCode)
        {
            var campaign = _dbContext.GetTaskAsync<Data.DiscountCampaign>("Campaigns", campaignCode);
            return campaign.Result;
        }

        public IDiscount ProvisionDiscount(string campaignCode, string beneficiaryId)
        {
            DiscountCampaign campaign = GetDiscountCampaign(campaignCode);
            return campaign.DiscountTemplate;
        }

        public IDiscount GetDiscount(string discountCode, string beneficiaryId)
        {
            throw new NotImplementedException();
        }

        

        public bool ApplyDiscount(string discountCode, string beneficiaryId)
        {
            throw new NotImplementedException();
        }
    }
}
