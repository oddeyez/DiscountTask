using System;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace DiscountCodeAPI.Data
{
    public abstract class DiscountTemplate
    {
        public string DiscountType { get; set; }
        public long ValidDays { get; set; }
    }

    public class FixedAmountDiscountTemplate : DiscountTemplate
    {
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

        public string CampaignCode { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public long NoOfDiscountItems { get; set; }

        public string DiscountType { get; set; }

        public long DiscountPeriod { get; set; }

    }

    public class FixedAmountDiscountCampaign : DiscountCampaign
    {
        public float FixedAmount { get; set; }
    }

    public class RelativeAmountDiscountCampaign : DiscountCampaign
    {
        public float Factor { get; set; }
    }

}
