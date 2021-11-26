using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IRoomMasterRepository : IRepositoryBase<RoomMasterViewM>
    {
        dynamic getConcerntextfile(int CompanyID);
        dynamic insertdata(RoomMasterViewM roomMaster);
        dynamic Updatedata(RoomMasterViewM roomMaster);
        RoomMasterViewM getRoomDet(int CompanyID, int roomID);
        RoomMasterViewM getRoomTariff(int CmpID);
        
    }
}
