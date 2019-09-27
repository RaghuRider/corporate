using AIEnterprise.Feature.Global.Controllers;
using AIEnterprise.Feature.Global.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace AIEnterprise.Feature.Global.Services
{
    public class FooterService : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<FooterController>();
            serviceCollection.AddTransient<IFooterRepository, FooterRepository>();
        }
    }
}