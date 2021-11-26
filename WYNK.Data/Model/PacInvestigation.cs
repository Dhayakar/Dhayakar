using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace WYNK.Data.Model
{
    public class PACInvestigation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PACInvestigationID { get; set; }
        public int CMPID { get; set; }
        public int ADMID { get; set; }
        public string UIN { get; set; }
        public string Hb_PCV { get; set; }
        public decimal PLCount { get; set; }
        public string BLGlu { get; set; }
        public Int16 SUricAcid { get; set; }
        public string T3 { get; set; }
        public string T4 { get; set; }
        public string TSH { get; set; }
        public string SElectNAPlus { get; set; }
        public string SElectKPlus { get; set; }
        public string SElectCAPlus { get; set; }
        public string UrineRMALB { get; set; }
        public string UrineRMSugar { get; set; }
        public string UrineRMKET { get; set; }
        public string UrineRMPlusCells { get; set; }
        public string UrineRMWBC { get; set; }
        public string UrineRMRBC { get; set; }
        public string XRay { get; set; }
        public string ECG { get; set; }
        public string ECHO { get; set; }
        public string ABG { get; set; }
        public string SpecialInvestigation { get; set; }
        public string TLC { get; set; }
        public string BlGroup { get; set; }
        public decimal BUrea { get; set; }
        public decimal Creatinine { get; set; }
        public string HBSAG { get; set; }
        public string HIV { get; set; }

        public string DLC { get; set; }
        public string PT { get; set; }
        public string PTTK { get; set; }
        public string SBil { get; set; }
        public string SGOTSGPT { get; set; }
        public string ALKPOGGT { get; set; }
        public string SProtein { get; set; }
        public string UnfitReasons { get; set; }
        public string Reviewedby1 { get; set; }
        public string Reviewedby2 { get; set; }
        public string Reviewedby3 { get; set; }
        public DateTime? Reviewedon1 { get; set; }
        public DateTime? Reviewedon2 { get; set; }
        public DateTime? Reviewedon3 { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
        public string Remarks3 { get; set; }
        public string PlanGA { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        public int DoctorID { get; set; }
    }
}
