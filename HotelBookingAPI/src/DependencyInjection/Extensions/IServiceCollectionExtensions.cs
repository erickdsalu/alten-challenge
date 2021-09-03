using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjection.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void ConfigureDI(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            Bootstraper.ConfigureServices(serviceCollection, configuration);
        }
    }
}
