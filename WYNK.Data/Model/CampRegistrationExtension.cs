using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class CampRegistrationExtension
    {
        [Key]

        public int ID { get; set; }
        public string CampUIN { get; set; }
        public bool Artificialeye { get; set; }
        public bool OD { get; set; }
        public bool OU { get; set; }
        public bool OS { get; set; }
        public int CMPID { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
  


    }


}
