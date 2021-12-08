# DiscountTask


## Introduction
The Discount Service mainly consists of two basic concepts:

### Discount Campaign
A discount campaign is an object that can generate discounts. A campaign can be created by a "Campaign" administrator for example. The administrator specifies the period for which the campaign is active, what type of discount it should generate and how many discount items it can generate. Once a campaign is added to the system it can generate discounts. A client can ask the campaign to generate a discount from the campaig. Typically this is done, by a client application to award a user.


## Discount
A discount is a component to keep track of whether the discount has been provisioned or used, whether it has expired or but also how to calculate the discount. There are currently two different discount types: "Fixed Amount" which will apply a fixed amount as the discount and "Relative Amount" which will calculate the discount by multiplying the sum with a factor. The second discount type can be used to offer '20% off', for example.

When a client application wants to award a user, for example. It calls the web service with a user reference (user id, email, etc.) and a campaign code. The service then generates a discount code and associates it ith the user reference. Later when the discount is applied (user wants to use the discount), the discount code and reference is sent to the service and validated. Then the discount will then calculate the discount and return the new amount, with the discount applied, to the client application.



    
## WebService API

### Create Campaign 
A campaign can be created with a HTTP Post Request to the service:
```
api/discount/createcampaign
{
    "DiscountType":"FixedAmount",
    "FromDate":"2020-01-01T00:00:00.000Z",
    "ToDate":"2020-04-01T00:00:00.000Z",
    "CampaignCode" : "BlackFriday",
    "NoOfDiscountItems" : 100,
    "FixedAmount" : 100,
    "DiscountPeriod" : 30
}
```

The example above will create a campaign that can generate discounts with a fixed amount (100) which will be valid for 30 days. The campaign itself will only be valid and generate codes during the timespan specified or until all the discount items have been provisioned.

### Generate Discount

To provision a discount and obtain a discount code, a client application can send a HTTP Get Request with campaign code and user reference. This will return a discount code.
```
api/discount/generate?campaignCode=<campaign code>&beneficiaryId=<user reference>
```    
   
### Apply Discount

To apply a discount and calculate the new sum a client application can send an HTTP Get Request with the discount code and user reference. This will return the new sum with the discount applied.

```
api/discount/generate?campaignCode=<campaign code>&beneficiaryId=<user reference>
``` 
    
## Use case sequence
![Use case](/docs/ComponentDia.png)
    
## Components
![Components](/docs/SequenceDia.png)
