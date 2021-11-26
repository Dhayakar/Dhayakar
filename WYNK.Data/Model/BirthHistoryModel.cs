using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace WYNK.Data.Model
{
    public class BirthHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CMPID { get; set; }
        public int RegistrationTranID { get; set; }
        public string UIN { get; set; }
        public string DeliveryType { get; set; }
        public decimal Weight { get; set; }
        public string BloodTransplant { get; set; }
        public string OxygenTherapy { get; set; }
        public string criticalillness { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string Vaccination { get; set; }
        public string GrowthDev { get; set; }
        public string Unit { get; set; }


    }
}
