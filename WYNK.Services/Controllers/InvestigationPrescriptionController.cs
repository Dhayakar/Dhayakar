
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class InvestigationPrescriptionController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public InvestigationPrescriptionController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("Getdescvalues/{id}")]
        public InvestigationPrescriptionk Getdescvalues(int id)
        {
            return _repoWrapper.InvestigationPrescription.Getdescvalues(id);
        }

        [HttpGet("GetUinDetails/{id}")]
        public InvestigationPrescriptionk GetUinDetails(int id)
        {
            return _repoWrapper.InvestigationPrescription.GetUinDetails(id);
        }
        [HttpPost("InsertInvPrescription/{CompanyID}/{TransactionTypeid}")]
        public dynamic InsertInvPrescription([FromBody]InvestigationPrescriptionk InvestigationPrescription, int CompanyID, int TransactionTypeid)
        {

            string generatenumber = _repoWrapper.Common.GenerateRunningCtrlNoo(TransactionTypeid, CompanyID, "GetRunningNo");
            if (generatenumber == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }
            InvestigationPrescription.InvPrescription.INVPRESNO = generatenumber;





            return _repoWrapper.InvestigationPrescription.InsertInvPrescription(InvestigationPrescription, CompanyID, TransactionTypeid);
        }

        [HttpPost("Updatepres/{UIN}")]
        public dynamic Updatepres([FromBody] InvestigationPrescriptionk InvestigationPrescription, string UIN)
        {
            return _repoWrapper.InvestigationPrescription.Updatepres(InvestigationPrescription, UIN);
        }

        [HttpPost("uploadImag/{id}/{desc}/{uin}")]
        public bool uploadImag(string id, string desc, string uin)
        {
            var file1 = Request.Form.Files[0];
            return _repoWrapper.InvestigationPrescription.uploadImag(file1, uin, desc, id);
        }

    }

}



