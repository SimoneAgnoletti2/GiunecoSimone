using GiunecoSimone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GiunecoSimone.Class;
using System.Web.DynamicData;
using System.Web.Services.Description;

namespace GiunecoSimone.Controllers
{
    public class AccountController : Controller
    {
        //no cache
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Exclude = "IsEmailVerified,ActivationCode")] Users user)
        {
            var Status = false;
            var Message = string.Empty;

            if (ModelState.IsValid)
            {
                #region Email già presente
                var isExist = IsEmailExist(user.EmailID);
                if (isExist)
                {
                    ModelState.AddModelError("EmailEsistente", "Email già usata!");
                    return View(user);
                }
                #endregion

                #region Activation Code
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password Hashing
                user.Pwd = Crypto.Hash(user.Pwd);
                user.ConfirmPwd = Crypto.Hash(user.ConfirmPwd);
                #endregion

                user.IsEmailVerified = false;

                #region Salva su db locale
                using (giuneco_Entities ue = new giuneco_Entities())
                {
                    ue.Users.Add(user);
                    ue.SaveChanges();
                }
                #endregion

                #region Invio Mail Verifica
                SendEmail(user.EmailID, user.ActivationCode.ToString());
                Message = "Registrazione completata. Il link per attivare il tuo account" +
                    $" è stato inviato alla tue casella postale: {user.EmailID}";

                Status = true;
                #endregion
            }
            else
            {
                Message = "Invalid Request";
            }

            ViewBag.Status = Status;
            ViewBag.Message = Message;

            return View(user);
        }


        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            try
            {

                var status = false;

                using (giuneco_Entities eu = new giuneco_Entities())
                {
                    eu.Configuration.ValidateOnSaveEnabled = false;
                    var compare = eu.Users.Where(x => x.ActivationCode.Equals(new Guid(id))).FirstOrDefault();
                    if (compare != null)
                    {
                        compare.IsEmailVerified = true;
                        eu.SaveChanges();
                        status = true;
                    }
                    else
                    {
                        ViewBag.Message = "Invalid Request";
                    }
                }
                ViewBag.Status = status;
                return View();
            }
            catch{ return View(); }
        }

        //no cache
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult LogIn()
        {
            return View();
        }

        //no cache
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(UserLogin userLogin)
        {
            var message = string.Empty;
            using (giuneco_Entities ue = new giuneco_Entities())
            {
                var compare = ue.Users.Where(x => x.EmailID.Equals(userLogin.EmailID)).FirstOrDefault();
                if (compare != null)
                {
                    if (string.Compare(Crypto.Hash(userLogin.Pwd), compare.Pwd) == 0)
                    {
                        if (!compare.IsEmailVerified)
                        {
                            message = "Devi prima attivare il tuo account!";
                        }
                        else
                        {
                            FormsAuthentication.SetAuthCookie(compare.FirstName + "\t" + compare.LastName, false);
                            Session["UserID"] = compare.Id;
                            return RedirectToAction("Home", "Home");
                        }
                    }
                    else
                    {
                        message = "Credenziali errate";
                    }
                }
                else
                {
                    message = "Credenziali errate";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("LogIn", "Account");
        }

        #region Password dimenticata
        public ActionResult ForgotPwd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPwd(string emailID)
        {
            var message = string.Empty;

            using (giuneco_Entities ue = new giuneco_Entities())
            {
                var account = ue.Users.Where(x => x.EmailID.Equals(emailID)).FirstOrDefault();
                if(account != null)
                {
                    var resetCode = Guid.NewGuid().ToString();
                    SendEmail(account.EmailID, resetCode, "ResetPwd");
                    account.ResetPwdCode = resetCode;
                    ue.Configuration.ValidateOnSaveEnabled = false;
                    ue.SaveChanges();
                    message = "Una email è stata inviata alla tua casella postale";
                }
                else
                {
                    message = "Account non trovato";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        public ActionResult ResetPwd(string id)
        {
            using (giuneco_Entities ue = new giuneco_Entities())
            {
                var user = ue.Users.Where(x => x.ResetPwdCode.Equals(id)).FirstOrDefault();
                if(user != null)
                {
                    ResetPwd rp = new ResetPwd();
                    rp.ResetCode = id;
                    return View();
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPwd(ResetPwd rp)
        {
            try
            {
                var id = Request.Url.ToString().Split(new string[] { "ResetPwd/" }, StringSplitOptions.None)[1];
                rp.ResetCode = id;
                var message = string.Empty;
                var status = false;
                using (giuneco_Entities ue = new giuneco_Entities())
                {
                    var user = ue.Users.Where(x => x.ResetPwdCode == rp.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Pwd = Crypto.Hash(rp.NewPassword);
                        user.ResetPwdCode = string.Empty;
                        ue.Configuration.ValidateOnSaveEnabled = false;
                        ue.SaveChanges();
                        message = "Password aggiornata con successo!";
                        status = true;
                    }
                }
                ViewBag.Message = message;
                ViewBag.Status = status;
                return View(rp);
            }
            catch 
            {
                ViewBag.Message = "Qualcosa è andato storto";
                ViewBag.Status = false;
                return View();
            }
        }
        #endregion

        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (giuneco_Entities ue = new giuneco_Entities())
            {
                var verifica = ue.Users.Where(x => x.EmailID.Equals(emailID)).FirstOrDefault();
                return verifica == null ? false : true;
            }
        }

        [NonAction]
        public void SendEmail(string emailID, string activationcode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/Account/" + emailFor + "/" + activationcode;

            var url = Request.Url.AbsoluteUri.Split(new string[] { "/Account" }, StringSplitOptions.None)[0];

            var link = url + verifyUrl;



            var email = new MailAddress("agnolettisimone@gmail.com", "Simone Agnoletti");
            var toEmail = new MailAddress(emailID);
            var fromEmailPWD = "sghhdornzoktmqjm";

            var subject = string.Empty;
            var body = string.Empty;

            if(emailFor.Equals("VerifyAccount"))
            {
                subject = "Il tuo account è stato creato con successo!";

                body = "<br><br>Sono lieto di annunciarti che il tuo account è stato" +
                " creato con successo. Clicca su questo link per attivare il tuo account!" +
                "<br><br><a href= '" + link + "'>" + link + "</a>";
            }
            else
            {
                subject = "Password reset";

                body = "<br><br> Perfavore clicca sul link per resettare la tua password " +
                    "<br><br><a href=" + link + ">Passord Reset</a>";

            }

            var smtp = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(email.Address, fromEmailPWD)
            };

            using (var message = new MailMessage(email.Address, toEmail.Address)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })

            smtp.Send(message);
        }

    }
}