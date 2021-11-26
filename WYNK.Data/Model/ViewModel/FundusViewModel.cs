using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class FundusViewM
    {
        public OneLine_Masters oneLine_Masters { get; set; }
        public ICollection<Descriptionfn> Descriptionfn { get; set; }//Descriptionsltssf
        public ICollection<Descriptionsltssf> Descriptionsltssf { get; set; }
        public ICollection<Descriptionsltsf> Descriptionsltsf { get; set; }
        public ICollection<Descriptionfu> Descriptionfu { get; set; }
        public FundusMaster FundusMaster { get; set; }
        public ICollection<SubslitDescription> SubslitDescription { get; set; }
        public ICollection<AddslitDescription> AddslitDescription { get; set; }
        public ICollection<DescriptionDtls> DescriptionDtls { get; set; }
        public ICollection<Descriptiontbfn> Descriptiontbfn { get; set; }
    }

    public class Descriptionfn
    {
        public int ID { get; set; }
        public string ParentDescription { get; set; }
        public string Righteye { get; set; }
        public string Lefteye { get; set; }
        public string Remarks { get; set; }
        public Boolean OD { get; set; }
        public Boolean OS { get; set; }
    }
    public class Descriptionsltssf
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string PTag { get; set; }
        public int? PID { get; set; }
        public bool IsActive { get; set; }
        public string IsActive1 { get; set; }

    }
    public class Descriptionsltsf
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string PTag { get; set; }
        public int? PID { get; set; }
        public bool IsActive { get; set; }
        public string IsActive1 { get; set; }

    }
    public class Descriptionfu
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string PTag { get; set; }
        public int? PID { get; set; }
        public bool IsActive { get; set; }
        public string IsActive1 { get; set; }

    }
    public class Descriptiontbfn
    {
        public int ID { get; set; }
        public string ParentDescription { get; set; }
        public ICollection<subfundus> subfundus { get; set; }
    }

    public class subfundus
    {
        public int SubID { get; set; }
        public int PropID { get; set; }
        public string SubDescription { get; set; }
        public string Property { get; set; }

        public string remarks { get; set; }

        public Boolean disableslodf { get; set; }
        public Boolean disableslosf { get; set; }
        public Boolean disableslouf { get; set; }
        public Boolean checkStatusodf { get; set; }
        public Boolean checkStatusosf { get; set; }
        public Boolean checkStatusouf { get; set; }

    }
}
