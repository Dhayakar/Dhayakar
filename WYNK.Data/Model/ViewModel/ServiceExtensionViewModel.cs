using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class ServiceExtensionViewModel
    {
        public ICollection<patientDtls> patientDtls { get; set; }//PayDetailsspp
        public ICollection<outpatient> outpatient { get; set; }

    }



    public class outpatient
    {
        public int paid { get; set; }
        public string uin { get; set; }
        public string name { get; set; }
        public string age { get; set; }
        public string gender { get; set; }
        public string address { get; set; }
        public string phoneno { get; set; }
        public string billtype { get; set; }

    }

    

}
