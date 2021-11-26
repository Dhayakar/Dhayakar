using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class SquintExt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }//CMPID
        public int CMPID { get; set; }
        public string FindingsID { get; set; }
        public string UIN { get; set; }
        public DateTime VisitDate { get; set; }
        public int Doctorid { get; set; }
        public int? AngleKappa { get; set; }
        public int? Patterns { get; set; }
        public int? ACAMethod { get; set; }
        public int? ACAValue { get; set; }
        public int? WFDTDistance { get; set; }
        public int? WFDTNear { get; set; }
        public int? StreopsisMethod { get; set; }
        public int? StreopsisValue { get; set; }
        public int? ARC { get; set; }
        public int? PBCTDisHor { get; set; }
        public string PBCTDisHorValue { get; set; }
        public int? PBCTDisVer { get; set; }
        public string PBCTDisVerValue { get; set; }
        public int? PBCTNearHor { get; set; }
        public string PBCTNearHorValue { get; set; }
        public int? PBCTNearVer { get; set; }
        public string PBCTNearVerValue { get; set; }
        public int? ModKrimHor { get; set; }
        public string ModKrimHorValue { get; set; }
        public int? ModKrimVer { get; set; }
        public string ModKrimVerValue { get; set; }
        public int? PriDevHor { get; set; }
        public string PriDevHorValue { get; set; }
        public int? PriDevVer { get; set; }
        public string PriDevVerValue { get; set; }
        public int? SecDevHor { get; set; }
        public string SecDevHorValue { get; set; }
        public int? SecDevVer { get; set; }
        public string SecDevVerValue { get; set; }
        public int? Amplitude { get; set; }
        public int? Frequency { get; set; }
        public int? Type { get; set; }
        public string Pursuit { get; set; }
        public string Saccade { get; set; }
        public string ConjugateDissociated { get; set; }
        public int? ABHeadPos { get; set; }
        public int? FreqOnConvergence { get; set; }
        public int? Occlusion { get; set; }
        public int? Oscillopsia { get; set; }
        public string AssHeadPosture { get; set; }
        public string SquintBasicExam { get; set; }
        public string SpecialTest { get; set; }
        public Boolean IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedUTC { get; set; }

        public int? VF1 { get; set; }
        public string VF1Value { get; set; }
        public int? VF2 { get; set; }
        public string VF2Value { get; set; }
        public int? VF3 { get; set; }
        public string VF3Value { get; set; }
        public int? VF4 { get; set; }
        public string VF4Value { get; set; }


    }
}


