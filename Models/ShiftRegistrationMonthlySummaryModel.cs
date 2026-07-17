using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_PAYROLLMOBILE.Models
{
    public class ShiftRegistrationMonthlySummaryModel
    {
        public string EmployeeId { get; set; } = "";
        public int Year { get; set; }
        public int Month { get; set; }
        public int RegisteredDays { get; set; }
        public int MaxDaysInMonth { get; set; }
    }

}
