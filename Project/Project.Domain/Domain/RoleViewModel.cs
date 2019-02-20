namespace Project.Domain
{
    /// <summary>
    /// A user attached to an account
    /// </summary>
    public class RoleViewModel : BaseDomain
    {
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public string LastUpdateBy { get; set; }
    }
}