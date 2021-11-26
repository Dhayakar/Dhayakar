using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
    public class SetupCountry
    {

        [Key]
        public int ID { get; set; }
        public string CountryName { get; set; }
        public string Currency { get; set; }
        public string CountryCode { get; set; }
        
        public string CountryDecimaldescription { get; set; }
        
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}
