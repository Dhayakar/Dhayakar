using System;
using System.Collections.Generic;
using System.Text;


namespace WYNK.Data.Model.ViewModel
{
    public class ErrorLogviewmodel
    {
        public ICollection<Totalerrorlog> Totalerrorlog { get; set; }
    }


    public class Totalerrorlog
    {
        public string totallines { get; set; }
       // public string errormessage { get; set; }

    }


}
