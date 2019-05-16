using Microsoft.AspNetCore.Mvc;
using Research.Controllers;
using Research.Data;
using Research.Services;
using Research.Services.Logging;
using Research.Services.Projects;
using Research.Web.Extensions;
using Research.Web.Factories;
using Research.Web.Framework.Mvc;
using Research.Web.Framework.Mvc.Filters;
using Research.Web.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Research.Web.Controllers
{
    public class ProjectController : BaseAdminController
    {
        private readonly IUserActivityService _userActivityService;
        private readonly IProjectModelFactory _projectModelFactory;
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService, 
            IProjectModelFactory projectModelFactory,
            IUserActivityService userActivityService)
        {
            _projectService = projectService;
            _projectModelFactory = projectModelFactory;
            _userActivityService = userActivityService;
        }
        // GET: /<controller>/
        //[AllowAnonymous]
        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }


        public IActionResult Edit(int id)
        {

            //try to get a project with the specified id
            var project = _projectService.GetProjectById(id);
            if (project == null)
                return RedirectToAction("List");

            //prepare model
            var model = _projectModelFactory.PrepareProjectModel(new ProjectModel(), project);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(ProjectModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProjects))
            //    return AccessDeniedView();

            //try to get a project with the specified id
            var project = _projectService.GetProjectById(model.Id);
            if (project == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                project = model.ToEntity(project);

                _projectService.UpdateProject(project);

                SuccessNotification("Project Updated");

                //activity log
                _userActivityService.InsertActivity("EditProject", "ActivityLog EditProject", project);

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = project.Id });
            }

            //prepare model
            model = _projectModelFactory.PrepareProjectModel(model, project);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet, ActionName("Create")]
        public IActionResult Create()
        {
            var model = _projectModelFactory.PrepareProjectModel(new ProjectModel(),null);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ProjectModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProjects))
            //    return AccessDeniedView();

            if (ModelState.IsValid)
            {

                var project = model.ToEntity<Project>();

                _projectService.InsertProject(project);

                SuccessNotification("Admin.ContentManagement.Projects.Added");

                //activity log
                _userActivityService.InsertActivity("AddNewProject", "ActivityLog.AddNewProject", project);

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = project.Id });
            }

            //prepare model
            model = _projectModelFactory.PrepareProjectModel(model, null);

            //if we got this far, something failed, redisplay form
            return View(model);
        }
        public virtual IActionResult List()
        {

            //prepare model
            var model = _projectModelFactory.PrepareProjectSearchModel(new ProjectSearchModel());

            return View(model);
        }

        public virtual IActionResult Info(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProjects))
            //    return AccessDeniedView();

            //try to get a project with the specified id
            var project = _projectService.GetProjectById(id);
            if (project == null)
                return RedirectToAction("List");

            //prepare model
            var model = _projectModelFactory.PrepareProjectModel(null, project);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProjects))
            //    return AccessDeniedView();

            //try to get a project with the specified id
            var project = _projectService.GetProjectById(id);
            if (project == null)
                return RedirectToAction("List");
            project.Deleted = true;
            _projectService.UpdateProject(project);

            SuccessNotification("Projects Deleted");

            //activity log
            _userActivityService.InsertActivity("DeleteProject", "ActivityLog.DeleteProject", project);

            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult List(ProjectSearchModel searchModel)
        {
            var model = _projectModelFactory.PrepareProjectListModel(searchModel);
            return Json(model);
        }

        #region project projects
        [HttpPost]
        public virtual IActionResult ProjectResearchersSelect(ProjectResearcherSearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
            //    return AccessDeniedKendoGridJson();

            //try to get a project with the specified id
            var project = _projectService.GetProjectById(searchModel.ProjectId);
            //?? throw new ArgumentException("No project found with the specified id");

            //prepare model
            if (project != null)
            {
                var model = _projectModelFactory.PrepareProjectResearcherListModel(searchModel, project);

                return Json(model);
            }
            else
                return Json(new ProjectResearcherListModel());
        }

        public virtual IActionResult ProjectResearcherAdd(int projectId, int researcherId,
            int roleId, int portion)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
            //    return AccessDeniedView();

            //try to get a project with the specified id
            var project = _projectService.GetProjectById(projectId);
            if (project == null)
                return Json(new { Result = false });

            var projectResearcher = new ProjectResearcher
            {
                ProjectId = project.Id,
                ResearcherId = researcherId,
                ProjectRoleId = roleId,
                Portion = portion
            };
            _projectService.InsertProjectResearcher(projectResearcher);
            //  project.ProjectResearchers.Add(projectResearcher);
            //  _projectService.UpdateResearcher(project);

            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual IActionResult ProjectResearcherDelete(int id, int projectId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
            //    return AccessDeniedView();

            //try to get a project with the specified id
            var project = _projectService.GetProjectById(projectId)
                ?? throw new ArgumentException("No project found with the specified id", nameof(projectId));

            //try to get a project education with the specified id
            var projectResearcher = project.ProjectResearchers.FirstOrDefault(vn => vn.Id == id)
                ?? throw new ArgumentException("No project researcher found with the specified id", nameof(id));

            _projectService.RemoveProjectResearcher(project, projectResearcher);

            return new NullJsonResult();
        }
        #endregion


        #region project professor
        [HttpPost]
        public virtual IActionResult ProjectProfessorsSelect(ProjectProfessorSearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProfessors))
            //    return AccessDeniedKendoGridJson();

            //try to get a project with the specified id
            var project = _projectService.GetProjectById(searchModel.ProjectId);
            //?? throw new ArgumentException("No project found with the specified id");

            //prepare model
            if (project != null)
            {
                var model = _projectModelFactory.PrepareProjectProfessorListModel(searchModel, project);

                return Json(model);
            }
            else
                return Json(new ProjectProfessorListModel());
        }

        public virtual IActionResult ProjectProfessorAdd(int projectId, int professorId,
            int professorTypeId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProfessors))
            //    return AccessDeniedView();

            //try to get a project with the specified id
            var project = _projectService.GetProjectById(projectId);
            if (project == null)
                return Json(new { Result = false });

            var projectProfessor = new ProjectProfessor
            {
                ProjectId = project.Id,
                ProfessorTypeId = professorTypeId,
                ProfessorId = professorId

            };
            _projectService.InsertProjectProfessor(projectProfessor);
            //  project.ProjectProfessors.Add(projectProfessor);
            //  _projectService.UpdateProfessor(project);

            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual IActionResult ProjectProfessorDelete(int id, int projectId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProfessors))
            //    return AccessDeniedView();

            //try to get a project with the specified id
            var project = _projectService.GetProjectById(projectId)
                ?? throw new ArgumentException("No project found with the specified id", nameof(projectId));

            //try to get a project professor with the specified id
            var projectProfessor = project.ProjectProfessors.FirstOrDefault(vn => vn.Id == id)
                ?? throw new ArgumentException("No project researcher found with the specified id", nameof(id));

            _projectService.RemoveProjectProfessor(project, projectProfessor);

            return new NullJsonResult();
        }
        #endregion
    }
}
