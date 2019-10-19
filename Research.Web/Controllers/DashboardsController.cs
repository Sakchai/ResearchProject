using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Research.Services;
using Research.Web.Models.Dashboard;

namespace Company.WebApplication1.Controllers
{
    public class DashboardsController : Controller
    {
        private readonly IUserService _userService;
        public DashboardsController(IUserService userService)
        {
            this._userService = userService;
        }
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
            int year = DateTime.Now.Year + 543;
            var projectByAgents = _userService.ProjectByAgentDashboard(year).ToArray();
            int n = projectByAgents.Count();
            if (n > 0)
            {
                string[] facultyList = new string[n];
                string[] fundAmountList = new string[n];
                string[] projectList = new string[n];
                string fundAmount = projectByAgents[0].FundAmount.ToString();
                string totalProject = projectByAgents[0].TotalProject.ToString();
                string totalResearcher = projectByAgents[0].TotalReseacher.ToString();
                string totalProfessor = projectByAgents[0].TotalProfessor.ToString();
                for (int i = 0; i < projectByAgents.Length; i++)
                {
                    facultyList[i] = $"'{projectByAgents[i].AgencyName}'";
                    fundAmountList[i] = $"{projectByAgents[i].FundAmount.ToString()}";
                    projectList[i] = $"{projectByAgents[i].NoOfProject.ToString()}";
                }
                var agencies = string.Join(",", facultyList);
                var fundAmounts = string.Join(",", fundAmountList);
                var noOfProjects = string.Join(",", projectList);
                var model = new DashboardModel
                {
                    FiscalYear = $"{year}",
                    FundAmount = fundAmount,
                    NoOfProject = $"{totalProject}",
                    NoOfReseacher = $"{totalResearcher}",
                    NoOfProfessor = $"{totalProfessor}",
                    FacultyList = $"[{agencies}]",
                    FundAmountList = $"[{fundAmounts}]",
                    ProjectList = $"[{noOfProjects}]"
                };
                return View(model);
            } else
            {
                var model = new DashboardModel
                {
                    FiscalYear = $"{year}",
                    FundAmount = "0",
                    NoOfProject = "0",
                    NoOfReseacher = "0",
                    NoOfProfessor = "0",
                    FacultyList = "['']",
                    FundAmountList = "[0]",
                    ProjectList = "[0]"
                };
                return View(model);
            }
            
        }

    }
}