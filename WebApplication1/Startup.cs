using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Autofac;
using Autofac.Extensions.DependencyInjection;
using LiteDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1
{
    public class Startup
    {
        private IContainer container = null;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database.litedb");
            services.AddMvc();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<LiteDatabase>((sp) => new LiteDatabase(connectionString));

            var builder = new ContainerBuilder();
            builder.RegisterType<LiteDatabase>().WithParameter("ConnectionString", connectionString);
            builder.RegisterType<UserServices>().As<IUserServices>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();

            builder.Populate(services);
            container = builder.Build();

            ImportSampleData();
        }

        public void ImportSampleData()
        {
            var userRepo = container.Resolve<IUserRepository>();

            var database = container.Resolve<LiteDatabase>();
            database.DropCollection("User");

            var sampleDataJson = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sampleUsers.json"));
            var users = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<User>>(sampleDataJson);

            users.ToList().ForEach(u => userRepo.CreateUser(u));
        }
    }
}