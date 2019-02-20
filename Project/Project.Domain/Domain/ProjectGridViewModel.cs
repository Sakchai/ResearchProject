﻿
using Project.Enum;
using System;
using System.Collections.Generic;

namespace Project.Domain
{
    /// <summary>
    /// A Researcher attached to a Project
    /// </summary>
    public class ProjectGridViewModel : BaseDomain
    {
        public int FiscalYear { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectNameTh { get; set; }
        public decimal FundAmount { get; set; }
        public DateTime StartContractDate { get; set; }

        public int ProjectStatusId { get; set; }
        public virtual string StartDate
        {
            get { return StartContractDate.ToString("dd/MM/yyyy"); }
        }
        public virtual string ProjectStatus { get { return ((ProjectStatus)ProjectStatusId).ToString(); } }
    }
}