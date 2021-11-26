using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class SurgeryCostDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string ICDCode { get; set; }
        public decimal? SURGERYCOST { get; set; }
        public string RoomType { get; set; }
        public decimal? PackageRate { get; set; }
        public decimal? DressingCharge { get; set; }
        public decimal? MedicationCharge { get; set; }
        public decimal? SurgeonCharge { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int CMPID { get; set; }
    }


}
