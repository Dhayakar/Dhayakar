using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class specialityvstest
    {
        public ICollection<SpecialityVSTest> SpecialityVSTest { get; set; }
        public ICollection<InvName> InvName { get; set; }
        public ICollection<Specialitydetials> Specialitydetials { get; set; }
        public ICollection<NONSpecialitydetials> NONSpecialitydetials { get; set; }
        public ICollection<SpecialityDetail> SpecialityDetail { get; set; }
        public ICollection<SSpecialityDetail> SSpecialityDetail { get; set; }
        public OneLine_Masters OneLineMaster { get; set; }
        public int Code { get; set; }
        public string CompanyID { get; set; }
        public string UserID { get; set; }
    }

    public class InvName
    {

        public string Description { get; set; }

    }

    public class SpecialityDetail
    {
        public string Description { get; set; }
        public Boolean Select { get; set; }

    }

    public class SSpecialityDetail
    {
        public string Description { get; set; }
        public Boolean Select { get; set; }

    }

    public class Specialitydetials
    {
        public string Itemdescription { get; set; }
        public string Itemtag { get; set; }
        public Boolean Itemselect { get; set; }

    }

    public class NONSpecialitydetials
    {
        public string Itemdescription { get; set; }
        public Boolean Itemselect { get; set; }

    }

}
