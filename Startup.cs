using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SwaggerOptions = WebApplicationAPI.Options.SwaggerOptions;

using WebApplicationAPI.Installers;
using System;

namespace WebApplicationAPI {
  public class Startup {
    public Startup(
        IConfiguration configuration
    ) {
      this.Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.InstallServicesInAssembly(this.Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(
        IApplicationBuilder app
      , IWebHostEnvironment env
    ) {
      /*if (env.IsDevelopment()) {*/
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
        SwaggerOptions swaggerOptions = this.Configuration
          .GetSection(nameof(SwaggerOptions))
          .Get<SwaggerOptions>();
        // Enable middleware to serve generated Swagger as a JSON endpoint. 
        app.UseSwagger(option => option.RouteTemplate = swaggerOptions.JsonRout);
        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint
        app.UseSwaggerUI(options =>
          options.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description));
      /*} else {*/
        // The default HSTS value is 30 days. You may want to change this for production scenarios,
        // see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      /*}*/
      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();
      app.UseEndpoints(endpoints => {
        endpoints.MapControllerRoute(
            name: "default"
          , pattern: "{controller=Home}/{action=Index}/{id?}"
        );
        endpoints.MapRazorPages();
      });
    }
  }
}
