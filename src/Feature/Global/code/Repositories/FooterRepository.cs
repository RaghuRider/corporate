using AIEnterprise.Feature.Global.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System;

namespace AIEnterprise.Feature.Global.Repositories
{
    public class FooterRepository : ModelRepository, IFooterRepository
    {
        public FooterRepository()
        {

        }


        public override IRenderingModelBase GetModel()
        {
            FooterModel footer = new FooterModel();

            if (Rendering.DataSource != null)
            {
                var dataSourceItem = ContentRepository.GetItem(Rendering.DataSource);
                footer.CopyRightsText = string.Format(dataSourceItem.Fields["Text"].Value, DateTime.Now.Year);
            }
            FillBaseProperties(footer);

            return footer;
        }
    }
}