namespace AIEnterprise.Foundation.DI.Infrastructure
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;

    public class MvcControllerServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvcControllers("AIEnterprise.Feature.*");
            serviceCollection.AddClassesWithServiceAttribute("AIEnterprise.Feature.*");
            serviceCollection.AddClassesWithServiceAttribute("AIEnterprise.Foundation.*");
            serviceCollection.AddMvcControllers("AIEnterprise.*");
            serviceCollection.AddClassesWithServiceAttribute("AIEnterprise.*");
        }
    }
}