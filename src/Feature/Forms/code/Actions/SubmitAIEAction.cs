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
using Feature.FormsExtensions.Fields.FileUpload;


namespace AIEnterprise.Feature.Forms.Actions
{
    public class SubmitAIEAction : SubmitActionBase<RedirectActionData>
    {
        Dictionary<string, List<string>> hstable = new Dictionary<string, List<string>>();

        bool isJobApplicationForm;
        bool isBookAMeeting = false;
        bool isLookingBrochure = false;
        string EmailTemplate = "AIE_ContactEmail";
        FileUploadModel fileUpload = null;

        #region Contact Form Variables
        string contactUsRecipientsIDs = Sitecore.Context.Site.SiteInfo.Properties.Get("ContactUsRecipientsIDs");
        string contactUsEmailsubject = Sitecore.Context.Site.SiteInfo.Properties.Get("ContactUsEmailsubject");
        #endregion

        #region Profile Form Variables
        string careersRecipientsIDs = Sitecore.Context.Site.SiteInfo.Properties.Get("CareersRecipientsIDs");
        string careersEmailsubject = Sitecore.Context.Site.SiteInfo.Properties.Get("CareersEmailsubject");
        #endregion

        #region Book a Meeting Form Variables
        string BookaMeetingRecipientsIDs = Sitecore.Context.Site.SiteInfo.Properties.Get("BookaMeetingRecipientsIDs");
        string BookaMeetingsubject = Sitecore.Context.Site.SiteInfo.Properties.Get("BookaMeetingsubject");
        #endregion

        #region Brochure Form Variables
        string LookingBrochureRecipientsIDs = Sitecore.Context.Site.SiteInfo.Properties.Get("LookingBrochureRecipientsIDs");
        string LookingBrochuresubject = Sitecore.Context.Site.SiteInfo.Properties.Get("LookingBrochuresubject");
        #endregion

        string SenderEmail = Sitecore.Context.Site.SiteInfo.Properties.Get("SenderEmail");

        public SubmitAIEAction(ISubmitActionData submitActionData) : base(submitActionData)
        {

        }

        protected override bool TryParse(string value, out RedirectActionData target)
        {
            target = new RedirectActionData { ReferenceId = new Guid() };
            return true;
        }

        //string currentPage = "/";
        public override void ExecuteAction(FormSubmitContext formSubmitContext, string parameters)
        {
            try
            {
                Log.Error("Error in Form Submission", "");

                string currentPage = "/";
                currentPage = HttpContext.Current.Request.Headers["Referer"];
                if (string.IsNullOrEmpty(currentPage) || currentPage == "/")
                {
                    Uri referrer = HttpContext.Current.Request.UrlReferrer;
                    currentPage = referrer.ToString();
                }
                if (currentPage.Contains("?"))
                {
                    currentPage = currentPage.Split('?')[0];
                }

                if (this.Execute("", formSubmitContext))
                {
                    var thankyou = currentPage + "?status=success";
                    var defaultUrlOptions = LinkManager.GetDefaultUrlOptions();
                    defaultUrlOptions.SiteResolving = Settings.Rendering.SiteResolving;
                    formSubmitContext.RedirectUrl = thankyou;
                    formSubmitContext.RedirectOnSuccess = true;
                    formSubmitContext.Abort();
                }
                else
                {
                    var errorpage = currentPage + "?status=failed";
                    var defaultUrlOptions = LinkManager.GetDefaultUrlOptions();
                    defaultUrlOptions.SiteResolving = Settings.Rendering.SiteResolving;
                    formSubmitContext.RedirectUrl = errorpage;
                    formSubmitContext.RedirectOnSuccess = true;
                    formSubmitContext.Abort();
                }
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
            if (formSubmitContext.Fields.Count > 4 && formSubmitContext.Fields[4] != null && formSubmitContext.Fields[4] is FileUploadModel)
            {
                fileUpload = formSubmitContext.Fields[4] as FileUploadModel;
            }
            //Get all Data Posted from FORM
            var formDict = new FormDictionary();

            hstable = formDict.GetFieldsDictionary(formSubmitContext.Fields);
            Dictionary<string, string> attributeList = new Dictionary<string, string>();

            #region Profile Form
            if (formSubmitContext.FormId != null && formSubmitContext.FormId.Equals(new Guid("{3C63E239-4DE8-4615-8C1F-B1C35A2167D8}")))
            {
                isJobApplicationForm = true;
                EmailTemplate = "AIE_CareersEmail";
            }
            #endregion

            #region Book a Meeting Form
            else if (formSubmitContext.FormId != null && formSubmitContext.FormId.Equals(new Guid("{12222371-463a-4679-b9e2-c15187a8116a}")))

            {
                isBookAMeeting = true;
                EmailTemplate = "AIE_BookAMeeting";
            }
            #endregion

            #region Looking For Brochure Form
            else if (formSubmitContext.FormId != null && formSubmitContext.FormId.Equals(new Guid("{9EDEE103-DE3C-45F7-970D-FC1D13D5D7D4}")))
            {
                isLookingBrochure = true;
                EmailTemplate = "AIE_LookingBrochure";
            }
            #endregion

            #region Contact Corporate Form
            else
            {
                EmailTemplate = "AIE_ContactEmail";
            } 
            #endregion

            bool isMailSent = SendEmailNotification(hstable);

            return isMailSent;
        }

        private bool SendEmailNotification(Dictionary<string, List<string>> formdata)
        {
            try
            {
                //Send the form details via emails
                EmailHelper emailHelper = new EmailHelper();
                string emailFrom = SenderEmail;
                string subject = string.Empty;
                string[] allEmails;
                var isMailSent = false;
                var emailBody = emailHelper.GetEmailBody(formdata, EmailTemplate);

                if (isJobApplicationForm)
                {
                    subject = careersEmailsubject + " " + emailHelper.GetValuefromDictionary(formdata, "JobApplied");
                    allEmails = careersRecipientsIDs.Split(';');
                }
                else if (isBookAMeeting)
                {
                    subject = BookaMeetingsubject;
                    allEmails = BookaMeetingRecipientsIDs.Split(';');
                }

                else if (isLookingBrochure)
                {
                    subject = LookingBrochuresubject;
                    allEmails = LookingBrochureRecipientsIDs.Split(';');
                }

                else
                {
                    subject = contactUsEmailsubject;
                    allEmails = contactUsRecipientsIDs.Split(';');
                }


                foreach (string email in allEmails)
                {
                    var aieEmail = new Email
                    {
                        To = email,
                        Subject = subject,
                        From = emailFrom,
                        Body = emailBody,
                        IsAttachement = isJobApplicationForm
                    };

                    isMailSent = emailHelper.Send(aieEmail, fileUpload);
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