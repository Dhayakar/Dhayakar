using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class DrugExtension
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DrugextensionID { get; set; }
        public int CMPID { get; set; }
        public int DrugID { get; set; }
        public string CountryCode { get; set; }
        public string CountryLanguage { get; set; }
        public string DrugDescription { get; set; }
        public string DrugSubDescription { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

    }



}
