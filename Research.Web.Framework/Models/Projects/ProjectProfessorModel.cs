
using Microsoft.AspNetCore.Mvc.Rendering;
using Research.Data;
using Research.Enum;
using Research.Web.Framework.Models;
using System.Collections.Generic;

namespace Research.Web.Models.Projects
{
    public class ProjectProfessorModel : BaseEntityModel
    {
        public ProjectProfessorModel()
        {
            AvailableProfessorTypes = new List<SelectListItem>();
        }
        public int ProjectId { get; set; }
        public int ProfessorId { get; set; }
        public int ProfessorTypeId { get; set; }
        public string ProfessorTyperName { get; set; }

        public IList<SelectListItem> AvailableProfessorTypes { get; set; }
    }
}
