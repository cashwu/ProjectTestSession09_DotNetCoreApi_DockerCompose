using System;
using DbInit.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DbInit
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if ((Configuration["INITDB"] ?? "false") == "true")
            {
                System.Threading.Thread.Sleep(10000);
                System.Console.WriteLine("Preparing Database ...");
                InitializeDatabase prepareData = new InitializeDatabase(this.Configuration);
                prepareData.Execute();
                System.Console.WriteLine("Database Preparation Complete");
            }

            app.UseMvc();
        }
    }
}