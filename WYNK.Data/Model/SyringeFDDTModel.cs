using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class SYRINGEFDDT
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CMPID { get; set; }
        public int REGTRANID { get; set; }
        public string UIN { get; set; }
        public DateTime VISITDATE { get; set; }
        public string Eye { get; set; }
        public string Stop { get; set; }
        public int FDDTSYRINGEID { get; set; }
        public string FDDTDESCRIPTION { get; set; }
        public string REGURGITATION { get; set; }
        public string FLUID { get; set; }
        public Boolean IsActive { get; set; }
        public int? DoctorID { get; set; }
        public string RemovalReasons { get; set; }
        public DateTime? RemovedDateTime { get; set; }
        public int? Removedby { get; set; }
        public string REMARKS { get; set; }
        public int CREATEDBY { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime CREATEDUTC { get; set; }
        public DateTime? UPDATEDUTC { get; set; }

    }
}
