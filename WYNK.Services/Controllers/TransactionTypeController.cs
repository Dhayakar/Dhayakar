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
    public class TransactionTypeController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public TransactionTypeController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("getContraTranDet")]
        public TransactionTypeViewM getContraTranDet()
        {
            return _repoWrapper.TransactionTypeViewM.getContraTranDet();
        }

        [HttpPost("insertdata")]
        public dynamic insertdata([FromBody]TransactionTypeViewM TransactionType)
        {
            return _repoWrapper.TransactionTypeViewM.insertdata(TransactionType);
        }


        [HttpPost("updatedata/{ID}")]
        public dynamic updatedata([FromBody]TransactionTypeViewM TransactionType, int ID)
        {
            return _repoWrapper.TransactionTypeViewM.updatedata(TransactionType, ID);
        }



    }
}