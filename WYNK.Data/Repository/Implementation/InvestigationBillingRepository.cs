
using System;
using System.Collections.Generic;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class InvestigationBillingRepository : RepositoryBase<InvestigationBilling>, IInvestigationBillingRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        
        public InvestigationBillingRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public InvestigationBilling GetPatDetails(string uin)

        {
            var invv = new InvestigationBilling();

            invv.Investigation = new InvestigationImages();
            invv.INV = new List<InvestigationImages>();
            invv.BillDetails = new List<BillDetails>();
            invv.PaymentMaster = new List<Payment_Master>();


            var invpre = WYNKContext.InvestigationPrescription.ToList();
            var use = CMPSContext.Users.ToList();
            var docmas = CMPSContext.DoctorMaster.ToList();
            var invim = WYNKContext.InvestigationImages.ToList();

            //var billtype = CMPSContext.Setup.Where(x => x.CMPID == cmpid).Select(x => x.BillingType).FirstOrDefault();

            //if (billtype != "Consolidated Billing")
            //{

                var bd = (from IP in invpre.Where(x => x.isbilled == false && x.UIN == uin && x.Tag =="O")
                          join IM in invim.Where(x => x.ExternalInternal == "Internal")
                          on IP.RegistrationTranID equals IM.RegistrationTranID
                          join US in use
                          on IP.CreatedBy equals US.Userid
                          join DOC in docmas
                          on US.Emailid equals DOC.EmailID



                          select new
                          {
                              rid = IP.RegistrationTranID,
                              ipid = IP.RandomUniqueID,
                              PrescribedDate = IP.CreatedUTC,
                              PrescribedBy = DOC.Firstname + " " + DOC.MiddleName + " " + DOC.LastName,
                              Remarks = IP.Remarks,
                          }

                              ).ToList();


                invv.PatientBillDetails = (from res in bd.GroupBy(x => x.rid)




                                           select new PatientBillDetails
                                           {
                                               ipid = res.Select(x => x.ipid).LastOrDefault(),
                                               rid = res.Select(x => x.rid).LastOrDefault(),
                                               PrescribedDate = res.Select(x => x.PrescribedDate).LastOrDefault(),
                                               PrescribedBy = res.Select(x => x.PrescribedBy).LastOrDefault(),
                                               Remarks = res.Select(x => x.Remarks).LastOrDefault(),
                                           }

                                  ).ToList();

           // }


            return invv;


        }







        public InvestigationBilling GetBillingDetails(string ipid, int TaxID)

        {
            var invv = new InvestigationBilling();
            invv.Investigation = new InvestigationImages();
            invv.INV = new List<InvestigationImages>();
            invv.BillDetails = new List<BillDetails>();
            invv.PaymentMaster = new List<Payment_Master>();

            var invpre = WYNKContext.InvestigationPrescription.ToList();
            var invtr = WYNKContext.InvestigationPrescriptionTran.ToList();
            var one = WYNKContext.Services.ToList();
            var taxMaster = CMPSContext.TaxMaster.ToList();
            var servicemaster = WYNKContext.ServiceMaster.ToList();

            decimal? GST = taxMaster.Where(x => x.ID == TaxID).Select(x => x.GSTPercentage).FirstOrDefault();
            var CESS = taxMaster.Where(x => x.ID == TaxID).Select(x => x.CESSPercentage).FirstOrDefault();
            var AdditionalCESS = taxMaster.Where(x => x.ID == TaxID).Select(x => x.AdditionalCESSPercentage).FirstOrDefault();
            invv.BillDetails = (from IP in invpre.Where(x => x.RandomUniqueID == ipid)
                                join IM in invtr
                                on IP.RandomUniqueID equals IM.IPID
                                join OLM in one
                                on IM.InvestigationID equals OLM.ID
                                select new BillDetails
                                       {
                                    Investigation = OLM.Description,
                                    ipid = IP.RandomUniqueID,
                                    iptid = IM.ID,
                                    invid = IM.InvestigationID,
                                    Amount = servicemaster.Where(x => x.ChildDescription == Convert.ToString(OLM.ID)).Select(x => x.TotalAmount).FirstOrDefault(),
                                    GrossAmount = servicemaster.Where(x => x.ChildDescription == Convert.ToString(OLM.ID)).Select(x => x.TotalAmount).FirstOrDefault(),
                                    TotalCost = servicemaster.Where(x => x.ChildDescription == Convert.ToString(OLM.ID)).Select(x => x.TotalAmount).FirstOrDefault(),
                                }

                              ).ToList();




            return invv;


        }




        public InvestigationBilling GetBillingTaxDetails(string ipid, int TaxID, int iptrid)

        {
            var invv = new InvestigationBilling();
            invv.Investigation = new InvestigationImages();
            invv.INV = new List<InvestigationImages>();
            invv.BillDetails = new List<BillDetails>();
            invv.PaymentMaster = new List<Payment_Master>();

            var invpre = WYNKContext.InvestigationPrescription.ToList();
            var invtr = WYNKContext.InvestigationPrescriptionTran.ToList();
            var one = WYNKContext.Services.ToList();
            var taxMaster = CMPSContext.TaxMaster.ToList();
            var servicemaster = WYNKContext.ServiceMaster.ToList();

            decimal? GST = taxMaster.Where(x => x.ID == TaxID).Select(x => x.GSTPercentage).FirstOrDefault();
            var CESS = taxMaster.Where(x => x.ID == TaxID).Select(x => x.CESSPercentage).FirstOrDefault();
            var AdditionalCESS = taxMaster.Where(x => x.ID == TaxID).Select(x => x.AdditionalCESSPercentage).FirstOrDefault();
            invv.BillDetails = (from IP in invpre.Where(x => x.RandomUniqueID == ipid)
                                join IM in invtr.Where(x => x.ID == iptrid) on IP.RandomUniqueID equals IM.IPID
                                join OLM in one on IM.InvestigationID equals OLM.ID
                                select new BillDetails
                                {
                                    Investigation = OLM.Description,
                                    ipid = IP.RandomUniqueID,
                                    iptid = IM.ID,
                                    invid = IM.InvestigationID,
                                    Amount = servicemaster.Where(x => x.ChildDescription == Convert.ToString(OLM.ID)).Select(x => x.TotalAmount).FirstOrDefault(),
                                    GrossAmount = servicemaster.Where(x => x.ChildDescription == Convert.ToString(OLM.ID)).Select(x => x.TotalAmount).FirstOrDefault(),
                                    TaxDescription = taxMaster.Where(x => x.ID == TaxID).Select(x => x.TaxDescription).FirstOrDefault(),
                                    TaxID = taxMaster.Where(x => x.ID == TaxID).Select(x => x.ID).FirstOrDefault(),
                                    CESSDescription = taxMaster.Where(x => x.ID == TaxID).Select(x => x.CESSDescription).FirstOrDefault(),
                                    AdditionalCESSDescription = taxMaster.Where(x => x.ID == TaxID).Select(x => x.AdditionalCESSDescription).FirstOrDefault(),
                                    GST = taxMaster.Where(x => x.ID == TaxID).Select(x => x.GSTPercentage).FirstOrDefault(),
                                    CESS = taxMaster.Where(x => x.ID == TaxID).Select(x => x.CESSPercentage).FirstOrDefault(),
                                    AdditionalCESS = taxMaster.Where(x => x.ID == TaxID).Select(x => x.AdditionalCESSPercentage).FirstOrDefault(),
                                    CGSTPercentage = taxMaster.Where(x => x.ID == TaxID).Select(x => x.CESSPercentage).FirstOrDefault(),
                                    SGSTPercentage = taxMaster.Where(x => x.ID == TaxID).Select(x => x.SGSTPercentage).FirstOrDefault(),
                                    IGSTPercentage = taxMaster.Where(x => x.ID == TaxID).Select(x => x.IGSTPercentage).FirstOrDefault(),
                                    GSTAmount = (servicemaster.Where(x => x.ChildDescription == Convert.ToString(OLM.ID)).Select(x => x.TotalAmount).FirstOrDefault()) * (GST / 100),
                                    CESSAmount = (servicemaster.Where(x => x.ChildDescription == Convert.ToString(OLM.ID)).Select(x => x.TotalAmount).FirstOrDefault()) * (CESS / 100),
                                    AdditionalCESSAmount = (servicemaster.Where(x => x.ChildDescription == Convert.ToString(OLM.ID)).Select(x => x.TotalAmount).FirstOrDefault()) * (AdditionalCESS / 100),
                                    TotalCost = (servicemaster.Where(x => x.ChildDescription == Convert.ToString(OLM.ID)).Select(x => x.TotalAmount).FirstOrDefault()) + (servicemaster.Where(x => x.ChildDescription == Convert.ToString(OLM.ID)).Select(x => x.TotalAmount).FirstOrDefault()) * ((GST / 100) + (CESS / 100) + (AdditionalCESS / 100)),
                                }

                              ).ToList();




            return invv;


        }




        public dynamic UpdateInvBilling(InvestigationBilling InvestigationBilling, int cmpPid, int TransactionTypeid)
        {

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var ibm = new InvestigationBillMaster();


                    ibm.CMPID = InvestigationBilling.InvestigationBillMaster.CMPID;
                    ibm.TransactionId = TransactionTypeid;
                    ibm.RegistrationTranID = InvestigationBilling.InvestigationBillMaster.RegistrationTranID;
                    ibm.UIN = InvestigationBilling.InvestigationBillMaster.UIN;
                    ibm.BillNo = InvestigationBilling.InvestigationBillMaster.BillNo;
                    ibm.GrossProductValue = InvestigationBilling.InvestigationBillMaster.GrossProductValue;
                    ibm.TotalDiscountValue = InvestigationBilling.InvestigationBillMaster.TotalDiscountValue;
                    ibm.TotalTaxValue = InvestigationBilling.InvestigationBillMaster.TotalTaxValue;
                    ibm.TotalCGSTTaxValue = (InvestigationBilling.InvestigationBillMaster.TotalTaxValue) / 2;
                    ibm.TotalSGSTTaxValue = (InvestigationBilling.InvestigationBillMaster.TotalTaxValue) / 2;
                    ibm.TotalBillValue = InvestigationBilling.InvestigationBillMaster.TotalBillValue;
                    ibm.CESSAmount = InvestigationBilling.InvestigationBillMaster.CESSAmount;
                    ibm.AdditionalCESSAmount = InvestigationBilling.InvestigationBillMaster.AdditionalCESSAmount;

                    ibm.CreatedUTC = DateTime.UtcNow;
                    ibm.CreatedBy = InvestigationBilling.InvestigationBillMaster.CreatedBy;
                    ibm.UpdatedBy = 0;
                    WYNKContext.InvestigationBillMaster.Add(ibm);

                    string cmpname = CMPSContext.Company.Where(x => x.CmpID == InvestigationBilling.InvestigationBillMaster.CMPID).Select(x => x.CompanyName).FirstOrDefault();
                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == InvestigationBilling.InvestigationBillMaster.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    string userid = Convert.ToString(InvestigationBilling.InvestigationBillMaster.CreatedBy);
                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(cmpname, "Investigation Billing", "User name :", username, "User ID :", userid, "Mode : Sumbit");

                    object names = ibm;
                    oErrorLogs.WriteErrorLogArray("InvestigationBillMaster", names);

                    WYNKContext.SaveChanges();
                    var crtby = ibm.CreatedBy;
                    var trid = ibm.TransactionId;
                    var uin = ibm.UIN;
                    var bilno = ibm.BillNo;
                    var cid = ibm.CMPID;
                    var rid = ibm.RegistrationTranID;
                    var gp = ibm.GrossProductValue;


                    if (InvestigationBilling.BillDetails.Count() > 0)
                    {
                        foreach (var item in InvestigationBilling.BillDetails.ToList())

                        {

                            var ibt = new InvestigationBillTran();


                            var billid = ibm.ID;

                            ibt.InvestigationBillID = billid;
                            ibt.InvPrescriptionID = item.ipid;
                            ibt.InvPrescriptionTranId = item.iptid;
                            ibt.InvestigationID = item.invid;
                            ibt.Amount = item.Amount;
                            ibt.DiscountPercentage = item.Discount;
                            ibt.DiscountAmount = item.DiscountAmount;
                            ibt.GSTPercentage = item.GST;
                            ibt.GSTTaxValue = item.GSTAmount;
                            ibt.CGSTPercentage = (item.GST) / 2;
                            ibt.CGSTTaxValue = (item.GSTAmount) / 2;
                            ibt.SGSTPercentage = (item.GST) / 2;
                            ibt.SGSTTaxValue = (item.GSTAmount) / 2;
                            ibt.CESSPercentage = item.CESS;
                            ibt.CESSAmount = item.CESSAmount;
                            ibt.AdditionalCESSPercentage = item.AdditionalCESS;
                            ibt.AdditionalCESSAmount = item.AdditionalCESSAmount;
                            ibt.CreatedUTC = DateTime.UtcNow;
                            ibt.CreatedBy = crtby;
                            ibt.UpdatedBy = 0;
                            ibt.TaxID = item.TaxID;
                            WYNKContext.InvestigationBillTran.AddRange(ibt);
                            ErrorLog oErrorLogstran = new ErrorLog();
                            object namestr = ibt;
                            oErrorLogstran.WriteErrorLogArray("InvestigationBillTran", namestr);
                            WYNKContext.SaveChanges();

                            var invprid = ibt.InvPrescriptionID;

                        }

                    }





                    if (InvestigationBilling.PaymentMaster.Count() > 0)
                    {
                        foreach (var item in InvestigationBilling.PaymentMaster.ToList())

                        {

                            var pm = new Payment_Master();


                            var billid = ibm.ID;

                            pm.UIN = uin;
                            pm.PaymentType = "O";
                            pm.PaymentMode = item.PaymentMode;
                            pm.InstrumentNumber = item.InstrumentNumber;
                            pm.Instrumentdate = item.Instrumentdate;
                            pm.BankName = item.BankName;
                            pm.BankBranch = item.BankBranch;
                            pm.Expirydate = item.Expirydate;
                            pm.Amount = item.Amount;
                            pm.IsBilled = true;
                            var Datee = DateTime.Now;
                            pm.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(s => s.ID == WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Datee && Convert.ToDateTime(x.FYTo) >= Datee && x.CMPID == cmpPid && x.IsActive == true).Select(x => x.ID).FirstOrDefault()).Select(s => s.FYAccYear).FirstOrDefault());
                            pm.PaymentReferenceID = billid;
                            pm.InVoiceNumber = bilno;
                            pm.InVoiceDate = DateTime.Now.Date;
                            pm.ReceiptNumber = bilno;
                            pm.ReceiptDate = DateTime.Now.Date;
                            pm.CreatedUTC = DateTime.UtcNow;
                            pm.CreatedBy = crtby;
                            pm.TransactionID = trid;
                            pm.CmpID = cid;
                            pm.IsCancelled = false;

                            WYNKContext.PaymentMaster.AddRange(pm);
                            ErrorLog oErrorLogspay = new ErrorLog();
                            object namepay = pm;
                            oErrorLogspay.WriteErrorLogArray("PaymentMaster", namepay);
                            WYNKContext.SaveChanges();



                        }

                    }




                    var master = WYNKContext.InvestigationPrescription.Where(x => x.RegistrationTranID == rid).ToList();
                    if (master != null)
                    {

                        master.All(x => { x.isbilled = true; return true; });
                        WYNKContext.InvestigationPrescription.UpdateRange(master);
                    }



                    if (InvestigationBilling.BillDetails.Count() > 0)
                    {
                        foreach (var item in InvestigationBilling.BillDetails.ToList())

                        {
                            var taxidd = CMPSContext.TaxMaster.Where(x => x.ID == item.TaxID).ToList();
                            var taxgrp = CMPSContext.TaxMaster.Where(x => x.ID == item.TaxID).Select(x=>x.TaxGroupId).FirstOrDefault();
                            if (taxidd != null && taxidd.Count != 0) {
                                taxidd.All(x => { x.istransact = false; return true; });
                                CMPSContext.TaxMaster.UpdateRange(taxidd);
                                CMPSContext.SaveChanges();
                            }

                            var now = DateTime.UtcNow;
                            var first = new DateTime(now.Year, now.Month, 1);
                            var last = first.AddMonths(1).AddDays(-1);

                            var taid = (from TS in WYNKContext.TaxSummary.Where(x => x.TransactionDate == last && x.CMPID == cmpPid && x.BillingType == "B" && x.TaxID == item.TaxID)
                                        select new
                                        {
                                            ret = TS.TaxID,

                                        }).ToList();
                            if (taid.Count == 0)
                            {
                                var tax = new Tax_Summary();
                                tax.TaxID = item.TaxID;
                                tax.CMPID = cmpPid;
                                tax.BillingType = "B";
                                tax.TransactionDate = last;
                                tax.TaxGroupID = taxgrp;
                                tax.GrossAmount = gp;
                                tax.TaxDescription = item.TaxDescription;
                                tax.CESSDescription = item.CESSDescription;
                                tax.AddlCESSDescription = item.AdditionalCESSDescription;
                                tax.TaxPercentage = CMPSContext.TaxMaster.Where(x => x.ID == item.TaxID).Select(x => x.GSTPercentage).FirstOrDefault();
                                tax.TaxValue = getvalue(item.GSTAmount);
                                tax.Tax1Percentage = Convert.ToInt16((CMPSContext.TaxMaster.Where(x => x.ID == item.TaxID).Select(x => x.GSTPercentage).FirstOrDefault()) / 2);
                                tax.Tax1Value = getvalue(item.GSTAmount / 2);
                                tax.Tax2Percentage = Convert.ToInt16((CMPSContext.TaxMaster.Where(x => x.ID == item.TaxID).Select(x => x.GSTPercentage).FirstOrDefault()) / 2);
                                tax.Tax2Value = getvalue(item.GSTAmount / 2);
                                tax.CESSPercentage = item.CESS;
                                tax.CESSValue = getvalue(item.CESSAmount);
                                tax.AddlCESSPercentage = item.AdditionalCESS;
                                tax.AddlCESSValue = getvalue(item.AdditionalCESSAmount);
                                tax.CreatedUTC = DateTime.UtcNow;
                                tax.CreatedBy = crtby;
                                WYNKContext.TaxSummary.AddRange(tax);
                                WYNKContext.SaveChanges();
                            }
                            else {

                                var masters = WYNKContext.TaxSummary.Where(x => x.TransactionDate == last && x.CMPID == cmpPid && x.BillingType == "B" && x.TaxID == item.TaxID).LastOrDefault();
                                masters.GrossAmount += gp;
                                masters.TaxValue += item.GSTAmount;
                                masters.Tax1Value += item.GSTAmount/2;
                                masters.Tax2Value += item.GSTAmount/2;
                                masters.CESSValue += item.CESSAmount;
                                masters.AddlCESSValue += item.AdditionalCESSAmount;
                                masters.UpdatedUTC = DateTime.UtcNow;
                                masters.UpdatedBy = crtby;
                                WYNKContext.TaxSummary.UpdateRange(masters);

                            }

                        }

                       

                    }





                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

                    var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                    var RunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpPid, "GetRunningNo");

                    if (RunningNumber == InvestigationBilling.InvestigationBillMaster.BillNo)
                    {
                        commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpPid, "UpdateRunningNo");
                    }
                    else
                    {
                        var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpPid, "UpdateRunningNo");

                        var ib = WYNKContext.InvestigationBillMaster.Where(x => x.BillNo == InvestigationBilling.InvestigationBillMaster.BillNo).FirstOrDefault();
                        ib.BillNo = GetRunningNumber;
                        WYNKContext.InvestigationBillMaster.UpdateRange(ib);

                        WYNKContext.SaveChanges();
                    }



                    if (WYNKContext.SaveChanges() >= 0) {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }
                        return new
                        {

                            Success = true,
                            Message = CommonMessage.saved,
                            ibid = ibm.ID,
                            rid = ibm.RegistrationTranID,
                            bill = ibm.BillNo,
                        };
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                    Console.Write(ex);
                }

            }
            return new
            {
                Success = false,
                Message = CommonMessage.Missing,
            };

        }

        public decimal? getvalue(decimal? data)
        {
            var a = 0;
            var b = data;
            if (b != null)
            {
                return b;
            }
            return a;
        }

        public InvestigationBilling GetInvesprint(int id, int rid, int cid, int tid)

        {

            var op = new InvestigationBilling();
            op.InvestigationBillTran = new List<InvestigationBillTran>();
            op.InvestigationBillMaster = new InvestigationBillMaster();
            op.PaymentMaster = new List<Payment_Master>();

            var taxMaster = CMPSContext.TaxMaster.ToList();

            op.InvestigationBillMaster.BillNo = WYNKContext.InvestigationBillMaster.Where(x => x.ID == id).Select(x => x.BillNo).FirstOrDefault();
            op.InvestigationBillMaster.CreatedUTC = WYNKContext.InvestigationBillMaster.Where(x => x.ID == id).Select(x => x.CreatedUTC).FirstOrDefault();

            op.InvestigationBillMaster.GrossProductValue = WYNKContext.InvestigationBillMaster.Where(x => x.ID == id).Select(x => x.GrossProductValue).FirstOrDefault();
            op.InvestigationBillMaster.TotalDiscountValue = WYNKContext.InvestigationBillMaster.Where(x => x.ID == id).Select(x => x.TotalDiscountValue).FirstOrDefault();
            op.InvestigationBillMaster.TotalTaxValue = WYNKContext.InvestigationBillMaster.Where(x => x.ID == id).Select(x => x.TotalTaxValue).FirstOrDefault();
            op.InvestigationBillMaster.CESSAmount = WYNKContext.InvestigationBillMaster.Where(x => x.ID == id).Select(x => x.CESSAmount).FirstOrDefault();
            op.InvestigationBillMaster.AdditionalCESSAmount = WYNKContext.InvestigationBillMaster.Where(x => x.ID == id).Select(x => x.AdditionalCESSAmount).FirstOrDefault();


            op.InvestigationBillMaster.TotalBillValue = WYNKContext.InvestigationBillMaster.Where(x => x.ID == id).Select(x => x.TotalBillValue).FirstOrDefault();
            var uin = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == rid).Select(x => x.UIN).FirstOrDefault();

            op.Registration = WYNKContext.Registration.Where(x => x.UIN == uin).FirstOrDefault();

            op.page = DateTime.Now.Year - op.Registration.DateofBirth.Year;


            op.CAddress = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.Address1).FirstOrDefault();

            op.Cphone = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.Phone1).FirstOrDefault();

            op.Cweb = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.Website).FirstOrDefault();

            op.Cname = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.CompanyName).FirstOrDefault();


            var onelinemas = WYNKContext.Services.ToList();

            op.InvestDetails = (from om in WYNKContext.InvestigationBillMaster.Where(x => x.ID == id && x.CMPID == cid)
                        join ot in WYNKContext.InvestigationBillTran on om.ID equals ot.InvestigationBillID
                        join olm in onelinemas on ot.InvestigationID equals olm.ID

                        select new InvestDetails
                        {
                            Description = olm.Description,
                            
                            value = ot.Amount,
                            discount = ot.DiscountPercentage,
                            dvalue = ot.DiscountAmount,
                            gst = ot.GSTPercentage,
                            gstvalue = ot.GSTTaxValue,
                            cess =ot.CESSPercentage,
                            cessvalue=ot.CESSAmount,
                            addcess = ot.AdditionalCESSPercentage,
                            addcessvalue= ot.AdditionalCESSAmount,
                            total = (ot.Amount + ot.GSTTaxValue + ot.CESSAmount + ot.AdditionalCESSAmount) - (ot.DiscountAmount),
                            TaxDescription = taxMaster.Where(v => v.CGSTPercentage == ot.GSTPercentage/2).Select(v=>v.TaxDescription).FirstOrDefault(),
                            CessDescription = taxMaster.Where(v => v.CGSTPercentage == ot.GSTPercentage/2).Select(v => v.CESSDescription).FirstOrDefault(),
                            AddcessDescription = taxMaster.Where(v => v.CGSTPercentage == ot.GSTPercentage/2).Select(v => v.AdditionalCESSDescription).FirstOrDefault(),

                        }).ToList();



            op.PayDetailss = (from om in WYNKContext.InvestigationBillMaster.Where(x => x.ID == id && x.CMPID == cid)
                             join pm in WYNKContext.PaymentMaster.Where(x => x.TransactionID == tid)
                             on om.ID equals pm.PaymentReferenceID

                             select new PayDetailss
                             {
                                 paymode = pm.PaymentMode,
                                 instno = pm.InstrumentNumber,
                                 instdt = pm.Instrumentdate,
                                 bname = pm.BankName,
                                 branch = pm.BankBranch,
                                 expiry = pm.Expirydate,
                                 amount = pm.Amount,

                             }).ToList();


            return op;
        }






        public InvestigationBilling Getreprint(int oid, int rid, int cid, int tid)

        {


            var op = new InvestigationBilling();
            op.InvestigationBillTran = new List<InvestigationBillTran>();
            op.InvestigationBillMaster = new InvestigationBillMaster();
            op.PaymentMaster = new List<Payment_Master>();

            var taxMaster = CMPSContext.TaxMaster.ToList();

            op.InvestigationBillMaster.BillNo = WYNKContext.InvestigationBillMaster.Where(x => x.ID == oid).Select(x => x.BillNo).FirstOrDefault();
            op.InvestigationBillMaster.CreatedUTC = WYNKContext.InvestigationBillMaster.Where(x => x.ID == oid).Select(x => x.CreatedUTC).FirstOrDefault();

            op.InvestigationBillMaster.GrossProductValue = WYNKContext.InvestigationBillMaster.Where(x => x.ID == oid).Select(x => x.GrossProductValue).FirstOrDefault();
            op.InvestigationBillMaster.TotalDiscountValue = WYNKContext.InvestigationBillMaster.Where(x => x.ID == oid).Select(x => x.TotalDiscountValue).FirstOrDefault();
            op.InvestigationBillMaster.TotalTaxValue = WYNKContext.InvestigationBillMaster.Where(x => x.ID == oid).Select(x => x.TotalTaxValue).FirstOrDefault();
            op.InvestigationBillMaster.CESSAmount = WYNKContext.InvestigationBillMaster.Where(x => x.ID == oid).Select(x => x.CESSAmount).FirstOrDefault();
            op.InvestigationBillMaster.AdditionalCESSAmount = WYNKContext.InvestigationBillMaster.Where(x => x.ID == oid).Select(x => x.AdditionalCESSAmount).FirstOrDefault();


            op.InvestigationBillMaster.TotalBillValue = WYNKContext.InvestigationBillMaster.Where(x => x.ID == oid).Select(x => x.TotalBillValue).FirstOrDefault();
            var uin = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == rid).Select(x => x.UIN).FirstOrDefault();

            op.Registration = WYNKContext.Registration.Where(x => x.UIN == uin).FirstOrDefault();

            op.rpage = DateTime.Now.Year - op.Registration.DateofBirth.Year;


            op.rCAddress = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.Address1).FirstOrDefault();

            op.rCphone = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.Phone1).FirstOrDefault();

            op.rCweb = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.Website).FirstOrDefault();

            op.rCname = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.CompanyName).FirstOrDefault();

            var onelinemas = WYNKContext.Services.ToList();

            op.InvestDetailsp = (from om in WYNKContext.InvestigationBillMaster.Where(x => x.ID == oid && x.CMPID == cid)
                                join ot in WYNKContext.InvestigationBillTran on om.ID equals ot.InvestigationBillID
                                join olm in onelinemas on ot.InvestigationID equals olm.ID

                                select new InvestDetailsp
                                {
                                    Descriptionp = olm.Description,

                                    valuep = ot.Amount,
                                    discountp = ot.DiscountPercentage,
                                    dvaluep = ot.DiscountAmount,
                                    gstp = ot.GSTPercentage,
                                    gstvaluep = ot.GSTTaxValue,
                                    cessp = ot.CESSPercentage,
                                    cessvaluep = ot.CESSAmount,
                                    addcessp = ot.AdditionalCESSPercentage,
                                    addcessvaluep = ot.AdditionalCESSAmount,
                                    totalp = (ot.Amount + ot.GSTTaxValue + ot.CESSAmount + ot.AdditionalCESSAmount) - (ot.DiscountAmount),
                                    TaxDescriptionp = taxMaster.Where(v => v.CGSTPercentage == ot.GSTPercentage / 2).Select(v => v.TaxDescription).FirstOrDefault(),
                                    CessDescriptionp = taxMaster.Where(v => v.CGSTPercentage == ot.GSTPercentage / 2).Select(v => v.CESSDescription).FirstOrDefault(),
                                    AddcessDescriptionp = taxMaster.Where(v => v.CGSTPercentage == ot.GSTPercentage / 2).Select(v => v.AdditionalCESSDescription).FirstOrDefault(),

                                }).ToList();





            op.PayDetailssp = (from om in WYNKContext.InvestigationBillMaster.Where(x => x.ID == oid && x.CMPID == cid)
                              join pm in WYNKContext.PaymentMaster.Where(x => x.TransactionID == tid)
                              on om.ID equals pm.PaymentReferenceID

                              select new PayDetailssp
                              {
                                  paymodep = pm.PaymentMode,
                                  instnop = pm.InstrumentNumber,
                                  instdtp = pm.Instrumentdate,
                                  bnamep = pm.BankName,
                                  branchp = pm.BankBranch,
                                  expiryp = pm.Expirydate,
                                  amountp = pm.Amount,

                              }).ToList();


            return op;
        }





        public InvestigationBilling GetReBillingDetails(int id)

        {

            var op = new InvestigationBilling();
            op.InvestigationBillTran = new List<InvestigationBillTran>();
            op.PaymentMaster = new List<Payment_Master>();
            op.InvestigationBillMaster = new InvestigationBillMaster();
            op.ReBillingDetails = (from opn in WYNKContext.InvestigationBillMaster
                                 join rt in WYNKContext.RegistrationTran on opn.RegistrationTranID equals rt.RegistrationTranID
                                 join rm in WYNKContext.Registration on opn.UIN equals rm.UIN
                                 select new ReBillingDetails
                                 {
                                     UIN = rm.UIN,
                                     Oid = opn.ID,
                                     ridd = opn.RegistrationTranID,
                                     Name = rm.Name + " " + rm.LastName,
                                     BillNo = opn.BillNo,
                                     BillDate = opn.CreatedUTC.Date,                                    
                                 }).ToList();

            return op;

        }


        public InvestigationBilling GetTaxDetails()

        {
            var getData = new InvestigationBilling();
            var TaxMaster = CMPSContext.TaxMaster.ToList();
            getData.GetTaxDetails = (from tax in TaxMaster.Where(x => x.IsActive == true)

                                     select new GetTaxDetails
                                     {
                                         TaxID = tax.ID,
                                         TaxDescription = tax.TaxDescription,
                                         CESSDescription = tax.CESSDescription,
                                         AdditionalCESSDescription = tax.AdditionalCESSDescription,
                                         GSTPercentage = tax.GSTPercentage,
                                         CESSPercentage = tax.CESSPercentage,
                                         AdditionalCESSPercentage = tax.AdditionalCESSPercentage,
                                         CGSTPercentage = tax.CGSTPercentage,
                                         SGSTPercentage = tax.SGSTPercentage,
                                         IGSTPercentage = tax.IGSTPercentage,
                                     }).ToList();


            return getData;

        }




        public InvestigationBilling Getprint(string billno, int cid, int tid)

        {

            var op = new InvestigationBilling();
            op.PaymentMaster = new List<Payment_Master>();

            var uin = WYNKContext.PaymentMaster.Where(x => x.InVoiceNumber == billno && x.CmpID == cid && x.TransactionID == tid).Select(x => x.UIN).FirstOrDefault();

            op.Registration = WYNKContext.Registration.Where(x => x.UIN == uin).FirstOrDefault();

            op.page = DateTime.Now.Year - op.Registration.DateofBirth.Year;


            op.CAddress = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.Address1).FirstOrDefault();

            op.Cphone = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.Phone1).FirstOrDefault();

            op.Cweb = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.Website).FirstOrDefault();

            op.Cname = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.CompanyName).FirstOrDefault();


            var onelinemas = CMPSContext.OneLineMaster.ToList();          


            op.PayDetailss = (from pm in WYNKContext.PaymentMaster.Where(x => x.TransactionID == tid && x.CmpID == cid && x.InVoiceNumber == billno)
                              
                              select new PayDetailss
                              {
                                  paymode = pm.PaymentMode,
                                  instno = pm.InstrumentNumber,
                                  instdt = pm.Instrumentdate,
                                  bname = pm.BankName,
                                  branch = pm.BankBranch,
                                  expiry = pm.Expirydate,
                                  amount = pm.Amount,
                                  tamount =+ pm.Amount,
                                  bilnum = pm.InVoiceNumber,
                                  bildtt = pm.CreatedUTC,

                              }).ToList();


            return op;
        }

    }
}







