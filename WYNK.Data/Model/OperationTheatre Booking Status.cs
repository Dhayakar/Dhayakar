using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class OperationTheatre_Booking_Status
    {
       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CMPID { get; set; }
        public int OPTID { get; set; }
        public DateTime Date { get; set; }
        public string HRS_FS_00 { get; set; }
        public string HRS_SS_00 { get; set; }
        public string HRS_FS_01 { get; set; }
        public string HRS_SS_01 { get; set; }
        public string HRS_FS_02 { get; set; }
        public string HRS_SS_02 { get; set; }
        public string HRS_FS_03 { get; set; }
        public string HRS_SS_03 { get; set; }
        public string HRS_FS_04 { get; set; }
        public string HRS_SS_04 { get; set; }
        public string HRS_FS_05 { get; set; }
        public string HRS_SS_05 { get; set; }
        public string HRS_FS_06 { get; set; }
        public string HRS_SS_06 { get; set; }
        public string HRS_FS_07 { get; set; }
        public string HRS_SS_07 { get; set; }
        public string HRS_FS_08 { get; set; }
        public string HRS_SS_08 { get; set; }
        public string HRS_FS_09 { get; set; }
        public string HRS_SS_09 { get; set; }
        public string HRS_FS_10 { get; set; }
        public string HRS_SS_10 { get; set; }
        public string HRS_FS_11 { get; set; }
        public string HRS_SS_11 { get; set; }
        public string HRS_FS_12 { get; set; }
        public string HRS_SS_12 { get; set; }
        public string HRS_FS_13 { get; set; }
        public string HRS_SS_13 { get; set; }
        public string HRS_FS_14 { get; set; }
        public string HRS_SS_14 { get; set; }
        public string HRS_FS_15 { get; set; }
        public string HRS_SS_15 { get; set; }
        public string HRS_FS_16 { get; set; }
        public string HRS_SS_16 { get; set; }
        public string HRS_FS_17 { get; set; }
        public string HRS_SS_17 { get; set; }
        public string HRS_FS_18 { get; set; }
        public string HRS_SS_18 { get; set; }
        public string HRS_FS_19 { get; set; }
        public string HRS_SS_19 { get; set; }
        public string HRS_FS_20 { get; set; }
        public string HRS_SS_20 { get; set; }
        public string HRS_FS_21 { get; set; }
        public string HRS_SS_21 { get; set; }
        public string HRS_FS_22 { get; set; }
        public string HRS_SS_22 { get; set; }
        public string HRS_FS_23 { get; set; }
        public string HRS_SS_23 { get; set; }
        public int TotalBookedHours { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string HRS_FS { get; set; }
    }
}
