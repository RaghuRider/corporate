using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Collections;
using AIEnterprise.Foundation.DI;
using Sitecore.Resources.Media;
using Sitecore.Data.Fields;
using Sitecore;

namespace AIEnterprise.Foundation.Common
{
    [Service(typeof(ISitecoreHelper))]
    public class SitecoreHelper : ISitecoreHelper
    {
        public SitecoreHelper()
        {
            this.Database = Factory.GetDatabase(Sitecore.Context.Database.Name);
        }
        public Database Database
        {
            get;
            private set;
        }

        /// <summary>
        /// Get item from sitecore
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="isLazy"></param>
        /// <param name="inferType"></param>
        /// <returns></returns>
        public T GetItem<T>(string path, bool isLazy = false, bool inferType = false) where T : class
        {            
            Item item = this.Database.GetItem(path);
            return item as T;
        }

        /// <summary>
        /// Get item from sitecore
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="isLazy"></param>
        /// <param name="inferType"></param>
        /// <returns></returns>
        public Item GetItem(string itemName)
        {
            Item renderingParameters = this.Database.GetItem("");
            if (renderingParameters != null && renderingParameters.Children.Count > 0)
            {
                return renderingParameters.Children.Where(x => x.Name.Equals(itemName)).FirstOrDefault();
            }
            return null;
        }
        /// <summary>
        /// Get child items of a parent item from Sitecore
        /// </summary>
        /// <param name="datasourceItem"></param>
        /// <returns></returns>
        public IEnumerable<Item> GetCompositeItems(Item datasourceItem)
        {
            List<Item> children = datasourceItem.Children.ToList<Item>();
            return children;
        }

        /// <summary>
        /// Get Image and its URL from Sitecore.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="imageField"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public ImageField ImageUrlGeneric(Item item, string imageField, MediaUrlOptions options = null)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            string imageUrl = string.Empty;
            var image = (ImageField)item.Fields[imageField];
            //if (image?.MediaItem != null)
            //{
            //var imageValue = new MediaItem(image.MediaItem);
            //imageUrl = StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(imageValue));
            //}
            return image;
        }

        public string ImageUrl(ImageField imageField, MediaUrlOptions options = null)
        {
            if (imageField == null)
            {

            }
            string imageUrl = string.Empty;
            //var image = (ImageField)item.Fields[imageField];
            if (imageField?.MediaItem != null)
            {
                var imageValue = new MediaItem(imageField.MediaItem);
                imageUrl = StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(imageValue));
            }
            return imageUrl;
        }
    }
}