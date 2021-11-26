using System;
using System.Collections.Generic;
using System.Text;


namespace WYNK.Data.Model.ViewModel
{
    public class TonomentryViewmodel
    {
        public Tonometry Tonometry { get; set; }
        public ICollection<Tonomrtryfull> Tonomrtryfull { get; set; }
    }


    public class Tonomrtryfull
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public bool Active { get; set; }

    }

}
