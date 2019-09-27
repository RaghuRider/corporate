using AIEnterprise.Feature.Global.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System.Web.Mvc;

namespace AIEnterprise.Feature.Global.Controllers
{
    public class FooterController : StandardController
    {

        private IFooterRepository _repository { get; }
        public FooterController(IFooterRepository repository)
        {
            this._repository = repository;
        }
        // GET: Global Model
        protected override object GetModel()
        {
            return _repository.GetModel();
        }
        /// <summary>
        /// Global action to get the model properties and return to view
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFooterCopyright()
        {
            return PartialView("FooterCopyright", GetModel());
        }
    }
}