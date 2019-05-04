using System;
using System.Linq;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Research.Configuration;
using Research.Core;
using Research.Core.Data;
using Research.Core.Domain;
using Research.Core.Http;
using Research.Data;
using Research.Domain.Security;
using Research.Framework.Infrastructure.Extensions;
using Research.Framework.Mvc.Routing;
using Research.Services.Authentication;
using Research.Services.Logging;
using Research.Services.Security;
using Research.Services.Tasks;
using StackExchange.Profiling.Storage;

namespace Research.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        /// <returns>Configured service provider</returns>
        public static IServiceProvider ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //add ResearchConfig configuration parameters
            services.ConfigureStartupConfig<ResearchConfig>(configuration.GetSection("Research"));
            //add hosting configuration parameters
            services.ConfigureStartupConfig<HostingConfig>(configuration.GetSection("Hosting"));
            //add accessor to HttpContext
            services.AddHttpContextAccessor();

            //create, initialize and configure the engine
            var engine = EngineContext.Create();
            engine.Initialize(services);
            var serviceProvider = engine.ConfigureServices(services, configuration);

            if (DataSettingsManager.DatabaseIsInstalled)
            {
                //chai
                //implement schedule tasks
                //database is already installed, so start scheduled tasks
                TaskManager.Instance.Initialize();
                TaskManager.Instance.Start();

                //log application start
                EngineContext.Current.Resolve<ILogger>().Information("Application started", null, null);
            }

            return serviceProvider;
        }

        /// <summary>
        /// Create, bind and register as service the specified configuration parameters 
        /// </summary>
        /// <typeparam name="TConfig">Configuration parameters</typeparam>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Set of key/value application configuration properties</param>
        /// <returns>Instance of configuration parameters</returns>
        public static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            //create instance of config
            var config = new TConfig();

            //bind it to the appropriate section of configuration
            configuration.Bind(config);

            //and register it as a service
            services.AddSingleton(config);

            return config;
        }

        /// <summary>
        /// Register HttpContextAccessor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// Adds services required for anti-forgery support
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddAntiForgery(this IServiceCollection services)
        {
            //override cookie name
            //chai
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = $"{ResearchCookieDefaults.Prefix}{ResearchCookieDefaults.AntiforgeryCookie}";

                //whether to allow the use of anti-forgery cookies from SSL protected page on the other store pages which are not
                options.Cookie.SecurePolicy = DataSettingsManager.DatabaseIsInstalled && EngineContext.Current.Resolve<SecuritySettings>().ForceSslForAllPages
                    ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.None;
            });
        }

        /// <summary>
        /// Adds services required for application session state
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpSession(this IServiceCollection services)
        {
            //chai
            services.AddSession(options =>
            {
                options.Cookie.Name = $"{ResearchCookieDefaults.Prefix}{ResearchCookieDefaults.SessionCookie}";
                options.Cookie.HttpOnly = true;

                //whether to allow the use of session values from SSL protected page on the other store pages which are not
                options.Cookie.SecurePolicy = DataSettingsManager.DatabaseIsInstalled && EngineContext.Current.Resolve<SecuritySettings>().ForceSslForAllPages
                    ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.None;
            });
        }

        /// <summary>
        /// Adds services required for themes support
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddThemes(this IServiceCollection services)
        {
            //if (!DataSettingsManager.DatabaseIsInstalled)
            //    return;

            ////themes support
            //services.Configure<RazorViewEngineOptions>(options =>
            //{
            //    options.ViewLocationExpanders.Add(new ThemeableViewLocationExpander());
            //});
        }

        /// <summary>
        /// Adds data protection services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddResearchDataProtection(this IServiceCollection services)
        {
            //check whether to persist data protection in Redis
            var researchConfig = services.BuildServiceProvider().GetRequiredService<ResearchConfig>();
            //if (researchConfig.RedisCachingEnabled && researchConfig.PersistDataProtectionKeysToRedis)
            //{
            //    //store keys in Redis
            //    services.AddDataProtection().PersistKeysToRedis(() =>
            //    {
            //        var redisConnectionWrapper = EngineContext.Current.Resolve<IRedisConnectionWrapper>();
            //        return redisConnectionWrapper.GetDatabase();
            //    }, ResearchCachingDefaults.RedisDataProtectionKey);
            //}
            //else
            //{
                var dataProtectionKeysPath = CommonHelper.DefaultFileProvider.MapPath("~/App_Data/DataProtectionKeys");
                var dataProtectionKeysFolder = new System.IO.DirectoryInfo(dataProtectionKeysPath);

                //configure the data protection system to persist keys to the specified directory
                services.AddDataProtection().PersistKeysToFileSystem(dataProtectionKeysFolder);
           // }
        }

        /// <summary>
        /// Adds authentication service
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddResearchAuthentication(this IServiceCollection services)
        {
            //chai
            //set default authentication schemes
            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = ResearchAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultSignInScheme = ResearchAuthenticationDefaults.ExternalAuthenticationScheme;
            });

            ////add main cookie authentication
            authenticationBuilder.AddCookie(ResearchAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = $"{ResearchCookieDefaults.Prefix}{ResearchCookieDefaults.AuthenticationCookie}";
                options.Cookie.HttpOnly = true;
                options.LoginPath = ResearchAuthenticationDefaults.LoginPath;
                options.AccessDeniedPath = ResearchAuthenticationDefaults.AccessDeniedPath;

                //whether to allow the use of authentication cookies from SSL protected page on the other store pages which are not
                options.Cookie.SecurePolicy = DataSettingsManager.DatabaseIsInstalled && EngineContext.Current.Resolve<SecuritySettings>().ForceSslForAllPages
                    ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.None;
            });

            ////add external authentication
            //authenticationBuilder.AddCookie(ResearchAuthenticationDefaults.ExternalAuthenticationScheme, options =>
            //{
            //    options.Cookie.Name = $"{ResearchCookieDefaults.Prefix}{ResearchCookieDefaults.ExternalAuthenticationCookie}";
            //    options.Cookie.HttpOnly = true;
            //    options.LoginPath = ResearchAuthenticationDefaults.LoginPath;
            //    options.AccessDeniedPath = ResearchAuthenticationDefaults.AccessDeniedPath;

            //    //whether to allow the use of authentication cookies from SSL protected page on the other store pages which are not
            //    options.Cookie.SecurePolicy = DataSettingsManager.DatabaseIsInstalled && EngineContext.Current.Resolve<SecuritySettings>().ForceSslForAllPages
            //        ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.None;
            //});

            ////register and configure external authentication plugins now
            //var typeFinder = new WebAppTypeFinder();
            //var externalAuthConfigurations = typeFinder.FindClassesOfType<IExternalAuthenticationRegistrar>();
            //var externalAuthInstances = externalAuthConfigurations
            //  //  .Where(x => PluginManager.FindPlugin(x)?.Installed ?? true) //ignore not installed plugins
            //    .Select(x => (IExternalAuthenticationRegistrar)Activator.CreateInstance(x));

            //foreach (var instance in externalAuthInstances)
            //    instance.Configure(authenticationBuilder);
        }

        /// <summary>
        /// Add and configure MVC for the application
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <returns>A builder for configuring MVC services</returns>
        public static IMvcBuilder AddResearchMvc(this IServiceCollection services)
        {
            //add basic MVC feature
            var mvcBuilder = services.AddMvc();

            //sets the default value of settings on MvcOptions to match the behavior of asp.net core mvc 2.1
            mvcBuilder.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var researchConfig = services.BuildServiceProvider().GetRequiredService<ResearchConfig>();
            if (researchConfig.UseSessionStateTempDataProvider)
            {
                //use session-based temp data provider
                mvcBuilder.AddSessionStateTempDataProvider();
            }
            //chai
            else
            {
                //use cookie-based temp data provider
                mvcBuilder.AddCookieTempDataProvider(options =>
                {
                    options.Cookie.Name = $"{ResearchCookieDefaults.Prefix}{ResearchCookieDefaults.TempDataCookie}";

                    //whether to allow the use of cookies from SSL protected page on the other store pages which are not
                    options.Cookie.SecurePolicy = DataSettingsManager.DatabaseIsInstalled && EngineContext.Current.Resolve<SecuritySettings>().ForceSslForAllPages
                        ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.None;
                });
            }

            //MVC now serializes JSON with camel case names by default, use this code to avoid it
            mvcBuilder.AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            //add custom display metadata provider
            //chai
            //mvcBuilder.AddMvcOptions(options => options.ModelMetadataDetailsProviders.Add(new ResearchMetadataProvider()));

            //add custom model binder provider (to the top of the provider list)
            //chai
            //mvcBuilder.AddMvcOptions(options => options.ModelBinderProviders.Insert(0, new ResearchModelBinderProvider()));

            //add fluent validation
            //chai
            //mvcBuilder.AddFluentValidation(configuration =>
            //{
            //    configuration.ValidatorFactoryType = typeof(ResearchValidatorFactory);
            //    //implicit/automatic validation of child properties
            //    configuration.ImplicitlyValidateChildProperties = true;
            //});

            return mvcBuilder;
        }

        /// <summary>
        /// Register custom RedirectResultExecutor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddResearchRedirectResultExecutor(this IServiceCollection services)
        {
            //we use custom redirect executor as a workaround to allow using non-ASCII characters in redirect URLs
            services.AddSingleton<IActionResultExecutor<RedirectResult>, ResearchRedirectResultExecutor>();
        }

        /// <summary>
        /// Register base object context
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddProjectdbContext(this IServiceCollection services)
        {
            services.AddDbContext<ProjectdbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServerWithLazyLoading(services);
            });
        }

        /// <summary>
        /// Add and configure MiniProfiler service
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddResearchMiniProfiler(this IServiceCollection services)
        {
            //whether database is already installed
            //if (!DataSettingsManager.DatabaseIsInstalled)
            //    return;

            services.AddMiniProfiler(miniProfilerOptions =>
            {
            //use memory cache provider for storing each result
            (miniProfilerOptions.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);

            //whether MiniProfiler should be displayed
            miniProfilerOptions.ShouldProfile = request =>
                EngineContext.Current.Resolve<ResearchInformationSettings>().DisplayMiniProfilerInPublicStore;

            //determine who can access the MiniProfiler results
            miniProfilerOptions.ResultsAuthorize = request => !EngineContext.Current.Resolve<ResearchInformationSettings>().DisplayMiniProfilerForAdminOnly;
            }).AddEntityFramework();
            //miniProfilerOptions.ResultsAuthorize = request => !EngineContext.Current.Resolve<ResearchInformationSettings>().DisplayMiniProfilerForAdminOnly;
            //  || EngineContext.Current.Resolve<IPermissionService>().Authorize(StandardPermissionProvider.AccessAdminPanel);

        }
    }
}