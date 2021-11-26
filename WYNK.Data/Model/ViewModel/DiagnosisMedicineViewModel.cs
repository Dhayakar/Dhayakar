using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class diagnosisvsmedicine
    {
        public ICollection<DiagnosisVSMedicine> DiagnosisVSMedicine { get; set; }
        public ICollection<Specialitydetialsdm> Specialitydetialsdm { get; set; }//NONSpecialitydetialsdm
        public ICollection<NONSpecialitydetialsdm> NONSpecialitydetialsdm { get; set; }
        public ICollection<SpecialityDetaildm> SpecialityDetaildm { get; set; }
        public ICollection<SSpecialityDetaildm> SSpecialityDetaildm { get; set; }
        public int Code { get; set; }//SubCode
        public string SubCode { get; set; }
        public string CompanyID { get; set; }
        public string UserID { get; set; }
    }

    public class Specialitydetialsdm
    {
        public int drugid { get; set; }
        public int genericid { get; set; }
        public string genericname { get; set; }

        public string Itemdescription { get; set; }
        public string Itemtag { get; set; }
        public Boolean Itemselect { get; set; }

    }

    public class NONSpecialitydetialsdm
    {
        public string Itemdescription { get; set; }//genericname
        public string genericname { get; set; }
        public Boolean Itemselect { get; set; }

    }

    public class SpecialityDetaildm
    {
        public string Description { get; set; }//Descriptionsub
        public string Descriptionsub { get; set; }
        public Boolean Select { get; set; }

    }

    public class SSpecialityDetaildm
    {
        public string Description { get; set; }
        public string Descriptionsub { get; set; }
        public Boolean Select { get; set; }

    }

}
