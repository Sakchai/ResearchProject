using Research.Data;

namespace Research.Core.Domain.Users
{
    /// <summary>
    /// User registered event
    /// </summary>
    public class UserRegisteredEvent
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="user">user</param>
        public UserRegisteredEvent(User user)
        {
            this.User = user;
        }

        /// <summary>
        /// User
        /// </summary>
        public User User
        {
            get;
        }
    }
}