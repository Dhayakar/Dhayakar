using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
    public class CampPatientFootfallTran
    {
        [Key]
        public string RandomUniqueID { get; set; }
        public string CAMPFFId { get; set; }
        public int CmpID { get; set; }
        public DateTime DateofSurgery { get; set; }
        public int SpecialityID { get; set; }
        public string SpecialityDesc { get; set; }
        public int? GeneralNormal { get; set; }
        public int? GeneralOcular { get; set; }
        public int? PaediatricNormal { get; set; }
        public int? PaediatricOcular { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int Createdby { get; set; }
        public int? Updatedby { get; set; }
    }
}
