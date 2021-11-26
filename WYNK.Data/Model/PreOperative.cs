using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
   public  class PreOperative
    {
        [Key]
        public int PreopID { get; set; }
        public int CMPID { get; set; }
        public int TransactionId { get; set; }
        public string UIN { get; set; }
        public DateTime PreOpDate { get; set; }
        public int SurgeryID { get; set; }
        public string AntibioticGivenBy { get; set; }
        public string CaseSheetPreparedBy { get; set;}
        public string Note { get; set; }
        public string Instruction { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}

  