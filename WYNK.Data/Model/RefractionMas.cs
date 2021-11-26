using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class RefractionMas
    {
        [Key]
        public int ID { get; set; }
        public int RegistrationTranID { get; set; }
        public string UIN { get; set; }
        public string Description { get; set; }
        public int? SubCategory { get; set; }
        public int? Type { get; set; }
        public string Ocular { get; set; }
        public string DistSph { get; set; }
        public string NearCyl { get; set; }
        public string PinAxis { get; set; }
        public string Add { get; set; }
        public string PowerGlass { get; set; }
        public string N_V_DESC { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string Iolnct { get; set; }
        public string Iolat { get; set; }
        public string Details { get; set; }
        public string Amsler { get; set; }
        public string Desc_Text { get; set; }
        public string CV_normal { get; set; }
        public string CV_defective { get; set; }
        public Boolean A_n_OD { get; set; }
        public Boolean A_abn_OD { get; set; }
        public Boolean A_n_OS { get; set; }
        public Boolean A_abn_OS { get; set; }
        public string PD { get; set; }
        public string Time { get; set; }
        public string Iolnctbd { get; set; }
        public string MPDOD { get; set; }
        public string MPDOS { get; set; }
        public Boolean Central { get; set; }
        public Boolean Steady { get; set; }
        public Boolean Maintained { get; set; }
        public Boolean Uncentral { get; set; }
        public Boolean Unsteady { get; set; }
        public Boolean Unmaintained { get; set; }
        public string ChartType { get; set; }


    }
}








