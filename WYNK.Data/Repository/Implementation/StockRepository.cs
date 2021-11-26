using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
namespace WYNK.Data.Repository.Implementation
{
    public class StockRepository : RepositoryBase<StockViewmodel>, IStockRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public StockRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic GetStockDetails2(string BrandName, DateTime Date)
        {
            var stockperiod = new StockViewmodel();
            var drug = WYNKContext.DrugMaster.ToList();
            int ManID = Convert.ToInt32(WYNKContext.DrugMaster.Where(x => x.Brand == BrandName).Select(x => x.Manufacturer).FirstOrDefault());
            stockperiod.Manufacturer = CMPSContext.OneLineMaster.Where(x => x.OLMID == ManID).Select(x => x.ParentDescription).FirstOrDefault();
            stockperiod.BrandName = BrandName;
            var item = WYNKContext.ItemBatch.ToList();
            var one = CMPSContext.OneLineMaster.ToList();
            var date = Convert.ToDateTime(Date).ToString("dd-MM-yyyy");
            stockperiod.StockDetails2 = (from D in drug
                                         join I in item.Where(x => Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(Date))
                                         on D.ID equals I.ItemID

                                         where D.Brand == BrandName
                                         orderby D.Brand
                                         select new StockDetails2
                                         {
                                             BatchNumber = I.ItemBatchNumber,
                                             BatchQty = I.ItemBatchQty,
                                             BatchIssueQty = I.ItemBatchissueQty,
                                             BatchBalanceQty = I.ItemBatchBalnceQty,
                                             BatchExpiry = I.ItemBatchExpiry

                                         }).ToList();

            return stockperiod;
        }
        public dynamic GetStockDetails3(string BrandName, DateTime Date)
        {
            var stockperiod = new StockViewmodel();
            var drug = WYNKContext.DrugMaster.ToList();
            int ManID = Convert.ToInt32(WYNKContext.DrugMaster.Where(x => x.Brand == BrandName).Select(x => x.Manufacturer).FirstOrDefault());
            stockperiod.Manufacturer = CMPSContext.OneLineMaster.Where(x => x.OLMID == ManID).Select(x => x.ParentDescription).FirstOrDefault();
            stockperiod.BrandName = BrandName;
            var item = WYNKContext.ItemBatch.ToList();
            var one = CMPSContext.OneLineMaster.ToList();
            var itns = WYNKContext.ItemBatchTrans.ToList();
            var date = Convert.ToDateTime(Date).ToString("dd-MM-yyyy");
            stockperiod.StockDetails2 = (from D in drug
                                         join I in item.Where(x => x.ItemBatchExpiry > Convert.ToDateTime(Date))
                                         on D.ID equals I.ItemID
                                         where D.Brand == BrandName
                                         orderby D.Brand
                                         select new StockDetails2
                                         {
                                             BrandName = D.Brand,
                                             BatchNumber = I.ItemBatchNumber,
                                             BatchQty = I.ItemBatchQty,
                                             BatchIssueQty = I.ItemBatchissueQty,
                                             BatchBalanceQty = I.ItemBatchBalnceQty,
                                             BatchExpiry = I.ItemBatchExpiry

                                         }).ToList();

            return stockperiod;
        }
        public dynamic GetStockDetails4(string BrandName, DateTime Date)
        {
            var stockperiod = new StockViewmodel();
            var drug = WYNKContext.DrugMaster.ToList();
            int ManID = Convert.ToInt32(WYNKContext.DrugMaster.Where(x => x.Brand == BrandName).Select(x => x.Manufacturer).FirstOrDefault());
            stockperiod.Manufacturer = CMPSContext.OneLineMaster.Where(x => x.OLMID == ManID).Select(x => x.ParentDescription).FirstOrDefault();
            stockperiod.BrandName = BrandName;
            var item = WYNKContext.ItemBatch.ToList();
            var one = CMPSContext.OneLineMaster.ToList();
            var itns = WYNKContext.ItemBatchTrans.ToList();

            var date = Convert.ToDateTime(Date).ToString("dd-MM-yyyy");

            stockperiod.StockDetails2 = (from D in drug
                                         join I in item.Where(x => x.ItemBatchExpiry < Convert.ToDateTime(Date))
                                         on D.ID equals I.ItemID

                                         where D.Brand == BrandName
                                         orderby D.Brand
                                         select new StockDetails2
                                         {
                                             BrandName = D.Brand,
                                             BatchNumber = I.ItemBatchNumber,
                                             BatchQty = I.ItemBatchQty,
                                             BatchIssueQty = I.ItemBatchissueQty,
                                             BatchBalanceQty = I.ItemBatchBalnceQty,
                                             BatchExpiry = I.ItemBatchExpiry


                                         }).ToList();

            return stockperiod;
        }


        public dynamic Search(DateTime Date, int CompanyID)
        {
            var stockperiod = new StockViewmodel();

            var drug = WYNKContext.DrugMaster.ToList();
            var item = WYNKContext.ItemBatch.ToList();
            var stmr = WYNKContext.StockMaster.ToList();
            var strn = WYNKContext.StockTran.ToList();
            var date = Convert.ToDateTime(Date).ToString("dd-MM-yyyy");
            stockperiod.StockDetails = (from SM in stmr.Where(x => x.CMPID == CompanyID)
                                        join ST in strn
                                        on SM.RandomUniqueID equals ST.SMID
                                        join IB in item.Where(x => Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(Date))
                                        on ST.ItemID equals IB.ItemID
                                        join DM in drug
                                        on IB.ItemID equals DM.ID

                                        select new StockDetails
                                        {
                                            BrandName = DM.Brand,
                                            PhysicalStock = IB.ItemBatchBalnceQty,

                                        }).ToList();

            stockperiod.get = (from res in stockperiod.StockDetails.GroupBy(x => (x.BrandName))
                               select new get

                               {
                                   BrandName = res.Key,
                                   PhysicalStock = res.Sum(x => (x.PhysicalStock)),

                               }).ToList();


            var v = (from d in item
                     .Where(x => Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(Date))
                     join Drg in drug
                     on d.ItemID equals Drg.ID
                     group d by new { Drg.Brand } into i
                     select new
                     {
                         Drug = i.Key,
                         ActStk = i.Where(x => x.CreatedUTC.Date <= Convert.ToDateTime(Date)).Sum(x => x.ItemBatchBalnceQty),
                         ExpStk = i.Where(x => x.ItemBatchExpiry.Date < Convert.ToDateTime(Date)).Sum(x => x.ItemBatchBalnceQty),
                         ConStk = i.Where(x => x.ItemBatchExpiry.Date >= Convert.ToDateTime(Date)).Sum(x => x.ItemBatchBalnceQty),
                     });


            stockperiod.StockDetails1 = (from res1 in v

                                         select new StockDetails1
                                         {

                                             PhysicalStock = res1.ActStk,
                                             ConsumableStock = res1.ConStk,
                                             ExpiredStock = res1.ExpStk,
                                             BrandName = res1.Drug.Brand,
                                             UOM = WYNKContext.DrugMaster.Where(x => x.Brand == res1.Drug.Brand).Select(x => x.UOM).FirstOrDefault()




                                         }).ToList();






            return stockperiod;



        }


    }
}


