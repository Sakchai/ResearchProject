using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Research.Web.Models.Dashboard;

namespace Company.WebApplication1.Controllers
{
    public class DashboardsController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Dashboard1");
        }

        public IActionResult Dashboard1()
        {
            return View();
        }

        public IActionResult Dashboard2()
        {
            var model = new DashboardModel { FiscalYear = "2562",
                FundAmount = "880,000",
                NoOfProject = "23",
                NoOfReseacher = "220",
                NoOfProfessor = "72",
                FacultyList = "['คณะครุศาสตร์','คณะมนุษยศาสตร์และสังคมศาสตร์','คณะวิทยาการจัดการ','คณะวิทยาศาสตร์และเทคโนโลยี','คณะเทคโนโลยีการเกษตรและเทคโนโลยีอุตสาหกรรม']",
                FundAmountList = "[2,4,5,4,3]",
                ProjectList = "[22,18,25,27,28]"
                };

            return View(model);
        }

    }
}