using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class UomMasterViewModel
    {


        public UOM_Master drugMaster { get; set; }

        public ICollection<UOMMaster> UOMMaster { get; set; }
    }


    public class UOMMaster
    {


        public int ID { get; set; }
        public string Description { get; set; }
 



    }


}

