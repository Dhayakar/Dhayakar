
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface ILensMasterRepository : IRepositoryBase<LensMatserDataView>
    {
        dynamic GettaxDetails(int ID);
        dynamic Insertlensmaster(LensMatserDataView Addlens);
        dynamic Getlensfull(string RandomUniqueID, int cmpid, string name);
        dynamic Updatelensmaster(LensMatserDataView uplens, string ID, int doctorID);
        dynamic Getframe(string Name, int cmpid);
        dynamic InsertFrameShape(LensMatserDataView AddFrameShape);
        dynamic lensmasterFrameShape();
        dynamic updateFrameShape(LensMatserDataView UpFrameShape, int IDD);
        dynamic DeleteFrameShape(int IDD);
        dynamic InsertFrameType(LensMatserDataView AddFrameType);
        dynamic lensmasterFrameType();
        dynamic updateFrameType(LensMatserDataView UpFrameType, int IDT);
        dynamic DeleteFrameType(int IDD);
        dynamic InsertFrameStyle(LensMatserDataView AddFrameStyle);
        dynamic lensmasterFrameStyle();
        dynamic updateFrameStyle(LensMatserDataView UpFrameStyle, int IDS);
        dynamic DeleteFrameStyle(int IDS);
        dynamic InsertFrameWidth(LensMatserDataView AddFrameWidth);
        dynamic lensmasterFrameWidth();
        dynamic updateFrameWidth(LensMatserDataView UpFrameWidth, int IDW);
        dynamic DeleteFrameWidth(int IDW);
        dynamic Deleteframelens(int ID, int Cmpid, string name);
        dynamic InsertUploadedExceldata(LensMatserDataView UpdateExceldatas, int CompanyID, int docotorid);
        dynamic InsertstockUploadedExceldata(LensMatserDataView UpdatestockExceldatas, int cmpid, int Createdby, int StoreID, int M_Vendor, int TransactionId);
    }
}
