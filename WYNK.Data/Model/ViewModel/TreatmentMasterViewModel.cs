using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
   public class TreatmentMasterDataView
    {
        public TreatmentMasterModel TreatmentMasterModel { get; set; }
        public ICollection<TableArray> TableArray { get; set; }
        public ICollection<GetDataArray> GetDataArray { get; set; }
    }

    public class TableArray
    {
        public int ID { get; set; }
        public int SpecialityID { get; set; }
        public string Description { get; set; }
        public Boolean IsActive { get; set; }

    }

    public class GetDataArray
    {
        public int ID { get; set; }
        public string SpecialityID1 { get; set; }
        public string Description1 { get; set; }
        public Boolean IsActive1 { get; set; }

    }
}
