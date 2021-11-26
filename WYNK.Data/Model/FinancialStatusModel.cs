using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WYNK.Data.Model
{
    public class FinancialStatus
    {
        [Key]
        public Int16 ID { get; set; }
        public int FYID { get; set; }
        public int FYStatus { get; set; }
        public DateTime DateTime { get; set; }
        public int ClosedBy { get; set; }
    }
}
