using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using AIEnterprise.Feature.Forms.Models;
using Feature.FormsExtensions.Fields.FileUpload;

namespace AIEnterprise.Feature.Forms.Helper
{
    public class EmailHelper
    {
        public string GetEmailBody(Dictionary<string, List<string>> formData, string EmailTemplate)
        {
            string emailHtml = string.Empty;
            try
            {
                var htmlEmailTemplate = GetEmailTemplate(EmailTemplate);

                if (htmlEmailTemplate == null)
                {
                    Sitecore.Diagnostics.Log.Info("Email Template is null", "");
                    return null;
                }

                htmlEmailTemplate = htmlEmailTemplate.Replace("#Name#", GetValuefromDictionary(formData, "your-name"));
                htmlEmailTemplate = htmlEmailTemplate.Replace("#Email#", GetValuefromDictionary(formData, "your-email"));
                htmlEmailTemplate = htmlEmailTemplate.Replace("#Message#", GetValuefromDictionary(formData, "your-message"));
                htmlEmailTemplate = htmlEmailTemplate.Replace("#Phone#", GetValuefromDictionary(formData, "phone"));
                htmlEmailTemplate = htmlEmailTemplate.Replace("#JobApplied#", GetValuefromDictionary(formData, "JobApplied"));
                htmlEmailTemplate = htmlEmailTemplate.Replace("#Organization#", GetValuefromDictionary(formData, "Organization"));
                htmlEmailTemplate = htmlEmailTemplate.Replace("#Evaluation#", GetValuefromDictionary(formData, "Evaluation"));

                //emailHtml = htmlEmailTemplate.ReplacePatternCaseInsensitive(replacements);
                emailHtml = htmlEmailTemplate;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Info($"Email failed to send: ", ex.Message);
            }
            return emailHtml;
        }

        public string RootPath => HttpContext.Current.Request.PhysicalApplicationPath;

        public string GetEmailTemplate(string template)
        {
            try
            {
                var path = string.Format("{0}EmailTemplate\\{1}.html", RootPath, template);
                var html = File.ReadAllText(path);
                return html;
            }
            catch
            {
                return null;
            }
        }

        public bool Send(Email email, FileUploadModel fileUpload)
        {
            var recipients = GetRecipients(email);
            var sitecoreEmail = new MailMessage(email.From, recipients.First(), email.Subject, email.Body);

            if (email.IsAttachement && fileUpload != null && fileUpload.File.ContentLength > 0)
            {
                Attachment attachment = new Attachment(fileUpload.File.InputStream, fileUpload.File.FileName);
                sitecoreEmail.Attachments.Add(attachment);
            }

            sitecoreEmail.IsBodyHtml = true;
            try
            {
                using (var client = CreateSmtpClient())
                {
                    Sitecore.Diagnostics.Log.Info("recipients.First()" + recipients.First(), "");
                    Sitecore.Diagnostics.Log.Info("email.Subject" + email.Subject, "");
                    Sitecore.Diagnostics.Log.Info("Email From address" + email.From, "");
                    if (client == null)
                        Sitecore.Diagnostics.Log.Info("SMTP Client is NULL", "");
                    client.Send(sitecoreEmail);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("SmtpClient not created:" + email.From + "--" + ex.StackTrace, "");
                return false;
            }
            return true;
        }

        private string[] GetRecipients(Email email)
        {
            return email.To.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
        }

        private static SmtpClient CreateSmtpClient()
        {
            string mailServer = Settings.MailServer;
            Sitecore.Diagnostics.Log.Info("SmtpClient ServerName:" + mailServer, "");
            SmtpClient smtpClient;
            if (string.IsNullOrEmpty(mailServer))
            {
                smtpClient = new SmtpClient();
            }
            else
            {
                int mailServerPort = Settings.MailServerPort;
                smtpClient = mailServerPort <= 0 ? new SmtpClient(mailServer) : new SmtpClient(mailServer, mailServerPort);
            }
            string mailServerUserName = Settings.MailServerUserName;
            bool enableSsl;
            smtpClient.EnableSsl = bool.TryParse(Sitecore.Configuration.Settings.GetSetting("Mail.MailServerEnableSsl"), out enableSsl) &&
                enableSsl;
            if (mailServerUserName.Length > 0)
            {
                string mailServerPassword = Settings.MailServerPassword;
                NetworkCredential networkCredential = new NetworkCredential(mailServerUserName, mailServerPassword);
                smtpClient.Credentials = networkCredential;
            }
            return smtpClient;
        }

        public string GetValuefromDictionary(Dictionary<string, List<string>> formData, string FieldName)
        {
            string fieldValue = string.Empty;
            if (formData.ContainsKey(FieldName))
            {
                List<string> fieldValues = new List<string>();
                if (formData.TryGetValue(FieldName, out fieldValues))
                    fieldValue = fieldValues[0];
            }
            return fieldValue;
        }
    }
}
