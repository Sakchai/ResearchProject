using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Research.Core.Caching;
using Research.Enum;
using Research.Services;
using Research.Services.Common;
using Research.Services.Directory;
using Research.Services.Helpers;
using Research.Services.Logging;
using Research.Services.Messages;
using Research.Services.Professors;
using Research.Services.FiscalSchedules;
using Research.Services.Researchers;
using Research.Data;
using System.Runtime.Serialization;

namespace Research.Web.Models.Factories
{
    /// <summary>
    /// Represents the implementation of the base model factory that implements a most common admin model factories methods
    /// </summary>
    public partial class BaseAdminModelFactory : IBaseAdminModelFactory
    {
        #region Fields

        private readonly IFacultyService _facultyService;
        private readonly ICountryService _countryService;
        private readonly IUserActivityService _userActivityService;
        private readonly IUserService _userService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IProfessorService _professorService;
        private readonly IProvinceService _provinceService;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IAcademicRankService _academicRankService;
        private readonly IResearchIssueService _researchIssueService;
        private readonly IAgencyService _agencyService;
        private readonly IResearcherService _researcherService;
        private readonly IEducationLevelService _educationLevelService;
        private readonly IStrategyGroupService _strategyGroupService;
        private readonly IFiscalScheduleService _fiscalScheduleService;
        private readonly ITitleService _titleService;
        private readonly IInstituteService _instituteService;
        #endregion

        #region Ctor

        public BaseAdminModelFactory(IFacultyService facultyService,
            ICountryService countryService,
            IUserActivityService userActivityService,
            IUserService userService,
            IDateTimeHelper dateTimeHelper,
            IEmailAccountService emailAccountService,
            IProfessorService professorService,
            IProvinceService provinceService,
            IStaticCacheManager cacheManager,
            IAcademicRankService academicRankService,
            IResearchIssueService researchIssueService,
            IAgencyService agencyService,
            IResearcherService researcherService,
            IEducationLevelService educationLevelService,
            IStrategyGroupService strategyGroupService,
            IFiscalScheduleService fiscalScheduleService,
            ITitleService titleService,
            IInstituteService instituteService)
        {
            this._facultyService = facultyService;
            this._countryService = countryService;
            this._userActivityService = userActivityService;
            this._userService = userService;
            this._dateTimeHelper = dateTimeHelper;
            this._emailAccountService = emailAccountService;
            this._professorService = professorService;
            this._provinceService = provinceService;
            this._cacheManager = cacheManager;
            this._academicRankService = academicRankService;
            this._researchIssueService = researchIssueService;
            this._agencyService = agencyService;
            this._researcherService = researcherService;
            this._educationLevelService = educationLevelService;
            this._strategyGroupService = strategyGroupService;
            this._fiscalScheduleService = fiscalScheduleService;
            this._titleService = titleService;
            this._instituteService = instituteService;
        }

