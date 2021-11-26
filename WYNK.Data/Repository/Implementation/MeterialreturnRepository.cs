using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class MeterialreturnRepository : RepositoryBase<Meterialview>, IMeterialreturnRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
       

        public MeterialreturnRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public Meterialview PopupSearch(int CMMPID)
        {
            var Vendor = CMPSContext.VendorMaster.ToList();
            var Store = CMPSContext.Storemasters.ToList();
            var Stock = WYNKContext.StockMaster.ToList();
            var StocT = WYNKContext.StockTran.ToList();
            var Drug = WYNKContext.DrugMaster.ToList();
            var PopupSearch = new Meterialview();
            PopupSearch.Meterials = new List<Meterials>();
            PopupSearch.Meterials = (from ST in Stock.Where(x => x.TransactionType == "R" && x.CMPID == CMMPID)
                                     join SM in Store
                                     on ST.StoreID equals SM.StoreID
                                     join VM in Vendor
                                     on ST.VendorID equals VM.ID

                                     select new Meterials
                                     {
                                         ReciptNumber = ST.DocumentNumber,
                                         StoreName = SM.Storename,
                                         VendorName = VM.Name,
                                        
                                         StoreID = ST.StoreID,
                                         PoID = ST.POID,

                                     }).ToList();
            return PopupSearch;
        }

        dynamic IMeterialreturnRepository.VendorSearch(string ReciptNumber)
        {
            var Vendor = CMPSContext.VendorMaster.ToList();
            var Store = CMPSContext.Storemasters.ToList();
            var Stock = WYNKContext.StockMaster.ToList();
            var VendorSearch = new Meterialview();
            VendorSearch.Meterials2 = new List<Meterials2>();
            VendorSearch.Meterials2 = (from ST in Stock
                                       join SM in Store
                                       on ST.StoreID equals SM.StoreID
                                       join VM in Vendor
                                       on ST.VendorID equals VM.ID

                                       where ST.DocumentNumber == ReciptNumber
                                       select new Meterials2
                                       {
                                           ReciptNo = ST.DocumentNumber,
                                           RecipDate = ST.DocumentDate.Date,
                                           VendorN = VM.Name,
                                           Adress1 = VM.Address1,
                                           Adress2 = VM.Address2,
                                           Location = VM.Location,
                                           GSTNO = VM.GSTNo,
                                           MobileNo = VM.MobileNo,
                                           VendorId = ST.VendorID,
                                           IsActive = VM.IsActive,


                                       }).ToList();
            return VendorSearch;
        }

     
        public Meterialview GetBatch(string Grn, string Drugvalue, int storeid)
        {
            var registration = WYNKContext.Registration.ToList();
            var M_StockMasTran = WYNKContext.StockTran.ToList();
            var M_ItemBatch = WYNKContext.ItemBatch.ToList();
            var M_ItemBatchTran = WYNKContext.ItemBatchTrans.ToList();
            var M_StockMas = WYNKContext.StockMaster.ToList();

            var ID = WYNKContext.DrugMaster.Where(x => x.Brand == Drugvalue).Select(x => x.ID).FirstOrDefault();
            var SMID = M_StockMas.Where(x => x.DocumentNumber == Grn).Select(x => x.RandomUniqueID).FirstOrDefault();


            var MetView = new Meterialview();


            MetView.BatchNumbers = (from IBT in M_ItemBatchTran.Where(x => x.SMID == SMID && x.ItemID == ID)
                                    select new BatchNumbers
                                    {

                                        itemvalue = IBT.ItemBatchNumber,
                                        Drug = Drugvalue,

                                    }).ToList();

            return MetView;
        }

        public Meterialview DrugQtySearch(string DrugValue, string GRN, string IBvalue, int Storeid)
        {

            var DGroup = WYNKContext.DrugGroup.ToList();
            var StockMas = WYNKContext.StockMaster.ToList();
            var StokMasTaran = WYNKContext.StockTran.ToList();
            var ItemBatchs = WYNKContext.ItemBatch.ToList();
            var ItemBathTran = WYNKContext.ItemBatchTrans.ToList();
            var M_DrugM = WYNKContext.DrugMaster.ToList();
            var ID = M_DrugM.Where(x => x.Brand == DrugValue).Select(x => x.ID).FirstOrDefault();
            var SMID = StockMas.Where(x => x.DocumentNumber == GRN).Select(x => x.RandomUniqueID).FirstOrDefault();
            var Disc = StokMasTaran.Where(x => x.SMID == SMID && x.ItemID == ID).Select(x => x.DiscountPercentage);
            var taxid = M_DrugM.Where(x => x.ID == ID).Select(x => x.TaxID).FirstOrDefault();
            var MN = M_DrugM.Where(x => x.ID == ID).Select(x => x.GenericName).FirstOrDefault();
            var tax = CMPSContext.TaxMaster.ToList();
            var IBID = WYNKContext.ItemBatch.Where(x => x.ItemID == ID && x.ItemBatchNumber == IBvalue && x.StoreID == Storeid).Select(x => x.RandomUniqueID).FirstOrDefault();
            var LockedQTy = ItemBatchs.Where(x => x.RandomUniqueID == IBID && x.StoreID == Storeid).Select(x => x.LockedQuantity).FirstOrDefault();

            if (LockedQTy == null)
            {
                LockedQTy = 0;
            }

            var DrugQtySearch = new Meterialview();
            DrugQtySearch.Meterials5 = new List<Meterials5>();
            DrugQtySearch.Meterials5 = (from sm in StockMas.Where(x => x.DocumentNumber == GRN)
                                        join stt in StokMasTaran on sm.RandomUniqueID equals stt.SMID
                                        join ibt in ItemBathTran on stt.RandomUniqueID equals ibt.STID
                                        join ib in ItemBatchs.Where(x => x.ItemBatchNumber == IBvalue && x.StoreID == Storeid) on ibt.ItemBatchID equals ib.RandomUniqueID
                                        join dm in M_DrugM.Where(x => x.ID == ID) on ib.ItemID equals dm.ID
                                        group ib by new { dm.Brand } into NewVal
                                        select new Meterials5
                                        {
                                            DrugName1 = DrugValue,
                                            RecivedQty1 = NewVal.Sum(x => x.ItemBatchQty),
                                            ConsumedQty2 = NewVal.Sum(x => x.ItemBatchissueQty),
                                            ConsumedQty1 = NewVal.Sum(x => x.ItemBatchissueQty) + LockedQTy,
                                            BalanceQty1 = NewVal.Sum(x => x.ItemBatchBalnceQty),
                                            BalanceQty2 = NewVal.Sum(x => x.ItemBatchBalnceQty) - LockedQTy,
                                            BatchList1 = IBvalue,
                                            Expired = ItemBathTran.Where(x => x.ItemID == ID && x.ItemBatchID == IBID).Select(x => x.ItemBatchExpiry.Date).FirstOrDefault(),
                                            Rate = M_DrugM.Where(x => x.ID == ID).Select(x => x.Rate).FirstOrDefault(),
                                            GrossproductValue = 0,
                                            TotalDiscountValue = 0,
                                            TotalPOValue = 0,
                                            TotalSGSTTaxValue = 0,
                                            TotalCGSTTaxValue = 0,
                                            TotalIGSTTaxValue = 0,
                                            TotalTaxValue = 0,
                                            CGSTPercentage = tax.Where(x => x.ID == taxid).Select(x => x.CGSTPercentage).FirstOrDefault(),
                                            SGSTPercentage = tax.Where(x => x.ID == taxid).Select(x => x.SGSTPercentage).FirstOrDefault(),
                                            IGSTPercentage = tax.Where(x => x.ID == taxid).Select(x => x.IGSTPercentage).FirstOrDefault(),
                                            GSTPercentage = tax.Where(x => x.ID == taxid).Select(x => x.GSTPercentage).FirstOrDefault(),
                                            DiscountPercentage = StokMasTaran.Where(x => x.SMID == SMID && x.ItemID == ID).Select(x => x.DiscountPercentage).FirstOrDefault(),
                                            UOM = M_DrugM.Where(x => x.ID == ID).Select(x => x.UOM).FirstOrDefault(),
                                            GenericName = DGroup.Where(x => x.ID == MN).Select(x => x.Description).FirstOrDefault(),
                                            GSTTaxValue = 0,
                                            SGSTTaxValue = 0,
                                            CGSTTaxValue = 0,
                                            IGSTTaxValue = 0,
                                            DescountAmopunt = 0,


                                        }).ToList();
            return DrugQtySearch;
        }

        public Meterialview UOMSearch(string GRN, string DRUG)
        {
            var StockMas = WYNKContext.StockMaster.ToList();
            var StokMasTaran = WYNKContext.StockTran.ToList();
            var ItemBatchs = WYNKContext.ItemBatch.ToList();
            var M_DrugM = WYNKContext.DrugMaster.ToList();
            var DGROUp = WYNKContext.DrugGroup.ToList();
            var ID = M_DrugM.Where(x => x.Brand == DRUG).Select(x => x.ID).FirstOrDefault();
            var MDName = M_DrugM.Where(x => x.ID == ID).Select(x => x.GenericName).FirstOrDefault();
            var itmtran = WYNKContext.ItemBatchTrans.ToList();

            var SMID = StockMas.Where(x => x.DocumentNumber == GRN).Select(x => x.RandomUniqueID).FirstOrDefault();

            var IBID = itmtran.Where(x => x.SMID == SMID).Select(c => c.ItemBatchID).ToList();


            var DrugNameSearch = new Meterialview();
            DrugNameSearch.Meterials4 = new List<Meterials4>();


            DrugNameSearch.Meterials4 = (from stock in StockMas.Where(x => x.DocumentNumber == GRN)
                                         join stn in StokMasTaran
                                         on stock.RandomUniqueID equals stn.SMID
                                         join itmt in itmtran
                                         on stn.RandomUniqueID equals itmt.STID
                                         join ib in ItemBatchs/*.Where(x => x.ItemBatchBalnceQty != 0 && x.LockedQuantity != x.ItemBatchBalnceQty && x.ItemBatchID==IBIDS)*/
                                         on itmt.ItemBatchID equals ib.RandomUniqueID
                                         select new Meterials4
                                         {
                                             DrugName1 = DRUG,
                                             GenericName = DGROUp.Where(x => x.ID == MDName).Select(x => x.Description).FirstOrDefault(),
                                             UOM = M_DrugM.Where(x => x.ID == ID).Select(x => x.UOM).FirstOrDefault(),

                                         }).ToList();



            return DrugNameSearch;
        }
        public dynamic InsertQty(Meterialview Con, int TransactionTypeid)
        {

            var Drug = WYNKContext.DrugMaster.ToList();
            var UOMMASTER = CMPSContext.uommaster.ToList();
            var Stcktran = WYNKContext.StockTran.ToList();
            var Stockmas = WYNKContext.StockMaster.ToList();
            var Trans = CMPSContext.TransactionType.ToList();
            var TType = Trans.Where(x => x.TransactionID == Con.StockmasterModel.TransactionID).Select(x => x.Rec_Issue_type).FirstOrDefault();

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {

                try
                {
                    var StockMaster = new StockMaster();
                    //StockMaster.DocumentNumber = Con.StockmasterModel.DocumentNumber = generatenumber;
                    StockMaster.TransactionID = Con.StockmasterModel.TransactionID;
                    StockMaster.CMPID = Con.StockmasterModel.CMPID;
                    StockMaster.DocumentNumber = Con.StockmasterModel.DocumentNumber;
                    StockMaster.DocumentDate = DateTime.Now;
                    StockMaster.POID = Con.StockmasterModel.POID;
                    StockMaster.StoreID = Con.StockmasterModel.StoreID;
                    StockMaster.TransactionType = TType;
                    StockMaster.VendorID = Con.StockmasterModel.VendorID;
                    StockMaster.GrossProductValue = Con.StockmasterModel.GrossProductValue;
                    StockMaster.TotalDiscountValue = Con.StockmasterModel.TotalDiscountValue;
                    StockMaster.TotalTaxValue = Con.StockmasterModel.TotalTaxValue;
                    StockMaster.TotalCGSTTaxValue = Con.StockmasterModel.TotalCGSTTaxValue;
                    StockMaster.TotalSGSTTaxValue = Con.StockmasterModel.TotalSGSTTaxValue;
                    StockMaster.TotalPOValue = Con.StockmasterModel.GrossProductValue + Con.StockmasterModel.TotalTaxValue;
                    StockMaster.IsCancelled = Con.StockmasterModel.IsCancelled;
                    StockMaster.TermsConditions = Con.StockmasterModel.TermsConditions;
                    StockMaster.IsDeleted = Con.StockmasterModel.IsDeleted;
                    StockMaster.CreatedUTC = DateTime.UtcNow;
                    WYNKContext.StockMaster.AddRange(StockMaster);
                    WYNKContext.SaveChanges();

                    var STSMID = StockMaster.RandomUniqueID;

                    foreach (var Arrays in Con.PushArray.ToList())
                    {
                        var drug = Arrays.DrugName1;
                        var ID = Drug.Where(x => x.Brand == drug).Select(x => x.ID).FirstOrDefault();
                        var joi = Drug.Where(x => x.ID == ID).Select(x => x.UOM).FirstOrDefault();
                        var UOM = UOMMASTER.Where(x => x.Description == joi).Select(x => x.id).FirstOrDefault();
                        var CSMID = Stockmas.Where(x => x.DocumentNumber == Con.StockmasterModel.GRN).Select(x => x.RandomUniqueID).FirstOrDefault();
                        var CSTID = Stcktran.Where(x => x.SMID == CSMID && x.ItemID == ID).Select(x => x.STID).FirstOrDefault();

                        var StockTran = new StockTran();
                        StockTran.SMID = STSMID;
                        StockTran.ItemID = ID;
                        StockTran.ItemQty = Arrays.QtyIssued;
                        StockTran.UOMID = UOM;
                        StockTran.ItemRate = Arrays.Rate;
                        StockTran.ItemValue = Arrays.Rate * Arrays.QtyIssued;
                        StockTran.ContraSMID = CSMID;
                        StockTran.ContraSTID = Convert.ToString(CSTID);
                        StockTran.DiscountPercentage = Arrays.DiscountPercentage;
                        StockTran.DiscountAmount = Arrays.DescountAmopunt;
                        StockTran.GSTPercentage = Arrays.GSTPercentage;
                        StockTran.GSTTaxValue = Arrays.GSTTaxValue;
                        StockTran.CGSTPercentage = Arrays.CGSTPercentage;
                        StockTran.CGSTTaxValue = Arrays.CGSTTaxValue;
                        StockTran.SGSTPercentage = Arrays.SGSTPercentage;
                        StockTran.SGSTTaxValue = Arrays.SGSTTaxValue;
                        StockTran.IsDeleted = Con.StockTranModel.IsDeleted;
                        StockTran.CreatedUTC = DateTime.UtcNow;
                        StockTran.CreatedBy = Con.StockTranModel.CreatedBy;
                        WYNKContext.StockTran.AddRange(StockTran);
                        WYNKContext.SaveChanges();
                        var STID = StockTran.RandomUniqueID;

                        var Itembatch = new ItemBatch();
                        var IBID = WYNKContext.ItemBatch.Where(x => x.ItemID == ID && x.ItemBatchNumber == Arrays.BatchList1).Select(x => x.ItemBatchID).FirstOrDefault();
                        Itembatch = WYNKContext.ItemBatch.Where(x => x.ItemBatchID == IBID).FirstOrDefault();
                        Itembatch.ItemBatchQty = Arrays.RecivedQty1;
                        Itembatch.ItemBatchissueQty = Arrays.ConsumedQty2 + Arrays.QtyIssued;
                        Itembatch.ItemBatchBalnceQty = Arrays.BalanceQty1 - Arrays.QtyIssued;
                        Itembatch.CreatedUTC = DateTime.UtcNow;
                        Itembatch.CreatedBy = Con.ItemBatchModel.CreatedBy;
                        WYNKContext.Entry(Itembatch).State = EntityState.Modified;


                        var ItembatchId = Itembatch.RandomUniqueID;


                        var ItemBatchTran = new ItemBatchTrans();
                        ItemBatchTran.ItemBatchID = ItembatchId;
                        ItemBatchTran.TC = Con.StockmasterModel.TransactionID;
                        ItemBatchTran.SMID = STSMID;
                        ItemBatchTran.STID = STID;
                        ItemBatchTran.ItemID = ID;
                        ItemBatchTran.ItemBatchNumber = Arrays.BatchList1;
                        ItemBatchTran.ItemBatchTransactedQty = Arrays.QtyIssued;
                        ItemBatchTran.ItemBatchExpiry = Arrays.Expired;
                        ItemBatchTran.UOMID = UOM;
                        ItemBatchTran.ContraItemBatchID = ItembatchId;
                        ItemBatchTran.CreatedUTC = DateTime.UtcNow;
                        ItemBatchTran.UpdatedUTC = Con.ItemBatchTranModel.UpdatedUTC;
                        ItemBatchTran.CreatedBy = Con.ItemBatchTranModel.CreatedBy;
                        ItemBatchTran.UpdatedBy = Con.ItemBatchTranModel.UpdatedBy;
                        WYNKContext.ItemBatchTrans.AddRange(ItemBatchTran);
                        WYNKContext.SaveChanges();
                    }


                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();
                    try
                    {
                        if (WYNKContext.SaveChanges() >= 0)

                            return new
                            {

                                Success = true,
                                Message = CommonMessage.saved,

                            };
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }

                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }


            }



            return new
            {
                Success = false,
                Message = CommonMessage.Missing,
            };
        }


        public dynamic getallreturndetails(string FromDate, string Todate, int value, int cmpid, int Trid)
        {
            var fdate = Convert.ToDateTime(FromDate);
            var tdate = Convert.ToDateTime(Todate);

            var contran = CMPSContext.TransactionType.Where(x => x.TransactionID == Trid).Select(x => x.ContraTransactionid).FirstOrDefault();
            var Type = CMPSContext.TransactionType.Where(x => x.TransactionID == contran).Select(x => x.Rec_Issue_type).FirstOrDefault();

            var opticalstockmaster = WYNKContext.OpticalStockMaster.Where(x => x.TransactionType == Type).AsNoTracking().ToList();
            var optram = WYNKContext.OpticalStockTran.AsNoTracking().ToList();
            var opbalance = WYNKContext.OpticalBalance.AsNoTracking().ToList();
            var lenstran = WYNKContext.Lenstrans.Where(x => x.Brand == value).AsNoTracking().ToList();
            var lensmastrer = WYNKContext.Lensmaster.AsNoTracking().ToList();
            var vendorm = CMPSContext.VendorMaster.AsNoTracking().ToList();
            var utctime = CMPSContext.Setup.Where(x => x.CMPID == cmpid).Select(x => x.UTCTime).FirstOrDefault();
            TimeSpan ts = TimeSpan.Parse(utctime);
            var year = DateTime.UtcNow.Date.Year;
            var fy = WYNKContext.FinancialYear.Where(x => x.CMPID == cmpid && x.FYAccYear == year).Select(x => x.ID).FirstOrDefault();
            var materialreturn = new Meterialview();
            materialreturn.OpticalMaterial = (from lt in lenstran
                                              join ob in opbalance on lt.ID equals ob.LTID
                                              join ot in optram on ob.LTID equals ot.LMIDID
                                              join om in opticalstockmaster on ot.RandomUniqueID equals om.RandomUniqueID
                                              where ob.ClosingBalance > 0 && ob.FYID == fy && (Convert.ToDateTime(om.DocumentDate).Date >= fdate && Convert.ToDateTime(om.DocumentDate).Date <= tdate)
                                              select new OpticalMaterial
                                              {
                                                  RnNumber = om.DocumentNumber,
                                                  RnDate = om.DocumentDate + ts,
                                                  Tqty = ot.ItemQty,
                                                  Vendor = vendorm.Where(x => x.ID == om.VendorID).Select(x => x.Name).FirstOrDefault(),
                                              }).OrderBy(x => x.RnDate).ToList();
            return materialreturn.OpticalMaterial;

        }
        public dynamic getallreturndetailsbasedonrn(string Phase, int CMPID, int trid)
        {


            var contran = CMPSContext.TransactionType.Where(x => x.TransactionID == trid).Select(x => x.ContraTransactionid).FirstOrDefault();
            var Type = CMPSContext.TransactionType.Where(x => x.TransactionID == contran).Select(x => x.Rec_Issue_type).FirstOrDefault();

            var opticalstockmaster = WYNKContext.OpticalStockMaster.Where(x => x.TransactionType == Type).AsNoTracking().ToList();
            var optram = WYNKContext.OpticalStockTran.AsNoTracking().ToList();
            var opbalance = WYNKContext.OpticalBalance.AsNoTracking().ToList();
            var lenstran = WYNKContext.Lenstrans.AsNoTracking().ToList();
            var lensmastrer = WYNKContext.Lensmaster.AsNoTracking().ToList();
            var vendorm = CMPSContext.VendorMaster.AsNoTracking().ToList();
            var utctime = CMPSContext.Setup.Where(x => x.CMPID == CMPID).Select(x => x.UTCTime).FirstOrDefault();
            TimeSpan ts = TimeSpan.Parse(utctime);
            var year = DateTime.UtcNow.Date.Year;
            var fy = WYNKContext.FinancialYear.Where(x => x.CMPID == CMPID && x.FYAccYear == year).Select(x => x.ID).FirstOrDefault();
            var materialreturn = new Meterialview();
            materialreturn.OpticalMaterial = (from lm in lensmastrer
                                              join lt in lenstran on lm.RandomUniqueID equals lt.LMID
                                              join ob in opbalance on lt.ID equals ob.LTID
                                              join ot in optram on ob.LTID equals ot.LMIDID
                                              join om in opticalstockmaster on ot.RandomUniqueID equals om.RandomUniqueID
                                              where ob.ClosingBalance > 0 && ob.FYID == fy && om.DocumentNumber == Phase
                                              group om by new { om.DocumentNumber } into gg
                                              select new OpticalMaterial
                                              {
                                                  RnNumber = gg.FirstOrDefault().DocumentNumber,
                                                  RnDate = gg.FirstOrDefault().DocumentDate + ts,                                                 
                                                  Vendor = vendorm.Where(x => x.ID == gg.FirstOrDefault().VendorID).Select(x => x.Name).FirstOrDefault(),
                                              }).OrderBy(x => x.RnDate).ToList();
            return materialreturn.OpticalMaterial;
        }
        public dynamic Getperiodmaterialdetails(string Phase, int value, int cmpid, int trid)
        {
            var materialreturn = new Meterialview();
            var contran = CMPSContext.TransactionType.Where(x => x.TransactionID == trid).Select(x => x.ContraTransactionid).FirstOrDefault();
            var Type = CMPSContext.TransactionType.Where(x => x.TransactionID == contran).Select(x => x.Rec_Issue_type).FirstOrDefault();

            var opticalstockmaster = WYNKContext.OpticalStockMaster.Where(x => x.TransactionType == Type).AsNoTracking().ToList();
            var optram = WYNKContext.OpticalStockTran.AsNoTracking().ToList();
            var opbalance = WYNKContext.OpticalBalance.AsNoTracking().ToList();
            var lenstran = WYNKContext.Lenstrans.Where(x => x.Brand == value).AsNoTracking().ToList();
            var lensmastrer = WYNKContext.Lensmaster.AsNoTracking().ToList();
            var vendorm = CMPSContext.VendorMaster.AsNoTracking().ToList();
            var utctime = CMPSContext.Setup.Where(x => x.CMPID == cmpid).Select(x => x.UTCTime).FirstOrDefault();
            TimeSpan ts = TimeSpan.Parse(utctime);
            var year = DateTime.UtcNow.Date.Year;
            var fy = WYNKContext.FinancialYear.Where(x => x.CMPID == cmpid && x.FYAccYear == year).Select(x => x.ID).FirstOrDefault();
            if (Phase == "onemonth")
            {
                var fdate = DateTime.UtcNow.Date.AddMonths(-1);
                var tdate = DateTime.UtcNow.Date;
                materialreturn.OpticalMaterial = (from lt in lenstran
                                                  join ob in opbalance on lt.ID equals ob.LTID
                                                  join ot in optram on ob.LTID equals ot.LMIDID
                                                  join om in opticalstockmaster on ot.RandomUniqueID equals om.RandomUniqueID
                                                  where ob.ClosingBalance > 0 && ob.FYID == fy && (Convert.ToDateTime(om.DocumentDate).Date >= fdate && Convert.ToDateTime(om.DocumentDate).Date <= tdate)
                                                  select new OpticalMaterial
                                                  {
                                                      RnNumber = om.DocumentNumber,
                                                      RnDate = om.DocumentDate + ts,
                                                      Tqty = ot.ItemQty,
                                                      Vendor = vendorm.Where(x => x.ID == om.VendorID).Select(x => x.Name).FirstOrDefault(),
                                                  }).OrderBy(x => x.RnDate).ToList();


            }
            else if (Phase == "three")
            {
                var fdate = DateTime.UtcNow.Date.AddMonths(-3);
                var tdate = DateTime.UtcNow.Date;
                materialreturn.OpticalMaterial = (from lt in lenstran
                                                  join ob in opbalance on lt.ID equals ob.LTID
                                                  join ot in optram on ob.LTID equals ot.LMIDID
                                                  join om in opticalstockmaster on ot.RandomUniqueID equals om.RandomUniqueID
                                                  where ob.ClosingBalance > 0 && ob.FYID == fy && (Convert.ToDateTime(om.DocumentDate).Date >= fdate && Convert.ToDateTime(om.DocumentDate).Date <= tdate)
                                                  select new OpticalMaterial
                                                  {
                                                      RnNumber = om.DocumentNumber,
                                                      RnDate = om.DocumentDate + ts,
                                                      Tqty = ot.ItemQty,
                                                      Vendor = vendorm.Where(x => x.ID == om.VendorID).Select(x => x.Name).FirstOrDefault(),
                                                  }).OrderBy(x => x.RnDate).ToList();

            }
            else if (Phase == "six")
            {
                var fdate = DateTime.UtcNow.Date.AddMonths(-6);
                var tdate = DateTime.UtcNow.Date;
                materialreturn.OpticalMaterial = (from lt in lenstran
                                                  join ob in opbalance on lt.ID equals ob.LTID
                                                  join ot in optram on ob.LTID equals ot.LMIDID
                                                  join om in opticalstockmaster on ot.RandomUniqueID equals om.RandomUniqueID
                                                  where ob.ClosingBalance > 0 && ob.FYID == fy && (Convert.ToDateTime(om.DocumentDate).Date >= fdate && Convert.ToDateTime(om.DocumentDate).Date <= tdate)
                                                  select new OpticalMaterial
                                                  {
                                                      RnNumber = om.DocumentNumber,
                                                      RnDate = om.DocumentDate + ts,
                                                      Tqty = ot.ItemQty,
                                                      Vendor = vendorm.Where(x => x.ID == om.VendorID).Select(x => x.Name).FirstOrDefault(),
                                                  }).OrderBy(x => x.RnDate).ToList();

            }
            return materialreturn.OpticalMaterial;
        }


        public dynamic getmaterialreturndetailsbasedonGRN(string Phase)
        {
            var materialreturn = new Meterialview();
            var opticalstockmaster = WYNKContext.OpticalStockMaster.Where(x => x.DocumentNumber == Phase).AsNoTracking().ToList();
            var optram = WYNKContext.OpticalStockTran.AsNoTracking().ToList();
            var opbalance = WYNKContext.OpticalBalance.Where(x => x.ClosingBalance > 0).AsNoTracking().ToList();
            var lenstran = WYNKContext.Lenstrans.AsNoTracking().ToList();
            var brandmastrer = WYNKContext.Brand.AsNoTracking().ToList();
            var vendorm = CMPSContext.VendorMaster.AsNoTracking().ToList();
            var storemas = CMPSContext.Storemasters.AsNoTracking().ToList();
            var locationmas = CMPSContext.LocationMaster.AsNoTracking().ToList();

            materialreturn.Opticamreturndetails = (from os in opticalstockmaster
                                                   join ot in optram on os.RandomUniqueID equals ot.RandomUniqueID
                                                   join ob in opbalance on ot.LMIDID equals ob.LTID
                                                   join lt in lenstran on ob.LTID equals lt.ID
                                                   join lm in brandmastrer on lt.Brand equals lm.ID
                                                   select new Opticamreturndetails
                                                   {
                                                       ItemID = lt.ID,
                                                       STID = ot.STID,
                                                       UOM = lt.UOMID,
                                                       Returnqty = 0,
                                                       Item = lm.Description,
                                                       Brand = WYNKContext.Brand.Where(x => x.ID == lt.Brand).Select(x => x.Description).FirstOrDefault(),
                                                       type = lm.BrandType,
                                                       model = lt.Model,
                                                       color = lt.Colour,
                                                       ClosingQty = ob.ClosingBalance,
                                                       itemqty = Checkquantity(ot.ItemQty, ot.ContraQty),
                                                       itemrate = ot.ItemRate,
                                                       itemvalue = Checkquantity(ot.ItemQty, ot.ContraQty) * ot.ItemRate,
                                                       storename = storemas.Where(x => x.StoreID == os.StoreID).Select(x => x.Storename).FirstOrDefault(),
                                                       storekeeper = storemas.Where(x => x.StoreID == os.StoreID).Select(x => x.StoreKeeper).FirstOrDefault(),
                                                       storelocation = storemas.Where(x => x.StoreID == os.StoreID).Select(x => x.StoreLocation).FirstOrDefault(),
                                                       storeAddress = storemas.Where(x => x.StoreID == os.StoreID).Select(x => x.Address1).FirstOrDefault(),
                                                       vendorname = vendorm.Where(x => x.ID == os.VendorID).Select(x => x.Name).FirstOrDefault(),
                                                       vendoradd = vendorm.Where(x => x.ID == os.VendorID).Select(x => x.Address1).FirstOrDefault(),
                                                       vendoradd2 = vendorm.Where(x => x.ID == os.VendorID).Select(x => x.Address2).FirstOrDefault(),
                                                       vendorlocation = getlocation(vendorm.Where(x => x.ID == os.VendorID).Select(x => x.Location).FirstOrDefault()),
                                                   }).ToList();

            var vendorid = opticalstockmaster.Select(x => x.VendorID).FirstOrDefault();
            materialreturn.vendorID = opticalstockmaster.Select(x => x.VendorID).FirstOrDefault();
            materialreturn.Storeid = opticalstockmaster.Select(x => x.StoreID).FirstOrDefault();
            var locid = vendorm.Where(x => x.ID == vendorid).Select(x => x.Location).FirstOrDefault();
            var locd = locationmas.Where(x => x.ID == locid).Select(x => x.ParentID).FirstOrDefault();
            var cityd = locationmas.Where(x => x.ID == locd).Select(x => x.ParentID).FirstOrDefault();
            var stated = locationmas.Where(x => x.ID == cityd).Select(x => x.ParentID).FirstOrDefault();
            materialreturn.vendorlocation = locationmas.Where(x => x.ID == locid).Select(x => x.ParentDescription).FirstOrDefault();
            materialreturn.vendorcity = locationmas.Where(x => x.ID == locd).Select(x => x.ParentDescription).FirstOrDefault();
            materialreturn.vendorstate = locationmas.Where(x => x.ID == cityd).Select(x => x.ParentDescription).FirstOrDefault();
            materialreturn.vendorcountry = locationmas.Where(x => x.ID == stated).Select(x => x.ParentDescription).FirstOrDefault();
            materialreturn.vendorgst = vendorm.Where(x => x.ID == vendorid).Select(x => x.GSTNo).FirstOrDefault();
            var opid = opticalstockmaster.Select(x => x.OpticalOrderID).FirstOrDefault();
            materialreturn.opticalGrnnumber = WYNKContext.OpticalOrder.Where(x => x.RandomUniqueID == opid).Select(x => x.OrderNumber).FirstOrDefault();
            materialreturn.opticalGrndate = WYNKContext.OpticalOrder.Where(x => x.RandomUniqueID == opid).Select(x => x.OrderDate).FirstOrDefault();
            materialreturn.storename = materialreturn.Opticamreturndetails.Select(x => x.storename).FirstOrDefault();
            materialreturn.storekeeper = materialreturn.Opticamreturndetails.Select(x => x.storekeeper).FirstOrDefault();
            materialreturn.storelocation = materialreturn.Opticamreturndetails.Select(x => x.storelocation).FirstOrDefault();
            materialreturn.storeAddress = materialreturn.Opticamreturndetails.Select(x => x.storeAddress).FirstOrDefault();
            materialreturn.vendorname = materialreturn.Opticamreturndetails.Select(x => x.vendorname).FirstOrDefault();
            materialreturn.vendoradd = materialreturn.Opticamreturndetails.Select(x => x.vendoradd).FirstOrDefault();
            materialreturn.vendoradd2 = materialreturn.Opticamreturndetails.Select(x => x.vendoradd2).FirstOrDefault();
            return materialreturn;
        }

        public dynamic Checkquantity(decimal qty, decimal? conqty)
        {
            decimal? qqty = Convert.ToDecimal(0.00);
            if (conqty == null)
            {
                qqty = qty;
            }
            else
            {
                qqty = qty - conqty;
            }
            return qqty;
        }

        public string getlocation(int? Phase)
        {
            var locid = Phase;
            var locname = "";
            if (Phase != 0)
            {
                locname = CMPSContext.LocationMaster.Where(x => x.ID == Phase).Select(x => x.ParentDescription).FirstOrDefault();
            }
            else
            {
                locname = "Location not exits";
            }

            return locname;
        }

        public dynamic Checkfinancialyear(string Phase, int cmpid)
        {
            var materialreturn = new Meterialview();
            var dateformat = Convert.ToDateTime(Phase).Year;
            var fy = WYNKContext.FinancialYear.Where(x => x.CMPID == cmpid && x.IsActive == true && x.FYAccYear == dateformat).FirstOrDefault();
            if (fy != null)
            {
                materialreturn.Yearstatus = true;
            }
            else
            {
                materialreturn.Yearstatus = false;
            }
            return materialreturn.Yearstatus;
        }

        public dynamic Submitopticaldata(Meterialview OpticalMaterialretrunsavedata)
        {
            var Docnumber = OpticalMaterialretrunsavedata.NewDocnumber;
            var tranas = OpticalMaterialretrunsavedata.TransactiomnID;
            var orgdata = OpticalMaterialretrunsavedata.Opticamreturnsubmitdetails.ToList();
            var totalpovalue = orgdata.Sum(x => Convert.ToDecimal(x.itemvalue));
            ErrorLog oErrorLogs = new ErrorLog();
            var opticalmaster = new OpticalStockMaster();
            var opticaltrans = new OpticalStockTran();
            var Date = DateTime.Now;
            var CurrentMonth = Date.Month;
            var FinancialYearId = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date
            && Convert.ToDateTime(x.FYTo) >= Date && x.CMPID == OpticalMaterialretrunsavedata.CompanyID && x.IsActive == true).Select(x => x.ID).FirstOrDefault();

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    opticalmaster.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                    string RandomUniqueID = opticalmaster.RandomUniqueID;
                    opticalmaster.TransactionID = tranas;
                    opticalmaster.CMPID = OpticalMaterialretrunsavedata.CompanyID;
                    opticalmaster.DocumentNumber = Docnumber;
                    var Datee = DateTime.Now;
                    opticalmaster.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= Datee && Convert.ToDateTime(b.FYTo) >= Datee && x.CMPID == opticalmaster.CMPID && x.IsActive == true).Select(f => f.ID).FirstOrDefault()).Select(c => c.FYAccYear).FirstOrDefault());
                    opticalmaster.DocumentDate = DateTime.UtcNow;
                    var opid = WYNKContext.OpticalStockMaster.Where(x => x.DocumentNumber == OpticalMaterialretrunsavedata.GRN).Select(x => x.RandomUniqueID).FirstOrDefault();
                    var Deptid = WYNKContext.OpticalStockMaster.Where(x => x.DocumentNumber == OpticalMaterialretrunsavedata.GRN).Select(x => x.DepartmentID).FirstOrDefault();
                    opticalmaster.OpticalOrderID = opid;
                    opticalmaster.StoreID = OpticalMaterialretrunsavedata.storeiud;
                    opticalmaster.TransactionType = CMPSContext.TransactionType.Where(X => X.TransactionID == tranas).Select(x => x.Rec_Issue_type).FirstOrDefault();
                    opticalmaster.VendorID = OpticalMaterialretrunsavedata.vendorid;
                    opticalmaster.DepartmentID = Deptid;
                    opticalmaster.TotalPOValue = totalpovalue;
                    opticalmaster.TotalDiscountValue = Convert.ToDecimal(0.00);
                    opticalmaster.IsCancelled = false;
                    opticalmaster.TermsConditions = OpticalMaterialretrunsavedata.Remarks;
                    opticalmaster.IsDeleted = false;
                    opticalmaster.CreatedUTC = DateTime.UtcNow;
                    opticalmaster.CreatedBy = OpticalMaterialretrunsavedata.UserID;
                    WYNKContext.OpticalStockMaster.AddRange(opticalmaster);

                    foreach (var item in orgdata)
                    {
                        opticaltrans.RandomUniqueID = RandomUniqueID;
                        opticaltrans.LMIDID = item.ItemID;
                        opticaltrans.ItemQty = item.Returnqty;
                        opticaltrans.UOMID = item.UOM;
                        opticaltrans.ItemRate = Convert.ToDecimal(item.itemrate);
                        opticaltrans.ItemValue = Convert.ToDecimal(item.itemvalue);
                        opticaltrans.DiscountPercentage = Convert.ToDecimal(0.00);
                        opticaltrans.DiscountAmount = Convert.ToDecimal(0.00);
                        opticaltrans.POID = 5;
                        opticaltrans.IsDeleted = false;
                        opticaltrans.CreatedUTC = DateTime.UtcNow;
                        opticaltrans.CreatedBy = OpticalMaterialretrunsavedata.UserID;
                        WYNKContext.OpticalStockTran.AddRange(opticaltrans);
                        ErrorLog oErrorLogstran = new ErrorLog();
                        object namestr = opticaltrans;
                        oErrorLogstran.WriteErrorLogArray("OpticalStockTran", namestr);

                        var otran = WYNKContext.OpticalStockTran.Where(x => x.STID == Convert.ToInt32(item.STID)).FirstOrDefault();

                        if (otran.ContraQty != null)
                        {
                            otran.ContraQty = item.Returnqty + otran.ContraQty;
                        }
                        else
                        {
                            otran.ContraQty = item.Returnqty;
                        }
                        WYNKContext.OpticalStockTran.Update(otran);

                        var ob = WYNKContext.OpticalBalance.Where(x => x.StoreID == OpticalMaterialretrunsavedata.storeiud
                        && x.CmpID == OpticalMaterialretrunsavedata.CompanyID && x.FYID == FinancialYearId && x.LTID == item.ItemID).FirstOrDefault();

                        if (ob != null)
                        {
                            switch (CurrentMonth)
                            {
                                case 1:
                                    ob.ISS01 = ob.ISS01 + item.Returnqty;
                                    break;
                                case 2:
                                    ob.ISS02 = ob.ISS02 + item.Returnqty;
                                    break;
                                case 3:
                                    ob.ISS03 = ob.ISS03 + item.Returnqty;
                                    break;
                                case 4:
                                    ob.ISS04 = ob.ISS04 + item.Returnqty;
                                    break;
                                case 5:
                                    ob.ISS05 = ob.ISS05 + item.Returnqty;
                                    break;
                                case 6:
                                    ob.ISS06 = ob.ISS06 + item.Returnqty;
                                    break;
                                case 7:
                                    ob.ISS07 = ob.ISS07 + item.Returnqty;
                                    break;
                                case 8:
                                    ob.ISS08 = ob.ISS08 + item.Returnqty;
                                    break;
                                case 9:
                                    ob.ISS09 = ob.ISS09 + item.Returnqty;
                                    break;
                                case 10:
                                    ob.ISS10 = ob.ISS10 + item.Returnqty;
                                    break;
                                case 11:
                                    ob.ISS11 = ob.ISS11 + item.Returnqty;
                                    break;
                                case 12:
                                    ob.ISS12 = ob.ISS12 + item.Returnqty;
                                    break;
                            }
                            ob.ClosingBalance = ob.ClosingBalance - item.Returnqty;
                            ob.UpdatedBy = OpticalMaterialretrunsavedata.UserID;
                            ob.UpdatedUTC = DateTime.UtcNow;
                            WYNKContext.OpticalBalance.UpdateRange(ob);
                            object IB = ob;
                            oErrorLogs.WriteErrorLogArray("OpticalBalance", IB);
                        }
                    }

                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();
                    try
                    {
                        if (WYNKContext.SaveChanges() >= 0)

                            return new
                            {
                                Success = true,
                                Message = CommonMessage.saved,
                            };
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
            }
            return new
            {
                Success = false,
                Message = CommonMessage.Missing,
            };
        }


        public dynamic GetHistoryDetails(int Phase, int cmpid)
        {

            var materialreturn = new Meterialview();
            var opticalstockmaster = WYNKContext.OpticalStockMaster.Where(x => x.CMPID == cmpid && x.TransactionType == "I").AsNoTracking().ToList();
            var optram = WYNKContext.OpticalStockTran.Where(x => x.LMIDID == Phase && x.ContraQty == null).AsNoTracking().ToList();
            var lenstran = WYNKContext.Lenstrans.AsNoTracking().ToList();
            var brandmastrer = WYNKContext.Brand.AsNoTracking().ToList();
            var vendorm = CMPSContext.VendorMaster.AsNoTracking().ToList();
            var storemas = CMPSContext.Storemasters.AsNoTracking().ToList();           
            var utctime = CMPSContext.Setup.Where(x => x.CMPID == cmpid).Select(x => x.UTCTime).FirstOrDefault();
            TimeSpan ts = TimeSpan.Parse(utctime);

            materialreturn.OpticalMaterialHistoryDetails = (from ot in optram
                                                            join os in opticalstockmaster on ot.RandomUniqueID equals os.RandomUniqueID
                                                            join lt in lenstran on ot.LMIDID equals lt.ID
                                                            join br in brandmastrer on lt.Brand equals br.ID
                                                            select new OpticalMaterialHistoryDetails
                                                            {
                                                                Docnumber = os.DocumentNumber,
                                                                DocDate = os.DocumentDate + ts,
                                                                ItemQty = ot.ItemQty,
                                                                ItemValue = ot.ItemValue,
                                                             
                                                            }).OrderBy(x => x.Brand).ToList();
            return materialreturn.OpticalMaterialHistoryDetails;

        }











    }


}
