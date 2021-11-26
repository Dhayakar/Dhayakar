using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class MainItemBalance
    {
        [Key]
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int? UOMID { get; set; }
        public int FYear { get; set; }
        public int OpeningBalance { get; set; }
        public int StoreID { get; set; }
        public int Rec01 { get; set; }
        public int Rec02 { get; set; }
        public int Rec03 { get; set; }
        public int Rec04 { get; set; }
        public int Rec05 { get; set; }
        public int Rec06 { get; set; }
        public int Rec07 { get; set; }
        public int Rec08 { get; set; }
        public int Rec09 { get; set; }
        public int Rec10 { get; set; }
        public int Rec11 { get; set; }
        public int Rec12 { get; set; }
        public int Iss01 { get; set; }
        public int Iss02 { get; set; }
        public int Iss03 { get; set; }
        public int Iss04 { get; set; }
        public int Iss05 { get; set; }
        public int Iss06 { get; set; }
        public int Iss07 { get; set; }
        public int Iss08 { get; set; }
        public int Iss09 { get; set; }
        public int Iss10 { get; set; }
        public int Iss11 { get; set; }
        public int Iss12 { get; set; }
        public int ClosingBalance { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int CmpID { get; set; }
    }
}
