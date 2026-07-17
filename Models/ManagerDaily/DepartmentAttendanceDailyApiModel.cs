using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_PAYROLLMOBILE.Models.ManagerDaily
{
 
    public  class DepartmentAttendanceDailyApiModel
    {
        public string EmployeeId { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? ShiftCode { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public int? WorkMinutes { get; set; }
        public decimal WorkHours { get; set; }
        public string AttendanceCode { get; set; } = string.Empty;
        public decimal? WorkDayFactor { get; set; }
    }

}
