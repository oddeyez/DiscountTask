using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DiscountCodeAPI.Data
{
    public interface IDiscount
    {
        public float GetResultSum(float sum);
        public float GetDiscountAmount(float sum);
    }

    public class Discount
    {
        public string DiscountType { get; set; }
        public long validFrom { get; set; }
        public long validTo { get; set; }
        public long DiscountCode { get; set; }
        public string BeneficiaryId { get; set; }
    }

    public class FixedAmountDiscountTemplate
    {
        public string DiscountType { get; set; }
        public long FixedAmount { get; set; }
        public long ValidDays { get; set; }
    }

    [BsonDiscriminator("FixedAmountDiscount")]
    public class FixedAmountDiscount : Discount, IDiscount
    {
        public FixedAmountDiscount() { FixedAmount = 0; }
        public FixedAmountDiscount(long amount) { FixedAmount = amount; }
        
        public long FixedAmount { get; set; }
        public float GetResultSum(float sum)
        {
            float discountedSum = sum - FixedAmount;
            return discountedSum > 0 ? discountedSum : 0;
        }
        public float GetDiscountAmount(float sum)
        {
            return FixedAmount;
        }


    }

    public class RelativeAmountDiscount : IDiscount
    {
        public RelativeAmountDiscount(long factor) { relativeFactor = factor; }
        public long relativeFactor { get; set; }
        public float GetResultSum(float sum)
        {
            return 0;
        }
        public float GetDiscountAmount(float sum)
        {
            return 0;
        }
    }
}
