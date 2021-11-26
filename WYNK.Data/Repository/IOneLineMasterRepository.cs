

using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IOneLineMasterRepository : IRepositoryBase<OneLineMasterViewModel>

    {
        dynamic InsertSlamp(OneLineMasterViewModel OneLineMaster, int userroleID, int CMPID);
        dynamic UdateSlamp(OneLineMasterViewModel OneLineMaster, int OLMID, int userroleID, int CMPID);
        dynamic DeleteSlamp(OneLineMasterViewModel OneLineMaster, int OLMID, int CMPID);
        dynamic DeleteSlampwithvalue(OneLineMasterViewModel OneLineMaster, int OLMID, int CMPID, int modvalue);
        dynamic UdateSlampwithvalue(OneLineMasterViewModel OneLineMaster, int OLMID, int userroleID, int CMPID, int modvalue);
        dynamic InsertSlampwithvalue(OneLineMasterViewModel OneLineMaster, int userroleID, int CMPID, int modvalue);

        OneLineMasterViewModel GetDetails(string MasterName, int CMPID);
    }

}


