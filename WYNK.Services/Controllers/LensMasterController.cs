
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;
using System;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class LensMasterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public LensMasterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("GettaxDetails/{ID}")]
        public dynamic GettaxDetails(int ID)
        {
            return _repoWrapper.LensMaster.GettaxDetails(ID);
        }


        [HttpPost("Insertlensmaster")]
        public dynamic Insertlensmaster([FromBody] LensMatserDataView Addlens)
        {
            return _repoWrapper.LensMaster.Insertlensmaster(Addlens);
        }

        [HttpGet("Getlensfull/{ID}/{Cmpid}/{name}")]
        public dynamic Getlensfull(string ID, int Cmpid, string name)
        {
            return _repoWrapper.LensMaster.Getlensfull(ID, Cmpid, name);
        }

        [HttpPost("Updatelensmaster/{ID}/{doctorID}")]
        public dynamic Updatelensmaster([FromBody] LensMatserDataView uplens, string ID, int doctorID)
        {
            return _repoWrapper.LensMaster.Updatelensmaster(uplens, ID, doctorID);
        }

        [HttpGet("Getframe/{Name}/{cmpid}")]
        public dynamic Getframe(string Name, int cmpid)
        {
            return _repoWrapper.LensMaster.Getframe(Name, cmpid);
        }

        [HttpPost("InsertFrameShape")]
        public dynamic InsertFrameShape([FromBody] LensMatserDataView AddFrameShape)
        {
            return _repoWrapper.LensMaster.InsertFrameShape(AddFrameShape);
        }

        [HttpGet("lensmasterFrameShape")]
        public dynamic lensmasterFrameShape()
        {
            return _repoWrapper.LensMaster.lensmasterFrameShape();
        }

        [HttpPost("updateFrameShape/{IDD}")]
        public dynamic updateFrameShape([FromBody] LensMatserDataView UpFrameShape, int IDD)
        {
            return _repoWrapper.LensMaster.updateFrameShape(UpFrameShape, IDD);
        }

        [HttpPost("DeleteFrameShape/{IDD}")]
        public dynamic DeleteFrameShape(int IDD)
        {
            return _repoWrapper.LensMaster.DeleteFrameShape(IDD);
        }

        [HttpPost("InsertFrameType")]
        public dynamic InsertFrameType([FromBody] LensMatserDataView AddFrameType)
        {
            return _repoWrapper.LensMaster.InsertFrameType(AddFrameType);
        }

        [HttpGet("lensmasterFrameType")]
        public dynamic lensmasterFrameType()
        {
            return _repoWrapper.LensMaster.lensmasterFrameType();
        }

        [HttpPost("updateFrameType/{IDT}")]
        public dynamic updateFrameType([FromBody] LensMatserDataView UpFrameType, int IDT)
        {
            return _repoWrapper.LensMaster.updateFrameType(UpFrameType, IDT);
        }

        [HttpPost("DeleteFrameType/{IDT}")]
        public dynamic DeleteFrameType(int IDT)
        {
            return _repoWrapper.LensMaster.DeleteFrameType(IDT);
        }

        [HttpPost("InsertFrameStyle")]
        public dynamic InsertFrameStyle([FromBody] LensMatserDataView AddFrameStyle)
        {
            return _repoWrapper.LensMaster.InsertFrameStyle(AddFrameStyle);
        }

        [HttpGet("lensmasterFrameStyle")]
        public dynamic lensmasterFrameStyle()
        {
            return _repoWrapper.LensMaster.lensmasterFrameStyle();
        }

        [HttpPost("updateFrameStyle/{IDS}")]
        public dynamic updateFrameStyle([FromBody] LensMatserDataView UpFrameStyle, int IDS)
        {
            return _repoWrapper.LensMaster.updateFrameStyle(UpFrameStyle, IDS);
        }

        [HttpPost("DeleteFrameStyle/{IDS}")]
        public dynamic DeleteFrameStyle(int IDS)
        {
            return _repoWrapper.LensMaster.DeleteFrameStyle(IDS);
        }

        [HttpPost("InsertFrameWidth")]
        public dynamic InsertFrameWidth([FromBody] LensMatserDataView AddFrameWidth)
        {
            return _repoWrapper.LensMaster.InsertFrameWidth(AddFrameWidth);
        }

        [HttpGet("lensmasterFrameWidth")]
        public dynamic lensmasterFrameWidth()
        {
            return _repoWrapper.LensMaster.lensmasterFrameWidth();
        }

        [HttpPost("updateFrameWidth/{IDW}")]
        public dynamic updateFrameWidth([FromBody] LensMatserDataView UpFrameWidth, int IDW)
        {
            return _repoWrapper.LensMaster.updateFrameWidth(UpFrameWidth, IDW);
        }

        [HttpPost("DeleteFrameWidth/{IDW}")]
        public dynamic DeleteFrameWidth(int IDW)
        {
            return _repoWrapper.LensMaster.DeleteFrameWidth(IDW);
        }


        [HttpPost("Deleteframelens/{lensTID}/{Cmpid}/{name}")]
        public dynamic Deleteframelens(int lensTID, int Cmpid, string name)
        {
            return _repoWrapper.LensMaster.Deleteframelens(lensTID, Cmpid, name);
        }


        [HttpPost("InsertUploadedExceldata/{CompanyID}/{docotorid}")]
        public dynamic InsertUploadedExceldata([FromBody] LensMatserDataView UpdateExceldatas, int CompanyID, int docotorid)
        {
            return _repoWrapper.LensMaster.InsertUploadedExceldata(UpdateExceldatas, CompanyID, docotorid);
        }

        [HttpPost("InsertstockUploadedExceldata/{CompanyID}/{docotorid}/{storename}/{M_Vendor}/{TransactionId}")]
        public dynamic InsertstockUploadedExceldata([FromBody] LensMatserDataView UpdatestockExceldatas, int CompanyID, int docotorid, int storename, int M_Vendor, int TransactionId)
        {
            String gnr = _repoWrapper.Common.GenerateRunningCtrlNoo(TransactionId, CompanyID, "GetRunningNo");

            if (gnr == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }

            UpdatestockExceldatas.ResData = gnr;

            return _repoWrapper.LensMaster.InsertstockUploadedExceldata(UpdatestockExceldatas, CompanyID, docotorid, storename, M_Vendor, TransactionId);
        }


    }
}
