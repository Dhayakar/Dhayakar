using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IRoomTransferRepository : IRepositoryBase<RoomTransferDataView>
    {
        RoomTransferDataView RoomDetailsSearch(string UIN);
        RoomTransferDataView AvailableRoomSearch(string Res);
        dynamic InsertRoomDetails(RoomTransferDataView Con);

    }
}
