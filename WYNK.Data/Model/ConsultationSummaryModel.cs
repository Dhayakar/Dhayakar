using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class ConsultantionSummary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }//ICDCode
        public int CmpID { get; set; }
        public int? ParentID { get; set; }
        public DateTime Date { get; set; }
        public int DoctorID { get; set; }
        public Decimal? ConsultationCharges { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int CreatedBy { get; set; }


    }
}


