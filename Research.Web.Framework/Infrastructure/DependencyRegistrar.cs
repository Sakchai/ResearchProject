using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Research.Configuration;
using Research.Core;
using Research.Core.Caching;
using Research.Core.Data;
using Research.Core.Domain.Common;
using Research.Data;
using Research.Data.Infrastructure;
using Research.Infrastructure;
using Research.Services;
using Research.Services.Common;
using Research.Services.Authentication;
using Research.Services.Configuration;
using Research.Services.Directory;
using Research.Services.Events;
using Research.Services.FiscalSchedules;
using Research.Services.Helpers;
using Research.Services.Logging;
using Research.Services.Media;
using Research.Services.Messages;
using Research.Services.Professors;
using Research.Services.Roles;
using Research.Services.Security;
using Research.Services.Tasks;
using Research.Services.Users;
using Research.Web.Areas.Admin.Factories;
using Research.Web.Factories;
using Research.Web.Framework;
using Research.Web.Framework.Mvc.Routing;
using Research.Web.Framework.UI;
using Research.Web.Models.Factories;
using Research.Services.Projects;
using Research.Services.Researchers;

namespace Research.Web.Infrastructure
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, ResearchConfig config)
        {
            //file provider
            builder.RegisterType<ResearchFileProvider>().As<IResearchFileProvider>().InstancePerLifetimeScope();

            builder.RegisterType<CommonSettings>().As<ISettings>().InstancePerLifetimeScope();

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            //user agent helper
            builder.RegisterType<UserAgentHelper>().As<IUserAgentHelper>().InstancePerLifetimeScope();

            //data layer
            builder.RegisterType<EfDataProviderManager>().As<IDataProviderManager>().InstancePerDependency();
            builder.Register(context => context.Resolve<IDataProviderManager>().DataProvider).As<IDataProvider>().InstancePerDependency();
            builder.Register(context => new ProjectdbContext(context.Resolve<DbContextOptions<ProjectdbContext>>()))
                .As<IDbContext>().InstancePerLifetimeScope();
            //repositories
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            //cache manager
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().InstancePerLifetimeScope();

            //static cache manager
            builder.RegisterType<MemoryCacheManager>()
                .As<ILocker>()
                .As<IStaticCacheManager>()
                .SingleInstance();
            //}
            builder.RegisterType<DefaultLogger>().As<ILogger>().InstancePerLifetimeScope();
            //work context
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();

            //services
            builder.RegisterType<TitleService>().As<ITitleService>().InstancePerLifetimeScope();
            builder.RegisterType<InstituteService>().As<IInstituteService>().InstancePerLifetimeScope();
            builder.RegisterType<AddressService>().As<IAddressService>().InstancePerLifetimeScope();
            builder.RegisterType<FiscalScheduleService>().As<IFiscalScheduleService>().InstancePerLifetimeScope();
            builder.RegisterType<AcademicRankService>().As<IAcademicRankService>().InstancePerLifetimeScope();
            builder.RegisterType<AgencyService>().As<IAgencyService>().InstancePerLifetimeScope();
            builder.RegisterType<EducationLevelService>().As<IEducationLevelService>().InstancePerLifetimeScope();
            builder.RegisterType<FacultyService>().As<IFacultyService>().InstancePerLifetimeScope();
            builder.RegisterType<StrategyGroupService>().As<IStrategyGroupService>().InstancePerLifetimeScope();
            builder.RegisterType<ProfessorService>().As<IProfessorService>().InstancePerLifetimeScope();
            builder.RegisterType<ResearchIssueService>().As<IResearchIssueService>().InstancePerLifetimeScope();
            builder.RegisterType<ProjectService>().As<IProjectService>().InstancePerLifetimeScope();
            builder.RegisterType<ResearcherService>().As<IResearcherService>().InstancePerLifetimeScope();
            builder.RegisterType<UserRegistrationService>().As<IUserRegistrationService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<PermissionService>().As<IPermissionService>().InstancePerLifetimeScope();
            builder.RegisterType<CountryService>().As<ICountryService>().InstancePerLifetimeScope();
            builder.RegisterType<ProvinceService>().As<IProvinceService>().InstancePerLifetimeScope();
            builder.RegisterType<DownloadService>().As<IDownloadService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageTemplateService>().As<IMessageTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<QueuedEmailService>().As<IQueuedEmailService>().InstancePerLifetimeScope();
            builder.RegisterType<EmailAccountService>().As<IEmailAccountService>().InstancePerLifetimeScope();
            builder.RegisterType<WorkflowMessageService>().As<IWorkflowMessageService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageTokenProvider>().As<IMessageTokenProvider>().InstancePerLifetimeScope();
            builder.RegisterType<Tokenizer>().As<ITokenizer>().InstancePerLifetimeScope();
            builder.RegisterType<EmailSender>().As<IEmailSender>().InstancePerLifetimeScope();
            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();
            builder.RegisterType<CookieAuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<UserActivityService>().As<IUserActivityService>().InstancePerLifetimeScope();
            builder.RegisterType<DateTimeHelper>().As<IDateTimeHelper>().InstancePerLifetimeScope();
            builder.RegisterType<PageHeadBuilder>().As<IPageHeadBuilder>().InstancePerLifetimeScope();
            builder.RegisterType<ScheduleTaskService>().As<IScheduleTaskService>().InstancePerLifetimeScope();
            //builder.RegisterType<ExportManager>().As<IExportManager>().InstancePerLifetimeScope();
            //builder.RegisterType<ImportManager>().As<IImportManager>().InstancePerLifetimeScope();
            //builder.RegisterType<PdfService>().As<IPdfService>().InstancePerLifetimeScope();
            //builder.RegisterType<UploadService>().As<IUploadService>().InstancePerLifetimeScope();
            builder.RegisterType<PictureService>().As<IPictureService>().SingleInstance();
            builder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance();
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().SingleInstance();
            builder.RegisterType<SettingService>().As<ISettingService>().InstancePerLifetimeScope();

            builder.RegisterType<ActionContextAccessor>().As<IActionContextAccessor>().InstancePerLifetimeScope();

            //register all settings
            builder.RegisterSource(new SettingsSource());

            //picture service
            builder.RegisterType<PictureService>().As<IPictureService>().InstancePerLifetimeScope();

            //event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>)))
                    .InstancePerLifetimeScope();
            }

            //factories
            builder.RegisterType<BaseAdminModelFactory>().As<IBaseAdminModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ProjectModelFactory>().As<IProjectModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<UserModelFactory>().As<IUserModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ActivityLogModelFactory>().As<IActivityLogModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ResearcherModelFactory>().As<IResearcherModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ProfessorModelFactory>().As<IProfessorModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ResearchIssueModelFactory>().As<IResearchIssueModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<FiscalScheduleModelFactory>().As<IFiscalScheduleModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<MessageTemplateModelFactory>().As<IMessageTemplateModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<EmailAccountModelFactory>().As<IEmailAccountModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ScheduleTaskModelFactory>().As<IScheduleTaskModelFactory>().InstancePerLifetimeScope();
            
        }

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order
        {
            get { return 0; }
        }
    }


    /// <summary>
    /// Setting source
    /// </summary>
    public class SettingsSource : IRegistrationSource
    {
        static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
            "BuildRegistration",
            BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// Registrations for
        /// </summary>
        /// <param name="service">Service</param>
        /// <param name="registrations">Registrations</param>
        /// <returns>Registrations</returns>
        public IEnumerable<IComponentRegistration> RegistrationsFor(
            Service service,
            Func<Service, IEnumerable<IComponentRegistration>> registrations)
        {
            var ts = service as TypedService;
            if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
            {
                var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
                yield return (IComponentRegistration)buildMethod.Invoke(null, null);
            }
        }

        static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
        {
            return RegistrationBuilder
                .ForDelegate((c, p) =>
                {
                    // var currentStoreId = c.Resolve<IStoreContext>().CurrentStore.Id;
                    //uncomment the code below if you want load settings per store only when you have two stores installed.
                    //var currentStoreId = c.Resolve<IStoreService>().GetAllStores().Count > 1
                    //    c.Resolve<IStoreContext>().CurrentStore.Id : 0;

                    //although it's better to connect to your database and execute the following SQL:
                    //DELETE FROM [Setting] WHERE [StoreId] > 0
                    //return c.Resolve<ISettingService>().LoadSetting<TSettings>(currentStoreId);
                    //chai
                    return c.Resolve<ISettingService>().LoadSetting<TSettings>(0);
                })
                .InstancePerLifetimeScope()
                .CreateRegistration();
        }

        /// <summary>
        /// Is adapter for individual components
        /// </summary>
        public bool IsAdapterForIndividualComponents { get { return false; } }
    }

}
