using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class preAuthorizationInsurance
    {
        [Key]
        public int ID                    { get; set; }
        public string UIN                   { get; set; }
        public int CmpID { get; set; }
        public string Healthinsurancecard { get; set; }
        public decimal AmtApproval           { get; set; }
        public int insuranceCardNo       { get; set; }
        public DateTime CardValidity          { get; set; }
        public DateTime preAuthorizationDate  { get; set; }
        public int policyNo              { get; set; }
        public string status                { get; set; }
        public string ModeOFCommunication   { get; set; }
        public DateTime? RequestSentOnDate     { get; set; }
        public bool isdelete              { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}

