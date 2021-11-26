
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;
using System;


namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class MeterialreturnController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public MeterialreturnController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("PopupSearch/{CMMPID}")]
        public Meterialview PopupSearch(int CMMPID)
        {
            return _repoWrapper.Meterial.PopupSearch(CMMPID);
        }

        [HttpGet("VendorSearch/{ReciptNumber}")]
        public dynamic VendorSearch(string ReciptNumber)
        {
            return _repoWrapper.Meterial.VendorSearch(ReciptNumber);
        }

        [HttpGet("DrugQtySearch/{DrugValue}/{GRN}/{IBvalue}/{Storeid}")]
        public Meterialview DrugSearch(string DrugValue, string GRN,string IBvalue,int Storeid)
        {
            return _repoWrapper.Meterial.DrugQtySearch(DrugValue, GRN, IBvalue, Storeid);
        }

        [HttpGet("UOMSearch/{GRN}/{DRUG}")]
        public Meterialview UOMSearch(string GRN,string DRUG)
        {
            return _repoWrapper.Meterial.UOMSearch(GRN,DRUG);
        }

        [HttpPost("InsertQty")]
        public dynamic InsertQty([FromBody]Meterialview Con, int TransactionTypeid)
        {            
            
            return _repoWrapper.Meterial.InsertQty(Con,  TransactionTypeid);
            
        }

        [HttpGet("GetBatch/{Grn}/{Drugvalue}/{storeid}")]
        public Meterialview GetBatch(string Grn, string Drugvalue,int storeid)
        {
            return _repoWrapper.Meterial.GetBatch(Grn, Drugvalue, storeid);
        }

        [HttpGet("getallreturndetails/{FromDate}/{Todate}/{value}/{cmpid}/{Trid}")]
        public dynamic getallreturndetails(string FromDate, string Todate, int value, int cmpid, int Trid)
        {
            return _repoWrapper.Meterial.getallreturndetails(FromDate, Todate, value, cmpid, Trid);
        }

        [HttpGet("Getperiodmaterialdetails/{Phase}/{value}/{cmpid}/{trid}")]
        public dynamic Getperiodmaterialdetails(string Phase, int value, int cmpid, int trid)
        {
            return _repoWrapper.Meterial.Getperiodmaterialdetails(Phase, value, cmpid, trid);
        }
        [HttpGet("getallreturndetailsbasedonrn/{Phase}/{CMPID}/{trid}")]
        public dynamic getallreturndetailsbasedonrn(string Phase, int CMPID, int trid)
        {
            return _repoWrapper.Meterial.getallreturndetailsbasedonrn(Phase, CMPID, trid);
        }


        [HttpGet("getmaterialreturndetailsbasedonGRN/{Phase}")]
        public dynamic getmaterialreturndetailsbasedonGRN(string Phase)
        {
            return _repoWrapper.Meterial.getmaterialreturndetailsbasedonGRN(Phase);
        }

        [HttpGet("Checkfinancialyear/{Phase}/{cmpid}")]
        public dynamic Checkfinancialyear(string Phase, int cmpid)
        {
            return _repoWrapper.Meterial.Checkfinancialyear(Phase, cmpid);
        }

        [HttpPost("Submitopticaldata")]
        public dynamic Submitopticaldata([FromBody]Meterialview OpticalMaterialretrunsavedata)
        {
            var cmpid = OpticalMaterialretrunsavedata.CompanyID;
            var transactionid = OpticalMaterialretrunsavedata.TransactiomnID;
            String gnr = _repoWrapper.Common.GenerateRunningCtrlNoo(transactionid, cmpid, "UpdateRunningNo");

            if (gnr == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }
            OpticalMaterialretrunsavedata.NewDocnumber = gnr;
            return _repoWrapper.Meterial.Submitopticaldata(OpticalMaterialretrunsavedata);

        }

        [HttpGet("GetHistoryDetails/{Phase}/{cmpid}")]
        public dynamic GetHistoryDetails(int Phase, int cmpid)
        {
            return _repoWrapper.Meterial.GetHistoryDetails(Phase, cmpid);
        }


    }
}
