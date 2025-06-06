using Abp.Dependency;
using Microsoft.Extensions.DependencyInjection;
using Shesha.Identity;

namespace NinjaKiwi.LifeQuest.Tests.DependencyInjection
{
    public static class ServiceCollectionRegistrar
    {
        public static void Register(IIocManager iocManager)
        {
            var services = new ServiceCollection();

            IdentityRegistrar.Register(services);
        }
    }
}
