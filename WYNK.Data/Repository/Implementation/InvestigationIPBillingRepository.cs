
using System;
using System.Collections.Generic;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class InvestigationIPBillingRepository : RepositoryBase<InvestigationIPBilling>, IInvestigationIPBillingRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        
        public InvestigationIPBillingRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public InvestigationIPBilling GetRegINVIPBILLDetails(int Cmpid)

        {
            var getData = new InvestigationIPBilling();
            var registration = WYNKContext.Registration.Where(x=>x.CMPID==Cmpid).ToList();
            var Admission = WYNKContext.Admission.Where(x => x.CMPID == Cmpid && x.DischargeID == null).ToList();
            var invP = WYNKContext.InvestigationPrescription.Where(x => x.CMPID == Cmpid && x.isbilled==false).ToList();            
            var InvestigationImages = WYNKContext.InvestigationImages.Where(x => x.CmpID == Cmpid && x.ExternalInternal== "Internal").GroupBy(x => x.UIN).ToList();
            getData.GetRegDetails = (from REG in registration
                                     join Adm in Admission on REG.UIN equals Adm.UIN
                                     join invP1 in invP on REG.UIN equals invP1.UIN
                                     join INVIMG in InvestigationImages on REG.UIN equals INVIMG.Key
                                     select new GetRegDetails
                                     {
                                         UIN = REG.UIN,
                                         Name = REG.Name,
                                         MiddleName = REG.MiddleName,
                                         LastName = REG.LastName,
                                         DateofBirth = REG.DateofBirth,
                                         Gender = REG.Gender,
                                         Age = PasswordEncodeandDecode.ToAgeString(REG.DateofBirth),

                                     }).ToList();


            return getData;

        }


        public InvestigationIPBilling GetRegINVBILLDetails(int Cmpid)

        {
            var getData = new InvestigationIPBilling();
            var billtype = CMPSContext.Setup.Where(x => x.CMPID == Cmpid).Select(x => x.BillingType).FirstOrDefault();

            if (billtype != "Consolidated Billing")
            {
                var registration = WYNKContext.Registration.ToList();
                var registrationtran = WYNKContext.RegistrationTran.ToList();
                var InvestigationImages = WYNKContext.InvestigationImages.Where(x => x.CmpID == Cmpid && x.ExternalInternal == "Internal").GroupBy(x => x.UIN).ToList();
                var invP = WYNKContext.InvestigationPrescription.Where(x => x.CMPID == Cmpid && x.isbilled == false && x.Tag == "O").ToList();
                getData.GetRegDetailsop = (from REG in registration
                                           join inv in invP on REG.UIN equals inv.UIN
                                           join INVIMG in InvestigationImages on REG.UIN equals INVIMG.Key
                                           select new GetRegDetailsop
                                           {
                                               UIN = REG.UIN,
                                               Name = REG.Name,
                                               MiddleName = REG.MiddleName,
                                               LastName = REG.LastName,
                                               DateofBirth = REG.DateofBirth,
                                               Gender = REG.Gender,
                                               Age = PasswordEncodeandDecode.ToAgeString(REG.DateofBirth),


                                           }).ToList();

            }
            return getData;

        }


        public InvestigationIPBilling GetInvIPDetails(string uin, int cmpid)

        {
            var invvIP = new InvestigationIPBilling();

            invvIP.Investigation = new InvestigationImages();
            invvIP.INV = new List<InvestigationImages>();
            invvIP.PaymentMaster = new List<Payment_Master>();

            var invpre = WYNKContext.InvestigationPrescription.Where(x=>x.CMPID== cmpid).ToList();
            var use = CMPSContext.Users.Where(x => x.CMPID == cmpid).ToList();
            var docmas = CMPSContext.DoctorMaster.Where(x => x.CMPID == cmpid).ToList();
            var invim = WYNKContext.InvestigationImages.Where(x => x.CmpID == cmpid).ToList();


            var bd = (from IP in invpre.Where(x => x.isbilled == false && x.UIN == uin)
                      join IM in invim.Where(x => x.ExternalInternal == "Internal") on IP.RegistrationTranID equals IM.RegistrationTranID
                      join US in use on IP.CreatedBy equals US.Userid
                      join DOC in docmas on US.Emailid equals DOC.EmailID
                      select new
                      {
                          rid = IP.RegistrationTranID,
                          ipid = IP.RandomUniqueID,
                          PrescribedDate = IP.CreatedUTC,
                          INVPRESNO = IP.INVPRESNO,
                          PrescribedBy = String.Concat(DOC.Firstname + " " + DOC.MiddleName + " " + DOC.LastName),
                          Remarks = IP.Remarks,
                      }

                       ).ToList();


            invvIP.PatientBillDetailsIP = (from res in bd.GroupBy(x => x.rid)

                                       select new PatientBillDetailsIP
                                       {
                                           ipid = res.Select(x => x.ipid).LastOrDefault(),
                                           rid = res.Select(x => x.rid).LastOrDefault(),
                                           PrescribedDate = res.Select(x => x.PrescribedDate).LastOrDefault(),
                                           PrescribedBy = res.Select(x => x.PrescribedBy).LastOrDefault(),
                                           Remarks = res.Select(x => x.Remarks).LastOrDefault(),
                                           INVPRESNO = res.Select(x => x.INVPRESNO).LastOrDefault(),
                                       }

                              ).ToList();




            return invvIP;


        }
        public InvestigationIPBilling GetIPTaxBillingDetails(string ipid,int TaxID,int inpID)

        {
            var invv = new InvestigationIPBilling();
            invv.Investigation = new InvestigationImages();
            invv.INV = new List<InvestigationImages>();
            invv.BillDetailsIP = new List<BillDetailsIP>();
            invv.PaymentMaster = new List<Payment_Master>();
            var invpre = WYNKContext.InvestigationPrescription.ToList();
            var invtr = WYNKContext.InvestigationPrescriptionTran.ToList();
            var one = CMPSContext.OneLineMaster.ToList();         
            var taxMaster = CMPSContext.TaxMaster.ToList();
            decimal? GST = taxMaster.Where(x => x.ID == TaxID).Select(x => x.GSTPercentage).FirstOrDefault();
            var CESS = taxMaster.Where(x => x.ID == TaxID).Select(x => x.CESSPercentage).FirstOrDefault();
            var AdditionalCESS = taxMaster.Where(x => x.ID == TaxID).Select(x => x.AdditionalCESSPercentage).FirstOrDefault();
            invv.BillDetailsIP = (from IP in invpre.Where(x => x.RandomUniqueID == Convert.ToString(ipid))
                                join IM in invtr.Where(x => x.ID == inpID) on IP.RandomUniqueID equals IM.IPID
                                join OLM in one on IM.InvestigationID equals OLM.OLMID

                                select new BillDetailsIP
                                {
                                    Investigation = OLM.ParentDescription,
                                    ipid = IP.RandomUniqueID,
                                    iptid = IM.ID,
                                    invid = IM.InvestigationID,
                                    Amount = OLM.Amount,
                                    GrossAmount = OLM.Amount,                    
                                    TaxDescription = taxMaster.Where(x=>x.ID==TaxID).Select(x=>x.TaxDescription).FirstOrDefault(),
                                    CESSDescription = taxMaster.Where(x => x.ID == TaxID).Select(x => x.CESSDescription).FirstOrDefault(),
                                    AdditionalCESSDescription = taxMaster.Where(x => x.ID == TaxID).Select(x => x.AdditionalCESSDescription).FirstOrDefault(),
                                    GST = taxMaster.Where(x => x.ID == TaxID).Select(x => x.GSTPercentage).FirstOrDefault(),
                                    CESS = taxMaster.Where(x => x.ID == TaxID).Select(x => x.CESSPercentage).FirstOrDefault(),
                                    AdditionalCESS = taxMaster.Where(x => x.ID == TaxID).Select(x => x.AdditionalCESSPercentage).FirstOrDefault(),
                                    CGSTPercentage = taxMaster.Where(x => x.ID == TaxID).Select(x => x.CESSPercentage).FirstOrDefault(),
                                    SGSTPercentage = taxMaster.Where(x => x.ID == TaxID).Select(x => x.SGSTPercentage).FirstOrDefault(),
                                    IGSTPercentage = taxMaster.Where(x => x.ID == TaxID).Select(x => x.IGSTPercentage).FirstOrDefault(),
                                    GSTAmount = OLM.Amount * (GST/ 100),
                                    CESSAmount = OLM.Amount * (CESS / 100),
                                    AdditionalCESSAmount = OLM.Amount * (AdditionalCESS / 100),
                                    TotalCost = OLM.Amount +(OLM.Amount)*((GST / 100)+ (CESS / 100)+ (AdditionalCESS / 100)),
                                }).ToList();

            return invv;


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

        public dynamic insertInvIPBilling(InvestigationIPBilling InvIPBilling, int cmpPid, int TransactionTypeid,string UIN)
        {

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var ibm = new InvestigationBillMaster();
                                        var respa = (from pa in WYNKContext.PatientAccount.Where(x => x.UIN == UIN && x.InvoiceNumber == null && x.CMPID == cmpPid)
                                 select new
                                 {
                                     ret = pa.UIN.Count(),

                                 }).ToList();

                    var PAID = WYNKContext.PatientAccount.Where(x => x.UIN == UIN && x.InvoiceNumber == null && x.CMPID == cmpPid).Select(x => x.PAID).LastOrDefault();
                    var ServiceTypeID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Investigation Test").Select(x => x.OLMID).FirstOrDefault();
                    var invespa = new PatientAccount();

                    if (respa.Count == 0)
                    {

                        invespa.CMPID = InvIPBilling.InvestigationBillMaster.CMPID;
                        invespa.UIN = UIN;
                        invespa.TotalProductValue = InvIPBilling.InvestigationBillMaster.GrossProductValue + InvIPBilling.InvestigationBillMaster.TotalDiscountValue;
                        invespa.TotalDiscountValue = InvIPBilling.InvestigationBillMaster.TotalDiscountValue;
                        invespa.TotalTaxValue = InvIPBilling.InvestigationBillMaster.TotalTaxValue;
                        invespa.TotalCGSTTaxValue = (InvIPBilling.InvestigationBillMaster.TotalTaxValue) / 2;
                        invespa.TotalSGSTTaxValue = (InvIPBilling.InvestigationBillMaster.TotalTaxValue) / 2;
                        invespa.TotalBillValue = InvIPBilling.InvestigationBillMaster.TotalBillValue;
                        invespa.CESSValue = InvIPBilling.InvestigationBillMaster.CESSAmount;
                        invespa.AdditionalCESSValue = InvIPBilling.InvestigationBillMaster.AdditionalCESSAmount;
                        invespa.CreatedUTC = DateTime.UtcNow;
                        invespa.CreatedBy = InvIPBilling.InvestigationBillMaster.CreatedBy;
                        WYNKContext.PatientAccount.AddRange(invespa);
                        ErrorLog oErrorLogstran = new ErrorLog();
                        object namestr = invespa;
                        oErrorLogstran.WriteErrorLogArray("PatientAccount", namestr);
                        WYNKContext.SaveChanges();

                    }

                    else
                    {

                        var masters1 = WYNKContext.PatientAccount.Where(x => x.UIN == UIN).LastOrDefault();
                        masters1.TotalProductValue += InvIPBilling.InvestigationBillMaster.GrossProductValue + InvIPBilling.InvestigationBillMaster.TotalDiscountValue;
                        masters1.TotalDiscountValue += InvIPBilling.InvestigationBillMaster.TotalDiscountValue;
                        masters1.TotalTaxValue += InvIPBilling.InvestigationBillMaster.TotalTaxValue;
                        masters1.TotalCGSTTaxValue += (InvIPBilling.InvestigationBillMaster.TotalTaxValue) / 2;
                        masters1.TotalSGSTTaxValue += (InvIPBilling.InvestigationBillMaster.TotalTaxValue) / 2;
                        masters1.TotalBillValue += InvIPBilling.InvestigationBillMaster.TotalBillValue;
                        masters1.CESSValue = InvIPBilling.InvestigationBillMaster.CESSAmount;
                        masters1.AdditionalCESSValue = InvIPBilling.InvestigationBillMaster.AdditionalCESSAmount;
                        masters1.UpdatedUTC = DateTime.UtcNow;
                        masters1.UpdatedBy = InvIPBilling.InvestigationBillMaster.CreatedBy;
                        WYNKContext.PatientAccount.UpdateRange(masters1);
                        ErrorLog oErrorLogstran = new ErrorLog();
                        object namestr = masters1;
                        oErrorLogstran.WriteErrorLogArray("PatientAccount", namestr);
                        WYNKContext.SaveChanges();
                    }
                    if (respa.Count == 0)
                    {
                        
                        PAID = invespa.PAID;
                    }
                    else
                    {

                    }
                    var respa1 = WYNKContext.PatientAccountDetail.Where(x => x.PAID == PAID && x.ServiceTypeID == ServiceTypeID).ToList();
                        if (respa1.Count == 0)
                       {                        
                        var patactdt = new PatientAccountDetail();
                        patactdt.PAID = PAID;
                        patactdt.OLMID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Investigation Test").Select(x => x.OLMID).FirstOrDefault();
                        patactdt.ServiceTypeID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Investigation Test").Select(x => x.ParentID).FirstOrDefault();
                        patactdt.ServiceDescription = "Investigation Test";
                        patactdt.TotalProductValue = InvIPBilling.InvestigationBillMaster.GrossProductValue + InvIPBilling.InvestigationBillMaster.TotalDiscountValue;
                        patactdt.TotalDiscountValue = InvIPBilling.InvestigationBillMaster.TotalDiscountValue;
                        patactdt.TotalTaxValue = InvIPBilling.InvestigationBillMaster.TotalTaxValue;
                        patactdt.TotalCGSTTaxValue = (InvIPBilling.InvestigationBillMaster.TotalTaxValue) / 2;
                        patactdt.TotalSGSTTaxValue = (InvIPBilling.InvestigationBillMaster.TotalTaxValue) / 2;
                        patactdt.TotalBillValue = InvIPBilling.InvestigationBillMaster.TotalBillValue;
                        patactdt.CESSValue = InvIPBilling.InvestigationBillMaster.CESSAmount;
                        patactdt.AdditonalCESSValue = InvIPBilling.InvestigationBillMaster.AdditionalCESSAmount;
                        patactdt.CreatedUTC = DateTime.UtcNow;
                        patactdt.CreatedBy = InvIPBilling.InvestigationBillMaster.CreatedBy;
                        WYNKContext.PatientAccountDetail.AddRange(patactdt);
                        ErrorLog oErrorLogstran = new ErrorLog();
                        object namestr = patactdt;
                        oErrorLogstran.WriteErrorLogArray("PatientAccountDetail", namestr);
                        WYNKContext.SaveChanges();
                    }
                    else
                    {
                        var masters = WYNKContext.PatientAccountDetail.Where(x => x.PAID == PAID && x.ServiceTypeID == ServiceTypeID).LastOrDefault();
                        masters.TotalProductValue += InvIPBilling.InvestigationBillMaster.GrossProductValue + InvIPBilling.InvestigationBillMaster.TotalDiscountValue;
                        masters.TotalDiscountValue += InvIPBilling.InvestigationBillMaster.TotalDiscountValue;
                        masters.TotalTaxValue += InvIPBilling.InvestigationBillMaster.TotalTaxValue;
                        masters.TotalCGSTTaxValue += (InvIPBilling.InvestigationBillMaster.TotalTaxValue) / 2;
                        masters.TotalSGSTTaxValue += (InvIPBilling.InvestigationBillMaster.TotalTaxValue) / 2;
                        masters.TotalBillValue += InvIPBilling.InvestigationBillMaster.TotalBillValue;
                        masters.CESSValue += InvIPBilling.InvestigationBillMaster.CESSAmount;
                        masters.AdditonalCESSValue += InvIPBilling.InvestigationBillMaster.AdditionalCESSAmount;
                        masters.UpdatedUTC = DateTime.UtcNow;
                        masters.UpdatedBy = InvIPBilling.InvestigationBillMaster.CreatedBy;
                        WYNKContext.PatientAccountDetail.UpdateRange(masters);
                        ErrorLog oErrorLogstran = new ErrorLog();
                        object namestr = masters;
                        oErrorLogstran.WriteErrorLogArray("PatientAccountDetail", namestr);

                    }
                    var PAccDetailID = WYNKContext.PatientAccountDetail.Where(x => x.PAID == PAID && x.ServiceTypeID == ServiceTypeID).Select(x => x.PAccDetailID).LastOrDefault();

                    foreach (var item in InvIPBilling.BillDetailsIP.ToList())

                    {
                        var paid = invespa.PAID;
                        var patactdttx = new PatientAccountDetailTax();
                        patactdttx.PAccDetailID = PAccDetailID;
                        patactdttx.ServiceTypeID = item.invid;
                        patactdttx.TaxID = item.TaxID;
                        patactdttx.TaxPercentage = 0;
                        patactdttx.TotalProductValue = item.Amount;
                        patactdttx.TotalDiscountValue = item.DiscountAmount;
                        patactdttx.TotalTaxValue = item.GST;
                        patactdttx.TotalCGSTTaxValue = (item.GSTAmount) / 2;
                        patactdttx.TotalSGSTTaxValue = (item.GSTAmount) / 2;
                        patactdttx.TotalValue = InvIPBilling.InvestigationBillMaster.TotalBillValue;
                        patactdttx.CESSValue = InvIPBilling.InvestigationBillMaster.CESSAmount;
                        patactdttx.AdditionalCESSValue = InvIPBilling.InvestigationBillMaster.AdditionalCESSAmount;
                        patactdttx.CreatedUTC = DateTime.UtcNow;
                        patactdttx.CreatedBy = InvIPBilling.InvestigationBillMaster.CreatedBy;
                        WYNKContext.PatientAccountDetailTax.AddRange(patactdttx);

                        ErrorLog oErrorLogstran = new ErrorLog();
                        object namestr = patactdttx;
                        oErrorLogstran.WriteErrorLogArray("PatientAccountDetailTax", namestr);

                        WYNKContext.SaveChanges();
                    }
                   
                    var GrossAMT=InvIPBilling.InvestigationBillMaster.GrossProductValue;

                    ibm.PAID = PAID;
                    ibm.CMPID = InvIPBilling.InvestigationBillMaster.CMPID;
                    ibm.TransactionId = TransactionTypeid;
                    ibm.RegistrationTranID = InvIPBilling.InvestigationBillMaster.RegistrationTranID;
                    ibm.UIN = InvIPBilling.InvestigationBillMaster.UIN;
                    ibm.BillNo = InvIPBilling.InvestigationBillMaster.BillNo;
                    ibm.GrossProductValue = InvIPBilling.InvestigationBillMaster.GrossProductValue;
                    ibm.TotalDiscountValue = InvIPBilling.InvestigationBillMaster.TotalDiscountValue;
                    ibm.TotalTaxValue = InvIPBilling.InvestigationBillMaster.TotalTaxValue;
                    ibm.TotalCGSTTaxValue = (InvIPBilling.InvestigationBillMaster.TotalTaxValue) / 2;
                    ibm.TotalSGSTTaxValue = (InvIPBilling.InvestigationBillMaster.TotalTaxValue) / 2;
                    ibm.TotalBillValue = InvIPBilling.InvestigationBillMaster.TotalBillValue;
                    ibm.CESSAmount = InvIPBilling.InvestigationBillMaster.CESSAmount;
                    ibm.AdditionalCESSAmount = InvIPBilling.InvestigationBillMaster.AdditionalCESSAmount;
                    ibm.CreatedUTC = DateTime.UtcNow;
                    ibm.CreatedBy = InvIPBilling.InvestigationBillMaster.CreatedBy;
                    WYNKContext.InvestigationBillMaster.Add(ibm);

                    string cmpname = CMPSContext.Company.Where(x => x.CmpID == cmpPid).Select(x => x.CompanyName).FirstOrDefault();
                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == InvIPBilling.InvestigationBillMaster.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    string userid = Convert.ToString(InvIPBilling.InvestigationBillMaster.CreatedBy);
                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(cmpname, "InvestigationIPBilling", "User name :", username, "User ID :", userid, "Mode : Add");
                    object names = ibm;
                    oErrorLogs.WriteErrorLogArray("InvestigationBillMaster", names);

                    WYNKContext.SaveChanges();

                    var crtby = ibm.CreatedBy;                   
                    if (InvIPBilling.BillDetailsIP.Count() > 0)
                    {
                        foreach (var item in InvIPBilling.BillDetailsIP.ToList())

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
                            ibt.TaxID = item.TaxID;
                            ibt.AdditionalCESSPercentage = item.AdditionalCESS;
                            ibt.AdditionalCESSAmount = item.AdditionalCESSAmount;
                            ibt.CreatedUTC = DateTime.UtcNow;
                            ibt.CreatedBy = crtby;
                            WYNKContext.InvestigationBillTran.AddRange(ibt);
                            ErrorLog oErrorLogstran = new ErrorLog();
                            object namestr = ibt;
                            oErrorLogstran.WriteErrorLogArray("InvestigationBillTran", namestr);
                            WYNKContext.SaveChanges();
                            var invprid = ibt.InvPrescriptionID;
                        }
                    }


                    if (InvIPBilling.BillDetailsIP.Count() > 0)
                    {
                        foreach (var item in InvIPBilling.BillDetailsIP.ToList())

                        {
                            var taxidd = CMPSContext.TaxMaster.Where(x => x.ID == item.TaxID).ToList();
                            var taxgrp = CMPSContext.TaxMaster.Where(x => x.ID == item.TaxID).Select(x => x.TaxGroupId).FirstOrDefault();
                            if (taxidd != null && taxidd.Count != 0)
                            {
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
                                tax.GrossAmount = GrossAMT;
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
                            else
                            {
                                var masters = WYNKContext.TaxSummary.Where(x => x.TransactionDate == last && x.CMPID == cmpPid && x.BillingType == "B" && x.TaxID == item.TaxID).LastOrDefault();
                                masters.GrossAmount += GrossAMT;
                                masters.TaxValue += item.GSTAmount;
                                masters.Tax1Value += item.GSTAmount / 2;
                                masters.Tax2Value += item.GSTAmount / 2;
                                masters.CESSValue += item.CESSAmount;
                                masters.AddlCESSValue += item.AdditionalCESSAmount;
                                masters.UpdatedUTC = DateTime.UtcNow;
                                masters.UpdatedBy = crtby;
                                WYNKContext.TaxSummary.UpdateRange(masters);
                            }
                        }
                    }


                    WYNKContext.SaveChanges();

                    if (WYNKContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }

                    dbContextTransaction.Commit();
                    if (WYNKContext.SaveChanges() >= 0)
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
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
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

        public InvestigationIPBilling GetTaxDetails()

        {
            var getData = new InvestigationIPBilling();
            var TaxMaster = CMPSContext.TaxMaster.ToList();
            getData.GetTaxDetails = (from tax in TaxMaster.Where(x=>x.IsActive==true )
                                     
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

        public InvestigationIPBilling GetIPBillingDetails(string ipid, int TaxID)

        {
            var invv = new InvestigationIPBilling();
            invv.Investigation = new InvestigationImages();
            invv.INV = new List<InvestigationImages>();
            invv.BillDetailsIP = new List<BillDetailsIP>();
            invv.PaymentMaster = new List<Payment_Master>();
            var invpre = WYNKContext.InvestigationPrescription.ToList();
            var invtr = WYNKContext.InvestigationPrescriptionTran.ToList();
            var one = CMPSContext.OneLineMaster.ToList();         
            var taxMaster = CMPSContext.TaxMaster.ToList();
        
            invv.BillDetailsIP = (from IP in invpre.Where(x => x.RandomUniqueID == Convert.ToString(ipid))
                                join IM in invtr
                                on IP.RandomUniqueID equals IM.IPID
                                join OLM in one
                                on IM.InvestigationID equals OLM.OLMID
                                select new BillDetailsIP
                                {
                                    Investigation = OLM.ParentDescription,
                                    ipid = IP.RandomUniqueID,
                                    iptid = IM.ID,
                                    invid = IM.InvestigationID,
                                    Amount = OLM.Amount,
                                    GrossAmount = OLM.Amount,
                                    TotalCost = OLM.Amount,
                                }

                              ).ToList();




            return invv;


        }
    }
}







