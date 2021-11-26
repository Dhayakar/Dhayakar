
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface ICampRegistrationRepository : IRepositoryBase<CampRegistrationViewModel>
    {

        dynamic InsertCampReg(CampRegistrationViewModel CampRegistration, int userid, int Transactionid);
        CampRegistrationViewModel GetCampRegistrationExtension(string CampRegNO, int CmpID);
        CampRegistrationViewModel GetCampkindetail(string CampUIN);
        dynamic UdateCampReg(CampRegistrationViewModel CampRegistrationUpdate, int CmpID, int userid, string Campuin);
        dynamic DeleteCampReg(string Campuin, int CMPID, int USERID, string Companyname);

    }
}
