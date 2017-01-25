using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApplication.Contracts;
using WebApplication.Services;

namespace WebApplication {
    public class Startup {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath);

            if(env.IsDevelopment()) {
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();
            services.AddSingleton<IResourceManifestCache, ResourceManifestCache>();
            services.AddSingleton<IConfigurationRoot>(Configuration);
            services.AddSingleton<ITicketService, TicketService>();
            services.AddSingleton<IMailService, MailService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole((text, logLevel) => logLevel >= LogLevel.Debug, true);
            loggerFactory.AddDebug();

            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Whopsie");
            }

            #region StaticFiles configuration

            var contentTypeProvider = new FileExtensionContentTypeProvider();
            contentTypeProvider.Mappings[".br"] = "application/brotli";

            app.UseStaticFiles(new StaticFileOptions {
                ContentTypeProvider = contentTypeProvider,
                OnPrepareResponse = context => {
                    context.Context.Response.Headers.Add("Cache-Control", "public, max-age=31536000");

                    var filename = context.File.Name.ToLowerInvariant();
                    if (filename.EndsWith(".gz")) {
                        if (filename.EndsWith("js.gz")) {
                            context.Context.Response.ContentType = "application/javascript";
                        } else if (filename.EndsWith("css.gz")) {
                            context.Context.Response.ContentType = "text/css";
                        }

                        context.Context.Response.Headers.Add("Content-Encoding", "gzip");
                    } else if (filename.EndsWith(".br")) {
                        if (filename.EndsWith("js.br")) {
                            context.Context.Response.ContentType = "application/javascript";
                        } else if (filename.ToLowerInvariant().EndsWith("css.br")) {
                            context.Context.Response.ContentType = "text/css";
                        }

                        context.Context.Response.Headers.Add("Content-Encoding", "br");
                    }
                }
            });

            #endregion
            
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
