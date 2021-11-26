using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class TaxMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string TaxDescription { get; set; }
        public Int16? GSTPercentage { get; set; }
        public decimal? CGSTPercentage { get; set; }
        public decimal? SGSTPercentage { get; set; }
        public Int16? IGSTPercentage { get; set; }
        public Int16 TaxGroupId { get; set; }

        public string CESSDescription { get; set; }
        public decimal? CESSPercentage { get; set; }
        public string AdditionalCESSDescription { get; set; }
        public decimal? AdditionalCESSPercentage { get; set; }

        public bool IsActive { get; set; }
        public bool istransact { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }



    }
}

