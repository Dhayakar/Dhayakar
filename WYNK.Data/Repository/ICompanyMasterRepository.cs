
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface ICompanyMasterRepository : IRepositoryBase<CompanyMasterView>
    {

        dynamic insertdata(CompanyMasterView companyMaster, string time, string currency, int cid);
        dynamic UpdateCompanyDet(CompanyMasterView companyMaster, int ID, string time, string currency, int cid);
        dynamic DeleteCompanyDet(int ID);
        CompanyMasterView SelectCompany();
        CompanyMasterView SelectModules();
        CompanyMasterView SelecNumberControldata();


        CompanyMasterView getCompanyName(int cmpid);
    }
}
