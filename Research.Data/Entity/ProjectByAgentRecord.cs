namespace Research.Data
{
    /// <summary>
    /// Represents a log model
    /// </summary>
    public partial class ProjectByAgentRecord
    {
        #region Properties

        public int TotalProject { get; set; }

        public int TotalReseacher { get; set; }

        public int TotalProfessor { get; set; }

        public string AgencyName { get; set; }

        public int NoOfProject { get; set; }

        public decimal FundAmount { get; set; }
        #endregion
    }
}