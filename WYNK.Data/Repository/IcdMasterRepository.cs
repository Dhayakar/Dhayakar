
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IcdMasterRepository : IRepositoryBase<ICDMaster>
    {

        dynamic AddIcd(ICDMaster AddIcd);
        dynamic SurgeryCostDetailSubmit(ICDMaster AddIcd);
        dynamic AddSpecial(ICDMaster AddSpecial);
        dynamic AddIcdgroup(ICDMaster Addicds);
        dynamic DeleteICD(string Code);
        dynamic UpdateIcd(ICDMaster icdmaster, string Code, int id);
        dynamic getSurgeryCostDetail(string ICDCODE, int CMPID,string roomtype);
        ICDMaster GetDoctorSpecialitydetails(int DID, int CMPID);
        dynamic GetAllICDDescription(string docid);

    }
}
