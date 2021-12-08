using System;
using DiscountCodeAPI.DataAccess;
using DiscountCodeAPI.Data;
using MongoDB.Driver;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace DiscountCodeAPI.Services
{
   
    public class DiscountFactory
    {
        public static DiscountCampaign CreateDiscountCampaign(JsonElement s)
        {
            if (s.GetProperty("DiscountType").GetString() == "FixedAmount")
            {
                var json = s.GetRawText();
                DiscountCampaign c = JsonSerializer.Deserialize<FixedAmountDiscountCampaign>(json);
                ValidationContext vc = new ValidationContext(c);
                if (Validator.TryValidateObject(c, vc, null, true))
                    return c;
                return null;
            }
            else if (s.GetProperty("DiscountType").GetString() == "RelativeAmount")
            {
                var json = s.GetRawText();
                DiscountCampaign c = JsonSerializer.Deserialize<RelativeAmountDiscountCampaign>(json);
                ValidationContext vc = new ValidationContext(c);
                if (Validator.TryValidateObject(c, vc, null, true))
                    return c;
                return null;
            }
            return null;
        }

       

        
    }

    public class DiscountService : IDiscountService
    {
        IMongoDBContext _dbContext;
        ICodeGenerator _codeGenerator;
        INotificationService _notificationService;

        private readonly IMongoCollection<DiscountCampaign> _discountCampaigns;

        public DiscountService(IMongoDBContext dbContext, ICodeGenerator codeGenerator, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _codeGenerator = codeGenerator;
            _notificationService = notificationService;
        }

        public bool CreateCampaign(DiscountCampaign campaign)
        {
            // Todo: Check that campaign doesn't already exist before storing
            _dbContext.CreateAsync("Campaigns", campaign);
            return true;
        }

        public DiscountCampaign GetDiscountCampaign(string campaignCode)
        {
            var campaign = _dbContext.GetTaskAsync<Data.DiscountCampaign>("Campaigns", campaignCode);
            return campaign.Result;
        }

        public Discount ProvisionDiscount(string campaignCode, string beneficiaryId)
        {
            DiscountCampaign campaign = GetDiscountCampaign(campaignCode);
            Discount d = CreateDiscountFromCampaign(campaign);
            d.BeneficiaryId = beneficiaryId;
            _dbContext.CreateAsync("DiscountCodes", d);

            // Todo: Reduce campaign items by one

            return d;

        }

        public Discount GetDiscount(string discountCode, string beneficiaryId)
        {
            throw new NotImplementedException();
        }

        

        public float ApplyDiscount(string discountCode, string beneficiaryId, float sum)
        {
            // Get Discount Object and calculate new sum
            // Set Discount State to "Used"
            // Return new sum
            throw new NotImplementedException();
        }

        public Discount CreateDiscountFromCampaign(DiscountCampaign campaign)
        {
            if (campaign.DiscountType == "FixedAmount")
            {
                FixedAmountDiscountCampaign f = (FixedAmountDiscountCampaign)campaign;
                FixedAmountDiscount d = new FixedAmountDiscount()
                {
                    FixedAmount = f.FixedAmount,
                    //DiscountType = f.DiscountType,
                    ValidFrom = DateTime.Now,
                    ValidTo = DateTime.Now.AddDays(f.DiscountPeriod),
                    DiscountCode = _codeGenerator.GenerateCode()
                };

                return d;
            }
            else if (campaign.DiscountType == "RelativeAmount")
            {
                RelativeAmountDiscountCampaign f = (RelativeAmountDiscountCampaign)campaign;
                RelativeAmountDiscount d = new RelativeAmountDiscount()
                {
                    Factor = f.Factor,
                    //DiscountType = f.DiscountType,
                    ValidFrom = DateTime.Now,
                    ValidTo = DateTime.Now.AddDays(f.DiscountPeriod),
                    DiscountCode = _codeGenerator.GenerateCode()
                };

                return d;
            }
            return null;

        }
    }
}
