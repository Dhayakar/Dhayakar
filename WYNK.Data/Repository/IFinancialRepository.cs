
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IFinancialRepository : IRepositoryBase<Financialview>
    {
        dynamic InsertFart(Financialview Fe);
        dynamic UpdateFart(Financialview Fe1, int ID, int FID);
        Financialview GetFpartDetail(int cmpid);
        Financialview GetStsDetail(int M_ID, int docotorid);
        Financialview GetStatusDetails(int docotorid, int FYD);        

    }
}
