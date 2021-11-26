using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace WYNK.Data.Model
{
    public class ProTax
    {
        [Key]
        public int ID { get; set; }
        public int CMPID { get; set; }
        public DateTime WithEffectFrom { get; set; }
        public int Frequency { get; set; }
        public int StateID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

    }
}
