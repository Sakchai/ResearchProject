using Microsoft.AspNetCore.Routing;

namespace Research.Web.Framework.Mvc.Routing
{
    /// <summary>
    /// Represents route publisher
    /// </summary>
    public interface IRoutePublisher
    {
        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        void RegisterRoutes(IRouteBuilder routeBuilder);
    }
}
