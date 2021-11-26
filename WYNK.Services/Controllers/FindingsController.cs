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
    public class FindingsController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public FindingsController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("docnames/{uin}")]
        public dynamic docnames(string uin)
        {
            return _repoWrapper.Findings.docnames(uin);
        }


      
        [HttpGet("GetFundnewDetails/{UIN}")]
        public Findings GetFundnewDetails(string UIN)
        {
            return _repoWrapper.Findings.GetFundnewDetails(UIN);
        }
        [HttpGet("GetOneeyedDetails/{UIN}/{cmpid}")]
        public Findings GetOneeyedDetails(string UIN, int cmpid)
        {
            return _repoWrapper.Findings.GetOneeyedDetails(UIN, cmpid);
        }

        [HttpGet("Getpastvalues/{id}")]
        public Findings Getpastvalues(string id)
        {
            return _repoWrapper.Findings.Getpastvalues(id);
        }

        [HttpGet("GetcmpDetails/{id}")]
        public Findings GetcmpDetails(int id)
        {
            return _repoWrapper.Findings.GetcmpDetails(id);
        }

        [HttpGet("GetPatientDetails/{UIN}/{CompanyID}")]
        public Findings GetPatientDetails(string UIN, int CompanyID)
        {
            return _repoWrapper.Findings.GetPatientDetails(UIN, CompanyID);
        }

        [HttpGet("GetallPatientDetails/{UIN}/{CompanyID}/{gmt}")]
        public Findings GetallPatientDetails(string UIN, int CompanyID, string gmt)
        {
            return _repoWrapper.Findings.GetallPatientDetails(UIN, CompanyID, gmt);
        }

        [HttpGet("GetregPatientDetails/{UIN}/{CompanyID}/{GMT}")]
        public Findings GetregPatientDetails(string UIN, int CompanyID, string GMT)
        {
            return _repoWrapper.Findings.GetregPatientDetails(UIN, CompanyID, GMT);
        }

        [HttpGet("Getcustvalues/{RID}/{GMT}")]
        public Findings Getcustvalues(int RID, string GMT)
        {
            return _repoWrapper.Findings.Getcustvalues(RID, GMT);
        }


        [HttpGet("GetSlitnewDetails/{UIN}")]
        public Findings GetSlitnewDetails(string UIN)
        {
            return _repoWrapper.Findings.GetSlitnewDetails(UIN);
        }


        
        
        [HttpGet("GetdiaDetails/{CmpID}")]
        public IEnumerable<Diag> GetdiaDetails(int CmpID)
        {
            return _repoWrapper.Findings.GetdiaDetails(CmpID);
        }

       

        [HttpPost("UpdateFindings/{UIN}/{DOCID}/{cpname}/{dcname}")]
        public dynamic UpdateFindings([FromBody] Findings Findings, string UIN, int DOCID, string cpname, string dcname)
        {
            return _repoWrapper.Findings.UpdateFindings(Findings, UIN, DOCID, cpname, dcname);
        }
        [HttpPost("UpdateTono/{UIN}/{DOCID}/{cpname}/{dcname}")]
        public dynamic UpdateTono([FromBody] Findings Findings, string UIN, int DOCID, string cpname, string dcname)
        {
            return _repoWrapper.Findings.UpdateTono(Findings, UIN, DOCID, cpname, dcname);
        }
        [HttpPost("uploadImagsqd/{id}/{desc}/{uin}")]
        public bool uploadImagsqd(string id, string desc, string uin)
        {
            var file1 = Request.Form.Files[0];
            return _repoWrapper.Findings.uploadImagsqd(file1, uin, desc, id);
        }

        [HttpPost("uploadImagsqs/{id}/{desc}/{uin}")]
        public bool uploadImagsqs(string id, string desc, string uin)
        {
            var file1 = Request.Form.Files[0];
            return _repoWrapper.Findings.uploadImagsqs(file1, uin, desc, id);
        }



        [HttpPost("uploadImagslod/{id}/{desc}/{uin}")]
        public bool uploadImagslod(string id, string desc, string uin)
        {
            var file1 = Request.Form.Files[0];
            return _repoWrapper.Findings.uploadImagslod(file1, uin, desc, id);
        }

        [HttpPost("uploadImagslos/{id}/{desc}/{uin}")]
        public bool uploadImagslos(string id, string desc, string uin)
        {
            var file1 = Request.Form.Files[0];
            return _repoWrapper.Findings.uploadImagslos(file1, uin, desc, id);
        }

        [HttpPost("uploadImagelod/{id}/{desc}/{uin}")]
        public bool uploadImagelod(string id, string desc, string uin)
        {
            var file1 = Request.Form.Files[0];
            return _repoWrapper.Findings.uploadImagelod(file1, uin, desc, id);
        }

        [HttpPost("uploadImagelos/{id}/{desc}/{uin}")]
        public bool uploadImagelos(string id, string desc, string uin)
        {
            var file1 = Request.Form.Files[0];
            return _repoWrapper.Findings.uploadImagelos(file1, uin, desc, id);
        }


        [HttpPost("uploadImagfnod/{id}/{desc}/{uin}")]
        public bool uploadImagfnod(string id, string desc, string uin)
        {
            var file1 = Request.Form.Files[0];
            return _repoWrapper.Findings.uploadImagfnod(file1, uin, desc, id);
        }

        [HttpPost("uploadImagfnos/{id}/{desc}/{uin}")]
        public bool uploadImagfnos(string id, string desc, string uin)
        {
            var file1 = Request.Form.Files[0];
            return _repoWrapper.Findings.uploadImagfnos(file1, uin, desc, id);
        }

        [HttpPost("uploadImagglod/{id}/{desc}/{uin}")]
        public bool uploadImagglod(string id, string desc, string uin)
        {
            var file1 = Request.Form.Files[0];
            return _repoWrapper.Findings.uploadImagglod(file1, uin, desc, id);
        }

        [HttpPost("uploadImagglos/{id}/{desc}/{uin}")]
        public bool uploadImagglos(string id, string desc, string uin)
        {
            var file1 = Request.Form.Files[0];
            return _repoWrapper.Findings.uploadImagglos(file1, uin, desc, id);
        }

        [HttpPost("uploadImagvfod/{id}/{desc}/{uin}")]
        public bool uploadImagvfod(string id, string desc, string uin)
        {
            var file1 = Request.Form.Files[0];
            return _repoWrapper.Findings.uploadImagvfod(file1, uin, desc, id);
        }

        [HttpPost("uploadImagvfos/{id}/{desc}/{uin}")]
        public bool uploadImagvfos(string id, string desc, string uin)
        {
            var file1 = Request.Form.Files[0];
            return _repoWrapper.Findings.uploadImagvfos(file1, uin, desc, id);
        }


        [HttpPost("UpdateDiagnosis")]
        public dynamic UpdateDiagnosis([FromBody] Findings Findings)
        {
            return _repoWrapper.Findings.UpdateDiagnosis(Findings);
        }
        

        [HttpPost("uploadImage/{uin}")]
        public bool UploadImage(string uin)
        {
            var file = Request.Form.Files[0];
            return _repoWrapper.Findings.UploadImage(file, uin);
        }


        [HttpPost("uploadImage1/{uin}")]
        public bool UploadImage1(string uin)
        {
            var file1 = Request.Form.Files[0];
            return _repoWrapper.Findings.UploadImage1(file1, uin);
        }

        [HttpPost("uploadFile/{uin}")]
        public bool uploadFile(string uin)
        {
            var files = Request.Form.Files[0];
            return _repoWrapper.Findings.uploadFile(files, uin);
        }


        [HttpPost("uploadImage2/{uin}")]
        public bool UploadImage2(string uin)
        {
            var file2 = Request.Form.Files[0];
            return _repoWrapper.Findings.UploadImage2(file2, uin);
        }

        [HttpPost("uploadImage3/{uin}")]
        public bool UploadImage3(string uin)
        {
            var file3 = Request.Form.Files[0];
            return _repoWrapper.Findings.UploadImage3(file3, uin);
        }



        [HttpPost("Remove/{ID}")]
        public dynamic Remove([FromBody] Findings Findings, int ID)
        {
            return _repoWrapper.Findings.Remove(Findings, ID);
        }



        [HttpGet("Getpatientimage/{UIN}")]
        public dynamic Getpatientimage(string UIN)
        {
            return _repoWrapper.Findings.Getpatientimage(UIN);
        }

        [HttpGet("Getpatientimagefnod/{UIN}")]
        public dynamic Getpatientimagefnod(string UIN)
        {
            return _repoWrapper.Findings.Getpatientimagefnod(UIN);
        }

        [HttpGet("Getpatientimageslod/{UIN}")]
        public dynamic Getpatientimageslod(string UIN)
        {
            return _repoWrapper.Findings.Getpatientimageslod(UIN);
        }

        [HttpGet("Getpatientimageslos/{UIN}")]
        public dynamic Getpatientimageslos(string UIN)
        {
            return _repoWrapper.Findings.Getpatientimageslos(UIN);
        }

        [HttpGet("Getpatientfile/{UIN}")]
        public dynamic Getpatientfile(string UIN)
        {
            return _repoWrapper.Findings.Getpatientfile(UIN);
        }



        [HttpGet("GetFDDtSyrDetails/{uin}/{GMT}/{Cmpid}")]
        public dynamic GetFDDtSyrDetails(string uin, string GMT,int Cmpid)
        {
            return _repoWrapper.Findings.GetFDDtSyrDetails(uin, GMT, Cmpid);
        }     
        
        [HttpGet("GetRemovedFDDtSyrDetails/{uin}/{GMT}/{CMPID}")]
        public dynamic GetRemovedFDDtSyrDetails(string uin, string GMT , int CMPID)
        {
            return _repoWrapper.Findings.GetRemovedFDDtSyrDetails(uin, GMT, CMPID);
        }

        [HttpPost("DeleteFDDTSyringe/{uin}/{ID}/{Cmpid}")]
        public dynamic DeleteFDDTSyringe([FromBody] FddtSyringeRemovals FddtSyringeRemovalss, string uin, int ID, int Cmpid)
        {
            return _repoWrapper.Findings.DeleteFDDTSyringe(FddtSyringeRemovalss, uin, ID, Cmpid);
        }

    }
}
