using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using VisitorManager.BusinessLogic;
using VisitorManager.Models;
using VisitorManager.Providers;
using VisitorManager.Results;

namespace VisitorManager.Controllers
{
    //[Authorize]
    //[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion




        #region MyAPI

        // POST api/Account/ShowVisitorByGatekeeperId
        [Route("ShowVisitorByGatekeeperId")]
        [HttpGet]
        public IHttpActionResult ShowVisitorByGatekeeperId(int id)
        {
            var result = "";
            try
            {
                List<Visitor> visitorDetails = new List<Visitor>();
                visitorDetails = DBHelper.GetVisitorsByGatekeeperId(id);
                if (visitorDetails.Count == 0)
                {
                    result = "No Record Found";
                }
                else
                {
                    result = "Success";
                }
                return Ok(new { message = result, data = visitorDetails });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "error " + ex });
            }
        }

        // POST api/Account/ShowVisitor
        [Route("ShowVisitor")]
        [HttpPost]
        public IHttpActionResult ShowVisitor(Visitor visitor)
        {
            var result = "";
            try
            {
                List<Visitor> visitorDetails = new List<Visitor>();
                visitorDetails = DBHelper.GetVisitorDetailsAPI(visitor);
                if (visitorDetails.Count == 0)
                {
                    result = "No Record Found";
                }
                else
                {
                    result = "Success";
                }
                return Ok(new { message = result, data = visitorDetails });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "error " + ex });
            }
        }

        // POST api/Account/GateKeeperVerifyOTP
        [Route("GateKeeperVerifyOTP")]
        [HttpPost]
        public IHttpActionResult GateKeeperVerifyOTP(User registerUser)
        {
            User user = new User();
            var result = "";
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                user = db.tbl_StaffUser.Where(x => x.OTP == registerUser.OTP && x.Id == registerUser.Id)
                    .Select(x => new User
                    {
                        Id = x.Id,
                        AdminId = x.AdminId == null ? 0 : (int)x.AdminId
                    }).FirstOrDefault();

                if (user != null)
                {
                    result = "Success";
                }
                else
                {
                    result = "Invalid OTP";
                }
                return Ok(new { message = result, data = user });
            }
            catch (Exception ex)
            {
                return Ok("error");
            }
        }

        // POST api/Account/GateKeeperLogin
        [Route("GateKeeperLogin")]
        [HttpPost]
        public IHttpActionResult GateKeeperLogin(GateKeeperDetails registerUser)
        {
            GateKeeperDetails user = new GateKeeperDetails();
            var result = "";
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                Guid authCode = Guid.NewGuid();
                var check = (from z in db.tbl_Gatekeeper where z.GatekeeperId == registerUser.GatekeeperId && z.Password == registerUser.Password select z).FirstOrDefault();
                if (check != null)
                {
                    registerUser.AuthCode = authCode.ToString();
                    DBHelper.UpdateGateKeeperAuthcode(registerUser);
                }
                user = db.tbl_Gatekeeper.Where(x => x.GatekeeperId == registerUser.GatekeeperId && x.Password == registerUser.Password)
                    .Select(x => new GateKeeperDetails
                    {
                        Id = x.Id,
                        AdminId = x.AdminId == null ? 0 : (int)x.AdminId,
                        GatekeeperId = x.GatekeeperId,
                        Name = x.Name,
                        AuthCode = x.AuthCode
                    }).FirstOrDefault();

                if (user != null)
                {
                    result = "Success";
                }
                else
                {
                    result = "Kindly check your login credentials. You might have entered wrong details.";
                }
                return Ok(new { message = result, data = user });
            }
            catch (Exception ex)
            {
                return Ok("error");
            }
        }

        // POST api/Account/GateKeeperCheckAuthCode
        [Route("GateKeeperCheckAuthCode")]
        [HttpPost]
        public IHttpActionResult GateKeeperCheckAuthCode(GateKeeperDetails registerUser)
        {
            GateKeeperDetails user = new GateKeeperDetails();
            var result = "";
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                user = db.tbl_Gatekeeper.Where(x => x.AuthCode == registerUser.AuthCode)
                    .Select(x => new GateKeeperDetails
                    {
                        Id = x.Id,
                        AuthCode = x.AuthCode,
                        AdminId = x.AdminId == null ? 0 : (int)x.AdminId
                    }).FirstOrDefault();

                if (user != null)
                {
                    result = "Success";
                }
                else
                {
                    result = "Invalid AuthCode";
                }
                return Ok(new { message = result, data = user });
            }
            catch (Exception ex)
            {
                return Ok("error");
            }
        }

        // POST api/Account/GateKeeperSendOTP
        [Route("GateKeeperSendOTP")]
        [HttpPost]
        public IHttpActionResult GateKeeperSendOTP(User registerUser)
        {
            User user = new User();
            var result = "";
            int random = CommonUtil.GetRandomNumber(1000, 9999);
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                user = db.tbl_StaffUser.Where(x => x.Mobile1 == registerUser.Mobile1 || x.Mobile2 == registerUser.Mobile1)
                    .Select(x => new User
                    {
                        Id = x.Id,
                        StaffId = x.StaffId,
                        StaffName = x.StaffName,
                        Email = x.Email,
                        Mobile1 = x.Mobile1,
                        OTP = random.ToString()
                    }).FirstOrDefault();
                if (user != null)
                {
                    DBHelper.UpdateGateKeeperSentOTP(user);
                    string msg = "Your OTP from Mysapp is " + user.OTP + ". Kindly use this OTP for Login process. Thank You";
                    string respStr = "";
                    string strApi = @"http://www.kit19.com/ComposeSMS.aspx?username=appcentra574727&password=8473&sender=MYSAPP&to=" + registerUser.Mobile1 + "&message=" + msg + "&priority=1&dnd=1&unicode=0";
                    HttpWebRequest requestFile = (HttpWebRequest)WebRequest.Create(strApi);
                    requestFile.ContentType = "application/html";
                    HttpWebResponse webResp = requestFile.GetResponse() as HttpWebResponse;
                    if (requestFile.HaveResponse)
                    {
                        if (webResp.StatusCode == HttpStatusCode.OK || webResp.StatusCode == HttpStatusCode.Accepted)
                        {
                            StreamReader respReader = new StreamReader(webResp.GetResponseStream());
                            respStr = respReader.ReadToEnd();
                        }
                    }

                    result = "Success";
                }
                else
                {
                    result = "Mobile Number is not valid";
                }
                return Ok(new { message = result, data = user });
            }
            catch (Exception ex)
            {
                return Ok("error");
            }
        }

        // POST api/Account/GateKeeperResendOTP
        [Route("GateKeeperResendOTP")]
        [HttpPost]
        public IHttpActionResult GateKeeperResendOTP(User registerUser)
        {
            User user = new User();
            var result = "";
            int random = CommonUtil.GetRandomNumber(1000, 9999);
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                user = db.tbl_StaffUser.Where(x => x.Mobile1 == registerUser.Mobile1 || x.Mobile2 == registerUser.Mobile1)
                    .Select(x => new User
                    {
                        Id = x.Id,
                        StaffId = x.StaffId,
                        StaffName = x.StaffName,
                        Email = x.Email,
                        Mobile1 = x.Mobile1,
                        OTP = random.ToString()
                    }).FirstOrDefault();
                if (user != null)
                {
                    DBHelper.UpdateGateKeeperSentOTP(user);
                    string msg = "Your OTP from Mysapp is " + user.OTP + ". Kindly use this OTP for Login process. Thank You";
                    string respStr = "";
                    string strApi = @"http://www.kit19.com/ComposeSMS.aspx?username=appcentra574727&password=8473&sender=MYSAPP&to=" + registerUser.Mobile1 + "&message=" + msg + "&priority=1&dnd=1&unicode=0";
                    HttpWebRequest requestFile = (HttpWebRequest)WebRequest.Create(strApi);
                    requestFile.ContentType = "application/html";
                    HttpWebResponse webResp = requestFile.GetResponse() as HttpWebResponse;
                    if (requestFile.HaveResponse)
                    {
                        if (webResp.StatusCode == HttpStatusCode.OK || webResp.StatusCode == HttpStatusCode.Accepted)
                        {
                            StreamReader respReader = new StreamReader(webResp.GetResponseStream());
                            respStr = respReader.ReadToEnd();
                        }
                    }

                    result = "Success";
                }
                else
                {
                    result = "Mobile Number is not valid";
                }
                return Ok(new { message = result, data = user });
            }
            catch (Exception ex)
            {
                return Ok("error");
            }
        }

        // POST api/Account/EditVisitor
        [Route("EditVisitor")]
        [HttpGet]
        public IHttpActionResult EditVisitor(int id)
        {
            Visitor guestResponse = new Visitor();
            var result = "";
            try
            {
                guestResponse = DBHelper.GetVisitorById(id);
                if (guestResponse != null)
                {
                    result = "Success";
                }
                else
                {
                    result = "error";
                }
                return Ok(new { message = result, data = guestResponse });
            }
            catch (Exception ex)
            {
                return Ok("error");
            }
        }

        // POST api/Account/UpdateVisitor
        [Route("UpdateVisitor")]
        [HttpPost]
        public IHttpActionResult UpdateVisitor(Visitor visitor)
        {
            bool data = false;
            var result = "";
            try
            {
                data = DBHelper.UpdateVisitor(visitor);
                if (data != false)
                {
                    result = "Success";
                }
                else
                {
                    result = "Failure";
                }

                return Ok(new { message = result, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "error" + ex });
            }
        }

        // POST api/Account/AddVisitor
        [Route("AddVisitor")]
        [HttpPost]
        public IHttpActionResult AddVisitor(Visitor visitor)
        {
            Visitor data = new Visitor();
            var result = "";
            try
            {
                //bool countResult = DBHelper.IsVisitorOutOfService(visitor);
                //if (countResult == true)
                //{
                    if (visitor.base64 != null)
                    {
                        if (visitor.base64.Length != 0)
                        {
                            var path1 = CommonUtil.APIImageSave(visitor.base64);
                            visitor.Image = path1;
                        }
                        else
                        {
                            visitor.Image = "";
                        }
                    }
                    else
                    {
                        visitor.Image = "";
                    }
                    int random = CommonUtil.GetRandomNumber(1000, 9999);
                    visitor.OTP = random;
                    GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                if(visitor.AdminId <= 0)
                {
                    visitor.AdminId = (from z in db.tbl_Gatekeeper where z.Id == visitor.GatekeeperId select z.AdminId).FirstOrDefault();
                }

                    //if (visitor.Mobile != null && visitor.Mobile != "")
                    //{
                    //    string msg = "Your OTP from Gatekeeper is " + visitor.OTP + ". Kindly use this OTP for entering process. Thank You";
                    //    string respStr = "";
                    //    string strApi = @"http://www.kit19.com/ComposeSMS.aspx?username=appcentra574727&password=8473&sender=MYSAPP&to=" + visitor.Mobile + "&message=" + msg + "&priority=1&dnd=1&unicode=0";
                    //    HttpWebRequest requestFile = (HttpWebRequest)WebRequest.Create(strApi);
                    //    requestFile.ContentType = "application/html";
                    //    HttpWebResponse webResp = requestFile.GetResponse() as HttpWebResponse;
                    //    if (requestFile.HaveResponse)
                    //    {
                    //        if (webResp.StatusCode == HttpStatusCode.OK || webResp.StatusCode == HttpStatusCode.Accepted)
                    //        {
                    //            StreamReader respReader = new StreamReader(webResp.GetResponseStream());
                    //            respStr = respReader.ReadToEnd();
                    //        }
                    //    }
                    data = DBHelper.SubmitVisitorDetails(visitor);
                    if (data != null)
                    {
                        result = "Success";
                    }
                    else
                    {
                        result = "Failure";
                    }
                //}
                //else
                //{
                //    result = "Your visitor's visit limit is over";
                //}

                return Ok(new { message = result, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "error" + ex });
            }
        }

        // POST api/Account/DeleteVisitor
        [Route("DeleteVisitor")]
        [HttpDelete]
        public IHttpActionResult DeleteVisitor(int id)
        {
            try
            {
                var result = "";
                bool data = DBHelper.DeleteVisitor(id);
                if (data)
                    result = "Success";
                else
                    result = "Failure";
                return Ok(new { message = result, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "error" + ex });
            }
        }

        // POST api/Account/GetVisitorByFilter
        [Route("GetStaffByGatekeeperId")]
        [HttpGet]
        public IHttpActionResult GetStaffByGatekeeperId(string id)
        {
            List<User> guestResponse = new List<User>();
            var result = "";
            try
            {
                guestResponse = DBHelper.GetStaffByGatekeeperId(id);
                if (guestResponse != null)
                {
                    result = "Success";
                }
                else
                {
                    result = "error";
                }
                return Ok(new { message = result, Data = guestResponse });
            }
            catch (Exception ex)
            {
                return Ok("error");
            }
        }

        // POST api/Account/GetVisitorByFilter
        [Route("GetVisitorByFilter")]
        [HttpGet]
        public IHttpActionResult GetVisitorByFilter(string name, string date, string vehNo, string loggedAdminId)
        {
            List<Visitor> guestResponse = new List<Visitor>();
            var result = "";
            try
            {
                guestResponse = DBHelper.GetVisitorPartialDetailsByFilter(name, date, vehNo, loggedAdminId);
                if (guestResponse != null)
                {
                    result = "Success";
                }
                else
                {
                    result = "error";
                }
                return Ok(new { message = result, Data = guestResponse });
            }
            catch (Exception ex)
            {
                return Ok("error");
            }
        }

        // POST api/Account/VerifyVisitorOTP
        [Route("VerifyVisitorOTP")]
        [HttpPost]
        public IHttpActionResult VerifyVisitorOTP(Visitor visitoir)
        {
            Visitor user = new Visitor();
            var result = "";
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                user = db.tbl_Visitor.Where(x => x.OTP == visitoir.OTP && x.Id == visitoir.Id)
                    .Select(x => new Visitor
                    {
                        Id = x.Id,
                        AdminId = x.AdminId == null ? 0 : (int)x.AdminId
                    }).FirstOrDefault();

                if (user != null)
                {
                    result = "Success";
                }
                else
                {
                    result = "Invalid OTP";
                }
                return Ok(new { message = result, Data = user });
            }
            catch (Exception ex)
            {
                return Ok("error");
            }
        }

        //// POST api/Account/CheckReturnVisitor
        //[Route("CheckReturnVisitor")]
        //[HttpPost]
        //public IHttpActionResult CheckReturnVisitor(Visitor visitor)
        //{
        //    Visitor data = new Visitor();
        //    var result = "";
        //    try
        //    {
        //        data = DBHelper.GetReturnVisitorDetails(visitor);
        //        if (data != null)
        //        {
        //            result = "Success";
        //        }
        //        else
        //        {
        //            result = "Failure";
        //        }
        //        return Ok(new { message = result, Data = data });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { message = "error" + ex });
        //    }
        //}


        // POST api/Account/ReturnVisitor
        [Route("ReturnVisitor")]
        [HttpPost]
        public IHttpActionResult ReturnVisitor(Visitor visitor)
        {
            var data = false;
            var result = "";
            try
            {
                data = DBHelper.UpdateVisitorTimeOut(visitor);
                if (data != false)
                {
                    result = "Success";
                }
                else
                {
                    result = "Failure";
                }
                return Ok(new { message = result, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "error" + ex });
            }
        }

        #region Admin API
        // POST api/Account/AdminLogin
        [Route("AdminLogin")]
        [HttpPost]
        public IHttpActionResult AdminLogin(AdminDetails registerUser)
        {
            AdminDetails user = new AdminDetails();
            var result = "";
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                Guid authCode = Guid.NewGuid();
                var check = (from z in db.tbl_Administrator where z.Password == registerUser.Password && (z.AdminId == registerUser.AdminId || z.Username == registerUser.Username) select z).FirstOrDefault();
                if (check != null)
                {
                    registerUser.AuthCode = authCode.ToString();
                    DBHelper.UpdateAdminAuthcode(registerUser);
                }
                user = db.tbl_Administrator.Where(x => x.AdminId == registerUser.AdminId && x.Password == registerUser.Password)
                    .Select(x => new AdminDetails
                    {
                        Id = x.Id,
                        AdminId = x.AdminId,
                        Name = x.Name,
                        AuthCode = x.AuthCode
                    }).FirstOrDefault();

                if (user != null)
                {
                    result = "Success";
                }
                else
                {
                    result = "Kindly check your login credentials. You might have entered wrong details.";
                }
                return Ok(new { message = result, data = user });
            }
            catch (Exception ex)
            {
                return Ok("error");
            }
        }

        // POST api/Account/AdminCheckAuthCode
        [Route("AdminCheckAuthCode")]
        [HttpPost]
        public IHttpActionResult AdminCheckAuthCode(AdminDetails registerUser)
        {
            AdminDetails user = new AdminDetails();
            var result = "";
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                user = db.tbl_Administrator.Where(x => x.AuthCode == registerUser.AuthCode)
                    .Select(x => new AdminDetails
                    {
                        Id = x.Id,
                        AuthCode = x.AuthCode,
                        AdminId = x.AdminId
                    }).FirstOrDefault();

                if (user != null)
                {
                    result = "Success";
                }
                else
                {
                    result = "Invalid AuthCode";
                }
                return Ok(new { message = result, data = user });
            }
            catch (Exception ex)
            {
                return Ok("error");
            }
        }

        // POST api/Account/ShowStafUser
        [Route("ShowStafUser")]
        [HttpPost]
        public IHttpActionResult ShowStafUser(User user)
        {
            var result = "";
            try
            {
                List<User> userDetails = new List<User>();
                userDetails = DBHelper.GetUserTableDetails(user.AdminId, user.StaffName);
                if (userDetails.Count == 0)
                {
                    result = "No Record Found";
                }
                else
                {
                    result = "Success";
                }
                return Ok(new { message = result, data = userDetails });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "error " + ex });
            }
        }

        // POST api/Account/ShowStafUserById
        [Route("ShowStafUserById")]
        [HttpGet]
        public IHttpActionResult ShowStafUserById(string Id)
        {
            var result = "";
            try
            {
                User user = new User();
                user = DBHelper.GetUserDetailForEdit(Id);
                if (user == null)
                {
                    result = "No Record Found";
                }
                else
                {
                    result = "Success";
                }
                return Ok(new { message = result, data = user });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "error " + ex });
            }
        }

        // POST api/Account/AddStaffUser
        [Route("AddStaffUser")]
        [HttpPost]
        public IHttpActionResult AddStaffUser(User user)
        {
            bool data = false;
            var result = "";
            try
            {
                int Id = 0;
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var data1 = (from z in db.tbl_StaffUser orderby z.Id select z).FirstOrDefault();
                if (data1 != null)
                {
                    Id = data1.Id;
                    Id = Id + 1;
                }
                else
                {
                    Id = Id + 1;
                }
                var staffUserId = Id.ToString().Length == 1 ? "STAFF0" + Id + "USER" : "STAFF" + Id + "USER";
                user.StaffId = staffUserId;
                data = DBHelper.AddUserGroup(user);
                if (data != false)
                {
                    result = "Success";
                }
                else
                {
                    result = "Failure";
                }

                return Ok(new { message = result, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "error" + ex });
            }
        }

        // POST api/Account/UpdateStaff
        [Route("UpdateStaff")]
        [HttpPost]
        public IHttpActionResult UpdateStaff(User user)
        {
            bool data = false;
            var result = "";
            try
            {
                data = DBHelper.UpdateUserGroup(user);
                if (data != false)
                {
                    result = "Success";
                }
                else
                {
                    result = "Failure";
                }

                return Ok(new { message = result, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "error" + ex });
            }
        }

        // POST api/Account/DeleteStaff
        [Route("DeleteStaff")]
        [HttpDelete]
        public IHttpActionResult DeleteStaff(string id)
        {
            try
            {
                var result = "";
                bool data = DBHelper.DeleteUserDetail(id);
                if (data)
                    result = "Success";
                else
                    result = "Failure";
                return Ok(new { message = result, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "error" + ex });
            }
        }

        // POST api/Account/ShowGatekeeper
        [Route("ShowGatekeeper")]
        [HttpPost]
        public IHttpActionResult ShowGatekeeper(GateKeeperDetails user)
        {
            var result = "";
            try
            {
                List<GateKeeperDetails> userDetails = new List<GateKeeperDetails>();
                userDetails = DBHelper.GetGateKeeperTableDetails(user.AdminId);
                if (userDetails.Count == 0)
                {
                    result = "No Record Found";
                }
                else
                {
                    result = "Success";
                }
                return Ok(new { message = result, data = userDetails });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "error " + ex });
            }
        }



        // POST api/Account/AddGateKeeper
        [Route("AddGateKeeper")]
        [HttpPost]
        public IHttpActionResult AddGateKeeper(GateKeeperDetails user)
        {
            bool data = false;
            var result = "";
            try
            {
                //if (visitor.base64.Length != 0)
                //{
                //    var path1 = CommonUtil.APIImageSave(visitor.base64);
                //    visitor.Image = path1;
                //}
                //else
                //{
                //    visitor.Image = "";
                //}

                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                int Id = 0;
                var data1 = (from z in db.tbl_Gatekeeper orderby z.Id select z).FirstOrDefault();
                if (data1 != null)
                {
                    Id = data1.Id;
                    Id = Id + 1;
                }
                else
                {
                    Id = Id + 1;
                }
                var gatekeeperId = Id.ToString().Length == 1 ? "GATE0" + Id + "KEEPER" : "GATE" + Id + "KEEPER";
                user.GatekeeperId = gatekeeperId;
                data = DBHelper.AddGatekeeperDetails(user);
                if (data != false)
                {
                    result = "Success";
                }
                else
                {
                    result = "Failure";
                }

                return Ok(new { message = result, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "error" + ex });
            }
        }

        // POST api/Account/UpdateGateKeeper
        [Route("UpdateGateKeeper")]
        [HttpPost]
        public IHttpActionResult UpdateGatekeeper(GateKeeperDetails user)
        {
            try
            {
                var result = "";

                bool data = DBHelper.UpdateGatekeeper(user);
                if (data)
                    result = "Success";
                else
                    result = "Error";
                return Ok(new { message = result, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "error" + ex });
            }
        }

        // POST api/Account/GetEditGatekeeper
        [Route("GetEditGatekeeper")]
        [HttpGet]
        public IHttpActionResult GetEditGatekeeper(string Id)
        {
            var result = "";
            try
            {
                GateKeeperDetails userDetails = new GateKeeperDetails();
                userDetails = DBHelper.GetGatekeeperDetailForEdit(Id);
                if (userDetails != null)
                {
                    result = "No Record Found";
                }
                else
                {
                    result = "Success";
                }
                return Ok(new { message = result, data = userDetails });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "error " + ex });
            }
        }

        // POST api/Account/DeleteGatekeeper
        [Route("DeleteGatekeeper")]
        [HttpDelete]
        public IHttpActionResult DeleteGatekeeper(string id)
        {
            try
            {
                var result = "";
                bool data = DBHelper.DeleteGatekeeperDetail(id);
                if (data)
                    result = "Success";
                else
                    result = "Failure";
                return Ok(new { message = result, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "error" + ex });
            }
        }

        #endregion

        #region Staff User API

        // POST api/Account/StaffUserCheckAuthCode
        [Route("StaffUserCheckAuthCode")]
        [HttpPost]
        public IHttpActionResult StaffUserCheckAuthCode(User user)
        {
            User userDetails = new User();
            var result = "";
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                userDetails = db.tbl_StaffUser.Where(x => x.AuthCode == user.AuthCode)
                    .Select(x => new User
                    {
                        Id = x.Id,
                        AuthCode = x.AuthCode == null ? Guid.Empty : (Guid)x.AuthCode,
                        AdminId = x.AdminId == null ? 0 : (int)x.AdminId
                    }).FirstOrDefault();

                if (userDetails != null)
                {
                    result = "Success";
                }
                else
                {
                    result = "Invalid AuthCode";
                }
                return Ok(new { message = result, data = user });
            }
            catch (Exception ex)
            {
                return Ok("error");
            }
        }

        // POST api/Account/StaffUserLogin
        [Route("StaffUserLogin")]
        [HttpPost]
        public IHttpActionResult StaffUserLogin(User registerUser)
        {
            User user = new User();
            var result = "";
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                Guid authCode = Guid.NewGuid();
                var check = (from z in db.tbl_StaffUser where z.StaffId == registerUser.StaffId && z.Password == registerUser.Password select z).FirstOrDefault();
                if (check != null)
                {
                    registerUser.AuthCode = authCode;
                    DBHelper.UpdateStaffUserAuthcode(registerUser);
                }
                user = db.tbl_StaffUser.Where(x => x.StaffId == registerUser.StaffId && x.Password == registerUser.Password)
                    .Select(x => new User
                    {
                        Id = x.Id,
                        AdminId = x.AdminId == null ? 0 : (int)x.AdminId,
                        StaffName = x.StaffName,
                        AuthCode = x.AuthCode == null ? Guid.Empty : (Guid)x.AuthCode
                    }).FirstOrDefault();

                if (user != null)
                {
                    result = "Success";
                }
                else
                {
                    result = "Kindly check your login credentials. You might have entered wrong details.";
                }
                return Ok(new { message = result, data = user });
            }
            catch (Exception ex)
            {
                return Ok("error");
            }
        }

        #endregion

        // POST api/Account/ShowVisitorByStaffId
        [Route("ShowVisitorByStaffId")]
        [HttpPost]
        public IHttpActionResult ShowVisitorByStaffId(Visitor visitor)
        {
            var result = "";
            try
            {
                List<Visitor> visitorDetails = new List<Visitor>();
                visitorDetails = DBHelper.GetVisitorDetailsByStaffId(visitor.Date, visitor.StaffId);
                if (visitorDetails.Count == 0)
                {
                    result = "No Record Found";
                }
                else
                {
                    result = "Success";
                }
                return Ok(new { message = result, data = visitorDetails });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "error " + ex });
            }
        }

        // POST api/Account/ForgetPasswordGatekeeper
        [Route("ForgetPasswordGatekeeper")]
        [HttpPost]
        public IHttpActionResult ForgetPasswordGatekeeper(GateKeeperDetails gatekeeper)
        {
            bool user = false;
            var result = "";
            var newMessage = "";
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var check = (from z in db.tbl_Gatekeeper where z.Email == gatekeeper.Email select z).FirstOrDefault();
                if (check != null)
                {
                    string password = check.Password;
                    user = DBHelper.ForgetPassword(gatekeeper.Email, password);
                    if (user == true)
                    {
                        newMessage = "Success";
                    }
                    else
                    {
                        newMessage = "Failure";
                    }
                    result = "Your password sent to your registered Email";
                }
                else
                {
                    result = "Invalid Email";
                    newMessage = "Failure";
                }

                return Ok(new { message = result, data = newMessage });
            }
            catch (Exception ex)
            {
                return Ok("error");
            }
        }

        // POST api/Account/ForgetPasswordStaff
        [Route("ForgetPasswordStaff")]
        [HttpPost]
        public IHttpActionResult ForgetPasswordStaff(User registerUser)
        {
            bool user = false;
            var result = "";
            var newMessage = "";
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var check = (from z in db.tbl_StaffUser where z.Email == registerUser.Email select z).FirstOrDefault();
                if (check != null)
                {
                    string password = check.Password;
                    user = DBHelper.ForgetPassword(registerUser.Email, password);
                    if (user == true)
                    {
                        newMessage = "Success";
                    }
                    else
                    {
                        newMessage = "Failure";
                    }
                    result = "Your password sent to your registered Email";
                }
                else
                {
                    result = "Invalid Email";
                    newMessage = "Failure";
                }

                return Ok(new { message = result, data = newMessage });
            }
            catch (Exception ex)
            {
                return Ok("error");
            }
        }

        // POST api/Account/ForgetPasswordGatekeeper
        [Route("ForgetPasswordAdmin")]
        [HttpPost]
        public IHttpActionResult ForgetPasswordAdmin(AdminDetails registerUser)
        {
            bool user = false;
            var result = "";
            var newMessage = "";
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                var check = (from z in db.tbl_Administrator where z.Email == registerUser.Email select z).FirstOrDefault();
                if (check != null)
                {
                    string password = check.Password;
                    user = DBHelper.ForgetPassword(registerUser.Email, password);
                    if (user == true)
                    {
                        newMessage = "Success";
                    }
                    else
                    {
                        newMessage = "Failure";
                    }
                    result = "Your password sent to your registered Email";
                }
                else
                {
                    result = "Invalid Email";
                    newMessage = "Failure";
                }

                return Ok(new { message = result, data = newMessage });
            }
            catch (Exception ex)
            {
                return Ok("error");
            }
        }

        // Get api/Account/GetAdminUser
        [Route("GetAdminUser")]
        [HttpGet]
        public IHttpActionResult GetAdminUser()
        {
            List<AdminDetails> guestResponse = new List<AdminDetails>();
            guestResponse = DBHelper.GetDashboardUserTableDetails();
            return Ok(new { data = guestResponse }); //Json(guestResponse, JsonRequestBehavior.AllowGet);
        }

        // POST api/Account/AllUserLogin
        [Route("AllUserLogin")]
        [HttpPost]
        public IHttpActionResult AllUserLogin(LoginViewModel registerUser)
        {
            LoginViewModel user = new LoginViewModel();
            var result = "";
            try
            {
                GatekeeperrrrEntities db = new GatekeeperrrrEntities();
                Guid authCode = Guid.NewGuid();
                var check = (from z in db.LoginViews where z.Password == registerUser.Password && (z.UserId == registerUser.UserId || z.UserName == registerUser.UserName) select z).FirstOrDefault();
                if (check != null)
                {
                    if (check.UserType == "Admin")
                    {
                        AdminDetails admin = new AdminDetails();
                        admin.AuthCode = authCode.ToString();
                        admin.AdminId = check.UserId;
                        DBHelper.UpdateAdminAuthcode(admin);
                    }
                    else if (check.UserType == "Staff")
                    {
                        User staffUser = new User();
                        staffUser.AuthCode = authCode;
                        staffUser.StaffId = check.UserId;
                        DBHelper.UpdateStaffUserAuthcode(staffUser);
                    }
                    else if (check.UserType == "Gatekeeper")
                    {
                        GateKeeperDetails gtDetails = new GateKeeperDetails();
                        gtDetails.AuthCode = authCode.ToString();
                        gtDetails.GatekeeperId = check.UserId;
                        DBHelper.UpdateGateKeeperAuthcode(gtDetails);
                    }
                }

                user = db.LoginViews.Where(x => x.Password == registerUser.Password && (x.UserId == registerUser.UserId || x.UserName == registerUser.UserName))
                   .Select(x => new LoginViewModel
                   {
                       Id = x.Id,
                       UserId = x.UserId,
                       SName = x.SName,
                       AuthCode = x.AuthCode.ToString(),
                       Type = x.Type,
                       PackageOrAdminId = (int)x.PackageOrAdminId
                   }).FirstOrDefault();

                if (user != null)
                {
                    result = "Success";
                }
                else
                {
                    result = "Kindly check your login credentials. You might have entered wrong details.";
                }
                return Ok(new { message = result, data = user });
            }
            catch (Exception ex)
            {
                return Ok("error");
            }
        }

        #endregion

    }

}