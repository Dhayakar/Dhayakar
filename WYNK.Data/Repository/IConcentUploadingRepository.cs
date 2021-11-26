using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IConcentUploadingRepository : IRepositoryBase<ConcentUploadingViewModel>
    {
        dynamic InsertConcent(ConcentUploadingViewModel Con);
        dynamic Getallconnectionstring(int CMPID, string Module);
    }
}


