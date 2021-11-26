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
    public class PACController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public PACController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("getPatientDrugtherapy/{ID}/{CMPID}")]
        public PACViewM getPatientDrugtherapy(string ID, int CMPID)
        {
            return _repoWrapper.PACViewM.getPatientDrugtherapy(ID, CMPID);
        }

        [HttpPost("InserData")]
        public dynamic InserData([FromBody]PACViewM PACHistory)
        {
            return _repoWrapper.PACViewM.InserData(PACHistory);
        }

        [HttpPost("InserExamData")]
        public dynamic InserExamData([FromBody]PACViewM PACExamination)
        {
            return _repoWrapper.PACViewM.InserExamData(PACExamination);
        }

        [HttpPost("InserInvData")]
        public dynamic InserInvData([FromBody]PACViewM PACInvestigation)
        {
            return _repoWrapper.PACViewM.InserInvData(PACInvestigation);
        }

        [HttpPost("PreOperativeInsert")]
        public dynamic PreOperativeInsert([FromBody]PACViewM PreOperativeInstruction)
        {
            return _repoWrapper.PACViewM.PreOperativeInsert(PreOperativeInstruction);
        }



        [HttpGet("getBMIrange/{ID}")]
        public PACViewM getBMIrange(decimal ID)
        {
            return _repoWrapper.PACViewM.getBMIrange(ID);
        }
        [HttpGet("getPACHistory/{UIN}/{ADMID}/{ID}")]
        public PACViewM getPACHistory(string UIN, int ADMID, int ID)
        {
            return _repoWrapper.PACViewM.getPACHistory(UIN, ADMID, ID);
        }

        [HttpGet("getPACExamination/{UIN}/{ADMID}/{ID}")]
        public PACViewM getPACExamination(string UIN, int ADMID, int ID)
        {
            return _repoWrapper.PACViewM.getPACExamination(UIN, ADMID, ID);
        }

        [HttpGet("getPACInvestigation/{UIN}/{ADMID}/{ID}")]
        public PACViewM getPACInvestigation(string UIN, int ADMID, int ID)
        {
            return _repoWrapper.PACViewM.getPACInvestigation(UIN, ADMID, ID);
        }
        [HttpGet("getPACPreOperative/{UIN}/{ADMID}/{ID}")]
        public PACViewM getPACPreOperative(string UIN, int ADMID, int ID)
        {
            return _repoWrapper.PACViewM.getPACPreOperative(UIN, ADMID, ID);
        }

        [HttpPut("UpdateData/{PACHID}")]
        public dynamic UpdateData([FromBody]PACViewM PACHistory, int PACHID)
        {
            return _repoWrapper.PACViewM.UpdateData(PACHistory, PACHID);
        }
        [HttpPut("UpdateExamData/{PACHID}")]
        public dynamic UpdateExamData([FromBody]PACViewM PACExamination, int PACHID)
        {
            return _repoWrapper.PACViewM.UpdateExamData(PACExamination, PACHID);
        }

        [HttpPut("UpdateInvData/{PACHID}")]
        public dynamic UpdateInvData([FromBody]PACViewM PACInvestigation, int PACHID)
        {
            return _repoWrapper.PACViewM.UpdateInvData(PACInvestigation, PACHID);
        }


        [HttpPut("UpdatePreOperativeInstructions/{PACPOID}")]
        public dynamic UpdatePreOperativeInstructions([FromBody]PACViewM PACPreOperative, int PACPOID)
        {
            return _repoWrapper.PACViewM.UpdatePreOperativeInstructions(PACPreOperative, PACPOID);
        }


        [HttpGet("Beforeinductionofanaesthesia/{CMPID}/{SAID}")]
        public dynamic Beforeinductionofanaesthesia(int CMPID, int SAID)
        {
            return _repoWrapper.PACViewM.Beforeinductionofanaesthesia(CMPID, SAID);
        }


        [HttpPost("Insertbeforeana")]
        public dynamic Insertbeforeana([FromBody]PACViewM Addbefore)
        {
            return _repoWrapper.PACViewM.Insertbeforeana(Addbefore);
        }


        [HttpGet("Beforeinductionofanaesthesiaprint/{OLD}/{SAID}/{cmpid}")]
        public dynamic Beforeinductionofanaesthesiaprint(int OLD, int SAID, int cmpid)
        {
            return _repoWrapper.PACViewM.Beforeinductionofanaesthesiaprint(OLD, SAID, cmpid);
        }

        [HttpGet("PACHISTORYPRINT/{PACID}/{cmpid}")]
        public dynamic PACHISTORYPRINT(int PACID, int cmpid)
        {
            return _repoWrapper.PACViewM.PACHISTORYPRINT(PACID, cmpid);
        }

        [HttpGet("PACPREOPINSPRINT/{PPOID}/{cmpid}")]
        public dynamic PACPREOPINSPRINT(int PPOID, int cmpid)
        {
            return _repoWrapper.PACViewM.PACPREOPINSPRINT(PPOID, cmpid);
        }

        [HttpGet("getPACExaminationprint/{PacexamID}/{cmpid}")]
        public dynamic getPACExaminationprint(int PacexamID, int cmpid)
        {
            return _repoWrapper.PACViewM.getPACExaminationprint(PacexamID, cmpid);
        }

        [HttpGet("getPACInvestigationprint/{PacexamID}/{cmpid}")]
        public dynamic getPACInvestigationprint(int PacexamID, int cmpid)
        {
            return _repoWrapper.PACViewM.getPACInvestigationprint(PacexamID, cmpid);
        }


    }
}