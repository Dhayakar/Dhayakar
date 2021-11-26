using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class OpticalStockLedgerRepository : RepositoryBase<OpticalStockLedgerDataView>, IOpticalStockLedgerRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public OpticalStockLedgerRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic GetStockLedger(OpticalStockLedgerDataView stockledger, string From, string To, int CompanyID, string Time)
        {
            var FinancialYear = WYNKContext.FinancialYear.ToList();
            var OpticalBalance = WYNKContext.OpticalBalance.ToList();
            var Lensmaster = WYNKContext.Lensmaster.ToList();
            var Lenstrans = WYNKContext.Lenstrans.ToList();
            var Brand = WYNKContext.Brand.ToList();
            var uommaster = CMPSContext.uommaster.ToList();
            var Storemasters = CMPSContext.Storemasters.ToList();
            var OpticalStockMaster = WYNKContext.OpticalStockMaster.ToList();
            var OpticalStockTran = WYNKContext.OpticalStockTran.ToList();
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


            var Opticalstkledger = new OpticalStockLedgerDataView();
            Opticalstkledger.StoreArray = new List<StoreArray>();
            Opticalstkledger.BrandArray = new List<BrandArray>();
            Opticalstkledger.Opticalstockledger = new List<Opticalstockledger>();
            Opticalstkledger.OpticalstockledgerI = new List<OpticalstockledgerI>();
            List<Receipt> Receipt = new List<Receipt>();
            List<Issue> Issue = new List<Issue>();
            Opticalstkledger.Companycomm = new List<Companycomm>();

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


                            Receipt.AddRange((from OB in OpticalBalance.Where(x => x.StoreID == Convert.ToInt32(sa.ID) && x.FYID == FinancialYearId && x.CmpID == CompanyID)
                                              join LT in Lenstrans on OB.LTID equals LT.ID
                                              join BR in Brand.Where(x => x.ID == Convert.ToInt32(ba.ID)) on LT.Brand equals BR.ID
                                              join UM in uommaster on LT.UOMID equals UM.id
                                              join LM in Lensmaster on LT.LMID equals LM.RandomUniqueID
                                              join ST in Storemasters.Where(x => x.CmpID == CompanyID) on OB.StoreID equals ST.StoreID
                                              join STR in OpticalStockTran on OB.LTID equals STR.LMIDID
                                              join SM in OpticalStockMaster.Where(x => x.CreatedUTC.Date >= fdate && x.CreatedUTC.Date <= tdate && x.TransactionType == "R" && x.CMPID == CompanyID && x.StoreID == Convert.ToInt32(sa.ID)) on STR.RandomUniqueID equals SM.RandomUniqueID
                                              join TR in TransactionType on SM.TransactionID equals TR.TransactionID
                                              join cm in Company.Where(x => x.CmpID == CompanyID) on OB.CmpID equals cm.CmpID
                                              select new Receipt
                                              {

                                                  CmpName = cm.CompanyName + "-" + cm.Address1,
                                                  CmpID = cm.CmpID,
                                                  DocumentNo = SM.DocumentNumber,
                                                  DocumentDate = SM.DocumentDate != null ? SM.DocumentDate.Value.Add(ts) : (DateTime?)null,
                                                  DocumentType = TR.Description,
                                                  Sphh = LT.Sph != null ? "Sph :" + " " + LT.Sph : null,
                                                  Cyll = LT.Cyl != null ? "Cyl :" + " " + LT.Cyl : null,
                                                  Axiss = LT.Axis != null ? "Axis :" + " " + LT.Axis : null,
                                                  Addd = LT.Add != null ? "Add :" + " " + LT.Add : null,
                                                  Fshpaee = one.Where(f => f.OLMID == LT.FrameShapeID).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Shape :" + " " + one.Where(f => f.OLMID == LT.FrameShapeID).Select(g => g.ParentDescription).FirstOrDefault() : null,
                                                  Ftypee = one.Where(f => f.OLMID == LT.FrameTypeID).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Type :" + " " + one.Where(f => f.OLMID == LT.FrameTypeID).Select(g => g.ParentDescription).FirstOrDefault() : null,
                                                  Fstylee = one.Where(f => f.OLMID == LT.FrameStyleID).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Style :" + " " + one.Where(f => f.OLMID == LT.FrameStyleID).Select(g => g.ParentDescription).FirstOrDefault() : null,
                                                  Fwidthh = one.Where(f => f.OLMID == LT.FrameWidthID).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Width :" + " " + one.Where(f => f.OLMID == LT.FrameWidthID).Select(g => g.ParentDescription).FirstOrDefault() : null,
                                                  Type = LM.LensType,
                                                  TypeID = LM.ID,
                                                  Store = ST.Storename,
                                                  StoreID = ST.StoreID,
                                                  Brand = BR.Description,
                                                  BrandID = BR.ID,
                                                  UOM = UM.Description,
                                                  Color = LT.Colour,
                                                  Recept = STR.ItemQty,
                                                  Issue = 0.0M,
                                                  REC01 = OB.REC01,
                                                  REC02 = OB.REC02,
                                                  REC03 = OB.REC03,
                                                  REC04 = OB.REC04,
                                                  REC05 = OB.REC05,
                                                  REC06 = OB.REC06,
                                                  REC07 = OB.REC07,
                                                  REC08 = OB.REC08,
                                                  REC09 = OB.REC09,
                                                  REC10 = OB.REC10,
                                                  REC11 = OB.REC11,
                                                  REC12 = OB.REC12,
                                                  ISS01 = OB.ISS01,
                                                  ISS02 = OB.ISS02,
                                                  ISS03 = OB.ISS03,
                                                  ISS04 = OB.ISS04,
                                                  ISS05 = OB.ISS05,
                                                  ISS06 = OB.ISS06,
                                                  ISS07 = OB.ISS07,
                                                  ISS08 = OB.ISS08,
                                                  ISS09 = OB.ISS09,
                                                  ISS10 = OB.ISS10,
                                                  ISS11 = OB.ISS11,
                                                  ISS12 = OB.ISS12,
                                                  ID = OB.ID,
                                                  LTID = OB.LTID,

                                              }).OrderByDescending(x => x.ID).ToList());
                        }
                    }

                    if (Receipt.Count() > 0)
                    {
                        foreach (var item in Receipt.ToList())
                        {
                            var osl = new Opticalstockledger();

                            for (var dt = STFdate; dt <= STdate;)
                            {
                                var ItemBalance = Opticalstkledger.Opticalstockledger.Where(x => x.LTID == item.LTID && x.StoreID == item.StoreID && x.CmpID == item.CmpID && x.DocumentNo == item.DocumentNo).FirstOrDefault();
                                var tdatemonth = SFdate.AddMonths(-1);
                                if (ItemBalance == null)
                                {

                                    int a = 0;
                                    int b = dt.Month;
                                    string c = a.ToString() + b.ToString();
                                    string newNumber = (b.ToString().Length == 1) ? c : dt.ToString();
                                    string issue = "ISS" + newNumber;
                                    string receipt = "REC" + newNumber;
                                    int Iss = Receipt.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID && w.DocumentNo == item.DocumentNo).Select(x => (int)x.GetType().GetProperty(issue).GetValue(x)).FirstOrDefault();
                                    int Rec = Receipt.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID && w.DocumentNo == item.DocumentNo).Select(x => (int)x.GetType().GetProperty(receipt).GetValue(x)).FirstOrDefault();
                                    osl.CmpName = item.CmpName;
                                    osl.CmpID = item.CmpID;
                                    osl.DocumentDate = item.DocumentDate;
                                    osl.DocumentNo = item.DocumentNo;
                                    osl.Type = item.Type;
                                    osl.Store = item.Store;
                                    osl.Brand = item.Brand;
                                    osl.UOM = item.UOM;
                                    osl.Color = item.Color;
                                    osl.Sphh = item.Sphh;
                                    osl.Cyll = item.Cyll;
                                    osl.Axiss = item.Axiss;
                                    osl.Addd = item.Addd;
                                    osl.Fshpaee = item.Fshpaee;
                                    osl.Ftypee = item.Ftypee;
                                    osl.Fstylee = item.Fstylee;
                                    osl.Fwidthh = item.Fwidthh;
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

                            Opticalstkledger.Opticalstockledger.Add(osl);
                            STFdate = DateTime.ParseExact(Fmonth.ToString("MM-yyyy"), format, CultureInfo.InvariantCulture);
                        }
                    }

                    foreach (var ba in BrandArray.ToList())
                    {
                        foreach (var sa in StoreArray.ToList())
                        {

                            Issue.AddRange((from OB in OpticalBalance.Where(x => x.StoreID == Convert.ToInt32(sa.ID) && x.FYID == FinancialYearId && x.CmpID == CompanyID)
                                            join LT in Lenstrans on OB.LTID equals LT.ID
                                            join BR in Brand.Where(x => x.ID == Convert.ToInt32(ba.ID)) on LT.Brand equals BR.ID
                                            join UM in uommaster on LT.UOMID equals UM.id
                                            join LM in Lensmaster on LT.LMID equals LM.RandomUniqueID
                                            join ST in Storemasters.Where(x => x.CmpID == CompanyID) on OB.StoreID equals ST.StoreID
                                            join STR in OpticalStockTran on OB.LTID equals STR.LMIDID
                                            join SM in OpticalStockMaster.Where(x => x.CreatedUTC.Date >= fdate && x.CreatedUTC.Date <= tdate && x.TransactionType == "I" && x.CMPID == CompanyID && x.StoreID == Convert.ToInt32(sa.ID)) on STR.RandomUniqueID equals SM.RandomUniqueID
                                            join TR in TransactionType on SM.TransactionID equals TR.TransactionID
                                            join cm in Company.Where(x => x.CmpID == CompanyID) on OB.CmpID equals cm.CmpID
                                            select new Issue
                                            {

                                                CmpName = cm.CompanyName + "-" + cm.Address1,
                                                CmpID = cm.CmpID,
                                                DocumentNo = SM.DocumentNumber,
                                                DocumentDate = SM.DocumentDate != null ? SM.DocumentDate.Value.Add(ts) : (DateTime?)null,
                                                DocumentType = TR.Description,
                                                Type = LM.LensType,
                                                TypeID = LM.ID,
                                                Store = ST.Storename,
                                                StoreID = ST.StoreID,
                                                Brand = BR.Description,
                                                BrandID = BR.ID,
                                                UOM = UM.Description,
                                                Color = LT.Colour,
                                                Sphh = LT.Sph != null ? "Sph :" + " " + LT.Sph : null,
                                                Cyll = LT.Cyl != null ? "Cyl :" + " " + LT.Cyl : null,
                                                Axiss = LT.Axis != null ? "Axis :" + " " + LT.Axis : null,
                                                Addd = LT.Add != null ? "Add :" + " " + LT.Add : null,
                                                Fshpaee = one.Where(f => f.OLMID == LT.FrameShapeID).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Shape :" + " " + one.Where(f => f.OLMID == LT.FrameShapeID).Select(g => g.ParentDescription).FirstOrDefault() : null,
                                                Ftypee = one.Where(f => f.OLMID == LT.FrameTypeID).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Type :" + " " + one.Where(f => f.OLMID == LT.FrameTypeID).Select(g => g.ParentDescription).FirstOrDefault() : null,
                                                Fstylee = one.Where(f => f.OLMID == LT.FrameStyleID).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Style :" + " " + one.Where(f => f.OLMID == LT.FrameStyleID).Select(g => g.ParentDescription).FirstOrDefault() : null,
                                                Fwidthh = one.Where(f => f.OLMID == LT.FrameWidthID).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Width :" + " " + one.Where(f => f.OLMID == LT.FrameWidthID).Select(g => g.ParentDescription).FirstOrDefault() : null,
                                                Receipt = 0.0M,
                                                Isue = STR.ItemQty,
                                                REC01 = OB.REC01,
                                                REC02 = OB.REC02,
                                                REC03 = OB.REC03,
                                                REC04 = OB.REC04,
                                                REC05 = OB.REC05,
                                                REC06 = OB.REC06,
                                                REC07 = OB.REC07,
                                                REC08 = OB.REC08,
                                                REC09 = OB.REC09,
                                                REC10 = OB.REC10,
                                                REC11 = OB.REC11,
                                                REC12 = OB.REC12,
                                                ISS01 = OB.ISS01,
                                                ISS02 = OB.ISS02,
                                                ISS03 = OB.ISS03,
                                                ISS04 = OB.ISS04,
                                                ISS05 = OB.ISS05,
                                                ISS06 = OB.ISS06,
                                                ISS07 = OB.ISS07,
                                                ISS08 = OB.ISS08,
                                                ISS09 = OB.ISS09,
                                                ISS10 = OB.ISS10,
                                                ISS11 = OB.ISS11,
                                                ISS12 = OB.ISS12,
                                                ID = OB.ID,
                                                LTID = OB.LTID,
                                            }).OrderByDescending(x => x.ID).ToList());
                        }
                    }

                    if (Issue.Count() > 0)
                    {
                        foreach (var item in Issue.ToList())
                        {
                            var osl = new OpticalstockledgerI();

                            for (var dt = STFdate; dt <= STdate;)
                            {
                                var ItemBalance = Opticalstkledger.OpticalstockledgerI.Where(x => x.LTID == item.LTID && x.StoreID == item.StoreID && x.CmpID == item.CmpID && x.DocumentNo == item.DocumentNo).FirstOrDefault();
                                var tdatemonth = SFdate.AddMonths(-1);
                                if (ItemBalance == null)
                                {

                                    int a = 0;
                                    int b = dt.Month;
                                    string c = a.ToString() + b.ToString();
                                    string newNumber = (b.ToString().Length == 1) ? c : dt.ToString();
                                    string issue = "ISS" + newNumber;
                                    string receipt = "REC" + newNumber;
                                    int Iss = Issue.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID && w.DocumentNo == item.DocumentNo).Select(x => (int)x.GetType().GetProperty(issue).GetValue(x)).FirstOrDefault();
                                    int Rec = Issue.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID && w.DocumentNo == item.DocumentNo).Select(x => (int)x.GetType().GetProperty(receipt).GetValue(x)).FirstOrDefault();
                                    osl.CmpName = item.CmpName;
                                    osl.CmpID = item.CmpID;
                                    osl.DocumentDate = item.DocumentDate;
                                    osl.DocumentNo = item.DocumentNo;
                                    osl.Type = item.Type;
                                    osl.Store = item.Store;
                                    osl.Brand = item.Brand;
                                    osl.UOM = item.UOM;
                                    osl.Color = item.Color;
                                    osl.Sphh = item.Sphh;
                                    osl.Cyll = item.Cyll;
                                    osl.Axiss = item.Axiss;
                                    osl.Addd = item.Addd;
                                    osl.Fshpaee = item.Fshpaee;
                                    osl.Ftypee = item.Ftypee;
                                    osl.Fstylee = item.Fstylee;
                                    osl.Fwidthh = item.Fwidthh;
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
                            Opticalstkledger.OpticalstockledgerI.Add(osl);
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


                            Issue.AddRange((from OB in OpticalBalance.Where(x => x.StoreID == Convert.ToInt32(sa.ID) && x.FYID == FinancialYearId && x.CmpID == CompanyID)
                                            join LT in Lenstrans on OB.LTID equals LT.ID
                                            join BR in Brand.Where(x => x.ID == Convert.ToInt32(ba.ID)) on LT.Brand equals BR.ID
                                            join UM in uommaster on LT.UOMID equals UM.id
                                            join LM in Lensmaster on LT.LMID equals LM.RandomUniqueID
                                            join ST in Storemasters.Where(x => x.CmpID == CompanyID) on OB.StoreID equals ST.StoreID
                                            join STR in OpticalStockTran on OB.LTID equals STR.LMIDID
                                            join SM in OpticalStockMaster.Where(x => x.CreatedUTC.Date >= fdate && x.CreatedUTC.Date <= tdate && x.TransactionType == "I" && x.CMPID == CompanyID && x.StoreID == Convert.ToInt32(sa.ID)) on STR.RandomUniqueID equals SM.RandomUniqueID
                                            join TR in TransactionType on SM.TransactionID equals TR.TransactionID
                                            join cm in Company.Where(x => x.CmpID == CompanyID) on OB.CmpID equals cm.CmpID
                                            select new Issue
                                            {

                                                CmpName = cm.CompanyName + "-" + cm.Address1,
                                                CmpID = cm.CmpID,
                                                DocumentNo = SM.DocumentNumber,
                                                DocumentDate = SM.DocumentDate != null ? SM.DocumentDate.Value.Add(ts) : (DateTime?)null,
                                                DocumentType = TR.Description,
                                                Type = LM.LensType,
                                                TypeID = LM.ID,
                                                Store = ST.Storename,
                                                StoreID = ST.StoreID,
                                                Brand = BR.Description,
                                                BrandID = BR.ID,
                                                UOM = UM.Description,
                                                Color = LT.Colour,
                                                Sphh = LT.Sph != null ? "Sph :" + " " + LT.Sph : null,
                                                Cyll = LT.Cyl != null ? "Cyl :" + " " + LT.Cyl : null,
                                                Axiss = LT.Axis != null ? "Axis :" + " " + LT.Axis : null,
                                                Addd = LT.Add != null ? "Add :" + " " + LT.Add : null,
                                                Fshpaee = one.Where(f => f.OLMID == LT.FrameShapeID).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Shape :" + " " + one.Where(f => f.OLMID == LT.FrameShapeID).Select(g => g.ParentDescription).FirstOrDefault() : null,
                                                Ftypee = one.Where(f => f.OLMID == LT.FrameTypeID).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Type :" + " " + one.Where(f => f.OLMID == LT.FrameTypeID).Select(g => g.ParentDescription).FirstOrDefault() : null,
                                                Fstylee = one.Where(f => f.OLMID == LT.FrameStyleID).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Style :" + " " + one.Where(f => f.OLMID == LT.FrameStyleID).Select(g => g.ParentDescription).FirstOrDefault() : null,
                                                Fwidthh = one.Where(f => f.OLMID == LT.FrameWidthID).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Width :" + " " + one.Where(f => f.OLMID == LT.FrameWidthID).Select(g => g.ParentDescription).FirstOrDefault() : null,
                                                Receipt = 0.0M,
                                                Isue = STR.ItemQty,
                                                REC01 = OB.REC01,
                                                REC02 = OB.REC02,
                                                REC03 = OB.REC03,
                                                REC04 = OB.REC04,
                                                REC05 = OB.REC05,
                                                REC06 = OB.REC06,
                                                REC07 = OB.REC07,
                                                REC08 = OB.REC08,
                                                REC09 = OB.REC09,
                                                REC10 = OB.REC10,
                                                REC11 = OB.REC11,
                                                REC12 = OB.REC12,
                                                ISS01 = OB.ISS01,
                                                ISS02 = OB.ISS02,
                                                ISS03 = OB.ISS03,
                                                ISS04 = OB.ISS04,
                                                ISS05 = OB.ISS05,
                                                ISS06 = OB.ISS06,
                                                ISS07 = OB.ISS07,
                                                ISS08 = OB.ISS08,
                                                ISS09 = OB.ISS09,
                                                ISS10 = OB.ISS10,
                                                ISS11 = OB.ISS11,
                                                ISS12 = OB.ISS12,
                                                ID = OB.ID,
                                                LTID = OB.LTID,
                                            }).OrderByDescending(x => x.ID).ToList());
                        }
                    }

                    if (Issue.Count() > 0)
                    {
                        foreach (var item in Issue.ToList())
                        {
                            var osl = new OpticalstockledgerI();

                            for (var dt = STFdate; dt <= STdate;)
                            {
                                var ItemBalance = Opticalstkledger.OpticalstockledgerI.Where(x => x.LTID == item.LTID && x.StoreID == item.StoreID && x.CmpID == item.CmpID && x.DocumentNo == item.DocumentNo).FirstOrDefault();
                                var tdatemonth = SFdate.AddMonths(-1);
                                if (ItemBalance == null)
                                {

                                    int a = 0;
                                    int b = dt.Month;
                                    string c = a.ToString() + b.ToString();
                                    string newNumber = (b.ToString().Length == 1) ? c : dt.ToString();
                                    string issue = "ISS" + newNumber;
                                    string receipt = "REC" + newNumber;
                                    int Iss = Issue.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID && w.DocumentNo == item.DocumentNo).Select(x => (int)x.GetType().GetProperty(issue).GetValue(x)).FirstOrDefault();
                                    int Rec = Issue.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID && w.DocumentNo == item.DocumentNo).Select(x => (int)x.GetType().GetProperty(receipt).GetValue(x)).FirstOrDefault();
                                    osl.CmpName = item.CmpName;
                                    osl.CmpID = item.CmpID;
                                    osl.DocumentDate = item.DocumentDate;
                                    osl.DocumentNo = item.DocumentNo;
                                    osl.Type = item.Type;
                                    osl.Store = item.Store;
                                    osl.Brand = item.Brand;
                                    osl.UOM = item.UOM;
                                    osl.Color = item.Color;
                                    osl.Sphh = item.Sphh;
                                    osl.Cyll = item.Cyll;
                                    osl.Axiss = item.Axiss;
                                    osl.Addd = item.Addd;
                                    osl.Fshpaee = item.Fshpaee;
                                    osl.Ftypee = item.Ftypee;
                                    osl.Fstylee = item.Fstylee;
                                    osl.Fwidthh = item.Fwidthh;
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
                            Opticalstkledger.OpticalstockledgerI.Add(osl);
                            STFdate = DateTime.ParseExact(Fmonth.ToString("MM-yyyy"), format, CultureInfo.InvariantCulture);
                        }
                    }

                    foreach (var sa in StoreArray.ToList())
                    {
                        foreach (var ba in BrandArray.ToList())
                        {


                            Receipt.AddRange((from OB in OpticalBalance.Where(x => x.StoreID == Convert.ToInt32(sa.ID) && x.FYID == FinancialYearId && x.CmpID == CompanyID)
                                              join LT in Lenstrans on OB.LTID equals LT.ID
                                              join BR in Brand.Where(x => x.ID == Convert.ToInt32(ba.ID)) on LT.Brand equals BR.ID
                                              join UM in uommaster on LT.UOMID equals UM.id
                                              join LM in Lensmaster on LT.LMID equals LM.RandomUniqueID
                                              join ST in Storemasters.Where(x => x.CmpID == CompanyID) on OB.StoreID equals ST.StoreID
                                              join STR in OpticalStockTran on OB.LTID equals STR.LMIDID
                                              join SM in OpticalStockMaster.Where(x => x.CreatedUTC.Date >= fdate && x.CreatedUTC.Date <= tdate && x.TransactionType == "R" && x.CMPID == CompanyID && x.StoreID == Convert.ToInt32(sa.ID)) on STR.RandomUniqueID equals SM.RandomUniqueID
                                              join TR in TransactionType on SM.TransactionID equals TR.TransactionID
                                              join cm in Company.Where(x => x.CmpID == CompanyID) on OB.CmpID equals cm.CmpID
                                              select new Receipt
                                              {

                                                  CmpName = cm.CompanyName + "-" + cm.Address1,
                                                  CmpID = cm.CmpID,
                                                  DocumentNo = SM.DocumentNumber,
                                                  DocumentDate = SM.DocumentDate != null ? SM.DocumentDate.Value.Add(ts) : (DateTime?)null,
                                                  DocumentType = TR.Description,
                                                  Type = LM.LensType,
                                                  TypeID = LM.ID,
                                                  Store = ST.Storename,
                                                  StoreID = ST.StoreID,
                                                  Brand = BR.Description,
                                                  BrandID = BR.ID,
                                                  UOM = UM.Description,
                                                  Color = LT.Colour,
                                                  Sphh = LT.Sph != null ? "Sph :" + " " + LT.Sph : null,
                                                  Cyll = LT.Cyl != null ? "Cyl :" + " " + LT.Cyl : null,
                                                  Axiss = LT.Axis != null ? "Axis :" + " " + LT.Axis : null,
                                                  Addd = LT.Add != null ? "Add :" + " " + LT.Add : null,
                                                  Fshpaee = one.Where(f => f.OLMID == LT.FrameShapeID).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Shape :" + " " + one.Where(f => f.OLMID == LT.FrameShapeID).Select(g => g.ParentDescription).FirstOrDefault() : null,
                                                  Ftypee = one.Where(f => f.OLMID == LT.FrameTypeID).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Type :" + " " + one.Where(f => f.OLMID == LT.FrameTypeID).Select(g => g.ParentDescription).FirstOrDefault() : null,
                                                  Fstylee = one.Where(f => f.OLMID == LT.FrameStyleID).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Style :" + " " + one.Where(f => f.OLMID == LT.FrameStyleID).Select(g => g.ParentDescription).FirstOrDefault() : null,
                                                  Fwidthh = one.Where(f => f.OLMID == LT.FrameWidthID).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Width :" + " " + one.Where(f => f.OLMID == LT.FrameWidthID).Select(g => g.ParentDescription).FirstOrDefault() : null,
                                                  Recept = STR.ItemQty,
                                                  Issue = 0.0M,
                                                  REC01 = OB.REC01,
                                                  REC02 = OB.REC02,
                                                  REC03 = OB.REC03,
                                                  REC04 = OB.REC04,
                                                  REC05 = OB.REC05,
                                                  REC06 = OB.REC06,
                                                  REC07 = OB.REC07,
                                                  REC08 = OB.REC08,
                                                  REC09 = OB.REC09,
                                                  REC10 = OB.REC10,
                                                  REC11 = OB.REC11,
                                                  REC12 = OB.REC12,
                                                  ISS01 = OB.ISS01,
                                                  ISS02 = OB.ISS02,
                                                  ISS03 = OB.ISS03,
                                                  ISS04 = OB.ISS04,
                                                  ISS05 = OB.ISS05,
                                                  ISS06 = OB.ISS06,
                                                  ISS07 = OB.ISS07,
                                                  ISS08 = OB.ISS08,
                                                  ISS09 = OB.ISS09,
                                                  ISS10 = OB.ISS10,
                                                  ISS11 = OB.ISS11,
                                                  ISS12 = OB.ISS12,
                                                  ID = OB.ID,
                                                  LTID = OB.LTID,
                                              }).OrderByDescending(x => x.ID).ToList());

                        }

                    }

                    if (Receipt.Count() > 0)
                    {
                        foreach (var item in Receipt.ToList())
                        {
                            var osl = new Opticalstockledger();

                            for (var dt = STFdate; dt <= STdate;)
                            {
                                var ItemBalance = Opticalstkledger.Opticalstockledger.Where(x => x.LTID == item.LTID && x.StoreID == item.StoreID && x.CmpID == item.CmpID && x.DocumentNo == item.DocumentNo).FirstOrDefault();
                                var tdatemonth = SFdate.AddMonths(-1);
                                if (ItemBalance == null)
                                {

                                    int a = 0;
                                    int b = dt.Month;
                                    string c = a.ToString() + b.ToString();
                                    string newNumber = (b.ToString().Length == 1) ? c : dt.ToString();
                                    string issue = "ISS" + newNumber;
                                    string receipt = "REC" + newNumber;
                                    int Iss = Receipt.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID && w.DocumentNo == item.DocumentNo).Select(x => (int)x.GetType().GetProperty(issue).GetValue(x)).FirstOrDefault();
                                    int Rec = Receipt.Where(w => w.LTID == item.LTID && w.StoreID == item.StoreID && w.CmpID == item.CmpID && w.DocumentNo == item.DocumentNo).Select(x => (int)x.GetType().GetProperty(receipt).GetValue(x)).FirstOrDefault();
                                    osl.CmpName = item.CmpName;
                                    osl.CmpID = item.CmpID;
                                    osl.DocumentDate = item.DocumentDate;
                                    osl.DocumentNo = item.DocumentNo;
                                    osl.Type = item.Type;
                                    osl.Store = item.Store;
                                    osl.Brand = item.Brand;
                                    osl.UOM = item.UOM;
                                    osl.Color = item.Color;
                                    osl.Sphh = item.Sphh;
                                    osl.Cyll = item.Cyll;
                                    osl.Axiss = item.Axiss;
                                    osl.Addd = item.Addd;
                                    osl.Fshpaee = item.Fshpaee;
                                    osl.Ftypee = item.Ftypee;
                                    osl.Fstylee = item.Fstylee;
                                    osl.Fwidthh = item.Fwidthh;
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

                            Opticalstkledger.Opticalstockledger.Add(osl);
                            STFdate = DateTime.ParseExact(Fmonth.ToString("MM-yyyy"), format, CultureInfo.InvariantCulture);
                        }
                    }

                }

            }

            Opticalstkledger.Companycomm = (from c in Company.Where(u => u.CmpID == CompanyID)

                                            select new Companycomm
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