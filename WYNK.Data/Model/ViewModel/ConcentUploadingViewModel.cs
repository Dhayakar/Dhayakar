using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
  public  class ConcentUploadingViewModel
    {
        public string Description { get; set; }
        public string Modulename { get; set; }
        public string CMPID { get; set; }

        public string ErrorMessaGE { get; set; }
        public string[] TOtalLines { get; set; }
        public ICollection<Concentdata> Concentdata { get; set; }
    }

    public class Concentdata
    {
        public string Description { get; set; }
        public DateTime date { get; set; }                
        public string[] TOtalLines { get; set; }
    }
}
