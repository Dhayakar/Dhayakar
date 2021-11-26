
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;
using System;
using Microsoft.Extensions.Configuration;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class CampRegistrationController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public CampRegistrationController(IRepositoryWrapper repoWrapper, IConfiguration config)
        {
            _repoWrapper = repoWrapper;

        }


        [HttpGet("GetCampRegistrationExtension/{CampRegNO}/{CmpID}")]
        public CampRegistrationViewModel GetCampRegistrationExtension(string CampRegNO, int CmpID)

        {
            return _repoWrapper.CampRegistration.GetCampRegistrationExtension(CampRegNO, CmpID);
        }

        [HttpGet("GetCampkindetail/{CampUIN}")]
        public CampRegistrationViewModel GetCampkindetail(string CampUIN)
        {
            return _repoWrapper.CampRegistration.GetCampkindetail(CampUIN);
        }

        [HttpPost("InsertCampReg/{CompanyID}/{userid}/{Transactionid}")]
        public dynamic InsertCampReg([FromBody] CampRegistrationViewModel CampRegistration, int CompanyID, int userid, int Transactionid)
        {
            string generatenumber = _repoWrapper.Common.GenerateRunningCtrlNoo(Transactionid, CompanyID, "GetRunningNo");

            if (Transactionid == 0)
            {
                var Errordata = _repoWrapper.Common.ErrorList("TransactionTypeid=null", "Registration", CompanyID, userid);

                return new
                {
                    Success = false,
                    Message = "TransactionTypeid Does'nt Exist"
                };
            }
            if (generatenumber == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }
            CampRegistration.CampRegistration.CampUIN = generatenumber;

            return _repoWrapper.CampRegistration.InsertCampReg(CampRegistration, userid, Transactionid);
      
        
        }



        [HttpPost("UdateCampReg/{CmpID}/{userid}/{Campuin}")]
        public dynamic UdateCampReg([FromBody] CampRegistrationViewModel CampRegistrationUpdate, int CmpID, int userid, string Campuin)
        {
            return _repoWrapper.CampRegistration.UdateCampReg(CampRegistrationUpdate, CmpID, userid , Campuin);
        }

        [HttpGet("DeleteCampReg/{Campuin}/{CMPID}/{USERID}/{Companyname}")]
        public dynamic DeleteCampReg(string Campuin, int CMPID, int USERID, string Companyname)
        {
            return _repoWrapper.CampRegistration.DeleteCampReg(Campuin, CMPID, USERID, Companyname);
        }



    }
}
