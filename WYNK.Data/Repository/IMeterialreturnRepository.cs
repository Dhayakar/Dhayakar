
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IMeterialreturnRepository : IRepositoryBase<Meterialview>
    {
        Meterialview PopupSearch(int CMMPID);
        dynamic VendorSearch(string ReciptNumber);
        Meterialview DrugQtySearch(string DrugValue, string GRN,string IBvalue,int Storeid);
        dynamic InsertQty(Meterialview Con ,int TransactionTypeid);
        Meterialview UOMSearch(string GRN,string DRUG);
        Meterialview GetBatch(string Grn, string Drugvalue,int storeid);
        dynamic getallreturndetails(string FromDate, string Todate, int value, int cmpid, int Trid);
        dynamic Getperiodmaterialdetails(string Phase, int value, int cmpid, int trid);
        dynamic Checkfinancialyear(string Phase, int cmpid);
        dynamic getmaterialreturndetailsbasedonGRN(string Phase);
        dynamic getallreturndetailsbasedonrn(string Phase, int CMPID, int trid);
        dynamic Submitopticaldata(Meterialview OpticalMaterialretrunsavedata);
        dynamic GetHistoryDetails(int Phase, int cmpid);
    }
}
