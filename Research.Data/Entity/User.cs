using System;
using System.Collections.Generic;
using System.Linq;
using Research.Enum;

namespace Research.Data
{
    public partial class User : BaseEntity
    {
        private IList<UserRole> _userRoles;
        private ICollection<UserUserRoleMapping> _userUserRoleMappings;
        public User()
        {
            this.UserGuid = Guid.NewGuid();
        }
        public int TitleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public int UserTypeId { get; set; }
        public UserType UserType {
            get => (UserType)UserTypeId;
            set => UserTypeId = (int) value;
        }
        public string MobileNumber { get; set; }
        public bool RequireReLogin { get; set; }
        public bool IsActive { get; set; }

        public bool Deleted { get; set; }
        public bool IsSystemAccount { get; set; }
        public string Roles { get; set; }
        public string Password { get; set; }
        public int ResearcherId { get; set; }
        public string SessionId { get; set; }
        public int AgencyId { get; set; }
        public string LastUpdateBy { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        /// <summary>
        /// Gets or sets the last IP address
        /// </summary>
        public string LastIpAddress { get; set; }
        /// <summary>
        /// Gets or sets the date and time of last login
        /// </summary>
        public DateTime? LastLoginDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last activity
        /// </summary>
        public DateTime LastActivityDateUtc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating number of failed login attempts (wrong password)
        /// </summary>
        public int FailedLoginAttempts { get; set; }

        /// <summary>
        /// Gets or sets the date and time until which a user cannot login (locked out)
        /// </summary>
        public DateTime? CannotLoginUntilDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the email that should be re-validated. Used in scenarios when a user is already registered and wants to change an email address.
        /// </summary>
        public string EmailToRevalidate { get; set; }

        public string SystemName { get; set; }
        public virtual Title Title { get; set; }
        public virtual Agency Agency { get; set; }
        public virtual Researcher Researcher { get; set; }

        /// <summary>
        /// Gets or sets user roles
        /// </summary>
        public virtual IList<UserRole> UserRoles
        {
            get => _userRoles ?? (_userRoles = UserUserRoleMappings.Select(mapping => mapping.UserRole).ToList());
        }

        /// <summary>
        /// Gets or sets user-user role mappings
        /// </summary>
        public virtual ICollection<UserUserRoleMapping> UserUserRoleMappings
        {
            get => _userUserRoleMappings ?? (_userUserRoleMappings = new List<UserUserRoleMapping>());
            protected set => _userUserRoleMappings = value;
        }

        //[Computed]
        //public string DecryptedPassword
        //{
        //    get { return Decrypt(Password); }
        //    set { Password = Encrypt(value); }
        //}
        public Guid UserGuid { get; set; }

        //private string Decrypt(string cipherText)
        //{
        //    return EntityHelper.Decrypt(cipherText);
        //}
        //private string Encrypt(string clearText)
        //{
        //    return EntityHelper.Encrypt(clearText);
        //}

    }


}
