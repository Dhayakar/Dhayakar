using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{

    public class Drug_Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Description { get; set; }
        public int? DrugFormID { get; set; }
        public string SideEffects { get; set; }
        public string Precautions { get; set; }
        public string Overdose { get; set; }
        public Boolean IsStepDown { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public Boolean IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public Int16 RetestInterval { get; set; }
        public Int16 RetestCriticalInterval { get; set; }
        public int Cmpid { get; set; }
    }


}
