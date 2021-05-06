using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitorManager.Models
{
    public class Visitor
    {
        public int Id { get; set; }
        public string VisitorName { get; set; }
        public string VisitorType { get; set; }
        public int? TotalVisitor { get; set; }
        public string WhomToVisit { get; set; }
        public string VhicleType { get; set; }
        public string VhicleNumber { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public DateTime Date { get; set; }
        public string AddDate { get; set; }
        public string Image { get; set; }
        public int StaffId { get; set; }
        public string Mobile { get; set; }
        public byte[] base64 { get; set; }
        public string Status { get; set; }
        public int AdminId { get; set; }
        public int OTP { get; set; } 
        public string Purpose { get; set; }
        public int GatekeeperId { get; set; }
    }
}