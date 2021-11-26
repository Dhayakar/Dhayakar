using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class OpticalOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID                     { get; set; }
        public int CmpID                  { get; set; }
        public string OrderNumber         { get; set; }
        public DateTime OrderDate         { get; set; }
        public string RefNo               { get; set; }
        public DateTime? RefDate          { get; set; }
        public int TransactionID          { get; set; }
        public int VendorID               { get; set; }
        public string DeliveryName        { get; set; }
        public DateTime? DeliveryDate     { get; set; }
        public string Instructions        { get; set; }
        public string DeliveryAddress1    { get; set; }
        public string DeliveryAddress2    { get; set; }
        public string DeliveryAddress3    { get; set; }
        public int?    DeliveryLocationID { get; set; }
        public Decimal GrossProductValue  { get; set; }
        public Decimal TotalProductValue  { get; set; }
        public Decimal TotalDiscountAmount{ get; set; }
        public Decimal TotalGSTTaxValue   { get; set; }
        public Decimal TotalCGSTTaxValue  { get; set; }
        public Decimal TotalSGSTTaxValue  { get; set; }
        public Decimal TotalIGSTTaxValue  { get; set; }
        public Decimal CESSAmount         { get; set; }
        public Decimal AddCESSPerAmt      { get; set; }
        public bool IsCancelled           { get; set; }
        public string TermsAndConditions  { get; set; }
        public Int16 Validity             { get; set; }
        public int IsOrderExecuted        { get; set; }
        public DateTime CreatedUTC        { get; set; }
        public DateTime? UpdatedUTC       { get; set; }
        public int CreatedBy              { get; set; }
        public int? UpdatedBy             { get; set; }
        public string Fyear               { get; set; }
        public string RandomUniqueID { get; set; }






    }
}
