using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Domain;
using Project.Domain.Service;
using Project.Web.Models.ProjectViewModels;
using System.Collections.Generic;
using System;
using Project.Enum;
using System.Linq;

namespace Project.Web.Factories
{
    public class ProjectModelFactory : IProjectModelFactory
    {
        private readonly ProjectResearcherService _projectResearcherService;

        public ProjectModelFactory(ProjectResearcherService projectResearcherService)
        {
            _projectResearcherService = projectResearcherService;
        }

        public CreateVm PrepareProjectCreateModel(CreateVm model)
        {
            model.AvailableFiscalSchedules.Add(new SelectListItem { Text = "--โปรดระบุปีงบประมาณ--", Value = "", Selected = true });
            model.AvailableProfessors.Add(new SelectListItem { Text = "--โปรดระบุผู้ทรงคุณวุฒิ--", Value = "", Selected = true });
            model.AvailableResearchIssues.Add(new SelectListItem { Text = "--โปรดระบุประเด็นการวิจัย--", Value = "", Selected = true });
            return model;
        }

        public ModifyVm PrepareProjectEditModel(ModifyVm model, ProjectModel project)
        {
            if (project != null)
            {
                //fill in model values from the entity
                model = model ?? new ModifyVm();
                model.ProjectId = project.Id;
                model.ProjectCode = project.ProjectCode;
                model.ProjectNameTh = project.ProjectNameTh;
                model.FiscalYear = project.FiscalYear;
                model.StartContractDate = project.StartContractDate;
                model.EndContractDate = project.EndContractDate;
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
                        Text = StringEnum.GetStringValue((ProjectStatus)value),
                        Value = value.ToString()
                    });
                }
            }
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
                    Text = StringEnum.GetStringValue((ProjectStatus)value),
                    Value = value.ToString()
                });
            }

            return searchModel;
        }
    }
}