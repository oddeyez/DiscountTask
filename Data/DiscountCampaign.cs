using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace DiscountCodeAPI.Data
{
    public abstract class DiscountTemplate
    {
        [StringLength(100)]
        public string DiscountType { get; set; }

        [Range(0, 360)]
        public long ValidDays { get; set; }
    }

    public class FixedAmountDiscountTemplate : DiscountTemplate
    {
        [Range(0, long.MaxValue)]
        public long FixedAmount { get; set; }
    }
    /*
    public class DiscountCampaign
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        public string CampaignCode { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public long NoOfDiscountItems { get; set; }

        [JsonIgnore]
        public DiscountTemplate DiscountTemplate { get; set; }
    }*/


    public class DiscountCampaign
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        [StringLength(100)]
        public string CampaignCode { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        [Range(0, long.MaxValue)]
        public long NoOfDiscountItems { get; set; }

        [StringLength(100)]
        public string DiscountType { get; set; }

        [Range(0,720)]
        public long DiscountPeriod { get; set; }

    }

    public class FixedAmountDiscountCampaign : DiscountCampaign
    {
        [Range(0, float.MaxValue)]
        public float FixedAmount { get; set; }
    }

    public class RelativeAmountDiscountCampaign : DiscountCampaign
    {
        [Range(0.0, 100.0)]
        public float Factor { get; set; }
    }

}
