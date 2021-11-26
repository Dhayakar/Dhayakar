using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class MedicineMapping
    {
        public ICollection<AlternativeDrug> AlternativeDrug { get; set; }
        public ICollection<Drgchld> Drgchld { get; set; }
        public ICollection<Drghis> Drghis { get; set; }//RetDrghis
        public ICollection<RetDrghis> RetDrghis { get; set; }


    }

    public class Drgchld
    {

        public int drugid { get; set; }
        public string drugname { get; set; }
        public Boolean checkStatus2 { get; set; }



    }

    public class Drghis
    {

        public int drugid { get; set; }
        public int drugchildid { get; set; }
        public string drugname { get; set; }


    }

    public class RetDrghis
    {

        public int drugid { get; set; }
        public int drugchildid { get; set; }
        public string drugname { get; set; }


    }

}

