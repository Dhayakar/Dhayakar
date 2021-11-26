
using WYNK.Data.Model.ViewModel;


namespace WYNK.Data.Repository
{
    public interface IlocationRepository : IRepositoryBase<LocationMasterViewModel>
    {

        dynamic Getcountry();
        dynamic InsertCountry(LocationMasterViewModel country);
        dynamic GetState(int ID);
        dynamic InsertState(LocationMasterViewModel state);
        dynamic GetCity(int ID);
        dynamic InsertCity(LocationMasterViewModel City);
        dynamic Getlocation(int ID);
        dynamic Insertlocation(LocationMasterViewModel Location);
        dynamic Getfulllocation(int ID);
        dynamic Updatelocation(LocationMasterViewModel upLocation, int ID);
        dynamic Updatecity(LocationMasterViewModel upcity, int ID);
        dynamic UpdateState(LocationMasterViewModel upstate, int ID);
        dynamic DeleteLocation(int ID);
    }
}
