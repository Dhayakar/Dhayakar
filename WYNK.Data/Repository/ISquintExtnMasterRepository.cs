

using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface ISquintExtnMasterRepository : IRepositoryBase<SquintExtnMasterViewModel>

    {
        dynamic InsertSquintExtnM(SquintExtnMasterViewModel InsertSquintExtnMaster, int userroleID,int CmpID);
        dynamic UpdateSquintExtnM(SquintExtnMasterViewModel UpdateSquintExtnM, int ID,int userroleID,int CmpID);
        dynamic DeleteSQEM(SquintExtnMasterViewModel DeleteMaster, int ID,int CmpID);

        SquintExtnMasterViewModel GetDetails(string MasterName,int CmpID);
    }

}


