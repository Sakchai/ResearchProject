namespace Project.Enum
{
    public enum ProjectStatus
    {
        [StringValue("รอการตรวจสอบ")]
        WaitingApproval = 1,

        [StringValue("อนุมัติ")]
        Approved = 2,

        [StringValue("ไม่อนุมัติ")]
        NotApprove = 3
    }

    public enum ProfessorType
    {
        [StringValue("ผู้ทรงคุณวุฒิภายใน")]
        InternalExpert = 1,

        [StringValue("ผู้ทรงคุณวุฒิภายนอก")]
        ExternalExpert = 2,
    }

    public enum ResearchStatus
    {
        [StringValue("ดำเนินการวิจัย")]
        Started = 1,

        [StringValue("รายงานความก้าวหน้า 1")]
        ReportedProgress1 = 2,

        [StringValue("รายงานความก้าวหน้า 2")]
        ReportedProgress2 = 3,

        [StringValue("ขยายเวลาวิจัย 1")]
        ExtendedTime1 = 4,

        [StringValue("ขยายเวลาวิจัย 2")]
        ExtendedTime2 = 5,

        [StringValue("ปิดเล่มโครงการวิจัย")]
        Closed = 6,

        [StringValue("คืนทุนวิจัย (ไม่สามารถทำอะไรได้อีก)")]
        Return = 7,
    }

    public enum ProjectRole
    {
        [StringValue("หัวหน้าโครงการ")]
        ProjectManager = 1,

        [StringValue("ผู้วิจัยร่วม")]
        AssistanceAcademic = 2,
    }

    public enum PersonType
    {
        [StringValue("สายวิชาการ")]
        Academic = 1,

        [StringValue("สายสนับสนุนวิชาการ")]
        AssistanceAcademic = 2,
    }

    public enum Gender
    {
        [StringValue("เพศชาย")]
        Male = 1,

        [StringValue("เพศหญิง")]
        Female = 2,
    }
}