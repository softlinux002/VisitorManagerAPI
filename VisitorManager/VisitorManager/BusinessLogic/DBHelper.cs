using VisitorManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Data.SqlClient;

namespace VisitorManager.BusinessLogic
{
    public class DBHelper
    {
        public static List<Visitor> GetVisitorDetails()
        {
            List<Visitor> visitor = new List<Visitor>();

            var url = CommonUtil.GetBaseUrl();

            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                visitor = (from z in db.tbl_Visitor
                           select
                          new Visitor
                          {
                              Id = z.Id,
                              VisitorName = z.VisitorName,
                              VisitorType = z.VisitorType,
                              TotalVisitor = z.TotalVisitor,
                              WhomToVisit = z.WhomToVisit,
                              VhicleType = z.VhicleType,
                              VhicleNumber = z.VhicleNumber,
                              TimeIn = z.TimeIn,
                              TimeOut = z.TimeOut,
                              Date = (DateTime)z.Date,
                              Image = url + z.Image
                          }).ToList();

                return visitor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static AdminDetails AdminLogin(string username, string password)
        {
            var result = "";

            try
            {
                AdminDetails admin = new AdminDetails();
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var login = (from z in db.tbl_Administrator where z.Username == username && z.Password == password select z).FirstOrDefault();
                if (login != null)
                {
                    admin.Id = login.Id;
                    admin.PackageId = login.PackageId == null ? 0 : (int)login.PackageId;
                    admin.Username = login.Username;
                    admin.Password = login.Password;
                    admin.Name = login.Name;
                    admin.Email = login.Email;
                    admin.Mobile = login.Mobile;
                    admin.City = login.City;
                    admin.State = login.State;
                    admin.Country = login.Country;
                    admin.ZipCode = login.ZipCode;
                    admin.Type = login.Type;
                    admin.Status = login.Status;
                }

                return admin;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Visitor> GetVisitorsByGatekeeperId(int gatekeeperId)
        {
            List<Visitor> visitor = new List<Visitor>();

            var url = CommonUtil.GetBaseUrl();

            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();

                var result = (from v in db.tbl_Visitor where v.GatekeeperId==gatekeeperId select v).ToList();
                for (int i = 0; i < result.Count; i++)
                {
                    Visitor v = new Visitor();
                    v.Id = result[i].Id;
                    v.VisitorName = result[i].VisitorName;
                    v.VisitorType = result[i].VisitorType;
                    v.TotalVisitor = result[i].TotalVisitor;
                    v.WhomToVisit = result[i].WhomToVisit;
                    v.VhicleType = result[i].VhicleType;
                    v.VhicleNumber = result[i].VhicleNumber;
                    v.TimeIn = result[i].TimeIn;
                    v.TimeOut = result[i].TimeOut;
                    v.Date = (DateTime)result[i].Date;
                    v.Image = url + result[i].Image;
                    v.Mobile = result[i].Mobile;
                    v.StaffId = result[i].StaffId == null ? 0 : (int)result[i].StaffId;
                    v.Status = result[i].Status;
                    v.Purpose = result[i].Purpose;
                    visitor.Add(v);
                }
                return visitor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Visitor> GetVisitorDetailsAPI(Visitor visitorDetails)
        {
            List<Visitor> visitor = new List<Visitor>();

            var url = CommonUtil.GetBaseUrl();

            try
            {
                string gatekeeperId = string.Empty;
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                if (visitorDetails.GatekeeperId == 0 || visitorDetails.GatekeeperId == null)
                {
                    gatekeeperId = "";
                }
                else
                {
                    gatekeeperId = visitorDetails.GatekeeperId.ToString();
                }
                var result = db.sp_GetVisitorsByDate(visitorDetails.Date.ToString("MM/dd/yyyy"), visitorDetails.AdminId, gatekeeperId).ToList();
                for (int i = 0; i < result.Count; i++)
                {
                    Visitor v = new Visitor();
                    v.Id = result[i].Id;
                    v.VisitorName = result[i].VisitorName;
                    v.VisitorType = result[i].VisitorType;
                    v.TotalVisitor = result[i].TotalVisitor;
                    v.WhomToVisit = result[i].WhomToVisit;
                    v.VhicleType = result[i].VhicleType;
                    v.VhicleNumber = result[i].VhicleNumber;
                    v.TimeIn = result[i].TimeIn;
                    v.TimeOut = result[i].TimeOut;
                    v.Date = (DateTime)result[i].Date;
                    v.Image = url + result[i].Image;
                    v.Mobile = result[i].Mobile;
                    v.StaffId = result[i].StaffId == null ? 0 : (int)result[i].StaffId;
                    v.Status = result[i].Status;
                    v.Purpose = result[i].Purpose;
                    visitor.Add(v);
                }
                return visitor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateGateKeeperSentOTP(User user)
        {
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var data = (from c in db.tbl_StaffUser where c.Id == user.Id select c).FirstOrDefault();
                data.OTP = user.OTP;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateGateKeeperAuthcode(GateKeeperDetails user)
        {
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var data = (from c in db.tbl_Gatekeeper where c.GatekeeperId == user.GatekeeperId select c).FirstOrDefault();
                data.AuthCode = user.AuthCode;
                db.SaveChanges();
                //User userData = new User();
                //userData.StaffName = data.StaffName;
                //userData.StaffId = data.StaffId;
                //userData.Id = data.Id;
                //userData.AuthCode = user.AuthCode;
                //userData.Email = data.Email;
                //return userData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<User> GetStaffByGatekeeperId(string gatekeeperId)
        {
            List<User> result = new List<User>();
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                SqlParameter id= new SqlParameter("@GatekeeperId", gatekeeperId);
                var data = db.Database.SqlQuery<User>("exec sp_GetStaffByGatekeeperId @GatekeeperId", id).ToList();
                
                foreach(User record in data)
                {
                    User user = new User();
                    user.Id = record.Id;
                    user.StaffName = record.StaffName;
                    result.Add(user);
                }
                return result;
            }
            catch (Exception ex)
            {
                //throw ex;
                return result;
            }
        }


        public static Visitor SubmitVisitorDetails(Visitor visitor)
        {
            Visitor result = new Visitor();
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                tbl_Visitor tblVisitor = new tbl_Visitor();
                if (visitor != null)
                {
                    tblVisitor.AdminId = visitor.AdminId;
                    tblVisitor.VisitorName = visitor.VisitorName;
                    tblVisitor.VisitorType = visitor.VisitorType;
                    tblVisitor.TotalVisitor = visitor.TotalVisitor;
                    tblVisitor.WhomToVisit = visitor.WhomToVisit;
                    tblVisitor.VhicleType = visitor.VhicleType;
                    tblVisitor.VhicleNumber = visitor.VhicleNumber;
                    tblVisitor.TimeIn = visitor.TimeIn;
                    tblVisitor.StaffId = visitor.StaffId;
                    tblVisitor.Date = Convert.ToDateTime(visitor.Date);
                    tblVisitor.Image = visitor.Image;
                    tblVisitor.Mobile = visitor.Mobile;
                    tblVisitor.Status = "In";
                    tblVisitor.OTP = visitor.OTP;
                    tblVisitor.Purpose = visitor.Purpose;
                    tblVisitor.GatekeeperId = visitor.GatekeeperId;
                    db.tbl_Visitor.Add(tblVisitor);
                    db.SaveChanges();
                }
                result = (from z in db.tbl_Visitor
                          orderby z.Id descending
                          select new Visitor
                          {
                              Id = z.Id,
                              OTP = (int)z.OTP
                          }).FirstOrDefault();
                //result = true;
                return result;
            }
            catch (Exception ex)
            {
                //throw ex;
                return result;
            }
        }

        public static Visitor GetVisitorById(int visitorId)
        {
            Visitor result = new Visitor();
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var data = (from z in db.tbl_Visitor where z.Id == visitorId select z).FirstOrDefault();

                if (data != null)
                {
                    result.Id = data.Id;
                    result.VisitorName = data.VisitorName;
                    result.VisitorType = data.VisitorType;
                    result.TotalVisitor = data.TotalVisitor;
                    result.WhomToVisit = data.WhomToVisit;
                    result.VhicleType = data.VhicleType;
                    result.VhicleNumber = data.VhicleNumber;
                    result.TimeIn = data.TimeIn;
                    result.TimeOut = data.TimeOut;
                    result.Image = data.Image;
                    result.StaffId = Convert.ToInt32(data.StaffId);
                    result.Mobile = data.Mobile;
                    result.Status = data.Status;
                    result.AdminId = Convert.ToInt32(data.AdminId);
                    result.Purpose = data.Purpose;
                    result.GatekeeperId = Convert.ToInt32(data.GatekeeperId);
                    result.Date = (DateTime)data.Date;
                    result.VisitorName = data.VisitorName;
                }

                return result;
            }
            catch (Exception ex)
            {
                //throw ex;
                return result;
            }
        }

        public static bool DeleteVisitor(int id)
        {
            bool result = false;
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var data = (from z in db.tbl_Visitor where z.Id == id select z).FirstOrDefault();
                db.tbl_Visitor.Remove(data);
                db.SaveChanges();
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                //throw ex;
                return result;
            }
        }

        //public static Visitor GetReturnVisitorDetails(Visitor visitor)
        //{
        //    var url = CommonUtil.GetBaseUrl();
        //    Visitor visitorDetails = new Visitor();
        //    try
        //    {
        //        GatekeeperrrrEntities db = new GatekeeperrrrEntities();
        //        visitorDetails = db.tbl_Visitor.Where(x => x.Mobile == visitor.Mobile)
        //            .OrderByDescending(x => x.Id)
        //            .Select(x => new Visitor
        //            {
        //                Id = x.Id,
        //                VisitorName = x.VisitorName,
        //                VisitorType = x.VisitorType,
        //                TotalVisitor = x.TotalVisitor,
        //                WhomToVisit = x.WhomToVisit,
        //                VhicleType = x.VhicleType,
        //                VhicleNumber = x.VhicleNumber,
        //                TimeIn = x.TimeIn,
        //                Date = (DateTime)x.Date,
        //                FlatNumber = x.FlatNumber,
        //                Mobile = x.Mobile,
        //                AdminId = x.AdminId==null?0:(int)x.AdminId,
        //                Image = url + x.Image
        //            }).FirstOrDefault();
        //        return visitorDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw ex;
        //        return null;
        //    }
        //}

        public static bool UpdateVisitor(Visitor visitor)
        {
            var result = false;
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var data = (from c in db.tbl_Visitor where c.Id == visitor.Id && c.Status=="In" select c).FirstOrDefault();
                data.VisitorName = visitor.VisitorName;
                data.VisitorType = visitor.VisitorType;
                data.TotalVisitor = visitor.TotalVisitor;
                data.WhomToVisit = visitor.WhomToVisit;
                data.VhicleType = visitor.VhicleType;
                data.VhicleNumber = visitor.VhicleNumber;
                data.StaffId = visitor.StaffId;
                data.Mobile = visitor.Mobile;
                data.Purpose = visitor.Purpose;
                db.SaveChanges();
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public static bool UpdateVisitorTimeOut(Visitor visitor)
        {
            var result = false;
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var data = (from c in db.tbl_Visitor where c.Id == visitor.Id select c).FirstOrDefault();
                data.TimeOut = visitor.TimeOut;
                data.Status = "Out";
                db.SaveChanges();
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public static List<User> GetAllStaff()
        {
            List<User> userDetails = new List<User>();
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                userDetails = (from z in db.tbl_StaffUser
                               select
                               new User
                               {
                                   Id = z.Id,
                                   StaffName = z.StaffName
                               }).ToList();

                return userDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<User> GetUserTableDetails(int adminId = 0, string name = "")
        {
            List<User> userDetails = new List<User>();

            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var data = db.sp_GetStaffByNameAndMobile(name, adminId).ToList();
                if (data.Count != 0)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        User userClass = new User();
                        userClass.Id = data[i].Id;
                        userClass.StaffName = data[i].StaffName;
                        userClass.StaffId = data[i].StaffId;
                        userClass.Email = data[i].Email;
                        userClass.Mobile1 = data[i].Mobile1;
                        userClass.Mobile2 = data[i].Mobile2;
                        userClass.Address = data[i].Address;
                        userClass.State = data[i].State;
                        userClass.City = data[i].City;
                        userClass.ZipCode = data[i].ZipCode;
                        userClass.Comment = data[i].Comment;
                        userClass.UserType = data[i].UserType;
                        userClass.Designation = data[i].Designation;
                        userClass.AdminId = data[i].AdminId == null ? 0 : (int)data[i].AdminId;
                        userDetails.Add(userClass);
                    }
                }
                return userDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<AdminDetails> GetDashboardUserTableDetails()
        {
            List<AdminDetails> userDetails = new List<AdminDetails>();

            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                userDetails = (from z in db.tbl_Administrator
                               join b in db.tbl_Package on z.PackageId equals b.Id
                               where z.Type=="Admin"
                               select new AdminDetails
                               {
                                   Id = z.Id,
                                   PackageId = (int)z.PackageId,
                                   PackageName = b.PackageName,
                                   Name = z.Name,
                                   Email = z.Email,
                                   Mobile = z.Mobile,
                                   State = z.State,
                                   City = z.City,
                                   ZipCode = z.ZipCode,
                                   Country = z.Country,
                                   Status = z.Status,
                               }).ToList();
                //if (data != null)
                //{
                //    for (int i = 0; i < data.Count; i++)
                //    {
                //        AdminDetails userClass = new AdminDetails();
                //        userClass.Id = data[i].Id;
                //        userClass.PackageId = data[i].PackageId;
                //        userClass.PackageName = data[i].PackageName;
                //        userClass.Name = data[i].Name;
                //        userClass.Email = data[i].Email;
                //        userClass.Mobile = data[i].Mobile;
                //        userClass.State = data[i].State;
                //        userClass.City = data[i].City;
                //        userClass.ZipCode = data[i].ZipCode;
                //        userClass.Country = data[i].Country;
                //        userClass.Status = data[i].Status;
                //        userDetails.Add(userClass);
                //    }
                //}
                return userDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddUserGroup(User user)
        {
            bool data = false;
            tbl_StaffUser userDetails = new tbl_StaffUser();
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();

                if (user != null)
                {
                    userDetails.StaffId = user.StaffId;
                    userDetails.StaffName = user.StaffName;
                    userDetails.Email = user.Email;
                    userDetails.Mobile1 = user.Mobile1;
                    userDetails.Mobile2 = user.Mobile2;
                    userDetails.Address = user.Address;
                    userDetails.State = user.State;
                    userDetails.City = user.City;
                    userDetails.ZipCode = user.ZipCode;
                    userDetails.Comment = user.Comment;
                    userDetails.UserType = user.UserType;
                    userDetails.Designation = user.Designation;
                    userDetails.Password = user.Password;
                    if(user.AdminId!=null || user.AdminId!=0)
                    {
                        userDetails.AdminId = user.AdminId;
                    }
                }
                db.tbl_StaffUser.Add(userDetails);
                db.SaveChanges();
                data = true;

                return data;
            }
            catch (Exception ex)
            {
                //throw ex;
                return data;
            }
        }

        public static User GetUserDetailForEdit(string Id)
        {
            int uId = Convert.ToInt32(Id);
            User userDetails = new User();
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                userDetails = (from z in db.tbl_StaffUser
                               where z.Id == uId
                               select
                                  new User
                                  {
                                      Id = z.Id,
                                      StaffId = z.StaffId,
                                      StaffName = z.StaffName,
                                      Email = z.Email,
                                      Mobile1 = z.Mobile1,
                                      Mobile2 = z.Mobile2,
                                      Address = z.Address,
                                      State = z.State,
                                      City = z.City,
                                      ZipCode = z.ZipCode,
                                      Comment = z.Comment,
                                      UserType = z.UserType,
                                      Designation = z.Designation,
                                      AdminId = (int)z.AdminId
                                  }).FirstOrDefault();
                return userDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateUserGroup(User user)
        {
            bool data = false;
            //tbl_StaffUser userDetails = new tbl_StaffUser();
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var userDetails = (from z in db.tbl_StaffUser where z.Id == user.Id select z).FirstOrDefault();
                if (userDetails != null)
                {
                    userDetails.StaffId = user.StaffId;
                    userDetails.StaffName = user.StaffName;
                    userDetails.Email = user.Email;
                    userDetails.Mobile1 = user.Mobile1;
                    userDetails.Mobile2 = user.Mobile2;
                    userDetails.Address = user.Address;
                    userDetails.State = user.State;
                    userDetails.City = user.City;
                    userDetails.ZipCode = user.ZipCode;
                    userDetails.Comment = user.Comment;
                    userDetails.Designation = user.Designation;
                    userDetails.AdminId = user.AdminId;
                    db.SaveChanges();
                    data = true;
                }
                else
                {
                    data = false;
                }

                return data;
            }
            catch (Exception ex)
            {
                //throw ex;
                return data;
            }
        }

        public static bool DeleteUserDetail(string id)
        {
            bool result = false;
            try
            {
                int uId = Convert.ToInt32(id);
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var data = (from z in db.tbl_StaffUser where z.Id == uId select z).FirstOrDefault();
                db.tbl_StaffUser.Remove(data);
                db.SaveChanges();
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                //throw ex;
                return result;
            }
        }

        public static List<Visitor> GetVisitorPartialDetails()
        {
            var url = CommonUtil.GetBaseUrl();
            List<Visitor> visitorDetails = new List<Visitor>();
            int adminId = Convert.ToInt32(HttpContext.Current.Session["AdminId"].ToString());
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                if (adminId <= 1)
                {
                    var data = (from z in db.tbl_Visitor select z).ToList();
                    if (data != null)
                    {
                        for (int i = 0; i < data.Count; i++)
                        {
                            Visitor visitorClass = new Visitor();
                            visitorClass.Id = data[i].Id;
                            visitorClass.VisitorName = data[i].VisitorName;
                            visitorClass.TotalVisitor = data[i].TotalVisitor;
                            visitorClass.WhomToVisit = data[i].WhomToVisit;
                            visitorClass.VhicleType = data[i].VhicleType;
                            visitorClass.VhicleNumber = data[i].VhicleNumber;
                            visitorClass.TimeIn = data[i].TimeIn;
                            visitorClass.TimeOut = data[i].TimeOut;
                            visitorClass.Date = (DateTime)data[i].Date;
                            visitorClass.Image = url + data[i].Image;
                            visitorClass.Mobile = data[i].Mobile;
                            visitorClass.Purpose = data[i].Purpose;
                            visitorClass.AdminId = data[i].AdminId == null ? 0 : (int)data[i].AdminId;
                            visitorDetails.Add(visitorClass);
                        }
                    }
                }
                else
                {
                    var data = (from z in db.tbl_Visitor where z.AdminId == adminId select z).ToList();
                    if (data != null)
                    {
                        for (int i = 0; i < data.Count; i++)
                        {
                            Visitor visitorClass = new Visitor();
                            visitorClass.Id = data[i].Id;
                            visitorClass.VisitorName = data[i].VisitorName;
                            visitorClass.TotalVisitor = data[i].TotalVisitor;
                            visitorClass.WhomToVisit = data[i].WhomToVisit;
                            visitorClass.VhicleType = data[i].VhicleType;
                            visitorClass.VhicleNumber = data[i].VhicleNumber;
                            visitorClass.TimeIn = data[i].TimeIn;
                            visitorClass.TimeOut = data[i].TimeOut;
                            visitorClass.Date = (DateTime)data[i].Date;
                            visitorClass.Image = url + data[i].Image;
                            visitorClass.Mobile = data[i].Mobile;
                            visitorClass.Purpose = data[i].Purpose;
                            visitorClass.AdminId = data[i].AdminId == null ? 0 : (int)data[i].AdminId;
                            visitorDetails.Add(visitorClass);
                        }
                    }
                }
                return visitorDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Visitor> GetVisitorPartialDetailsByFilter(string name, string date, string vehNo, string loggedAdminId)
        {
            var url = CommonUtil.GetBaseUrl();
            List<Visitor> visitorDetails = new List<Visitor>();
            int adminId;
            if (string.IsNullOrEmpty(loggedAdminId))
            {
                adminId = Convert.ToInt32(HttpContext.Current.Session["AdminId"].ToString());
            }
            else
            {
                adminId= Convert.ToInt32(loggedAdminId);
            }

            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                if (!string.IsNullOrEmpty(date))
                {
                    date = (Convert.ToDateTime(date)).ToString("MM/dd/yyyy");
                }
                else
                {
                    date = "";
                }
                if (name == null)
                {
                    name = "";
                }
                if (vehNo == null)
                {
                    vehNo = "";
                }
                var data = db.sp_GetVisitorsForAdmin(date, name, vehNo, adminId).ToList();
                if (data != null)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        Visitor visitorClass = new Visitor();
                        visitorClass.Id = data[i].Id;
                        visitorClass.VisitorName = data[i].VisitorName;
                        visitorClass.TotalVisitor = data[i].TotalVisitor;
                        visitorClass.WhomToVisit = data[i].WhomToVisit;
                        visitorClass.VhicleType = data[i].VhicleType;
                        visitorClass.VhicleNumber = data[i].VhicleNumber;
                        visitorClass.TimeIn = data[i].TimeIn;
                        visitorClass.TimeOut = data[i].TimeOut;
                        visitorClass.Date = (DateTime)data[i].Date;
                        visitorClass.Image = url + data[i].Image;
                        visitorClass.Mobile = data[i].Mobile;
                        visitorClass.Purpose = data[i].Purpose;
                        visitorClass.AdminId = data[i].AdminId == null ? 0 : (int)data[i].AdminId;
                        visitorDetails.Add(visitorClass);
                    }
                }
                return visitorDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddNewUserAdmin(AdminDetails user)
        {
            bool data = false;
            tbl_Administrator userDetails = new tbl_Administrator();
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();

                if (user != null)
                {
                    userDetails.AdminId = user.AdminId;
                    userDetails.PackageId = user.PackageId;
                    userDetails.Username = user.Username;
                    userDetails.Password = user.Password;
                    userDetails.Name = user.Name;
                    userDetails.Email = user.Email;
                    userDetails.State = user.State;
                    userDetails.City = user.City;
                    userDetails.ZipCode = user.ZipCode;
                    userDetails.Mobile = user.Mobile;
                    userDetails.Country = user.Country;
                    userDetails.Status = user.Status;
                    userDetails.Type = "Admin";
                }
                db.tbl_Administrator.Add(userDetails);
                db.SaveChanges();
                data = true;

                return data;
            }
            catch (Exception ex)
            {
                //throw ex;
                return data;
            }
        }

        public static List<Package> GetPackageForDropDown()
        {
            List<Package> package = new List<Package>();
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                package = (from z in db.tbl_Package
                           select
                           new Package
                           {
                               Id = z.Id,
                               PackageName = z.PackageName
                           }).ToList();

                return package;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<GateKeeperDetails> GetGateKeeperTableDetails(int adminId)
        {
            List<GateKeeperDetails> userDetails = new List<GateKeeperDetails>();

            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var data = (from z in db.tbl_Gatekeeper where z.AdminId == adminId select z).ToList();
                if (data != null)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        GateKeeperDetails userClass = new GateKeeperDetails();
                        userClass.Id = data[i].Id;
                        userClass.AdminId = data[i].AdminId;
                        userClass.GatekeeperId = data[i].GatekeeperId;
                        userClass.Name = data[i].Name;
                        userClass.Email = data[i].Email;
                        userClass.Mobile = data[i].Mobile;
                        userClass.Country = data[i].Country;
                        userClass.State = data[i].State;
                        userClass.City = data[i].City;
                        userClass.ZipCode = data[i].ZipCode;
                        userDetails.Add(userClass);
                    }
                }
                return userDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddGatekeeperDetails(GateKeeperDetails user)
        {
            bool data = false;
            tbl_Gatekeeper userDetails = new tbl_Gatekeeper();
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();

                if (user != null)
                {
                    userDetails.AdminId = user.AdminId;
                    userDetails.GatekeeperId = user.GatekeeperId;
                    userDetails.Name = user.Name;
                    userDetails.Email = user.Email;
                    userDetails.State = user.State;
                    userDetails.City = user.City;
                    userDetails.ZipCode = user.ZipCode;
                    userDetails.Mobile = user.Mobile;
                    userDetails.Country = user.Country;
                    userDetails.Password = user.Password;
                }
                db.tbl_Gatekeeper.Add(userDetails);
                db.SaveChanges();
                data = true;

                return data;
            }
            catch (Exception ex)
            {
                //throw ex;
                return data;
            }
        }

        public static bool UpdateGatekeeper(GateKeeperDetails user)
        {
            bool data = false;
            //tbl_StaffUser userDetails = new tbl_StaffUser();
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var userDetails = (from z in db.tbl_Gatekeeper where z.Id == user.Id select z).FirstOrDefault();
                if (userDetails != null)
                {
                    userDetails.GatekeeperId = user.GatekeeperId;
                    userDetails.Email = user.Email;
                    userDetails.Name = user.Name;
                    userDetails.Mobile = user.Mobile;
                    userDetails.State = user.State;
                    userDetails.City = user.City;
                    userDetails.ZipCode = user.ZipCode;
                    userDetails.Country = user.Country;
                    db.SaveChanges();
                    data = true;
                }
                else
                {
                    data = false;
                }

                return data;
            }
            catch (Exception ex)
            {
                //throw ex;
                return data;
            }
        }

        public static bool DeleteGatekeeperDetail(string id)
        {
            bool result = false;
            try
            {
                int uId = Convert.ToInt32(id);
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var data = (from z in db.tbl_Gatekeeper where z.Id == uId select z).FirstOrDefault();
                db.tbl_Gatekeeper.Remove(data);
                db.SaveChanges();
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                //throw ex;
                return result;
            }
        }

        public static GateKeeperDetails GetGatekeeperDetailForEdit(string Id)
        {
            int uId = Convert.ToInt32(Id);
            GateKeeperDetails userDetails = new GateKeeperDetails();
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                userDetails = (from z in db.tbl_Gatekeeper
                               where z.Id == uId
                               select
                                  new GateKeeperDetails
                                  {
                                      Id = z.Id,
                                      AdminId = z.AdminId,
                                      GatekeeperId = z.GatekeeperId,
                                      Name = z.Name,
                                      Email = z.Email,
                                      Mobile = z.Mobile,
                                      Country = z.Country,
                                      State = z.State,
                                      City = z.City,
                                      ZipCode = z.ZipCode
                                  }).FirstOrDefault();
                return userDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Admin API
        public static void UpdateAdminAuthcode(AdminDetails user)
        {
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var data = (from c in db.tbl_Administrator where c.AdminId == user.AdminId select c).FirstOrDefault();
                data.AuthCode = user.AuthCode;
                db.SaveChanges();
                //User userData = new User();
                //userData.StaffName = data.StaffName;
                //userData.StaffId = data.StaffId;
                //userData.Id = data.Id;
                //userData.AuthCode = user.AuthCode;
                //userData.Email = data.Email;
                //return userData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public static void UpdateStaffUserAuthcode(User user)
        {
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var data = (from c in db.tbl_StaffUser where c.StaffId == user.StaffId select c).FirstOrDefault();
                data.AuthCode = user.AuthCode;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Visitor> GetVisitorDetailsByStaffId(DateTime date, int staffId = 0)
        {
            string url = CommonUtil.GetBaseUrl();
            string selectedDate = string.Empty;
            List<Visitor> visitorDetails = new List<Visitor>();
            if (date != null)
            {
                selectedDate = date.ToString("MM/dd/yyyy");
            }
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var data = db.sp_GetVisitorByStaffId(selectedDate, staffId.ToString()).ToList();
                if (data != null)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        Visitor visitorClass = new Visitor();
                        visitorClass.Id = data[i].Id;
                        visitorClass.VisitorName = data[i].VisitorName;
                        visitorClass.TotalVisitor = data[i].TotalVisitor;
                        visitorClass.WhomToVisit = data[i].WhomToVisit;
                        visitorClass.VhicleType = data[i].VhicleType;
                        visitorClass.VhicleNumber = data[i].VhicleNumber;
                        visitorClass.TimeIn = data[i].TimeIn;
                        visitorClass.TimeOut = data[i].TimeOut;
                        visitorClass.Date = (DateTime)data[i].Date;
                        visitorClass.Image = url + data[i].Image;
                        visitorClass.Mobile = data[i].Mobile;
                        visitorClass.Purpose = data[i].Purpose;
                        visitorClass.AdminId = data[i].AdminId == null ? 0 : (int)data[i].AdminId;
                        visitorDetails.Add(visitorClass);
                    }
                }
                return visitorDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static AdminDetails UpdateAdminStatus(string adminAppId)
        {
            AdminDetails data = new AdminDetails();
            //tbl_StaffUser userDetails = new tbl_StaffUser();
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var userDetails = (from z in db.tbl_Administrator where z.AdminId == adminAppId select z).FirstOrDefault();
                if (userDetails != null)
                {
                    userDetails.Status = "Allowed";
                    userDetails.ExpiryDate = DateTime.Now.AddYears(1);
                    db.SaveChanges();

                    data.Id = userDetails.Id;
                    data.AdminId = userDetails.AdminId;
                    data.Name = userDetails.Name;
                    data.Email = userDetails.Email;
                    data.Username = userDetails.Username;
                }

                return data;
            }
            catch (Exception ex)
            {
                //throw ex;
                return data;
            }
        }

        public static bool AddPaymentDetails(PaymentDetails payment)
        {
            bool data = false;
            tbl_Payment paymentDetails = new tbl_Payment();
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();

                if (payment != null)
                {
                    paymentDetails.AdminId = payment.AdminId;
                    paymentDetails.AdminAppId = payment.AdminAppId;
                    paymentDetails.PaymentId = payment.PaymentId;
                    paymentDetails.Guid = payment.Guid;
                    paymentDetails.Token = payment.Token;
                    paymentDetails.PayerID = payment.PayerID;
                    paymentDetails.Date = DateTime.Now;
                    paymentDetails.Amount = payment.Amount;
                }
                db.tbl_Payment.Add(paymentDetails);
                db.SaveChanges();
                data = true;

                return data;
            }
            catch (Exception ex)
            {
                //throw ex;
                return data;
            }
        }

        public static bool ForgetPassword(string email, string password)
        {
            bool data = false;
            //tbl_StaffUser userDetails = new tbl_StaffUser();
            if (email != "")
            {
                try
                {
                    MailMessage mm = new MailMessage();
                    mm.To.Add(email);
                    mm.From = new MailAddress("noreply@gatekeeperr.com");
                    mm.Subject = "Gatekeeper Password";
                    mm.Body = "Your Gatekeeper application password is '" + password + "'. Now you can login with this. Thanks for using our Gatekeeper APP.";
                    mm.IsBodyHtml = true;
                    mm.Priority = MailPriority.High;
                    SmtpClient smtp = new SmtpClient();
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("noreply@gatekeeperr.com", "Techno@123");
                    smtp.EnableSsl = true;
                    smtp.Send(mm);

                    data = true;
                    return data;
                }
                catch (Exception ex)
                {
                    return data;
                }
            }
            else
            {
                return data;
            }
        }

        public static bool AdminEmailReminder(string Id)
        {
            bool data = false;
            //tbl_StaffUser userDetails = new tbl_StaffUser();
            try
            {
                int adminId = Convert.ToInt32(Id);
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var userDetails = (from z in db.tbl_Administrator where z.Id == adminId select z).FirstOrDefault();
                if (userDetails != null)
                {
                    string email = userDetails.Email;

                    DateTime expiry = (DateTime)userDetails.ExpiryDate;
                    StringBuilder message = new StringBuilder();
                    message.Append("<html>");
                    message.Append("<body>");
                    //message.Append("<div>Congratulations</div><br />");
                    message.Append("<div>Your Gatekeeper app access is expired on '" + expiry.ToString("dd-MM-yyyy") + "'.</div><br />");
                    message.Append("<div>If you want to renew your plan then go to our website and Renew it.</div><br />");
                    message.Append("</body>");
                    message.Append("</html>");
                    //string message = "Your Gatekeeper app access is expired on '"+ expiry.ToString("dd-MM-yyyy") + "'. If you want to renew your plan then go to our website and Renew it.";
                    string subject = "Expiry date reminder for Gatekeeper app";
                    CommonUtil.SentEmail(email, message.ToString(), subject);
                    data = true;
                }
                else
                {
                    data = false;
                }

                return data;
            }
            catch (Exception ex)
            {
                //throw ex;
                return data;
            }
        }

        public static AdminDetails GetAdminDetailsById(string Id)
        {
            int uId = Convert.ToInt32(Id);
            AdminDetails userDetails = new AdminDetails();
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                userDetails = (from z in db.tbl_Administrator
                               where z.Id == uId
                               select
                                  new AdminDetails
                                  {
                                      Id = z.Id,
                                      AdminId = z.AdminId,
                                      PackageId = (int)z.PackageId,
                                      Name = z.Name,
                                      Email = z.Email,
                                      Mobile = z.Mobile,
                                      Country = z.Country,
                                      State = z.State,
                                      City = z.City,
                                      ZipCode = z.ZipCode,
                                      Username = z.Username
                                  }).FirstOrDefault();
                return userDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Package GetPackage(string Id)
        {
            int uId = Convert.ToInt32(Id);
            Package packageDetails = new Package();
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                packageDetails = (from z in db.tbl_Package
                                  where z.Id == uId
                                  select
                                     new Package
                                     {
                                         Id = z.Id,
                                         PackageName = z.PackageName,
                                         Description = z.Description,
                                         AnnuallyPrice = z.AnnuallyPrice
                                     }).FirstOrDefault();
                return packageDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ValidateAlreadyExistEmail(string email)
        {
            var data = false;
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var result = (from z in db.tbl_Administrator where z.Email == email select z).FirstOrDefault();
                if (result != null)
                    data = true;
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ValidateAlreadyExistUsername(string username)
        {
            var data = false;
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var result = (from z in db.tbl_Administrator where z.Username == username select z).FirstOrDefault();
                if (result != null)
                    data = true;
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Country> GetCountryDetails()
        {
            List<Country> country = new List<Country>();
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                country = (from z in db.tbl_country
                           select
                           new Country
                           {
                               Id = z.Id,
                               country_id = (int)z.country_id,
                               name = z.name,
                               iso_code_2 = z.iso_code_2,
                               iso_code_3 = z.iso_code_3,
                               address_format = z.address_format,
                               postcode_required = z.postcode_required,
                               status = z.status
                           }).ToList();

                return country;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<State> GetStateDetails(int countryId)
        {
            List<State> state = new List<State>();
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                state = (from z in db.tbl_state
                         where z.country_id == countryId
                         select
                           new State
                           {
                               Id = z.Id,
                               country_id = (int)z.country_id,
                               zone_id = (int)z.zone_id,
                               name = z.name,
                               code = z.code,
                               status = z.status
                           }).ToList();

                return state;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string RemainingDays(int adminId)
        {
            var result = string.Empty;
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var data = (from z in db.tbl_Administrator
                            where z.Id == adminId && z.Status == "Allowed"
                            select
                              new AdminDetails
                              {
                                  Id = z.Id,
                                  Name = z.Name,
                                  ExpiryDate = (DateTime)z.ExpiryDate,
                                  Status = z.Status
                              }).FirstOrDefault();
                DateTime exDate = data.ExpiryDate;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ForgetPassword(string email)
        {
            string data = string.Empty;
            //tbl_StaffUser userDetails = new tbl_StaffUser();
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var userDetails = (from z in db.tbl_Administrator where z.Email == email select z).FirstOrDefault();
                if (userDetails != null)
                {
                    string password = userDetails.Password;
                    string username = userDetails.Username;

                    StringBuilder message = new StringBuilder();
                    message.Append("<html>");
                    message.Append("<body>");
                    //message.Append("<div>Congratulations</div><br />");
                    message.Append("<div>You are registered with '" + email + "'. Your username is '" + username + "' and password is '" + password + "' </div><br />");
                    message.Append("<div>Thanks for stay on our Gatekeeper application.</div><br />");
                    message.Append("</body>");
                    message.Append("</html>");
                    //string message = "Your Gatekeeper app access is expired on '"+ expiry.ToString("dd-MM-yyyy") + "'. If you want to renew your plan then go to our website and Renew it.";
                    string subject = "Username and Password for login to Gatekeeper web application";
                    CommonUtil.SentEmail(email, message.ToString(), subject);
                    data = "Success";
                }
                else
                {
                    data = "Your email does not exist in our system";
                }

                return data;
            }
            catch (Exception ex)
            {
                //throw ex;
                return data;
            }
        }

        public static bool IsVisitorOutOfService(Visitor visitor)
        {
            bool result = false;
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var data = (from z in db.tbl_Administrator
                            join p in db.tbl_Package on z.PackageId equals p.Id
                            select new Package
                            {
                                Id = p.Id,
                                NoOfVisitorSupported = p.NoOfVisitorSupported
                            }).FirstOrDefault();
                //result = true;
                //string visitorDate = (Convert.ToDateTime(visitor.AddDate)).ToString("MM/dd/yyyy");
                DateTime oDate = DateTime.ParseExact(visitor.AddDate, "yyyy-MM-dd HH:mm tt", null);
                string visitorDate = oDate.ToString("MM/dd/yyyy");
                var totalCount = db.sp_GetTotalVisitorForToday(visitorDate, visitor.AdminId.ToString()).FirstOrDefault();
                if (totalCount < data.NoOfVisitorSupported)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                //throw ex;
                return result;
            }
        }

    }
}