using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
    public class OpticalSummary
    {
        [Key]
        public string RandomUniqueID { get; set; }
        public int CmpID { get; set; }
        public DateTime Date { get; set; }
        public int? FrameLensType { get; set; }
        public int? Brand { get; set; }
        public int? LensPower { get; set; }
        public decimal? BilledNumbers { get; set; }
        public decimal? BilledAmount { get; set; }
        public decimal? CollectedAmount { get; set; }
        public int Createdby { get; set; }
        public int? Updatedby { get; set; }
    }
}
