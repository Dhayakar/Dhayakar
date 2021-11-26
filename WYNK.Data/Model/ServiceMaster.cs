using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace WYNK.Data.Model
{
   public class ServiceMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string parentDescription { get; set; }
        public string Tag { get; set; }
        public string ChildDescription { get; set; }
        public int DoctorID { get; set; }
        public int InsuranceID { get; set; }
        public int VisitType { get; set; }
        public int RoomID { get; set; }
        public int CMPID { get; set; }    
        public int DiscountPercentage { get; set; }
        public decimal AmountEligible { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal? TotalAmount { get; set; }
        public int IcdSpecialityCode { get; set; }
        public string Icdcode { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public Boolean IsActive { get; set; }
       
    }
}
