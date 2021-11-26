
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IMedicineMappingRepository : IRepositoryBase<MedicineMapping>
    {

        MedicineMapping Getchilddrug(int drugid, int cmpid);//Getrecdtls
        MedicineMapping Getrecdtls(int drugid, int cmpid);
        MedicineMapping Getvalues();
        dynamic InsertMedicineMapping(MedicineMapping MedicineMapping, int userid, int drugid);//UpdateFood



    }
}
