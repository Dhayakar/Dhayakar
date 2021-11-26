using WYNK.Data.Model.ViewModel;



namespace WYNK.Data.Repository
{
    public interface IMedicalBillRegisterRepository : IRepositoryBase<BillingPharmacy>
    {
        dynamic getMedBillDet(string FromDate, string Todate, int CompanyID);

    }
}
