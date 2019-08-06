using System.Net;
using Research.Core;
using Research.Core.Http;
using Research.Services.Tasks;

namespace Research.Services.Common
{
    /// <summary>
    /// Represents a task for keeping the site alive
    /// </summary>
    public partial class KeepAliveTask : IScheduleTask
    {
        #region Fields

        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="webHelper">Web helper</param>
        public KeepAliveTask(IWebHelper webHelper)
        {
            this._webHelper = webHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes a task
        /// </summary>
        public void Execute()
        {
            var keepAliveUrl = $"{ResearchHttpDefaults.KeepAlivePath}";
            using (var wc = new WebClient())
            {
                wc.DownloadString(keepAliveUrl);
            }
        }

        #endregion
    }
}