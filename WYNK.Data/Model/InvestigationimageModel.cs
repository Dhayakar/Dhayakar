using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class InvestigationImages
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CmpID { get; set; }
        public string UIN { get; set; }
        public int? RegistrationTranID { get; set; }
        public int InvestigationID { get; set; }
        public string InvestigationDescription { get; set; }
        public int InvestigationTranID { get; set; }
        public string ImageLocation { get; set; }
        public string ExternalInternal { get; set; }
        public string ImageCapturedLocation { get; set; }
        public Decimal? InvestigationAmount { get; set; }
        public string Remarks { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ReferredBy { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? TaxID { get; set; }
        public int? TaxPercentage { get; set; }
        public Decimal? TaxValue { get; set; }
        public Decimal? DiscountPercentage { get; set; }
        public Decimal? DiscountValue { get; set; }
        public Decimal TotalValue { get; set; }


    }
}


