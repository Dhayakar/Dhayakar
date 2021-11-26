using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class AdvancePaymentRepository : RepositoryBase<AdvancePaymentViewModel>, IAdvancePaymentRepository
    {


        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public AdvancePaymentRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }




        public dynamic getADVRePrint(string UIN, int cmpid)
        {

            var advancePayment = new AdvancePaymentViewModel();
            advancePayment.ADVreP = (from pay in WYNKContext.PaymentMaster.Where(x => x.UIN == UIN && x.CmpID == cmpid && x.PaymentType == "A").GroupBy(x => x.ReceiptNumber)
                                     select new ADVreP
                                     {


                                         ReceiptNumber = pay.Key,
                                         ReceiptDate = pay.Select(x => x.ReceiptDate).FirstOrDefault(),
                                     }).OrderByDescending(x => x.ReceiptNumber).ToList();



            return advancePayment;
        }
        public dynamic getADVRePrintEdit(string ReceiptNumber)
        {

            var advancePayment = new AdvancePaymentViewModel();
            advancePayment.ADVrePEdit = (from pay in WYNKContext.PaymentMaster.Where(x => x.ReceiptNumber == ReceiptNumber)
                                         select new ADVrePEdit
                                         {
                                             PaymentMode = pay.PaymentMode,
                                             InstrumentNumber = pay.InstrumentNumber,
                                             Instrumentdate = pay.Instrumentdate,
                                             BankName = pay.BankName,
                                             Expirydate = pay.Expirydate,
                                             BankBranch = pay.BankBranch,
                                             Amount = pay.Amount,
                                             TAmount = WYNKContext.PaymentMaster.Where(x => x.ReceiptNumber == ReceiptNumber).Select(x => x.Amount).Sum(),
                                         }).ToList();



            return advancePayment;
        }
        public dynamic getADVRePrintF(string ReceiptNumber, int cmpid)
        {

            var advancePayment = new AdvancePaymentViewModel();
            var PaymentMaster = WYNKContext.PaymentMaster.ToList();
            var Company = CMPSContext.Company.ToList();

            advancePayment.ADVrePF = (from pay in PaymentMaster.Where(x => x.ReceiptNumber == ReceiptNumber && x.CmpID==cmpid && x.PaymentType =="A")
                                      select new ADVrePF
                                      {
                                          PaymentMode = pay.PaymentMode,
                                          InstrumentNumber = pay.InstrumentNumber,
                                          Instrumentdate = pay.Instrumentdate,
                                          BankName = pay.BankName,
                                          Expirydate = pay.Expirydate,
                                          BankBranch = pay.BankBranch,
                                          Amount = pay.Amount,
                                          PAddress = Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1 != null ? x.Address1 : string.Empty).FirstOrDefault(),
                                          PAddress2 = Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2 != null ? x.Address2 : string.Empty).FirstOrDefault(),
                                          PAddress3 = Company.Where(x => x.CmpID == cmpid).Select(x => x.Address3 != null ? x.Address3 : string.Empty).FirstOrDefault(),
                                          Pphone = Company.Where(x => x.CmpID == cmpid).Select(x => x.Phone1).FirstOrDefault(),
                                          Pweb = Company.Where(x => x.CmpID == cmpid).Select(x => x.Website).FirstOrDefault(),
                                          PCompnayname = Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault(),




                                      }).ToList();



            return advancePayment;
        }
        public dynamic AddAdvance(AdvancePaymentViewModel AddBill, int CompanyID, int TransactionTypeid)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                var payment = new Payment_Master();
                try
                {

                    if (AddBill.paymenttran.Count > 0)
                    {

                        foreach (var item in AddBill.paymenttran.ToList())
                        {
                            var payment1 = new Payment_Master();
                            payment1.UIN = AddBill.payment.UIN;
                            payment1.ReceiptNumber = AddBill.payment.ReceiptNumber;
                            payment1.CmpID = CompanyID;
                            var Datee = DateTime.Now;
                            payment1.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= Datee && Convert.ToDateTime(b.FYTo) >= Datee && b.CMPID == CompanyID && b.IsActive == true).Select(f => f.ID).FirstOrDefault()).Select(c => c.FYAccYear).FirstOrDefault());
                            payment1.PaymentType = "A";
                            payment1.PaymentMode = item.PaymentMode;
                            payment1.InstrumentNumber = item.InstrumentNumber;

                            if (item.Instrumentdate != null)
                            {
                                payment1.Instrumentdate = Convert.ToDateTime(item.Instrumentdate);
                            }
                            else
                            {
                                payment1.Instrumentdate = null;
                            }
                            if (item.Expirydate != null)
                            {
                                payment1.Expirydate = Convert.ToDateTime(item.Expirydate);
                            }
                            else
                            {
                                payment1.Expirydate = null;
                            }
                            payment1.TransactionID = TransactionTypeid;
                            payment1.BankBranch = item.BankBranch;
                            payment1.BankName = item.BankName;
                            payment1.Amount = Convert.ToDecimal(item.Amount);
                            payment1.ReceiptDate = AddBill.payment.ReceiptDate;
                            payment1.IsBilled = false;
                            payment1.CreatedBy = AddBill.uid;
                            payment1.InVoiceDate = null;
                            payment1.CreatedUTC = DateTime.UtcNow;
                            WYNKContext.PaymentMaster.Add(payment1);
                            WYNKContext.SaveChanges();
                        }

                    }
                   
                    var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                    var RunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, CompanyID, "GetRunningNo");
                    if (RunningNumber == AddBill.payment.ReceiptNumber)
                    {
                        commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, CompanyID, "UpdateRunningNo");
                    }
                    else
                    {
                        var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, CompanyID, "UpdateRunningNo");
                       
                        payment = WYNKContext.PaymentMaster.Where(x => x.ReceiptNumber == AddBill.payment.ReceiptNumber).FirstOrDefault();
                        payment.ReceiptNumber = GetRunningNumber;
                        WYNKContext.Entry(payment).State = EntityState.Modified;
                        WYNKContext.SaveChanges();
                    }
                    dbContextTransaction.Commit();
                    return new
                    {
                        Success = true,
                        Message = CommonMessage.saved,
                        ReceiptNumber = AddBill.payment.ReceiptNumber,
                        ReceiptDate = AddBill.payment.ReceiptDate,
                        uin = AddBill.payment.UIN,
                        PCompnayname = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault(),
                        PAddress = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.Address1 != null ? x.Address1 : string.Empty).FirstOrDefault(),
                        PAddress2 = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.Address2 != null ? x.Address2 : string.Empty).FirstOrDefault(),
                        PAddress3 = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.Address3 != null ? x.Address3 : string.Empty).FirstOrDefault(),
                        Pphone = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.Phone1).FirstOrDefault(),
                        Pweb = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.Website).FirstOrDefault(),


                    };

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                    string msg = ex.InnerException.Message;
                    return new { Success = false, Message = msg, Adv = AddBill.payment.ReceiptNumber };
                }
            }
        }






    }
}













