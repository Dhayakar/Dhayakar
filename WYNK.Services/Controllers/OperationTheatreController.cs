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
    public class OperationTheatreController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public OperationTheatreController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpPost("Insertoperation")]
        public dynamic Insertoperation([FromBody]OperationTheatres Operation)
        {
            return _repoWrapper.OperationTheatre.Insertoperation(Operation);
        }


        [HttpPost("Updateoperation/{ID}")]
        public dynamic Updateoperation([FromBody] OperationTheatres Operationup, int ID)
        {
            return _repoWrapper.OperationTheatre.Updateoperation(Operationup, ID);
        }


        [HttpPost("Deleteoperation/{ID}")]
        public dynamic Deleteoperation(int ID)
        {
            return _repoWrapper.OperationTheatre.Deleteoperation(ID);
        }


    }

}



