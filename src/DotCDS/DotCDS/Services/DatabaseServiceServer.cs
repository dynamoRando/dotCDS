using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Services
{
    internal class DatabaseServiceServer
    {
        #region Private Fields
        private IHost _server;
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        public static void Run(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public Task RunAsync(string[] args, string[] url, DatabaseServiceHandler processor, PortSettings portSettings)
        {
            if (_server is null)
            {
                _server = CreateHostBuilder(args, url, processor, portSettings).Build();
            }

            return _server.RunAsync();
        }

        public Task StopAsync()
        {
            return _server.StopAsync();
        }
        #endregion

        #region Private Methods
        private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<DatabaseServiceStartup>();
            });


        private static IHostBuilder CreateHostBuilder(string[] args, string[] url, DatabaseServiceHandler processor, PortSettings portSettings)
        {
            return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<DatabaseServiceStartup>();
                webBuilder.UseUrls(url);
                webBuilder.ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(portSettings.PortNumber, listenOptions => { listenOptions.Protocols = HttpProtocols.Http2; });
                });
            }).ConfigureServices(
                foo =>
                {
                    foo.Add(new ServiceDescriptor(typeof(DatabaseServiceHandler), processor));
                });
        }

        #endregion
    }
}
