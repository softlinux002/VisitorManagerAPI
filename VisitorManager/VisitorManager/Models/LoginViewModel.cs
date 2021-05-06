using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitorManager.Models
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        public int PackageOrAdminId { get; set; }
        public string SName { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string AuthCode { get; set; }
        public string Mobile2 { get; set; }
        public string UserType { get; set; }
    }
}