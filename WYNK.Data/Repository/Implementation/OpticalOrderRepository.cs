using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class OpticalOrderRepository : RepositoryBase<OpticalOrderView>, IOpticalOrderRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public OpticalOrderRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic GetvenderDetails(int VendorName, int CMPID)
        {
            var OpticalOrderView = new OpticalOrderView();
            OpticalOrderView.VendorDetails = new List<VendorDetails>();
            var CompanyMaster = CMPSContext.Company.AsNoTracking().ToList();



            OpticalOrderView.VendorDetails1 = (from VD in CMPSContext.VendorMaster.Where(X => X.IsDeleted == false && X.IsActive == true && X.CmpID == CMPID && X.VendorCategory != 1)
                                               select new VendorDetails1
                                               {

                                                   ID = VD.ID,
                                                   VendorName = VD.Name,
                                                   Address1 = VD.Address1,
                                                   Address2 = VD.Address2,
                                                   PhoneNo = VD.PhoneNo,
                                                   GSTNo = VD.GSTNo,
                                                   Location = CMPSContext.LocationMaster.Where(X => X.ID == Convert.ToInt16(VD.Location)).Select(x => x.ParentDescription).FirstOrDefault(),
                                                   City = Convert.ToString(CMPSContext.LocationMaster.Where(x => x.ID == Convert.ToInt16(VD.Location)).Select(x => x.ParentDescription).FirstOrDefault()),
                                                   }).AsNoTracking().ToList();


            OpticalOrderView.VendorDetails = (from VD in CMPSContext.VendorMaster.Where(x => x.ID == VendorName && x.CmpID == CMPID)
                                              select new VendorDetails
                                              {

                                                  ID = VD.ID,
                                                  VendorName = VD.Name,
                                                  Address1 = VD.Address1,
                                                  Address2 = VD.Address2,
                                                  PhoneNo = VD.PhoneNo,
                                                  GSTNo = VD.GSTNo,
                                                  Location = CMPSContext.LocationMaster.Where(X => X.ID == Convert.ToInt16(VD.Location)).Select(x => x.ParentDescription).FirstOrDefault(),
                                                  City = Convert.ToString(CMPSContext.LocationMaster.Where(x => x.ID == Convert.ToInt16(VD.Location)).Select(x => x.ParentID).FirstOrDefault()),
                                                  CompanyName = CompanyMaster.Where(x => x.CmpID == CMPID).Select(x => x.CompanyName).FirstOrDefault(),
                                                  CAddress1 = CompanyMaster.Where(x => x.CmpID == CMPID).Select(x => x.Address1).FirstOrDefault(),
                                                  CAddress2 = CompanyMaster.Where(x => x.CmpID == CMPID).Select(x => x.Address2).FirstOrDefault(),
                                                  CAddress3 = CompanyMaster.Where(x => x.CmpID == CMPID).Select(x => x.Address3).FirstOrDefault(),
                                                  CLocationID = CMPSContext.LocationMaster.Where(X => X.ID == CompanyMaster.Where(g => g.CmpID == CMPID).Select(g => g.LocationID).FirstOrDefault()).Select(x => x.ParentDescription).FirstOrDefault(),
                                                  CCity = Convert.ToString(CMPSContext.LocationMaster.Where(x => x.ID == CompanyMaster.Where(g => g.CmpID == CMPID).Select(g => g.LocationID).FirstOrDefault()).Select(x => x.ParentID).FirstOrDefault()),
                                              }).AsNoTracking().ToList();


            return OpticalOrderView;
        }
        public OpticalOrderView GetlocationDetails(int id)
        {

            var locDetails = new OpticalOrderView();

            var v = CMPSContext.LocationMaster.Where(x => x.ID == id).Select(x => x.ParentDescription).FirstOrDefault();
            var stateid = CMPSContext.LocationMaster.Where(x => x.ParentDescription == v).Select(x => x.ParentID).FirstOrDefault();
            locDetails.ParentDescriptionstate = CMPSContext.LocationMaster.Where(x => x.ID == stateid).Select(x => x.ParentDescription).FirstOrDefault();

            var countryid = CMPSContext.LocationMaster.Where(x => x.ParentDescription == locDetails.ParentDescriptionstate).Select(x => x.ParentID).FirstOrDefault();
            locDetails.ParentDescriptioncountry = CMPSContext.LocationMaster.Where(x => x.ID == countryid).Select(x => x.ParentDescription).FirstOrDefault();
            return locDetails;

        }
        public decimal? getvalue(decimal? data)
        {
            var zero = 0;
            var nulldata = data;
            if (nulldata != null)
            {
                return nulldata;
            }
            return zero;
        }
        public dynamic GetOpticalDetails(int lensTranID, int CmpID)
        {
            var LensTrans = WYNKContext.Lenstrans.AsNoTracking().ToList();
            var uommaster = CMPSContext.uommaster.AsNoTracking().ToList();

            var OpticalDetails = new OpticalOrderView();
            OpticalDetails.OpticalOrderdetails = new List<OpticalOrderdetails>();

            var TaxID = WYNKContext.Lenstrans.Where(x => x.ID == lensTranID).Select(x => x.TaxID).FirstOrDefault();
            OpticalDetails.OpticalOrderdetails = (from LT in LensTrans.Where(x => x.ID == lensTranID)
                                                  join UM in uommaster on LT.UOMID equals UM.id
                                                  select new OpticalOrderdetails
                                                  {
                                                      LMID = LT.LMID,
                                                      LTID = LT.ID,
                                                      UOMDescription = UM.Description,
                                                      Prize = LT.Prize,
                                                      GST = Convert.ToInt16(getvalue(CMPSContext.TaxMaster.Where(x => x.ID == TaxID).Select(x => x.GSTPercentage).FirstOrDefault())),
                                                      IGST = Convert.ToInt16(getvalue(CMPSContext.TaxMaster.Where(x => x.ID == TaxID).Select(x => x.IGSTPercentage).FirstOrDefault())),
                                                      CESS = getvalue(CMPSContext.TaxMaster.Where(x => x.ID == TaxID).Select(x => x.CESSPercentage).FirstOrDefault()),
                                                      AdditionalCESS = getvalue(CMPSContext.TaxMaster.Where(x => x.ID == TaxID).Select(x => x.AdditionalCESSPercentage).FirstOrDefault()),
                                                      TaxDescription = CMPSContext.TaxMaster.Where(x => x.ID == TaxID).Select(t => t.TaxDescription).FirstOrDefault(),
                                                      CESSDescription = CMPSContext.TaxMaster.Where(x => x.ID == TaxID).Select(t => t.CESSDescription).FirstOrDefault(),
                                                      AdditionalCESSDescription = CMPSContext.TaxMaster.Where(x => x.ID == TaxID).Select(t => t.AdditionalCESSDescription).FirstOrDefault(),

                                                  }
                                        ).ToList();
            return OpticalDetails;
        }

        public dynamic InsertOpticalOrder(OpticalOrderView AddOptical, int cmpid, int TransactionTypeid)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var OpticalOrderM = new OpticalOrder();

                    OpticalOrderM.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                    OpticalOrderM.CmpID = AddOptical.OpticalOrder.CmpID;
                    OpticalOrderM.OrderNumber = AddOptical.OpticalOrder.OrderNumber;
                    OpticalOrderM.Fyear = AddOptical.OpticalOrder.Fyear;
                    OpticalOrderM.OrderDate = AddOptical.OpticalOrder.OrderDate;
                    OpticalOrderM.RefNo = AddOptical.OpticalOrder.RefNo;
                    OpticalOrderM.RefDate = AddOptical.OpticalOrder.RefDate;
                    OpticalOrderM.TransactionID = TransactionTypeid;
                    OpticalOrderM.VendorID = AddOptical.OpticalOrder.VendorID;
                    OpticalOrderM.DeliveryName = AddOptical.OpticalOrder.DeliveryName;
                    OpticalOrderM.DeliveryDate = AddOptical.OpticalOrder.DeliveryDate;
                    OpticalOrderM.Instructions = AddOptical.OpticalOrder.Instructions;
                    OpticalOrderM.DeliveryAddress1 = AddOptical.OpticalOrder.DeliveryAddress1;
                    OpticalOrderM.DeliveryAddress2 = AddOptical.OpticalOrder.DeliveryAddress2;
                    OpticalOrderM.DeliveryAddress3 = AddOptical.OpticalOrder.DeliveryAddress3;
                    OpticalOrderM.DeliveryLocationID = AddOptical.OpticalOrder.DeliveryLocationID;
                    OpticalOrderM.GrossProductValue = AddOptical.OpticalOrder.GrossProductValue;
                    OpticalOrderM.TotalProductValue = AddOptical.OpticalOrder.TotalProductValue;
                    OpticalOrderM.TotalDiscountAmount = AddOptical.OpticalOrder.TotalDiscountAmount;
                    OpticalOrderM.TotalGSTTaxValue = AddOptical.OpticalOrder.TotalGSTTaxValue;
                    OpticalOrderM.TotalCGSTTaxValue = AddOptical.OpticalOrder.TotalCGSTTaxValue;
                    OpticalOrderM.TotalSGSTTaxValue = AddOptical.OpticalOrder.TotalSGSTTaxValue;
                    OpticalOrderM.TotalIGSTTaxValue = AddOptical.OpticalOrder.TotalIGSTTaxValue;
                    OpticalOrderM.CESSAmount = AddOptical.OpticalOrder.CESSAmount;
                    OpticalOrderM.AddCESSPerAmt = AddOptical.OpticalOrder.AddCESSPerAmt;
                    OpticalOrderM.IsCancelled = false;
                    OpticalOrderM.TermsAndConditions = AddOptical.OpticalOrder.TermsAndConditions;
                    OpticalOrderM.Validity = AddOptical.OpticalOrder.Validity;
                    OpticalOrderM.IsOrderExecuted = 0;
                    OpticalOrderM.CreatedUTC = DateTime.UtcNow;
                    OpticalOrderM.CreatedBy = AddOptical.OpticalOrder.CreatedBy;
                    WYNKContext.OpticalOrder.Add(OpticalOrderM);

                    var ramuID = OpticalOrderM.RandomUniqueID;

                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == AddOptical.OpticalOrder.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    string userid = Convert.ToString(AddOptical.OpticalOrder.CreatedBy);
                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(AddOptical.Companyname, "OpticalOrder", "User name :", username, "User ID :", Convert.ToString(userid), "Mode : Add");
                    object names = OpticalOrderM;
                    oErrorLogs.WriteErrorLogArray("OpticalOrder", names);


                    foreach (var item in AddOptical.OPticalDetails.ToList())

                    {
                        var OpticalOrderTran = new OpticalOrderTran();
                        var GetOOID = OpticalOrderM.ID;
                        OpticalOrderTran.RandomUniqueID = ramuID;
                        OpticalOrderTran.LTID = item.LTID;
                        OpticalOrderTran.UOMID = CMPSContext.uommaster.Where(x => x.Description == item.UOMDescription).Select(x => x.id).FirstOrDefault();
                        OpticalOrderTran.OrderedQty = item.Quantity;
                        OpticalOrderTran.ReceivedQty = 0;
                        OpticalOrderTran.Price = item.Prize;
                        OpticalOrderTran.DiscountPercentage = item.Discount;
                        OpticalOrderTran.DiscountAmount = item.DiscountAmount;
                        OpticalOrderTran.GSTPercentage = item.GST;
                        OpticalOrderTran.GSTTaxValue = item.GSTAmount;
                        OpticalOrderTran.SGSTPercentage = item.GST / 2;
                        OpticalOrderTran.CGSTPercentage = item.GST / 2;
                        OpticalOrderTran.SGSTTaxValue = item.GSTAmount / 2;
                        OpticalOrderTran.CGSTTaxValue = item.GSTAmount / 2;
                        OpticalOrderTran.IGSTPercentage = item.IGST;
                        OpticalOrderTran.IGSTTaxValue = item.IGSTAmount;
                        OpticalOrderTran.CESSPercentage = item.CESS;
                        OpticalOrderTran.CESSAmount = item.CESSAmount;
                        OpticalOrderTran.AdditionalCESSPercentage = item.AdditionalCESS;
                        OpticalOrderTran.AddCESSPerAmt = item.AdditionalCESSAmount;
                        OpticalOrderTran.IsCancelled = false;
                        OpticalOrderTran.CreatedUTC = DateTime.UtcNow;
                        OpticalOrderTran.CreatedBy = AddOptical.OpticalOrder.CreatedBy;
                        WYNKContext.OpticalOrderTran.AddRange(OpticalOrderTran);

                        ErrorLog oErrorLogstran = new ErrorLog();
                        object namestr = OpticalOrderTran;
                        oErrorLogstran.WriteErrorLogArray("OpticalOrderTran", namestr);

                    }

                    WYNKContext.SaveChanges();


                    foreach (var item1 in AddOptical.paymenttran.ToList())
                    {
                        var payment = new Payment_Master();
                        payment.UIN = AddOptical.OpticalOrder.OrderNumber;
                        payment.PaymentType = "A";
                        payment.PaymentMode = item1.PaymentMode;
                        payment.OLMID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == item1.PaymentMode).Select(x => x.OLMID).FirstOrDefault();
                        payment.Amount = item1.Amount;
                        payment.BankBranch = item1.BankBranch;
                        payment.BankName = item1.BankName;
                        payment.InstrumentNumber = item1.InstrumentNumber;
                        if (item1.Instrumentdate != null)
                        {
                            payment.Instrumentdate = Convert.ToDateTime(item1.Instrumentdate).AddDays(1);
                        }
                        else
                        {
                            payment.Instrumentdate = null;
                        }
                        if (item1.Expirydate != null)
                        {
                            payment.Expirydate = Convert.ToDateTime(item1.Expirydate).AddDays(1);
                        }
                        else
                        {
                            payment.Expirydate = null;
                        }
                        payment.PaymentReferenceID = null;
                        payment.InVoiceDate = DateTime.UtcNow;
                        payment.InVoiceNumber = AddOptical.OpticalOrder.OrderNumber;
                        payment.ReceiptDate = DateTime.UtcNow;
                        payment.ReceiptNumber = AddOptical.ReceiptRunningNo;
                        payment.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= DateTime.Now && Convert.ToDateTime(b.FYTo) >= DateTime.Now && b.CMPID == cmpid && b.IsActive == true).Select(f => f.ID).FirstOrDefault()).Select(c => c.FYAccYear).FirstOrDefault());
                        payment.TransactionID = TransactionTypeid;
                        payment.CmpID = cmpid;
                        payment.IsBilled = false;
                        payment.CreatedUTC = DateTime.UtcNow;
                        payment.CreatedBy = AddOptical.OpticalOrder.CreatedBy;
                        WYNKContext.PaymentMaster.Add(payment);
                        ErrorLog oErrorLogstran = new ErrorLog();
                        object namestr = payment;
                        oErrorLogstran.WriteErrorLogArray("PaymentMaster", namestr);
                        WYNKContext.SaveChanges();
                    }
                    var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                    var RunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, AddOptical.OpticalOrder.CmpID, "GetRunningNo");

                    if (RunningNumber == AddOptical.OpticalOrder.OrderNumber)
                    {
                        commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, AddOptical.OpticalOrder.CmpID, "UpdateRunningNo");
                    }
                    else
                    {
                        var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, AddOptical.OpticalOrder.CmpID, "UpdateRunningNo");
                        var OpticalOrderno = WYNKContext.OpticalOrder.Where(x => x.OrderNumber == AddOptical.OpticalOrder.OrderNumber).FirstOrDefault();
                        OpticalOrderno.OrderNumber = GetRunningNumber;
                        WYNKContext.OpticalOrder.UpdateRange(OpticalOrderno);
                        WYNKContext.SaveChanges();
                    }


                    var RecContraID = commonRepos.GettingReceiptTcID(TransactionTypeid, cmpid);
                    var RunningNumber1 = commonRepos.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), AddOptical.OpticalOrder.CmpID, "GetRunningNo");


                    if (RunningNumber1 == AddOptical.ReceiptRunningNo)
                    {
                        commonRepos.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), cmpid, "UpdateRunningNo");
                    }
                    else
                    {
                        var payments = WYNKContext.PaymentMaster.Where(x => x.ReceiptNumber == AddOptical.ReceiptRunningNo && x.TransactionID == TransactionTypeid).ToList();
                        payments.All(x => { x.ReceiptNumber = RunningNumber1; return true; });
                        WYNKContext.PaymentMaster.UpdateRange(payments); ;
                    }
                    WYNKContext.SaveChanges();


                    if (WYNKContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }

                    dbContextTransaction.Commit();
                    return new
                    {
                        Success = true,
                        OPticalnum = OpticalOrderM.OrderNumber,
                        OOID = OpticalOrderM.ID,
                        PAddress = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1).FirstOrDefault(),
                        PAddress2 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault(),
                        PAddress3 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address3).FirstOrDefault(),
                        Pphone = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Phone1).FirstOrDefault(),
                        Pweb = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Website).FirstOrDefault(),
                        PCompnayname = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault(),
                        ReceiptDate = DateTime.UtcNow,
                        ReceiptNumber = AddOptical.ReceiptRunningNo,
                    };
                }
                catch (Exception ex)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                    string msg = ex.InnerException.Message;
                    return new { Success = false, Message = msg, grn = AddOptical.OpticalOrder.OrderNumber };
                }


            }
        }
        public dynamic OpticalUpdateDetails(int Cmpid)
        {
            var OpticalOrder = WYNKContext.OpticalOrder.AsNoTracking().ToList();

            var OpticalDetailsUpdate = new OpticalOrderView();
            OpticalDetailsUpdate.OpticalOrderUpdate = (from OO in OpticalOrder.Where(x => x.CmpID == Cmpid)

                                                       select new OpticalOrderUpdate
                                                       {
                                                           ID = OO.RandomUniqueID,
                                                           OrderNumber = OO.OrderNumber,
                                                           OrderDate = OO.OrderDate,
                                                           RefNo = OO.RefNo,
                                                           RefDate = OO.RefDate,
                                                           VendorName = CMPSContext.VendorMaster.Where(x => x.ID == OO.VendorID).Select(x => x.Name).FirstOrDefault(),
                                                           PhoneNo = CMPSContext.VendorMaster.Where(x => x.ID == OO.VendorID).Select(x => x.PhoneNo).FirstOrDefault(),
                                                           Address1 = CMPSContext.VendorMaster.Where(x => x.ID == OO.VendorID).Select(x => x.Address1).FirstOrDefault(),
                                                           Address2 = CMPSContext.VendorMaster.Where(x => x.ID == OO.VendorID).Select(x => x.Address2).FirstOrDefault(),
                                                           GSTNo = CMPSContext.VendorMaster.Where(x => x.ID == OO.VendorID).Select(x => x.GSTNo).FirstOrDefault(),
                                                           VendorID = OO.VendorID,
                                                           DeliveryName = OO.DeliveryName,
                                                           DeliveryDate = OO.DeliveryDate,
                                                           DeliveryAddress1 = OO.DeliveryAddress1,
                                                           DeliveryAddress2 = OO.DeliveryAddress2,
                                                           DeliveryAddress3 = OO.DeliveryAddress3,
                                                           Validity = OO.Validity,
                                                           TermsAndConditions = OO.TermsAndConditions,
                                                           DeliveryLocationName = CMPSContext.LocationMaster.Where(x => x.ID == OO.DeliveryLocationID).Select(x => x.ParentDescription).FirstOrDefault(),
                                                           Dcity = Convert.ToString(CMPSContext.LocationMaster.Where(x => x.ID == Convert.ToInt16(OO.DeliveryLocationID)).Select(x => x.ParentID).FirstOrDefault()),
                                                           Status = Enum.GetName(typeof(OpticalOrderStatus), (OO.IsOrderExecuted)),
                                                       }
                                        ).OrderByDescending(x => x.OrderNumber).ToList();

            return new
            {
                OpticalDetailsUpdate,
                OOID = OpticalDetailsUpdate.OpticalOrderUpdate.Select(x => x.ID).FirstOrDefault(),
            };
        }

        public dynamic OpticalbindingDetails(int Cmpid, string OOID)
        {
            var OpticalOrderTran = WYNKContext.OpticalOrderTran.AsNoTracking().ToList();
            var CompanyMaster = CMPSContext.Company.AsNoTracking().ToList();

            var LTID = OpticalOrderTran.Where(x => x.RandomUniqueID == OOID).Select(x => x.LTID).FirstOrDefault();
            var TaxID = WYNKContext.Lenstrans.AsNoTracking().Where(x => x.ID == LTID).Select(x => x.TaxID).FirstOrDefault();

            var OpticalDetailsUpdate = new OpticalOrderView();
            OpticalDetailsUpdate.OpticalbindingDet = (from OOT in OpticalOrderTran.Where(x => x.RandomUniqueID == OOID && x.IsCancelled == false)
                                                      select new OpticalbindingDet
                                                      {
                                                          ID = OOT.ID,
                                                          Index = WYNKContext.Lenstrans.Where(x => x.ID == OOT.LTID).Select(x => x.Index).FirstOrDefault(),
                                                          Model = WYNKContext.Lenstrans.Where(x => x.ID == OOT.LTID).Select(x => x.Model).FirstOrDefault(),
                                                          Color = WYNKContext.Lenstrans.Where(x => x.ID == OOT.LTID).Select(x => x.Colour).FirstOrDefault(),
                                                          Type = WYNKContext.Brand.Where(x => x.ID == WYNKContext.Lenstrans.Where(z => z.ID == OOT.LTID).Select(z => z.Brand).FirstOrDefault())
                                                               .Select(x => x.BrandType).FirstOrDefault(),
                                                          Brand = WYNKContext.Brand.Where(x => x.ID == WYNKContext.Lenstrans.Where(z => z.ID == OOT.LTID).Select(z => z.Brand).FirstOrDefault())
                                                                .Select(x => x.Description).FirstOrDefault(),
                                                          UOMDescription = CMPSContext.uommaster.Where(x => x.id == OOT.UOMID).Select(x => x.Description).FirstOrDefault(),
                                                          Quantity = OOT.OrderedQty,
                                                          Prize = OOT.Price,
                                                          Amount = OOT.OrderedQty * OOT.Price,
                                                          Discount = OOT.DiscountPercentage,
                                                          DiscountAmount = OOT.DiscountAmount,
                                                          GST = OOT.GSTPercentage,
                                                          GSTAmount = OOT.GSTTaxValue,
                                                          IGST = OOT.IGSTPercentage,
                                                          IGSTAmount = OOT.IGSTTaxValue,
                                                          CESS = OOT.CESSPercentage,
                                                          CESSAmount = OOT.CESSAmount,
                                                          AdditionalCESS = OOT.AdditionalCESSPercentage,
                                                          AdditionalCESSAmount = OOT.AddCESSPerAmt,
                                                          GrossAmount = OOT.OrderedQty * OOT.Price - OOT.OrderedQty * OOT.Price * OOT.DiscountPercentage / 100,
                                                          TotalAmount = (OOT.OrderedQty * OOT.Price - OOT.OrderedQty * OOT.Price * OOT.DiscountPercentage / 100) +
                                                                     (OOT.GSTTaxValue) + (OOT.CESSAmount) + (OOT.AddCESSPerAmt),
                                                          CompanyName = CompanyMaster.Where(x => x.CmpID == Cmpid).Select(x => x.CompanyName).FirstOrDefault(),
                                                          CAddress1 = CompanyMaster.Where(x => x.CmpID == Cmpid).Select(x => x.Address1).FirstOrDefault(),
                                                          CAddress2 = CompanyMaster.Where(x => x.CmpID == Cmpid).Select(x => x.Address2).FirstOrDefault(),
                                                          CAddress3 = CompanyMaster.Where(x => x.CmpID == Cmpid).Select(x => x.Address3).FirstOrDefault(),
                                                          CPhone1 = CompanyMaster.Where(x => x.CmpID == Cmpid).Select(x => x.Phone1).FirstOrDefault(),
                                                          CWebsite = CompanyMaster.Where(x => x.CmpID == Cmpid).Select(x => x.Website).FirstOrDefault(),
                                                          TaxDescription = CMPSContext.TaxMaster.Where(x => x.ID == TaxID).Select(t => t.TaxDescription).FirstOrDefault(),
                                                          CESSDescription = CMPSContext.TaxMaster.Where(x => x.ID == TaxID).Select(t => t.CESSDescription).FirstOrDefault(),
                                                          AdditionalCESSDescription = CMPSContext.TaxMaster.Where(x => x.ID == TaxID).Select(t => t.AdditionalCESSDescription).FirstOrDefault(),

                                                      }
                                 ).ToList();

            OpticalDetailsUpdate.Totalsum = OpticalDetailsUpdate.OpticalbindingDet.Select(x => x.TotalAmount).Sum();
            return OpticalDetailsUpdate;
        }



        public dynamic UpdateOpticalOrder(OpticalOrderView OpticalUpdate, int Cmpid, int TransactionTypeid, int OpticalOrderID)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var OpticalOrderM = new OpticalOrder();
                    OpticalOrderM = WYNKContext.OpticalOrder.Where(x => x.ID == OpticalOrderID).FirstOrDefault();
                    OpticalOrderM.CmpID = OpticalUpdate.OpticalOrder.CmpID;
                    OpticalOrderM.OrderDate = OpticalUpdate.OpticalOrder.OrderDate;
                    OpticalOrderM.RefNo = OpticalUpdate.OpticalOrder.RefNo;
                    OpticalOrderM.RefDate = OpticalUpdate.OpticalOrder.RefDate;
                    OpticalOrderM.VendorID = OpticalUpdate.OpticalOrder.VendorID;
                    OpticalOrderM.DeliveryName = OpticalUpdate.OpticalOrder.DeliveryName;
                    OpticalOrderM.DeliveryDate = OpticalUpdate.OpticalOrder.DeliveryDate;
                    OpticalOrderM.Instructions = OpticalUpdate.OpticalOrder.Instructions;
                    OpticalOrderM.DeliveryAddress1 = OpticalUpdate.OpticalOrder.DeliveryAddress1;
                    OpticalOrderM.DeliveryAddress2 = OpticalUpdate.OpticalOrder.DeliveryAddress2;
                    OpticalOrderM.DeliveryAddress3 = OpticalUpdate.OpticalOrder.DeliveryAddress3;
                    OpticalOrderM.DeliveryLocationID = OpticalUpdate.OpticalOrder.DeliveryLocationID;
                    OpticalOrderM.GrossProductValue = OpticalUpdate.OpticalOrder.GrossProductValue;
                    OpticalOrderM.TotalProductValue = OpticalUpdate.OpticalOrder.TotalProductValue;
                    OpticalOrderM.TotalDiscountAmount = OpticalUpdate.OpticalOrder.TotalDiscountAmount;
                    OpticalOrderM.TotalGSTTaxValue = OpticalUpdate.OpticalOrder.TotalGSTTaxValue;
                    OpticalOrderM.TotalCGSTTaxValue = OpticalUpdate.OpticalOrder.TotalCGSTTaxValue;
                    OpticalOrderM.TotalSGSTTaxValue = OpticalUpdate.OpticalOrder.TotalSGSTTaxValue;
                    OpticalOrderM.TotalIGSTTaxValue = OpticalUpdate.OpticalOrder.TotalIGSTTaxValue;
                    OpticalOrderM.CESSAmount = OpticalUpdate.OpticalOrder.CESSAmount;
                    OpticalOrderM.AddCESSPerAmt = OpticalUpdate.OpticalOrder.AddCESSPerAmt;
                    OpticalOrderM.IsCancelled = false;
                    OpticalOrderM.TermsAndConditions = OpticalUpdate.OpticalOrder.TermsAndConditions;
                    OpticalOrderM.Validity = OpticalUpdate.OpticalOrder.Validity;
                    OpticalOrderM.IsOrderExecuted = 0;
                    OpticalOrderM.UpdatedUTC = DateTime.UtcNow;
                    OpticalOrderM.UpdatedBy = OpticalUpdate.OpticalOrder.CreatedBy;
                    WYNKContext.Entry(OpticalOrderM).State = EntityState.Modified;
                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == OpticalUpdate.OpticalOrder.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    string userid = Convert.ToString(OpticalUpdate.OpticalOrder.CreatedBy);
                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(OpticalUpdate.Companyname, "OpticalOrder", "User name :", username, "User ID :", Convert.ToString(userid), "Mode : Update");
                    object names = OpticalOrderM;
                    oErrorLogs.WriteErrorLogArray("OpticalOrder", names);


                    foreach (var item in OpticalUpdate.OPticalDetails.ToList())
                    {
                        var OOID = WYNKContext.OpticalOrderTran.Where(x => x.ID == item.ID).Select(x => x.ID).FirstOrDefault();
                        var OpticalOrderTran = new OpticalOrderTran();
                        if (OOID != 0)
                        {

                            OpticalOrderTran = WYNKContext.OpticalOrderTran.Where(x => x.ID == OOID).FirstOrDefault();
                            OpticalOrderTran.UOMID = CMPSContext.uommaster.Where(x => x.Description == item.UOMDescription).Select(x => x.id).FirstOrDefault();
                            OpticalOrderTran.OrderedQty = item.Quantity;
                            OpticalOrderTran.ReceivedQty = 0;
                            OpticalOrderTran.Price = item.Prize;
                            OpticalOrderTran.DiscountPercentage = item.Discount;
                            OpticalOrderTran.DiscountAmount = item.DiscountAmount;
                            OpticalOrderTran.GSTPercentage = item.GST;
                            OpticalOrderTran.GSTTaxValue = item.GSTAmount;
                            OpticalOrderTran.SGSTPercentage = item.GST / 2;
                            OpticalOrderTran.CGSTPercentage = item.GST / 2;
                            OpticalOrderTran.SGSTTaxValue = item.GSTAmount / 2;
                            OpticalOrderTran.CGSTTaxValue = item.GSTAmount / 2;
                            OpticalOrderTran.IGSTPercentage = item.IGST;
                            OpticalOrderTran.IGSTTaxValue = item.IGSTAmount;
                            OpticalOrderTran.CESSPercentage = item.CESS;
                            OpticalOrderTran.CESSAmount = item.CESSAmount;
                            OpticalOrderTran.AdditionalCESSPercentage = item.AdditionalCESS;
                            OpticalOrderTran.AddCESSPerAmt = item.AdditionalCESSAmount;
                            OpticalOrderTran.IsCancelled = false;
                            OpticalOrderTran.UpdatedUTC = DateTime.UtcNow;
                            OpticalOrderTran.UpdatedBy = OpticalUpdate.OpticalOrder.CreatedBy;
                            WYNKContext.Entry(OpticalOrderTran).State = EntityState.Modified;
                            ErrorLog oErrorLogstran = new ErrorLog();
                            object namestr = OpticalOrderTran;
                            oErrorLogstran.WriteErrorLogArray("OpticalOrderTran", namestr);
                        }
                        else
                        {

                            OpticalOrderTran.RandomUniqueID = Convert.ToString(OpticalOrderID);

                            OpticalOrderTran.UOMID = CMPSContext.uommaster.Where(x => x.Description == item.UOMDescription).Select(x => x.id).FirstOrDefault();
                            OpticalOrderTran.OrderedQty = item.Quantity;
                            OpticalOrderTran.ReceivedQty = 0;
                            OpticalOrderTran.Price = item.Prize;
                            OpticalOrderTran.DiscountPercentage = item.Discount;
                            OpticalOrderTran.DiscountAmount = item.DiscountAmount;
                            OpticalOrderTran.GSTPercentage = item.GST;
                            OpticalOrderTran.GSTTaxValue = item.GSTAmount;
                            OpticalOrderTran.SGSTPercentage = item.GST / 2;
                            OpticalOrderTran.CGSTPercentage = item.GST / 2;
                            OpticalOrderTran.SGSTTaxValue = item.GSTAmount / 2;
                            OpticalOrderTran.CGSTTaxValue = item.GSTAmount / 2;
                            OpticalOrderTran.IGSTPercentage = item.IGST;
                            OpticalOrderTran.IGSTTaxValue = item.IGSTAmount;
                            OpticalOrderTran.CESSPercentage = item.CESS;
                            OpticalOrderTran.CESSAmount = item.CESSAmount;
                            OpticalOrderTran.AdditionalCESSPercentage = item.AdditionalCESS;
                            OpticalOrderTran.AddCESSPerAmt = item.AdditionalCESSAmount;
                            OpticalOrderTran.IsCancelled = false;
                            OpticalOrderTran.CreatedUTC = DateTime.UtcNow;
                            OpticalOrderTran.CreatedBy = OpticalUpdate.OpticalOrder.CreatedBy;
                            WYNKContext.OpticalOrderTran.AddRange(OpticalOrderTran);
                            ErrorLog oErrorLogstran = new ErrorLog();
                            object namestr = OpticalOrderTran;
                            oErrorLogstran.WriteErrorLogArray("OpticalOrderTran", namestr);

                        }
                    }
                    WYNKContext.SaveChanges();


                    foreach (var item1 in OpticalUpdate.paymenttran.ToList())
                    {
                        var payment = new Payment_Master();
                        payment.UIN = OpticalUpdate.OpticalOrder.OrderNumber;
                        payment.PaymentType = "A";
                        payment.PaymentMode = item1.PaymentMode;
                        payment.OLMID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == item1.PaymentMode).Select(x => x.OLMID).FirstOrDefault();
                        payment.Amount = item1.Amount;
                        payment.BankBranch = item1.BankBranch;
                        payment.BankName = item1.BankName;
                        payment.InstrumentNumber = item1.InstrumentNumber;
                        if (item1.Instrumentdate != null)
                        {
                            payment.Instrumentdate = Convert.ToDateTime(item1.Instrumentdate).AddDays(1);
                        }
                        else
                        {
                            payment.Instrumentdate = null;
                        }
                        if (item1.Expirydate != null)
                        {
                            payment.Expirydate = Convert.ToDateTime(item1.Expirydate).AddDays(1);
                        }
                        else
                        {
                            payment.Expirydate = null;
                        }
                        payment.PaymentReferenceID = null;
                        payment.InVoiceDate = DateTime.UtcNow;
                        payment.InVoiceNumber = OpticalUpdate.OpticalOrder.OrderNumber;
                        payment.ReceiptDate = DateTime.UtcNow;
                        payment.ReceiptNumber = OpticalUpdate.ReceiptRunningNo;
                        payment.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= DateTime.Now && Convert.ToDateTime(b.FYTo) >= DateTime.Now && b.CMPID == Cmpid && b.IsActive == true).Select(f => f.ID).FirstOrDefault()).Select(c => c.FYAccYear).FirstOrDefault());
                        payment.TransactionID = TransactionTypeid;
                        payment.CmpID = Cmpid;
                        payment.IsBilled = false;
                        payment.CreatedUTC = DateTime.UtcNow;
                        payment.CreatedBy = OpticalUpdate.OpticalOrder.CreatedBy;
                        WYNKContext.PaymentMaster.Add(payment);
                        ErrorLog oErrorLogstran = new ErrorLog();
                        object namestr = payment;
                        oErrorLogstran.WriteErrorLogArray("PaymentMaster", namestr);
                        WYNKContext.SaveChanges();
                    }



                    if (WYNKContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }


                    dbContextTransaction.Commit();
                    return new
                    {
                        Success = true,

                    };
                }

                catch (Exception ex)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                    dbContextTransaction.Rollback();

                    Console.Write(ex);
                }
                return new
                {
                    Success = false,

                };
            }
        }

        public dynamic DeleteOpticalOrder(OpticalOrderView OpticalDelete, int Cmpid, int TransactionTypeid, int OpticaltrnID)
        {
            try
            {
                if (OpticaltrnID != 0)
                {
                    var OpticalOrderTran = new OpticalOrderTran();
                    OpticalOrderTran = WYNKContext.OpticalOrderTran.Where(x => x.ID == OpticaltrnID).FirstOrDefault();
                    OpticalOrderTran.IsCancelled = true;
                    WYNKContext.Entry(OpticalOrderTran).State = EntityState.Modified;

                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(OpticalDelete.Companyname, "OpticalOrder", "User name :", "username", "User ID :", "", "Mode : Delete");
                    object names = OpticalOrderTran;
                    oErrorLogs.WriteErrorLogArray("OpticalOrder", names);

                    WYNKContext.SaveChanges();
                }
                else
                {
                    return new
                    {
                        Success = true,

                    };
                }
                return new
                {
                    Success = true,

                };
            }
            catch (Exception ex)
            {
                ErrorLog oErrorLog = new ErrorLog();
                oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                Console.Write(ex);
            }
            return new
            {
                Success = false,

            };
        }


        public OpticalOrderView getpayment(string OrderNumber, int Cmpid)
        {
            var payment = new OpticalOrderView();
            var PaymentMaster = WYNKContext.PaymentMaster.AsNoTracking().ToList();
            payment.paymentReMode1 = (from PTY in PaymentMaster.Where(x => x.InVoiceNumber == OrderNumber && x.CmpID == Cmpid)

                                      select new paymentReMode1
                                      {
                                          PaymentMode = PTY.PaymentMode,
                                          InstrumentNumber = PTY.InstrumentNumber,
                                          Instrumentdate = PTY.Instrumentdate,
                                          BankName = PTY.BankName,
                                          BankBranch = PTY.BankBranch,
                                          Expirydate = PTY.Expirydate,
                                          Amount = PTY.Amount,
                                          InVoiceDate = PTY.InVoiceDate,
                                          InVoiceNumber = PTY.InVoiceNumber,
                                      }).ToList();

            payment.TOpayMode1 = payment.paymentReMode1.Select(x => x.Amount).Sum();

            return payment;
        }


       
    }
}