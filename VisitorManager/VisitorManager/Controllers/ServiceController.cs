using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisitorManager.BusinessLogic;
using VisitorManager.Models;

namespace VisitorManager.Controllers
{
    public class ServiceController : Controller
    {
        // GET: Service
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(string adminId,string password, string staffId, string staffName, string email, string mobile1, string mobile2, string address, string state, string city, string zipCode, string comment, string Designation)
        {
            try
            {
                var result = "";
                User user = new User();
                user.StaffId = staffId;
                user.StaffName = staffName;
                user.Email = email;
                user.Mobile1 = mobile1;
                user.Mobile2 = mobile2;
                user.Address = address;
                user.State = state;
                user.City = city;
                user.ZipCode = zipCode;
                user.Comment = comment;
                user.Designation = Designation;
                user.UserType = "Staff";
                user.Password = password;
                if(Session["Type"].ToString() == "Admin")
                {
                    user.AdminId = Convert.ToInt32(Session["AdminId"].ToString());
                }
                else
                {
                    user.AdminId = Convert.ToInt32(adminId);
                }
                
                bool data = DBHelper.AddUserGroup(user);
                if (data)
                    result = "Success";
                else
                    result = "Failure";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GetEditUserDetails(string Id)
        {
            User user = new User();
            user = DBHelper.GetUserDetailForEdit(Id);
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UserDetailUpdate(string adminId, string staffId, string staffName, string email, string mobile1, string mobile2, string address, string state, string city, string zipCode, string comment, string Designation, string id)
        {
            try
            {
                var result = "";
                User user = new User();
                user.StaffId = staffId;
                user.StaffName = staffName;
                user.Email = email;
                user.Mobile1 = mobile1;
                user.Mobile2 = mobile2;
                user.Address = address;
                user.State = state;
                user.City = city;
                user.ZipCode = zipCode;
                user.Comment = comment;
                user.Designation = Designation;
                user.Id = Convert.ToInt32(id);
                if (Session["Type"].ToString() == "Admin")
                {
                    user.AdminId = Convert.ToInt32(Session["AdminId"].ToString());
                }
                else
                {
                    user.AdminId = Convert.ToInt32(adminId);
                }

                bool data = DBHelper.UpdateUserGroup(user);
                if (data)
                    result = "Success";
                else
                    result = "Failure";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UserDetailDelete(string id)
        {
            try
            {
                var result = "";
                bool data = DBHelper.DeleteUserDetail(id);
                if (data)
                    result = "Success";
                else
                    result = "Failure";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SignupNewUser(string adminAppId, string packageId, string username, string password, string name, string email, string mobile, string country, string state, string city, string zipCode)
        {
            try
            {
                var result = "";
                AdminDetails user = new AdminDetails();
                user.PackageId = Convert.ToInt32(packageId);
                user.Username = username;
                user.Password = password;
                user.Name = name;
                user.Email = email;
                user.Mobile = mobile;
                user.Country = country;
                user.State = state;
                user.City = city;
                user.ZipCode = zipCode;
                user.AdminId = adminAppId;
                if (user.PackageId == 1)
                {
                    user.Status = "Allowed";
                }
                else
                {
                    user.Status = "NotAllowed";
                }

                bool data = DBHelper.AddNewUserAdmin(user);
                if (data)
                {
                    Session["PaymentName"] = name;
                    Session["PaymentEmail"] = email;
                    Session["PaymentMobile"] = mobile;
                    Session["PackageId"] = packageId;
                    Session["AdminAppId"] = adminAppId;
                    result = "Success";
                }
                else
                {
                    result = "Failure";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GetUserGroup()
        {
            List<Package> package = new List<Package>();
            package = DBHelper.GetPackageForDropDown();
            return Json(package, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddGatekeeper(string gatekeeperId, string name, string email, string mobile, string country, string state, string city, string zipCode, string password)
        {
            try
            {
                var result = "";

                GateKeeperDetails user = new GateKeeperDetails();
                if (Session["AdminId"] != null)
                {
                    int adminId = Convert.ToInt32(Session["AdminId"].ToString());
                    user.AdminId = adminId;
                }
                user.GatekeeperId = gatekeeperId;
                user.Name = name;
                user.Email = email;
                user.Mobile = mobile;
                user.Country = country;
                user.State = state;
                user.City = city;
                user.ZipCode = zipCode;
                user.Password = password;

                bool data = DBHelper.AddGatekeeperDetails(user);
                if (data)
                    result = "Success";
                else
                    result = "Failure";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GatekeeperDetailUpdate(string gatekeeperId, string name, string email, string mobile, string country, string state, string city, string zipCode, string id)
        {
            try
            {
                var result = "";

                GateKeeperDetails user = new GateKeeperDetails();
                if (Session["AdminId"] != null)
                {
                    int adminId = Convert.ToInt32(Session["AdminId"].ToString());
                    user.AdminId = adminId;
                }
                user.GatekeeperId = gatekeeperId;
                user.Name = name;
                user.Email = email;
                user.Mobile = mobile;
                user.Country = country;
                user.State = state;
                user.City = city;
                user.ZipCode = zipCode;
                user.Id = Convert.ToInt32(id);

                bool data = DBHelper.UpdateGatekeeper(user);
                if (data)
                    result = "Success";
                else
                    result = "Failure";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GatekeeperDelete(string id)
        {
            try
            {
                var result = "";
                bool data = DBHelper.DeleteGatekeeperDetail(id);
                if (data)
                    result = "Success";
                else
                    result = "Failure";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GetEditGatekeeperDetails(string Id)
        {
            GateKeeperDetails user = new GateKeeperDetails();
            user = DBHelper.GetGatekeeperDetailForEdit(Id);
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EmailReminder(string Id)
        {
            try
            {
                var result = "";

                bool data = DBHelper.AdminEmailReminder(Id);
                if (data)
                    result = "Success";
                else
                    result = "Failure";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult RenewPackage(string packageId)
        {
            try
            {
                var result = "";
                AdminDetails user = new AdminDetails();
                //user.PackageId = Convert.ToInt32(packageId);
                //user.Username = username;
                //user.Password = password;
                //user.Name = name;
                //user.Email = email;
                //user.Mobile = mobile;
                //user.Country = country;
                //user.State = state;
                //user.City = city;
                //user.ZipCode = zipCode;
                //user.AdminId = adminAppId;

                user = DBHelper.GetAdminDetailsById(Session["AdminId"].ToString());
                if (user != null)
                {
                    Session["PaymentName"] = user.Name;
                    Session["PaymentEmail"] = user.Email;
                    Session["PaymentUsername"] = user.Username;
                    Session["PaymentMobile"] = user.Mobile;
                    Session["PackageId"] = packageId;
                    Session["AdminAppId"] = user.AdminId;
                    result = "Success";
                }
                else
                {
                    result = "Failure";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GetPackageDetails(string Id)
        {
            Package package = new Package();
            package = DBHelper.GetPackage(Id);
            return Json(package, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckAlreadyExistEmail(string email)
        {
            try
            {
                var result = "";

                bool data = DBHelper.ValidateAlreadyExistEmail(email);
                if (data)
                    result = "Success";
                else
                    result = "Failure";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult CheckAlreadyExistUsername(string username)
        {
            try
            {
                var result = "";

                bool data = DBHelper.ValidateAlreadyExistUsername(username);
                if (data)
                    result = "Success";
                else
                    result = "Failure";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GetCountry()
        {
            List<Country> country = new List<Country>();
            country = DBHelper.GetCountryDetails();
            return Json(country, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetState(string countryId)
        {
            int id = Convert.ToInt32(countryId);
            List<State> state = new List<State>();
            state = DBHelper.GetStateDetails(id);
            return Json(state, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetAdminUser()
        {
            List<AdminDetails> guestResponse = new List<AdminDetails>();
            guestResponse = DBHelper.GetDashboardUserTableDetails();
            return Json(guestResponse, JsonRequestBehavior.AllowGet);
        }

    }
}