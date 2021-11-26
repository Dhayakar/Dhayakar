using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class VISUALACUITY
    {
        [Key]
        public int ID { get; set; }
        public int RegistrationTranID { get; set; }
        public string Description { get; set; }
        public int? SubCategory { get; set; }
        public string Ocular { get; set; }
        public string DistSph { get; set; }
        public string NearCyl { get; set; }
        public string PowerGlass { get; set; }
        public string N_V_DESC { get; set; }
        public string PinAxis { get; set; }//Date
        public DateTime Date { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string RefTag { get; set; }
        public Boolean Central { get; set; }
        public Boolean Steady { get; set; }
        public Boolean Maintained { get; set; }
        public Boolean Uncentral { get; set; }
        public Boolean Unsteady { get; set; }
        public Boolean Unmaintained { get; set; }
        public string ChartType { get; set; }
        public string Remarks { get; set; }

    }
}
