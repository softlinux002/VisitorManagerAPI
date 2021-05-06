using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitorManager.Models
{
    public class GateKeeperDetails
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public string GatekeeperId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Password { get; set; }
        public string AuthCode { get; set; }
    }
}