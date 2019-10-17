

using System.Runtime.Serialization;

namespace Research.Enum
{
    public enum Degree
    {
        [EnumMember(Value = "ปริญญาตรี")]
        BachelorDegrees  = 1,
        [EnumMember(Value = "ปริญญาโท")]
        MasterDegrees = 2,
        [EnumMember(Value = "ปริญญาเอก")]
        DoctorDegrees = 3
    }

    /// <summary>
    /// Rounding type
    /// </summary>
    public enum RoundingType
    {
        /// <summary>
        /// Default rounding (Match.Round(num, 2))
        /// </summary>
        Rounding001 = 0,

        /// <summary>
        /// <![CDATA[Prices are rounded up to the nearest multiple of 5 cents for sales ending in: 3¢ & 4¢ round to 5¢; and, 8¢ & 9¢ round to 10¢]]>
        /// </summary>
        Rounding005Up = 10,

        /// <summary>
        /// <![CDATA[Prices are rounded down to the nearest multiple of 5 cents for sales ending in: 1¢ & 2¢ to 0¢; and, 6¢ & 7¢ to 5¢]]>
        /// </summary>
        Rounding005Down = 20,

        /// <summary>
        /// <![CDATA[Round up to the nearest 10 cent value for sales ending in 5¢]]>
        /// </summary>
        Rounding01Up = 30,

        /// <summary>
        /// <![CDATA[Round down to the nearest 10 cent value for sales ending in 5¢]]>
        /// </summary>
        Rounding01Down = 40,

        /// <summary>
        /// <![CDATA[Sales ending in 1–24 cents round down to 0¢
        /// Sales ending in 25–49 cents round up to 50¢
        /// Sales ending in 51–74 cents round down to 50¢
        /// Sales ending in 75–99 cents round up to the next whole dollar]]>
        /// </summary>
        Rounding05 = 50,

        /// <summary>
        /// Sales ending in 1–49 cents round down to 0
        /// Sales ending in 50–99 cents round up to the next whole dollar
        /// For example, Swedish Krona
        /// </summary>
        Rounding1 = 60,

        /// <summary>
        /// Sales ending in 1–99 cents round up to the next whole dollar
        /// </summary>
        Rounding1Up = 70
    }
    public enum UserType
    {
        [EnumMember(Value = "ผู้ดูแลระบบ")]
        Researchers = 1,
        [EnumMember(Value = "นักวิจัย")]
        Administrators = 2,
        [EnumMember(Value = "เจ้าหน้าที่สถาบันวิจัยและพัฒนา")]
        ResearchDevelopmentInstituteStaffs = 3,
        [EnumMember(Value = "ผู้ประสานงานวิจัย")]
        ResearchCoordinators = 4,
        [EnumMember(Value = "ผู้บริหาร")]
        Managements = 5,
        [EnumMember(Value = "Guests")]
        Guests = 6
    }

    /// <summary>
    /// Represents the customer name formatting enumeration
    /// </summary>
    public enum UserNameFormat
    {
        /// <summary>
        /// Show emails
        /// </summary>
        ShowEmails = 1,

        /// <summary>
        /// Show usernames
        /// </summary>
        ShowUsernames = 2,

        /// <summary>
        /// Show full names
        /// </summary>
        ShowFullNames = 3,

        /// <summary>
        /// Show first name
        /// </summary>
        ShowFirstName = 10
    }

    /// <summary>
    /// Represents the customer registration type formatting enumeration
    /// </summary>
    public enum UserRegistrationType
    {
        /// <summary>
        /// Standard account creation
        /// </summary>
        Standard = 1,

        /// <summary>
        /// Email validation is required after registration
        /// </summary>
        EmailValidation = 2,

        /// <summary>
        /// A customer should be approved by administrator
        /// </summary>
        AdminApproval = 3,

        /// <summary>
        /// Registration is disabled
        /// </summary>
        Disabled = 4
    }

    /// <summary>
    /// Represents the customer login result enumeration
    /// </summary>
    public enum UserLoginResults
    {
        /// <summary>
        /// Login successful
        /// </summary>
        Successful = 1,

        /// <summary>
        /// User does not exist (email or username)
        /// </summary>
        UserNotExist = 2,

        /// <summary>
        /// Wrong password
        /// </summary>
        WrongPassword = 3,

        /// <summary>
        /// Account have not been activated
        /// </summary>
        NotActive = 4,

