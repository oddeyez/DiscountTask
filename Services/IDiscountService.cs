using System;
using DiscountCodeAPI.Data;

namespace DiscountCodeAPI.Services
{
    public interface IDiscountService
    {
        bool CreateCampaign(DiscountCampaign campaign);
        DiscountCampaign GetDiscountCampaign(string campaignCode);

        Discount ProvisionDiscount(string campaignCode, string beneficiaryId);
        Discount GetDiscount(string discountCode, string beneficiaryId);

        float ApplyDiscount(string discountCode, string beneficiaryId);
    }
}
