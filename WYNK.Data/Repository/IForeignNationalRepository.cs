
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IForeignNationalRepository : IRepositoryBase<ForeignNationalViewModel>
    {
        
        dynamic InsertForeignNational(ForeignNationalViewModel AddForeignNational);
        dynamic getForeignNational(int CmpID);
        dynamic UpdateForeignNational(ForeignNationalViewModel ForeignNationalUpdate, int ID,int Cmpid);

    }
}
