using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using WYNK.Data.Repository;
using WYNK.Data.Common;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class RoomTransferController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public RoomTransferController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("RoomDetailsSearch/{UIN}")] 
        public RoomTransferDataView RoomDetailsSearch(string UIN)
        {
            return _repoWrapper.RoomTransfer.RoomDetailsSearch(UIN);
        }
        [HttpGet("AvailableRoomSearch/{Res}")] 
        public RoomTransferDataView AvailableRoomSearch(string Res)
        {
            return _repoWrapper.RoomTransfer.AvailableRoomSearch(Res);
        }


        [HttpPost("InsertRoomDetails")]
        public dynamic InsertRoomDetails([FromBody]RoomTransferDataView Con)
        {
            return _repoWrapper.RoomTransfer.InsertRoomDetails(Con);
        }
       
    }
}
