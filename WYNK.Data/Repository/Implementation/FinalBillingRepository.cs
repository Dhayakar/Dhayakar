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
    class FinalBillingRepository : RepositoryBase<FinalBillingMaster>, IFinalBillingRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public FinalBillingRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public FinalBillingMaster Getcmpdtls(int cmpid)
        {


            var CmpDtls = new FinalBillingMaster();

            CmpDtls.Cmpaddress = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1).FirstOrDefault();

            CmpDtls.Cmpphno = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Phone1).FirstOrDefault();

            CmpDtls.Companyname = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault();

            CmpDtls.Cmplocation = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.LocationName).FirstOrDefault();

            CmpDtls.Cmpgstno = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.GSTNo).FirstOrDefault();

            return CmpDtls;

        }

        public FinalBillingMaster Getunbilleddtls(int cmpid, int tranid)
        {


            var FinalBilling = new FinalBillingMaster();

            var patientaccount = WYNKContext.PatientAccount.ToList();
            var registration = WYNKContext.Registration.ToList();
            var customermaster = WYNKContext.CustomerMaster.ToList();

            var reg = (from PA in patientaccount.Where(u => u.CMPID == cmpid && u.InvoiceNumber == null && u.InvoiceDate == null && u.TransactionID == tranid).OrderByDescending(x => x.CreatedUTC)
                       join REG in registration on PA.UIN equals REG.UIN

                       select new
                       {
                           paid = PA.PAID,
                           uin = PA.UIN,
                           name = REG.Name + " " + REG.MiddleName + " " + REG.LastName,
                           gender = REG.Gender,
                           age = PasswordEncodeandDecode.ToAgeString(REG.DateofBirth),
                           address = REG.Address1,
                           phone = REG.Phone,

                       }).ToList();

            var Creg = (from PA in patientaccount.Where(u => u.CMPID == cmpid && u.InvoiceNumber == null && u.InvoiceDate == null && u.TransactionID == tranid).OrderByDescending(x => x.CreatedUTC)
                        join CM in customermaster on PA.UIN equals Convert.ToString(CM.ID)
                        select new
                        {
                            paid = PA.PAID,
                            uin = PA.UIN,
                            name = CM.Name + " " + CM.MiddleName + " " + CM.LastName,
                            gender = "",
                            age = "",
                            address = CM.Address1,
                            phone = CM.PhoneNo,

                        }).ToList();

            var res = reg.Concat(Creg);

            FinalBilling.patientDtls = (from R in res.GroupBy(x => x.uin)
                                        select new patientDtls
                                        {
                                            paid = R.Select(x => x.paid).FirstOrDefault(),
                                            uin = R.Select(x => x.uin).FirstOrDefault(),
                                            name = R.Select(x => x.name).FirstOrDefault(),
                                            gender = R.Select(x => x.gender).FirstOrDefault(),
                                            age = R.Select(x => x.age).FirstOrDefault(),
                                            address = R.Select(x => x.address).FirstOrDefault(),
                                            phoneno = R.Select(x => x.phone).FirstOrDefault(),

                                        }).ToList();



            return FinalBilling;

        }


        public FinalBillingMaster GetReBillingDetails(int id, string gmt)
        {


            var FinalBilling = new FinalBillingMaster();

            var patientaccount = WYNKContext.PatientAccount.ToList();
            var registration = WYNKContext.Registration.ToList();
            var customermaster = WYNKContext.CustomerMaster.ToList();
            TimeSpan ts = TimeSpan.Parse(gmt);
            var reg = (from PA in patientaccount.Where(u => u.CMPID == id && u.InvoiceNumber != null && u.InvoiceDate != null).OrderByDescending(x => x.CreatedUTC)
                       join REG in registration on PA.UIN equals REG.UIN

                       select new
                       {
                           paid = PA.PAID,
                           billno = PA.InvoiceNumber,
                           billdt = PA.InvoiceDate.Value.Add(ts),
                           total = PA.TotalBillValue,
                           uin = PA.UIN,
                           name = REG.Name + " " + REG.MiddleName + " " + REG.LastName,
                           gender = REG.Gender,
                           age = PasswordEncodeandDecode.ToAgeString(REG.DateofBirth),
                           address = REG.Address1,
                           phone = REG.Phone,

                       }).ToList();

            var Creg = (from PA in patientaccount.Where(u => u.CMPID == id && u.InvoiceNumber != null && u.InvoiceDate != null).OrderByDescending(x => x.CreatedUTC)
                        join CM in customermaster on PA.UIN equals Convert.ToString(CM.ID)
                        select new
                        {
                            paid = PA.PAID,
                            billno = PA.InvoiceNumber,
                            billdt = PA.InvoiceDate.Value.Add(ts),
                            total = PA.TotalBillValue,
                            uin = PA.UIN,
                            name = CM.Name + " " + CM.MiddleName + " " + CM.LastName,
                            gender = "",
                            age = "",
                            address = CM.Address1,
                            phone = CM.PhoneNo,

                        }).ToList();

            var res = reg.Concat(Creg);

            FinalBilling.patientDtlsp = (from R in res.GroupBy(x => x.uin)
                                         select new patientDtlsp
                                         {
                                             paid = R.Select(x => x.paid).FirstOrDefault(),
                                             billno = R.Select(x => x.billno).FirstOrDefault(),
                                             billdt = R.Select(x => x.billdt).FirstOrDefault(),
                                             total = R.Select(x => x.total).FirstOrDefault(),
                                             uin = R.Select(x => x.uin).FirstOrDefault(),
                                             name = R.Select(x => x.name).FirstOrDefault(),
                                             gender = R.Select(x => x.gender).FirstOrDefault(),
                                             age = R.Select(x => x.age).FirstOrDefault(),
                                             address = R.Select(x => x.address).FirstOrDefault(),
                                             phoneno = R.Select(x => x.phone).FirstOrDefault(),

                                         }).ToList();



            return FinalBilling;

        }


        public FinalBillingMaster Getbillingdtls(int paid, int cmpid)
        {

            var FinalBilling = new FinalBillingMaster();
            var patientaccount = WYNKContext.PatientAccount.ToList();
            var patientaccountdtl = WYNKContext.PatientAccountDetail.ToList();

            FinalBilling.ServiceDtls = (from PA in patientaccount.Where(x => x.PAID == paid && x.CMPID == cmpid)
                                        join PAD in patientaccountdtl on PA.PAID equals PAD.PAID
                                        select new ServiceDtls
                                        {
                                            padid = PAD.PAccDetailID,
                                            ServiceDescription = PAD.ServiceDescription,
                                            Amount = PAD.TotalProductValue,
                                            DiscountAmount = (PAD.TotalDiscountValue != null ? PAD.TotalDiscountValue : 0),
                                            GSTAmount = (PAD.TotalTaxValue != null ? PAD.TotalTaxValue : 0) + (PAD.CESSValue != null ? PAD.CESSValue : 0) + (PAD.AdditonalCESSValue != null ? PAD.AdditonalCESSValue : 0),
                                            TotalCost = PAD.TotalBillValue,
                                        }).ToList();

            return FinalBilling;
        }

        public FinalBillingMaster Getbreakupdtls(int padid, int cmpid)
        {

            var FinalBilling = new FinalBillingMaster();
            var patientaccount = WYNKContext.PatientAccount.ToList();
            var patientaccountdtl = WYNKContext.PatientAccountDetail.ToList();
            var patientaccountdtltax = WYNKContext.PatientAccountDetailTax.ToList();

            FinalBilling.ItmDtls = (from PAD in patientaccountdtl.Where(x => x.PAccDetailID == padid)
                                    join PADT in patientaccountdtltax on PAD.PAccDetailID equals PADT.PAccDetailID
                                    select new ItmDtls
                                    {
                                        Description = PADT.Description,
                                        paccdtltaxid = PADT.PAccDetailTaxID,
                                        TaxID = Convert.ToInt32(PADT.TaxID),
                                        Price = Calculateunitprice(PADT.Quantity, PADT.TotalProductValue),
                                        Amount = PADT.TotalProductValue,
                                        Qty = (PADT.Quantity != null ? PADT.Quantity : 0),
                                        DiscountAmount = (PADT.TotalDiscountValue != null ? PADT.TotalDiscountValue : 0),
                                        Discount = ((PADT.TotalDiscountValue != null ? PADT.TotalDiscountValue : 0) / PADT.TotalProductValue) * 100,
                                        TaxDescription = CMPSContext.TaxMaster.Where(x => x.ID == PADT.TaxID).Select(x => x.TaxDescription).FirstOrDefault(),
                                        CESSDescription = CMPSContext.TaxMaster.Where(x => x.ID == PADT.TaxID).Select(x => x.CESSDescription).FirstOrDefault(),
                                        AdditionalCESSDescription = CMPSContext.TaxMaster.Where(x => x.ID == PADT.TaxID).Select(x => x.AdditionalCESSDescription).FirstOrDefault(),
                                        GST = CMPSContext.TaxMaster.Where(x => x.ID == PADT.TaxID).Select(x => x.GSTPercentage).FirstOrDefault(),
                                        CESS = CMPSContext.TaxMaster.Where(x => x.ID == PADT.TaxID).Select(x => x.CESSPercentage).FirstOrDefault(),
                                        AdditionalCESS = CMPSContext.TaxMaster.Where(x => x.ID == PADT.TaxID).Select(x => x.AdditionalCESSPercentage).FirstOrDefault(),
                                        GSTAmount = PADT.TotalTaxValue,
                                        CESSAmount = PADT.CESSValue,
                                        AdditionalCESSAmount = PADT.AdditionalCESSValue,
                                        TotalCost = PADT.TotalValue,
                                    }).ToList();

            return FinalBilling;
        }

        public decimal Calculateunitprice(Decimal? quantity, Decimal? total)
        {

            if (quantity != null && quantity != 0)
            {
                var result = total / quantity;
                return Convert.ToDecimal(result);
            }
            else
            {
                var result = total;
                return Convert.ToDecimal(result);
            }

        }

        public FinalBillingMaster Getprint(int paid, string bill, int cid, int tid, string gmt)

        {

            var op = new FinalBillingMaster();
            op.PatientAccountDetail = new List<PatientAccountDetail>();
            op.PatientAccount = new PatientAccount();
            op.PaymentMaster = new List<Payment_Master>();

            var taxMaster = CMPSContext.TaxMaster.ToList();
            TimeSpan ts = TimeSpan.Parse(gmt);
            op.PatientAccount.InvoiceNumber = WYNKContext.PatientAccount.Where(x => x.PAID == paid).Select(x => x.InvoiceNumber).FirstOrDefault();
            op.PatientAccount.InvoiceDate = WYNKContext.PatientAccount.Where(x => x.PAID == paid).Select(x => x.InvoiceDate.Value.Add(ts)).FirstOrDefault();
            op.PatientAccount.TotalBillValue = WYNKContext.PatientAccount.Where(x => x.PAID == paid).Select(x => x.TotalBillValue).FirstOrDefault();

            var patientaccount = WYNKContext.PatientAccount.ToList();
            var registration = WYNKContext.Registration.ToList();
            var customermaster = WYNKContext.CustomerMaster.ToList();

            var reg = (from PA in patientaccount.Where(u => u.CMPID == cid && u.InvoiceNumber == bill && u.PAID == paid).OrderByDescending(x => x.CreatedUTC)
                       join REG in registration on PA.UIN equals REG.UIN

                       select new
                       {
                           paid = PA.PAID,
                           uin = PA.UIN,
                           name = REG.Name + " " + REG.MiddleName + " " + REG.LastName,
                           gender = REG.Gender,
                           age = PasswordEncodeandDecode.ToAgeString(REG.DateofBirth),
                           address = REG.Address1,
                           phone = REG.Phone,

                       }).ToList();

            var Creg = (from PA in patientaccount.Where(u => u.CMPID == cid && u.InvoiceNumber == null && u.PAID == paid).OrderByDescending(x => x.CreatedUTC)
                        join CM in customermaster on PA.UIN equals Convert.ToString(CM.ID)
                        select new
                        {
                            paid = PA.PAID,
                            uin = PA.UIN,
                            name = CM.Name + " " + CM.MiddleName + " " + CM.LastName,
                            gender = "",
                            age = "",
                            address = CM.Address1,
                            phone = CM.PhoneNo,

                        }).ToList();

            var res = reg.Concat(Creg);

            op.patientDtlsprint = (from R in res.GroupBy(x => x.uin)
                                   select new patientDtlsprint
                                   {
                                       paid = R.Select(x => x.paid).FirstOrDefault(),
                                       uin = R.Select(x => x.uin).FirstOrDefault(),
                                       name = R.Select(x => x.name).FirstOrDefault(),
                                       gender = R.Select(x => x.gender).FirstOrDefault(),
                                       age = R.Select(x => x.age).FirstOrDefault(),
                                       address = R.Select(x => x.address).FirstOrDefault(),
                                       phoneno = R.Select(x => x.phone).FirstOrDefault(),

                                   }).ToList();



            var patientaccountdtl = WYNKContext.PatientAccountDetail.ToList();

            op.ServiceDtlsprint = (from PAD in patientaccountdtl.Where(x => x.PAID == paid)

                                   select new ServiceDtlsprint
                                   {
                                       padid = PAD.PAccDetailID,
                                       ServiceDescription = PAD.ServiceDescription,
                                       Amount = PAD.TotalProductValue,
                                       DiscountAmount = (PAD.TotalDiscountValue != null ? PAD.TotalDiscountValue : 0),
                                       GSTAmount = (PAD.TotalTaxValue != null ? PAD.TotalTaxValue : 0) + (PAD.CESSValue != null ? PAD.CESSValue : 0) + (PAD.AdditonalCESSValue != null ? PAD.AdditonalCESSValue : 0),
                                       TotalCost = PAD.TotalBillValue,
                                   }).ToList();



            op.PayDetailss = (from om in WYNKContext.PatientAccount.Where(x => x.PAID == paid && x.CMPID == cid && x.InvoiceNumber == bill)
                              join pm in WYNKContext.PaymentMaster.Where(x => x.TransactionID == tid)
                              on om.PAID equals pm.PaymentReferenceID

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


        public FinalBillingMaster Getrepbillingdtls(int paid, int cmpid, string gmt)

        {

            var op = new FinalBillingMaster();
            op.PatientAccountDetail = new List<PatientAccountDetail>();
            op.PatientAccount = new PatientAccount();
            op.PaymentMaster = new List<Payment_Master>();

            var taxMaster = CMPSContext.TaxMaster.ToList();
            TimeSpan ts = TimeSpan.Parse(gmt);
            op.PatientAccount.InvoiceNumber = WYNKContext.PatientAccount.Where(x => x.PAID == paid).Select(x => x.InvoiceNumber).FirstOrDefault();
            op.PatientAccount.InvoiceDate = WYNKContext.PatientAccount.Where(x => x.PAID == paid).Select(x => x.InvoiceDate.Value.Add(ts)).FirstOrDefault();
            op.PatientAccount.TotalBillValue = WYNKContext.PatientAccount.Where(x => x.PAID == paid).Select(x => x.TotalBillValue).FirstOrDefault();

            var patientaccount = WYNKContext.PatientAccount.ToList();
            var registration = WYNKContext.Registration.ToList();
            var customermaster = WYNKContext.CustomerMaster.ToList();

            var patientaccountdtl = WYNKContext.PatientAccountDetail.ToList();

            op.ServiceDtlsprintp = (from PAD in patientaccountdtl.Where(x => x.PAID == paid)

                                    select new ServiceDtlsprintp
                                    {
                                        padid = PAD.PAccDetailID,
                                        ServiceDescription = PAD.ServiceDescription,
                                        Amount = PAD.TotalProductValue,
                                        DiscountAmount = (PAD.TotalDiscountValue != null ? PAD.TotalDiscountValue : 0),
                                        GSTAmount = (PAD.TotalTaxValue != null ? PAD.TotalTaxValue : 0) + (PAD.CESSValue != null ? PAD.CESSValue : 0) + (PAD.AdditonalCESSValue != null ? PAD.AdditonalCESSValue : 0),
                                        TotalCost = PAD.TotalBillValue,
                                    }).ToList();



            op.PayDetailsspp = (from om in WYNKContext.PatientAccount.Where(x => x.PAID == paid && x.CMPID == cmpid)
                                join pm in WYNKContext.PaymentMaster
                                on om.PAID equals pm.PaymentReferenceID

                                select new PayDetailsspp
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

        public dynamic InsertBilling(FinalBillingMaster payment, int cmpid, string UIN, int TransactionTypeid, int paid)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var PatAcc = new PatientAccount();

                    var PAID = WYNKContext.PatientAccount.Where(x => x.UIN == UIN && x.InvoiceNumber == null && x.PAID == paid && x.TransactionID == TransactionTypeid).Select(x => x.PAID).FirstOrDefault();
                    if (PAID != null && PAID != 0)
                    {
                        PatAcc = WYNKContext.PatientAccount.Where(x => x.PAID == PAID).FirstOrDefault();
                        PatAcc.InvoiceNumber = payment.payment.InVoiceNumber;
                        PatAcc.InvoiceDate = DateTime.UtcNow;
                        WYNKContext.Entry(PatAcc).State = EntityState.Modified;

                    }

                    var medbill = new MedicalBill_Master();
                    var medbillid = WYNKContext.MedicalBillMaster.Where(x => x.UIN == UIN && x.BillNo == "999").Select(x => x.ID).FirstOrDefault();

                    if (medbillid != null && medbillid != 0)
                    {
                        medbill = WYNKContext.MedicalBillMaster.Where(x => x.ID == medbillid).FirstOrDefault();
                        medbill.BillNo = payment.payment.InVoiceNumber;
                        medbill.PAID = paid;
                        WYNKContext.Entry(medbill).State = EntityState.Modified;

                    }

                    var optbill = new OpticalInvoiceMaster();

                    if (payment.PaymentMaster.Count() > 0)
                    {
                        foreach (var item in payment.PaymentMaster.ToList())

                        {

                            var pm = new Payment_Master();

                            pm.UIN = UIN;
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
                            pm.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(s => s.ID == WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Datee && Convert.ToDateTime(x.FYTo) >= Datee && x.CMPID == cmpid && x.IsActive == true).Select(x => x.ID).FirstOrDefault()).Select(s => s.FYAccYear).FirstOrDefault());
                            pm.PaymentReferenceID = paid;
                            pm.InVoiceNumber = payment.payment.InVoiceNumber;
                            pm.InVoiceDate = DateTime.UtcNow;
                            pm.ReceiptNumber = payment.ReceiptRunningNo;
                            pm.ReceiptDate = DateTime.Now.Date;
                            pm.CreatedUTC = DateTime.UtcNow;
                            pm.CreatedBy = 3162;
                            pm.TransactionID = TransactionTypeid;
                            pm.CmpID = cmpid;
                            pm.IsCancelled = false;

                            WYNKContext.PaymentMaster.AddRange(pm);
                            ErrorLog oErrorLogspay = new ErrorLog();
                            object namepay = pm;
                            oErrorLogspay.WriteErrorLogArray("PaymentMaster", namepay);
                            WYNKContext.SaveChanges();



                        }

                    }


                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

                    var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                    var RunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpid, "GetRunningNo");

                    if (RunningNumber == payment.payment.InVoiceNumber)
                    {
                        commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpid, "UpdateRunningNo");
                    }
                    else
                    {
                        var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpid, "UpdateRunningNo");

                        var ib = WYNKContext.PatientAccount.Where(x => x.InvoiceNumber == payment.payment.InVoiceNumber).FirstOrDefault();
                        ib.InvoiceNumber = GetRunningNumber;
                        WYNKContext.PatientAccount.UpdateRange(ib);

                        WYNKContext.SaveChanges();
                    }


                    var RecContraID = commonRepos.GettingReceiptTcID(TransactionTypeid, cmpid);
                    var ReceiptRunningNumber = commonRepos.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), cmpid, "GetRunningNo");
                    if (ReceiptRunningNumber == payment.ReceiptRunningNo)
                    {
                        commonRepos.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), cmpid, "UpdateRunningNo");
                    }
                    else
                    {
                        var payments = WYNKContext.PaymentMaster.Where(x => x.ReceiptNumber == payment.ReceiptRunningNo && x.TransactionID == TransactionTypeid).ToList();
                        payments.All(x => { x.ReceiptNumber = ReceiptRunningNumber; return true; });
                        WYNKContext.PaymentMaster.UpdateRange(payments);
                    }

                    if (WYNKContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }
                    return new
                    {

                        Success = true,
                        Message = CommonMessage.saved,
                        paid = PAID,
                        bill = PatAcc.InvoiceNumber,
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



        public FinalBillingMaster Getunbilleddtlspackage(int cmpid, string tranid)
        {
            var FinalBilling = new FinalBillingMaster();
            var patientaccount = WYNKContext.PatientAccount.Where(x =>x.InvoiceNumber == null && x.InvoiceDate == null).ToList();
            var registration = WYNKContext.Registration.Where(x => x.UIN.StartsWith(tranid, StringComparison.OrdinalIgnoreCase)
                              || x.Phone.StartsWith(tranid) || x.Name.StartsWith(tranid, StringComparison.OrdinalIgnoreCase)
                              || x.MiddleName == tranid
                              || x.AadharNumber == tranid
                              || x.LastName.StartsWith(tranid, StringComparison.OrdinalIgnoreCase)
                              || x.Address1.StartsWith(tranid, StringComparison.OrdinalIgnoreCase)).ToList();
            var admission = WYNKContext.Admission.ToList();

            FinalBilling.patientDtls = (from PA in patientaccount.Where(u => u.CMPID == cmpid).OrderByDescending(x => x.CreatedUTC)
                                        join SA in admission.Where(x => x.IsSurgeryCompleted == true && x.BillingType == "Pkg" && x.DischargeID == null) on PA.UIN equals SA.UIN
                                        join REG in registration on PA.UIN equals REG.UIN
                                        select new patientDtls
                                        {
                                            paid = PA.PAID,
                                            uin = PA.UIN,
                                            name = REG.Name + " " + REG.MiddleName + " " + REG.LastName,
                                            gender = REG.Gender,
                                            age = PasswordEncodeandDecode.ToAgeString(REG.DateofBirth),
                                            address = REG.Address1,
                                            phoneno = REG.Phone,
                                            billtype = SA.BillingType,
                                        }).ToList();

            return FinalBilling;

        }

        public dynamic Getbilleddtlspackage(int cmpid, int tranid)
        {
            var FinalBilling = new FinalBillingMaster();
            var patientaccount = WYNKContext.PatientAccount.ToList();
            var registration = WYNKContext.Registration.ToList();
            var admission = WYNKContext.Admission.ToList();

            FinalBilling.patientDtls = (from PA in patientaccount.Where(u => u.CMPID == cmpid && u.InvoiceNumber != null && u.InvoiceDate != null
                                        && u.TransactionID == tranid).OrderByDescending(x => x.CreatedUTC)
                                        join SA in admission.Where(x => x.InvoiceDate != null && x.InvoiceDate != null && x.IsSurgeryCompleted == true
                                        && x.BillingType == "Pkg" && x.DischargeID == null) on PA.UIN equals SA.UIN
                                        join REG in registration on PA.UIN equals REG.UIN
                                        select new patientDtls
                                        {
                                            paid = PA.PAID,
                                            uin = PA.UIN,
                                            name = REG.Name + " " + REG.MiddleName + " " + REG.LastName,
                                            gender = REG.Gender,
                                            age = PasswordEncodeandDecode.ToAgeString(REG.DateofBirth),
                                            address = REG.Address1,
                                            phoneno = REG.Phone,
                                            billtype = SA.BillingType,
                                            billno = PA.InvoiceNumber,
                                            billdate = PA.InvoiceDate,
                                        }).ToList();

            return FinalBilling;

        }

        public FinalBillingMaster Getbillingdtlspackage(int paid, int cmpid, string billtype)
        {

            var FinalBilling = new FinalBillingMaster();
            var patientaccount = WYNKContext.PatientAccount.ToList();
            var patientaccountdtl = WYNKContext.PatientAccountDetail.ToList();

            if (billtype != "Pkg")
            {
                FinalBilling.ServiceDtls = (from PA in patientaccount.Where(x => x.PAID == paid && x.CMPID == cmpid)
                                            join PAD in patientaccountdtl on PA.PAID equals PAD.PAID
                                            select new ServiceDtls
                                            {
                                                padid = PAD.PAccDetailID,
                                                ServiceDescription = PAD.ServiceDescription,
                                                Amount = PAD.TotalProductValue,
                                                DiscountAmount = (PAD.TotalDiscountValue != null ? PAD.TotalDiscountValue : 0),
                                                GSTAmount = (PAD.TotalTaxValue != null ? PAD.TotalTaxValue : 0) + (PAD.CESSValue != null ? PAD.CESSValue : 0) + (PAD.AdditonalCESSValue != null ? PAD.AdditonalCESSValue : 0),
                                                TotalCost = PAD.TotalBillValue,
                                            }).ToList();
            }

            else if (billtype == "Pkg")
            {

                var patientaccountdtls = WYNKContext.PatientAccountDetail.Where(x => x.PAID == paid).ToList();
                var patientaccounts = WYNKContext.PatientAccount.Where(x => x.PAID == paid && x.InvoiceDate == null && x.InvoiceNumber == null).ToList();
                var patientaccountdtltax = WYNKContext.PatientAccountDetailTax.ToList();
                var packagemapping = WYNKContext.PackageMapping.Where(x => x.CMPID == cmpid && x.IsActive == true).ToList();
                var services = WYNKContext.Services.ToList();

                FinalBilling.PackageBillingListtotals = new List<PackageBillingListtotal>();
                FinalBilling.unPackageBillingLists = new List<unPackageBillingList>();

                var UIN = patientaccounts.Select(x => x.UIN).FirstOrDefault();

                FinalBilling.Lensused = patientaccounts.Select(x => x.Description).FirstOrDefault();
                FinalBilling.Procedure = WYNKContext.ICDMaster.Where(x => x.ICDCODE == WYNKContext.SurgeryTran.Where(y => y.SurgeryID == WYNKContext.Surgery.Where(z => z.UIN == UIN && z.CMPID == cmpid).Select(z => z.RandomUniqueID).FirstOrDefault()).Select(y => y.ICDCode).FirstOrDefault()).Select(z => z.ICDDESCRIPTION).FirstOrDefault();
                var dada = (from gm in patientaccounts
                            join cc in patientaccountdtls on gm.PAID equals cc.PAID
                            join pt in patientaccountdtltax on cc.PAccDetailID equals pt.PAccDetailID
                            select new
                            {
                                padid = gm.PAID,
                            }).ToList();
                if (dada.Count() > 0)
                {
                    var nonservicedetails = (from gm in patientaccounts
                                             join cc in patientaccountdtls.Where(x => x.ServiceDescription != "Surgery Cost") on gm.PAID equals cc.PAID
                                             join padx in patientaccountdtltax on cc.PAccDetailID equals padx.PAccDetailID
                                             select new
                                             {
                                                 serviceid = padx.ServiceTypeID,
                                                 pacctaxid = padx.PAccDetailTaxID,
                                                 padetailid = padx.PAccDetailID,
                                                 paidid = paid,
                                                 uin = gm.UIN,
                                                 amount = padx.TotalProductValue,
                                                 ServiceDescription = cc.ServiceDescription,
                                             }).ToList();
                    var Servicedetails = (from gm in patientaccounts
                                          join cc in patientaccountdtls.Where(x => x.ServiceDescription == "Surgery Cost") on gm.PAID equals cc.PAID
                                          select new
                                          {
                                              gst = cc.TotalTaxValue,
                                              discount = cc.TotalDiscountValue,
                                              padetailid = cc.PAccDetailID,
                                              paidid = paid,
                                              uin = gm.UIN,
                                              amount = cc.TotalProductValue,
                                              ServiceTypeID = cc.ServiceTypeID,
                                              Description = cc.ServiceDescription,
                                              ServiceDescription = services.Where(y => y.ID == cc.ServiceTypeID).Select(y => y.Description).FirstOrDefault(),
                                          }).ToList();


                    foreach (var iitem in Servicedetails)
                    {
                        var packagerelateddata = new PackageBillingListtotal();
                        packagerelateddata.Date = WYNKContext.Surgery.Where(x => x.UIN == iitem.uin && x.CMPID == cmpid).Select(x => x.DateofSurgery).FirstOrDefault();
                        packagerelateddata.Description = iitem.Description;
                        packagerelateddata.ServiceDescription = services.Where(y => y.ID == iitem.ServiceTypeID).Select(y => y.Description).FirstOrDefault();
                        packagerelateddata.Amount = iitem.amount;
                        packagerelateddata.Dummyamout = 0;
                        FinalBilling.PackageBillingListtotals.Add(packagerelateddata);
                    }

                    foreach (var item in nonservicedetails)
                    {
                        var pid = packagemapping.Where(x => x.ServiceID == item.serviceid).FirstOrDefault();
                        if (pid != null)
                        {
                            var packagerelateddata = new PackageBillingListtotal();
                            packagerelateddata.Date = WYNKContext.Surgery.Where(x => x.UIN == item.uin && x.CMPID == cmpid).Select(x => x.DateofSurgery).FirstOrDefault();
                            packagerelateddata.Description = item.ServiceDescription;
                            packagerelateddata.ServiceDescription = services.Where(y => y.ID == item.serviceid).Select(y => y.Description).FirstOrDefault();
                            packagerelateddata.Amount = item.amount;
                            packagerelateddata.Dummyamout = 0;
                            FinalBilling.PackageBillingListtotals.Add(packagerelateddata);
                        }
                        else
                        {
                            var packagerelateddata = new PackageBillingListtotal();
                            packagerelateddata.Date = WYNKContext.Surgery.Where(x => x.UIN == item.uin && x.CMPID == cmpid).Select(x => x.DateofSurgery).FirstOrDefault();
                            packagerelateddata.Description = item.ServiceDescription;
                            packagerelateddata.ServiceDescription = services.Where(y => y.ID == item.serviceid).Select(y => y.Description).FirstOrDefault();
                            packagerelateddata.Amount = item.amount;
                            packagerelateddata.Dummyamout = item.amount;
                            FinalBilling.PackageBillingListtotals.Add(packagerelateddata);
                        }
                    }

             
                    FinalBilling.PackageBillingList = (from sd in Servicedetails
                                                       select new PackageBillingList
                                                       {
                                                           Discount = (sd.discount != null ? sd.discount : 0),
                                                           TempDiscount = (sd.discount != null ? sd.discount : 0),
                                                           GSTAmount = (sd.gst != null ? sd.gst : 0),
                                                           Description = sd.ServiceDescription,
                                                           PackageBillingListtotal = FinalBilling.PackageBillingListtotals,
                                                           //unPackageBillingList = FinalBilling.unPackageBillingLists,
                                                           TotalCost = (FinalBilling.PackageBillingListtotals.Sum(x => x.Amount) != null ? FinalBilling.PackageBillingListtotals.Sum(x => x.Amount) : 0),
                                                           Amount = (FinalBilling.PackageBillingListtotals.Sum(x => x.Amount) != null ? FinalBilling.PackageBillingListtotals.Sum(x => x.Amount) : 0),
                                                       }).ToList();
                }
                else
                {
                    var Servicedetails = (from gm in patientaccounts
                                          join cc in patientaccountdtls.Where(x => x.ServiceDescription == "Surgery Cost") on gm.PAID equals cc.PAID
                                          select new
                                          {
                                              gst = cc.TotalTaxValue,
                                              discount = cc.TotalDiscountValue,
                                              padetailid = cc.PAccDetailID,
                                              paidid = paid,
                                              uin = gm.UIN,
                                              amount = cc.TotalProductValue,
                                              ServiceTypeID = cc.ServiceTypeID,
                                              Description = cc.ServiceDescription,
                                              ServiceDescription = services.Where(y => y.ID == cc.ServiceTypeID).Select(y => y.Description).FirstOrDefault(),
                                          }).ToList();
                    foreach (var iitem in Servicedetails)
                    {
                        var packagerelateddata = new PackageBillingListtotal();
                        packagerelateddata.Date = WYNKContext.Surgery.Where(x => x.UIN == iitem.uin && x.CMPID == cmpid).Select(x => x.DateofSurgery).FirstOrDefault();
                        packagerelateddata.Description = iitem.Description;
                        packagerelateddata.ServiceDescription = services.Where(y => y.ID == iitem.ServiceTypeID).Select(y => y.Description).FirstOrDefault();
                        packagerelateddata.Amount = iitem.amount;
                        packagerelateddata.Dummyamout = 0;
                        FinalBilling.PackageBillingListtotals.Add(packagerelateddata);
                    }

                    FinalBilling.PackageBillingList = (from sd in Servicedetails
                                                       select new PackageBillingList
                                                       {
                                                           Discount = (sd.discount != null ? sd.discount : 0),
                                                           TempDiscount = (sd.discount != null ? sd.discount : 0),
                                                           GSTAmount = (sd.gst != null ? sd.gst : 0),
                                                           Description = sd.ServiceDescription,
                                                           PackageBillingListtotal = FinalBilling.PackageBillingListtotals,
                                                           //unPackageBillingList = FinalBilling.unPackageBillingLists,
                                                           TotalCost = (FinalBilling.PackageBillingListtotals.Sum(x => x.Amount) != null ? FinalBilling.PackageBillingListtotals.Sum(x => x.Amount) : 0),
                                                           Amount = (FinalBilling.PackageBillingListtotals.Sum(x => x.Amount) != null ? FinalBilling.PackageBillingListtotals.Sum(x => x.Amount) : 0),
                                                       }).ToList();
                }

            }
            FinalBilling.totaldicountprint = FinalBilling.PackageBillingList.Sum(x => x.Discount);
            FinalBilling.totaltaxprint = FinalBilling.PackageBillingList.Sum(x => x.GSTAmount);

            var discprice = (FinalBilling.PackageBillingList.Sum(x => x.Discount) != null ? FinalBilling.PackageBillingList.Sum(x => x.Discount) : 0) + (FinalBilling.PackageBillingList.Sum(x => x.GSTAmount) != null ? FinalBilling.PackageBillingList.Sum(x => x.GSTAmount) : 0);

            FinalBilling.totalnetamount = FinalBilling.PackageBillingList.Sum(x => x.Amount) - discprice;
            FinalBilling.totalcostamount = FinalBilling.PackageBillingList.Sum(x => x.Amount) - discprice;
            FinalBilling.minustotalnetamount = FinalBilling.PackageBillingList.Sum(x => x.Amount) - discprice;

            return FinalBilling;
        }


        public dynamic Getbreakupbilling(int cmpid, int padtid)
        {
            var FinalBilling = new FinalBillingMaster();
            var patientaccountdtl = WYNKContext.PatientAccountDetail.Where(x => x.PAID == padtid && x.ServiceDescription != "Surgery Cost").ToList();
            var patientaccount = WYNKContext.PatientAccount.Where(x => x.PAID == padtid && x.InvoiceDate == null && x.InvoiceNumber == null).ToList();
            var patientaccountdtltax = WYNKContext.PatientAccountDetailTax.ToList();
            var packagemapping = WYNKContext.PackageMapping.Where(x => x.CMPID == cmpid && x.IsActive == true).ToList();
            var services = WYNKContext.Services.ToList();

            var NonServiceDtlsss = (from gm in patientaccount
                                    join cc in patientaccountdtl on gm.PAID equals cc.PAID
                                    join pt in patientaccountdtltax on cc.PAccDetailID equals pt.PAccDetailID
                                    join pm in packagemapping on pt.ServiceTypeID equals pm.ServiceID
                                    select new
                                    {
                                        surgdate = WYNKContext.Surgery.Where(x => x.UIN == gm.UIN && x.CMPID == gm.CMPID).Select(x => x.DateofSurgery).FirstOrDefault(),
                                        paccdtltaxid = pt.PAccDetailTaxID,
                                        discount = 0,
                                        Description = cc.ServiceDescription,
                                        ServiceDescription = services.Where(y => y.ID == packagemapping.Where(x => x.ServiceID == pt.ServiceTypeID).Select(x => x.ServiceID).FirstOrDefault()).Select(y => y.Description).FirstOrDefault(),
                                    }).ToList();
            var ServiceDtlsss = (from cc in WYNKContext.PatientAccountDetail.Where(x => x.ServiceDescription == "Surgery Cost" && x.PAID == padtid)
                                 select new
                                 {
                                     surgdate = WYNKContext.Surgery.Where(x => x.UIN == WYNKContext.PatientAccount.Where(y => y.PAID == padtid).Select(y => y.UIN).FirstOrDefault() && x.CMPID == cmpid).Select(x => x.DateofSurgery).FirstOrDefault(),
                                     paccdtltaxid = cc.PAccDetailID,
                                     discount = 0,
                                     Description = cc.ServiceDescription,
                                     ServiceDescription = WYNKContext.ICDSpecialityCode.Where(y => y.ID == cc.OLMID).Select(y => y.SpecialityDescription).FirstOrDefault(),
                                 }).ToList();

            var merged = ServiceDtlsss.Union(NonServiceDtlsss);
            var viceDtls = (from cv in merged
                            select new ServiceDtlsprintNew
                            {
                                Discount = cv.discount,
                                Description = cv.Description,
                                ServiceDescription = cv.ServiceDescription,
                                Date = cv.surgdate,
                            }).ToList();




            return viceDtls;
        }

        public FinalBillingMaster Getprintpackage(int paid, string bill, int cid, int tid, string gmt)

        {

            var op = new FinalBillingMaster();
            op.PatientAccountDetail = new List<PatientAccountDetail>();
            op.PatientAccount = new PatientAccount();
            op.PaymentMaster = new List<Payment_Master>();

            var taxMaster = CMPSContext.TaxMaster.ToList();
            TimeSpan ts = TimeSpan.Parse(gmt);
            op.PatientAccount.InvoiceNumber = WYNKContext.PatientAccount.Where(x => x.PAID == paid).Select(x => x.InvoiceNumber).FirstOrDefault();
            op.PatientAccount.InvoiceDate = WYNKContext.PatientAccount.Where(x => x.PAID == paid).Select(x => x.InvoiceDate.Value.Add(ts)).FirstOrDefault();
            op.PatientAccount.TotalBillValue = WYNKContext.PatientAccount.Where(x => x.PAID == paid).Select(x => x.TotalBillValue).FirstOrDefault();

            var patientaccount = WYNKContext.PatientAccount.ToList();
            var registration = WYNKContext.Registration.ToList();
            var customermaster = WYNKContext.CustomerMaster.ToList();

            op.patientDtlsprint = (from PA in patientaccount.Where(u => u.CMPID == cid && u.InvoiceNumber == bill && u.PAID == paid).OrderByDescending(x => x.CreatedUTC)
                                   join REG in registration on PA.UIN equals REG.UIN

                                   select new patientDtlsprint
                                   {
                                       paid = PA.PAID,
                                       uin = PA.UIN,
                                       name = REG.Name + " " + REG.MiddleName + " " + REG.LastName,
                                       gender = REG.Gender,
                                       age = PasswordEncodeandDecode.ToAgeString(REG.DateofBirth),
                                       address = REG.Address1,
                                       phoneno = REG.Phone,

                                   }).ToList();



            var patientaccountdtl = WYNKContext.PatientAccountDetail.ToList();

            op.ServiceDtlsprint = (from PAD in patientaccountdtl.Where(x => x.PAID == paid)

                                   select new ServiceDtlsprint
                                   {
                                       padid = PAD.PAccDetailID,
                                       ServiceDescription = PAD.ServiceDescription,
                                       Amount = PAD.TotalProductValue + (PAD.TotalDiscountValue != null ? PAD.TotalDiscountValue : 0),
                                       DiscountAmount = (PAD.TotalDiscountValue != null ? PAD.TotalDiscountValue : 0),
                                       GSTAmount = (PAD.TotalTaxValue != null ? PAD.TotalTaxValue : 0) + (PAD.CESSValue != null ? PAD.CESSValue : 0) + (PAD.AdditonalCESSValue != null ? PAD.AdditonalCESSValue : 0),
                                       TotalCost = PAD.TotalBillValue,
                                   }).ToList();



            op.PayDetailss = (from om in WYNKContext.PatientAccount.Where(x => x.PAID == paid && x.CMPID == cid && x.InvoiceNumber == bill)
                              join pm in WYNKContext.PaymentMaster.Where(x => x.TransactionID == tid)
                              on om.PAID equals pm.PaymentReferenceID

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


        public FinalBillingMaster Getbreakupdtlspackage(int padid, int cmpid)
        {

            var FinalBilling = new FinalBillingMaster();
            var patientaccount = WYNKContext.PatientAccount.ToList();
            var patientaccountdtl = WYNKContext.PatientAccountDetail.ToList();
            var patientaccountdtltax = WYNKContext.PatientAccountDetailTax.ToList();
            var icdspec = WYNKContext.ICDSpecialityCode.ToList();

            var type = WYNKContext.PatientAccountDetail.Where(x => x.PAccDetailID == padid).Select(x => x.ServiceDescription).FirstOrDefault();

            if (type == "Surgery Cost")
            {
                FinalBilling.ItmDtls = (from PAD in patientaccountdtl.Where(x => x.PAccDetailID == padid)
                                        join IS in icdspec on PAD.OLMID equals IS.ID
                                        join PA in patientaccount on PAD.PAID equals PA.PAID
                                        select new ItmDtls
                                        {
                                            Description = IS.SpecialityDescription,
                                            Lensused = PA.Description,
                                            Amount = PAD.TotalProductValue,
                                            DiscountAmount = (PAD.TotalDiscountValue != null ? PAD.TotalDiscountValue : 0),
                                            GSTAmount = (PAD.TotalTaxValue != null ? PAD.TotalTaxValue : 0),
                                            TotalCost = PAD.TotalBillValue,
                                        }).ToList();
            }
            return FinalBilling;
        }

        public dynamic InsertBillingpackage(FinalBillingMaster payment, int cmpid, string UIN, int TransactionTypeid, int paid, decimal netamount, decimal discamount)
        {
            var inv = "";
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    if (payment.PaymentMaster.Count() > 0)
                    {
                        var PatAcc = new PatientAccount();
                        var PAID = WYNKContext.PatientAccount.Where(x => x.UIN == UIN && x.PAID == paid && x.TransactionID == TransactionTypeid).Select(x => x.PAID).FirstOrDefault();
                        if (PAID != null && PAID != 0)
                        {
                            PatAcc = WYNKContext.PatientAccount.Where(x => x.PAID == PAID).FirstOrDefault();
                            PatAcc.InvoiceNumber = payment.InVoiceNumber;
                            PatAcc.InvoiceDate = DateTime.UtcNow;
                            WYNKContext.Entry(PatAcc).State = EntityState.Modified;
                        }
                        var adm = new Admission();
                        var admid = WYNKContext.Admission.Where(x => x.UIN == UIN).Select(x => x.AdmID).FirstOrDefault();
                        if (admid != null && admid != 0)
                        {
                            adm = WYNKContext.Admission.Where(x => x.AdmID == admid).FirstOrDefault();
                            adm.InvoiceNumber = payment.InVoiceNumber;
                            adm.InvoiceDate = DateTime.UtcNow;
                            adm.Paid = paid;
                            WYNKContext.Entry(adm).State = EntityState.Modified;
                            inv = payment.InVoiceNumber;
                        }
                        if (payment.PaymentMaster.Count() > 0)
                        {
                            foreach (var item in payment.PaymentMaster.ToList())
                            {
                                var pm = new Payment_Master();
                                pm.UIN = UIN;
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
                                pm.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(s => s.ID == WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Datee && Convert.ToDateTime(x.FYTo) >= Datee && x.CMPID == cmpid && x.IsActive == true).Select(x => x.ID).FirstOrDefault()).Select(s => s.FYAccYear).FirstOrDefault());
                                pm.PaymentReferenceID = paid;
                                pm.InVoiceNumber = payment.InVoiceNumber;
                                pm.InVoiceDate = DateTime.UtcNow;
                                pm.ReceiptNumber = payment.ReceiptRunningNo;
                                pm.ReceiptDate = DateTime.Now.Date;
                                pm.CreatedUTC = DateTime.UtcNow;
                                pm.CreatedBy = 3162;
                                pm.TransactionID = TransactionTypeid;
                                pm.CmpID = cmpid;
                                pm.IsCancelled = false;
                                WYNKContext.PaymentMaster.AddRange(pm);
                                ErrorLog oErrorLogspay = new ErrorLog();
                                object namepay = pm;
                                oErrorLogspay.WriteErrorLogArray("PaymentMaster", namepay);
                                WYNKContext.SaveChanges();
                            }
                        }

                        //var list = payment.ServiceDtls;
                        //foreach (var item in list)
                        //{
                        //    var PAIDs = WYNKContext.PatientAccount.Where(x => x.PAID == paid).FirstOrDefault();
                        //    PAIDs.TotalProductValue = item.TotalCost;
                        //    PAIDs.TotalBillValue = item.TotalCost;
                        //    PAIDs.TotalDiscountValue = item.DiscountAmount;
                        //    WYNKContext.PatientAccount.UpdateRange(PAIDs);
                        //}
                        var PAIDs = WYNKContext.PatientAccount.Where(x => x.PAID == paid).FirstOrDefault();
                        PAIDs.TotalProductValue = netamount;
                        PAIDs.TotalBillValue = netamount;
                        PAIDs.TotalDiscountValue = discamount;
                        WYNKContext.PatientAccount.UpdateRange(PAIDs);

                        WYNKContext.SaveChanges();
                        dbContextTransaction.Commit();
                        var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                        var RunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpid, "GetRunningNo");
                        if (RunningNumber == payment.InVoiceNumber)
                        {
                            commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpid, "UpdateRunningNo");
                        }
                        else
                        {
                            var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpid, "UpdateRunningNo");
                            var ib = WYNKContext.PatientAccount.Where(x => x.InvoiceNumber == payment.InVoiceNumber).FirstOrDefault();
                            ib.InvoiceNumber = GetRunningNumber;
                            WYNKContext.PatientAccount.UpdateRange(ib);
                            WYNKContext.SaveChanges();
                        }
                        var RecContraID = commonRepos.GettingReceiptTcID(TransactionTypeid, cmpid);
                        var ReceiptRunningNumber = commonRepos.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), cmpid, "GetRunningNo");
                        if (ReceiptRunningNumber == payment.ReceiptRunningNo)
                        {
                            commonRepos.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), cmpid, "UpdateRunningNo");
                        }
                        else
                        {
                            var payments = WYNKContext.PaymentMaster.Where(x => x.ReceiptNumber == payment.ReceiptRunningNo && x.TransactionID == TransactionTypeid).ToList();
                            payments.All(x => { x.ReceiptNumber = ReceiptRunningNumber; return true; });
                            WYNKContext.PaymentMaster.UpdateRange(payments);
                        }

                    }
                    else
                    {
                        //var list = payment.ServiceDtls;
                        //foreach (var item in list)
                        //{
                        //    var PAIDs = WYNKContext.PatientAccount.Where(x => x.PAID == paid).FirstOrDefault();
                        //    PAIDs.TotalProductValue = item.TotalCost;
                        //    PAIDs.TotalBillValue = item.TotalCost;
                        //    PAIDs.TotalDiscountValue = item.DiscountAmount;
                        //    WYNKContext.PatientAccount.UpdateRange(PAIDs);
                        //}

                        var PAIDs = WYNKContext.PatientAccount.Where(x => x.PAID == paid).FirstOrDefault();
                        PAIDs.TotalProductValue = netamount;
                        PAIDs.TotalBillValue = netamount;
                        PAIDs.TotalDiscountValue = discamount;
                        WYNKContext.PatientAccount.UpdateRange(PAIDs);

                        WYNKContext.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    if (WYNKContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }
                    return new
                    {
                        Success = true,
                        Message = CommonMessage.saved,
                        paid = paid,
                        bill = inv,
                        billdate = DateTime.UtcNow,
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


        public dynamic GetPrintbillingdtlspackage(int paid, int cmpid, string billtype)
        {
            var op = new FinalBillingMaster();
            var uin = WYNKContext.PatientAccount.Where(x => x.PAID == paid && x.CMPID == cmpid && x.InvoiceNumber == billtype).Select(x => x.UIN).FirstOrDefault();
            var pa = WYNKContext.PatientAccount.Where(x => x.PAID == paid && x.CMPID == cmpid && x.InvoiceNumber == billtype).ToList();
            var pad = WYNKContext.PatientAccountDetail.Where(x => x.PAID == paid && x.ServiceDescription == "Surgery Cost").ToList();
            var paymentdetails = WYNKContext.PaymentMaster.Where(x => x.InVoiceNumber == billtype).ToList();
            var gmt = CMPSContext.Setup.Where(x => x.CMPID == cmpid).FirstOrDefault();

            TimeSpan ts = TimeSpan.Parse(gmt.UTCTime);
            op.ServiceDtls = (from cc in pa
                              join cd in pad on cc.PAID equals cd.PAID
                              select new ServiceDtls
                              {
                                  ServiceDescription = WYNKContext.ICDSpecialityCode.Where(x => x.ID == cd.OLMID && x.IsActive == true).Select(x => x.SpecialityDescription).FirstOrDefault(),
                                  Amount = cc.TotalProductValue + cc.TotalDiscountValue,                                  
                                  DiscountAmount = cc.TotalDiscountValue,
                                  GSTAmount = (cc.TotalTaxValue != null ? cc.TotalTaxValue : 0),
                                  TotalCost = cc.TotalBillValue,
                                  CGST = (cc.TotalCGSTTaxValue != null ? cc.TotalCGSTTaxValue : 0),
                                  SGST = (cc.TotalSGSTTaxValue != null ? cc.TotalSGSTTaxValue : 0),
                                  COUNTRY = gmt.Symbol,
                                  PackageBillingLists = Getbreakupbillingsss(cmpid, paid, billtype),
                              }).ToList();

            op.PayDetailss = (from cc in paymentdetails
                              select new PayDetailss
                              {
                                  paymode = cc.PaymentMode,
                                  instdt = cc.Instrumentdate,
                                  instno = cc.InstrumentNumber,
                                  bname = cc.BankName,
                                  branch = cc.BankBranch,
                                  expiry = cc.Expirydate,
                                  amount = cc.Amount,
                                  bildtt = cc.ReceiptDate,
                                  bilnum = cc.ReceiptNumber,
                              }).ToList();
            op.Totalamt = op.PayDetailss.Sum(x => x.amount);
            op.Totalcostyprint = op.ServiceDtls.Sum(x => x.Amount);
            op.totaldicountprint = op.ServiceDtls.Sum(x => x.DiscountAmount);
            op.totaltaxprint = op.ServiceDtls.Sum(x => x.GSTAmount);
            op.totalnetamount = op.ServiceDtls.Sum(x => x.TotalCost);
            op.Admdate = WYNKContext.Admission.OrderByDescending(x =>x.CreatedUTC).Where(x => x.UIN == uin).Select(x => x.AdmDate).FirstOrDefault() + ts;
            op.Admnumber = WYNKContext.Admission.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == uin).Select(x => x.AdmissionNumber).FirstOrDefault();
            op.Invoicedate = WYNKContext.PatientAccount.Where(x => x.PAID == paid && x.CMPID == cmpid && x.InvoiceNumber == billtype).Select(x =>x.InvoiceDate).FirstOrDefault() + ts;
            op.PackageBillingList = op.ServiceDtls.Select(x => x.PackageBillingLists).FirstOrDefault();
            op.minustotalnetamount = op.totalnetamount - op.Totalamt;

            //FinalBilling.Lensused = patientaccounts.Select(x => x.Description).FirstOrDefault();
            //FinalBilling.Procedure = WYNKContext.ICDMaster.Where(x => x.ICDCODE == WYNKContext.SurgeryTran.Where(y => y.SurgeryID == WYNKContext.Surgery.Where(z => z.UIN == UIN && z.CMPID == cmpid).Select(z => z.RandomUniqueID).FirstOrDefault()).Select(y => y.ICDCode).FirstOrDefault()).Select(z => z.ICDDESCRIPTION).FirstOrDefault();

            return op;

        }


        public dynamic Getbreakupbillingsss(int cmpid, int padtid, string billtype)
        {
            var FinalBilling = new FinalBillingMaster();
            var testaccdtl = padtid;                        
            var patientaccountdtltax = WYNKContext.PatientAccountDetailTax.ToList();
            var packagemapping = WYNKContext.PackageMapping.Where(x => x.CMPID == cmpid && x.IsActive == true).ToList();
            var services = WYNKContext.Services.ToList();


            var patientaccountdtls = WYNKContext.PatientAccountDetail.Where(x => x.PAID == testaccdtl).ToList();
            var patientaccounts = WYNKContext.PatientAccount.Where(x => x.PAID == testaccdtl && x.InvoiceNumber == billtype).ToList();

            FinalBilling.PackageBillingListtotals = new List<PackageBillingListtotal>();
            FinalBilling.unPackageBillingLists = new List<unPackageBillingList>();

            var UIN = patientaccounts.Select(x => x.UIN).FirstOrDefault();

            FinalBilling.Lensused = patientaccounts.Select(x => x.Description).FirstOrDefault();
            FinalBilling.Procedure = WYNKContext.ICDMaster.Where(x => x.ICDCODE == WYNKContext.SurgeryTran.Where(y => y.SurgeryID == WYNKContext.Surgery.Where(z => z.UIN == UIN && z.CMPID == cmpid).Select(z => z.RandomUniqueID).FirstOrDefault()).Select(y => y.ICDCode).FirstOrDefault()).Select(z => z.ICDDESCRIPTION).FirstOrDefault();
            var dada = (from gm in patientaccounts
                        join cc in patientaccountdtls on gm.PAID equals cc.PAID
                        join pt in patientaccountdtltax on cc.PAccDetailID equals pt.PAccDetailID
                        select new
                        {
                            padid = gm.PAID,
                        }).ToList();
            if (dada.Count() > 0)
            {
                var nonservicedetails = (from gm in patientaccounts
                                         join cc in patientaccountdtls.Where(x => x.ServiceDescription != "Surgery Cost") on gm.PAID equals cc.PAID
                                         join padx in patientaccountdtltax on cc.PAccDetailID equals padx.PAccDetailID
                                         select new
                                         {
                                             serviceid = padx.ServiceTypeID,
                                             pacctaxid = padx.PAccDetailTaxID,
                                             padetailid = padx.PAccDetailID,
                                             paidid = testaccdtl,
                                             uin = gm.UIN,
                                             amount = padx.TotalProductValue,
                                             ServiceDescription = cc.ServiceDescription,
                                         }).ToList();
                var Servicedetails = (from gm in patientaccounts
                                      join cc in patientaccountdtls.Where(x => x.ServiceDescription == "Surgery Cost") on gm.PAID equals cc.PAID
                                      select new
                                      {
                                          gst = cc.TotalTaxValue,
                                          discount = cc.TotalDiscountValue,
                                          padetailid = cc.PAccDetailID,
                                          paidid = testaccdtl,
                                          uin = gm.UIN,
                                          amount = cc.TotalProductValue,
                                          ServiceTypeID = cc.ServiceTypeID,
                                          Description = cc.ServiceDescription,
                                          ServiceDescription = services.Where(y => y.ID == cc.ServiceTypeID).Select(y => y.Description).FirstOrDefault(),
                                      }).ToList();


                foreach (var iitem in Servicedetails)
                {
                    var packagerelateddata = new PackageBillingListtotal();
                    packagerelateddata.Date = WYNKContext.Surgery.Where(x => x.UIN == iitem.uin && x.CMPID == cmpid).Select(x => x.DateofSurgery).FirstOrDefault();
                    packagerelateddata.Description = iitem.Description;
                    packagerelateddata.ServiceDescription = services.Where(y => y.ID == iitem.ServiceTypeID).Select(y => y.Description).FirstOrDefault();
                    packagerelateddata.Amount = iitem.amount;
                    packagerelateddata.Dummyamout = 0;
                    FinalBilling.PackageBillingListtotals.Add(packagerelateddata);
                }

                foreach (var item in nonservicedetails)
                {
                    var pid = packagemapping.Where(x => x.ServiceID == item.serviceid).FirstOrDefault();
                    if (pid != null)
                    {
                        var packagerelateddata = new PackageBillingListtotal();
                        packagerelateddata.Date = WYNKContext.Surgery.Where(x => x.UIN == item.uin && x.CMPID == cmpid).Select(x => x.DateofSurgery).FirstOrDefault();
                        packagerelateddata.Description = item.ServiceDescription;
                        packagerelateddata.ServiceDescription = services.Where(y => y.ID == item.serviceid).Select(y => y.Description).FirstOrDefault();
                        packagerelateddata.Amount = item.amount;
                        packagerelateddata.Dummyamout = item.amount;
                        FinalBilling.PackageBillingListtotals.Add(packagerelateddata);
                    }
                    else
                    {
                        var packagerelateddata = new PackageBillingListtotal();
                        packagerelateddata.Date = WYNKContext.Surgery.Where(x => x.UIN == item.uin && x.CMPID == cmpid).Select(x => x.DateofSurgery).FirstOrDefault();
                        packagerelateddata.Description = item.ServiceDescription;
                        packagerelateddata.ServiceDescription = services.Where(y => y.ID == item.serviceid).Select(y => y.Description).FirstOrDefault();
                        packagerelateddata.Amount = item.amount;
                        packagerelateddata.Dummyamout = item.amount;
                        FinalBilling.PackageBillingListtotals.Add(packagerelateddata);
                    }
                }


                FinalBilling.PackageBillingList = (from sd in Servicedetails
                                                   select new PackageBillingList
                                                   {
                                                       Discount = (sd.discount != null ? sd.discount : 0),
                                                       TempDiscount = (sd.discount != null ? sd.discount : 0),
                                                       GSTAmount = (sd.gst != null ? sd.gst : 0),
                                                       Description = sd.ServiceDescription,
                                                       PackageBillingListtotal = FinalBilling.PackageBillingListtotals,
                                                       //unPackageBillingList = FinalBilling.unPackageBillingLists,
                                                       TotalCost = (FinalBilling.PackageBillingListtotals.Sum(x => x.Amount) != null ? FinalBilling.PackageBillingListtotals.Sum(x => x.Amount) : 0),
                                                       Amount = (FinalBilling.PackageBillingListtotals.Sum(x => x.Amount) != null ? FinalBilling.PackageBillingListtotals.Sum(x => x.Amount) : 0),
                                                   }).ToList();
            }
            else
            {
                var Servicedetails = (from gm in patientaccounts
                                      join cc in patientaccountdtls.Where(x => x.ServiceDescription == "Surgery Cost") on gm.PAID equals cc.PAID
                                      select new
                                      {
                                          gst = cc.TotalTaxValue,
                                          discount = cc.TotalDiscountValue,
                                          padetailid = cc.PAccDetailID,
                                          paidid = testaccdtl,
                                          uin = gm.UIN,
                                          amount = cc.TotalProductValue,
                                          ServiceTypeID = cc.ServiceTypeID,
                                          Description = cc.ServiceDescription,
                                          ServiceDescription = services.Where(y => y.ID == cc.ServiceTypeID).Select(y => y.Description).FirstOrDefault(),
                                      }).ToList();
                foreach (var iitem in Servicedetails)
                {
                    var packagerelateddata = new PackageBillingListtotal();
                    packagerelateddata.Date = WYNKContext.Surgery.Where(x => x.UIN == iitem.uin && x.CMPID == cmpid).Select(x => x.DateofSurgery).FirstOrDefault();
                    packagerelateddata.Description = iitem.Description;
                    packagerelateddata.ServiceDescription = services.Where(y => y.ID == iitem.ServiceTypeID).Select(y => y.Description).FirstOrDefault();
                    packagerelateddata.Amount = iitem.amount;
                    packagerelateddata.Dummyamout = 0;
                    FinalBilling.PackageBillingListtotals.Add(packagerelateddata);
                }

                FinalBilling.PackageBillingList = (from sd in Servicedetails
                                                   select new PackageBillingList
                                                   {
                                                       Discount = (sd.discount != null ? sd.discount : 0),
                                                       TempDiscount = (sd.discount != null ? sd.discount : 0),
                                                       GSTAmount = (sd.gst != null ? sd.gst : 0),
                                                       Description = sd.ServiceDescription,
                                                       PackageBillingListtotal = FinalBilling.PackageBillingListtotals,
                                                       //unPackageBillingList = FinalBilling.unPackageBillingLists,
                                                       TotalCost = (FinalBilling.PackageBillingListtotals.Sum(x => x.Amount) != null ? FinalBilling.PackageBillingListtotals.Sum(x => x.Amount) : 0),
                                                       Amount = (FinalBilling.PackageBillingListtotals.Sum(x => x.Amount) != null ? FinalBilling.PackageBillingListtotals.Sum(x => x.Amount) : 0),
                                                   }).ToList();
            }



            //var NonServiceDtlsss = (from gm in patientaccount
            //                        join cc in patientaccountdtl on gm.PAID equals cc.PAID
            //                        join pt in patientaccountdtltax on cc.PAccDetailID equals pt.PAccDetailID                                    
            //                        select new
            //                        {
            //                            serviceid = pt.ServiceTypeID,
            //                            paccdtltaxid = pt.PAccDetailTaxID,
            //                            discount = 0,
            //                            Description = cc.ServiceDescription,
            //                            ServiceDescription = services.Where(y => y.ID == cc.ServiceTypeID).Select(y => y.Description).FirstOrDefault(),
            //                        }).ToList();

            //foreach (var item in NonServiceDtlsss)
            //{
            //    var pid = packagemapping.Where(x => x.ServiceID == item.serviceid).FirstOrDefault();
            //    if (pid != null)
            //    {
            //        var packagerelateddata = new PackageBillingListtotal();
            //        packagerelateddata.Date = WYNKContext.Surgery.Where(x => x.UIN == item.uin && x.CMPID == cmpid).Select(x => x.DateofSurgery).FirstOrDefault();
            //        packagerelateddata.Description = item.ServiceDescription;
            //        packagerelateddata.ServiceDescription = services.Where(y => y.ID == item.serviceid).Select(y => y.Description).FirstOrDefault();
            //        packagerelateddata.Amount = item.amount;
            //        packagerelateddata.Dummyamout = 0;
            //        FinalBilling.PackageBillingListtotals.Add(packagerelateddata);
            //    }
            //    else
            //    {
            //        var packagerelateddata = new PackageBillingListtotal();
            //        packagerelateddata.Date = WYNKContext.Surgery.Where(x => x.UIN == item.uin && x.CMPID == cmpid).Select(x => x.DateofSurgery).FirstOrDefault();
            //        packagerelateddata.Description = item.ServiceDescription;
            //        packagerelateddata.ServiceDescription = services.Where(y => y.ID == item.serviceid).Select(y => y.Description).FirstOrDefault();
            //        packagerelateddata.Amount = item.amount;
            //        packagerelateddata.Dummyamout = item.amount;
            //        FinalBilling.PackageBillingListtotals.Add(packagerelateddata);
            //    }
            //}



            //var ServiceDtlsss = (from cc in WYNKContext.PatientAccountDetail.Where(x => x.ServiceDescription == "Surgery Cost" && x.PAID == testaccdtl)
            //                     select new
            //                     {
            //                         paccdtltaxid = cc.PAccDetailID,
            //                         discount = 0,
            //                         Description = cc.ServiceDescription,
            //                         ServiceDescription = services.Where(y => y.ID == cc.ServiceTypeID).Select(y => y.Description).FirstOrDefault(),
            //                     }).ToList();

            //var merged = ServiceDtlsss.Union(NonServiceDtlsss);
            //var viceDtls = (from cv in merged
            //                select new ServiceDtlsprintNew
            //                {
            //                    Discount = cv.discount,
            //                    Description = cv.Description,
            //                    ServiceDescription = cv.ServiceDescription,
            //                }).ToList();
            return FinalBilling.PackageBillingList;
        }
        public dynamic UpdatePackageBilling(FinalBillingMaster payment, int cmpid, int padtid)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    if (payment.ItmDtls.Count() > 0)
                    {
                        foreach (var item in payment.ItmDtls.ToList())
                        {
                            if (item.Description == "Surgery Cost" && (item.DiscountAmount != 0 || item.TaxID != 0))
                            {
                                var pddaat = WYNKContext.PatientAccountDetail.Where(x => x.PAccDetailID == item.paccdtltaxid).FirstOrDefault();
                                pddaat.TotalProductValue = item.TotalCost;
                                pddaat.TotalDiscountValue = item.DiscountAmount;
                                pddaat.TotalTaxValue = item.GSTAmount;
                                pddaat.TotalCGSTTaxValue = (item.GSTAmount) / 2;
                                pddaat.TotalSGSTTaxValue = (item.GSTAmount) / 2;
                                pddaat.TotalBillValue = item.TotalCost;
                                pddaat.CESSValue = item.CESSAmount;
                                pddaat.AdditonalCESSValue = item.AdditionalCESSAmount;
                                WYNKContext.PatientAccountDetail.UpdateRange(pddaat);
                                WYNKContext.SaveChanges();
                            }
                            else
                            {
                                if ((item.DiscountAmount != 0 || item.TaxID != 0) && item.Description != "Surgery Cost")
                                {
                                    var apddaat = WYNKContext.PatientAccountDetailTax.Where(x => x.PAccDetailTaxID == item.paccdtltaxid).FirstOrDefault();
                                    apddaat.TaxID = item.TaxID;
                                    apddaat.TaxPercentage = CMPSContext.TaxMaster.Where(x => x.ID == item.TaxID).Select(x => x.GSTPercentage).FirstOrDefault();
                                    apddaat.TotalProductValue = item.TotalCost;
                                    apddaat.TotalDiscountValue = item.DiscountAmount;
                                    apddaat.TotalTaxValue = item.GSTAmount;
                                    apddaat.TotalCGSTTaxValue = (item.GSTAmount) / 2;
                                    apddaat.TotalSGSTTaxValue = (item.GSTAmount) / 2;
                                    apddaat.AdditionalCESSValue = item.AdditionalCESSAmount;
                                    apddaat.CESSValue = item.CESSAmount;
                                    apddaat.UpdatedUTC = DateTime.UtcNow;
                                    apddaat.AdditionalCESSValue = item.AdditionalCESSAmount;
                                    WYNKContext.PatientAccountDetailTax.UpdateRange(apddaat);
                                    WYNKContext.SaveChanges();

                                    var pddaatss = WYNKContext.PatientAccountDetailTax.Where(x => x.PAccDetailTaxID == item.paccdtltaxid).Select(x => x.PAccDetailID).FirstOrDefault();
                                    var pddaat = WYNKContext.PatientAccountDetail.Where(x => x.PAccDetailID == pddaatss).FirstOrDefault();
                                    pddaat.TotalProductValue = WYNKContext.PatientAccountDetailTax.Where(x => x.PAccDetailTaxID == item.paccdtltaxid).Sum(x => x.TotalProductValue);
                                    pddaat.TotalDiscountValue = WYNKContext.PatientAccountDetailTax.Where(x => x.PAccDetailTaxID == item.paccdtltaxid).Sum(x => x.TotalDiscountValue);
                                    pddaat.TotalTaxValue = WYNKContext.PatientAccountDetailTax.Where(x => x.PAccDetailTaxID == item.paccdtltaxid).Sum(x => x.TotalTaxValue);
                                    pddaat.TotalCGSTTaxValue = WYNKContext.PatientAccountDetailTax.Where(x => x.PAccDetailTaxID == item.paccdtltaxid).Sum(x => x.TotalCGSTTaxValue);
                                    pddaat.TotalSGSTTaxValue = WYNKContext.PatientAccountDetailTax.Where(x => x.PAccDetailTaxID == item.paccdtltaxid).Sum(x => x.TotalSGSTTaxValue);
                                    pddaat.TotalBillValue = WYNKContext.PatientAccountDetailTax.Where(x => x.PAccDetailTaxID == item.paccdtltaxid).Sum(x => x.TotalProductValue);
                                    pddaat.CESSValue = WYNKContext.PatientAccountDetailTax.Where(x => x.PAccDetailTaxID == item.paccdtltaxid).Sum(x => x.CESSValue);
                                    pddaat.AdditonalCESSValue = WYNKContext.PatientAccountDetailTax.Where(x => x.PAccDetailTaxID == item.paccdtltaxid).Sum(x => x.AdditionalCESSValue);
                                    WYNKContext.PatientAccountDetail.UpdateRange(pddaat);
                                    WYNKContext.SaveChanges();
                                }
                            }

                        }

                        var paid = WYNKContext.PatientAccountDetail.Where(x => x.PAccDetailID == padtid).Select(x => x.PAID).FirstOrDefault();
                        var sapddaat = WYNKContext.PatientAccount.Where(x => x.PAID == paid).FirstOrDefault();
                        sapddaat.TotalProductValue = WYNKContext.PatientAccountDetail.Where(x => x.PAID == paid).Sum(x => x.TotalProductValue);
                        sapddaat.TotalDiscountValue = WYNKContext.PatientAccountDetail.Where(x => x.PAID == paid).Sum(x => x.TotalDiscountValue);
                        sapddaat.TotalTaxValue = WYNKContext.PatientAccountDetail.Where(x => x.PAID == paid).Sum(x => x.TotalTaxValue);
                        sapddaat.TotalCGSTTaxValue = WYNKContext.PatientAccountDetail.Where(x => x.PAID == paid).Sum(x => x.TotalCGSTTaxValue);
                        sapddaat.TotalSGSTTaxValue = WYNKContext.PatientAccountDetail.Where(x => x.PAID == paid).Sum(x => x.TotalSGSTTaxValue);
                        sapddaat.TotalBillValue = WYNKContext.PatientAccountDetail.Where(x => x.PAID == paid).Sum(x => x.TotalBillValue);
                        sapddaat.CESSValue = WYNKContext.PatientAccountDetail.Where(x => x.PAID == paid).Sum(x => x.CESSValue);
                        sapddaat.AdditionalCESSValue = WYNKContext.PatientAccountDetail.Where(x => x.PAID == paid).Sum(x => x.AdditonalCESSValue);
                        WYNKContext.PatientAccount.UpdateRange(sapddaat);
                        WYNKContext.SaveChanges();
                    }
                    if (WYNKContext.SaveChanges() >= 0)
                    {
                        dbContextTransaction.Commit();
                        return new
                        {
                            Success = true,
                        };
                    }
                }
                catch (Exception)
                {
                    dbContextTransaction.Commit();
                    return new
                    {
                        Success = false,
                    };
                }
                return new
                {
                    Success = false,
                };
            }

        }

        public dynamic UpdateBilling(FinalBillingMaster payment, int cmpid, int padtid)
        {
            try
            {
                var parid = WYNKContext.PatientAccountDetail.Where(x => x.PAccDetailID == padtid).OrderByDescending(x => x.CreatedUTC).Select(r => r.PAccDetailID).FirstOrDefault();

                if (parid != null)
                {

                    if (payment.ItmDtls.Count() > 0)
                    {
                        foreach (var item in payment.ItmDtls.ToList())
                        {
                            var fd = new PatientAccountDetailTax();

                            if (item.paccdtltaxid != 0)
                            {
                                fd = WYNKContext.PatientAccountDetailTax.Where(x => x.PAccDetailTaxID == item.paccdtltaxid).FirstOrDefault();
                                fd.TaxID = item.TaxID;
                                fd.TotalTaxValue = item.GSTAmount;
                                fd.CESSValue = item.CESSAmount;
                                fd.AdditionalCESSValue = item.AdditionalCESSAmount;
                                fd.TotalDiscountValue = item.DiscountAmount;
                                fd.TotalValue = Convert.ToDecimal(item.TotalCost);
                                WYNKContext.PatientAccountDetailTax.UpdateRange(fd);
                            }

                        }
                    }

                }

                var paid = WYNKContext.PatientAccountDetail.Where(x => x.PAccDetailID == padtid).OrderByDescending(x => x.CreatedUTC).Select(r => r.PAccDetailID).FirstOrDefault();

                if (paid != null)
                {


                    var master = WYNKContext.PatientAccountDetail.Where(x => x.PAccDetailID == padtid).ToList();
                    if (master != null)
                    {

                        master.All(x =>
                        {
                            x.TotalBillValue = 0;
                            x.TotalDiscountValue = 0; x.TotalTaxValue = 0; x.CESSValue = 0; x.AdditonalCESSValue = 0; return true;
                        });
                        WYNKContext.PatientAccountDetail.UpdateRange(master);
                    }
                    WYNKContext.SaveChanges();
                    if (payment.ItmDtls.Count() > 0)
                    {
                        foreach (var item in payment.ItmDtls.ToList())
                        {
                            var fd = new PatientAccountDetail();

                            if (item.paccdtltaxid != 0)
                            {
                                fd = WYNKContext.PatientAccountDetail.Where(x => x.PAccDetailID == padtid).FirstOrDefault();
                                fd.TotalBillValue += item.TotalCost;
                                fd.TotalDiscountValue += (item.DiscountAmount != null ? item.DiscountAmount : 0);
                                fd.TotalTaxValue += (item.GSTAmount != null ? item.GSTAmount : 0);
                                fd.CESSValue += (item.CESSAmount != null ? item.CESSAmount : 0);
                                fd.AdditonalCESSValue += (item.AdditionalCESSAmount != null ? item.AdditionalCESSAmount : 0);
                                WYNKContext.PatientAccountDetail.UpdateRange(fd);
                            }

                        }
                    }

                }



                WYNKContext.SaveChanges();
                return new
                {
                    Success = true,
                };
            }
            catch (Exception)
            {
                return new
                {
                    Success = false,
                };
            }
        }

    }
}
