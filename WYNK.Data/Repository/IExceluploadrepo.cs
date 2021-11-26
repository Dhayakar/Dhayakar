using System;

using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IExceluploadrepo : IRepositoryBase<ExcelViewModel>
    {
        dynamic UpdateExceldata(ExcelViewModel UpdateExceldata, int CmpId);
        dynamic UpdateDrugExceldata(ExcelViewModel UpdateExceldatas, int CmpId,int Createdby);
        dynamic UpdateStrabismusdata(ExcelViewModel UpdateExceldatass, int CmpId, int Createdby);
        dynamic GetStatusDrugStock(DrugStockExcel DrugList,int CmpId );
        dynamic GetStatusOpticalStock(OpticalStockExcel OpticalList, int CmpId);
        dynamic SubmitDrugStock(DrugStockExcel DrugList,int CmpId,int UserID,int TC,DateTime Dates);
        dynamic SubmitOpticalStock(OpticalStockExcel OpticList, int CmpId, int UserID, int TC, DateTime Dates);
        dynamic GettingDrugIdsDesc(int CmpId);
        dynamic VendorDrugCheck(DrugStockExcel DrugList, int CmpId,int VendorID);
        dynamic UpdateSurgeryExceldata(ExcelViewModel UpdateSurgeryExceldata, int CmpId, int Createdby, int Icd, string surgery, int popup);
        dynamic GetExistingRecord(int ICD, string Surgery);

    }
}