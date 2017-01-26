using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication {
    public class Program {
        public static void Main(string[] args) {


            var builder = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>();

            if(string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")))
                builder = builder.UseUrls("http://*:1339");

            var host = builder.Build();
            host.Run();
        }
    }
}
