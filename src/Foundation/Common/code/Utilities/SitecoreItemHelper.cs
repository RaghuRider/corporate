using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIEnterprise.Foundation.Common.Utilities
{
    public static class SitecoreItemHelper
    {
        /// <summary>
        /// Get item from sitecore
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="isLazy"></param>
        /// <param name="inferType"></param>
        /// <returns></returns>
        public static string GetCssClass(string itemName)
        {
            try
            {
                Item renderingParameters = Sitecore.Context.Database.GetItem(GlobalConstants.renderingParametersPath);
                if (renderingParameters != null && renderingParameters.Children.Count > 0)
                {
                    var item = renderingParameters.Axes.GetDescendants().Where(x => x.Name.Equals(itemName) && x.TemplateID.ToString().Equals(TestimonialConstants.CssClassTemplateID)).FirstOrDefault();
                    if (item != null)
                    {
                        return item.Fields[TestimonialConstants.CssClass].Value;
                    }
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error in SitecoreItemHelper - GetCssClass Method" + ex.Message, "");
            }
            return string.Empty;
        }
    }
}