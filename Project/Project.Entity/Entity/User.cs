using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using System.Text;

namespace Project.Entity
{
    public partial class User : BaseEntity
    {
        public User()
        {
            UserRole = new HashSet<UserRole>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public UserType UserType { get; set; }
        public string MobileNumber { get; set; }
        public bool RequireReLogin { get; set; }
        public bool? IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool IsAdminRole { get; set; }
        public string Roles { get; set; }
        public string Password { get; set; }
        public int? ResearcherId { get; set; }
        public string SessionId { get; set; }
        public int? FacultyId { get; set; }
        public string LastUpdateBy { get; set; }

        public virtual Faculty Faculty { get; set; }
        public virtual Researcher Researcher { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }

        [Computed]
        public string DecryptedPassword
        {
            get { return Decrypt(Password); }
            set { Password = Encrypt(value); }
        }
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }

        private string Decrypt(string cipherText)
        {
            return EntityHelper.Decrypt(cipherText);
        }
        private string Encrypt(string clearText)
        {
            return EntityHelper.Encrypt(clearText);
        }
    }

    public enum UserType
    {
        Researcher = 1,
        ResearchDevelopmentInstituteStaff = 2,
        ResearchCoordinator = 3
    }
}
