
using WYNK.Data.Model.ViewModel;


namespace WYNK.Data.Repository
{
    public interface ICampMasterRepository : IRepositoryBase<campmasterViewModel>
    {
        dynamic AddOrganization(string ID);
        dynamic UpdateOrganization(string ID,string status, string Desc);
        dynamic DeleteOrganization(string ID, int valueolmid);
        dynamic Getallorganization();
        dynamic Getallcampmasterdata();
        dynamic SubmitOrganizationData(campmasterViewModel Fundus);
        dynamic SubmitCampmasterData(campmasterViewModel Fundus);
        
        dynamic UpdateOrganizationData(campmasterViewModel Fundus);
        dynamic Updatecampmasterdata(campmasterViewModel Fundus);
        

    }
}
