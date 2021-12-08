using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DiscountCodeAPI.Data;
using System.Text.Json;
using DiscountCodeAPI.Services;

namespace DiscountCodeAPI.Controllers
{
   
    

    [ApiController]
    [Route("[controller]")]
    public class DiscountController : ControllerBase
    {
     

        private readonly ILogger<DiscountController> _logger;
        private readonly IDiscountService _discountService;

        public DiscountController(ILogger<DiscountController> logger, IDiscountService service)
        {
            _logger = logger;
            _discountService = service;
        }

       

        [HttpPost]
        [Route("api/discount/createcampaign")]
        public DiscountCampaign CreateCampaign([FromBody] JsonElement s)
        {
          
            DiscountCampaign c = DiscountFactory.CreateDiscountCampaign(s);
            
            _discountService.CreateCampaign(c);

            return c;



        }

        [HttpGet]
        [Route("api/discount/generate")]
        public Discount GenerateDiscount( [FromQuery] string campaignCode, [FromQuery] string beneficiaryId)
        {
            Discount discount =_discountService.ProvisionDiscount(campaignCode, beneficiaryId);
            return discount;
        }

        [HttpPost]
        [Route("api/discount/apply")]
        public float ApplyDiscount(float sum, string discountCode, string beneficiaryId)
        {
            Discount discount = _discountService.GetDiscount(discountCode,beneficiaryId);


            float newSum = ((IDiscount) discount).GetResultSum(sum);
            //_discountService.DisableDiscount(discount);
            return newSum;
        }




      
        
    }
}
