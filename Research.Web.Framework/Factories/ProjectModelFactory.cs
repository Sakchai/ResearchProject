﻿using System;
using System.Linq;
using Research.Web.Models.Projects;
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
            _baseAdminModelFactory.PrepareResearchIssues(model.AvailableResearchIssues,true, "--ระบุประเด็นการวิจัย--");
            _baseAdminModelFactory.PrepareProfessors(model.AvailableResearchIssues, true, "--ระบุผู้ทรงคุณวุฒิ--");
            _baseAdminModelFactory.PrepareFiscalSchedules(model.AvailableFiscalSchedules,true, "--ระบุปีงบประมาณ--");
            _baseAdminModelFactory.PrepareProjectStatuses(model.AvailableProjectStatuses,true, "--ระบุผลการพิจารณา--");
            return model;
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
                        ProjectNameTh = project.ProjectNameTh,
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
            _baseAdminModelFactory.PrepareFiscalSchedules(searchModel.AvailableFiscalYears,true, "--ปีงบประมาณ--");
            _baseAdminModelFactory.PrepareProjectStatuses(searchModel.AvailableProjectStatuses,true, "--สถานะผลการพิจารณา--");
            _baseAdminModelFactory.PrepareProgressStatuses(searchModel.AvailableProgressStatuses,true, "--สถานะโครงการวิจัย--");
            //prepare page parameters
            searchModel.SetGridPageSize();
            return searchModel;
        }
    }
}