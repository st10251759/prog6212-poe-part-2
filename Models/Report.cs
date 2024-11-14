namespace ST10251759_PROG6212_POE.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FilePath { get; set; }
    }
}
