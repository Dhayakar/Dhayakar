using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
   public class PatientInsuranceBilgTran
    {
        [Key]
        public int Id { get; set; }
        public int CmpID { get; set; }
        public int PAINSID { get; set; }
        public int PAID { get; set; }
        public decimal AmountAvailed { get; set; }
        public DateTime? DateAvailed { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        
        

    }
}
