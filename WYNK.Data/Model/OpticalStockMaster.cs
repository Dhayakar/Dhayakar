using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WYNK.Data.Model
{

    public class OpticalStockMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 SMID { get; set; }
        public int TransactionID { get; set; }
        public int CMPID { get; set; }
        public string DocumentNumber { get; set; }
        public string Fyear { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string OpticalOrderID { get; set; }
        public int StoreID { get; set; }
        public string TransactionType { get; set; }
        public int VendorID { get; set; }
        public int DepartmentID { get; set; }
        public Decimal? GrossProductValue { get; set; }
        public Decimal? TotalDiscountValue { get; set; }
        public Decimal? TotalTaxValue { get; set; }
        public Decimal? TotalCGSTTaxValue { get; set; }
        public Decimal? TotalSGSTTaxValue { get; set; }
        public Decimal? TotalIGSTTaxValue { get; set; }
        public Decimal? TotalPOValue { get; set; }
        public Decimal? CESSAmount { get; set; }
        public Decimal? AdditionalCESSAmount { get; set; }
        public Boolean IsCancelled { get; set; }
        public string TermsConditions { get; set; }
        public Boolean IsDeleted { get; set; }
        public int? ReceiverStoreID { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string RandomUniqueID { get; set; }


    }
}
