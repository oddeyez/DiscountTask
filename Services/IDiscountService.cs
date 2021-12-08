using System;
using DiscountCodeAPI.Data;

namespace DiscountCodeAPI.Services
{
    public interface IDiscountService
    {
        bool CreateCampaign(DiscountCampaign campaign);
        DiscountCampaign GetDiscountCampaign(string campaignCode);

        IDiscount ProvisionDiscount(string campaignCode, string beneficiaryId);
        IDiscount GetDiscount(string discountCode, string beneficiaryId);

        bool ApplyDiscount(string discountCode, string beneficiaryId);
    }
}
