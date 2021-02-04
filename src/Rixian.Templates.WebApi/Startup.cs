using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;

namespace Rixian.Templates.WebApi
{
    public class Startup : Rixian.Extensions.AspNetCore.BaseStartup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
            : base(configuration, environment)
        {
            //Rixian.Extensions.AspNetCore.re
            //Rixian.Extensions.AspNetCore.DataProtection.DataProtectionConfig
            //FooBar.BaseStartup
            //FooBar.TestCsFile
            //Microsoft.AspNetCore.EnableDataProtectionAttribute
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        //public override void ConfigureServicesCore(IServiceCollection services)
        //{
        //}
    }
}