        public BaseAdminModelFactory()
        {
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Prepare default item
        /// </summary>
        /// <param name="items">Available items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use "All" text</param>
        protected virtual void PrepareDefaultItem(IList<SelectListItem> items, bool withSpecialDefaultItem, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //whether to insert the first special item for the default value
            if (!withSpecialDefaultItem)
                return;

            //at now we use "0" as the default value
            const string value = "0";

            //prepare item text
          //  defaultItemText = defaultItemText ?? _localizationService.GetResource("Admin.Common.All");

            //insert this default item at first
            items.Insert(0, new SelectListItem { Text = defaultItemText, Value = value });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare available activity log types
        /// </summary>
        /// <param name="items">Activity log type items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareActivityLogTypes(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available activity log types
            var availableActivityTypes = _userActivityService.GetAllActivityTypes();
            foreach (var activityType in availableActivityTypes)
            {
                items.Add(new SelectListItem { Value = activityType.Id.ToString(), Text = activityType.Name });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        /// <summary>
        /// Prepare available order statuses
        /// </summary>
        /// <param name="items">Order status items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareProjectStatuses(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available order statuses
            var availableStatusItems = ProjectStatus.WaitingApproval.ToSelectList(false);
            foreach (var statusItem in availableStatusItems)
            {
                items.Add(statusItem);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        /// <summary>
        /// Prepare available Research Issues
        /// </summary>
        /// <param name="items">Research issues items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareResearchIssues(IList<SelectListItem> items, int fiscalYear = 0, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available research issues
            var researchIssuesStatusItems = new List<ResearchIssue>();
            if (fiscalYear != 0)
                researchIssuesStatusItems = _researchIssueService.GetAllResearchIssues().Where(x => x.FiscalYear == fiscalYear).ToList();
            else
                researchIssuesStatusItems = _researchIssueService.GetAllResearchIssues();

            foreach (var researchIssue in researchIssuesStatusItems)
            {
                items.Add(new SelectListItem { Value = researchIssue.Id.ToString(), Text = researchIssue.Name });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        /// <summary>
        /// Prepare available agencies
        /// </summary>
        /// <param name="items">Agencies items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareAgencies(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available shipping statuses
            var agenciesStatusItems = _agencyService.GetAllAgencies();
            foreach (var agency in agenciesStatusItems)
            {
                items.Add(new SelectListItem { Value = agency.Id.ToString(), Text = agency.Name });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        /// <summary>
        /// Prepare available countries
        /// </summary>
        /// <param name="items">Country items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareCountries(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available countries
            var availableCountries = _countryService.GetAllCountries(showHidden: true);
            foreach (var country in availableCountries)
            {
                items.Add(new SelectListItem { Value = country.Id.ToString(), Text = country.Name });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText ?? "Thailand");
        }

        /// <summary>
        /// Prepare available states and provinces
        /// </summary>
        /// <param name="items">State and province items</param>
        /// <param name="countryId">Country identifier; pass null to don't load states and provinces</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareProvinces(IList<SelectListItem> items,bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //if (countryId.HasValue)
            //{
                //prepare available states and provinces of the country
                var availableStates = _provinceService.GetProvinces(showHidden: true);
                foreach (var state in availableStates)
                {
                    items.Add(new SelectListItem { Value = state.Id.ToString(), Text = state.Name });
                }

                //insert special item for the default value
                if (items.Any())
                    PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText ?? "กรุงเทพมหานคร");
            //}

            ////insert special item for the default value
            //if (!items.Any())
            //    PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText ?? "Thailand");
        }

        /// <summary>
        /// Prepare available user roles
        /// </summary>
        /// <param name="items">User role items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareUserRoles(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var availableRoles = _userService.GetAllUserRoles();
            foreach (var role in availableRoles)
            {
                items.Add(new SelectListItem { Value = role.Id.ToString(), Text = role.Name });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        /// <summary>
        /// Prepare available email accounts
        /// </summary>
        /// <param name="items">Email account items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareEmailAccounts(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available email accounts
            var availableEmailAccounts = _emailAccountService.GetAllEmailAccounts();
            foreach (var emailAccount in availableEmailAccounts)
            {
                items.Add(new SelectListItem { Value = emailAccount.Id.ToString(), Text = $"{emailAccount.DisplayName} ({emailAccount.Email})" });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        /// <summary>
        /// Prepare available Academic Ranks
        /// </summary>
        /// <param name="items">eAcademic Rank items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareAcademicRanks(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            
            //prepare available academic ranks
            var availableAcademicRanks = _academicRankService.GetAllAcademicRanks();
            foreach (var academicRank in availableAcademicRanks)
            {
                items.Add(new SelectListItem { Value = academicRank.Id.ToString(), Text = academicRank.NameTh });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText ?? "ระบุ Academic Rank");
        }

        /// <summary>
        /// Prepare available facuties
        /// </summary>
        /// <param name="items">Faculty items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareFacuties(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available facuties
            var availableFaculties = _facultyService.GetAllFaculties();
            foreach (var faculty in availableFaculties)
            {
                items.Add(new SelectListItem { Value = faculty.Id.ToString(), Text = faculty.Name });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText ?? "ระบุคณะ");
        }

        /// <summary>
        /// Prepare available professors
        /// </summary>
        /// <param name="items">Professor items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareProfessors(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available professors
            var availableProfessors = _professorService.GetAllProfessors();
            foreach (var professor in availableProfessors)
            {
                items.Add(new SelectListItem { Value = professor.Id.ToString(), Text = $"{professor.TitleName}{professor.FirstName} {professor.LastName}" });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText ?? "ระบุผู้ทรงคุณวุฒิ");
        }

        /// <summary>
        /// Prepare available researchers
        /// </summary>
        /// <param name="items">Researcher items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareResearchers(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available researchers            
            var availableResearchers = _researcherService.GetAllResearchers();
            foreach (var researcher in availableResearchers)
            {
                items.Add(new SelectListItem { Value = researcher.Id.ToString(), Text = $"{researcher.TitleName}{researcher.FirstName} {researcher.LastName}" });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText ?? "ระบุผู้วิจัย");
        }

        /// <summary>
        /// Prepare available Education Levels
        /// </summary>
        /// <param name="items">Education Levels items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareEducationLevels(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available education levels
            var availableEducationLevels = _educationLevelService.GetAllEducationLevels();
            foreach (var educationLevel in availableEducationLevels)
            {
                items.Add(new SelectListItem { Value = educationLevel.Id.ToString(), Text = educationLevel.Name });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText ?? "ระบุระดับการศึกษา");
        }

