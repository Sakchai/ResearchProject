using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project.Domain;
using Project.Domain.Service;
using Project.Web.Factories;
using Project.Web.Models.ProjectViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project.Web.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IProjectModelFactory _projectModelFactory;
        private readonly ProjectService _projectService;
        private readonly ResearcherService _researcherService;
        private readonly ILogger _logger;

        public ProjectController(ProjectService projectService
                                , ResearcherService researcherService
                                , IProjectModelFactory projectModelFactory
                                , ILoggerFactory loggerFactory)
        {
            _researcherService = researcherService;
            _projectService = projectService;
            _projectModelFactory = projectModelFactory;
            _logger = loggerFactory.CreateLogger<ProjectController>();

        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult Edit(int id)
        {

            //try to get a project with the specified id
            var project = _projectService.GetOne(id);
            if (project == null)
                return RedirectToAction("List");

            //prepare model
            var model = _projectModelFactory.PrepareProjectEditModel(null, project);

            return View(model);
        }

        public IActionResult Create()
        {
            AddPageHeader("เพิ่มข้อเสนอโครงการวิจัย", "");
            var model = new CreateVm();
            model = _projectModelFactory.PrepareProjectCreateModel(model);
            return View(model);
        }

        public virtual IActionResult List()
        {
            AddPageHeader("ข้อเสนอโครงการวิจัย", "");
            //prepare model
            var model = _projectModelFactory.PrepareProjectSearchModel(new ProjectSearchViewModel());

            return View(model);
        }
        [HttpPost]
        public IActionResult ExportExcelSelected(string selectedIds)
        {

            var projects = new List<Entity.Project>();
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
        public virtual IActionResult ExportXmlAll(ProjectSearchViewModel model)
        {
            return View();
        }
        [HttpPost]
        public virtual IActionResult ExportXmlSelected(string selectedIds)
        {
            return View();
        }

        [HttpPost, ActionName("List")]
        public virtual IActionResult ExportExcelAll(ProjectSearchViewModel model)
        {
            return View();
        }
       // [HttpPost]
        public ActionResult ProjectList(ProjectSearchViewModel searchModel)
        {
            var data = _projectService.GetGridAll().ToList();
            var model = new ProjectListModel();
            model.Data = data;
            model.Total = data.Count;
            model.Errors = "";
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
