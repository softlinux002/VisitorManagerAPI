using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace VisitorManager.BusinessLogic
{
    public class CommonUtil
    {
        public static int GetRandomNumber(int min, int max)
        {
            Random getrandom = new Random();
            lock (getrandom) // synchronize
            {
                return getrandom.Next(min, max);
            }
        }

        public static string ImageSave(string attachmentURL)
        {
            string base64 = attachmentURL.Substring(attachmentURL.IndexOf(',') + 1);
            base64 = base64.Trim('\0');
            byte[] img = Convert.FromBase64String(base64);
            var imageConverter = new ImageConverter();
            var image = (System.Drawing.Image)imageConverter.ConvertFrom(img);
            Bitmap bmp = new Bitmap(image);
            string path = "../ComplaintImage/";
            Guid imageName = Guid.NewGuid();
            var newFileName = imageName + ".jpg";
            var path1 = HttpContext.Current.Server.MapPath(path + newFileName);

            bmp.Save(path1, ImageFormat.Jpeg);
            return "ComplaintImage/" + newFileName;
        }

        public static string APIImageSave(byte[] base64)
        {
            var imageConverter = new ImageConverter();
            var image = (System.Drawing.Image)imageConverter.ConvertFrom(base64);
            Bitmap bmp = new Bitmap(image);

            string path = "../../ComplaintImage/";
            Guid imageName = Guid.NewGuid();
            var newFileName = imageName + ".jpg";
            var path1 = HttpContext.Current.Server.MapPath(path + newFileName);

            bmp.Save(path1, ImageFormat.Jpeg);
            return "ComplaintImage/" + newFileName;
        }

        public static string GetBaseUrl()
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/")
                appUrl = "/" + appUrl;

            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            return baseUrl;
        }

        public static void SentEmail(string email, string message, string subject)
        {
            try
            {
                MailMessage mm = new MailMessage();
                mm.To.Add(email);
                mm.From = new MailAddress("noreply@gatekeeperr.com");
                mm.Subject = subject;
                mm.Body = message;
                mm.IsBodyHtml = true;
                mm.Priority = MailPriority.High;
                SmtpClient smtp = new SmtpClient();
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Host = "smtp.zoho.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("noreply@gatekeeperr.com", "Techno@123");
                smtp.EnableSsl = true;
                smtp.Send(mm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}