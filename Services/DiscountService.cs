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
        IDiscountStore _discountStore;
        ICodeGenerator _codeGenerator;
        INotificationService _notificationService;

        private readonly IMongoCollection<DiscountCampaign> _discountCampaigns;

        public DiscountService(IDiscountStore discountStore, ICodeGenerator codeGenerator, INotificationService notificationService)
        {
            _discountStore = discountStore;
            _codeGenerator = codeGenerator;
            _notificationService = notificationService;
        }

        public bool CreateCampaign(DiscountCampaign campaign)
        {
            // Todo: Check that campaign doesn't already exist before storing
            _discountStore.CreateAsync("Campaigns", campaign);
            return true;
        }

        public DiscountCampaign GetDiscountCampaign(string campaignCode)
        {
            var campaign = _discountStore.GetDiscountCampaignAsync("Campaigns", campaignCode);
            return campaign.Result;
        }

        public Discount ProvisionDiscount(string campaignCode, string beneficiaryId)
        {
            DiscountCampaign campaign = GetDiscountCampaign(campaignCode);
            Discount d = CreateDiscountFromCampaign(campaign);
            d.BeneficiaryId = beneficiaryId;
            _discountStore.CreateAsync("DiscountCodes", d);

            // Todo: Check that discount is not already provisioned or used
            // Todo: Reduce campaign items by one

            return d;

        }

        public Discount GetDiscount(string discountCode, string beneficiaryId)
        {
            var discount = _discountStore.GetDiscountAsync("DiscountCodes", discountCode);
            return discount.Result;
        }

        

        public float ApplyDiscount(string discountCode, string beneficiaryId, float sum)
        {
            
            // Todo: Set Discount State to "Used"
            Discount discount = GetDiscount(discountCode, beneficiaryId);
            float newSum = ((IDiscount)discount).GetResultSum(sum);
            return newSum;
        }

        public Discount CreateDiscountFromCampaign(DiscountCampaign campaign)
        {
            if (campaign.DiscountType == "FixedAmount")
            {
                FixedAmountDiscountCampaign f = (FixedAmountDiscountCampaign)campaign;
                FixedAmountDiscount d = new FixedAmountDiscount()
                {
                    FixedAmount = f.FixedAmount,
                    ValidFrom = DateTime.Now,
                    ValidTo = DateTime.Now.AddDays(f.DiscountPeriod),
                    DiscountCode = _codeGenerator.GenerateCode()
                };

                return d;
            }
            else if (campaign.DiscountType == "RelativeAmount")
            {
                RelativeAmountDiscountCampaign relativeCampaign = (RelativeAmountDiscountCampaign)campaign;
                RelativeAmountDiscount relativeDiscount = new RelativeAmountDiscount()
                {
                    Factor = relativeCampaign.Factor,
                    ValidFrom = DateTime.Now,
                    ValidTo = DateTime.Now.AddDays(relativeCampaign.DiscountPeriod),
                    DiscountCode = _codeGenerator.GenerateCode()
                };

                return relativeDiscount;
            }
            return null;

        }
    }
}
