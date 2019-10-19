using System;
using System.Linq;
using Research.Web.Models.Projects;
using Research.Web.Extensions;
using Research.Data;
using Research.Web.Models.Factories;
using Research.Services.Projects;
using Research.Enum;
using System.Runtime.Serialization;
using Research.Core;
using Research.Services.Media;

namespace Research.Web.Factories
{
    public class ProjectModelFactory : IProjectModelFactory
    {
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IProjectService _projectService;
        private readonly IDownloadService _downloadService;
        private readonly IWorkContext _workContext;

        public ProjectModelFactory(IBaseAdminModelFactory baseAdminModelFactory, 
            IProjectService projectService,
            IDownloadService downloadService,
            IWorkContext workContext)
        {
            this._baseAdminModelFactory = baseAdminModelFactory;
            this._projectService = projectService;
            this._downloadService = downloadService;
            this._workContext = workContext;
        }

        public virtual ProjectResearcherSearchModel PrepareProjectResearcherSearchModel(ProjectResearcherSearchModel searchModel, Project project)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (project == null)
                throw new ArgumentNullException(nameof(project));

            searchModel.ProjectId = project.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public virtual ProjectProfessorSearchModel PrepareProjectProfessorSearchModel(ProjectProfessorSearchModel searchModel, Project project)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (project == null)
                throw new ArgumentNullException(nameof(project));

            searchModel.ProjectId = project.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }
        /// <summary>
        /// Prepare paged project list model
        /// </summary>
        /// <param name="searchModel">Project search model</param>
        /// <returns>Project list model</returns>
        public ProjectListModel PrepareProjectListModel(ProjectSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            string userName = string.Empty;
            if (_workContext.CurrentUser.UserTypeId == 2) //Researchers Role will see only owner projects
                userName = _workContext.CurrentUser.UserName;
            var projects = _projectService.GetAllProjects(projectNameTH:searchModel.SearchProjectName, 
                                        fiscalYear:searchModel.FiscalScheduleId,
                                        projectStatusId: searchModel.ProjectStatusId,
                                        agencyId: searchModel.AgencyId,
                                        progressStatusId: searchModel.ProgressStatusId,
                                        createdBy: userName,
                                        pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            var model = new ProjectListModel
            {
                Data = projects.Select(project =>
                {
                    // fill in model values from the entity

                    var projectModel = new ProjectModel
                    {
                        Id = project.Id,
                        FiscalYear = project.FiscalYear,
                        ProjectCode = project.ProjectCode,
                        ProjectNameTh = project.ProjectNameTh,
                        ProjectStartDateName  = CommonHelper.ConvertToThaiDate(project.ProjectStartDate),
                        ProgressStatusName = project.ProjectProgresses.LastOrDefault() != null ? project.ProjectProgresses.LastOrDefault().ProgressStatus.GetAttributeOfType<EnumMemberAttribute>().Value : string.Empty,
                        ProjectStatusName = (int) project.ProjectStatus != 0 ? project.ProjectStatus.GetAttributeOfType<EnumMemberAttribute>().Value : string.Empty,
                    };

                    return projectModel;
                }),
                Total = projects.TotalCount
            };
            return model;
        }


        public ProjectSearchModel PrepareProjectSearchModel(ProjectSearchModel searchModel)
        {
            _baseAdminModelFactory.PrepareAgencies(searchModel.AvailableAgencies,true, "--หน่วยงาน--");
            _baseAdminModelFactory.PrepareFiscalYears(searchModel.AvailableFiscalYears,true, "--ปีงบประมาณ--");
            _baseAdminModelFactory.PrepareProjectStatuses(searchModel.AvailableProjectStatuses,true, "--สถานะผลการพิจารณา--");
            _baseAdminModelFactory.PrepareProgressStatuses(searchModel.AvailableProgressStatuses,true, "--สถานะโครงการวิจัย--");
            //prepare page parameters
            searchModel.SetGridPageSize();
            return searchModel;
        }

        public ProjectResearcherListModel PrepareProjectResearcherListModel(ProjectResearcherSearchModel searchModel, Project project)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (project == null)
                throw new ArgumentNullException(nameof(project));

            //get researcher educations
            //chai
            //var researcherEducations = researcher.ResearcherEducations.OrderByDescending(edu => edu.Degree).ToList();
            var projectResearchers = _projectService.GetAllProjectResearchers(project.Id).ToList();
            //prepare list model
            var model = new ProjectResearcherListModel
            {
                Data = projectResearchers.PaginationByRequestModel(searchModel).Select(x =>
                {
                    //fill in model values from the entity       
                    var projectResearcherModel = new ProjectResearcherModel
                    {
                        Id = x.Id,
                        Portion = x.Portion,
                        ProjectId = x.ProjectId,
                        RoleName = (int)x.ProjectRole != 0 ? x.ProjectRole.GetAttributeOfType<EnumMemberAttribute>().Value : string.Empty,
                        ResearcherId = x.ResearcherId,
                        ProjectRoleId = x.ProjectRoleId,
                        ResearcherName = x.ResearcherName,
  
                    };
                    return projectResearcherModel;
                }),
                Total = projectResearchers.Count
            };

            return model;
        }

  

