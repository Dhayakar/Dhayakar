using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class EstimateTracking
    {
        [Key]
        public int ID                    { get; set; }
        public int PreAID { get; set; }
        
        public string UIN                   { get; set; }
        public int CmpID { get; set; }

        public string ClaimNO { get; set; }
        public string status                { get; set; }
        public string ModeOFCommunication   { get; set; }
        public DateTime ApprovedDate { get; set; }
        public decimal ApprovedLimit { get; set; }
        
        public bool isdelete              { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}

