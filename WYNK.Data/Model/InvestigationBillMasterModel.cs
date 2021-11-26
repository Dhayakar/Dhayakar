using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{

    public class InvestigationBillMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CMPID { get; set; }
        public int TransactionId { get; set; }
        public int? RegistrationTranID { get; set; }
        public string UIN { get; set; }
        public string BillNo { get; set; }
        public int? PAID { get; set; }//IsDeleted
        public int? ContraID { get; set; }

        public decimal GrossProductValue { get; set; }
        public decimal? TotalDiscountValue { get; set; }
        public decimal? TotalTaxValue { get; set; }
        public decimal? TotalCGSTTaxValue { get; set; }
        public decimal? TotalSGSTTaxValue { get; set; }
        public decimal? TotalIGSTTaxValue { get; set; }
        public decimal TotalBillValue { get; set; }
        public decimal? CESSAmount { get; set; }
        public decimal? AdditionalCESSAmount { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }

}
