# DiscountTask

The WebService mainly consists of three different components:

- DiscountCampaign
A discount campaign is a discount generator that can be created by a "Campaign" administrator. The administrator specifies the period for which the campaign is active, what type of discount it should generate and how many discount items it can generate. Once a campaign is created, a client can ask
the campaign for a discount by the campaign name. Typically this is done, by an application to award a user.

A campaign can be created with a HTTP Post Request to api/discount/createcampaign with the following body:
{
    "DiscountType":"FixedAmount",
    "FromDate":"2020-01-01T00:00:00.000Z",
    "ToDate":"2020-04-01T00:00:00.000Z",
    "CampaignCode" : "BlackFriday",
    "NoOfDiscountItems" : 100,
    "FixedAmount" : 100,
    "DiscountPeriod" : 30
}

The example above will create a campaign that can generate a discount with a fixed amount (100) which will be valid for 30 days. The campaign itself will only be valid until 2020-04-01.

- Discount
A discount can have different forms. A 

The WebService API Consists of three methods:

-- C
