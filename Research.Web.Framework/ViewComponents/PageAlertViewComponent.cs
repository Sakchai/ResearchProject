﻿
using Microsoft.AspNetCore.Mvc;
using Research.Web.Models;
using System;
using System.Collections.Generic;

namespace Research.Web.ViewComponents
{
    public class PageAlertViewComponent : ViewComponent
    {

        public PageAlertViewComponent()
        {
        }

        public IViewComponentResult Invoke(string filter)
        {
            List<Message> messages;
            if (ViewBag.PageAlerts == null)
            {
                messages = new List<Message>();
            }
            else
            {
                messages = new List<Message>(ViewBag.PageAlerts);
            }
            return View(messages);
        }
    }
}
