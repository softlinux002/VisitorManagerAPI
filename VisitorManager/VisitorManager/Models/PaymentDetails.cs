using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitorManager.Models
{
    public class PaymentDetails
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public string AdminAppId { get; set; }
        public string PaymentId { get; set; }
        public string Guid { get; set; }
        public string Token { get; set; }
        public string PayerID { get; set; }
        public DateTime Date { get; set; }
        public string Amount { get; set; }
    }
}