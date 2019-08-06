
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Research.Web.Models;

namespace Research.Web.ViewComponents
{
    public class BreadcrumbViewComponent : ViewComponent
    {

        public BreadcrumbViewComponent()
        {
            
        }

        public IViewComponentResult Invoke(string filter)
        {
            if (ViewBag.Breadcrumb == null)
            {
                ViewBag.Breadcrumb = new List<Message>();
            }
            
            return View(ViewBag.Breadcrumb as List<Message>);
        }
    }
}
