using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Umabrco_mandatory.ViewModels;
using Umbraco.Web.Mvc;
using Umbraco.Core.Models;

namespace Umabrco_mandatory.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: ContactFormSurface
        public ActionResult Index() {
            return PartialView("ContactForm", new ContactForm());
        }
    [HttpPost]
    public ActionResult HandleFormSubmit(ContactForm model)
        {
            if (!ModelState.IsValid) {
                return CurrentUmbracoPage();
            }
            TempData["succes"] = true;

            MailMessage message = new MailMessage();
            message.To.Add("Mrazova.Albina@gmail.com");
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message;

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("Mrazova.Albina@gmail.com", "xbemewaunbzfcmgn");
                // send mail
                smtp.Send(message);

                IContent msg = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "message");
                msg.SetValue("messageName", model.Name);
                msg.SetValue("email", model.Email);
                msg.SetValue("subject", model.Subject);
                msg.SetValue("messageContent", model.Message);

                Services.ContentService.Save(msg);

            }
            return RedirectToCurrentUmbracoPage();
        }
       
    }
}