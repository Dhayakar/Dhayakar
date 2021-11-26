using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class CustomerOrderRepository : RepositoryBase<CustomerOrderViewModel>, ICustomerOrderRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        public CustomerOrderRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic GetOfferDetail(int CMPID, int LMID, int LTID)
        {
            try
            {
                var OfferAvailable = WYNKContext.Offer.Where(x => x.From.Date <= DateTime.Now.Date && x.To.Date >= DateTime.Now.Date && x.IsActive == true).FirstOrDefault();

                if (OfferAvailable != null)
                {
                    if (OfferAvailable.IsAllbrand == true && OfferAvailable.IsAllModel == true)
                    {
                        return new
                        {
                            Success = true,
                            Message = "Offer Details",
                            DiscountPercent = OfferAvailable.DiscountPercentage
                        };
                    }
                    else
                    {

                        var offerTran = WYNKContext.OfferTran.Where(x => x.OFID == OfferAvailable.ID && x.LMID == LMID && x.LTID == LTID).FirstOrDefault();

                        if (offerTran == null)
                        {
                            return new
                            {
                                Success = false,
                                Message = "No Offers Available"
                            };
                        }

                        if (offerTran.OfferType == "Discount")
                        {
                            return new
                            {
                                Success = true,
                                Message = "Offer Details",
                                DiscountPercent = offerTran.DiscountPercentage
                            };
                        }

                        else if (offerTran.OfferType == "One Plus Offer")
                        {
                            var offerTranExtension = WYNKContext.OfferTranExtension.Where(x => x.OfferID == OfferAvailable.ID && x.OfferTranID == offerTran.ID).Select(x => x.OfferLTID).ToList();

                            var OnePlusOfferValue = offerTran.OnePlusOfferValue;

                            var TaxMas = CMPSContext.TaxMaster.ToList();
                            var UOM = CMPSContext.uommaster.ToList();

                            var OfferDetails = (from LM in WYNKContext.Lensmaster
                                                join LT in WYNKContext.Lenstrans on LM.RandomUniqueID equals LT.LMID
                                                join BM in WYNKContext.Brand on LT.Brand equals BM.ID
                                                where offerTranExtension.Contains(LT.ID)
                                                select new OfferDetail
                                                {
                                                    LTID = LT.ID,
                                                    LMID = LM.ID,
                                                    Brand = BM.Description,

                                                    Description = LM.Description,
                                                    Index = LT.Index,
                                                    Color = LT.Colour,
                                                    Size = LT.Size,
                                                    Price = LT.Prize,
                                                    Model = LT.Model,
                                                    GST = TaxMas.Where(x => x.ID == LM.TaxID).Select(x => x.GSTPercentage).FirstOrDefault(),
                                                    CESS = LT.CESSAmount,
                                                    AddCess = LT.ADDCESSAmount,
                                                    TaxIDD = LM.TaxID,
                                                    Type = LM.LensType,
                                                    HSNNo = LT.HSNNo,
                                                    UOM = UOM.Where(x => x.id == LT.UOMID).Select(x => x.Description).FirstOrDefault(),
                                                }).ToList();



                            return new
                            {
                                Success = true,
                                Message = "Offer Details",
                                OnePlusOfferDetails = "OnePlus Offer",
                                OfferDetails = OfferDetails,
                                OnePlusOfferValue = OnePlusOfferValue
                            };
                        }
                    }

                }

                return new
                {
                    Success = false,
                    Message = "No Offers Available"
                };
            }
            catch (Exception ex)
            {

                Console.Write(ex);
            }
            return new
            {
                Success = false,
            };

        }

        public decimal? DiscountOffer(int LensMasterID, int LensTranId, decimal Price)
        {
            var OfferAvailable = WYNKContext.Offer.Where(x => x.From.Date <= DateTime.Now.Date && x.To.Date >= DateTime.Now.Date && x.IsActive == true).ToList();


            if (OfferAvailable != null)
            {
                foreach (var offers in OfferAvailable.ToList())
                {

                    var offerID = offers.ID;

                    decimal? DiscPrecentage = WYNKContext.OfferTran.Where(x => x.OFID == offerID && x.LMID == LensMasterID && x.LTID == LensTranId).Select(x => x.DiscountPercentage).FirstOrDefault();

                    if (DiscPrecentage == null)
                    {
                        return null;
                    }
                    else
                    {
                        return Price * DiscPrecentage;
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        public dynamic SubmitCustomerOrder(CustomerOrderViewModel CustomerOrderDetails)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var UOM = CMPSContext.uommaster.ToList();
                    var CMP = CMPSContext.Company.Where(x => x.CmpID == CustomerOrderDetails.Cmpid).FirstOrDefault();

                    var CustomerOrder = new CustomerOrder();
                    CustomerOrder.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                    CustomerOrder.CmpID = CustomerOrderDetails.Cmpid;
                    CustomerOrder.CusID = CustomerOrderDetails.CustomerId;
                    CustomerOrder.TCID = CustomerOrderDetails.Tc;
                    CustomerOrder.OrderNo = CustomerOrderDetails.RunningNo;
                    CustomerOrder.OrderDate = DateTime.UtcNow;
                    CustomerOrder.RefNo = Convert.ToString(CustomerOrderDetails.RefNo);
                    CustomerOrder.RefDate = CustomerOrderDetails.RefDate;
                    CustomerOrder.Deliverydate = CustomerOrderDetails.Deliverydate != null ? Convert.ToDateTime(CustomerOrderDetails.Deliverydate).AddDays(1) : (DateTime?)null;
                    CustomerOrder.Remarks = CustomerOrderDetails.Remarks;
                    CustomerOrder.IsCancelled = false;
                    CustomerOrder.OrderStatus = "Open";
                    CustomerOrder.Fyear = CustomerOrderDetails.Fyear;
                    CustomerOrder.CreatedUTC = DateTime.UtcNow;
                    CustomerOrder.CreatedBy = CustomerOrderDetails.CreatedBy;
                    WYNKContext.CustomerOrder.Add(CustomerOrder);
                   

                    string cmpname = CMP.CompanyName;
                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == CustomerOrderDetails.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    string userid = Convert.ToString(CustomerOrderDetails.CreatedBy);
                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(cmpname, "CUSTOMER ORDER", "User name :", username, "User ID :", userid, "Mode : Sumbit");

                    object names = CustomerOrder;
                    oErrorLogs.WriteErrorLogArray("CustomerOrder", names);

                   


                    foreach (var item in CustomerOrderDetails.CustomerItemOrders.ToList())
                    {
                        var CustomerOrderTran = new CustomerOrderTran();
                     
                        CustomerOrderTran.COID = CustomerOrder.RandomUniqueID;
                        CustomerOrderTran.LTID = item.LTID;
                        CustomerOrderTran.UOMID = UOM.Where(x => x.Description == item.UOM).Select(x => x.id).FirstOrDefault();
                        CustomerOrderTran.OrderedQty = item.Quantity;
                        CustomerOrderTran.ItemRate = item.UnitPrice;
                        CustomerOrderTran.GrossValue = item.GrossAmount;
                        CustomerOrderTran.ItemValue = item.Amount;
                        CustomerOrderTran.DiscountPercentage = item.Discount;
                        CustomerOrderTran.DiscountAmount = item.DiscountAmount;
                        CustomerOrderTran.GSTPercentage = item.SGST + item.CGST;
                        CustomerOrderTran.GSTTaxValue = item.GrossAmount * (CustomerOrderTran.GSTPercentage / 100);
                        CustomerOrderTran.CGSTPercentage = item.CGST;
                        CustomerOrderTran.CGSTTaxValue = CustomerOrderTran.GSTTaxValue / 2;
                        CustomerOrderTran.SGSTPercentage = item.SGST;
                        CustomerOrderTran.SGSTTaxValue = CustomerOrderTran.GSTTaxValue / 2;
                        CustomerOrderTran.CESSPercentage = item.CESS;
                        CustomerOrderTran.CESSAmount = item.CESS != null ? item.CESSValue : null;
                        CustomerOrderTran.AdditionalCESSPercentage = item.AddCess;
                        CustomerOrderTran.AddCESSPerAmt = item.AddCess != null ? item.AddCessValue : null;
                        CustomerOrderTran.IsCancelled = false;
                        CustomerOrderTran.ReceivedQty = 0;
                        CustomerOrderTran.CreatedUTC = DateTime.UtcNow;
                        CustomerOrderTran.CreatedBy = CustomerOrderDetails.CreatedBy;
                        WYNKContext.CustomerOrderTran.Add(CustomerOrderTran);

                        ErrorLog oErrorLogstran = new ErrorLog();
                        object CustomerOrderTra = CustomerOrderTran;
                        oErrorLogstran.WriteErrorLogArray("CustomerOrderTran", CustomerOrderTra);

                        WYNKContext.SaveChanges();

                        var CustomerOrderUpdate = WYNKContext.CustomerOrder.Where(x => x.RandomUniqueID == CustomerOrder.RandomUniqueID).FirstOrDefault();
                        CustomerOrderUpdate.GrossProductValue = CustomerOrderUpdate.GrossProductValue != null ? (CustomerOrderUpdate.GrossProductValue + item.GrossAmount) : 0 + item.GrossAmount;
                        CustomerOrderUpdate.TotalDiscountValue = CustomerOrderUpdate.TotalDiscountValue != null ? (CustomerOrderUpdate.TotalDiscountValue + item.DiscountAmount) : 0 + item.DiscountAmount;
                        CustomerOrderUpdate.TotalTaxvalue = CustomerOrderUpdate.TotalTaxvalue != null ? (CustomerOrderUpdate.TotalTaxvalue + CustomerOrderTran.GSTTaxValue) : 0 + CustomerOrderTran.GSTTaxValue;
                        CustomerOrderUpdate.TotalCGSTTaxValue = CustomerOrderUpdate.TotalCGSTTaxValue != null ? (CustomerOrderUpdate.TotalCGSTTaxValue + CustomerOrderTran.CGSTTaxValue) : 0 + CustomerOrderTran.CGSTTaxValue;
                        CustomerOrderUpdate.TotalSGSTTaxValue = CustomerOrderUpdate.TotalSGSTTaxValue != null ? (CustomerOrderUpdate.TotalSGSTTaxValue + CustomerOrderTran.SGSTTaxValue) : 0 + CustomerOrderTran.SGSTTaxValue;
                        CustomerOrderUpdate.TotalCESSValue = CustomerOrderUpdate.TotalCESSValue != null ? (CustomerOrderUpdate.TotalCESSValue + item.CESSValue) : 0 + item.CESSValue;
                        CustomerOrderUpdate.TotalAddCESSValue = CustomerOrderUpdate.TotalAddCESSValue != null ? (CustomerOrderUpdate.TotalAddCESSValue + item.AddCessValue) : 0 + item.AddCessValue;
                        CustomerOrderUpdate.TotalProductValue = CustomerOrderUpdate.TotalProductValue != null ? (CustomerOrderUpdate.TotalProductValue + item.Amount + item.CESSValue + item.AddCessValue) : 0 + item.Amount;
                        WYNKContext.CustomerOrder.UpdateRange(CustomerOrderUpdate);


                        object namestr = CustomerOrderUpdate;
                        oErrorLogstran.WriteErrorLogArray("CustomerOrder", namestr);
                    }
          

                    if (CustomerOrderDetails.paymenttran.Count > 0)
                    {
                        foreach (var item1 in CustomerOrderDetails.paymenttran.ToList())
                        {
                            var payment = new Payment_Master();
                            payment.UIN = CustomerOrderDetails.RunningNo;
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
                            payment.InVoiceNumber = CustomerOrderDetails.RunningNo;
                            payment.ReceiptDate = DateTime.UtcNow;
                            payment.ReceiptNumber = CustomerOrderDetails.ReceiptRunningNo;
                            var date = DateTime.Now;
                            payment.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= date && Convert.ToDateTime(b.FYTo) >= date && b.CMPID == CustomerOrderDetails.Cmpid && b.IsActive == true).Select(f => f.ID).FirstOrDefault()).Select(c => c.FYAccYear).FirstOrDefault());
                            payment.TransactionID = CustomerOrderDetails.Tc;
                            payment.CmpID = CustomerOrderDetails.Cmpid;
                            payment.IsBilled = false;
                            payment.CreatedUTC = DateTime.UtcNow;
                            payment.CreatedBy = CustomerOrderDetails.CreatedBy;
                            WYNKContext.PaymentMaster.Add(payment);

                            ErrorLog oErrorLogstran = new ErrorLog();
                            object namestr = payment;
                            oErrorLogstran.WriteErrorLogArray("PaymentMaster", namestr);
                        }


                        var OpticalSummarys = WYNKContext.OpticalSummary.Where(x => x.CmpID == CustomerOrderDetails.Cmpid && x.Date.Date == DateTime.UtcNow.Date).FirstOrDefault();
                        if (OpticalSummarys == null)
                        {
                            var OpticalSummary = new OpticalSummary();
                            OpticalSummary.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                            OpticalSummary.CmpID = CustomerOrderDetails.Cmpid;
                            OpticalSummary.Date = DateTime.UtcNow.Date;
                            OpticalSummary.CollectedAmount = CustomerOrderDetails.paymenttran.Sum(x=>x.Amount);
                            OpticalSummary.Createdby = CustomerOrderDetails.CreatedBy;
                            WYNKContext.OpticalSummary.AddRange(OpticalSummary);
                            WYNKContext.SaveChanges();
                        }
                        else {
                            OpticalSummarys.CollectedAmount = OpticalSummarys.CollectedAmount + CustomerOrderDetails.paymenttran.Sum(x => x.Amount);
                            OpticalSummarys.Updatedby = CustomerOrderDetails.CreatedBy;
                            WYNKContext.OpticalSummary.UpdateRange(OpticalSummarys);
                            WYNKContext.SaveChanges();
                        }

                    }

                    var OPticalPres = WYNKContext.OpticalPrescription.Where(x => x.UIN == CustomerOrderDetails.UIN && x.RegistrationTranID == CustomerOrderDetails.RegTranId && x.CMPID == CustomerOrderDetails.Cmpid).ToList();
                    OPticalPres.All(x => { x.COID = CustomerOrder.ID; return true; });
                    WYNKContext.OpticalPrescription.UpdateRange(OPticalPres);


                    WYNKContext.SaveChanges();

                    var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                    var RunningNumber = commonRepos.GenerateRunningCtrlNoo(CustomerOrderDetails.Tc, CustomerOrderDetails.Cmpid, "GetRunningNo");
                    if (RunningNumber == CustomerOrderDetails.RunningNo)
                    {
                        commonRepos.GenerateRunningCtrlNoo(CustomerOrderDetails.Tc, CustomerOrderDetails.Cmpid, "UpdateRunningNo");
                    }
                    else
                    {
                        var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(CustomerOrderDetails.Tc, CustomerOrderDetails.Cmpid, "UpdateRunningNo");

                        var CusOrder = WYNKContext.CustomerOrder.Where(x => x.OrderNo == CustomerOrderDetails.RunningNo).FirstOrDefault();
                        CusOrder.OrderNo = GetRunningNumber;
                        WYNKContext.CustomerOrder.UpdateRange(CusOrder);

                        if (CustomerOrderDetails.paymenttran.Count > 0)
                        {
                            var PayRef = WYNKContext.PaymentMaster.Where(x => x.UIN == CustomerOrderDetails.RunningNo).ToList();
                            PayRef.All(x => { x.UIN = GetRunningNumber; return true; });
                            WYNKContext.PaymentMaster.UpdateRange(PayRef);
                        }
                        WYNKContext.SaveChanges();
                    }

                    if (CustomerOrderDetails.paymenttran.Count > 0)
                    {
                        var RecContraID = commonRepos.GettingReceiptTcID(CustomerOrderDetails.Tc, CustomerOrderDetails.Cmpid);
                        var ReceiptRunningNumber = commonRepos.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), CustomerOrderDetails.Cmpid, "GetRunningNo");

                        if (ReceiptRunningNumber == CustomerOrderDetails.ReceiptRunningNo)
                        {
                            commonRepos.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), CustomerOrderDetails.Cmpid, "UpdateRunningNo");
                        }
                        else
                        {
                            var payments = WYNKContext.PaymentMaster.Where(x => x.ReceiptNumber == CustomerOrderDetails.ReceiptRunningNo && x.TransactionID == CustomerOrderDetails.Tc).ToList();
                            payments.All(x => { x.ReceiptNumber = ReceiptRunningNumber; return true; });
                            WYNKContext.PaymentMaster.UpdateRange(payments);
                        }
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
                        Message = "Saved successfully",
                        CompanyDetails = CMP,
                        OrderNo = CustomerOrderDetails.RunningNo,
                        OrderDate = CustomerOrder.OrderDate,
                        ReceiptNumber = CustomerOrderDetails.ReceiptRunningNo,
                        ReceiptDate = CustomerOrderDetails.ReceiptRunningNo == null ? (DateTime?)null : DateTime.UtcNow,
                    };

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                  
                    if (ex.InnerException != null)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Error", ex.InnerException.Message.ToString());
                        string msg = ex.InnerException.Message;
                        return new { Success = false, Message = msg, grn = CustomerOrderDetails.RunningNo };
                    }
                    else
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Error", ex.Message.ToString());
                        return new { Success = false };
                    }



                }

            }

        }

        public dynamic GetCustomerOrderedList(int CMPID, int TC)
        {
            try
            {
        
                var custmaster = WYNKContext.CustomerMaster.Where(x => x.CmpID == CMPID).AsNoTracking().ToList();

                var CustomerOrderList = (from Co in WYNKContext.CustomerOrder.Where(x => x.CmpID == CMPID && x.IsCancelled == false && x.TCID == TC && x.OrderStatus != "Closed")
                                         join CM in custmaster on Co.CusID equals CM.ID
                                         select new
                                         {
                                             OrderNo = Co.OrderNo,
                                             OrderDate = Co.OrderDate,
                                             CustomerName = String.Concat(CM.Name, " ", CM.MiddleName != null ? CM.MiddleName : "", " ", CM.LastName != null ? CM.LastName : ""),
                                             OrderStatus = Co.OrderStatus,
                                         }).AsNoTracking().ToList().OrderByDescending(x => x.OrderDate);

                return new
                {
                    Success = true,
                    Message = "Success",
                    CustomerOrderList = CustomerOrderList,
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                    Message = ex.Message,
                };
            }

        }

        public dynamic GetOrderNoDetails(int CMPID, string OrderNo)
        {
            try
            {
                var CustomerOrderedList = new CustomerOrderedList();
                var CustomerOrder = WYNKContext.CustomerOrder.Where(x => x.OrderNo == OrderNo).FirstOrDefault();
                var OnelineMaster = CMPSContext.OneLineMaster.ToList();

                CustomerOrderedList.OrderNo = CustomerOrder.OrderNo;
                CustomerOrderedList.OrderDate = CustomerOrder.OrderDate;
                CustomerOrderedList.RefNo = CustomerOrder.RefNo;
                CustomerOrderedList.RefDate = CustomerOrder.RefDate;
                CustomerOrderedList.Deliverydate = CustomerOrder.Deliverydate;
                CustomerOrderedList.Remarks = CustomerOrder.Remarks;

                var CustomerName = WYNKContext.CustomerMaster.Where(x => x.ID == CustomerOrder.CusID).FirstOrDefault();
                CustomerOrderedList.CustomerName = String.Concat(CustomerName.Name, " ", CustomerName.MiddleName != null ? CustomerName.MiddleName : "", " ", CustomerName.LastName != null ? CustomerName.LastName : "");
                CustomerOrderedList.CustomerAddress1 = WYNKContext.CustomerMaster.Where(x => x.ID == CustomerOrder.CusID).Select(x => x.Address1).FirstOrDefault();
                CustomerOrderedList.CustomerAddress2 = WYNKContext.CustomerMaster.Where(x => x.ID == CustomerOrder.CusID).Select(x => x.Address2).FirstOrDefault();
                CustomerOrderedList.CustomerAddress3 = WYNKContext.CustomerMaster.Where(x => x.ID == CustomerOrder.CusID).Select(x => x.Address3).FirstOrDefault();
                CustomerOrderedList.CustomerMobileNo = WYNKContext.CustomerMaster.Where(x => x.ID == CustomerOrder.CusID).Select(x => x.MobileNo).FirstOrDefault();

                CustomerOrderedList.paymenttran = (from payment in WYNKContext.PaymentMaster.Where(x => x.UIN == CustomerOrder.OrderNo)
                                                   select new Payment_Master
                                                   {
                                                       PaymentMode = OnelineMaster.Where(x => x.OLMID == payment.OLMID).Select(x => x.ParentDescription).FirstOrDefault(),
                                                       InstrumentNumber = payment.InstrumentNumber,
                                                       Instrumentdate = payment.Instrumentdate,
                                                       BankName = payment.BankName,
                                                       BankBranch = payment.BankBranch,
                                                       Expirydate = payment.Expirydate,
                                                       Amount = payment.Amount,
                                                   }).AsNoTracking().ToList();

                CustomerOrderedList.ReceiptNumber = WYNKContext.PaymentMaster.Where(x => x.UIN == CustomerOrder.OrderNo).Select(x => x.ReceiptNumber).FirstOrDefault();
                CustomerOrderedList.M_ReceiptNoDate = WYNKContext.PaymentMaster.Where(x => x.UIN == CustomerOrder.OrderNo).Select(x => x.ReceiptDate).FirstOrDefault();


                var UOM = CMPSContext.uommaster.ToList();
                var custordertran = WYNKContext.CustomerOrderTran.Where(x => x.COID == CustomerOrder.RandomUniqueID).ToList();
                var lensmaster = WYNKContext.Lensmaster.ToList();
                var lenstran = WYNKContext.Lenstrans.ToList();
                var brand = WYNKContext.Brand.ToList();

                CustomerOrderedList.CustomerItemOrders = (from LM in lensmaster
                                                          join LT in lenstran on LM.RandomUniqueID equals LT.LMID
                                                          join BM in brand on LT.Brand equals BM.ID
                                                          join res in custordertran on LT.ID equals res.LTID
                                                          select new CustomerItemOrder
                                                          {
                                                              Type = LM.LensType,
                                                              Brand = BM.Description,
                                                              Model = LT.Model,

                                                              //LensOptions = LT.LensOption,

                                                              Index = LT.Index,
                                                              Color = LT.Colour,
                                                              HSNNo = LT.HSNNo,
                                                              UOM = UOM.Where(x => x.id == res.UOMID).Select(x => x.Description).FirstOrDefault(),
                                                              Quantity = res.OrderedQty,
                                                              UnitPrice = Convert.ToInt32(res.ItemRate),
                                                              Discount = Convert.ToDecimal(res.DiscountPercentage),
                                                              DiscountAmount = Convert.ToDecimal(res.DiscountAmount),
                                                              GrossAmount = Convert.ToDecimal(res.GrossValue),
                                                              CGST = Convert.ToDecimal(res.CGSTPercentage),
                                                              SGST = Convert.ToDecimal(res.SGSTPercentage),
                                                              Amount = Convert.ToDecimal(res.ItemValue),
                                                              GST = res.GSTPercentage,
                                                              CESS = res.CESSPercentage,
                                                              AddCess = res.AdditionalCESSPercentage,
                                                              GSTValue = res.GSTTaxValue,
                                                              CESSValue = res.CESSAmount,
                                                              AddCessValue = res.AddCESSPerAmt,
                                                              GSTDesc = TaxDecsription(res.LTID, "GSTDesc"),
                                                              CESSDesc = TaxDecsription(res.LTID, "CESSDesc"),
                                                              AddCessDesc = TaxDecsription(res.LTID, "AddCessDesc"),
                                                              Sph = LT.Sph,
                                                              Cyl = LT.Cyl,
                                                              Axis = LT.Axis,
                                                              Add = LT.Add,
                                                              Description = LT.Description,
                                                          }).ToList();



                CustomerOrderedList.OpticalPrescription = new List<string>();

                var IsOpticalPrescriptionFound = WYNKContext.CustomerOrder.Where(x => x.ID == CustomerOrder.ID).Select(x => x.OpticalPrescriptionPath).FirstOrDefault();
                if (IsOpticalPrescriptionFound != null)
                {
                    var osfn = IsOpticalPrescriptionFound;
                    var osfi = "/CustomerOrderPrescription/";
                    var currentDir = Directory.GetCurrentDirectory();
                    string path = currentDir + osfi + osfn;
                    if ((File.Exists(path)))
                    {
                        string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                        CustomerOrderedList.OpticalPrescription.Add("data:image/png;base64," + imageData);
                    }
                }

                return new
                {
                    Success = true,
                    Message = "Success",
                    CustomerOrderedList = CustomerOrderedList,
                    CompanyDetails = CMPSContext.Company.Where(x => x.CmpID == CMPID).FirstOrDefault(),
                };

            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                };
            }
        }

        public string TaxDecsription(int LTID, string Desc)
        {
            var TaxId = WYNKContext.Lenstrans.Where(lt => lt.ID == LTID).Select(lt => lt.TaxID).FirstOrDefault();
            var TaxDesc = " ";
            if (Desc == "GSTDesc")
            {
                TaxDesc = CMPSContext.TaxMaster.Where(tax => tax.ID == TaxId).Select(x => x.TaxDescription).FirstOrDefault();
            }
            else if (Desc == "CESSDesc")
            {
                TaxDesc = CMPSContext.TaxMaster.Where(tax => tax.ID == TaxId).Select(x => x.CESSDescription).FirstOrDefault();

            }
            else
            {
                TaxDesc = CMPSContext.TaxMaster.Where(tax => tax.ID == TaxId).Select(x => x.AdditionalCESSDescription).FirstOrDefault();
            }
            return TaxDesc;
        }

        public DateTime Changetime(DateTime date)
        {
            var UTCdate = DateTime.UtcNow;
            var NewDate = new DateTime(date.Year, date.Month, date.Day, UTCdate.Hour, UTCdate.Minute, UTCdate.Second);
            return NewDate;
        }

        public dynamic SubmitCustomerOrderCancel(CustomerOrderViewModel CustomerOrderCancelDetails, DateTime CancelDate)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var CMP = CMPSContext.Company.Where(x => x.CmpID == CustomerOrderCancelDetails.Cmpid).FirstOrDefault();

                    var CancelOrder = WYNKContext.CustomerOrder.Where(x => x.OrderNo == CustomerOrderCancelDetails.OrderNo).FirstOrDefault();
                    CancelOrder.OrderStatus = "Cancelled";
                    CancelOrder.IsCancelled = true;
                    var CancelledDate = Convert.ToDateTime(CustomerOrderCancelDetails.OrderDate);
                    CancelOrder.CancelledDate = Changetime(CancelledDate);
                    CancelOrder.CancelledReasons = CustomerOrderCancelDetails.CancelledReasons;
                    CancelOrder.UpdatedBy = CustomerOrderCancelDetails.CreatedBy;
                    CancelOrder.UpdatedUTC = DateTime.UtcNow;
                    WYNKContext.CustomerOrder.UpdateRange(CancelOrder);

                    string cmpname = CMP.CompanyName;
                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == CustomerOrderCancelDetails.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    string userid = Convert.ToString(CustomerOrderCancelDetails.CreatedBy);
                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(cmpname, "CUSTOMER ORDER CANCEL", "User name :", username, "User ID :", userid, "Mode : Sumbit");
                    object names = CancelOrder;
                    oErrorLogs.WriteErrorLogArray("CustomerOrder", names);


                    var CustomerOrder = new CustomerOrder();
                    CustomerOrder.ContraOrderID = CancelOrder.ID;
                    CustomerOrder.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                    CustomerOrder.CmpID = CustomerOrderCancelDetails.Cmpid;
                    CustomerOrder.CusID = CancelOrder.CusID;
                    CustomerOrder.TCID = CustomerOrderCancelDetails.Tc;
                    CustomerOrder.OrderNo = CustomerOrderCancelDetails.RunningNo;
                    CustomerOrder.OrderDate = Changetime(CancelledDate);
                    CustomerOrder.IsCancelled = true;
                    CustomerOrder.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= DateTime.Now && Convert.ToDateTime(b.FYTo) >= DateTime.Now && x.CMPID == CustomerOrderCancelDetails.Cmpid && x.IsActive == true).Select(f => f.ID).FirstOrDefault()).Select(c => c.FYAccYear).FirstOrDefault());
                    CustomerOrder.OrderStatus = "Cancelled";
                    CustomerOrder.CreatedUTC = DateTime.UtcNow;
                    CustomerOrder.CreatedBy = CustomerOrderCancelDetails.CreatedBy;
                    WYNKContext.CustomerOrder.Add(CustomerOrder);

                    object names1 = CustomerOrder;
                    oErrorLogs.WriteErrorLogArray("CustomerOrder", names1);

                    WYNKContext.SaveChanges();

                    var CancelOrderTran = WYNKContext.CustomerOrderTran.Where(x => x.COID == CancelOrder.RandomUniqueID).ToList();
                    CancelOrderTran.All(x => { x.IsCancelled = true; return true; });
                    WYNKContext.CustomerOrderTran.UpdateRange(CancelOrderTran);

                    object names2 = CancelOrderTran;
                    oErrorLogs.WriteErrorLogArray("CustomerOrderTran", names2);

                    if (CustomerOrderCancelDetails.paymenttran.Count > 0)
                    {
                        var PayRef = WYNKContext.PaymentMaster.Where(x => x.UIN == CustomerOrderCancelDetails.OrderNo).ToList();
                        PayRef.All(x => { x.IsCancelled = true; return true; });
                        WYNKContext.PaymentMaster.UpdateRange(PayRef);
                    }

                    if (CustomerOrderCancelDetails.paymenttran.Count > 0)
                    {
                        foreach (var item1 in CustomerOrderCancelDetails.paymenttran.ToList())
                        {
                            var payment = new Payment_Master();
                            payment.UIN = CustomerOrderCancelDetails.RunningNo;
                            payment.PaymentType = "R";
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
                            payment.InVoiceNumber = CustomerOrderCancelDetails.RunningNo;
                            payment.ReceiptDate = DateTime.UtcNow;
                            payment.ReceiptNumber = CustomerOrderCancelDetails.ReceiptRunningNo;
                            payment.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= DateTime.Now && Convert.ToDateTime(b.FYTo) >= DateTime.Now && x.CMPID == CustomerOrderCancelDetails.Cmpid && x.IsActive == true).Select(f => f.ID).FirstOrDefault()).Select(c => c.FYAccYear).FirstOrDefault());
                            payment.TransactionID = CustomerOrderCancelDetails.Tc;
                            payment.CmpID = CustomerOrderCancelDetails.Cmpid;
                            payment.IsBilled = true;
                            payment.CreatedUTC = DateTime.UtcNow;
                            payment.CreatedBy = CustomerOrderCancelDetails.CreatedBy;
                            WYNKContext.PaymentMaster.Add(payment);

                            object names4 = payment;
                            oErrorLogs.WriteErrorLogArray("PaymentMaster", names4);

                        }
                    }

                    WYNKContext.SaveChanges();

                    var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                    var RunningNumber = commonRepos.GenerateRunningCtrlNoo(CustomerOrderCancelDetails.Tc, CustomerOrderCancelDetails.Cmpid, "GetRunningNo");
                    if (RunningNumber == CustomerOrderCancelDetails.RunningNo)
                    {
                        commonRepos.GenerateRunningCtrlNoo(CustomerOrderCancelDetails.Tc, CustomerOrderCancelDetails.Cmpid, "UpdateRunningNo");
                    }
                    else
                    {
                        var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(CustomerOrderCancelDetails.Tc, CustomerOrderCancelDetails.Cmpid, "UpdateRunningNo");

                        var CusOrder = WYNKContext.CustomerOrder.Where(x => x.OrderNo == CustomerOrderCancelDetails.RunningNo).FirstOrDefault();
                        CusOrder.OrderNo = GetRunningNumber;
                        WYNKContext.CustomerOrder.UpdateRange(CusOrder);

                        object CusOrderq = CusOrder;
                        oErrorLogs.WriteErrorLogArray("PaymentMaster", CusOrderq);

                        if (CustomerOrderCancelDetails.paymenttran.Count > 0)
                        {
                            var PayRefs = WYNKContext.PaymentMaster.Where(x => x.UIN == CustomerOrderCancelDetails.RunningNo).ToList();
                            PayRefs.All(x => { x.UIN = GetRunningNumber; return true; });
                            WYNKContext.PaymentMaster.UpdateRange(PayRefs);
                            object names4 = PayRefs;
                            oErrorLogs.WriteErrorLogArray("PaymentMaster", names4);
                        }
                    }

                    if (CustomerOrderCancelDetails.paymenttran.Count > 0)
                    {
                        var RecContraID = commonRepos.GettingReceiptTcID(CustomerOrderCancelDetails.Tc, CustomerOrderCancelDetails.Cmpid);
                        var ReceiptRunningNumber = commonRepos.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), CustomerOrderCancelDetails.Cmpid, "GetRunningNo");
                        if (ReceiptRunningNumber == CustomerOrderCancelDetails.ReceiptRunningNo)
                        {
                            commonRepos.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), CustomerOrderCancelDetails.Cmpid, "UpdateRunningNo");
                        }
                        else
                        {
                            var payments = WYNKContext.PaymentMaster.Where(x => x.ReceiptNumber == CustomerOrderCancelDetails.ReceiptRunningNo && x.TransactionID == CustomerOrderCancelDetails.Tc).ToList();
                            payments.All(x => { x.ReceiptNumber = ReceiptRunningNumber; return true; });
                            WYNKContext.PaymentMaster.UpdateRange(payments);
                        }
                    }

                    if (WYNKContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }

                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();
                    return new
                    {
                        Success = true,
                        Message = "Saved successfully",
                        CompanyDetails = CMP,
                        CancelOrderNo = CustomerOrderCancelDetails.RunningNo,
                        CancelOrderDate = CancelOrder.CancelledDate,
                        ReceiptNumber = CustomerOrderCancelDetails.ReceiptRunningNo,
                        ReceiptDate = DateTime.UtcNow,
                    };
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    if (ex.InnerException != null)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Error", ex.InnerException.Message.ToString());
                        string msg = ex.InnerException.Message;
                        return new { Success = false, Message = msg, grn = CustomerOrderCancelDetails.RunningNo };
                    }
                    else
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Error", ex.Message.ToString());
                        string msg = ex.Message;
                        return new { Success = false, Message = msg, grn = CustomerOrderCancelDetails.RunningNo };
                    }
                }
            }
        }
        public dynamic GetCustomerCancelOrderedList(int CMPID, int TC)
        {
            try
            {
                var CustomerCancelOrderList = (from Co in WYNKContext.CustomerOrder.Where(x => x.CmpID == CMPID && x.IsCancelled == true && x.TCID == TC)
                                               join CM in WYNKContext.CustomerMaster.Where(x => x.CmpID == CMPID) on Co.CusID equals CM.ID
                                               select new
                                               {
                                                   OrderNo = Co.OrderNo,
                                                   OrderDate = Co.OrderDate,
                                                   CustomerName = CM.Name,
                                                   OrderStatus = Co.OrderStatus,
                                               }).AsNoTracking().ToList();

                return new
                {
                    Success = true,
                    Message = "Success",
                    CustomerCancelOrderList = CustomerCancelOrderList,
                };
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error", ex.InnerException.Message.ToString());
                    string msg = ex.InnerException.Message;
                    return new { Success = false };
                }
                else
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error", ex.Message.ToString());
                    string msg = ex.Message;
                    return new { Success = false };
                }
            }
        }

        public dynamic GetCancelOrderNoDetails(int CMPID, string CancelOrderNo)
        {
            try
            {

                var GetOrderNo = WYNKContext.CustomerOrder.Where(x => x.OrderNo == CancelOrderNo).FirstOrDefault();
                var OrderNo = WYNKContext.CustomerOrder.Where(x => x.ID == GetOrderNo.ContraOrderID).Select(x => x.OrderNo).FirstOrDefault();

                var CustomerOrderedList = new CustomerOrderedList();
                var OnelineMaster = CMPSContext.OneLineMaster.ToList();

                var CustomerOrder = WYNKContext.CustomerOrder.Where(x => x.OrderNo == OrderNo).FirstOrDefault();

                CustomerOrderedList.OrderNo = CustomerOrder.OrderNo;
                CustomerOrderedList.OrderDate = CustomerOrder.OrderDate;
                CustomerOrderedList.RefNo = CustomerOrder.RefNo;
                CustomerOrderedList.RefDate = CustomerOrder.RefDate;

                CustomerOrderedList.CustomerName = WYNKContext.CustomerMaster.Where(x => x.ID == CustomerOrder.CusID).Select(x => x.Name).FirstOrDefault();
                CustomerOrderedList.CustomerAddress1 = WYNKContext.CustomerMaster.Where(x => x.ID == CustomerOrder.CusID).Select(x => x.Address1).FirstOrDefault();
                CustomerOrderedList.CustomerAddress2 = WYNKContext.CustomerMaster.Where(x => x.ID == CustomerOrder.CusID).Select(x => x.Address2).FirstOrDefault();
                CustomerOrderedList.CustomerAddress3 = WYNKContext.CustomerMaster.Where(x => x.ID == CustomerOrder.CusID).Select(x => x.Address3).FirstOrDefault();
                CustomerOrderedList.CustomerMobileNo = WYNKContext.CustomerMaster.Where(x => x.ID == CustomerOrder.CusID).Select(x => x.MobileNo).FirstOrDefault();

                CustomerOrderedList.paymenttran = (from payment in WYNKContext.PaymentMaster.Where(x => x.UIN == CustomerOrder.OrderNo)
                                                   select new Payment_Master
                                                   {
                                                       PaymentMode = OnelineMaster.Where(x => x.OLMID == payment.OLMID).Select(x => x.ParentDescription).FirstOrDefault(),
                                                       InstrumentNumber = payment.InstrumentNumber,
                                                       Instrumentdate = payment.Instrumentdate,
                                                       BankName = payment.BankName,
                                                       BankBranch = payment.BankBranch,
                                                       Expirydate = payment.Expirydate,
                                                       Amount = payment.Amount,
                                                   }).ToList();


                CustomerOrderedList.ReceiptNumber = WYNKContext.PaymentMaster.Where(x => x.UIN == CancelOrderNo).Select(x => x.ReceiptNumber).FirstOrDefault();
                CustomerOrderedList.M_ReceiptNoDate = WYNKContext.PaymentMaster.Where(x => x.UIN == CancelOrderNo).Select(x => x.ReceiptDate).FirstOrDefault();


                CustomerOrderedList.RefundDetails = (from payment in WYNKContext.PaymentMaster.Where(x => x.UIN == GetOrderNo.OrderNo)
                                                     select new Payment_Master
                                                     {
                                                         PaymentMode = OnelineMaster.Where(x => x.OLMID == payment.OLMID).Select(x => x.ParentDescription).FirstOrDefault(),
                                                         InstrumentNumber = payment.InstrumentNumber,
                                                         Instrumentdate = payment.Instrumentdate,
                                                         BankName = payment.BankName,
                                                         BankBranch = payment.BankBranch,
                                                         Expirydate = payment.Expirydate,
                                                         Amount = payment.Amount,
                                                     }).ToList();


                var UOM = CMPSContext.uommaster.ToList();

                var CustomerOrderTran = WYNKContext.CustomerOrderTran.ToList();

                CustomerOrderedList.CustomerItemOrders = (from LM in WYNKContext.Lensmaster
                                                          join LT in WYNKContext.Lenstrans on LM.RandomUniqueID equals LT.LMID
                                                          join BM in WYNKContext.Brand on LT.Brand equals BM.ID
                                                          join res in CustomerOrderTran.Where(x => x.COID == CustomerOrder.RandomUniqueID) on LT.ID equals res.LTID
                                                          select new CustomerItemOrder
                                                          {
                                                              Type = LM.LensType,
                                                              Brand = BM.Description,
                                                              Model = LT.Model,


                                                              Index = LT.Index,
                                                              Color = LT.Colour,
                                                              HSNNo = LT.HSNNo,
                                                              UOM = UOM.Where(x => x.id == res.UOMID).Select(x => x.Description).FirstOrDefault(),
                                                              Quantity = res.OrderedQty,
                                                              UnitPrice = Convert.ToInt32(res.ItemRate),
                                                              Discount = Convert.ToInt32(res.DiscountPercentage),
                                                              DiscountAmount = Convert.ToInt32(res.DiscountAmount),
                                                              GrossAmount = Convert.ToInt32(res.GrossValue),
                                                              CGST = Convert.ToDecimal(res.CGSTPercentage),
                                                              SGST = Convert.ToDecimal(res.SGSTPercentage),
                                                              Amount = Convert.ToInt32(res.ItemValue),
                                                              Sph = LT.Sph,
                                                              Cyl = LT.Cyl,
                                                              Axis = LT.Axis,
                                                              Add = LT.Add,
                                                              Description = LT.Description,
                                                          }).ToList();


                return new
                {
                    Success = true,
                    Message = "Success",
                    CustomerOrderedList = CustomerOrderedList,
                    CompanyDetails = CMPSContext.Company.Where(x => x.CmpID == CMPID).FirstOrDefault(),
                    CancelOrderNo = CancelOrderNo,
                    CancelReason = CustomerOrder.CancelledReasons,
                    CancelDate = GetOrderNo.OrderDate
                };


            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error", ex.InnerException.Message.ToString());
                    string msg = ex.InnerException.Message;
                    return new { Success = false };
                }
                else
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error", ex.Message.ToString());
                    string msg = ex.Message;
                    return new { Success = false };
                }
            }
         
        }

        public dynamic GetfinalopDetails(int CMPID)
        {
            var registration = WYNKContext.Registration.ToList();
            var user = CMPSContext.Users.ToList();
            var docmas = CMPSContext.DoctorMaster.ToList();
            var userrole = CMPSContext.UserVsRole.ToList();
            var final = new OpticalPrescriptionview();
            final.opfinalprescri = new List<opfinalprescri>();

            var groups = (from REF in WYNKContext.OpticalPrescription.Where(u => u.CMPID == CMPID && u.COID == null)
                          join us in user on REF.CreatedBy equals us.Userid


                          join doc in docmas on
                          us.Username equals doc.EmailID
                          join usr in userrole on
                          us.Username equals usr.UserName


                          select new
                          {
                              RID = REF.RegistrationTranID,
                              Pdate = REF.CreatedUTC,
                              DName = doc.Firstname + " " + doc.MiddleName + " " + doc.LastName,
                              Role = usr.RoleDescription,
                              uin = REF.UIN,
                              name = registration.Where(x => x.UIN == REF.UIN).Select(x => x.Name + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                              FirstName = registration.Where(x => x.UIN == REF.UIN).Select(x => x.Name).FirstOrDefault(),
                              Middlename = registration.Where(x => x.UIN == REF.UIN).Select(x => x.MiddleName).FirstOrDefault(),
                              Lastname = registration.Where(x => x.UIN == REF.UIN).Select(x => x.LastName).FirstOrDefault(),
                              Gender = registration.Where(x => x.UIN == REF.UIN).Select(x => x.Gender).FirstOrDefault(),
                          }).AsNoTracking().ToList().OrderByDescending(x => x.Pdate);



            var Final = (from REF in groups.GroupBy(x => x.RID)
                         select new
                         {

                             UIN = REF.Select(x => x.uin).FirstOrDefault(),
                             Name = REF.Select(x => x.name).FirstOrDefault(),
                             FirstName = REF.Select(x => x.FirstName).FirstOrDefault(),
                             Middlename = REF.Select(x => x.Middlename).FirstOrDefault(),
                             Lastname = REF.Select(x => x.Lastname).FirstOrDefault(),
                             Doctorname = REF.Select(x => x.DName).FirstOrDefault(),
                             PrescriptionDate = REF.Select(x => x.Pdate).FirstOrDefault(),
                             RegistrationTranID = REF.Select(x => x.RID).FirstOrDefault(),
                             Address1 = registration.Where(z => z.UIN == REF.Select(y => y.uin).FirstOrDefault()).Select(x => x.Address1).FirstOrDefault(),
                             Address2 = registration.Where(z => z.UIN == REF.Select(y => y.uin).FirstOrDefault()).Select(x => x.Address2).FirstOrDefault(),
                             Address3 = registration.Where(z => z.UIN == REF.Select(y => y.uin).FirstOrDefault()).Select(x => x.Address3).FirstOrDefault(),
                             MobileNo = registration.Where(z => z.UIN == REF.Select(y => y.uin).FirstOrDefault()).Select(x => x.Phone).FirstOrDefault(),
                         }).ToList();



            return new
            {
                Success = true,
                Message = "Success",
                data = Final
            };
        }

        public dynamic IsCustomerFound(int CMPID, string UIN)
        {
            var IsFound = WYNKContext.CustomerMaster.Where(x => x.CmpID == CMPID && x.UIN == UIN && x.IsDeleted == false).FirstOrDefault();
            if (IsFound != null)
            {
                return new
                {
                    Success = true,
                    Message = "Found",
                    CusID = IsFound.ID,
                    Name = IsFound.Name + " " + IsFound.MiddleName + " " + IsFound.LastName,
                    Address = IsFound.Address1 + " " + IsFound.Address2 + " " + IsFound.Address3,
                    MobileNo = IsFound.MobileNo,
                };

            }
            else
            {

                return new
                {
                    Success = false,
                    Message = "Not Found",
                };
            }
        }

        public dynamic CustomerDetailsSubmit(CustomerSubmit CustomerSubmitDetails, int CMPID, int UserId)
        {
            try
            {
                var CusMaster = new CustomerMaster();
                CusMaster.UIN = CustomerSubmitDetails.CustomerDatas.UIN;
                CusMaster.Name = CustomerSubmitDetails.CustomerDatas.FirstName;
                CusMaster.MiddleName = CustomerSubmitDetails.CustomerDatas.Middlename;
                CusMaster.LastName = CustomerSubmitDetails.CustomerDatas.Lastname;
                CusMaster.Address1 = CustomerSubmitDetails.CustomerDatas.Address1;
                CusMaster.Address2 = CustomerSubmitDetails.CustomerDatas.Address2;
                CusMaster.Address3 = CustomerSubmitDetails.CustomerDatas.Address3;
                CusMaster.MobileNo = CustomerSubmitDetails.CustomerDatas.MobileNo;
                CusMaster.IsDeleted = false;
                CusMaster.CreatedUTC = DateTime.UtcNow;
                CusMaster.CreatedBy = UserId;
                CusMaster.CmpID = CMPID;

                WYNKContext.CustomerMaster.Add(CusMaster);
                WYNKContext.SaveChanges();

                var IsFound = WYNKContext.CustomerMaster.Where(x => x.CmpID == CMPID && x.UIN == CustomerSubmitDetails.CustomerDatas.UIN && x.IsDeleted == false).FirstOrDefault();

                return new
                {
                    Success = true,
                    Message = "Save Successfully",
                    CusID = IsFound.ID,
                    Name = IsFound.Name + " " + IsFound.MiddleName + " " + IsFound.LastName,
                    Address = IsFound.Address1 + " " + IsFound.Address2 + " " + IsFound.Address3,
                    MobileNo = IsFound.MobileNo,
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

        public dynamic UploadImage(IFormFile file, string CustomerOrderNo)
        {
            try
            {
                var currentDir = Directory.GetCurrentDirectory();
                if (!Directory.Exists(currentDir + "/CustomerOrderPrescription/"))
                    Directory.CreateDirectory(currentDir + "/CustomerOrderPrescription/");
                var fileName = $"{CustomerOrderNo}{Path.GetExtension(file.FileName)}";
                var path = $"{currentDir}/CustomerOrderPrescription/{fileName}";

                if ((File.Exists(path)))
                    File.Delete(path);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                    var photo = WYNKContext.CustomerOrder.Where(x => x.OrderNo == CustomerOrderNo).FirstOrDefault();
                    photo.OpticalPrescriptionPath = fileName;
                    WYNKContext.Entry(photo).State = EntityState.Modified;
                    WYNKContext.SaveChanges();
                    var tt = WYNKContext.SaveChanges() > 0;
                    return tt;
                }
            }
            catch (Exception)
            {
                return new
                {
                    Success = false,
                };
            }
        }
     
        public dynamic InsertOpticalPrescription(CustomerOrderViewModel AddOpticalPrescription, int CustomerID, int cmpID, int userid)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var OPMcount = WYNKContext.OpticalPrescriptionmaster.Where(x => x.CustomerMasterID == CustomerID && x.CMPID == cmpID).Count();

                    if (OPMcount != 0)
                    {
                        var opm = WYNKContext.OpticalPrescriptionmaster.Where(x => x.CustomerMasterID == CustomerID && x.CMPID == cmpID).ToList();
                        var op = WYNKContext.OpticalPrescription.Where(x => x.CustomerMasterID == CustomerID && x.CMPID == cmpID).ToList();
                        WYNKContext.OpticalPrescriptionmaster.RemoveRange(opm);
                        WYNKContext.OpticalPrescription.RemoveRange(op);
                        WYNKContext.SaveChanges();
                    }

                        if (AddOpticalPrescription.FINALPRESCRIPTION != null)
                        {
                            if (AddOpticalPrescription.FINALPRESCRIPTION.Count() > 0)
                            {

                                var finod = AddOpticalPrescription.FINALPRESCRIPTION.Where(x => x.Description == "Final Prescription").ToList();
                                var finos = AddOpticalPrescription.FINALPRESCRIPTION.Where(x => x.Description == "Final Prescription").ToList();
                                var one = CMPSContext.OneLineMaster.ToList();
                                var masteroptical = WYNKContext.CustomerMaster.Where(x => x.ID == CustomerID && x.CmpID == cmpID && x.IsDeleted == false).FirstOrDefault();

                                var RefOP = new OpticalPrescriptionmaster();
                                RefOP.UIN = masteroptical.UIN;
                                RefOP.CustomerMasterID = CustomerID;
                                RefOP.Name = masteroptical.Name;
                                RefOP.MiddleName = masteroptical.MiddleName;
                                RefOP.LastName = masteroptical.LastName;
                                RefOP.Phone = masteroptical.PhoneNo;
                                RefOP.PrescriptionDate = DateTime.UtcNow;
                                RefOP.CMPID = cmpID;
                                RefOP.CreatedUTC = DateTime.UtcNow;
                                RefOP.CreatedBy = userid;
                                WYNKContext.OpticalPrescriptionmaster.AddRange(RefOP);
                                ErrorLog oErrorLogstran2 = new ErrorLog();
                                object namestr2 = RefOP;
                                oErrorLogstran2.WriteErrorLogArray("OpticalPrescription", namestr2);

                                if (finod.Count() > 0)
                                {
                                    foreach (var item in finod.ToList())
                                    {
                                        var Ref1 = new OpticalPrescriptionn();
                                        var st = item.DVName = "Distance Vision";

                                        if (item.Ocular == "OD" && st == one.Where(z => z.ParentDescription == "Distance Vision").Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSph != "" || item.NearCyl != "" || item.PinAxis != "" || item.Add != "" || item.Remarks != ""))
                                        {
                                          
                                            Ref1.UIN = masteroptical.UIN;
                                            Ref1.CustomerMasterID = CustomerID;
                                            Ref1.PrescriptionDate = DateTime.UtcNow;
                                            Ref1.Ocular = item.Ocular;
                                            Ref1.DistSph = item.DistSph;
                                            Ref1.NearCyl = item.NearCyl;
                                            Ref1.PinAxis = item.PinAxis;
                                            Ref1.Add = item.Add;
                                            Ref1.Remarks = item.Remarks;
                                            Ref1.PD = item.PD;
                                            Ref1.MPDOD = item.MPDOD;
                                            Ref1.MPDOS = item.MPDOS;
                                            Ref1.CMPID = cmpID;
                                            Ref1.Type = item.DV == 0 ? one.Where(z => z.ParentDescription == "Distance Vision" && item.DVName == "Distance Vision").Select(s => s.OLMID).FirstOrDefault() : item.DV;
                                            Ref1.CreatedUTC = DateTime.UtcNow;
                                            Ref1.CreatedBy = userid;
                                            WYNKContext.OpticalPrescription.AddRange(Ref1);
                                            ErrorLog oErrorLogstran1 = new ErrorLog();
                                            object namestr1 = Ref1;
                                            oErrorLogstran1.WriteErrorLogArray("OpticalPrescription", namestr1);

                                        }
                                    }

                                }

                                if (finod.Count() > 0)
                                {
                                    foreach (var item in finod.ToList())
                                    {
                                        var Ref = new OpticalPrescriptionn();
                                        var st = item.DVName = "Near Vision";
                                        if (item.Ocular == "OD" && st == one.Where(z => z.ParentDescription == "Near Vision").Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSphNVOD != "" || item.AddNVOD != ""))
                                        {

                                            Ref.UIN = masteroptical.UIN;
                                            Ref.CustomerMasterID = CustomerID;
                                            Ref.PrescriptionDate = DateTime.UtcNow;
                                            Ref.Ocular = item.Ocular;
                                            Ref.DistSph = item.DistSphNVOD;
                                            Ref.Add = item.AddNVOD;
                                            Ref.PD = item.PD;
                                            Ref.MPDOD = item.MPDOD;
                                            Ref.MPDOS = item.MPDOS;
                                            Ref.CMPID = cmpID;
                                            Ref.Remarks = item.Remarks;
                                            Ref.Type = item.NV == 0 ? one.Where(z => z.ParentDescription == "Near Vision" && item.DVName == "Near Vision").Select(s => s.OLMID).FirstOrDefault() : item.NV;
                                            Ref.CreatedUTC = DateTime.UtcNow;
                                            Ref.CreatedBy = userid;
                                            WYNKContext.OpticalPrescription.AddRange(Ref);
                                            ErrorLog oErrorLogstran = new ErrorLog();
                                            object namestr = Ref;
                                            oErrorLogstran.WriteErrorLogArray("OpticalPrescription", namestr);

                                        }
                                    }

                                }

                                if (finos.Count() > 0)
                                {
                                    foreach (var item in finos.ToList())
                                    {
                                        var Ref = new OpticalPrescriptionn();
                                        var st = item.DVName = "Distance Vision";

                                        if (item.OcularOS == "OS" && st == one.Where(z => z.ParentDescription == "Distance Vision").Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSphOS != "" || item.NearCylOS != "" || item.PinAxisOS != "" || item.AddOS != "" || item.Remarks != ""))
                                        {

                                            Ref.UIN = masteroptical.UIN;
                                            Ref.CustomerMasterID = CustomerID;
                                            Ref.PrescriptionDate = DateTime.UtcNow;
                                            Ref.Ocular = item.OcularOS;
                                            Ref.DistSph = item.DistSphOS;
                                            Ref.NearCyl = item.NearCylOS;
                                            Ref.PinAxis = item.PinAxisOS;
                                            Ref.Add = item.AddOS;
                                            Ref.Remarks = item.Remarks;
                                            Ref.PD = item.PD;
                                            Ref.MPDOD = item.MPDOD;
                                            Ref.MPDOS = item.MPDOS;
                                            Ref.CMPID = cmpID;
                                            Ref.Type = item.DV == 0 ? one.Where(z => z.ParentDescription == "Distance Vision" && item.DVName == "Distance Vision").Select(s => s.OLMID).FirstOrDefault() : item.DV;
                                            Ref.CreatedUTC = DateTime.UtcNow;
                                            Ref.CreatedBy = userid;
                                            WYNKContext.OpticalPrescription.AddRange(Ref);
                                            ErrorLog oErrorLogstran = new ErrorLog();
                                            object namestr = Ref;
                                            oErrorLogstran.WriteErrorLogArray("OpticalPrescription", namestr);

                                        }

                                    }
                                }

                                if (finos.Count() > 0)
                                {
                                    foreach (var item in finos.ToList())
                                    {
                                        var Ref = new OpticalPrescriptionn();
                                        var st = item.DVName = "Near Vision";

                                        if (item.OcularOS == "OS" && st == one.Where(z => z.ParentDescription == "Near Vision").Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSphNVOS != "" || item.AddNVOS != ""))
                                        {

                                            Ref.UIN = masteroptical.UIN;
                                            Ref.CustomerMasterID = CustomerID;
                                            Ref.PrescriptionDate = DateTime.UtcNow;
                                            Ref.Ocular = item.OcularOS;
                                            Ref.DistSph = item.DistSphNVOS;
                                            Ref.Add = item.AddNVOS;
                                            Ref.PD = item.PD;
                                            Ref.MPDOD = item.MPDOD;
                                            Ref.MPDOS = item.MPDOS;
                                            Ref.CMPID = cmpID;
                                            Ref.Remarks = item.Remarks;
                                            Ref.Type = item.NV == 0 ? one.Where(z => z.ParentDescription == "Near Vision" && item.DVName == "Near Vision").Select(s => s.OLMID).FirstOrDefault() : item.NV;
                                            Ref.CreatedUTC = DateTime.UtcNow;
                                            Ref.CreatedBy = userid;
                                            WYNKContext.OpticalPrescription.AddRange(Ref);
                                            ErrorLog oErrorLogstran = new ErrorLog();
                                            object namestr = Ref;
                                            oErrorLogstran.WriteErrorLogArray("OpticalPrescription", namestr);

                                        }

                                    }
                                }


                            }
                        }
                   
                 


                    

                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

                    return new
                    {
                        Success = true,
                    };

                }


                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
                return new
                {
                    Success = false,
                };


            }
        }



        public dynamic GetOpticalPrescription(int CusMasID, int CMPID)
        {
            var GetOpticalPrescription = new CustomerOrderViewModel();

            var OpticalPrescriptionmaster = WYNKContext.OpticalPrescriptionmaster.Where(x=>x.CMPID==CMPID).ToList();
            var OpticalPrescription = WYNKContext.OpticalPrescription.Where(x => x.CMPID == CMPID).ToList();

           
            var one = CMPSContext.OneLineMaster.ToList();

            GetOpticalPrescription.opticprescription1 = (from op in OpticalPrescription.Where(x => x.CustomerMasterID == CusMasID)
                                                    
                                                        select new opticprescription1
                                                        {
                                                            Type = op.Type,
                                                            Ocular = op.Ocular,
                                                            DistSph = op.DistSph,
                                                            NearCyl = op.NearCyl,
                                                            PinAxis = op.PinAxis,
                                                            Add = op.Add ,
                                                            Remarks = op.Remarks,
                                                            PD = op.PD,
                                                            MPDOD = op.MPDOD,
                                                            MPDOS = op.MPDOS,

                                                        }).ToList();

            return GetOpticalPrescription;



        }


    }
}













