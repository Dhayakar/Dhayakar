
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface ICounsellingRepository : IRepositoryBase<Counselling_Master>
    {

        Counselling_Master getConcerntextfile(int CompanyID);
        Counselling_Master getsearchdetails(int CompanyID);
        Counselling_Master GetParticularLens(string Specdesc);
        dynamic getuintotaldatahistory(int CompanyID, string UIN);

        dynamic InsertCouns(Counselling_Master couns);
        Counselling_Master GetCnsDetail();
        dynamic deletecns(int? ID);
        dynamic UpdateCouns(Counselling_Master cps, int ID);
        dynamic getSurgeonDetails(SURGDETAILS SUrgeondsesss);        
        dynamic getanesthetistDetails(SURGDETAILS Anesthesesss);
       

        dynamic InsertCounsellingData(savingCounsellingdetails InsertCounselling);
    }
}