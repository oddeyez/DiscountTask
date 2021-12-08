using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DiscountCodeAPI.Data
{

    public enum DiscountState {
        Provisioned,
        Used
    }

    public interface IDiscount
    {
        public float GetResultSum(float sum);
        public float GetDiscountAmount(float sum);
    }

    public class Discount
    {
        //public string DiscountType { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string DiscountCode { get; set; }
        public string BeneficiaryId { get; set; }
        public DiscountState State { get; set; }
    }

    

    [BsonDiscriminator("FixedAmountDiscount")]
    public class FixedAmountDiscount : Discount, IDiscount
    {
        public FixedAmountDiscount() { FixedAmount = 0; }
        
        
        public float FixedAmount { get; set; }
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

    public class RelativeAmountDiscount : Discount, IDiscount
    {
        public RelativeAmountDiscount() {}
        public float Factor { get; set; }
        public float GetResultSum(float sum)
        {
            return (1-Factor)*sum;
        }
        public float GetDiscountAmount(float sum)
        {
            return Factor*sum;
        }
    }
}
