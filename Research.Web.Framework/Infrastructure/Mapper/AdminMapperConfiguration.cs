using AutoMapper;
using Research.Core.Domain.Users;
using Research.Data;
using Research.Infrastructure.Mapper;
using Research.Web.Framework.Models;
using Research.Web.Models.Directory;
using Research.Web.Models.Logging;
using Research.Web.Models.Messages;
using Research.Web.Models.Researchers;
using Research.Web.Models.Tasks;
using Research.Web.Models.Users;

namespace Research.Web.Framework.Infrastructure.Mapper
{
    /// <summary>
    /// AutoMapper configuration for admin area models
    /// </summary>
    public class AdminMapperConfiguration : Profile, IOrderedMapperProfile
    {
        #region Ctor

        public AdminMapperConfiguration()
        {
            //create specific maps
            CreateUsersMaps();
            CreateDirectoryMaps();
            CreateLoggingMaps();
            CreateMessagesMaps();
            CreateTasksMaps();
            CreateResearchersMaps();

            //add some generic mapping rules
            ForAllMaps((mapConfiguration, map) =>
            {
                //exclude Form and CustomProperties from mapping BaseResearchModel
                if (typeof(BaseResearchModel).IsAssignableFrom(mapConfiguration.DestinationType))
                {
                    map.ForMember(nameof(BaseResearchModel.Form), options => options.Ignore());
                    map.ForMember(nameof(BaseResearchModel.CustomProperties), options => options.Ignore());
                }

                //exclude ActiveStoreScopeConfiguration from mapping ISettingsModel
                if (typeof(ISettingsModel).IsAssignableFrom(mapConfiguration.DestinationType))
                    map.ForMember(nameof(ISettingsModel.ActiveStoreScopeConfiguration), options => options.Ignore());


            });

        }

        #endregion

        #region Utilities

        /// <summary>
        /// Create customers maps 
        /// </summary>
        protected virtual void CreateUsersMaps()
        {

            CreateMap<UserRole, UserRoleModel>();
            CreateMap<UserRoleModel, UserRole>();

            CreateMap<UserSettings, UserSettingsModel>();
            CreateMap<UserSettingsModel, UserSettings>()
                .ForMember(settings => settings.AvatarMaximumSizeBytes, options => options.Ignore())
                .ForMember(settings => settings.DeleteGuestTaskOlderThanMinutes, options => options.Ignore())
                .ForMember(settings => settings.DownloadableProductsValidateUser, options => options.Ignore())
                .ForMember(settings => settings.HashedPasswordFormat, options => options.Ignore())
                .ForMember(settings => settings.OnlineUserMinutes, options => options.Ignore())
                .ForMember(settings => settings.SuffixDeletedUsers, options => options.Ignore());

        }

        /// <summary>
        /// Create directory maps 
        /// </summary>
        protected virtual void CreateDirectoryMaps()
        {
            CreateMap<Country, CountryModel>();
            CreateMap<CountryModel, Country>()
                .ForMember(entity => entity.Provinces, options => options.Ignore());

            CreateMap<Province, ProvinceModel>();
            CreateMap<ProvinceModel, Province>()
                .ForMember(entity => entity.Country, options => options.Ignore());
        }

        /// <summary>
        /// Create logging maps 
        /// </summary>
        protected virtual void CreateLoggingMaps()
        {
            CreateMap<ActivityLog, ActivityLogModel>()
                .ForMember(model => model.ActivityLogTypeName, options => options.MapFrom(entity => entity.ActivityLogType.Name))
                .ForMember(model => model.CreatedOn, options => options.Ignore());
                //.ForMember(model => model.UserEmail, options => options.MapFrom(entity => entity.User.Email));
            CreateMap<ActivityLogModel, ActivityLog>()
                .ForMember(entity => entity.ActivityLogType, options => options.Ignore())
                .ForMember(entity => entity.ActivityLogTypeId, options => options.Ignore())
                .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
                .ForMember(entity => entity.User, options => options.Ignore())
                .ForMember(entity => entity.EntityId, options => options.Ignore())
                .ForMember(entity => entity.EntityName, options => options.Ignore());

            CreateMap<ActivityLogType, ActivityLogTypeModel>();
            CreateMap<ActivityLogTypeModel, ActivityLogType>()
                .ForMember(entity => entity.SystemKeyword, options => options.Ignore());

            CreateMap<Log, LogModel>()
                .ForMember(model => model.CreatedOn, options => options.Ignore())
                .ForMember(model => model.UserEmail, options => options.Ignore());
            CreateMap<LogModel, Log>()
                .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
                .ForMember(entity => entity.User, options => options.Ignore())
                .ForMember(entity => entity.LogLevelId, options => options.Ignore());
        }


        /// <summary>
        /// Create messages maps 
        /// </summary>
        protected virtual void CreateMessagesMaps()
        {

            CreateMap<EmailAccount, EmailAccountModel>();
            CreateMap<EmailAccountModel, EmailAccount>()
                .ForMember(entity => entity.Password, options => options.Ignore());

            CreateMap<MessageTemplate, MessageTemplateModel>();
            CreateMap<MessageTemplateModel, MessageTemplate>()
                .ForMember(entity => entity.DelayPeriod, options => options.Ignore());

            CreateMap<QueuedEmail, QueuedEmailModel>()
                .ForMember(model => model.CreatedOn, options => options.Ignore())
                .ForMember(model => model.DontSendBeforeDate, options => options.Ignore())
                .ForMember(model => model.EmailAccountName, options => options.MapFrom(entity => entity.EmailAccount != null ? entity.EmailAccount.FriendlyName : string.Empty))
                .ForMember(model => model.PriorityName, options => options.Ignore())
                .ForMember(model => model.SendImmediately, options => options.Ignore())
                .ForMember(model => model.SentOn, options => options.Ignore());
            CreateMap<QueuedEmailModel, QueuedEmail>()
                .ForMember(entity => entity.AttachmentFileName, options => options.Ignore())
                .ForMember(entity => entity.AttachmentFilePath, options => options.Ignore())
                .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
                .ForMember(entity => entity.DontSendBeforeDateUtc, options => options.Ignore())
                .ForMember(entity => entity.EmailAccount, options => options.Ignore())
                .ForMember(entity => entity.EmailAccountId, options => options.Ignore())
                .ForMember(entity => entity.Priority, options => options.Ignore())
                .ForMember(entity => entity.PriorityId, options => options.Ignore())
                .ForMember(entity => entity.SentOnUtc, options => options.Ignore());
        }


        /// <summary>
        /// Create tasks maps 
        /// </summary>
        protected virtual void CreateTasksMaps()
        {
            CreateMap<ScheduleTask, ScheduleTaskModel>();
            CreateMap<ScheduleTaskModel, ScheduleTask>()
                .ForMember(entity => entity.Type, options => options.Ignore());
        }


        /// <summary>
        /// Create vendors maps 
        /// </summary>
        protected virtual void CreateResearchersMaps()
        {
            CreateMap<Researcher, ResearcherModel>();
            CreateMap<ResearcherModel, Researcher>()
                .ForMember(entity => entity.Deleted, options => options.Ignore());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order => 0;

        #endregion
    }
}