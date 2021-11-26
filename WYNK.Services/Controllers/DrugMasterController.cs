
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class DrugMasterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public DrugMasterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpPost("AddUOM/{UOM}/{DoctorID}")]
        public dynamic AddUOM(string UOM, int DoctorID)
        {
            return _repoWrapper.drugMaster.AddUOM(UOM, DoctorID);
        }

        [HttpPost("AddDrug/{Cmpid}")]
        public dynamic AddDrug([FromBody] DrugMaster AddDrug, int? cmpid)
        {
            return _repoWrapper.drugMaster.AddDrug(AddDrug, cmpid);
        }


        [HttpPost("UpdateDrug/{ID}/{Cmpid}")]
        public dynamic UpdateDrug([FromBody] DrugMaster DrugMaster, int? ID,int? cmpid)
        {
            return _repoWrapper.drugMaster.UpdateDrug(DrugMaster, ID, cmpid);
        }



        [HttpPost("deleteDrug/{ID}")]
        public dynamic deleteDrug(int? ID)
        {
            return _repoWrapper.drugMaster.deleteDrug(ID);
        }



        [HttpPost("AddDrugGroup")]
        public dynamic AddDrugGroup([FromBody] DrugMaster AddDrugGroup)
        {
            return _repoWrapper.drugMaster.AddDrugGroup(AddDrugGroup);
        }


        [HttpGet("DrugGroupFormDesc/{Value}")]
        public dynamic DrugGroupFormDesc(int Value)
        {
            return _repoWrapper.drugMaster.DrugGroupFormDesc(Value);
        }


        [HttpGet("getTaxValues/{ID}")]
        public dynamic getTaxValues(int ID)
        {
            return _repoWrapper.drugMaster.getTaxValues(ID);
        }

        [HttpPost("updatedata/{ID}")]
        public dynamic updatedata([FromBody] DrugMaster UpdateDrugGroup, int ID)
        {
            return _repoWrapper.drugMaster.updatedata(UpdateDrugGroup, ID);
        }


        [HttpPost("DeleteGenericMedicine/{ID}")]
        public dynamic DeleteGenericMedicine(int ID)
        {
            return _repoWrapper.drugMaster.DeleteGenericMedicine(ID);
        }





        [HttpPost("uploadImag/{M_Brand}")]
        public bool uploadImag(string M_Brand)
        {
            var file = Request.Form.Files[0];
            return _repoWrapper.drugMaster.uploadImagedrug(file, M_Brand);
        }

        [HttpGet("GetDrugimage/{Brand}")]
        public dynamic GetDrugimage(string Brand)
        {
            return _repoWrapper.drugMaster.GetDrugimage(Brand);
        }

        [HttpPost("DeleteDrugGroup/{ID}")]
        public dynamic DeleteDrugGroup(int ID)
        {
            return _repoWrapper.drugMaster.DeleteDrugGroup(ID);
        }

    }
}



