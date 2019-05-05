﻿
using Research.Web.Models;
using System;
using System.Collections.Generic;

namespace Research.Common
{
    /// <summary>
    /// This is where you customize the navigation sidebar
    /// </summary>
    public static class ModuleHelper
    {
        public enum Module
        {
            Home,
            AddProject,
            Projects,
            Dashboards,
            About,
            Contact,
            Error,
            Login,
            Register,
            SuperAdmin,
            Role,
            ActivityLogs,
            AddResearcher,
            Researchs,
            Professors,
            AddProfessor,
            EditResearcher,
            Users,
            AddUser,
            ResearchIssues,
            FiscalSchedules,
        }

        public static SidebarMenu AddHeader(string name)
        {
            return new SidebarMenu
            {
                Type = SidebarMenuType.Header,
                Name = name,
            };
        }

        public static SidebarMenu AddTree(string name, string iconClassName = "fa fa-link")
        {
            return new SidebarMenu
            {
                Type = SidebarMenuType.Tree,
                IsActive = false,
                Name = name,
                IconClassName = iconClassName,
                URLPath = "#",
            };
        }

        public static SidebarMenu AddModule(Module module, Tuple<int, int, int> counter = null)
        {
            if (counter == null)
                counter = Tuple.Create(0, 0, 0);

            switch (module)
            {

                case Module.Projects:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "ข้อเสนอโครงการวิจัย",
                        IconClassName = "fa fa-calendar",
                        URLPath = "/Project/List",
                        LinkCounter = counter,
                    };
                case Module.AddProject:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "ยื่นข้อเสนอโครงการวิจัย",
                        IconClassName = "fa fa-graduation-cap",
                        URLPath = "/Project/Add",
                        LinkCounter = counter,
                    };
                case Module.ResearchIssues:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "รายการประเด็นการวิจัย",
                        IconClassName = "fa fa-heart-o",
                        URLPath = "/ResearchIssue/List",
                        LinkCounter = counter,
                    };
                case Module.FiscalSchedules:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "วันเปิดรับข้อเสนอ",
                        IconClassName = "fa fa-hourglass-half",
                        URLPath = "/ResearchIssue/List",
                        LinkCounter = counter,
                    };
                case Module.Researchs:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "รายการข้อมูลนักวิจัย",
                        IconClassName = "fa fa-users",
                        URLPath = "/Researcher/List",
                        LinkCounter = counter,
                    };
                case Module.AddResearcher:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "เพิ่มผู้วิจัย",
                        IconClassName = "fa fa-user-plus",
                        URLPath = "/Researcher/Add",
                        LinkCounter = counter,
                    };
                case Module.Professors:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "รายการข้อมูลผู้ทรงคุณวุฒิ",
                        IconClassName = "fa fa-users",
                        URLPath = "/Professor/List",
                        LinkCounter = counter,
                    };
                case Module.AddProfessor:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "เพิ่มผู้ทรงคุณวุฒิ",
                        IconClassName = "fa fa-user-plus",
                        URLPath = "/Professor/Add",
                        LinkCounter = counter,
                    };
                case Module.Users:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "รายการบัญชีผู้ดูแลระบบ",
                        IconClassName = "fa fa-users",
                        URLPath = "/User/List",
                        LinkCounter = counter,
                    };
                case Module.AddUser:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "เพิ่มผู้ใช้",
                        IconClassName = "fa fa-user-plus",
                        URLPath = "/User/Add",
                        LinkCounter = counter,
                    };
                case Module.ActivityLogs:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "ข้อมูลการเข้าใช้ระบบ",
                        IconClassName = "fa fa-laptop",
                        URLPath = "/ActivityLog/ListLogs",
                        LinkCounter = counter,
                    };
                case Module.Dashboards:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "ภาพรวม",
                        IconClassName = "fa fa-dashboard",
                        URLPath = "/Dashboards",
                        LinkCounter = counter,
                    };
                case Module.Login:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Login",
                        IconClassName = "fa fa-sign-in",
                        URLPath = "/User/Login",
                        LinkCounter = counter,
                    };
                case Module.Register:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "ลงทะเบียนผู้วิจัย",
                        IconClassName = "fa fa-user-plus",
                        URLPath = "/User/Register",
                        LinkCounter = counter,
                    };
                case Module.About:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "เกี่ยวกับโครงการ",
                        IconClassName = "fa fa-group",
                        URLPath = "/Home/About",
                        LinkCounter = counter,
                    };
                case Module.Contact:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "ผู้ติดต่อ",
                        IconClassName = "fa fa-phone",
                        URLPath = "/Home/Contact",
                        LinkCounter = counter,
                    };
 

                default:
                    break;
            }

            return null;
        }
    }
}
