﻿using Research.Services.Tasks;

namespace Research.Services.Logging
{
    /// <summary>
    /// Represents a task to clear [Log] table
    /// </summary>
    public partial class ClearLogTask : IScheduleTask
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logger">Logger</param>
        public ClearLogTask(ILogger logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// Executes a task
        /// </summary>
        public virtual void Execute()
        {
            _logger.ClearLog();
        }
    }
}
