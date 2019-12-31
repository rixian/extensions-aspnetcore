// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.Sample.Api
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging(b => b
                .AddFilter("Microsoft", LogLevel.Debug)
                .AddFilter("System", LogLevel.Debug)
                .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                .SetMinimumLevel(LogLevel.Debug))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseApiSetup()
                        .UseStartup<Startup>();
                });
    }
}