        /// <summary>
        /// Prepare available Strategy Groups
        /// </summary>
        /// <param name="items">Strategy Groups items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareStrategyGroups(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available education levels
            var availableStrategyGroups = _strategyGroupService.GetAllStrategyGroups();
            foreach (var strategyGroup in availableStrategyGroups)
            {
                items.Add(new SelectListItem { Value = strategyGroup.Id.ToString(), Text = strategyGroup.Name });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText ?? "ระบุ Strategy Group");
        }

        /// <summary>
        /// Prepare available log levels
        /// </summary>
        /// <param name="items">Log level items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareLogLevels(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available log levels
            var availableLogLevelItems = LogLevel.Debug.ToSelectList(false);
            foreach (var logLevelItem in availableLogLevelItems)
            {
                items.Add(logLevelItem);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public void PrepareFiscalSchedules(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available activity log types
            var availableFiscalSchedules = _fiscalScheduleService.GetAllFiscalSchedules();
            foreach (var fiscalSchedule in availableFiscalSchedules)
            {
                items.Add(new SelectListItem { Value = fiscalSchedule.Id.ToString(), Text = fiscalSchedule.ScholarName });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public void PrepareProgressStatuses(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available order statuses
            var availableStatusItems = ProgressStatus.Started.ToSelectList(false);
            foreach (var statusItem in availableStatusItems)
            {
                items.Add(statusItem);

            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public void PrepareGenders(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available order statuses
            var availableStatusItems = Gender.Male.ToSelectList(false);
            foreach (var statusItem in availableStatusItems)
            {
                items.Add(statusItem);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public void PrepareTitles(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available activity log types
            var availableTitles = _titleService.GetAllTitles();
            foreach (var title in availableTitles)
            {
                items.Add(new SelectListItem { Value = title.Id.ToString(), Text = title.TitleNameTH });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public void PreparePersonalTypes(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available order statuses
            var availableStatusItems = PersonalType.Academic.ToSelectList(false);
            foreach (var statusItem in availableStatusItems)
            {
                items.Add(statusItem);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public void PrepareDegrees(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available order statuses
            var availableStatusItems = Degree.BachelorDegrees.ToSelectList(false);
            foreach (var statusItem in availableStatusItems)
            {
                items.Add(statusItem);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public void PrepareInstitutes(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available activity log types
            var availableInstitutes = _instituteService.GetAllInstitutes();
            foreach (var institute in availableInstitutes)
            {
                items.Add(new SelectListItem { Value = institute.Id.ToString(), Text = institute.Name });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public void PrepareProjectRoles(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available order statuses
            var availableStatusItems = ProjectRole.ProjectManager.ToSelectList(false);
            foreach (var statusItem in availableStatusItems)
            {
                items.Add(statusItem);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public void PrepareProfessorTypes(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available order statuses
            var availableStatusItems = ProfessorType.InternalExpert.ToSelectList(false);
            foreach (var statusItem in availableStatusItems)
            {
                items.Add(statusItem);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public void PrepareActiveStatuses(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available order statuses
            var availableStatusItems = ActiveStatus.Active.ToSelectList(false);
            foreach (var statusItem in availableStatusItems)
            {
                items.Add(statusItem);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public void PrepareFiscalYears(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available fiscalYears;
            
            var availableYears = Enumerable.Range(DateTime.Now.Year-3 + 543, 5).ToList();
            foreach (var year in availableYears)
            {
                items.Add(new SelectListItem { Value = year.ToString(), Text = year.ToString() });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }



        #endregion
    }
}