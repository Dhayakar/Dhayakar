using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class IOProcedureTemplateViewModel
    {
      public string SurgeryDescription { get; set; }
      public IOProcedureTemp IOProcedureTemp { get; set; }
      public ICollection<IOProcedureTemp> IOProcedureTempDetails { get; set; }
    }

    public class IOProcedureTemp {
        public string icdSpeciality { get; set; }
        //public string Eye { get; set; }
        public string SurgeryDescription { get; set; }
        public string OTNotesDescription { get; set; }
        public string UserInputType { get; set; }
        public string InputValue { get; set; }
        public Boolean Status { get; set; }

        public int? ID { get; set; } 
        public Boolean changed { get; set; } 
        public Boolean Added { get; set; } 
    }




}

