using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class InvestigationPrescriptionk
    {
        public InvestigationPrescription InvPrescription { get; set; }
        public ICollection<InvestigationPrescriptionTran> InvPsT { get; set; }
        public ICollection<UploadInvestigationPrescription> UploadInvestigationPrescription { get; set; }
        public ICollection<invdtt> invdtt { get; set; }
        public ICollection<FBdetail> FBdetail { get; set; }
        public string Companyname { get; set; }
    }


    public class invdtt

    {
        public int speid { get; set; }
        public string spename { get; set; }
        public int invid { get; set; }
        public string invName { get; set; }
        public string dname { get; set; }

    }


}

