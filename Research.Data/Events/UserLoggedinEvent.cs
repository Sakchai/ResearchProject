using Research.Data;

namespace Research.Core.Domain.Users
{
    /// <summary>
    /// User logged-in event
    /// </summary>
    public class UserLoggedinEvent
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="user">User</param>
        public UserLoggedinEvent(User user)
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