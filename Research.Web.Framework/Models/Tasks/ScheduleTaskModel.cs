
using Research.Web.Framework.Models;

namespace Research.Web.Models.Tasks
{
    /// <summary>
    /// Represents a schedule task model
    /// </summary>
    //[Validator(typeof(ScheduleTaskValidator))]
    public partial class ScheduleTaskModel : BaseEntityModel
    {
        #region Properties

        //[NopResourceDisplayName("Admin.System.ScheduleTasks.Name")]
        public string Name { get; set; }

        //[NopResourceDisplayName("Admin.System.ScheduleTasks.Seconds")]
        public int Seconds { get; set; }

        //[NopResourceDisplayName("Admin.System.ScheduleTasks.Enabled")]
        public bool Enabled { get; set; }

        //[NopResourceDisplayName("Admin.System.ScheduleTasks.StopOnError")]
        public bool StopOnError { get; set; }

        //[NopResourceDisplayName("Admin.System.ScheduleTasks.LastStart")]
        public string LastStartUtc { get; set; }

        //[NopResourceDisplayName("Admin.System.ScheduleTasks.LastEnd")]
        public string LastEndUtc { get; set; }

        //[NopResourceDisplayName("Admin.System.ScheduleTasks.LastSuccess")]
        public string LastSuccessUtc { get; set; }

        #endregion
    }
}