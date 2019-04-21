using Research.Infrastructure;
using Research.Services.Events;
using System.Collections.Generic;
using System.Linq;

namespace Research.Services.Events
{
    /// <summary>
    /// Event subscription service
    /// </summary>
    public class SubscriptionService : ISubscriptionService
    {
        #region Methods

        /// <summary>
        /// Get subscriptions
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Event consumers</returns>
        public IList<IConsumer<T>> GetSubscriptions<T>()
        {
            return EngineContext.Current.ResolveAll<IConsumer<T>>().ToList();
        }

        #endregion
    }
}