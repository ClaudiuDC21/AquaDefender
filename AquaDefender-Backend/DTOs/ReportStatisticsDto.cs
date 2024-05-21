using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaDefender_Backend.DTOs
{
    public class ReportStatisticsDto
    {
        public int TotalReports { get; set; }
        public int NewReports { get; set; }
        public int CasesInProgress { get; set; }
        public int ResolvedReports { get; set; }
        public int VeryLowSeverity { get; set; }
        public int LowSeverity { get; set; }
        public int MediumSeverity { get; set; }
        public int HighSeverity { get; set; }
        public int VeryHighSeverity { get; set; }
    }
}