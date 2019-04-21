using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Research.Web.Models.Projects;
using Research.Enum;
using Research.Services;
using Research.Data;
using Research.Domain;


namespace Research.Web.Factories
{
    public class ProjectModelFactory : IProjectModelFactory
    {
        private readonly IProjectService _projectResearcherService;
        private readonly IProjectService _projectService;

        public ProjectModelFactory(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public CreateVm PrepareProjectCreateModel(CreateVm model)
        {
            model.AvailableFiscalSchedules.Add(new SelectListItem { Text = "--โปรดระบุปีงบประมาณ--", Value = "", Selected = true });
            model.AvailableProfessors.Add(new SelectListItem { Text = "--โปรดระบุผู้ทรงคุณวุฒิ--", Value = "", Selected = true });
            model.AvailableResearchIssues.Add(new SelectListItem { Text = "--โปรดระบุประเด็นการวิจัย--", Value = "", Selected = true });
            return model;
        }

        public ModifyVm PrepareProjectEditModel(ModifyVm model, Project project)
        {
            if (project != null)
            {
                //fill in model values from the entity
                model = model ?? new ModifyVm();
                model.ProjectId = project.Id;
                model.ProjectCode = project.ProjectCode;
                model.ProjectNameTh = project.ProjectNameTh;
                model.FiscalYear = project.FiscalYear;
                model.StartContractDate = project.ProjectStartDate;
                model.EndContractDate = project.ProjectEndDate;
                model.FundAmount = project.FundAmount;
                model.Modified = project.Modified;
                model.LastUpdateBy = project.LastUpdateBy;
                model.AvailableProfessors.Add(new SelectListItem { Selected = false, Text = "--ระบุผู้ทรงคุณวุฒิ--", Value = ""});
                model.AvailableResearchIssues.Add(new SelectListItem { Selected = false, Text = "--ระบุประเด็นการวิจัย--", Value = ""});
                model.AvailableProjectStatuses.Add(new SelectListItem { Selected=false, Text = "--ระบุสถานะโครงการ--", Value = ""});
                var projectStatusOptionsIds = System.Enum.GetValues(typeof(ProjectStatus)).Cast<int>().ToList();

                foreach (var value in projectStatusOptionsIds)
                {
                    model.AvailableProjectStatuses.Add(new SelectListItem
                    {
                        Selected = project.ProjectStatusId == value,
                       // Text = StringEnum.GetStringValue((ProjectStatus)value),
                        Value = value.ToString()
                    });
                }
            }
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
            searchModel.AvailableFaculties.Add(new SelectListItem { Text = "--หน่วยงานหลัก--", Value = "", Selected = true });
            searchModel.AvailableFiscalYears.Add(new SelectListItem { Text = "--ปีงบประมาณ--", Value = "", Selected = true });
            searchModel.AvailablePageSizes = "10";
            searchModel.AvailableResearchStatuses.Add(new SelectListItem { Text = "--สถานะโครงการวิจัย--", Value = "", Selected = true });
            searchModel.AvailableProjectStatuses.Add(new SelectListItem { Text = "--สถานะโครงการ--", Value = "", Selected = true });
            var projectStatusOptionsIds = System.Enum.GetValues(typeof(ProjectStatus)).Cast<int>().ToList();

            foreach (var value in projectStatusOptionsIds)
            {
                searchModel.AvailableProjectStatuses.Add(new SelectListItem
                {
                   //Text = StringEnum.GetStringValue((ProjectStatus)value),
                    Value = value.ToString()
                });
            }

            return searchModel;
        }
    }
}