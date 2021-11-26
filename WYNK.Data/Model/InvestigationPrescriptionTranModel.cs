using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class InvestigationPrescriptionTran
    {
        [Key]
        public int ID { get; set; }
        public string IPID { get; set; }
        public int InvestigationID { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int SpecialityID { get; set; }
        

    }
}

