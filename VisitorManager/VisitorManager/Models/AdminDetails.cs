using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitorManager.Models
{
    public class AdminDetails
    {
        public int Id { get; set; }
        public int PackageId { get; set; }
        public string Username { get; set; }
        public string PackageName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string AdminId { get; set; }
        public string AuthCode { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}