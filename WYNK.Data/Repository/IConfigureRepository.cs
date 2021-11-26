
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IConfigureRepository : IRepositoryBase<ConfigureViewModel>
    {
        dynamic InsertCon(ConfigureViewModel Con);
        dynamic UpdateCon(ConfigureViewModel Con1, int ID);
        dynamic Gettrans(int ID);
        dynamic ConfiguretransDet(int ID);        

    }

}


