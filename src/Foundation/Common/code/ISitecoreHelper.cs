using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnterprise.Foundation.Common
{
    public interface ISitecoreHelper
    {
        T GetItem<T>(string path, bool isLazy = false, bool inferType = false) where T : class;
        //List<T> GetChildren<T>(string Id, bool isLazy = false, bool inferType = false) where T : List<T>;
        IEnumerable<Item> GetCompositeItems(Item datasourceItem);
        ImageField ImageUrlGeneric(Item item, string imageField, MediaUrlOptions options = null);
        string ImageUrl(ImageField imageField, MediaUrlOptions options = null);
    }
}
