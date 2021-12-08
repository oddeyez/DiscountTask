using System;
using System.Collections.Generic;
namespace DiscountCodeAPI.Services
{

    public class Recipient
    {
        public string Email { get; set; }
        public string SmsPhone { get; set; }
    }

    public interface INotificationService
    {
        void SendNotification(IEnumerable<Recipient> recipients, string message);
    }
}
