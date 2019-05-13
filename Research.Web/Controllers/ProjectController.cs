using Microsoft.AspNetCore.Mvc;
using Research.Controllers;
using Research.Services;
using Research.Services.Projects;
using Research.Web.Factories;
using Research.Web.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Research.Web.Controllers
{
    public class ProjectController : BaseAdminController
    {
        private readonly IProjectModelFactory _projectModelFactory;
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService
                                , IProjectModelFactory projectModelFactory)
        {
            _projectService = projectService;
            _projectModelFactory = projectModelFactory;
        }
        // GET: /<controller>/
        //[AllowAnonymous]
        public IActionResult Index()
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

        public IActionResult Create()
        {
            AddPageHeader("เพิ่มข้อเสนอโครงการวิจัย", "");
            var model = _projectModelFactory.PrepareProjectModel(new ProjectModel(),null);
            return View(model);
        }
        public virtual IActionResult List()
        {
            AddPageHeader("ข้อเสนอโครงการวิจัย", "");
            //prepare model
            var model = _projectModelFactory.PrepareProjectSearchModel(new ProjectSearchModel());

            return View(model);
        }
        [HttpPost]
        public IActionResult ExportExcelSelected(string selectedIds)
        {

            var projects = new List<Research.Data.Project>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x))
                    .ToArray();
                //projects.AddRange(_projectService.GetProjectsByIds(ids));
            }

            //try
            //{
            //    var bytes = _exportManager.ExportProjectsToXlsx(projects);
            //    return File(bytes, MimeTypes.TextXlsx, "projects.xlsx");
            //}
            //catch (Exception exc)
            //{
            //    return RedirectToAction("List");
            //}
            return View();
        }


        [HttpPost, ActionName("List")]
        public virtual IActionResult ExportXmlAll(ProjectSearchModel model)
        {
            return View();
        }
        [HttpPost]
        public virtual IActionResult ExportXmlSelected(string selectedIds)
        {
            return View();
        }

        [HttpPost, ActionName("List")]
        public virtual IActionResult ExportExcelAll(ProjectSearchModel model)
        {
            return View();
        }
        [HttpPost]
        public ActionResult ProjectResearcherList(int projectId)
        {
           // var data = _projectService.GetProjectResearcherByProjectId(projectId).ToList();
           var model = new ProjectListModel();
            //model.Data = data;
            //model.Total = data.Count;
            //model.Errors = "";
            return Json(model);
        }
        [HttpPost]
        public ActionResult ProjectList(ProjectSearchModel searchModel)
        {
            var model = _projectModelFactory.PrepareProjectListModel(searchModel);
            //var data = _projectService.GetAllProjects().ToList();
            //var model = new ProjectListModel();
            ////model.Data = data;
            //model.Total = data.Count;
            //model.Errors = "";
            return Json(model);

            //try
            //{
            //    // Initialization.
            //    string search = Request.Form["search[value]"][0];
            //    string draw = Request.Form["draw"][0];
            //    string order = Request.Form["order[0][column]"][0];
            //    string orderDir = Request.Form["order[0][dir]"][0];
            //    int startRec = Convert.ToInt32(Request.Form["start"][0]);
            //    int pageSize = Convert.ToInt32(Request.Form["length"][0]);

            //    _logger.LogInformation("Call GetGridAll()");
            //    // Loading.
            //    List<ProjectGridViewModel> data = _projectService.GetGridAll().ToList();

            //    // Total record count.
            //    int totalRecords = data.Count;

            //    // Verification.
            //    //if (!string.IsNullOrEmpty(search) &&
            //    //    !string.IsNullOrWhiteSpace(search))
            //    //{
            //    //    // Apply search
            //    //    data = data.Where(p => p.sr.ToString().ToLower().Contains(search.ToLower()) ||
            //    //                           p.ordertracknumber.ToLower().Contains(search.ToLower()) ||
            //    //                           p.quantity.ToString().ToLower().Contains(search.ToLower()) ||
            //    //                           p.productname.ToLower().Contains(search.ToLower()) ||
            //    //                           p.specialoffer.ToLower().Contains(search.ToLower()) ||
            //    //                           p.unitprice.ToString().ToLower().Contains(search.ToLower()) ||
            //    //                           p.unitpricediscount.ToString().ToLower().Contains(search.ToLower())).ToList();
            //    //}

            //    // Sorting.
            //    //data = this.SortByColumnWithOrder(order, orderDir, data);

            //    // Filter record count.
            //    int recFilter = data.Count;

            //    // Apply pagination.
            //    data = data.Skip(startRec).Take(pageSize).ToList();

            //    // Loading drop down lists.
            //    var result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = data });
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    // Info
            //    Console.Write(ex);
            //    return null;
            //}
        }
    }
}
