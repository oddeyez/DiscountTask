using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DiscountCodeAPI.Data
{
    /// <summary>
    /// Enum DiscountState to specify what state a discount code can be in
    /// </summary>
    public enum DiscountState {
        Provisioned,
        Used
    }

    /// <summary>
    /// Interface IDiscount for calculating the resulting sum or the discount amount
    /// </summary>
    public interface IDiscount
    {
        /// <summary>
        /// Calculates the sum after the discount has been applied
        /// </summary>
        /// <param name="sum">The original sum to apply the discount to</param>
        /// <returns>The calculated sum</returns>
        public float GetResultSum(float sum);

        /// <summary>
        /// Calculates the total discount 
        /// </summary>
        /// <param name="sum">The original sum to apply the discount to</param>
        /// <returns></returns>
        public float GetDiscountAmount(float sum);
    }

    public class Discount
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        [StringLength(100)]
        public string DiscountCode { get; set; }
        [StringLength(100)]
        public string BeneficiaryId { get; set; }
        public DiscountState State { get; set; }
    }

    [BsonDiscriminator("FixedAmountDiscount")]
    public class FixedAmountDiscount : Discount, IDiscount
    {
        public FixedAmountDiscount() { FixedAmount = 0; }
        [Range(0, float.MaxValue)]
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

    [BsonDiscriminator("RelativeAmountDiscount")]
    public class RelativeAmountDiscount : Discount, IDiscount
    {
        public RelativeAmountDiscount() {}
        
        [Range(0.0, 100.0)]
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
