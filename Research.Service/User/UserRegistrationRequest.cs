

using Research.Data;
using Research.Enum;

namespace Research.Services.Users
{
    /// <summary>
    /// User registration request
    /// </summary>
    public class UserRegistrationRequest
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="email">Email</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="passwordFormat">Password format</param>
        /// <param name="isApproved">Is approved</param>
        public UserRegistrationRequest(User user, string email, string idcard,
            string password,
            string username,
            string gender,
            PasswordFormat passwordFormat,
            bool isApproved = true)
        {
            this.User = user;
            this.Email = email;
            this.IDCard = idcard;
            this.Password = password;
            this.Username = username;
            this.Gender = gender;
            this.PasswordFormat = passwordFormat;
            this.IsApproved = isApproved;
        }

        /// <summary>
        /// User
        /// </summary>
        public User User { get; set; }

        public string Gender { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Password format
        /// </summary>
        public PasswordFormat PasswordFormat { get; set; }

        /// <summary>
        /// Is approved
        /// </summary>
        public bool IsApproved { get; set; }
        public string IDCard { get; set; }

    }
}