        /// <summary>
        /// User has been deleted 
        /// </summary>
        Deleted = 5,

        /// <summary>
        /// User not registered 
        /// </summary>
        NotRegistered = 6,

        /// <summary>
        /// Locked out
        /// </summary>
        LockedOut = 7
    }

    /// <summary>
    /// Represents priority of queued email
    /// </summary>
    public enum QueuedEmailPriority
    {
        /// <summary>
        /// Low
        /// </summary>
        Low = 0,

        /// <summary>
        /// High
        /// </summary>
        High = 5
    }
    /// <summary>
    /// Represents the period of message delay
    /// </summary>
    public enum MessageDelayPeriod
    {
        /// <summary>
        /// Hours
        /// </summary>
        Hours = 0,

        /// <summary>
        /// Days
        /// </summary>
        Days = 1
    }

    /// <summary>
    /// Represents a picture item type
    /// </summary>
    public enum PictureType
    {
        /// <summary>
        /// Entities (products, categories, manufacturers)
        /// </summary>
        Entity = 1,
        /// <summary>
        /// Pdf
        /// </summary>
        Pdf = 2,

        /// <summary>
        /// Avatar
        /// </summary>
        Avatar = 10
    }

    /// <summary>
    /// Password format
    /// </summary>
    public enum PasswordFormat
    {
        /// <summary>
        /// Clear
        /// </summary>
        Clear = 0,

        /// <summary>
        /// Hashed
        /// </summary>
        Hashed = 1,

        /// <summary>
        /// Encrypted
        /// </summary>
        Encrypted = 2
    }

    /// <summary>
    /// Represents a log level
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Debug
        /// </summary>
        Debug = 10,

        /// <summary>
        /// Information
        /// </summary>
        Information = 20,

        /// <summary>
        /// Warning
        /// </summary>
        Warning = 30,

        /// <summary>
        /// Error
        /// </summary>
        Error = 40,

        /// <summary>
        /// Fatal
        /// </summary>
        Fatal = 50
    }

    /// <summary>
    /// Represents data provider type enumeration
    /// </summary>
    public enum DataProviderType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember(Value = "")]
        Unknown,

        /// <summary>
        /// MS SQL Server
        /// </summary>
        [EnumMember(Value = "sqlserver")]
        SqlServer
    }
    public enum ProjectStatus
    {
        [EnumMember(Value = "รอการตรวจสอบ")]
        WaitingApproval = 1,

        [EnumMember(Value = "อนุมัติ")]
        Approved = 2,

        [EnumMember(Value = "ไม่อนุมัติ")]
        NotApprove = 3
    }

    public enum ProfessorType
    {

        [EnumMember(Value = "ผู้ทรงคุณวุฒิภายใน")]
        InternalExpert = 1,

        [EnumMember(Value = "ผู้ทรงคุณวุฒิภายนอก")]
        ExternalExpert = 2,
    }
    public enum ActiveStatus
    {
        [EnumMember(Value = "สำเร็จ")]
        Active = 1,

        [EnumMember(Value = "ไม่สำเร็จ")]
        NotActive = 9,
    }

    public enum ProgressStatus
    {
        [EnumMember(Value = "ดำเนินการวิจัย")]
        Started = 1,

        [EnumMember(Value = "รายงานความก้าวหน้า1")]
        ReportedProgress1 = 2,

        [EnumMember(Value = "รายงานความก้าวหน้า2")]
        ReportedProgress2 = 3,

        [EnumMember(Value = "ปิดเล่มโครงการวิจัย")]
        Closed = 4,

        [EnumMember(Value = "คืนทุนวิจัย")]
        Return = 5,
    }

    public enum ProjectRole
    {
        [EnumMember(Value = "หัวหน้าโครงการ")]
        ProjectManager = 1,

        [EnumMember(Value = "ผู้วิจัยร่วม")]
        AssistanceAcademic = 2,
    }

    public enum PersonalType
    {
        [EnumMember(Value = "สายวิชาการ")]
        Academic = 1,

        [EnumMember(Value = "สายสนับสนุนวิชาการ")]
        AssistanceAcademic = 2,
    }

    public enum Gender
    {
        [EnumMember(Value = "เพศชาย")]
        Male = 1,

        [EnumMember(Value = "เพศหญิง")]
        Female = 2,
    }
}