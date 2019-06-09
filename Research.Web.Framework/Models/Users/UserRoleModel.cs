using System.Collections.Generic;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Research.Web.Models;

namespace Research.Web.Models.Users
{
    /// <summary>
    /// Represents a customer role model
    /// </summary>
   // [Validator(typeof(UserRoleValidator))]
    public partial class UserRoleModel : BaseEntityModel
    {

        #region Properties

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool IsSystemRole { get; set; }

        public string SystemName { get; set; }

        public bool EnablePasswordLifetime { get; set; }

    

        #endregion
    }
}