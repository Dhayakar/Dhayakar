using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class SurgeryMaster
    {
        [Key]
        public int ID { get; set; }
        public int RegistrationTranID { get; set; }
        public string UIN { get; set; }
        public int? OTID { get; set; }
        public string AdmID { get; set; }
        public DateTime? DateofSurgery { get; set; }
        public string Remarks { get; set; }
        public string SMID { get; set; }
        public decimal? SurgeryCost { get; set; }
        public bool SurgeryPerformedInHouse { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? ReviewDate { get; set; }
        public int CMPID { get; set; }
        public string RandomUniqueID { get; set; }
    }
}

