using System;
using Microsoft.AspNetCore.Mvc;
using Research.Core;
using Research.Core.Domain.Users;
using Research.Services.Logging;
using Research.Services.Security;
using Research.Web.Framework.Controllers;
using Research.Web.Framework.Mvc.Filters;
using Research.Web.Controllers;
using Research.Services;
using Research.Data;
using Research.Web.Models.Users;
using Research.Web.Extensions;
using Research.Web.Models.Factories;

namespace Research.Web.Areas.Admin.Controllers
{
    public partial class UserRoleController : BaseAdminController
    {
        #region Fields

        private readonly IUserActivityService _userActivityService;
        private readonly IUserRoleModelFactory _userRoleModelFactory;
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public UserRoleController(IUserActivityService userActivityService,
            IUserRoleModelFactory userRoleModelFactory,
            IUserService userService,
            IPermissionService permissionService,
            IWorkContext workContext)
        {
            this._userActivityService = userActivityService;
            this._userRoleModelFactory = userRoleModelFactory;
            this._userService = userService;
            this._permissionService = permissionService;
            this._workContext = workContext;
        }

        #endregion

        #region Methods

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //prepare model
            var model = _userRoleModelFactory.PrepareUserRoleSearchModel(new UserRoleSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(UserRoleSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _userRoleModelFactory.PrepareUserRoleListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //prepare model
            var model = _userRoleModelFactory.PrepareUserRoleModel(new UserRoleModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(UserRoleModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var userRole = model.ToEntity<UserRole>();
                _userService.InsertUserRole(userRole);

                //activity log
                _userActivityService.InsertActivity("AddNewUserRole",
                    string.Format("ActivityLog.AddNewUserRole {0}", userRole.Name), userRole);

                SuccessNotification("Admin.Users.UserRoles.Added");

                return continueEditing ? RedirectToAction("Edit", new { id = userRole.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _userRoleModelFactory.PrepareUserRoleModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user role with the specified id
            var userRole = _userService.GetUserRoleById(id);
            if (userRole == null)
                return RedirectToAction("List");

            //prepare model
            var model = _userRoleModelFactory.PrepareUserRoleModel(null, userRole);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(UserRoleModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user role with the specified id
            var userRole = _userService.GetUserRoleById(model.Id);
            if (userRole == null)
                return RedirectToAction("List");

            try
            {
                if (ModelState.IsValid)
                {
                    if (userRole.IsSystemRole && !model.IsActive)
                        throw new ResearchException("Admin.Users.UserRoles.Fields.Active.CantEditSystem");

                    if (userRole.IsSystemRole && !userRole.SystemName.Equals(model.SystemName, StringComparison.InvariantCultureIgnoreCase))
                        throw new ResearchException("Admin.Users.UserRoles.Fields.SystemName.CantEditSystem");


                    userRole = model.ToEntity(userRole);
                    _userService.UpdateUserRole(userRole);

                    //activity log
                    _userActivityService.InsertActivity("EditUserRole",
                        string.Format("ActivityLog.EditUserRole", userRole.Name), userRole);

                    SuccessNotification("Admin.Users.UserRoles.Updated");

                    return continueEditing ? RedirectToAction("Edit", new { id = userRole.Id }) : RedirectToAction("List");
                }

                //prepare model
                model = _userRoleModelFactory.PrepareUserRoleModel(model, userRole, true);

                //if we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = userRole.Id });
            }
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user role with the specified id
            var userRole = _userService.GetUserRoleById(id);
            if (userRole == null)
                return RedirectToAction("List");

            try
            {
                _userService.DeleteUserRole(userRole);

                //activity log
                _userActivityService.InsertActivity("DeleteUserRole",
                    string.Format("ActivityLog.DeleteUserRole {0}", userRole.Name), userRole);

                SuccessNotification("Admin.Users.UserRoles.Deleted");

                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = userRole.Id });
            }
        }

    
        #endregion
    }
}