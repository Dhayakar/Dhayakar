using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class MedicalBill_Tran
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int? MedicalBillID { get; set; }
        public int DrugID { get; set; }
        public decimal? Quantity { get; set; }
        public string MedicalPrescriptionID { get; set; }
        public int? MedicalBillParentID { get; set; }
        public string UOM { get; set; }
        public bool? IsMedicinePrescribed { get; set; }
        public decimal? ItemRate { get; set; }
        public decimal? ItemValue { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GSTPercentage { get; set; }
        public decimal? GSTTaxValue { get; set; }
        public decimal? CGSTPercentage { get; set; }
        public decimal? CGSTTaxValue { get; set; }
        public decimal? SGSTPercentage { get; set; }
        public decimal? SGSTTaxValue { get; set; }
        public decimal? CESSPercentage { get; set; }
        public decimal? CESSValue { get; set; }
        public decimal? AdditionalCESSPercentage { get; set; }
        public decimal? AdditionalCESSValue { get; set; }
        public int? MedicalPrescriptionTranId { get; set; }
        public DateTime CreatedUTC { get; set; }
       // public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }

        public int? TaxID { get; set; }
        //  public int? UpdateddBy { get; set; }


        public decimal? ReturnQuantity { get; set; }
        public int? MedicalBillParentTranID { get; set; }
        public bool? Status { get; set; }
    }

}