        public ProjectModel PrepareProjectModel(ProjectModel model, Project project)
        {
            if (project != null)
            {
                //fill in model values from the entity

                model = model ?? project.ToModel<ProjectModel>();
                model.Id = project.Id;
                model.ProjectCode = project.ProjectCode;
                model.FiscalYear = project.FiscalYear;
                model.ProjectNameTh = project.ProjectNameTh;
                model.ProjectNameEn = project.ProjectNameEn;
                model.PlanNameTh = project.PlanNameTh;
                model.PlanNameEn = project.PlanNameEn;
                model.ProjectType = project.ProjectType;
                model.FundAmount = project.FundAmount;
                model.StrategyGroupId = project.StrategyGroupId;
                model.StrategyGroupName = project.StrategyGroup.Name;
                if (project.ProjectStatusId != null)
                    model.ProjectStatusId = project.ProjectStatusId.Value;
                if (project.ProjectStartDate != null)
                {
                    model.ProjectStartDateName = CommonHelper.ConvertToThaiDate(project.ProjectStartDate);
                    model.ProjectStartDate = project.ProjectStartDate.AddYears(543);
                }
                if (project.ProjectEndDate != null)
                {

                    model.ProjectEndDateName = CommonHelper.ConvertToThaiDate(project.ProjectEndDate);
                    model.ProjectEndDate = project.ProjectEndDate.AddYears(543);
                }
                model.LastUpdateBy = project.LastUpdateBy;
                model.Comment = project.Comment;
                model.ResearchIssueId = project.ResearchIssueId.Value;
                model.ResearchIssueName = project.ResearchIssue != null ? project.ResearchIssue.Name : string.Empty;
                var projectProgress = project.ProjectProgresses.OrderByDescending(x=> x.Id).LastOrDefault();
                model.StrategyGroupName = project.StrategyGroup != null ? project.StrategyGroup.Name : string.Empty;
                model.ProjectTypeName = project.ProjectType.Equals("N") ? "โครงการวิจัยใหม่" : "โครงการวิจัยต่อเนื่อง";
                model.ProjectStatusName = project.ProjectStatus.GetAttributeOfType<EnumMemberAttribute>().Value;
                model.ProgressStatusName = projectProgress != null ? projectProgress.ProgressStatus.GetAttributeOfType<EnumMemberAttribute>().Value : string.Empty;
                model.CreatedName = CommonHelper.ConvertToThaiDate(project.Created);
                model.ProjectResearcherListModel = PrepareProjectResearcherListModel(new ProjectResearcherSearchModel { ProjectId = project.Id }, project);
                model.ProjectProfessorListModel = PrepareProjectProfessorListModel(new ProjectProfessorSearchModel { ProjectId = project.Id }, project);

            } else
            {
                model.ProjectCode = _projectService.GetNextNumber();
                model.ProjectType = "N";
                model.ProjectStartDate = DateTime.Today.AddYears(543);
                model.ProjectEndDate = DateTime.Today.AddYears(543);
                model.ProjectStatusId = (int) ProjectStatus.WaitingApproval;
            }

            model.AddProjectProgressStartDate = DateTime.Today.AddYears(543);
            model.AddProjectProgressEndDate = DateTime.Today.AddYears(543);

            int fiscalYear = DateTime.Today.Year + 543;

            _baseAdminModelFactory.PrepareResearchIssues(model.AvailableResearchIssues, fiscalYear, true, "--ระบุประเด็นการวิจัย--");
            _baseAdminModelFactory.PrepareProjectStatuses(model.AvailableProjectStatuses, true, "--ระบุผลการพิจารณา--");

            _baseAdminModelFactory.PrepareResearchers(model.AvailableResearchers, true, "--ระบุผู้วิจัย--");
            _baseAdminModelFactory.PrepareProjectRoles(model.AvailableProjectRoles, true, "--ระบุบทบาทในโครงการ--");

            _baseAdminModelFactory.PrepareProfessors(model.AvailableProfessors, true, "--ระบุผู้ทรงคุณวุฒิ--");
            _baseAdminModelFactory.PrepareProfessorTypes(model.AvailableProfessorTypes, true, "--ระบุประเภทผู้ทรงคุณวุฒิ--");

            _baseAdminModelFactory.PrepareProgressStatuses(model.AvailableProgressStatuses, true, "--ระบุสถานะโครงการวิจัย--");
            _baseAdminModelFactory.PrepareStrategyGroups(model.AvailableStrategyGroups, true, "--ระบุรหัสยุทธศาสตร์--");
            return model;
        }

