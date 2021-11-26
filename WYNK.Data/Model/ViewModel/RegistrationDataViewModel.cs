using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class RegistrationDataViewModel
    {
        public PatientRegistrationMaster Master { get; set; }
        public ICollection<Commonlistmodel> OptoCommonlistmodel { get; set; }
        public ICollection<Commonlistmodel> EyedoctorCommonlistmodel { get; set; }
        public ICollection<Commonlistmodel> visionCommonlistmodel { get; set; }
        public ICollection<Commonlistmodel> ViewCommonlistmodel { get; set; }

    }


    public class Commonlistmodel
    {
        public string Text { get; set; }
        public string Text1 { get; set; }
        public string Value { get; set; }
    }


}
