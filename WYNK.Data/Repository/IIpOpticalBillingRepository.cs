
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IIpOpticalBillingRepository : IRepositoryBase<OpticalBilling>
    {

        OpticalBilling GetPatientDetails(int id);
        OpticalBilling GetOpticalDetails(int id, int rid);
        OpticalBilling GetLensDetails();
        dynamic UpdateOPBilling(OpticalBilling OpticalBilling, string UIN);
    }
}
