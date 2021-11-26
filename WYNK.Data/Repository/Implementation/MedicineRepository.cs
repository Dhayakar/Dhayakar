
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class MedicineRepository : RepositoryBase<Vmmedicine>, IMedicineRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;

        public MedicineRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }
        public Vmmedicine ManualSearch(DateTime GivenDate, int CompanyID)
        {
            var manualsearch = new Vmmedicine();
            var drug = WYNKContext.DrugMaster.ToList();
            var ToDate = Convert.ToDateTime(GivenDate).ToString("dd-MM-yyyy");
            var StoreMaster = CMPSContext.Storemasters.Where(x => x.CmpID == CompanyID).ToList();

            manualsearch.BatchDrugs = (from IB in WYNKContext.ItemBatch.Where(x => (x.ItemBatchExpiry.Date <= Convert.ToDateTime(ToDate)) && x.ItemBatchBalnceQty > 0)
                                       join DR in WYNKContext.DrugMaster.Where(x => x.Cmpid == CompanyID) on IB.ItemID equals DR.ID
                                       select new Medicine
                                       {
                                           Brand = DR.Brand,
                                           ItemBatchNumber = IB.ItemBatchNumber,
                                           UOM = DR.UOM,
                                           ItemBatchQty = IB.ItemBatchQty,
                                           ItemBatchBalanceQty = IB.ItemBatchBalnceQty,
                                           ItemBatchExpiry = IB.ItemBatchExpiry,
                                           StoreName = StoreMaster.Where(x => x.StoreID == IB.StoreID).Select(x => x.Storename).FirstOrDefault(),
                                       }).ToList();

            manualsearch.SerialDrugs = (from IS in WYNKContext.ItemSerial.Where(x => (x.ExpiryDate <= Convert.ToDateTime(ToDate)) && x.IssueNo == null)
                                        join DR in WYNKContext.DrugMaster.Where(x => x.Cmpid == CompanyID) on IS.ItemID equals DR.ID
                                        select new Medicine
                                        {
                                            Brand = DR.Brand,
                                            ItemBatchNumber = IS.SerialNo,
                                            UOM = DR.UOM,
                                            ItemBatchQty = 1,
                                            ItemBatchBalanceQty = 1,
                                            ItemBatchExpiry = Convert.ToDateTime(IS.ExpiryDate),
                                            StoreName = StoreMaster.Where(x => x.StoreID == IS.StoreID).Select(x => x.Storename).FirstOrDefault(),
                                        }).ToList();

            return manualsearch;
        }
        public Vmmedicine MonthSearch(DateTime FromDate, DateTime ToDate, int CompanyID)
        {
            var manualsearch = new Vmmedicine();
            var drug = WYNKContext.DrugMaster.ToList();
            var FromDates = Convert.ToDateTime(FromDate).ToString("dd-MM-yyyy");
            var ToDates = Convert.ToDateTime(ToDate).ToString("dd-MM-yyyy");
            var StoreMaster = CMPSContext.Storemasters.Where(x => x.CmpID == CompanyID).ToList();

            manualsearch.BatchDrugs = (from IB in WYNKContext.ItemBatch.Where(x => x.ItemBatchExpiry.Date >= Convert.ToDateTime(FromDates) && x.ItemBatchExpiry.Date <= Convert.ToDateTime(ToDates) && x.ItemBatchBalnceQty > 0)
                                       join DR in WYNKContext.DrugMaster.Where(x => x.Cmpid == CompanyID) on IB.ItemID equals DR.ID
                                       select new Medicine
                                       {
                                           Brand = DR.Brand,
                                           ItemBatchNumber = IB.ItemBatchNumber,
                                           UOM = DR.UOM,
                                           ItemBatchQty = IB.ItemBatchQty,
                                           ItemBatchBalanceQty = IB.ItemBatchBalnceQty,
                                           ItemBatchExpiry = IB.ItemBatchExpiry,
                                           StoreName = StoreMaster.Where(x => x.StoreID == IB.StoreID).Select(x => x.Storename).FirstOrDefault(),
                                       }).ToList();

            manualsearch.SerialDrugs = (from IS in WYNKContext.ItemSerial.Where(x => x.ExpiryDate >= Convert.ToDateTime(FromDates) && x.ExpiryDate <= Convert.ToDateTime(ToDates) && x.IssueNo == null)
                                        join DR in WYNKContext.DrugMaster.Where(x => x.Cmpid == CompanyID) on IS.ItemID equals DR.ID
                                        select new Medicine
                                        {
                                            Brand = DR.Brand,
                                            ItemBatchNumber = IS.SerialNo,
                                            UOM = DR.UOM,
                                            ItemBatchQty = 1,
                                            ItemBatchBalanceQty = 1,
                                            ItemBatchExpiry = Convert.ToDateTime(IS.ExpiryDate),
                                            StoreName = StoreMaster.Where(x => x.StoreID == IS.StoreID).Select(x => x.Storename).FirstOrDefault(),
                                        }).ToList();
            return manualsearch;
        }
    }
}
