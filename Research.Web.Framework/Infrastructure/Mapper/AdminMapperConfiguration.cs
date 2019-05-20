using System;
using AutoMapper;
using Research.Core.Domain.Users;
using Research.Data;
using Research.Infrastructure.Mapper;
using Research.Web.Framework.Models;
using Research.Web.Models;
using Research.Web.Models.Common;
using Research.Web.Models.Directory;
using Research.Web.Models.Logging;
using Research.Web.Models.Messages;
using Research.Web.Models.Professors;
using Research.Web.Models.Projects;
using Research.Web.Models.Researchers;
using Research.Web.Models.ResearchIssues;
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
            CreateCommonMaps();
            CreateDirectoryMaps();
            CreateLoggingMaps();
            CreateMessagesMaps();
            CreateTasksMaps();
            CreateResearchersMaps();
            CreateProfessorsMaps();
            CreateProjectsMaps();
            CreateUsersMaps();
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
        /// Create common maps 
        /// </summary>
        protected virtual void CreateCommonMaps()
        {
            CreateMap<Address, AddressModel>()
                .ForMember(model => model.ProvinceName, options => options.MapFrom(entity => entity.Province != null ? entity.Province.Name : null));
            CreateMap<AddressModel, Address>()
                .ForMember(entity => entity.Province, options => options.Ignore());
            CreateMap<ResearchIssue, ResearchIssueModel>()
                .ForMember(model => model.AvailableFiscalYears, options => options.Ignore());
            CreateMap<ResearchIssueModel, ResearchIssue>()
                .ForMember(model => model.Projects, options => options.Ignore());
        }
        /// <summary>
        /// Create directory maps 
        /// </summary>
        protected virtual void CreateDirectoryMaps()
        {
            CreateMap<Country, CountryModel>();
            CreateMap<CountryModel, Country>();

            CreateMap<Province, ProvinceModel>();
            CreateMap<ProvinceModel, Province>();
        }

        /// <summary>
        /// Create logging maps 
        /// </summary>
        protected virtual void CreateLoggingMaps()
        {
            CreateMap<ActivityLog, ActivityLogModel>()
                .ForMember(model => model.ActivityLogTypeName, options => options.MapFrom(entity => entity.ActivityLogType.Name))
                .ForMember(model => model.CreatedOn, options => options.Ignore())
                //chai
                //.ForMember(model => model.UserEmail, options => options.MapFrom(entity => entity.User.Email));
                .ForMember(model => model.UserEmail, options => options.Ignore());
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
            CreateMap<Researcher, ResearcherModel>()
                .ForMember(model => model.AvailableAcademicRanks, options => options.Ignore())
                .ForMember(model => model.AvailableAgencies, options => options.Ignore())
                .ForMember(model => model.AvailablePersonalTypes, options => options.Ignore())
                .ForMember(model => model.TitleName, options => options.MapFrom(entity => entity.Title != null ? entity.Title.TitleNameTH : null))
                .ForMember(model => model.AgencyName, options => options.MapFrom(entity => entity.Agency != null ? entity.Agency.Name : null))
                .ForMember(model => model.FullName, options => options.Ignore())
                .ForMember(model => model.IsCompleted, options => options.Ignore())
                .ForMember(model => model.AvailableTitles, options => options.Ignore())
                .ForMember(model => model.AddEducationCountryId, options => options.Ignore())
                .ForMember(model => model.AddEducationDegreeId, options => options.Ignore())
                .ForMember(model => model.AddEducationEducationLevelId, options => options.Ignore())
                .ForMember(model => model.AddEducationGraduationYear, options => options.Ignore())
                .ForMember(model => model.AddEducationInstituteId, options => options.Ignore())
                .ForMember(model => model.ResearcherEducationSearchModel, options => options.Ignore())
                .ForMember(model => model.AvailableAddEducationCountries, options => options.Ignore())
                .ForMember(model => model.AvailableAddEducationDegrees, options => options.Ignore())
                .ForMember(model => model.AvailableAddEducationEducationLevels, options => options.Ignore())
                .ForMember(model => model.AvailableAddEducationInstitutes, options => options.Ignore())
                .ForMember(model => model.AddressModel, options => options.Ignore());

            CreateMap<ResearcherModel, Researcher>()
                .ForMember(entity => entity.Deleted, options => options.Ignore())
                .ForMember(entity => entity.AcademicRank, options => options.Ignore())
                .ForMember(entity => entity.Address, options => options.Ignore())
                .ForMember(entity => entity.Agency, options => options.Ignore())
                .ForMember(entity => entity.Users, options => options.Ignore())
                .ForMember(entity => entity.PersonalType, options => options.Ignore());

        }

        private void CreateProfessorsMaps()
        {
            CreateMap<Professor, ProfessorModel>()
                .ForMember(model => model.AddressModel, options => options.Ignore())
                .ForMember(model => model.AvailableProvinces, options => options.Ignore())
                .ForMember(model => model.AvailableTitles, options => options.Ignore());
            CreateMap<ProfessorModel, Professor>()
                 .ForMember(model => model.ProfessorHistories, options => options.Ignore());
        }
        private void CreateProjectsMaps()
        {
            CreateMap<Project, ProjectModel>()
                .ForMember(model => model.AvailableProfessors, options => options.Ignore())
                .ForMember(model => model.AvailableProfessorTypes, options => options.Ignore())
                .ForMember(model => model.AvailableProgressStatuses, options => options.Ignore())
                .ForMember(model => model.AvailableProjectRoles, options => options.Ignore())
                .ForMember(model => model.AvailableProjectStatuses, options => options.Ignore())
                .ForMember(model => model.AvailableResearchers, options => options.Ignore())
                .ForMember(model => model.AvailableResearchIssues, options => options.Ignore())
                .ForMember(model => model.AvailableStrategyGroups, options => options.Ignore())
                .ForMember(model => model.ProgressStatusName, options => options.Ignore())
                .ForMember(model => model.StartContractDateName, options => options.Ignore())
                .ForMember(model => model.ProjectProfessorSearchModel, options => options.Ignore())
                .ForMember(model => model.ProjectProgressSearchModel, options => options.Ignore())
                .ForMember(model => model.ProjectResearcherSearchModel, options => options.Ignore())
                .ForMember(model => model.AddProjectPortion, options => options.Ignore())
                .ForMember(model => model.AddProjectProfessorId, options => options.Ignore())
                .ForMember(model => model.AddProjectProfessorTypeId, options => options.Ignore())
                .ForMember(model => model.AddProjectProgressComment, options => options.Ignore())
                .ForMember(model => model.AddProjectProgressEndDate, options => options.Ignore())
                .ForMember(model => model.AddProjectProgressStartDate, options => options.Ignore())
                .ForMember(model => model.AddProjectProgressStatusId, options => options.Ignore())
                .ForMember(model => model.AddProjectResearcherId, options => options.Ignore())
                .ForMember(model => model.AddProjectRoleId, options => options.Ignore());

            CreateMap<ProjectModel, Project>()
                .ForMember(entity => entity.FiscalSchedule, options => options.Ignore())
                .ForMember(entity => entity.ProjectStatus, options => options.Ignore())
                .ForMember(entity => entity.ProjectType, options => options.Ignore())
                .ForMember(entity => entity.ResearchIssue, options => options.Ignore())
                .ForMember(entity => entity.StrategyGroup, options => options.Ignore());

        }

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
        #endregion

        #region Properties

        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order => 0;

        #endregion
    }
}