using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplicationAPI.ExtensionMethods;

namespace WebApplicationAPI.Installers {
  public static class InstallerExtensions {
    public static void InstallServicesInAssembly(
        this IServiceCollection services
      , IConfiguration configuration
    ) {
      IEnumerable<IInstaller> installers = typeof(Startup).Assembly.ExportedTypes
        .Where(type => typeof(IInstaller).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
        .Select(Activator.CreateInstance)
        .Cast<IInstaller>();

      installers.ForEach(installer => installer.InstallServices(services, configuration));
    }
  }
}