using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class SlitLampViewM
    {
        public OneLine_Masters oneLine_Masters { get; set; }
        public ICollection<Description> Description { get; set; }
        public ICollection<Descriptiontb> Descriptiontb { get; set; }
        public ICollection<SubslitDescription> SubslitDescription { get; set; }
        public ICollection<Descriptionsltss> Descriptionsltss { get; set; }
        public ICollection<Descriptionslts> Descriptionslts { get; set; }
        public ICollection<Descriptionslt> Descriptionslt { get; set; }
        public ICollection<AddslitDescription> AddslitDescription { get; set; }
        public ICollection<DescriptionDtls> DescriptionDtls { get; set; }
        public SlitLampMaster SlitLampMaster { get; set; }
    }



    public class Description
    {
        public int ID { get; set; }
        public string ParentDescription { get; set; }
        public string Righteye { get; set; }
        public string Lefteye { get; set; }
        public string Remarks { get; set; }
        public Boolean OD { get; set; }
        public Boolean OS { get; set; }
    }



    public class Descriptionslt
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string PTag { get; set; }
        public int? PID { get; set; }
        public bool IsActive { get; set; }
        public string IsActive1 { get; set; }

    }

    public class Descriptionslts
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string PTag { get; set; }
        public int? PID { get; set; }
        public bool IsActive { get; set; }
        public string IsActive1 { get; set; }

    }

    public class Descriptionsltss
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string PTag { get; set; }
        public int? PID { get; set; }
        public bool IsActive { get; set; }
        public string IsActive1 { get; set; }

    }
    public class Descriptiontb
    {
        public int ID { get; set; }
        public string ParentDescription { get; set; }
        public ICollection<subslit> subslit { get; set; }
    }

    public class subslit
    {
        public int SubID { get; set; }
        public int PropID { get; set; }
        public string SubDescription { get; set; }
        public string Property { get; set; }

        public string remarks { get; set; }

        public Boolean disableslod { get; set; }
        public Boolean disableslos { get; set; }
        public Boolean disableslou { get; set; }
        public Boolean checkStatusod { get; set; }
        public Boolean checkStatusos { get; set; }
        public Boolean checkStatusou { get; set; }

    }
    public class SubslitDescription
    {
        public int ID { get; set; }
        public string SubDescription { get; set; }
    }

    public class AddslitDescription
    {
        public int ID { get; set; }
        public string AdditionalDescription { get; set; }
    }

    public class DescriptionDtls
    {
        public string Description { get; set; }
        public string SubDescription { get; set; }
        public string AddDescription { get; set; }
        public Boolean disab { get; set; }
    }
}
