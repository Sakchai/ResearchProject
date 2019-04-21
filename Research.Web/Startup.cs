using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Research.Infrastructure.Extensions;
using Research.Web.Framework;
using System;

namespace Research
{
    public class Startup
    {
        #region Properties

        /// <summary>
        /// Get Configuration of the application
        /// </summary>
        public IConfiguration Configuration { get; }

        #endregion

        #region Ctor

        public Startup(IConfiguration configuration)
        {
            //set configuration
            Configuration = configuration;
        }

        #endregion

        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.ConfigureApplicationServices(Configuration);
        }

        /// <summary>
        /// Configure the application HTTP request pipeline
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            application.ConfigureRequestPipeline();
        }

        //public Startup(IHostingEnvironment env)
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

        //    if (env.IsDevelopment())
        //    {
        //        // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
        //        builder.AddUserSecrets<Startup>();
        //    }

        //    builder.AddEnvironmentVariables();
        //    Configuration = builder.Build();

        //}

        //public IConfigurationRoot Configuration { get; }
        ////public Startup(IConfiguration configuration)
        ////{
        ////    //set configuration
        ////    Configuration = configuration;
        ////}

        //// This method gets called by the runtime. Use this method to add services to the container.
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    // Add framework services.
        //    //services.AddTransient<ProjectdbContext, ProjectdbContext>(sp =>
        //    //{
        //    //    return new ProjectdbContext(Configuration.GetConnectionString("Research.API"));
        //    //});

        //    //add object context
        //    services.AddResearchObjectContext();

        //    ////add EF services
        //    services.AddEntityFrameworkSqlServer();
        //    services.AddEntityFrameworkProxies();
        //    services.ConfigureApplicationServices(Configuration);


        //    //services.AddDbContext<ApplicationDbContext>(options =>
        //    //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        //    //services.AddIdentity<ApplicationUser, IdentityRole>()
        //    //    .AddEntityFrameworkStores<ApplicationDbContext>()
        //    //    .AddDefaultTokenProviders();

        //    //services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();
        //    //services.AddScoped<SignInManager<ApplicationUser>, AuditableSignInManager<ApplicationUser>>();
        //    //services.AddMvc();
        //    //services.AddMvc(config =>
        //    //{
        //    //    var policy = new AuthorizationPolicyBuilder()
        //    //                     .RequireAuthenticatedUser()
        //    //                     .Build();
        //    //    config.Filters.Add(new AuthorizeFilter(policy));
        //    //});


        //    #region "DI code"
        //    //chai

        //    //general unitofwork and repository injections
        //    //services.AddTransient<IUnitOfWork, UnitOfWork>();

        //    //services.AddTransient(typeof(IService<,>), typeof(GenericService<,>));
        //    //services.AddTransient(typeof(IServiceAsync<,>), typeof(GenericServiceAsync<,>));

        //    // services.AddTransient(typeof(RoleService), typeof(RoleService));
        //    //  services.AddTransient(typeof(IResearcherService), typeof(ResearcherService));


        //    //file provider
        //    //services.AddTransient(typeof(IResearchFileProvider), typeof(ResearchFileProvider));

        //    ////web helper
        //    //services.AddTransient(typeof(IWebHelper), typeof(WebHelper));

        //    //data layer
        //    //services.AddTransient(typeof(IDbContext), typeof(ResearchObjectContext));
        //    ////repositories
        //    //services.AddTransient(typeof(IRepository<>), typeof(EfRepository<>));
        //    //services.AddTransient(typeof(ICacheManager), typeof(PerRequestCacheManager));
        //    ////static cache manager
        //    //services.AddTransient(typeof(ILocker), typeof(MemoryCacheManager));
        //    //services.AddTransient(typeof(IStaticCacheManager), typeof(MemoryCacheManager));

        //    //services.AddTransient(typeof(IScheduleTaskService), typeof(ScheduleTaskService));
        //    //services.AddTransient(typeof(IEventPublisher), typeof(EventPublisher));
        //    //services.AddTransient(typeof(ISubscriptionService), typeof(SubscriptionService));


        //    services.AddTransient(typeof(IProjectService), typeof(ProjectService));
        //    //services.AddTransient(typeof(IResearcherService), typeof(ResearcherService));
        //    // services.AddTransient(typeof(IProjectResearcherService), typeof(ProjectResearcherService));
        //    //services.AddTransient<IProjectModelFactory, ProjectModelFactory>();
        //    #endregion

        //    //data mapper profiler setting
        //    //Mapper.Initialize((config) =>
        //    //{
        //    //    config.AddProfile<MappingProfile>();
        //    //});

        //    //services.Configure<CookieAuthenticationOptions>(options =>
        //    //{
        //    //    options.LoginPath = new PathString("/User/Login");
        //    //});

        //    // Add application services.

        //}

        //// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        //{
        //    loggerFactory.AddConsole(Configuration.GetSection("Logging"));
        //    loggerFactory.AddDebug();

        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //        app.UseDatabaseErrorPage();
        //        app.UseBrowserLink();
        //    }
        //    else
        //    {
        //        app.UseExceptionHandler("/Home/Error");
        //    }

        //    app.UseStatusCodePagesWithRedirects("~/Home/Error/{0}");

        //    app.UseStaticFiles();
        //    app.UseAuthentication();

        //    //Add middleware here
        //    //app.UseMiddleware<RequestLoggingMiddleware>();


        //    // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

        //    //do not add middleware here (it wont be invoked)
        //    app.UseMvc(routes =>
        //    {
        //        routes.MapRoute(
        //            name: "default",
        //            template: "{controller=Project}/{action=List}/{id?}");
        //    });



        //    app.Use(async (context, next) =>
        //    {
        //        await next.Invoke();
        //    });

        //    app.Run(async context =>
        //    {
        //        var logger = loggerFactory.CreateLogger(env.EnvironmentName);
        //        logger.LogInformation("No endpoint found for request {path}", context.Request.Path);
        //        await context.Response.WriteAsync("No endpoint found - try /api/todo.");
        //        //await context.Response.WriteAsync("ill be executed after the code above!");
        //        //Debug.WriteLine("invoke thru await next.Invoke();");
        //    });

        //    // Populate default user admin
        //    //DataSeed.Seed(app.ApplicationServices).Wait();
        //}
    }
}
