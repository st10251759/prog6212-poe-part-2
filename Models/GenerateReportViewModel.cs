using System;
using System.Collections.Generic;

namespace ST10251759_PROG6212_POE.Models
{
    public class GenerateReportViewModel
    {
        public string ReportName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ReportType { get; set; }

        public List<Report> ExistingReports { get; set; } = new List<Report>();// To display existing reports
    }
}
