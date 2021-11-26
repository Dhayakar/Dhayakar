using System;
using System.Collections.Generic;
using System.Text;


namespace WYNK.Data.Model.ViewModel
{
    public class AllergyViewmodel
    {
        public allergy allergy { get; set; }
        public ICollection<allergyhis> allergyhis { get; set; }
        public ICollection<Descriptionhis> Descriptionhis { get; set; }
    }


    public class allergyhis
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public bool Active { get; set; }

    }

    public class Descriptionhis
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public bool Active { get; set; }

    }

}
