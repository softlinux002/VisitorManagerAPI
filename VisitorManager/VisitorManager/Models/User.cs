using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitorManager.Models
{
    public class User
    {
        public int Id { get; set; }
        public string StaffId { get; set; }
        public int AdminId { get; set; }
        public string StaffName { get; set; }
        public string Email { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Comment { get; set; }
        public string UserType { get; set; }
        public string Designation { get; set; }
        public string OTP { get; set; }
        public Guid AuthCode { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
    }

    public class UserGroup
    {
        public int Id { get; set; }
        public string UserGroupName { get; set; }
    }

    
}