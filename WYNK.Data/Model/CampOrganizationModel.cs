using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
   public class Camporganization_Model
    {
        [Key]
        public int ID { get; set; }
        public string OrganizationName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int? Location { get; set; }
        public int City { get; set; }
        public int State { get; set; }
        public int Country { get; set; }
        public string ContactPerson { get; set; }
        public Int64 Phone { get; set; }
        public string EMailID { get; set; }
        public string Website { get; set; }
        public int OrganizationType { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int Createdby { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int? Updatedby { get; set; }
    }
}
