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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class GatekeeperrrrEntities : DbContext
    {
        public GatekeeperrrrEntities()
            : base("name=GatekeeperrrrEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbl_Administrator> tbl_Administrator { get; set; }
        public virtual DbSet<tbl_country> tbl_country { get; set; }
        public virtual DbSet<tbl_Gatekeeper> tbl_Gatekeeper { get; set; }
        public virtual DbSet<tbl_Payment> tbl_Payment { get; set; }
        public virtual DbSet<tbl_StaffUser> tbl_StaffUser { get; set; }
        public virtual DbSet<tbl_state> tbl_state { get; set; }
        public virtual DbSet<tbl_Visitor> tbl_Visitor { get; set; }
        public virtual DbSet<tbl_Package> tbl_Package { get; set; }
        public virtual DbSet<LoginView> LoginViews { get; set; }
    
        public virtual ObjectResult<sp_GetStaffByNameAndMobile_Result> sp_GetStaffByNameAndMobile(string name, Nullable<int> adminId)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var adminIdParameter = adminId.HasValue ?
                new ObjectParameter("AdminId", adminId) :
                new ObjectParameter("AdminId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetStaffByNameAndMobile_Result>("sp_GetStaffByNameAndMobile", nameParameter, adminIdParameter);
        }
    
        public virtual ObjectResult<sp_GetVisitorByStaffId_Result> sp_GetVisitorByStaffId(string date, string staffId)
        {
            var dateParameter = date != null ?
                new ObjectParameter("Date", date) :
                new ObjectParameter("Date", typeof(string));
    
            var staffIdParameter = staffId != null ?
                new ObjectParameter("StaffId", staffId) :
                new ObjectParameter("StaffId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetVisitorByStaffId_Result>("sp_GetVisitorByStaffId", dateParameter, staffIdParameter);
        }
    
        public virtual ObjectResult<sp_GetVisitorsByDate_Result> sp_GetVisitorsByDate(string date, Nullable<int> adminId, string gatekeeperId)
        {
            var dateParameter = date != null ?
                new ObjectParameter("Date", date) :
                new ObjectParameter("Date", typeof(string));
    
            var adminIdParameter = adminId.HasValue ?
                new ObjectParameter("AdminId", adminId) :
                new ObjectParameter("AdminId", typeof(int));
    
            var gatekeeperIdParameter = gatekeeperId != null ?
                new ObjectParameter("GatekeeperId", gatekeeperId) :
                new ObjectParameter("GatekeeperId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetVisitorsByDate_Result>("sp_GetVisitorsByDate", dateParameter, adminIdParameter, gatekeeperIdParameter);
        }
    
        public virtual ObjectResult<sp_GetVisitorsForAdmin_Result> sp_GetVisitorsForAdmin(string date, string visitorName, string vehicleNumber, Nullable<int> adminId)
        {
            var dateParameter = date != null ?
                new ObjectParameter("Date", date) :
                new ObjectParameter("Date", typeof(string));
    
            var visitorNameParameter = visitorName != null ?
                new ObjectParameter("VisitorName", visitorName) :
                new ObjectParameter("VisitorName", typeof(string));
    
            var vehicleNumberParameter = vehicleNumber != null ?
                new ObjectParameter("VehicleNumber", vehicleNumber) :
                new ObjectParameter("VehicleNumber", typeof(string));
    
            var adminIdParameter = adminId.HasValue ?
                new ObjectParameter("AdminId", adminId) :
                new ObjectParameter("AdminId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetVisitorsForAdmin_Result>("sp_GetVisitorsForAdmin", dateParameter, visitorNameParameter, vehicleNumberParameter, adminIdParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> sp_GetTotalVisitorForToday(string date, string adminId)
        {
            var dateParameter = date != null ?
                new ObjectParameter("Date", date) :
                new ObjectParameter("Date", typeof(string));
    
            var adminIdParameter = adminId != null ?
                new ObjectParameter("AdminId", adminId) :
                new ObjectParameter("AdminId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("sp_GetTotalVisitorForToday", dateParameter, adminIdParameter);
        }
    
        public virtual ObjectResult<sp_GetStaffByGatekeeperId_Result> sp_GetStaffByGatekeeperId(string gatekeeperId)
        {
            var gatekeeperIdParameter = gatekeeperId != null ?
                new ObjectParameter("GatekeeperId", gatekeeperId) :
                new ObjectParameter("GatekeeperId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetStaffByGatekeeperId_Result>("sp_GetStaffByGatekeeperId", gatekeeperIdParameter);
        }
    }
}
