
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
            Projects,
            Dashboards,
            About,
            Contact,
            Error,
            Login,
            Register,
            SuperAdmin,
            Role,
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
                //case Module.Home:
                //    return new SidebarMenu
                //    {
                //        Type = SidebarMenuType.Link,
                //        Name = "Home",
                //        IconClassName = "fa fa-link",
                //        URLPath = "/",
                //        LinkCounter = counter,
                //    };
                case Module.Projects:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "ข้อเสนอโครงการวิจัย",
                        IconClassName = "fa fa-link",
                        URLPath = "/Project",
                        LinkCounter = counter,
                    };
                //case Module.Dashboards:
                //    return new SidebarMenu
                //    {
                //        Type = SidebarMenuType.Link,
                //        Name = "Dashboards",
                //        IconClassName = "fa fa-link",
                //        URLPath = "/Dashboards",
                //        LinkCounter = counter,
                //    };
                case Module.Login:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Login",
                        IconClassName = "fa fa-sign-in",
                        URLPath = "/Account/Login",
                        LinkCounter = counter,
                    };
                case Module.Register:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Register",
                        IconClassName = "fa fa-user-plus",
                        URLPath = "/Account/Register",
                        LinkCounter = counter,
                    };
                case Module.About:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "About",
                        IconClassName = "fa fa-group",
                        URLPath = "/Home/About",
                        LinkCounter = counter,
                    };
                case Module.Contact:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Contact",
                        IconClassName = "fa fa-phone",
                        URLPath = "/Home/Contact",
                        LinkCounter = counter,
                    };
                //case Module.Error:
                //    return new SidebarMenu
                //    {
                //        Type = SidebarMenuType.Link,
                //        Name = "Error",
                //        IconClassName = "fa fa-warning",
                //        URLPath = "/Home/Error",
                //        LinkCounter = counter,
                //    };
                case Module.SuperAdmin:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "User",
                        IconClassName = "fa fa-link",
                        URLPath = "/SuperAdmin",
                        LinkCounter = counter,
                    };
                case Module.Role:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Role",
                        IconClassName = "fa fa-link",
                        URLPath = "/Role",
                        LinkCounter = counter,
                    };

                default:
                    break;
            }

            return null;
        }
    }
}
