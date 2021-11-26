
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Data.Repository.Implementation
{
    class DrugStockSummaryRepository : RepositoryBase<DrugStockSummaryDataView>, IDrugStockSummaryRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public DrugStockSummaryRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic GetStockSummary(DrugStockSummaryDataView stocksummary, string From, string To, int CompanyID, string Time)
        {
            var FinancialYear = WYNKContext.FinancialYear.ToList();
            var OpticalBalance = WYNKContext.ItemBalance.ToList();
            var Brand = WYNKContext.Brand.ToList();
            var uommaster = CMPSContext.uommaster.ToList();
            var Storemasters = CMPSContext.Storemasters.ToList();
            var Company = CMPSContext.Company.ToList();
            var one = CMPSContext.OneLineMaster.ToList();
            var drugmas = WYNKContext.DrugMaster.ToList();
            TimeSpan ts = TimeSpan.Parse(Time);

            var Opticalstksummary = new DrugStockSummaryDataView();

            Opticalstksummary.Companycommu = new List<Companycommu>();
            Opticalstksummary.StoreArrays = new List<StoreArrays>();
            Opticalstksummary.BrandArrays = new List<BrandArrays>();
            List<Stocksummaryarrayd> Stocksummaryarrayd = new List<Stocksummaryarrayd>();
            Opticalstksummary.OpticalStocksummaryarray = new List<OpticalStocksummaryarray>();

            DateTime DT;
            var appdate = DateTime.TryParseExact(From.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT);
            {
                DT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var fdate = DT;

            DateTime DT1;

            var appdate1 = DateTime.TryParseExact(To.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT1);
            {
                DT1.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var tdate = DT1;

            string format = "MM-yyyy";
            DateTime SFdate = DateTime.ParseExact(fdate.ToString("MM-yyyy"), format, CultureInfo.InvariantCulture);
            DateTime STdate = DateTime.ParseExact(tdate.ToString("MM-yyyy"), format, CultureInfo.InvariantCulture);
            var FinancialYearId = WYNKContext.FinancialYear.Where(x => x.FYFrom <= fdate && x.FYTo >= tdate && x.CMPID == CompanyID && x.IsActive == true).Select(x => x.ID).FirstOrDefault();
            var Fmonth = WYNKContext.FinancialYear.Where(x => x.FYFrom <= fdate && x.FYTo >= tdate && x.CMPID == CompanyID && x.IsActive == true).Select(x => x.FYFrom).FirstOrDefault();
            DateTime STFdate = DateTime.ParseExact(Fmonth.ToString("MM-yyyy"), format, CultureInfo.InvariantCulture);


            var StoreArray = (from s in stocksummary.StoreArrays

                              select new
                              {
                                  ID = s.StoreID,
                                  Name = s.StoreName,

                              }).ToList();

            var BrandArray = (from b in stocksummary.BrandArrays

                              select new
                              {
                                  ID = b.BrandID,
                                  Name = b.BrandName,

                              }).ToList();




            var concatarray = StoreArray.Count() >= BrandArray.Count() ? StoreArray.ToList() : BrandArray.ToList();

            if (concatarray.Count() == StoreArray.Count())
            {                
                if (concatarray.Count() > 0)
                {
                    foreach (var ba in BrandArray.ToList())
                    {
                        foreach (var sa in StoreArray.ToList())
                        {

                            Stocksummaryarrayd.AddRange((from OB in OpticalBalance.Where(x => x.StoreID == Convert.ToInt32(sa.ID) && x.CmpID == CompanyID && x.FYear == FinancialYearId)
                                                        join DM in drugmas.Where(x => x.ID == Convert.ToInt32(ba.ID)) on OB.ItemID equals DM.ID
                                                        join ST in Storemasters.Where(x => x.CmpID == CompanyID) on OB.StoreID equals ST.StoreID
                                                        join cm in Company.Where(x => x.CmpID == CompanyID) on OB.CmpID equals cm.CmpID

                                                        select new Stocksummaryarrayd
                                                        {

                                                            CmpName = cm.CompanyName + "-" + cm.Address1,
                                                            Store = ST.Storename,
                                                            Brand = DM.Brand,
                                                            UOM = DM.UOM,
                                                            Rec01 = Convert.ToInt32(OB.Rec01),
                                                            Rec02 = Convert.ToInt32(OB.Rec02),
                                                            Rec03 = Convert.ToInt32(OB.Rec03),
                                                            Rec04 = Convert.ToInt32(OB.Rec04),
                                                            Rec05 = Convert.ToInt32(OB.Rec05),
                                                            Rec06 = Convert.ToInt32(OB.Rec06),
                                                            Rec07 = Convert.ToInt32(OB.Rec07),
                                                            Rec08 = Convert.ToInt32(OB.Rec08),
                                                            Rec09 = Convert.ToInt32(OB.Rec09),
                                                            Rec10 = Convert.ToInt32(OB.Rec10),
                                                            Rec11 = Convert.ToInt32(OB.Rec11),
                                                            Rec12 = Convert.ToInt32(OB.Rec12),
                                                            Iss01 = Convert.ToInt32(OB.Iss01),
                                                            Iss02 = Convert.ToInt32(OB.Iss02),
                                                            Iss03 = Convert.ToInt32(OB.Iss03),
                                                            Iss04 = Convert.ToInt32(OB.Iss04),
                                                            Iss05 = Convert.ToInt32(OB.Iss05),
                                                            Iss06 = Convert.ToInt32(OB.Iss06),
                                                            Iss07 = Convert.ToInt32(OB.Iss07),
                                                            Iss08 = Convert.ToInt32(OB.Iss08),
                                                            Iss09 = Convert.ToInt32(OB.Iss09),
                                                            Iss10 = Convert.ToInt32(OB.Iss10),
                                                            Iss11 = Convert.ToInt32(OB.Iss11),
                                                            Iss12 = Convert.ToInt32(OB.Iss12),
                                                            Openingstock = OB.OpeningBalance,
                                                            ID = OB.ID,
                                                            LTID = OB.ItemID,
                                                            StoreID = OB.StoreID,
                                                        }).ToList());




                        }
                    }

                }

                if (Stocksummaryarrayd.Count() > 0)
                {
                    foreach (var item in Stocksummaryarrayd.ToList())
                    {
                        var osl = new OpticalStocksummaryarray();

                        for (var dt = STFdate; dt <= STdate;)
                        {
                            var itm = Opticalstksummary.OpticalStocksummaryarray.Where(x => x.LTID == item.LTID && x.StoreID == item.StoreID && x.CmpID == item.CmpID).FirstOrDefault();
                            var tdatemonth = SFdate.AddMonths(-1);
                            if (itm == null)
                            {

                                int a = 0;
                                int b = dt.Month;
                                string c = a.ToString() + b.ToString();
                                //string newNumber = (b.ToString().Length == 1) ? c : dt.ToString();
                                string newNumber = (b.ToString().Length == 1) ? c : b.ToString();
                                string issue = "Iss" + newNumber;
                                string receipt = "Rec" + newNumber;
                                int Iss = Stocksummaryarrayd.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID).Select(x => (int)x.GetType().GetProperty(issue).GetValue(x)).FirstOrDefault();
                                int Rec = Stocksummaryarrayd.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID).Select(x => (int)x.GetType().GetProperty(receipt).GetValue(x)).FirstOrDefault();

                                osl.CmpName = item.CmpName;
                                osl.CmpID = item.CmpID;
                                osl.Store = item.Store;
                                osl.Brand = item.Brand;
                                osl.UOM = item.UOM;
                                osl.ID = item.ID;
                                osl.LTID = item.LTID;
                                osl.StoreID = item.StoreID;
                                osl.Receipt += Rec;
                                osl.Issue += Iss;
                                osl.Closingstock += item.Openingstock + (Rec - Iss);
                                osl.Openingstock += dt <= tdatemonth ? item.Openingstock + (Rec - Iss) : 0;
                                STFdate = STFdate.AddMonths(1);
                                dt = STFdate;
                            }

                        }
                        Opticalstksummary.OpticalStocksummaryarray.Add(osl);
                        STFdate = DateTime.ParseExact(Fmonth.ToString("MM-yyyy"), format, CultureInfo.InvariantCulture);
                    }
                }
            }
            else
            {

                if (concatarray.Count() > 0)
                {
                    foreach (var sa in StoreArray.ToList())
                    {
                        foreach (var ba in BrandArray.ToList())
                        {
                            Stocksummaryarrayd.AddRange((from OB in OpticalBalance.Where(x => x.StoreID == Convert.ToInt32(sa.ID) && x.CmpID == CompanyID && x.FYear == FinancialYearId)
                                                        join DM in drugmas.Where(x => x.ID == Convert.ToInt32(ba.ID)) on OB.ItemID equals DM.ID
                                                        join ST in Storemasters.Where(x => x.CmpID == CompanyID) on OB.StoreID equals ST.StoreID
                                                        join cm in Company.Where(x => x.CmpID == CompanyID) on OB.CmpID equals cm.CmpID

                                                        select new Stocksummaryarrayd
                                                        {

                                                            CmpName = cm.CompanyName + "-" + cm.Address1,
                                                            CmpID = cm.CmpID,
                                                            Store = ST.Storename,
                                                            Brand = DM.Brand,
                                                            UOM = DM.UOM,                                               
                                                            Rec01 = Convert.ToInt32(OB.Rec01),
                                                            Rec02 = Convert.ToInt32(OB.Rec02),
                                                            Rec03 = Convert.ToInt32(OB.Rec03),
                                                            Rec04 = Convert.ToInt32(OB.Rec04),
                                                            Rec05 = Convert.ToInt32(OB.Rec05),
                                                            Rec06 = Convert.ToInt32(OB.Rec06),
                                                            Rec07 = Convert.ToInt32(OB.Rec07),
                                                            Rec08 = Convert.ToInt32(OB.Rec08),
                                                            Rec09 = Convert.ToInt32(OB.Rec09),
                                                            Rec10 = Convert.ToInt32(OB.Rec10),
                                                            Rec11 = Convert.ToInt32(OB.Rec11),
                                                            Rec12 = Convert.ToInt32(OB.Rec12),
                                                            Iss01 = Convert.ToInt32(OB.Iss01),
                                                            Iss02 = Convert.ToInt32(OB.Iss02),
                                                            Iss03 = Convert.ToInt32(OB.Iss03),
                                                            Iss04 = Convert.ToInt32(OB.Iss04),
                                                            Iss05 = Convert.ToInt32(OB.Iss05),
                                                            Iss06 = Convert.ToInt32(OB.Iss06),
                                                            Iss07 = Convert.ToInt32(OB.Iss07),
                                                            Iss08 = Convert.ToInt32(OB.Iss08),
                                                            Iss09 = Convert.ToInt32(OB.Iss09),
                                                            Iss10 = Convert.ToInt32(OB.Iss10),
                                                            Iss11 = Convert.ToInt32(OB.Iss11),
                                                            Iss12 = Convert.ToInt32(OB.Iss12),
                                                            Openingstock = OB.OpeningBalance,
                                                            ID = OB.ID,
                                                            LTID = OB.ItemID,
                                                            StoreID = OB.StoreID,
                                                        }).ToList());
                        }
                    }

                }

                if (Stocksummaryarrayd.Count() > 0)
                {
                    foreach (var item in Stocksummaryarrayd.ToList())
                    {
                        var osl = new OpticalStocksummaryarray();

                        for (var dt = STFdate; dt <= STdate;)
                        {
                            var itm = Opticalstksummary.OpticalStocksummaryarray.Where(x => x.LTID == item.LTID && x.StoreID == item.StoreID && x.CmpID == item.CmpID).FirstOrDefault();
                            var tdatemonth = SFdate.AddMonths(-1);
                            if (itm == null)
                            {

                                int a = 0;
                                int b = dt.Month;
                                string c = a.ToString() + b.ToString();
                                string newNumber = (b.ToString().Length == 1) ? c : b.ToString();
                                string issue = "Iss" + newNumber;
                                string receipt = "Rec" + newNumber;
                                int Iss = Stocksummaryarrayd.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID).Select(x => (int)x.GetType().GetProperty(issue).GetValue(x)).FirstOrDefault();
                                int Rec = Stocksummaryarrayd.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID).Select(x => (int)x.GetType().GetProperty(receipt).GetValue(x)).FirstOrDefault();

                                osl.CmpName = item.CmpName;
                                osl.CmpID = item.CmpID;
                                osl.Store = item.Store;
                                osl.Brand = item.Brand;
                                osl.UOM = item.UOM;
                                osl.ID = item.ID;
                                osl.LTID = item.LTID;
                                osl.StoreID = item.StoreID;
                                osl.Receipt += Rec;
                                osl.Issue += Iss;
                                osl.Closingstock += item.Openingstock + (Rec - Iss);
                                osl.Openingstock += dt <= tdatemonth ? item.Openingstock + (Rec - Iss) : 0;
                                STFdate = STFdate.AddMonths(1);
                                dt = STFdate;
                            }

                        }
                        Opticalstksummary.OpticalStocksummaryarray.Add(osl);
                        STFdate = DateTime.ParseExact(Fmonth.ToString("MM-yyyy"), format, CultureInfo.InvariantCulture);
                    }
                }

            }

            Opticalstksummary.Companycommu = (from c in Company.Where(u => u.CmpID == CompanyID)

                                              select new Companycommu
                                              {
                                                  Companyname = c.CompanyName,
                                                  Address = c.Address1 + "" + c.Address2 + "" + c.Address3,
                                                  Phoneno = c.Phone1,
                                                  Web = c.Website,
                                                  Location = c.LocationName,
                                              }).ToList();


            return Opticalstksummary;
        }



        public dynamic GetStockLedger(DrugStockSummaryDataView stockledger, string From, string To, int CompanyID, string Time)
        {
            var FinancialYear = WYNKContext.FinancialYear.ToList();
            var ItemBalance = WYNKContext.ItemBalance.ToList();
            var drugmaster = WYNKContext.DrugMaster.ToList();
            var uommaster = CMPSContext.uommaster.ToList();
            var Storemasters = CMPSContext.Storemasters.ToList();
            var StockMaster = WYNKContext.StockMaster.ToList();
            var StockTran = WYNKContext.StockTran.ToList();
            var TransactionType = CMPSContext.TransactionType.ToList();
            var Company = CMPSContext.Company.ToList();
            var one = CMPSContext.OneLineMaster.ToList();
            TimeSpan ts = TimeSpan.Parse(Time);


            DateTime DT;
            var appdate = DateTime.TryParseExact(From.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT);
            {
                DT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var fdate = DT;

            DateTime DT1;
            var appdate1 = DateTime.TryParseExact(To.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT1);
            {
                DT1.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var tdate = DT1;

            string format = "MM-yyyy";
            DateTime SFdate = DateTime.ParseExact(fdate.ToString("MM-yyyy"), format, CultureInfo.InvariantCulture);
            DateTime STdate = DateTime.ParseExact(tdate.ToString("MM-yyyy"), format, CultureInfo.InvariantCulture);
            var FinancialYearId = WYNKContext.FinancialYear.Where(x => x.FYFrom <= fdate && x.FYTo >= tdate && x.CMPID == CompanyID && x.IsActive == true).Select(x => x.ID).FirstOrDefault();
            var Fmonth = WYNKContext.FinancialYear.Where(x => x.FYFrom <= fdate && x.FYTo >= tdate && x.CMPID == CompanyID && x.IsActive == true).Select(x => x.FYFrom).FirstOrDefault();
            DateTime STFdate = DateTime.ParseExact(Fmonth.ToString("MM-yyyy"), format, CultureInfo.InvariantCulture);


            var Opticalstkledger = new DrugStockSummaryDataView();
            Opticalstkledger.StoreArray = new List<StoreArray>();
            Opticalstkledger.BrandArray = new List<BrandArray>();
            Opticalstkledger.Opticalstockledgerd = new List<Opticalstockledgerd>();
            Opticalstkledger.OpticalstockledgerId = new List<OpticalstockledgerId>();
            List<Receiptd> Receiptd = new List<Receiptd>();
            List<Issued> Issued = new List<Issued>();
            Opticalstkledger.Companycommd = new List<Companycommd>();

            var StoreArray = (from s in stockledger.StoreArray

                              select new
                              {
                                  ID = s.StoreID,
                                  Name = s.StoreName,

                              }).ToList();

            var BrandArray = (from b in stockledger.BrandArray

                              select new
                              {
                                  ID = b.BrandID,
                                  Name = b.BrandName,

                              }).ToList();



            var concatarray = StoreArray.Count() >= BrandArray.Count() ? StoreArray.ToList() : BrandArray.ToList();


            if (concatarray.Count() == StoreArray.Count())
            {

                if (concatarray.Count() > 0)
                {

                    foreach (var ba in BrandArray.ToList())
                    {
                        foreach (var sa in StoreArray.ToList())
                        {


                            Receiptd.AddRange((from OB in ItemBalance.Where(x => x.StoreID == Convert.ToInt32(sa.ID) && x.FYear == FinancialYearId && x.CmpID == CompanyID)
                                               join DM in drugmaster.Where(x => x.ID == Convert.ToInt32(ba.ID)) on OB.ItemID equals DM.ID
                                               join ST in Storemasters.Where(x => x.CmpID == CompanyID) on OB.StoreID equals ST.StoreID
                                              join STR in StockTran on OB.ItemID equals STR.ItemID
                                              join SM in StockMaster.Where(x => x.CreatedUTC.Date >= fdate && x.CreatedUTC.Date <= tdate && x.TransactionType == "R" && x.CMPID == CompanyID && x.StoreID == Convert.ToInt32(sa.ID)) on STR.SMID equals SM.RandomUniqueID
                                              join TR in TransactionType on SM.TransactionID equals TR.TransactionID
                                              join cm in Company.Where(x => x.CmpID == CompanyID) on OB.CmpID equals cm.CmpID
                                              select new Receiptd
                                              {

                                                  CmpName = cm.CompanyName + "-" + cm.Address1,
                                                  CmpID = cm.CmpID,
                                                  DocumentNo = SM.DocumentNumber,
                                                  DocumentDate = SM.DocumentDate.Add(ts),
                                                  DocumentType = TR.Description,                                                  
                                                  Store = ST.Storename,
                                                  StoreID = ST.StoreID,
                                                  Brand = DM.Brand,
                                                  BrandID = DM.ID,
                                                  UOM = DM.UOM,
                                                  Recept = STR.ItemQty,
                                                  Issue = 0.0M,
                                                  Rec01 = Convert.ToInt32(OB.Rec01),
                                                  Rec02 = Convert.ToInt32(OB.Rec02),
                                                  Rec03 = Convert.ToInt32(OB.Rec03),
                                                  Rec04 = Convert.ToInt32(OB.Rec04),
                                                  Rec05 = Convert.ToInt32(OB.Rec05),
                                                  Rec06 = Convert.ToInt32(OB.Rec06),
                                                  Rec07 = Convert.ToInt32(OB.Rec07),
                                                  Rec08 = Convert.ToInt32(OB.Rec08),
                                                  Rec09 = Convert.ToInt32(OB.Rec09),
                                                  Rec10 = Convert.ToInt32(OB.Rec10),
                                                  Rec11 = Convert.ToInt32(OB.Rec11),
                                                  Rec12 = Convert.ToInt32(OB.Rec12),
                                                  Iss01 = Convert.ToInt32(OB.Iss01),
                                                  Iss02 = Convert.ToInt32(OB.Iss02),
                                                  Iss03 = Convert.ToInt32(OB.Iss03),
                                                  Iss04 = Convert.ToInt32(OB.Iss04),
                                                  Iss05 = Convert.ToInt32(OB.Iss05),
                                                  Iss06 = Convert.ToInt32(OB.Iss06),
                                                  Iss07 = Convert.ToInt32(OB.Iss07),
                                                  Iss08 = Convert.ToInt32(OB.Iss08),
                                                  Iss09 = Convert.ToInt32(OB.Iss09),
                                                  Iss10 = Convert.ToInt32(OB.Iss10),
                                                  Iss11 = Convert.ToInt32(OB.Iss11),
                                                  Iss12 = Convert.ToInt32(OB.Iss12),
                                                  ID = OB.ID,
                                                  LTID = OB.ItemID,

                                              }).OrderByDescending(x => x.ID).ToList());
                        }
                    }

                    if (Receiptd.Count() > 0)
                    {
                        foreach (var item in Receiptd.ToList())
                        {
                            var osl = new Opticalstockledgerd();

                            for (var dt = STFdate; dt <= STdate;)
                            {
                                var ItemBalances = Opticalstkledger.Opticalstockledgerd.Where(x => x.LTID == item.LTID && x.StoreID == item.StoreID && x.CmpID == item.CmpID && x.DocumentNo == item.DocumentNo).FirstOrDefault();
                                var tdatemonth = SFdate.AddMonths(-1);
                                if (ItemBalances == null)
                                {

                                    int a = 0;
                                    int b = dt.Month;
                                    string c = a.ToString() + b.ToString();
                                    string newNumber = (b.ToString().Length == 1) ? c : b.ToString();
                                    string issue = "Iss" + newNumber;
                                    string receipt = "Rec" + newNumber;
                                    int Iss = Receiptd.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID && w.DocumentNo == item.DocumentNo).Select(x => (int)x.GetType().GetProperty(issue).GetValue(x)).FirstOrDefault();
                                    int Rec = Receiptd.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID && w.DocumentNo == item.DocumentNo).Select(x => (int)x.GetType().GetProperty(receipt).GetValue(x)).FirstOrDefault();
                                    osl.CmpName = item.CmpName;
                                    osl.CmpID = item.CmpID;
                                    osl.DocumentDate = item.DocumentDate;
                                    osl.DocumentNo = item.DocumentNo;
                                    osl.Type = item.Type;
                                    osl.Store = item.Store;
                                    osl.Brand = item.Brand;
                                    osl.UOM = item.UOM;
                                    osl.ID = item.ID;
                                    osl.LTID = item.LTID;
                                    osl.StoreID = item.StoreID;
                                    osl.Receipt = item.Recept;
                                    osl.Closingstock += item.Openingstock + (Rec - Iss);
                                    osl.Openingstock += dt <= tdatemonth ? item.Openingstock + (Rec - Iss) : 0;
                                    STFdate = STFdate.AddMonths(1);
                                    dt = STFdate;
                                }
                            }

                            Opticalstkledger.Opticalstockledgerd.Add(osl);
                            STFdate = DateTime.ParseExact(Fmonth.ToString("MM-yyyy"), format, CultureInfo.InvariantCulture);
                        }
                    }

                    foreach (var ba in BrandArray.ToList())
                    {
                        foreach (var sa in StoreArray.ToList())
                        {

                            Issued.AddRange((from OB in ItemBalance.Where(x => x.StoreID == Convert.ToInt32(sa.ID) && x.FYear == FinancialYearId && x.CmpID == CompanyID)
                                             join DM in drugmaster.Where(x => x.ID == Convert.ToInt32(ba.ID)) on OB.ItemID equals DM.ID
                                             join ST in Storemasters.Where(x => x.CmpID == CompanyID) on OB.StoreID equals ST.StoreID
                                            join STR in StockTran on OB.ItemID equals STR.ItemID
                                            join SM in StockMaster.Where(x => x.CreatedUTC.Date >= fdate && x.CreatedUTC.Date <= tdate && x.TransactionType == "I" && x.CMPID == CompanyID && x.StoreID == Convert.ToInt32(sa.ID)) on STR.SMID equals SM.RandomUniqueID
                                            join TR in TransactionType on SM.TransactionID equals TR.TransactionID
                                            join cm in Company.Where(x => x.CmpID == CompanyID) on OB.CmpID equals cm.CmpID
                                            select new Issued
                                            {

                                                CmpName = cm.CompanyName + "-" + cm.Address1,
                                                CmpID = cm.CmpID,
                                                DocumentNo = SM.DocumentNumber,
                                                DocumentDate = SM.DocumentDate.Add(ts),
                                                DocumentType = TR.Description,
                                                Store = ST.Storename,
                                                StoreID = ST.StoreID,
                                                Brand = DM.Brand,
                                                BrandID = DM.ID,
                                                UOM = DM.UOM,
                                                Receipt = 0.0M,
                                                Isue = STR.ItemQty,
                                                Rec01 = Convert.ToInt32(OB.Rec01),
                                                Rec02 = Convert.ToInt32(OB.Rec02),
                                                Rec03 = Convert.ToInt32(OB.Rec03),
                                                Rec04 = Convert.ToInt32(OB.Rec04),
                                                Rec05 = Convert.ToInt32(OB.Rec05),
                                                Rec06 = Convert.ToInt32(OB.Rec06),
                                                Rec07 = Convert.ToInt32(OB.Rec07),
                                                Rec08 = Convert.ToInt32(OB.Rec08),
                                                Rec09 = Convert.ToInt32(OB.Rec09),
                                                Rec10 = Convert.ToInt32(OB.Rec10),
                                                Rec11 = Convert.ToInt32(OB.Rec11),
                                                Rec12 = Convert.ToInt32(OB.Rec12),
                                                Iss01 = Convert.ToInt32(OB.Iss01),
                                                Iss02 = Convert.ToInt32(OB.Iss02),
                                                Iss03 = Convert.ToInt32(OB.Iss03),
                                                Iss04 = Convert.ToInt32(OB.Iss04),
                                                Iss05 = Convert.ToInt32(OB.Iss05),
                                                Iss06 = Convert.ToInt32(OB.Iss06),
                                                Iss07 = Convert.ToInt32(OB.Iss07),
                                                Iss08 = Convert.ToInt32(OB.Iss08),
                                                Iss09 = Convert.ToInt32(OB.Iss09),
                                                Iss10 = Convert.ToInt32(OB.Iss10),
                                                Iss11 = Convert.ToInt32(OB.Iss11),
                                                Iss12 = Convert.ToInt32(OB.Iss12),
                                                ID = OB.ID,
                                                LTID = OB.ItemID,
                                            }).OrderByDescending(x => x.ID).ToList());
                        }
                    }

                    if (Issued.Count() > 0)
                    {
                        foreach (var item in Issued.ToList())
                        {
                            var osl = new OpticalstockledgerId();

                            for (var dt = STFdate; dt <= STdate;)
                            {
                                var ItemBalances = Opticalstkledger.OpticalstockledgerId.Where(x => x.LTID == item.LTID && x.StoreID == item.StoreID && x.CmpID == item.CmpID && x.DocumentNo == item.DocumentNo).FirstOrDefault();
                                var tdatemonth = SFdate.AddMonths(-1);
                                if (ItemBalances == null)
                                {

                                    int a = 0;
                                    int b = dt.Month;
                                    string c = a.ToString() + b.ToString();
                                    string newNumber = (b.ToString().Length == 1) ? c : dt.ToString();
                                    string issue = "Iss" + newNumber;
                                    string receipt = "Rec" + newNumber;
                                    int Iss = Issued.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID && w.DocumentNo == item.DocumentNo).Select(x => (int)x.GetType().GetProperty(issue).GetValue(x)).FirstOrDefault();
                                    int Rec = Issued.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID && w.DocumentNo == item.DocumentNo).Select(x => (int)x.GetType().GetProperty(receipt).GetValue(x)).FirstOrDefault();
                                    osl.CmpName = item.CmpName;
                                    osl.CmpID = item.CmpID;
                                    osl.DocumentDate = item.DocumentDate;
                                    osl.DocumentNo = item.DocumentNo;
                                    osl.Type = item.Type;
                                    osl.Store = item.Store;
                                    osl.Brand = item.Brand;
                                    osl.UOM = item.UOM;
                                    osl.ID = item.ID;
                                    osl.LTID = item.LTID;
                                    osl.StoreID = item.StoreID;
                                    osl.Issue = item.Isue;
                                    osl.Closingstock += item.Openingstock + (Rec - Iss);
                                    osl.Openingstock += dt <= tdatemonth ? item.Openingstock + (Rec - Iss) : 0;

                                    STFdate = STFdate.AddMonths(1);
                                    dt = STFdate;
                                }
                            }
                            Opticalstkledger.OpticalstockledgerId.Add(osl);
                            STFdate = DateTime.ParseExact(Fmonth.ToString("MM-yyyy"), format, CultureInfo.InvariantCulture);
                        }
                    }
                }
            }
            else
            {

                if (concatarray.Count() > 0)
                {

                    foreach (var sa in StoreArray.ToList())
                    {
                        foreach (var ba in BrandArray.ToList())
                        {


                            Issued.AddRange((from OB in ItemBalance.Where(x => x.StoreID == Convert.ToInt32(sa.ID) && x.FYear == FinancialYearId && x.CmpID == CompanyID)
                                             join DM in drugmaster.Where(x => x.ID == Convert.ToInt32(ba.ID)) on OB.ItemID equals DM.ID
                                             join ST in Storemasters.Where(x => x.CmpID == CompanyID) on OB.StoreID equals ST.StoreID
                                            join STR in StockTran on OB.ItemID equals STR.ItemID
                                            join SM in StockMaster.Where(x => x.CreatedUTC.Date >= fdate && x.CreatedUTC.Date <= tdate && x.TransactionType == "I" && x.CMPID == CompanyID && x.StoreID == Convert.ToInt32(sa.ID)) on STR.SMID equals SM.RandomUniqueID
                                            join TR in TransactionType on SM.TransactionID equals TR.TransactionID
                                            join cm in Company.Where(x => x.CmpID == CompanyID) on OB.CmpID equals cm.CmpID
                                            select new Issued
                                            {

                                                CmpName = cm.CompanyName + "-" + cm.Address1,
                                                CmpID = cm.CmpID,
                                                DocumentNo = SM.DocumentNumber,
                                                DocumentDate = SM.DocumentDate.Add(ts),
                                                DocumentType = TR.Description,                                               
                                                Store = ST.Storename,
                                                StoreID = ST.StoreID,
                                                Brand = DM.Brand,
                                                BrandID = DM.ID,
                                                UOM = DM.UOM,
                                                Receipt = 0.0M,
                                                Isue = STR.ItemQty,
                                                Rec01 = Convert.ToInt32(OB.Rec01),
                                                Rec02 = Convert.ToInt32(OB.Rec02),
                                                Rec03 = Convert.ToInt32(OB.Rec03),
                                                Rec04 = Convert.ToInt32(OB.Rec04),
                                                Rec05 = Convert.ToInt32(OB.Rec05),
                                                Rec06 = Convert.ToInt32(OB.Rec06),
                                                Rec07 = Convert.ToInt32(OB.Rec07),
                                                Rec08 = Convert.ToInt32(OB.Rec08),
                                                Rec09 = Convert.ToInt32(OB.Rec09),
                                                Rec10 = Convert.ToInt32(OB.Rec10),
                                                Rec11 = Convert.ToInt32(OB.Rec11),
                                                Rec12 = Convert.ToInt32(OB.Rec12),
                                                Iss01 = Convert.ToInt32(OB.Iss01),
                                                Iss02 = Convert.ToInt32(OB.Iss02),
                                                Iss03 = Convert.ToInt32(OB.Iss03),
                                                Iss04 = Convert.ToInt32(OB.Iss04),
                                                Iss05 = Convert.ToInt32(OB.Iss05),
                                                Iss06 = Convert.ToInt32(OB.Iss06),
                                                Iss07 = Convert.ToInt32(OB.Iss07),
                                                Iss08 = Convert.ToInt32(OB.Iss08),
                                                Iss09 = Convert.ToInt32(OB.Iss09),
                                                Iss10 = Convert.ToInt32(OB.Iss10),
                                                Iss11 = Convert.ToInt32(OB.Iss11),
                                                Iss12 = Convert.ToInt32(OB.Iss12),
                                                ID = OB.ID,
                                                LTID = OB.ItemID,
                                            }).OrderByDescending(x => x.ID).ToList());
                        }
                    }

                    if (Issued.Count() > 0)
                    {
                        foreach (var item in Issued.ToList())
                        {
                            var osl = new OpticalstockledgerId();

                            for (var dt = STFdate; dt <= STdate;)
                            {
                                var ItemBalances = Opticalstkledger.OpticalstockledgerId.Where(x => x.LTID == item.LTID && x.StoreID == item.StoreID && x.CmpID == item.CmpID && x.DocumentNo == item.DocumentNo).FirstOrDefault();
                                var tdatemonth = SFdate.AddMonths(-1);
                                if (ItemBalances == null)
                                {

                                    int a = 0;
                                    int b = dt.Month;
                                    string c = a.ToString() + b.ToString();
                                    string newNumber = (b.ToString().Length == 1) ? c : dt.ToString();
                                    string issue = "Iss" + newNumber;
                                    string receipt = "Rec" + newNumber;
                                    int Iss = Issued.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID && w.DocumentNo == item.DocumentNo).Select(x => (int)x.GetType().GetProperty(issue).GetValue(x)).FirstOrDefault();
                                    int Rec = Issued.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID && w.DocumentNo == item.DocumentNo).Select(x => (int)x.GetType().GetProperty(receipt).GetValue(x)).FirstOrDefault();
                                    osl.CmpName = item.CmpName;
                                    osl.CmpID = item.CmpID;
                                    osl.DocumentDate = item.DocumentDate;
                                    osl.DocumentNo = item.DocumentNo;
                                    osl.Type = item.Type;
                                    osl.Store = item.Store;
                                    osl.Brand = item.Brand;
                                    osl.UOM = item.UOM;                                  
                                    osl.ID = item.ID;
                                    osl.LTID = item.LTID;
                                    osl.StoreID = item.StoreID;
                                    osl.Issue = item.Isue;
                                    osl.Closingstock += item.Openingstock + (Rec - Iss);
                                    osl.Openingstock += dt <= tdatemonth ? item.Openingstock + (Rec - Iss) : 0;

                                    STFdate = STFdate.AddMonths(1);
                                    dt = STFdate;
                                }
                            }
                            Opticalstkledger.OpticalstockledgerId.Add(osl);
                            STFdate = DateTime.ParseExact(Fmonth.ToString("MM-yyyy"), format, CultureInfo.InvariantCulture);
                        }
                    }

                    foreach (var sa in StoreArray.ToList())
                    {
                        foreach (var ba in BrandArray.ToList())
                        {


                            Receiptd.AddRange((from OB in ItemBalance.Where(x => x.StoreID == Convert.ToInt32(sa.ID) && x.FYear == FinancialYearId && x.CmpID == CompanyID)
                                               join DM in drugmaster.Where(x => x.ID == Convert.ToInt32(ba.ID)) on OB.ItemID equals DM.ID
                                               join ST in Storemasters.Where(x => x.CmpID == CompanyID) on OB.StoreID equals ST.StoreID
                                              join STR in StockTran on OB.ItemID equals STR.ItemID
                                              join SM in StockMaster.Where(x => x.CreatedUTC.Date >= fdate && x.CreatedUTC.Date <= tdate && x.TransactionType == "R" && x.CMPID == CompanyID && x.StoreID == Convert.ToInt32(sa.ID)) on STR.SMID equals SM.RandomUniqueID
                                              join TR in TransactionType on SM.TransactionID equals TR.TransactionID
                                              join cm in Company.Where(x => x.CmpID == CompanyID) on OB.CmpID equals cm.CmpID
                                              select new Receiptd
                                              {

                                                  CmpName = cm.CompanyName + "-" + cm.Address1,
                                                  CmpID = cm.CmpID,
                                                  DocumentNo = SM.DocumentNumber,
                                                  DocumentDate = SM.DocumentDate.Add(ts),
                                                  DocumentType = TR.Description,
                                                  Store = ST.Storename,
                                                  StoreID = ST.StoreID,
                                                  Brand = DM.Brand,
                                                  BrandID = DM.ID,
                                                  UOM = DM.UOM,
                                                  Recept = STR.ItemQty,
                                                  Issue = 0.0M,
                                                  Rec01 = Convert.ToInt32(OB.Rec01),
                                                  Rec02 = Convert.ToInt32(OB.Rec02),
                                                  Rec03 = Convert.ToInt32(OB.Rec03),
                                                  Rec04 = Convert.ToInt32(OB.Rec04),
                                                  Rec05 = Convert.ToInt32(OB.Rec05),
                                                  Rec06 = Convert.ToInt32(OB.Rec06),
                                                  Rec07 = Convert.ToInt32(OB.Rec07),
                                                  Rec08 = Convert.ToInt32(OB.Rec08),
                                                  Rec09 = Convert.ToInt32(OB.Rec09),
                                                  Rec10 = Convert.ToInt32(OB.Rec10),
                                                  Rec11 = Convert.ToInt32(OB.Rec11),
                                                  Rec12 = Convert.ToInt32(OB.Rec12),
                                                  Iss01 = Convert.ToInt32(OB.Iss01),
                                                  Iss02 = Convert.ToInt32(OB.Iss02),
                                                  Iss03 = Convert.ToInt32(OB.Iss03),
                                                  Iss04 = Convert.ToInt32(OB.Iss04),
                                                  Iss05 = Convert.ToInt32(OB.Iss05),
                                                  Iss06 = Convert.ToInt32(OB.Iss06),
                                                  Iss07 = Convert.ToInt32(OB.Iss07),
                                                  Iss08 = Convert.ToInt32(OB.Iss08),
                                                  Iss09 = Convert.ToInt32(OB.Iss09),
                                                  Iss10 = Convert.ToInt32(OB.Iss10),
                                                  Iss11 = Convert.ToInt32(OB.Iss11),
                                                  Iss12 = Convert.ToInt32(OB.Iss12),
                                                  ID = OB.ID,
                                                  LTID = OB.ItemID,
                                              }).OrderByDescending(x => x.ID).ToList());

                        }

                    }

                    if (Receiptd.Count() > 0)
                    {
                        foreach (var item in Receiptd.ToList())
                        {
                            var osl = new Opticalstockledgerd();

                            for (var dt = STFdate; dt <= STdate;)
                            {
                                var ItemBalances = Opticalstkledger.Opticalstockledgerd.Where(x => x.LTID == item.LTID && x.StoreID == item.StoreID && x.CmpID == item.CmpID && x.DocumentNo == item.DocumentNo).FirstOrDefault();
                                var tdatemonth = SFdate.AddMonths(-1);
                                if (ItemBalances == null)
                                {

                                    int a = 0;
                                    int b = dt.Month;
                                    string c = a.ToString() + b.ToString();
                                    string newNumber = (b.ToString().Length == 1) ? c : dt.ToString();
                                    string issue = "Iss" + newNumber;
                                    string receipt = "Rec" + newNumber;
                                    int Iss = Receiptd.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID && w.DocumentNo == item.DocumentNo).Select(x => (int)x.GetType().GetProperty(issue).GetValue(x)).FirstOrDefault();
                                    int Rec = Receiptd.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID && w.DocumentNo == item.DocumentNo).Select(x => (int)x.GetType().GetProperty(receipt).GetValue(x)).FirstOrDefault();
                                    osl.CmpName = item.CmpName;
                                    osl.CmpID = item.CmpID;
                                    osl.DocumentDate = item.DocumentDate;
                                    osl.DocumentNo = item.DocumentNo;
                                    osl.Store = item.Store;
                                    osl.Brand = item.Brand;
                                    osl.UOM = item.UOM;                                    
                                    osl.ID = item.ID;
                                    osl.LTID = item.LTID;
                                    osl.StoreID = item.StoreID;
                                    osl.Receipt = item.Recept;
                                    osl.Closingstock += item.Openingstock + (Rec - Iss);
                                    osl.Openingstock += dt <= tdatemonth ? item.Openingstock + (Rec - Iss) : 0;
                                    STFdate = STFdate.AddMonths(1);
                                    dt = STFdate;
                                }
                            }

                            Opticalstkledger.Opticalstockledgerd.Add(osl);
                            STFdate = DateTime.ParseExact(Fmonth.ToString("MM-yyyy"), format, CultureInfo.InvariantCulture);
                        }
                    }

                }

            }

            Opticalstkledger.Companycommd = (from c in Company.Where(u => u.CmpID == CompanyID)

                                            select new Companycommd
                                            {
                                                Companyname = c.CompanyName,
                                                Address = c.Address1 + "" + c.Address2 + "" + c.Address3,
                                                Phoneno = c.Phone1,
                                                Web = c.Website,
                                                Location = c.LocationName,
                                            }).ToList();


            return Opticalstkledger;
        }



    }
}
