using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenSubtitleDownloader.Config;

namespace OpenSubtitleDownloader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                            .SetBasePath(AppContext.BaseDirectory)
                            .AddJsonFile("appsettings.json")
                            // .AddEnvironmentVariables()
                            .Build();


            
            var runtimeConfig = new RuntimeConfig(config);
            var worker = new Worker(runtimeConfig);
            worker.SingleExecution();
        }

        
    }
}
