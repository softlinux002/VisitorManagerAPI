//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VisitorManager.Models
{
    using System;
    
    public partial class sp_GetVisitorsByDate_Result
    {
        public int Id { get; set; }
        public Nullable<int> AdminId { get; set; }
        public string VisitorName { get; set; }
        public string VisitorType { get; set; }
        public Nullable<int> TotalVisitor { get; set; }
        public string WhomToVisit { get; set; }
        public string VhicleType { get; set; }
        public string VhicleNumber { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Image { get; set; }
        public Nullable<int> StaffId { get; set; }
        public string Mobile { get; set; }
        public string Status { get; set; }
        public Nullable<int> OTP { get; set; }
        public string Purpose { get; set; }
        public Nullable<int> GatekeeperId { get; set; }
    }
}
