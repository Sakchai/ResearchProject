using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Domain;
using Project.Domain.Service;
using Project.Web.Models.ProjectViewModels;

namespace Project.Web.Factories
{
    public class ProjectModelFactory : IProjectModelFactory
    {
        private readonly ProjectService _projectService;

        public ProjectModelFactory(ProjectService projectService)
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

        public ModifyVm PrepareProjectEditModel(ModifyVm model, ProjectModel project)
        {
            if (project != null)
            {
                //fill in model values from the entity
                model = model ?? new ModifyVm();
                model.AvailableFiscalSchedules.Add(new SelectListItem { Text = "--โปรดระบุปีงบประมาณ--", Value = "", Selected = true });
                model.AvailableProfessors.Add(new SelectListItem { Text = "--โปรดระบุผู้ทรงคุณวุฒิ--", Value = "", Selected = true });
                model.AvailableResearchIssues.Add(new SelectListItem { Text = "--โปรดระบุประเด็นการวิจัย--", Value = "", Selected = true });
                model.AvailableProjectStatuses.Add(new SelectListItem { Text = "--โปรดระบุสถานะโครงการ--", Value = "", Selected = true });
            }
            return model;
        }

        public ProjectSearchViewModel PrepareProjectSearchModel(ProjectSearchViewModel searchModel)
        {
            searchModel.AvailableFaculties.Add(new SelectListItem { Text = "--หน่วยงานหลัก--", Value = "", Selected = true });
            searchModel.AvailableFiscalYears.Add(new SelectListItem { Text = "--ปีงบประมาณ--", Value = "", Selected = true });
            searchModel.AvailablePageSizes = "10";
            searchModel.AvailableProjectStatuses.Add(new SelectListItem { Text = "--สถานะโครงการ--", Value = "", Selected = true });
            searchModel.AvailableResearchStatuses.Add(new SelectListItem { Text = "--สถานะโครงการวิจัย--", Value = "", Selected = true });

            return searchModel;
        }
    }
}