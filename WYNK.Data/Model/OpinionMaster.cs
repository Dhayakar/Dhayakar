using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class OpinionMaster
    {
        [Key]
        public int ID { get; set; }
        public int RegistrationTranID { get; set; }
        public int ContraOpionionID { get; set; }
        public int FromDoctor { get; set; }
        public int ToDoctor { get; set; }
        public string OpinionDescription { get; set; }
        public int OpinionStatus { get; set; }
        public string OpinionTriggeredFrom { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}

