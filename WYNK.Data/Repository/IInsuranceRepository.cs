
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IInsuranceRepository : IRepositoryBase<InsuranceViewModel>
    {
        InsuranceViewModel GetlocationDetails(int id);
        InsuranceViewModel GetPinCodeDetails(int id);
        dynamic InsertInsurance(InsuranceViewModel AddInsurance);
        dynamic UpdateInsurance(InsuranceViewModel InsuranceUpdate, int ID);


        InsuranceViewModel GetPreAuthorizationdtls(int cmpid);
        InsuranceViewModel GetPreAUZupdate(int cmpid);
        dynamic InsertPreAuthorization(InsuranceViewModel AddInsurance, int ID);
        dynamic DeletePreA(InsuranceViewModel AddInsurance, int ID);


        InsuranceViewModel GetEstimateTrackingdtls(int cmpid);
        dynamic InsertEstimateTracking(InsuranceViewModel AddEstimateTracking);
        InsuranceViewModel GetETDetails(int cmpid);
        
    }
}
