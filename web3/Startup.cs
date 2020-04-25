using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OC;
using OC.Contacts.ContactsMenu;
using OC.Files;
using OC.Preview;
using OCP;
using OCP.AppFramework;
using OCP.ContactsNs.ContactsMenu;
using IContainer = Autofac.IContainer;

namespace web3
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddControllersAsServices();
            services.AddHttpContextAccessor();
            return services.BuildServiceProvider();
            // // Create the container builder.
            // var builder = new ContainerBuilder();
            //
            // // Register dependencies, populate the services from
            // // the collection, and build the container.
            // //
            // // Note that Populate is basically a foreach to add things
            // // into Autofac that are in the collection. If you register
            // // things in Autofac BEFORE Populate then the stuff in the
            // // ServiceCollection can override those things; if you register
            // // AFTER Populate those registrations can override things
            // // in the ServiceCollection. Mix and match as needed.
            // builder.Populate(services);
            // builder.RegisterType<OC.Calendar.Manager>().Named<OCP.Calendar.IManager>("CalendarManager");
            // builder.RegisterType<OC.Calendar.Resource.Manager>().Named<OCP.Calendar.Resource.IManager>("CalendarResourceBackendManager");
            // builder.RegisterType<OC.Calendar.Room.Manager>()
            //     .Named<OCP.Calendar.Room.IManager>("CalendarRoomBackendManager");
            // builder.RegisterType<OC.ContactsManager>().Named<OCP.ContactsNs.IManager>("ContactsManager");
            // builder.RegisterType<ActionFactory>().As<IActionFactory>();
            // builder.Register<PreviewManager>(( c,p) =>
            // {
            //     var server = c.Resolve<Server>();
            //     return new PreviewManager(server.getConfig(),
            //         server.getRootFolder(),
            //         server.getAppDataDir("preview"),
            //         server.getEventDispatcher(),
            //         "123");
            // }).Named<OCP.IPreview>("PreviewManager");
            // builder.Register<Watcher>((c, p) =>
            // {
            //     var server = p.TypedAs<Server>();
            //     return new Watcher(server.getAppDataDir("preview"));
            // });
            // builder.Register<OCP.Encryption.IManager>((c, p) =>
            // {
            //     var server = c.Resolve<IServerContainer>();
            //     var view = new View();
            //     var util = new OC.Encryption.Util(view, server.getUserManager(), server.getGroupManager(), server.getConfig());
            //     return new OC.Encryption.Manager(server.getConfig(),server.getLogger(),server.getL10N("core"),
            //         new View(), util, new List<string>());
            // });
            // this.ApplicationContainer = builder.Build();
            //
            // // Create the IServiceProvider based on the container.
            // return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}