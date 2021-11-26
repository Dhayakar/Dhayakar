using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;


namespace WYNK.Data.Model
{
    public class Monitoring
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CMPID { get; set; }
        public string SAID { get; set; }
        public string UIN { get; set; }
        public Boolean? ECG { get; set; }
        public string ECGLeadsDesc { get; set; }
        public Boolean? SpO2 { get; set; }
        public string SpO2SiteDesc { get; set; }
        public string Temp { get; set; }
        public string TempDesc { get; set; }
        public Boolean? FiO2 { get; set; }
        public string ETCO2 { get; set; }
        public string ETCO2Desc { get; set; }
        public Boolean? InspETAA { get; set; }
        public Boolean? AirwayPress { get; set; }
        public Boolean? NIBP { get; set; }
        public Boolean? IBP { get; set; }
        public string IBPSiteDesc { get; set; }
        public Boolean? CVP { get; set; }
        public string CVPSiteDesc { get; set; }
        public Boolean? PCWP { get; set; }
        public Boolean? Resp { get; set; }
        public string RespSource { get; set; }
        public Boolean? BloodLoss { get; set; }
        public Boolean? UrineOutput { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}


