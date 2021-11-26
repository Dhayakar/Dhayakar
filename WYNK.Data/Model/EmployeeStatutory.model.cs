using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class EmployeeStatutory
    {
        [Key]


        public int ID { get; set; }
        public int CmpID { get; set; }
        public int EmpID { get; set; }
        public bool IsPFEligible { get; set; }
        public string PFNumber { get; set; }
        public DateTime? PFCommencementDate { get; set; }
        public decimal VPFPercentage { get; set; }
        public bool IsESIEligible { get; set; }
        public string ESINumber { get; set; }
        public DateTime? ESICommencementDate { get; set; }
        public bool IsProfessionalTaxEligible { get; set; }      
        public string GratuityNo { get; set; }
        public string UANNo { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }



    }
}