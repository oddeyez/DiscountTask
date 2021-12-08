using System;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace DiscountCodeAPI.Data
{
    public class FixedAmountDiscountTemplate
    {
        public string DiscountType { get; set; }
        public long FixedAmount { get; set; }
        public long ValidDays { get; set; }
    }

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
        public IDiscount DiscountTemplate { get; set; }
    }

}
