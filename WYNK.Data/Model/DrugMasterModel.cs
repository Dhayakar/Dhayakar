using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class Drug_Master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Brand { get; set; }
        public int? Manufacturer { get; set; }
        public int GenericName { get; set; }
        public string DrugGroup { get; set; }
        public string UOM { get; set; }
        public decimal Rate { get; set; }
        public string HSNNo { get; set; }
        public decimal? Gst { get; set; }
        public decimal? SGst { get; set; }
        public decimal? CGst { get; set; }
        public decimal? CESSPercentage { get; set; }
        public decimal? AdditionalCESSPercentage { get; set; }
        public int? TaxID { get; set; }
        public int? IGST { get; set; }
        public int? VendorCode { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public Boolean IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string DrugImagePath { get; set; }
        public int? DrugCategory { get; set; }
        public string DrugSubDescription { get; set; }
        public string DrugComposition { get; set; }
        public string Power { get; set; }
        public string Aconstant { get; set; }
        public string ModelNo { get; set; }
        public string OpticDia { get; set; }
        public string Length { get; set; }
        public int? DrugTracker { get; set; }
        public int Cmpid { get; set; }
    }

}
