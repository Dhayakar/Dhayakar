using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class DepartView
    {
        public Depart Des { get; set; }
        public ICollection<DepartDetails> DepartDetails { get; set; }

    }

    public class DepartDetails
    {

        public int ID { get; set; }
        public string Description { get; set; }
        public string DepartmentIncharge { get; set; }
        public string DepartmentLocation { get; set; }
        public bool IsActive { get; set; }
        public Boolean IsDeleted { get; set; }

    }

}
