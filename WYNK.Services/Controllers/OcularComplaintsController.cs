using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]

    public class OcularComplaintsController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public OcularComplaintsController(IRepositoryWrapper repowrapper)
        {
            _repoWrapper = repowrapper;
        }

        [HttpPost("InsertOcularComplaints")]
        public dynamic InsertOcularComplaints([FromBody] OcularComplaintsViewModel InsertOcularComplaints)
        {
            return _repoWrapper.OcularComplaints.InsertOcularComplaints(InsertOcularComplaints);
        }

        [HttpPost("updateOcularComplaints/{ID}")]
        public dynamic updateOcularComplaints([FromBody] OcularComplaintsViewModel InsertOcularComplaints, int ID)
        {
            return _repoWrapper.OcularComplaints.updateOcularComplaints(InsertOcularComplaints, ID);
        }

        [HttpPost("Deleteocularcomplaints/{ID}/{Reasons}")]
        public dynamic Deleteocularcomplaints(int ID, string Reasons)
        {
            return _repoWrapper.OcularComplaints.Deleteocularcomplaints(ID, Reasons);
        }
        //InsertnewOcularComplaints

        [HttpPost("InsertnewOcularComplaints/{UIN}")]
        public dynamic InsertnewOcularComplaints([FromBody] OcularComplaintsViewModel InsertnewOcularComplaints, string UIN)
        {
            return _repoWrapper.OcularComplaints.InsertnewOcularComplaints(InsertnewOcularComplaints, UIN);
        }
        [HttpGet("DeletenewOcularComplaints/{UIN}/{Description}/{Reasons}/{CMPID}")]
        public dynamic DeletenewOcularComplaints(string UIN, string Description, string Reasons, int CMPID)
        {
            return _repoWrapper.OcularComplaints.DeletenewOcularComplaints(UIN, Description, Reasons, CMPID);
        }
        [HttpGet("Deletenewsystemicconditions/{UIN}/{Description}/{Reasons}/{CMPID}")]
        public dynamic Deletenewsystemicconditions(string UIN, string Description, string Reasons, int CMPID)
        {
            return _repoWrapper.OcularComplaints.Deletenewsystemicconditions(UIN, Description, Reasons, CMPID);
        }



        [HttpGet("GetDeletednewOcularComplaints/{UIN}/{CMPID}")]
        public dynamic GetDeletednewOcularComplaints(string UIN, int CMPID)
        {
            return _repoWrapper.OcularComplaints.GetDeletednewOcularComplaints(UIN, CMPID);
        }


        [HttpGet("GetDeletednewsystemic/{UIN}/{CMPID}")]
        public dynamic GetDeletednewsystemic(string UIN, int CMPID)
        {
            return _repoWrapper.OcularComplaints.GetDeletednewsystemic(UIN, CMPID);
        }


        [HttpPost("InsertOcularMaster")]
        public dynamic InsertOcularMaster([FromBody] OcularComplaintsViewModel AddOculardata)
        {
            return _repoWrapper.OcularComplaints.InsertOcularMaster(AddOculardata);
        }


        [HttpPost("Insertsystemconditions")]
        public dynamic Insertsystemconditions([FromBody] OcularComplaintsViewModel Addsystemicdata)
        {
            return _repoWrapper.OcularComplaints.Insertsystemconditions(Addsystemicdata);
        }


        

        [HttpPost("UpdateOcularMaster/{ID}")]
        public dynamic UpdateOcularMaster([FromBody] OcularComplaintsViewModel updateOculardata,int ID)
        {
            return _repoWrapper.OcularComplaints.UpdateOcularMaster(updateOculardata,ID);
        }

        [HttpPost("UpdatesystemicMaster/{ID}")]
        public dynamic UpdatesystemicMaster([FromBody] OcularComplaintsViewModel updatesystemicdata, int ID)
        {
            return _repoWrapper.OcularComplaints.UpdatesystemicMaster(updatesystemicdata, ID);
        }


        [HttpPost("DeleteOcular/{ID}")]
        public dynamic DeleteOcular(int ID)
        {
            return _repoWrapper.OcularComplaints.DeleteOcular(ID);
        }


        [HttpPost("Deletesystemic/{ID}")]
        public dynamic Deletesystemic(int ID)
        {
            return _repoWrapper.OcularComplaints.Deletesystemic(ID);
        }
        

    }
}