using System;
using System.Collections.Generic;
using System.Web;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using Newtonsoft.Json;
using RestSharp;
using Sitecore.Links;
using Sitecore.Configuration;
using Sitecore.ExperienceForms.Processing.Actions.Models;
using AIEnterprise.Feature.Forms.DataDictionary;
using AIEnterprise.Feature.Forms.Helper;
using AIEnterprise.Feature.Forms.Models;
//using ServiceMaster.SVMBrands.Feature.SVMForms.Models;
//using ServiceMaster.SVMBrands.Feature.SVMForms.Helper;
//using ServiceMaster.SVMBrands.Foundation.Common;
//using Sitecore.Data.Items;
//using ServiceMaster.SVMBrands.Foundation.WildCards.Models;

namespace AIEnterprise.Feature.Forms.Actions
{
    public class SubmitAIEAction : SubmitActionBase<RedirectActionData>
    {
        Dictionary<string,
        List<string>> hstable = new Dictionary<string,
        List<string>>();


        string toEmailID = Sitecore.Context.Site.SiteInfo.Properties.Get("AIEToEmail");
        string subjectEmail = Sitecore.Context.Site.SiteInfo.Properties.Get("AIEEmailSubject");
        string isOAFMailEnabled = Sitecore.Context.Site.SiteInfo.Properties.Get("AIEEmailEnabled");
        

        public SubmitAIEAction(ISubmitActionData submitActionData) : base(submitActionData)
        {

        }

        protected override bool TryParse(string value, out RedirectActionData target)
        {
            target = new RedirectActionData { ReferenceId = new Guid() };
            return true;
        }

        string currentPage = "/";
        public override void ExecuteAction(FormSubmitContext formSubmitContext, string parameters)
        {
            try
            {
                bool excutethis = this.Execute("", formSubmitContext);
                var thankyou = currentPage + "?status=success";
                Sitecore.Diagnostics.Log.Info("thank you page url" + thankyou, "");
                var defaultUrlOptions = LinkManager.GetDefaultUrlOptions();
                defaultUrlOptions.SiteResolving = Settings.Rendering.SiteResolving;
                formSubmitContext.RedirectUrl = thankyou;
                formSubmitContext.RedirectOnSuccess = true;
                formSubmitContext.Abort();
            }
            catch (Exception e)
            {
                Log.Error("Error in Form Submission" + e.Message, "");
                Log.Error("Error in Form Submission2" + e.StackTrace, "");
                Log.Error("Error in Form Submission1" + e.InnerException.ToString(), "");
            }
        }




        public bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(data, nameof(data));
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));

            //Get all Data Posted from FORM
            var formDict = new FormDictionary();
            var hstable = formDict.GetFieldsDictionary(formSubmitContext.Fields);
            bool isMailSent = SendEmailNotification(hstable);
            return true;
        }

        private bool SendEmailNotification(Dictionary<string, List<string>> formdata)
        {
            try
            {
                //Send the form details via emails
                EmailHelper emailHeler = new EmailHelper();
                string emailFrom = emailHeler.GetValuefromDictionary(formdata, "your-email");
                string subject = subjectEmail;
                var allEmails = toEmailID.Split(';');
                var isMailSent = false;
                var emailBody = emailHeler.GetEmailBody(formdata);

                foreach (var email in allEmails)
                {
                    var aieEmail = new Email
                    {
                        To = email,
                        Subject = subject,
                        From = emailFrom,
                        Body = emailBody
                    };

                    isMailSent = emailHeler.Send(aieEmail);
                    if (!isMailSent)
                    {
                        Log.Error("$Email sender failed", "FormSubmission");
                    }
                }
                return isMailSent;

            }
            catch (Exception e)
            {
                Log.Error("Error in Form sending Form Data via mail : " + e.StackTrace + e.Message, "");
                return false;
            }
        }

        private string GetFieldById(object firstNameFieldId, IList<IViewModel> fields)
        {
            throw new NotImplementedException();
        }

        protected override bool Execute(RedirectActionData data, FormSubmitContext formSubmitContext)
        {
            throw new NotImplementedException();
        }
    }
}