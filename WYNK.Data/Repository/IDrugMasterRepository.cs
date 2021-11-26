
using WYNK.Data.Model.ViewModel;
using Microsoft.AspNetCore.Http;

namespace WYNK.Data.Repository
{
    public interface IDrugMasterRepository : IRepositoryBase<DrugMaster>
    {

        dynamic AddDrug(DrugMaster AddDrug, int? cmpid);
        dynamic UpdateDrug(DrugMaster DrugMaster, int? ID,int? cmpid);
        dynamic deleteDrug(int? iD);
        dynamic AddDrugGroup(DrugMaster AddDrugGroup);
        dynamic AddUOM(string UOM, int DoctorID);
        dynamic DrugGroupFormDesc(int Value);
        dynamic getTaxValues(int ID);
        dynamic updatedata(DrugMaster UpdateDrugGroup, int ID);


        dynamic DeleteGenericMedicine(int ID);


        dynamic uploadImagedrug(IFormFile file, string M_Brand);
        dynamic GetDrugimage(string Brand);
        dynamic DeleteDrugGroup(int ID);
    }
}
