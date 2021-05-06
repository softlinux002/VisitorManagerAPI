using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitorManager.Models
{
    public class PayPalViewModel
    {
        public string UserName { get; set; }
        public PayPalBillingAddressVM PayPalBillingAddressVM { get; set; }
        public ItemViewModel ItemViewModel { get; set; }
    }

    public class PayPalBillingAddressVM
    {
        public string line1 { get; set; }
        public string city { get; set; }
        public string Postal_code { get; set; }
        public string state { get; set; }
        public string Country { get; set; }
    }

    public class ItemViewModel
    {
        public string Name { get; set; }
        public string Quantity { get; set; }
        public string Currency { get; set; }
        public string Price { get; set; }
        public string SKU { get; set; }
    }
    public class Company
    {
        public bool PaymentSuccess { get; set; }
    }
}