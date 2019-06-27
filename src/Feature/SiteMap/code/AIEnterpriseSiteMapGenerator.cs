using AIEnterprise.Feature.SiteMap.Models;
using AIEnterprise.Foundation.Common.Utilities.SitecoreExtensions;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace AIEnterprise.Feature.SiteMap
{
    public class AIEnterpriseSiteMapGenerator : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            //This check will verify if the physical path of the request exists or not.
            if (!System.IO.File.Exists(args.HttpContext.Request.PhysicalPath) &&
                !System.IO.Directory.Exists(args.HttpContext.Request.PhysicalPath))
            {
                Assert.ArgumentNotNull(args, "args");
                //Check if the request is of sitemap.xml then only allow the request to serve sitemap.xml

                if (args.Url == null || !args.Url.FilePath.ToLower().EndsWith("sitemap.xml")) return;


                if (args.Url.FilePath.ToLower().Equals("/sitemap.xml"))
                {
                    //Write XML Response for Sitemap.
                    var response = HttpContext.Current.Response;
                    try
                    {
                        // Homepage of the Website.
                        // Start path will give homepage including Multisite.
                        var homepage = Sitecore.Context.Database.GetItem(args.StartPath);
                        var ser = new XmlSerializer(typeof(Urlset));
                        var urlSet = new Urlset();

                        //Create node of Homepage in Sitemap.
                        var tmpurlset = new List<Url>();
                        var config = AppendLanguage();
                        tmpurlset.Add(new Url
                        {
                            Loc = GetAbsoluteLink(LinkManager.GetItemUrl(homepage, new UrlOptions() { LanguageEmbedding = (config == 2 ? LanguageEmbedding.Always : (config == 1 ? LanguageEmbedding.AsNeeded : LanguageEmbedding.Never)) })),
                            Lastmod = homepage.Statistics.Updated.ToString("yyyy-MM-dd"),
                            Changefrequency = homepage.Fields["ChangeFrequency"] != null ? homepage.Fields["Changefrequency"].ToEnum<SitemapChangeFrequency>() : SitemapChangeFrequency.Weekly,
                            Priority = this.GetPriority(homepage)
                        });

                        var sitemapPages = GetAllSitePageItems(homepage, Sitecore.Context.Database);

                        if (sitemapPages != null)
                        {
                            {
                                tmpurlset.AddRange(sitemapPages.Select(childItem => new Url
                                {
                                    Loc = GetAbsoluteLink(LinkManager.GetItemUrl(childItem, new UrlOptions() { LanguageEmbedding = (config == 2 ? LanguageEmbedding.Always : (config == 1 ? LanguageEmbedding.AsNeeded : LanguageEmbedding.Never)) })),
                                    Lastmod = childItem.Statistics.Updated.ToString("yyyy-MM-dd"),
                                    Changefrequency = childItem.Fields["ChangeFrequency"] != null ? childItem.Fields["Changefrequency"].ToEnum<SitemapChangeFrequency>() : SitemapChangeFrequency.Weekly,
                                    Priority = this.GetPriority(childItem)
                                }));
                            }
                            tmpurlset.RemoveAll(x => x.Changefrequency == SitemapChangeFrequency.DoNotInclude);
                            //tmpurlset.Sort();                          
                        }

                        urlSet.Urls = tmpurlset;

                        //Sort the List Alphabetically
                        urlSet.Urls = urlSet.Urls.OrderBy(k => k.Loc).ToList();

                        response.AddHeader("Content-Type", "text/xml");
                        ser.Serialize(response.OutputStream, urlSet);

                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error - Sitemap.xml." + ex.Message + " - " + ex.StackTrace, ex, this);
                    }

                    HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                    HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                    HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass
                                                                               //Response Ends Here
                }


            }
        }

        public string GetPriority(Item item)
        {
            if (item != null && item.Fields != null && item.Fields["Priority"] != null && !string.IsNullOrEmpty(item.Fields["Priority"].Value))
                return ((LookupField)item.Fields["Priority"]).TargetItem.DisplayName;
            return string.Empty;
        }

        /// <summary>
        /// Create Absolute url as per the site
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        private static string GetAbsoluteLink(string relativeUrl)
        {
            try
            {
                relativeUrl = relativeUrl.StartsWith("/en/") ?
                           relativeUrl.Replace("/en/", string.Empty) :
                           relativeUrl.EndsWith("/en") ? relativeUrl.Replace("/en", string.Empty) : relativeUrl;
                relativeUrl = relativeUrl.ToLower().Replace(" ", "-");



            }
            catch (Exception e)
            {
                Log.Error("Error - GetAbsoluteLink() " + e.Message + " - " + e.StackTrace, e, new object());
            }
            if (relativeUrl != null && relativeUrl == "/")
                return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + "/";
            else
                return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + "/" + relativeUrl.Remove(0, 1) + "/";
        }

        ///
        /// Append language or not in URL to return language specific sitemap.xml
        /// 

        private static int AppendLanguage()
        {
            return string.IsNullOrEmpty(Sitecore.Configuration.Settings.GetSetting("LanguageEmbedForSitemap")) ? 0 : System.Convert.ToInt32((Sitecore.Configuration.Settings.GetSetting("LanguageEmbedForSitemap")));
        }

        public static IList<Item> GetAllSitePageItems(Item item, Database db)
        {
            List<Item> allPageItems;
            var selectedPageItems = new List<Item>();
            var tempPageItems = new List<Item>();

            allPageItems = item.Axes.GetDescendants().Where(x => x.TemplateID.ToString().Equals(Sitecore.XA.Foundation.Multisite.Templates.Page.ID) || IsDerivedFromTemplate(x)).ToList();
            foreach (var tempPageItem in allPageItems)
            {
                if ((tempPageItem != null) && (tempPageItem.Fields != null))
                    tempPageItems.Add(tempPageItem);
            }

            selectedPageItems.AddRange(tempPageItems);
            return selectedPageItems;
        }

        private static bool IsDerivedFromTemplate(Item item)
        {
            return item.IsDerived(Sitecore.XA.Foundation.Multisite.Templates.Page.ID);

            //return item.IsDerived(new ID(Sitecore.Configuration.Settings.GetSetting(SiteMapConstants.SxaPageTemplateID)));
        }
    }

    public enum SitemapChangeFrequency
    {
        DoNotInclude,
        Always,
        Never,
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Yearly,
    }

}