        public ProjectProfessorListModel PrepareProjectProfessorListModel(ProjectProfessorSearchModel searchModel, Project project)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (project == null)
                throw new ArgumentNullException(nameof(project));

            //get researcher educations
            //chai
            //var researcherEducations = researcher.ResearcherEducations.OrderByDescending(edu => edu.Degree).ToList();
            var projectProfessors = _projectService.GetAllProjectProfessors(project.Id).ToList();
            //prepare list model
            var model = new ProjectProfessorListModel
            {
                Data = projectProfessors.PaginationByRequestModel(searchModel).Select(x =>
                {
                    //fill in model values from the entity  
                   // var title = (x.Professor.Title != null) ? x.Professor.Title.TitleNameTH : string.Empty;
                   // var fullName = (x.Professor != null) ? $"{title}{x.Professor.FirstName} {x.Professor.LastName}" : string.Empty;
                    var projectProfessorModel = new ProjectProfessorModel
                    {
                        Id = x.Id,
                        ProjectId = x.ProjectId,
                        ProfessorId = x.ProfessorId,
                        ProfessorTypeId = x.ProfessorTypeId,
                        ProfessorTypeName = x.ProfessorType.GetAttributeOfType<EnumMemberAttribute>().Value,
                        ProfessorName = x.ProfessorName,
                    };



                    return projectProfessorModel;
                }),
                Total = projectProfessors.Count
            };

            return model;
        }

        public ProjectProgressSearchModel PrepareProjectProgressSearchModel(ProjectProgressSearchModel searchModel, Project project)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (project == null)
                throw new ArgumentNullException(nameof(project));

            searchModel.ProjectId = project.Id;
            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public ProjectProgressListModel PrepareProjectProgressListModel(ProjectProgressSearchModel searchModel, Project project)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (project == null)
                throw new ArgumentNullException(nameof(project));

            //get researcher educations
            //chai
            //var researcherEducations = researcher.ResearcherEducations.OrderByDescending(edu => edu.Degree).ToList();
            var projectProgresses = _projectService.GetAllProjectProgresses(project.Id).ToList();
            //prepare list model
            var model = new ProjectProgressListModel
            {
                Data = projectProgresses.PaginationByRequestModel(searchModel).Select(x =>
                {
                    //fill in model values from the entity       
                    string guid = x.ProjectUploadId != 0 ? _downloadService.GetDownloadById(x.ProjectUploadId.Value).DownloadGuid.ToString() : string.Empty;
                    var projectProfessorModel = new ProjectProgressModel
                    {
                        Id = x.Id,
                        ProjectId = x.ProjectId,
                        Comment = x.Comment,
                        LastUpdateBy = x.LastUpdateBy,
                        ModifiedName = CommonHelper.ConvertToThaiDate(x.Modified),
                        ProgressStartDateName = CommonHelper.ConvertToThaiDate(x.ProgressStartDate),
                        ProgressEndDateName = CommonHelper.ConvertToThaiDate(x.ProgressEndDate),
                        ProgressStatusName = x.ProgressStatus.GetAttributeOfType<EnumMemberAttribute>().Value,
                        DownloadGuid = guid,
                    };

                    return projectProfessorModel;
                }),
                Total = projectProgresses.Count
            };

            return model;
        }
    }
}