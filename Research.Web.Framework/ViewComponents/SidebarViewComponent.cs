using Research.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Research.Web.Models;
using Research.Services.Security;
using Research.Core;

namespace Research.Web.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;

        public SidebarViewComponent(IPermissionService permissionService,
            IWorkContext workContext)
        {
            this._permissionService = permissionService;
            this._workContext = workContext;
        }

        public IViewComponentResult Invoke(string filter)
        {
            //you can do the access rights checking here by using session, user, and/or filter parameter
            var sidebars = new List<SidebarMenu>();

            //if (((ClaimsPrincipal)User).GetUserProperty("AccessProfile").Contains("VES_008, Payroll"))
            //{
            //}

            // sidebars.Add(ModuleHelper.AddHeader("MAIN NAVIGATION"));
            //sidebars.Add(ModuleHelper.AddModule(ModuleHelper.Module.Dashboards));

            int userTypeId = _workContext.CurrentUser.UserTypeId;
            sidebars.Add(ModuleHelper.AddTree("โครงการวิจัย/ผู้วิจัย", "fa fa-university"));
            sidebars.Last().TreeChild = new List<SidebarMenu>()
            {
                ModuleHelper.AddModule(ModuleHelper.Module.Projects),
                ModuleHelper.AddModule(ModuleHelper.Module.Researchs),
            };
            if (userTypeId != 2)
            {
                
                sidebars.Add(ModuleHelper.AddTree("ข้อมูลของระบบ", "fa fa-university"));
                sidebars.Last().TreeChild = new List<SidebarMenu>()
                {
                    ModuleHelper.AddModule(ModuleHelper.Module.Professors),
                    ModuleHelper.AddModule(ModuleHelper.Module.ResearchIssues),
                    ModuleHelper.AddModule(ModuleHelper.Module.FiscalSchedules),
                    ModuleHelper.AddModule(ModuleHelper.Module.Users),
                    ModuleHelper.AddModule(ModuleHelper.Module.MessageTemplate),
                    ModuleHelper.AddModule(ModuleHelper.Module.EmailAccount),
                    ModuleHelper.AddModule(ModuleHelper.Module.ScheduleTask),
                    ModuleHelper.AddModule(ModuleHelper.Module.UserRole),
                    ModuleHelper.AddModule(ModuleHelper.Module.Security),
                    ModuleHelper.AddModule(ModuleHelper.Module.Logs),
                   // ModuleHelper.AddModule(ModuleHelper.Module.ActivityLogs),
                };
            }
            sidebars.Add(ModuleHelper.AddModule(ModuleHelper.Module.SignOut, Tuple.Create(0, 1, 0)));
            //sidebars.Add(ModuleHelper.AddModule(ModuleHelper.Module.About, Tuple.Create(0, 1, 0)));
            //sidebars.Add(ModuleHelper.AddModule(ModuleHelper.Module.Contact, Tuple.Create(1, 0, 0)));
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