using Research.Web.Framework.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Research.Web.Models.Users
{
    public class UserAgreementModel : BaseResearchModel
    {
        public Guid UserGuid { get; set; }
        public string UserAgreementText { get; set; }
    }
}
