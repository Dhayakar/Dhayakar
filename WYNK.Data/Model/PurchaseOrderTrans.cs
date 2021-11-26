using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class PurchaseOrderTrans
    {
        [Key]
        public Int64 POTranID { get; set; }
        public string RandomUniqueID { get; set; }
        public int ItemID { get; set; }
        public decimal ItemQty { get; set; }
        public decimal PORecdQty { get; set; }
        public decimal POCancelledQty { get; set; }
        public int UOMID { get; set; }
        public decimal ItemRate { get; set; }
        public decimal ItemValue { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal GSTTaxValue { get; set; }
        public decimal CGSTPercentage { get; set; }
        public decimal CGSTTaxValue { get; set; }
        public decimal SGSTPercentage { get; set; }
        public decimal SGSTTaxValue { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string ParentPONumber { get; set; }
        public DateTime? ParentPODate { get; set; }
        public decimal IGSTTaxValue { get; set; }
        public decimal IGSTPercentage { get; set; }
        public decimal CESSPercentage { get; set; }
        public decimal CESSAmount { get; set; }
        public decimal AdditionalCESSPercentage { get; set; }
        public decimal AddCESSAmount { get; set; }



    }
}
