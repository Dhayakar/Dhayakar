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
    public class OpticalGRNController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public OpticalGRNController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("GetGrn/{val}/{Time}")]
        public dynamic GetGrn(int val, string Time)

        {
            return _repoWrapper.OpticalGrn.GetGrn(val, Time);
        }

        [HttpGet("GetGrndetails/{val}/{Time}")]
        public dynamic GetGrndetails(int val, string Time)

        {
            return _repoWrapper.OpticalGrn.GetGrndetails(val, Time);
        }

        [HttpGet("GetGrntrns/{ID}")]
        public dynamic GetGrntrns(string ID)

        {
            return _repoWrapper.OpticalGrn.GetGrntrns(ID);
        }

        [HttpGet("GetGrntrnsdetails/{RandomUniqueID}")]
        public dynamic GetGrntrnsdetails(string RandomUniqueID)

        {
            return _repoWrapper.OpticalGrn.GetGrntrnsdetails(RandomUniqueID);
        }

        [HttpGet("GetGrnloc/{val}")]
        public dynamic GetGrnloc(string val)

        {
            return _repoWrapper.OpticalGrn.GetGrnloc(val);
        }

        [HttpGet("GetGrnlocation/{RandomUniqueID}")]
        public dynamic GetGrnlocation(string RandomUniqueID)

        {
            return _repoWrapper.OpticalGrn.GetGrnlocation(RandomUniqueID);
        }

        [HttpPost("Insertopticalgrn/{cmpid}/{TransactionTypeid}")]
        public dynamic Insertopticalgrn([FromBody] OpticalGrnDataView Addopticalgrn, int cmpid, int TransactionTypeid)
        {
            String gnr = _repoWrapper.Common.GenerateRunningCtrlNoo(TransactionTypeid, cmpid, "GetRunningNo");

            if (gnr == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }

            Addopticalgrn.OpticalStockMaster.DocumentNumber = gnr;

            return _repoWrapper.OpticalGrn.Insertopticalgrn(Addopticalgrn, cmpid, TransactionTypeid);
        }


        [HttpGet("Opticalgrnprint/{RandomUniqueID}/{CMPID}/{Time}")]
        public dynamic Opticalgrnprint(string RandomUniqueID, int CMPID, string Time)
        {
            return _repoWrapper.OpticalGrn.Opticalgrnprint(RandomUniqueID, CMPID, Time);
        }





        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
