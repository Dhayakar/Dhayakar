
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
   
    [Route("[controller]")]
    
    public class DoctorMasterController : Controller
    {
        private IRepositoryWrapper _repoWrapper; 
        public DoctorMasterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

      
        [HttpPost("UpdateDoctorMaster")]
        public dynamic UpdateDoctorMaster([FromBody] DoctorMaster DoctorMaster)
        {
            return _repoWrapper.doctorMaster.UpdateDoctorMaster(DoctorMaster);
        }



        [HttpPost("UpdateDoctorVisitingHours")]
        public dynamic UpdateDoctorVisitingHours([FromBody] DoctorMaster DoctorMaster)
        {
            return _repoWrapper.doctorMaster.UpdateDoctorVisitingHours(DoctorMaster);
        }




        [HttpGet("Getdoctorhours/{CMPID}/{Doctorid}")]
        public dynamic Getdoctorhours(int CMPID, int Doctorid)
        {
            return _repoWrapper.doctorMaster.Getdoctorhours(CMPID, Doctorid);
        }


        [HttpGet("Getpatientimage/{regNo}")]
        public dynamic Getpatientimage(string regNo)
        {
            return _repoWrapper.doctorMaster.Getpatientimage(regNo);
        }



        [HttpPost("AddSpeciality")]
        public dynamic AddSpeciality([FromBody] DoctorMaster AddSpecial)
        {
            return _repoWrapper.doctorMaster.AddSpeciality(AddSpecial);
        }


        [HttpPost("AddEngage")]
        public dynamic AddEngage([FromBody] DoctorMaster EngagementType)
        {
            return _repoWrapper.doctorMaster.AddEngage(EngagementType);
        }

        [HttpPost("DeleteDoctor1/{DoctorID}")]
        public dynamic DeleteDoctor1(int? DoctorID)
        {
            return _repoWrapper.doctorMaster.DeleteDoctor1(DoctorID);
        }




        [HttpDelete("DeleteDoctor/{DoctorID}/{DoctorSpecialityID}")]
        public dynamic DeleteDoctor(int? DoctorID, int? DoctorSpecialityID)
        {
            return _repoWrapper.doctorMaster.DeleteDoctor(DoctorID, DoctorSpecialityID);
        }

        [HttpPost("UpDateDocMas/{DoctorID}")]
        public dynamic UpDateDocMas([FromBody] DoctorMaster DoctorMaster, int? DoctorID)
        {
            return _repoWrapper.doctorMaster.UpDateDocMas(DoctorMaster, DoctorID);
        }


        [HttpGet("DoctorAssignDetails/{ID}")]
        public dynamic DoctorAssignDetails(int ID)
        {
            return _repoWrapper.doctorMaster.DoctorAssignDetails(ID);
        }

        [HttpGet("DeleteDoctorpastdata/{CMPID}/{DOCID}/{EXTENSIONID}")]
        public dynamic DeleteDoctorpastdata(int CMPID, int DOCID, int EXTENSIONID)
        {
            return _repoWrapper.doctorMaster.DeleteDoctorpastdata(CMPID, DOCID, EXTENSIONID);
        }


        [HttpGet("GetlocationDetails/{id}")]
        public DoctorMaster GetlocationDetails(int id)
        {
            return _repoWrapper.doctorMaster.GetlocationDetails(id);
        }

        [HttpPost("UploadImage/{docid}")]
        public bool UploadImage(string docid)
        {
            var file = Request.Form.Files[0];
            return _repoWrapper.doctorMaster.UploadImage(file, docid);
        }


        [HttpGet("DeleteDocImage/{docid}")]
        public bool DeleteDocImage(string docid)
        {
            return _repoWrapper.doctorMaster.DeleteDocImage(docid);
        }



        [HttpGet("Getalldoctorssearch/{docid}")]
        public dynamic Getalldoctorssearch(string docid)
        {
            return _repoWrapper.doctorMaster.Getalldoctorssearch(docid);
        }

        

    }
}
