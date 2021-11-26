
using WYNK.Data.Model.ViewModel;


namespace WYNK.Data.Repository
{
    public interface ICustomerMasterRepository : IRepositoryBase<CustomerMasterViewModel>
    {
        dynamic SubmitCustomer(CustomerMasterViewModel CustomerDetails);
        dynamic UpdateCustomerMaster(CustomerMasterViewModel CustomerDetails,int ID);
        dynamic DeleteCustomerMaster(int ID, int CMPID, int USERID);
    }
}
