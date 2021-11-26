using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{ 
    public class ForeignNationalViewModel
    {
        public ForeignNational ForeignNational { get; set; }
        public ICollection<ForeignNationaldetials> ForeignNationaldetials { get; set; }
    }



    public class ForeignNationaldetials
    {
        public int ID { get; set; }
        public bool IsForeignNational { get; set; }
        //public string IsForeignNationalINT { get; set; }   
        public decimal? NormalFee { get; set; }
        public decimal? SuperSpecialityFee { get; set; }
        public bool IsActive { get; set; }
    }

}
