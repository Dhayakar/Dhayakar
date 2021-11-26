
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;
using System;


namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class ExcelUploadAPIController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        public ExcelUploadAPIController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpPost("UpdateExceldata/{CmpId}")]
        public dynamic UpdateExceldata([FromBody] ExcelViewModel UpdateExceldata, int CmpId)
        {
            return _repoWrapper.IExceluploadrepo.UpdateExceldata(UpdateExceldata, CmpId); 
        }



        [HttpPost("UpdateDrugExceldata/{CmpId}/{Createdby}")]
        public dynamic UpdateDrugExceldata([FromBody] ExcelViewModel UpdateDrugExceldata, int CmpId, int Createdby)
        {
            return _repoWrapper.IExceluploadrepo.UpdateDrugExceldata(UpdateDrugExceldata, CmpId, Createdby);
        }

        [HttpPost("UpdateStrabismusdata/{CmpId}/{Createdby}")]
        public dynamic UpdateStrabismusdata([FromBody] ExcelViewModel UpdateStrabismusdata, int CmpId, int Createdby)
        {
            return _repoWrapper.IExceluploadrepo.UpdateStrabismusdata(UpdateStrabismusdata, CmpId, Createdby);
        }



        [HttpPost("GetStatusDrugStock/{CmpId}")]
        public dynamic GetStatusDrugStock([FromBody] DrugStockExcel DrugList, int CmpId)
        {
            return _repoWrapper.IExceluploadrepo.GetStatusDrugStock(DrugList, CmpId);
        }


        [HttpPost("GetStatusOpticalStock/{CmpId}")]
        public dynamic GetStatusOpticalStock([FromBody] OpticalStockExcel OpticalList, int CmpId)
        {
            return _repoWrapper.IExceluploadrepo.GetStatusOpticalStock(OpticalList, CmpId);
        }



        [HttpPost("SubmitDrugStock/{CmpId}/{UserID}/{TC}/{Dates}")]
        public dynamic SubmitDrugStock([FromBody] DrugStockExcel DrugList, int CmpId,int UserID,int TC,DateTime Dates)
        {

            DrugList.RunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(TC, CmpId, "GetRunningNo");

            if (DrugList.RunningNo == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }

            return _repoWrapper.IExceluploadrepo.SubmitDrugStock(DrugList, CmpId, UserID,TC, Dates);
        }



        [HttpPost("SubmitOpticalStock/{CmpId}/{UserID}/{TC}/{Dates}")]
        public dynamic SubmitOpticalStock([FromBody] OpticalStockExcel OpticList, int CmpId, int UserID, int TC, DateTime Dates)
        {

            OpticList.RunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(TC, CmpId, "GetRunningNo");

            if (OpticList.RunningNo == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }

            return _repoWrapper.IExceluploadrepo.SubmitOpticalStock(OpticList, CmpId, UserID, TC, Dates);
        }



        [HttpGet("GettingDrugIdsDesc/{CmpId}")]
        public dynamic GettingDrugIdsDesc(int CmpId)
        {
            return _repoWrapper.IExceluploadrepo.GettingDrugIdsDesc(CmpId);
        }


        [HttpPost("VendorDrugCheck/{CmpId}/{VendorID}")]
        public dynamic VendorDrugCheck([FromBody] DrugStockExcel DrugStockExcel, int CmpId,int VendorID)
        {
            return _repoWrapper.IExceluploadrepo.VendorDrugCheck(DrugStockExcel, CmpId, VendorID);
        }


        [HttpPost("UpdateSurgeryExceldata/{CmpId}/{Createdby}/{Icd}/{surgery}/{popup}")]
        public dynamic UpdateSurgeryExceldata([FromBody] ExcelViewModel UpdateSurgeryExceldata, int CmpId, int Createdby, int Icd, string surgery, int popup)
        {
            return _repoWrapper.IExceluploadrepo.UpdateSurgeryExceldata(UpdateSurgeryExceldata, CmpId, Createdby, Icd, surgery, popup);
        }


        [HttpGet("GetExistingRecord/{ICD}/{Surgery}")]
        public dynamic GetExistingRecord(int ICD, string Surgery)
        {
            return _repoWrapper.IExceluploadrepo.GetExistingRecord(ICD, Surgery);
        }
    }
}