
using WYNK.Data.Model.ViewModel;


namespace WYNK.Data.Repository
{
  public interface IFundusRepository :IRepositoryBase<FundusViewM>
    {

        dynamic GetDesc();//GetDescfif
        dynamic GetDescfif();
        dynamic GetDescfn();
        dynamic InsertDesc(FundusViewM Fundus);
        dynamic GetSubDesc(int ID);
        dynamic InsertSubdesc(FundusViewM Fundus);
        dynamic GetadDesc(int ID);
        dynamic InsertAddesc(FundusViewM Fundus);
        dynamic UpdateDesc(FundusViewM Fundus, int ID);
        dynamic GetfullDesc(int ID);
        dynamic UpdateSubDesc(FundusViewM Fundus, int ID);
        dynamic UpdateaddDesc(FundusViewM Fundus, int ID);
        dynamic Deletedes(FundusViewM Fundus, int ID);
        dynamic Deletedess(FundusViewM Fundus, int ID);
        dynamic Deletedesss(FundusViewM Fundus, int ID);
        dynamic GetSubDescfi(int ID);//GetaddDescfi
        dynamic GetaddDescfi(int ID);
    }
}
