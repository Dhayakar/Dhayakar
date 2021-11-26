using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WYNK.Data.Model
{
    public class RunningNumberControl
    {
        [Key]
        public string RnControl_Code { get; set; }
        public string Control_Description { get; set; }
        public decimal Control_Value { get; set; }
        public string Control_String_Value { get; set; }
        public Int16 Length { get; set; }
        public DateTime StartPeriod { get; set; }
        public DateTime? EndPeriod { get; set; }
        public int SiteId { get; set; }
        public bool IsActive { get; set; }

        internal static object Equals(Func<int, string> generateRunningCtrlNoo)
        {
            throw new NotImplementedException();
        }
    }
}
