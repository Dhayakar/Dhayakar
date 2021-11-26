using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{

    public class CustomerOrderTran
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string COID { get; set; }
        public int LTID { get; set; }
        public int UOMID { get; set; }
        public int OrderedQty { get; set; }
        public int ReceivedQty { get; set; }
        public decimal? ItemRate { get; set; }
        public decimal? GrossValue { get; set; }
        public decimal? ItemValue { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GSTPercentage { get; set; }
        public decimal? GSTTaxValue { get; set; }
        public decimal? CGSTPercentage { get; set; }
        public decimal? CGSTTaxValue { get; set; }
        public decimal? SGSTPercentage { get; set; }
        public decimal? SGSTTaxValue { get; set; }
        public decimal? IGSTPercentage { get; set; }
        public decimal? TotalIGSTTaxValue { get; set; }
        public decimal? CESSPercentage { get; set; }
        public decimal? CESSAmount { get; set; }
        public decimal? AdditionalCESSPercentage { get; set; }
        public decimal? AddCESSPerAmt { get; set; }
        public Boolean IsCancelled { get; set; }
        public string ContraCustomerNo { get; set; }
        public DateTime? ContraCustomerDate { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }

}
