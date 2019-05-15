using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Research.Web.Models.Projects;
using Research.Web.Extensions;
using Research.Data;
using Research.Web.Models.Factories;
using Research.Services.Projects;

namespace Research.Web.Factories
{
    public class ProjectModelFactory : IProjectModelFactory
    {
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IProjectService _projectService;

        public ProjectModelFactory(IBaseAdminModelFactory baseAdminModelFactory, 
            IProjectService projectService)
        {
            this._baseAdminModelFactory = baseAdminModelFactory;
            this._projectService = projectService;
        }

        public ProjectModel PrepareProjectModel(ProjectModel model)
        {
            _baseAdminModelFactory.PrepareResearchIssues(model.AvailableResearchIssues, true, "--ระบุประเด็นการวิจัย--");
            _baseAdminModelFactory.PrepareProfessors(model.AvailableResearchIssues, true, "--ระบุผู้ทรงคุณวุฒิ--");
            _baseAdminModelFactory.PrepareFiscalSchedules(model.AvailableFiscalSchedules, true, "--ระบุปีงบประมาณ--");
            _baseAdminModelFactory.PrepareProjectStatuses(model.AvailableProjectStatuses, true, "--ระบุผลการพิจารณา--");

            return model;
        }

        public ProjectModel PrepareProjectModel(ProjectModel model, Project project, bool v)
        {
            if (project != null)
            {
                //fill in model values from the entity
                model.ProjectCode = project.ProjectCode;
                model.ProjectNameTh = project.ProjectNameTh;
                model.FiscalYear = project.FiscalYear;
                model.StartContractDate = project.ProjectStartDate;
                model.EndContractDate = project.ProjectEndDate;
                model.FundAmount = project.FundAmount;
                model.LastUpdateBy = project.LastUpdateBy;
            }
            _baseAdminModelFactory.PrepareResearchIssues(model.AvailableResearchIssues,true, "--ระบุประเด็นการวิจัย--");
            _baseAdminModelFactory.PrepareProfessors(model.AvailableResearchIssues, true, "--ระบุผู้ทรงคุณวุฒิ--");
            _baseAdminModelFactory.PrepareFiscalSchedules(model.AvailableFiscalSchedules,true, "--ระบุปีงบประมาณ--");
            _baseAdminModelFactory.PrepareProjectStatuses(model.AvailableProjectStatuses,true, "--ระบุผลการพิจารณา--");
            return model;
        }

        protected virtual ProjectResearcherSearchModel PrepareProjectResearcherSearchModel(ProjectResearcherSearchModel searchModel, Project project)
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

            var projects = _projectService.GetAllProjects(pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            var model = new ProjectListModel
            {
                Data = projects.Select(project =>
                {
                    // fill in model values from the entity
                    var projectModel = new ProjectModel
                    {
                        Id = project.Id,
                        ProjectCode = project.ProjectCode,
                        ProjectNameTh = project.ProjectNameTh,
                        StartContractDateName = ConvertToThaiDate(project.ProjectStartDate),
                        ProgressStatusName = project.ProjectProgresses.LastOrDefault() != null ? project.ProjectProgresses.LastOrDefault().ProgressStatus.ToString() : string.Empty,
                        ProjectStatusName = project.ProjectStatus.ToString()
                    };

                    return projectModel;
                }),
                Total = projects.TotalCount
            };
            return model;
        }

        private string ConvertToThaiDate(DateTime date)
        {
            string[] months = new string[] { "มกราคม", "กุมภาพันธ์", "มีนาคม", "เมษายน", "พฤษภาคม", "มิถุนายน", "กรกฎาคม", "สิงหาคม", "กันยายน", "ตุลาคม", "พฤศจิกายน", "ธันวาคม" };
            int day = date.Day;
            int year = date.Year + 543;
            string month = months[date.Month - 1];
            return $"{day} {month} {year}";
        }
        public ProjectSearchModel PrepareProjectSearchModel(ProjectSearchModel searchModel)
        {

            _baseAdminModelFactory.PrepareAgencies(searchModel.AvailableAgencies,true, "--หน่วยงาน--");
            _baseAdminModelFactory.PrepareFiscalSchedules(searchModel.AvailableFiscalYears,true, "--ปีงบประมาณ--");
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
                        RoleName = x.ProjectRole.ToString(),
                        ResearcherId = x.ResearcherId,
                        ProjectRoleId = x.ProjectRoleId,
                        ResearcherName = $"{x.Researcher.FirstName} {x.Researcher.LastName}"
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
                model.ProjectCode = project.ProjectCode;
                model.ProjectNameTh = project.ProjectNameTh;
                model.FiscalYear = project.FiscalYear;
                model.StartContractDate = project.ProjectStartDate;
                model.EndContractDate = project.ProjectEndDate;
                model.FundAmount = project.FundAmount;
                model.LastUpdateBy = project.LastUpdateBy;
            }
            _baseAdminModelFactory.PrepareResearchIssues(model.AvailableResearchIssues, true, "--ระบุประเด็นการวิจัย--");
            _baseAdminModelFactory.PrepareProfessors(model.AvailableResearchIssues, true, "--ระบุผู้ทรงคุณวุฒิ--");
            _baseAdminModelFactory.PrepareFiscalSchedules(model.AvailableFiscalSchedules, true, "--ระบุปีงบประมาณ--");
            _baseAdminModelFactory.PrepareProjectStatuses(model.AvailableProjectStatuses, true, "--ระบุผลการพิจารณา--");
            return model;
        }
    }
}