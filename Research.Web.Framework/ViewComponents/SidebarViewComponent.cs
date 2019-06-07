using Research.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Research.Web.Models;

namespace Research.Web.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        public SidebarViewComponent()
        {
        }

        public IViewComponentResult Invoke(string filter)
        {
            //you can do the access rights checking here by using session, user, and/or filter parameter
            var sidebars = new List<SidebarMenu>();

            //if (((ClaimsPrincipal)User).GetUserProperty("AccessProfile").Contains("VES_008, Payroll"))
            //{
            //}

            // sidebars.Add(ModuleHelper.AddHeader("MAIN NAVIGATION"));
            sidebars.Add(ModuleHelper.AddModule(ModuleHelper.Module.Dashboards));
            sidebars.Add(ModuleHelper.AddTree("โครงการวิจัย", "fa fa-calendar"));
            sidebars.Last().TreeChild = new List<SidebarMenu>()
            {
                ModuleHelper.AddModule(ModuleHelper.Module.Projects),
                ModuleHelper.AddModule(ModuleHelper.Module.AddProject)
            };
            sidebars.Add(ModuleHelper.AddTree("ผู้วิจัย/ผู้ทรงคุณวุฒิ", "fa fa-users"));
            sidebars.Last().TreeChild = new List<SidebarMenu>()
            {
                ModuleHelper.AddModule(ModuleHelper.Module.Researchs),
                ModuleHelper.AddModule(ModuleHelper.Module.AddResearcher),
                ModuleHelper.AddModule(ModuleHelper.Module.Professors),
                ModuleHelper.AddModule(ModuleHelper.Module.AddProfessor),
            };
            sidebars.Add(ModuleHelper.AddTree("ข้อมูลของระบบ", "fa fa-university"));
            sidebars.Last().TreeChild = new List<SidebarMenu>()
            {
                ModuleHelper.AddModule(ModuleHelper.Module.ResearchIssues),
                ModuleHelper.AddModule(ModuleHelper.Module.FiscalSchedules),
                ModuleHelper.AddModule(ModuleHelper.Module.Users),
                ModuleHelper.AddModule(ModuleHelper.Module.AddUser),
                ModuleHelper.AddModule(ModuleHelper.Module.ActivityLogs),
                ModuleHelper.AddModule(ModuleHelper.Module.MessageTemplate),
                ModuleHelper.AddModule(ModuleHelper.Module.EmailAccount)
            };
            sidebars.Add(ModuleHelper.AddModule(ModuleHelper.Module.About, Tuple.Create(0, 1, 0)));
            sidebars.Add(ModuleHelper.AddModule(ModuleHelper.Module.Contact, Tuple.Create(1, 0, 0)));
            //sidebars.Add(ModuleHelper.AddTree("Account"));
            //sidebars.Last().TreeChild = new List<SidebarMenu>()
            //{
            //    ModuleHelper.AddModule(ModuleHelper.Module.Login),
            //    ModuleHelper.AddModule(ModuleHelper.Module.Register, Tuple.Create(1, 1, 1)),
            //};

            //if (User.IsInRole("SuperAdmins"))
            //{
            //    sidebars.Add(ModuleHelper.AddTree("Administration"));
            //    sidebars.Last().TreeChild = new List<SidebarMenu>()
            //{
            //    ModuleHelper.AddModule(ModuleHelper.Module.SuperAdmin),
            //    ModuleHelper.AddModule(ModuleHelper.Module.Role),
            //};
            //}

            return View(sidebars);
        }
    }
}