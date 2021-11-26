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
    public class SurgeryadmissionandassignController : Controller
    {

        private IRepositoryWrapper _repoWrapper;

        public SurgeryadmissionandassignController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("GetDoctDetails/{cmpid}")]
        public dynamic GetDoctDetails(int cmpid)
        {
            return _repoWrapper.Surgeryadmissionandassign.GetDoctDetails(cmpid);
        }

        [HttpGet("Getpatient/{cmpid}/{Time}")]
        public dynamic Getpatient(int cmpid, string Time)
        {
            return _repoWrapper.Surgeryadmissionandassign.Getpatient(cmpid, Time);
        }


        [HttpGet("Getviewpatient/{cmpid}/{Time}")]
        public dynamic Getviewpatient(int cmpid, string Time)
        {
            return _repoWrapper.Surgeryadmissionandassign.Getviewpatient(cmpid, Time);
        }


        [HttpGet("Getdemography/{UIN}/{RegID}/{Time}")]
        public dynamic Getdemography(string UIN, int RegID, string Time)
        {
            return _repoWrapper.Surgeryadmissionandassign.Getdemography(UIN, RegID, Time);
        }

        [HttpPost("Getroom/{roomid}/{cmpid}")]
        public dynamic Getroom([FromBody] Surgeryadmissionandassign sur, string roomid, int cmpid)
        {
            return _repoWrapper.Surgeryadmissionandassign.Getroom(sur, roomid, cmpid);
        }



        [HttpPost("InsertSurgeryAdmandAssign/{cmpid}/{TransactionId}/{M_TelNo}/{Category_M}")]
        public dynamic InsertSurgeryAdmandAssign([FromBody] Surgeryadmissionandassign Addsurgery, int cmpid, int TransactionId, string M_TelNo, string Category_M)
        {


            int? RecContraID = _repoWrapper.Common.GettingReceiptTcID(TransactionId, cmpid);

            if (RecContraID == null)
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Mapped in Transaction Table"
                };
            }
            Addsurgery.ReceiptRunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), cmpid, "GetRunningNo");

            if (Addsurgery.ReceiptRunningNo == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Receipt Running Number Does'nt Exist"
                };
            }

            String generatenumber = _repoWrapper.Common.GenerateRunningCtrlNoo(TransactionId, cmpid, "GetRunningNo");
            if (generatenumber == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }

            Addsurgery.AdmissionNumber = generatenumber;
            return _repoWrapper.Surgeryadmissionandassign.InsertSurgeryAdmandAssign(Addsurgery, cmpid, TransactionId, M_TelNo, Category_M);

        }



        [HttpPost("UpdateSurgeryAdmandAssign/{cmpid}/{TransactionId}/{M_TelNo}/{Category_M}")]
        public dynamic UpdateSurgeryAdmandAssign([FromBody] Surgeryadmissionandassign Addsurgery, int cmpid, int TransactionId, string M_TelNo, string Category_M)
        {
            return _repoWrapper.Surgeryadmissionandassign.UpdateSurgeryAdmandAssign(Addsurgery, cmpid, TransactionId, M_TelNo, Category_M);
        }



        [HttpGet("Getadmissionprint/{AdmitNo}/{cmpid}/{uin}/{tid}/{Time}/{recepno}")]
        public dynamic Getadmissionprint(string AdmitNo, int? cmpid, string uin, int? tid, string Time, string recepno)
        {
            return _repoWrapper.Surgeryadmissionandassign.Getadmissionprint(AdmitNo, cmpid, uin, tid, Time, recepno);
        }

        [HttpGet("Getviewselectpatient/{RandomUniqueID}/{Rid}/{Time}/{uin}/{AdmID}")]
        public dynamic Getviewselectpatient(string RandomUniqueID, int Rid, string Time, string uin, int AdmID)
        {
            return _repoWrapper.Surgeryadmissionandassign.Getviewselectpatient(RandomUniqueID, Rid, Time, uin, AdmID);
        }


    }
}