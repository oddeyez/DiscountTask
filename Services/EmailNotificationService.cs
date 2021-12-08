using System;
using System.Collections.Generic;
namespace DiscountCodeAPI.Services
{
    public class EmailNotificationService : INotificationService
    {
        public EmailNotificationService()
        {
            
        }

        public void SendNotification(IEnumerable<Recipient> recipients, string message)
        {
            Console.WriteLine($"Message:{message}");
        }
    }
}
