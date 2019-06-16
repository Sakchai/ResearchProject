using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Research.Controllers;
using Research.Core;
using Research.Data;
using Research.Enum;
using Research.Services;
using Research.Services.FiscalSchedules;
using Research.Services.Logging;
using Research.Services.Projects;
using Research.Web.Extensions;
using Research.Web.Factories;
using Research.Web.Framework.Mvc;
using Research.Web.Framework.Mvc.Filters;
using Research.Web.Models.Projects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Research.Web.Controllers
{
    public class ProjectController : BaseAdminController
    {
        private readonly IUserActivityService _userActivityService;
        private readonly IProjectModelFactory _projectModelFactory;
        private readonly IProjectService _projectService;
        private readonly IWorkContext _workContext;
        private readonly IFiscalScheduleService _fiscalScheduleService;

        public ProjectController(IProjectService projectService, 
            IProjectModelFactory projectModelFactory,
            IUserActivityService userActivityService,
            IWorkContext workContext,
            IFiscalScheduleService fiscalScheduleService)
        {
            this._projectService = projectService;
            this._projectModelFactory = projectModelFactory;
            this._userActivityService = userActivityService;
            this._workContext = workContext;
            this._fiscalScheduleService = fiscalScheduleService;
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

            int fiscalYear = DateTime.Now.Year + 543;
            var fiscalSchedule = _fiscalScheduleService.GetAllFiscalSchedules(fiscalScheduleName: string.Empty,
                                  fiscalYear: fiscalYear)
                                  .Where(x => x.ClosingDate >= DateTime.Today &&
                                       x.OpeningDate <= DateTime.Today)
                                  .OrderByDescending(x => x.OpeningDate).FirstOrDefault();

            if (fiscalSchedule == null)
                ModelState.AddModelError("", "ไม่พบช่วงเวลา วันเปิดรับข้อเสนอโครงการวิจัย โปรดติดต่อผู้ดูแลระบบ");

            project = model.ToEntity(project);

            if (project.ProjectStartDate <= fiscalSchedule.OpeningDate)
                ModelState.AddModelError("", "วันที่ยื่นข้อเสนอโครงการ น้อยกว่าวันเปิดรับข้อเสนอโครงการวิจัย.");
            
            if (project.ProjectEndDate >= fiscalSchedule.ClosingDate)
                ModelState.AddModelError("", "วันสุดท้ายยื่นข้อเสนอโครงการ มากกว่าวันปิดรับข้อเสนอโครงการวิจัย.");


            if (ModelState.IsValid)
            {
                

                _projectService.UpdateProject(project);

                SuccessNotification("Project Updated");

                //activity log
                //_userActivityService.InsertActivity("EditProject", "ActivityLog EditProject", project);

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

            int fiscalYear = DateTime.Now.Year + 543;
            var fiscalSchedule = _fiscalScheduleService.GetAllFiscalSchedules(fiscalScheduleName: string.Empty,
                                  fiscalYear: fiscalYear)
                                  .Where(x => x.ClosingDate <= DateTime.Today &&
                                       x.OpeningDate >= DateTime.Today)
                                  .OrderByDescending(x => x.OpeningDate).FirstOrDefault();

            if (fiscalSchedule == null)
                ModelState.AddModelError("", "ไม่พบช่วงเวลา วันเปิดรับข้อเสนอโครงการวิจัย โปรดติดต่อผู้ดูแลระบบ");

            if (ModelState.IsValid)
            {
                var user = _workContext.CurrentUser;
                var project = model.ToEntity<Project>();
                project.ProjectType = string.IsNullOrEmpty(model.ProjectType) ? "N" : model.ProjectType;
                project.ProjectStartDate = fiscalSchedule.OpeningDate;
                project.ProjectEndDate = fiscalSchedule.ClosingDate;
                project.Created = DateTime.UtcNow;
                project.Modified = DateTime.UtcNow;
                project.LastUpdateBy = user.UserName;
                project.ProjectStatusId = (int) ProjectStatus.WaitingApproval;
                _projectService.InsertProject(project);

                var researcher = user.Researcher;

                if (researcher != null)
                {
                    var projectResearcher = new ProjectResearcher
                    {
                        ProjectId = project.Id,
                        FirstName = researcher.FirstName,
                        LastName = researcher.LastName,
                        Portion = 100,
                        ProjectRoleId = (int)ProjectRole.ProjectManager,
                        ResearcherId = researcher.Id
                    };
                    _projectService.InsertProjectResearcher(projectResearcher);
                }

                SuccessNotification("Admin.ContentManagement.Projects.Added");

                //activity log
                //_userActivityService.InsertActivity("AddNewProject", "ActivityLog.AddNewProject", project);

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
            //_userActivityService.InsertActivity("DeleteProject", "ActivityLog.DeleteProject", project);

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

            ////try to get a project education with the specified id
            //var projectResearcher = project.ProjectResearchers.FirstOrDefault(vn => vn.Id == id)
            //    ?? throw new ArgumentException("No project researcher found with the specified id", nameof(id));
            var projectResearcher = _projectService.GetProjectResearchersById(id)
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

        #region project progress
        [HttpPost]
        public virtual IActionResult ProjectProgressesSelect(ProjectProgressSearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProgresss))
            //    return AccessDeniedKendoGridJson();

            //try to get a project with the specified id
            var project = _projectService.GetProjectById(searchModel.ProjectId);
            //?? throw new ArgumentException("No project found with the specified id");

            //prepare model
            if (project != null)
            {
                var model = _projectModelFactory.PrepareProjectProgressListModel(searchModel, project);

                return Json(model);
            }
            else
                return Json(new ProjectProgressListModel());
        }

        public virtual IActionResult ProjectProgressAdd(int projectId, int progressStatusId,
            string startDate, string endDate, string comment, int uploadId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProgresss))
            //    return AccessDeniedView();

            //try to get a project with the specified id
            var project = _projectService.GetProjectById(projectId);
            if (project == null)
                return Json(new { Result = false });

            var projectProgress = new ProjectProgress
            {
                ProjectId = project.Id,
                ProgressStatusId = progressStatusId,
                ProgressStartDate = DateTime.Parse(startDate),
                ProgressEndDate = DateTime.Parse(endDate),
                Comment = comment,
                Modified = DateTime.Now,
                ProjectUploadId = uploadId
            };
            _projectService.InsertProjectProgress(projectProgress);
            //  project.ProjectProgresss.Add(projectProgress);
            //  _projectService.UpdateProgress(project);

            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual IActionResult ProjectProgressDelete(int id, int projectId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProgresss))
            //    return AccessDeniedView();

            //try to get a project with the specified id
            var project = _projectService.GetProjectById(projectId)
                ?? throw new ArgumentException("No project found with the specified id", nameof(projectId));

            //try to get a project professor with the specified id
            var projectProgress = project.ProjectProgresses.FirstOrDefault(vn => vn.Id == id)
                ?? throw new ArgumentException("No project researcher found with the specified id", nameof(id));

            _projectService.RemoveProjectProgress(project, projectProgress);

            return new NullJsonResult();
        }
        #endregion

        #region Export / Import

        public virtual IActionResult ExportPdf(int id, int projectId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageLanguages))
            //    return AccessDeniedView();

            var project = _projectService.GetProjectById(projectId)
                ?? throw new ArgumentException("No project found with the specified id", nameof(projectId));

            //try to get a project professor with the specified id
            var projectProgress = project.ProjectProgresses.FirstOrDefault(vn => vn.Id == id)
                ?? throw new ArgumentException("No project progress found with the specified id", nameof(id));

            if (projectProgress == null)
                return RedirectToAction("List");

            try
            {
                var pdf = _projectService.ExportResourcesToPdf(projectProgress);
                return File(Encoding.UTF8.GetBytes(pdf), "application/pdf", "project_progress.pdf");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public virtual IActionResult ImportPdf(int id, int projectId, IFormFile importpdffile)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageLanguages))
            //    return AccessDeniedView();

            //try to get a language with the specified id
            var project = _projectService.GetProjectById(projectId)
                ?? throw new ArgumentException("No project found with the specified id", nameof(projectId));

            //try to get a project professor with the specified id
            var projectProgress = project.ProjectProgresses.FirstOrDefault(vn => vn.Id == id)
                ?? throw new ArgumentException("No project progress found with the specified id", nameof(id));

            if (projectProgress == null)
                return RedirectToAction("List");

            try
            {
                if (importpdffile != null && importpdffile.Length > 0)
                {
                    using (var sr = new StreamReader(importpdffile.OpenReadStream(), Encoding.UTF8))
                    {
                        _projectService.ImportResourcesFromPdf(projectProgress, sr.ReadToEnd());
                    }
                }
                else
                {
                    ErrorNotification("Admin.Common.UploadFile");
                    return RedirectToAction("Edit", new { id = projectProgress.Id, projectId = project.Id });
                }

                SuccessNotification("Admin.Configuration.Languages.Imported");
                return RedirectToAction("Edit", new { id = projectProgress.Id, projectId = project.Id });
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = projectProgress.Id, projectId = project.Id });
            }
        }

        #endregion
    }
}
