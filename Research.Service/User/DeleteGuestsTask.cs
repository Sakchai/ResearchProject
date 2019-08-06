using System;
using Research.Core.Domain.Users;
using Research.Services.Tasks;

namespace Research.Services.Users
{
    /// <summary>
    /// Represents a task for deleting guest customers
    /// </summary>
    public partial class DeleteGuestsTask : IScheduleTask
    {
        private readonly IUserService _customerService;
        private readonly UserSettings _customerSettings;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="customerService">User service</param>
        /// <param name="customerSettings">User settings</param>
        public DeleteGuestsTask(IUserService customerService,
            UserSettings customerSettings)
        {
            this._customerService = customerService;
            this._customerSettings = customerSettings;
        }

        /// <summary>
        /// Executes a task
        /// </summary>
        public void Execute()
        {
            var olderThanMinutes = _customerSettings.DeleteGuestTaskOlderThanMinutes;
            // Default value in case 0 is returned.  0 would effectively disable this service and harm performance.
            olderThanMinutes = olderThanMinutes == 0 ? 1440 : olderThanMinutes;
    
            _customerService.DeleteGuestUsers(null, DateTime.UtcNow.AddMinutes(-olderThanMinutes), true);
        }
    }
}
