using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
    public class CampMaster_Model
    {
        [Key]
        public int CampID { get; set; }
        public string CampName { get; set; }
        public DateTime Campfrom { get; set; }
        public DateTime Campto { get; set; }
        public int Organisedby { get; set; }
        public int? Location { get; set; }
        public int City { get; set; }
        public int State { get; set; }
        public int Country { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int Createdby { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int? Updatedby { get; set; }
        public Boolean IsActive { get; set; }
    }
}
