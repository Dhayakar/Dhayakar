using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class InvestigationExtn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }//ICDCode
        public int CmpID { get; set; }
        public string UIN { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int DoctorID { get; set; }
        public Boolean Tag { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int? UpdatedBy { get; set; }
        public string IPID { get; set; }


    }
}


