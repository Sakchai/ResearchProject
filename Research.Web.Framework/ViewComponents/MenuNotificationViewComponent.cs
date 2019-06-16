
using Microsoft.AspNetCore.Mvc;
using Research.Web.Models;
using System.Collections.Generic;

namespace Research.Web.ViewComponents
{
    public class MenuNotificationViewComponent : ViewComponent
    {

        public MenuNotificationViewComponent()
        {
        }

        public IViewComponentResult Invoke(string filter)
        {
            var messages = GetData();
            return View(messages);
        }

        private List<Message> GetData()
        {
            var messages = new List<Message>();
            //messages.Add(new Message
            //{
            //    Id = 1,
            //    FontAwesomeIcon = "fa fa-users text-aqua",
            //    ShortDesc = "5 new members joined today",
            //    URLPath = "#",
            //});

            return messages;
        }
    }
}
