using System;
using System.Collections.Generic;
using System.Text;


namespace WYNK.Data.Model.ViewModel
{
    public class BrandView
    {
        public ICollection<Brand> Brand { get; set; }
        public ICollection<Brandfull> Brandfull { get; set; }

    }

    public class Brandfull
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string BrandType { get; set; }
        public Boolean IsActive { get; set; }
        public string IsActivename { get; set; }

    }


}
