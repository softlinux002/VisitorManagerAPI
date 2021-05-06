using PayPal.Api;
using paytm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VisitorManager.BusinessLogic;
using VisitorManager.Models;

namespace VisitorManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult MainPage()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            int Id = 0;
            GatekeeperrrrEntities db = new GatekeeperrrrEntities();
            var data = (from z in db.tbl_Administrator orderby z.Id descending select z).FirstOrDefault();
            if (data != null)
            {
                Id = data.Id;
                Id = Id + 1;
            }
            else
            {
                Id = Id + 1;
            }
            var adminAppId = Id.ToString().Length == 1 ? "ADMIN0" + Id + "APP" : "ADMIN" + Id + "APP";
            ViewBag.adminAppId = adminAppId;
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var result = string.Empty;
            AdminDetails admin = new AdminDetails();
            admin = DBHelper.AdminLogin(username, password);
            if (admin != null)
            {
                Session["Username"] = username;
                Session["AdminEmail"] = admin.Email;
                Session["Status"] = admin.Status;
                Session["Type"] = admin.Type;
                Session["AdminName"] = admin.Name;
                Session["PackageId"] = admin.PackageId;
                Session["AdminId"] = admin.Id;
                result = Session["Type"].ToString() + "," + admin.Status;
            }
            else
            {
                result = "Failure";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public PartialViewResult GetDashboardUserTablePartial()
        {
            List<AdminDetails> guestResponse = new List<AdminDetails>();
            guestResponse = DBHelper.GetDashboardUserTableDetails();
            return PartialView("_DashboardUserPartial", guestResponse);
        }

        public ActionResult AddStaffUser()
        {
            int Id = 0;
            GatekeeperrrrEntities db = new GatekeeperrrrEntities();
            var data = (from z in db.tbl_StaffUser orderby z.Id descending select z).FirstOrDefault();
            if (data != null)
            {
                Id = data.Id;
                Id = Id + 1;
            }
            else
            {
                Id = Id + 1;
            }
            var staffUserId = Id.ToString().Length == 1 ? "STAFF0" + Id + "USER" : "STAFF" + Id + "USER";
            ViewBag.staffId = staffUserId;


            return View();
        }

        public ActionResult UpdateStaffUser()
        {
            return View();
        }

        [SessionTimeout]
        public ActionResult UpdateVisitorManager()
        {

            return View();
        }

        [SessionTimeout]
        public ActionResult VisitorManager()
        {
            int Id = 0;
            GatekeeperrrrEntities db = new GatekeeperrrrEntities();
            var data = (from z in db.tbl_Gatekeeper orderby z.Id descending select z).FirstOrDefault();
            if (data != null)
            {
                Id = data.Id;
                Id = Id + 1;
            }
            else
            {
                Id = Id + 1;
            }
            var gatekeeperId = Id.ToString().Length == 1 ? "GATE0" + Id + "KEEPER" : "GATE" + Id + "KEEPER";
            ViewBag.gateKeeperId = gatekeeperId;
            return View();
        }

        [HttpPost]
        [SessionTimeout]
        public PartialViewResult GetGatekeeperTablePartial()
        {
            List<GateKeeperDetails> guestResponse = new List<GateKeeperDetails>();
            int adminId = 0;
            if (Session["AdminId"] != null)
            {
                adminId = Convert.ToInt32(Session["AdminId"].ToString());
            }
            guestResponse = DBHelper.GetGateKeeperTableDetails(adminId);
            return PartialView("_GatekeeperPartial", guestResponse);
        }

        [HttpPost]
        [SessionTimeout]
        public PartialViewResult GetUserTablePartial()
        {
            int adminId = 0;
            if (Session["AdminId"] != null)
            {
                adminId = Convert.ToInt32(Session["AdminId"].ToString());
            }
            List<User> guestResponse = new List<User>(adminId);
            guestResponse = DBHelper.GetUserTableDetails();
            return PartialView("_UserTablePartial", guestResponse);
        }

        [SessionTimeout]
        public ActionResult Visitor()
        {
            if (Session["AdminId"] == null && Session["AdminEmail"] == null)
            {
                RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost]
        public PartialViewResult GetVisitorPartial()
        {
            List<Visitor> guestResponse = new List<Visitor>();
            guestResponse = DBHelper.GetVisitorPartialDetails();
            return PartialView("_VisitorPartial", guestResponse);
        }

        [HttpPost]
        public PartialViewResult GetVisitorPartialByFilter(string name, string date, string vehNo)
        {
            List<Visitor> guestResponse = new List<Visitor>();
            string adminId = string.Empty;
            guestResponse = DBHelper.GetVisitorPartialDetailsByFilter(name, date, vehNo,adminId);
            return PartialView("_VisitorPartial", guestResponse);
        }

        public ActionResult AdminUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logout()
        {
            Session.Abandon();
            var result = "Success";
            //return RedirectToAction("Login","Home");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerateChecksum()
        {
            var url = CommonUtil.GetBaseUrl();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("MID", "TECHNO10925072008750");
            parameters.Add("CHANNEL_ID", "WEB");
            parameters.Add("INDUSTRY_TYPE_ID", "Retail");
            parameters.Add("WEBSITE", "WEB_STAGING");
            //parameters.Add("EMAIL", "email value");
            //parameters.Add("MOBILE_NO", "mobile value");
            parameters.Add("CUST_ID", "1234");
            parameters.Add("ORDER_ID", "abdr333gate");
            parameters.Add("TXN_AMOUNT", "10");
            parameters.Add("CALLBACK_URL", url + "home/VerifyChecksum");
            //This parameter is not mandatory. Use this to pass the callback url dynamically.

            string checksum = paytm.CheckSum.generateCheckSum("Kr!7q@GB85aCiSSm", parameters);

            string paytmURL = "https://securegw-stage.paytm.in/theia/processTransaction?orderid=abdr333gate";  // + orderid  //"https://pguat.paytm.com/oltp-web/processTransaction";

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>Please do not refresh this page...</h1></center>";
            outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            foreach (string key in parameters.Keys)
            {
                outputHTML += "<input type='hidden' name='" + key + "' value='" + parameters[key] + "'>";
            }
            outputHTML += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            Response.Write(outputHTML);
            return new EmptyResult();
        }

        public ActionResult VerifyChecksum()
        {
            String merchantKey = "Kr!7q@GB85aCiSSm"; // Replace the with the Merchant Key provided by Paytm at the time of registration.

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string paytmChecksum = "";
            foreach (string key in Request.Form.Keys)
            {
                parameters.Add(key.Trim(), Request.Form[key].Trim());
            }

            if (parameters.ContainsKey("CHECKSUMHASH"))
            {
                paytmChecksum = parameters["CHECKSUMHASH"];
                parameters.Remove("CHECKSUMHASH");
            }

            if (CheckSum.verifyCheckSum(merchantKey, parameters, paytmChecksum))
            {
                Response.Write("Checksum Matched");
            }
            else
            {
                Response.Write("Checksum MisMatch");
            }
            return View();
        }

        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult PaymentWithCreditCard()
        {
            //create and item for which you are taking payment
            //if you need to add more items in the list
            //Then you will need to create multiple item objects or use some loop to instantiate object
            Item item = new Item();
            item.name = "Demo Item";
            item.currency = "USD";
            item.price = "5";
            item.quantity = "1";
            item.sku = "sku";

            //Now make a List of Item and add the above item to it
            //you can create as many items as you want and add to this list
            List<Item> itms = new List<Item>();
            itms.Add(item);
            ItemList itemList = new ItemList();
            itemList.items = itms;

            //Address for the payment
            Address billingAddress = new Address();
            billingAddress.city = "NewYork";
            billingAddress.country_code = "US";
            billingAddress.line1 = "23rd street kew gardens";
            billingAddress.postal_code = "43210";
            billingAddress.state = "NY";


            //Now Create an object of credit card and add above details to it
            //Please replace your credit card details over here which you got from paypal
            CreditCard crdtCard = new CreditCard();
            crdtCard.billing_address = billingAddress;
            crdtCard.cvv2 = "946";  //card cvv2 number
            crdtCard.expire_month = 11; //card expire date
            crdtCard.expire_year = 2023; //card expire year
            crdtCard.first_name = "Aman";
            crdtCard.last_name = "Thakur";
            crdtCard.number = "4855562717277617"; //enter your credit card number here
            crdtCard.type = "visa"; //credit card type here paypal allows 4 types

            // Specify details of your payment amount.
            Details details = new Details();
            details.shipping = "1";
            details.subtotal = "5";
            details.tax = "1";

            // Specify your total payment amount and assign the details object
            Amount amnt = new Amount();
            amnt.currency = "USD";
            // Total = shipping tax + subtotal.
            amnt.total = "7";
            amnt.details = details;

            // Now make a transaction object and assign the Amount object
            Transaction tran = new Transaction();
            tran.amount = amnt;
            tran.description = "Description about the payment amount.";
            tran.item_list = itemList;
            tran.invoice_number = "your invoice number which you are generating";

            // Now, we have to make a list of transaction and add the transactions object
            // to this list. You can create one or more object as per your requirements

            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(tran);

            // Now we need to specify the FundingInstrument of the Payer
            // for credit card payments, set the CreditCard which we made above

            FundingInstrument fundInstrument = new FundingInstrument();
            fundInstrument.credit_card = crdtCard;

            // The Payment creation API requires a list of FundingIntrument

            List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
            fundingInstrumentList.Add(fundInstrument);

            // Now create Payer object and assign the fundinginstrument list to the object
            Payer payr = new Payer();
            payr.funding_instruments = fundingInstrumentList;
            payr.payment_method = "credit_card";

            // finally create the payment object and assign the payer object & transaction list to it
            Payment pymnt = new Payment();
            pymnt.intent = "sale";
            pymnt.payer = payr;
            pymnt.transactions = transactions;

            try
            {
                //getting context from the paypal
                //basically we are sending the clientID and clientSecret key in this function
                //to the get the context from the paypal API to make the payment
                //for which we have created the object above.

                //Basically, apiContext object has a accesstoken which is sent by the paypal
                //to authenticate the payment to facilitator account.
                //An access token could be an alphanumeric string

                APIContext apiContext = Configuration.GetAPIContext();

                //Create is a Payment class function which actually sends the payment details
                //to the paypal API for the payment. The function is passed with the ApiContext
                //which we received above.

                Payment createdPayment = pymnt.Create(apiContext);

                //if the createdPayment.state is "approved" it means the payment was successful else not

                if (createdPayment.state.ToLower() != "approved")
                {
                    return View("FailureView");
                }
            }
            catch (PayPal.PayPalException ex)
            {
                //Logger.Log("Error: " + ex.Message);
                return View("FailureView" + ex.Message);
            }

            return View("SuccessView");
        }

        public ActionResult PaymentExecutePaypal()
        {
            var result = string.Empty;
            var amount = string.Empty;
            var guidData = string.Empty;
            string email = string.Empty;
            string userName = string.Empty;
            try
            {
                APIContext apiContext = Configuration.GetAPIContext();

                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    return RedirectToAction("PaymentWithPaypal", "Home");
                }

                // This section is executed when we have received all the payments parameters

                // from the previous call to the function Create

                // Executing a payment

                var guid = Request.Params["paymentId"];

                var executedPayment = ExecutePayment(apiContext, payerId, guid as string);

                if (executedPayment.state.ToLower() != "approved")
                {
                    return View("FailureView");
                }
                else
                {
                    amount = executedPayment.transactions[0].amount.total;
                    guidData = Request.Params["guid"].ToString();
                    if (Session["AdminAppId"] != null)
                    {
                        var adminAppId = Session["AdminAppId"].ToString();
                        AdminDetails data = new AdminDetails();
                        data = DBHelper.UpdateAdminStatus(adminAppId);

                        PaymentDetails payment = new PaymentDetails();
                        payment.AdminId = data.Id;
                        payment.AdminAppId = data.AdminId;
                        payment.PaymentId = Request.Params["paymentId"];
                        payment.Guid = guidData;
                        payment.Token = Request.Params["token"];
                        payment.PayerID = payerId;
                        payment.Amount = amount.ToString();

                        var paymentResult = DBHelper.AddPaymentDetails(payment);
                        result = "Success";

                        userName = data.Username;
                        email = data.Email;
                        string subject = "Payment details for Gatekeeper app";
                        StringBuilder message = new StringBuilder();
                        message.Append("<html>");
                        message.Append("<body>");
                        message.Append("<div>Congratulations</div><br />");
                        message.Append("<div>Your payment has been processed successfully. Please find the order Details below</div><br />");
                        message.Append("<div>Order ID - '" + guidData + "'</div><br />");
                        message.Append("<div>Amount - '" + amount.ToString() + "'</div><br />");
                        message.Append("<div>Username - '" + userName + "'</div><br />");
                        message.Append("<div>Date of Purchase - '" + DateTime.Now.ToString("dd-MM-yyyy") + "'</div><br />");
                        message.Append("<div>Thank you so much for being a part of Gatekeeperr application.</div><br />");
                        message.Append("<div><span>| </span><a href='#'>Print Receipt</a><span> | </span><a href='#'>Resend Details on Mail</a><span> | </span></div><br />");
                        message.Append("</body>");
                        message.Append("</html>");
                        CommonUtil.SentEmail(email, message.ToString(), subject);
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.log("Error" + ex.Message);
                //return View("FailureView" + ex.Message);
                return View("Login", "Home", new { status = "Failure" });
            }

            return RedirectToAction("Login", "Home", new { status = result, orderid = guidData, amount = amount, email = email, username = userName, date = DateTime.Now.ToString("dd-MM-yyyy") });
        }

        public ActionResult PaymentWithPaypal()
        {
            //getting the apiContext as earlier
            APIContext apiContext = Configuration.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    // So we have provided URL of this controller only
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                                "/Home/PaymentExecutePaypal?";

                    //guid we are generating for storing the paymentID received in session
                    //after calling the create function and it is used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                //Logger.log("Error" + ex.Message);
                return View("FailureView" + ex.Message);
                //return RedirectToAction("Login", "Home");
            }

            return RedirectToAction("Login", "Home");
        }

        private PayPal.Api.Payment payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        public Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            // Get Package Details
            GatekeeperrrrEntities db = new GatekeeperrrrEntities();
            string packageName = string.Empty;
            string packagePrice = string.Empty;
            string packageDescription = string.Empty;
            int pakgeId = 0;
            if (Session["PackageId"] != null)
            {
                pakgeId = Convert.ToInt32(Session["PackageId"].ToString());
            }
            var package = (from z in db.tbl_Package where z.Id == pakgeId select z).FirstOrDefault();
            if (package != null)
            {
                packageName = package.PackageName;
                packagePrice = package.AnnuallyPrice.ToString();
                packageDescription = package.Description;
            }

            //similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };

            itemList.items.Add(new Item()
            {
                name = packageName,
                currency = "USD",
                price = packagePrice,
                quantity = "1",
                sku = packageDescription
            });

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = packagePrice
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = packagePrice, // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
                invoice_number = "1234",
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);

        }

        public ActionResult Payment()
        {
            return View();
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(string email)
        {
            try
            {
                var result = "";
                string data = DBHelper.ForgetPassword(email);
                if (data == "Success")
                    result = "Success";
                else
                    result = data;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
