﻿using Research.Web.Models.Logging;
using System.Collections.Generic;

namespace Research.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the activity log model factory
    /// </summary>
    public partial interface IActivityLogModelFactory
    {
        /// <summary>
        /// Prepare activity log type models
        /// </summary>
        /// <returns>List of activity log type models</returns>
        IList<ActivityLogTypeModel> PrepareActivityLogTypeModels();

        /// <summary>
        /// Prepare activity log search model
        /// </summary>
        /// <param name="searchModel">Activity log search model</param>
        /// <returns>Activity log search model</returns>
        ActivityLogSearchModel PrepareActivityLogSearchModel(ActivityLogSearchModel searchModel);

        /// <summary>
        /// Prepare paged activity log list model
        /// </summary>
        /// <param name="searchModel">Activity log search model</param>
        /// <returns>Activity log list model</returns>
        ActivityLogListModel PrepareActivityLogListModel(ActivityLogSearchModel searchModel);
    }
}