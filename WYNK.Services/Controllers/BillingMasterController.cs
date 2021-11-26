using System;
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Common;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class BillingMasterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public BillingMasterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }



        [HttpGet("CurrentDateSearch/{SelectedDate}/{CmpID}/{Status}/{GMT}")]
        public BillingPharmacy CurrentDateSearch(string SelectedDate, int CmpID, string Status,string GMT)
        {

            return _repoWrapper.Billing.CurrentDateSearch(SelectedDate, CmpID, Status, GMT);
        }

        [HttpGet("PeriodSearch/{FromDate}/{Todate}/{CmpID}/{Status}/{GMT}")]
        public BillingPharmacy PeriodSearch(string FromDate, string Todate, int CmpID,string Status, string GMT)
        {
            return _repoWrapper.Billing.PeriodSearch(FromDate,Todate,CmpID, Status, GMT);
        }

        [HttpPost("GetMedicalPrescriptionIddetails/{MedicalPrescriptionId}/{storeID}/{CmpID}")]
        public BillingPharmacy GetMedicalPrescriptionIddetails(string MedicalPrescriptionId,int storeID, int CmpID)
        {
            return _repoWrapper.Billing.GetMedicalPrescriptionIddetails(MedicalPrescriptionId, storeID, CmpID);
        }



        [HttpPost("AddBillingDetails")]
        public dynamic AddBillingDetails([FromBody] BillingPharmacy AddBill)
        {
            //String gr = _repoWrapper.Common.GenerateRunningCtrl(3009);
            //AddBill.MedicalBillMaster.BillNo = gr;
            return _repoWrapper.Billing.AddBillingDetailsAsync(AddBill);
        }


        [HttpPost("AddOutPatientBilling/{GMT}")]
        public dynamic AddOutPatientBilling([FromBody] OutPatientBillingPharmacy AddOPBill, string GMT)
        {
            //if (AddOPBill.Status == "Open")
            //{
                int? RecContraID = _repoWrapper.Common.GettingReceiptTcID(AddOPBill.Tc, AddOPBill.CmpId);

                if(RecContraID == null)
                {
                    return new
                    {
                        Success = false,
                        Message = "Receipt Running Number Does'nt Mapped in Transaction Table"
                    };
                }
                AddOPBill.ReceiptRunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), AddOPBill.CmpId, "GetRunningNo");

                if (AddOPBill.ReceiptRunningNo == "Running Number Does'nt Exist")
                {
                    return new
                    {
                        Success = false,
                        Message = "Receipt Running Number Does'nt Exist"
                    };
                }


                String gr = _repoWrapper.Common.GenerateRunningCtrlNoo(AddOPBill.Tc, AddOPBill.CmpId, "GetRunningNo");
                AddOPBill.MedicalBillMaster.BillNo = gr;

                if (AddOPBill.MedicalBillMaster.BillNo == "Running Number Does'nt Exist")
                {
                    return new
                    {
                        Success = false,
                        Message = "Running Number Does'nt Exist"
                    };
                }
         //   }


            //if (AddOPBill.Status == "Partially Closed")
            //{
            //    int? RecContraID = _repoWrapper.Common.GettingReceiptTcID(AddOPBill.Tc, AddOPBill.CmpId);

            //    if (RecContraID == null)
            //    {
            //        return new
            //        {
            //            Success = false,
            //            Message = "Receipt Running Number Does'nt Exist"
            //        };
            //    }
            //    AddOPBill.ReceiptRunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), AddOPBill.CmpId);
            //}



            return _repoWrapper.Billing.AddOutPatientBilling(AddOPBill, GMT);
        }



        [HttpGet("GetdrugDetails/{ID}/{StoreID}/{CmpID}")]
        public dynamic GetdrugDetails(int ID, int StoreID, int CmpID)
        {
            return _repoWrapper.Billing.GetdrugDetails(ID, StoreID, CmpID);
        }

        [HttpGet("GetClosedDetails/{ID}/{CmpID}/{TC}/{GMT}")]
        public dynamic GetClosedDetails(string ID,int CmpID,int TC, string GMT)
        {
            return _repoWrapper.Billing.GetClosedDetails(ID, CmpID, TC, GMT);
        }

        [HttpPost("GetPartiallyClosedDetails/{MedicalPrescriptionId}/{StoreID}/{CmpID}")]
        public dynamic GetPartiallyClosedDetails(string MedicalPrescriptionId, int StoreID, int CmpID)
        {
            return _repoWrapper.Billing.GetPartiallyClosedDetails(MedicalPrescriptionId, StoreID, CmpID);
        }




        [HttpGet("CheckMedPresQuantity/{Quantity}/{DrugID}/{StoreId}")]
        public dynamic CheckMedPresQuantity(int Quantity, int DrugID, int StoreId)
        {
            return _repoWrapper.Billing.CheckMedPresQuantity(Quantity, DrugID ,StoreId);
        }


        [HttpPost("PendingPayments/{CmpID}/{GMT}")]
        public dynamic PendingPayments([FromBody] PendingPayments PendingPayments, int CmpID, string GMT)
        {
            int? RecContraID = _repoWrapper.Common.GettingReceiptTcID(PendingPayments.PendingPayment.TC, CmpID);
            if (RecContraID == null)
            {
                return new
                {
                    Success = false,
                    Message = "Receipt Running Number Does'nt Mapped in Transaction Table"
                };
            }
            PendingPayments.ReceiptRunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), CmpID, "GetRunningNo");
            if (PendingPayments.ReceiptRunningNo == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Receipt Running Number Does'nt Exist"
                };
            }



            return _repoWrapper.Billing.PendingPayments(PendingPayments, CmpID,GMT);
        }



        [HttpGet("CurrentDateSearchOPR/{SelectedDate}/{CmpID}/{Status}/{ContraTc}/{GMT}")]
        public dynamic CurrentDateSearchOPR(string SelectedDate, int CmpID, string Status,int ContraTc,string GMT)
        {

            return _repoWrapper.Billing.CurrentDateSearchOPR(SelectedDate, CmpID, Status, ContraTc, GMT);
        }

        [HttpGet("CurrentDateSearchOPRBills/{SelectedDate}/{CmpID}/{Status}/{Tc}/{GMT}")]
        public dynamic CurrentDateSearchOPRBills(string SelectedDate, int CmpID, string Status, int Tc, string GMT)
        {

            return _repoWrapper.Billing.CurrentDateSearchOPRBills(SelectedDate, CmpID, Status, Tc, GMT);
        }


        [HttpGet("PeriodSearchOPR/{FromDate}/{Todate}/{CmpID}/{Status}/{ContraTc}/{GMT}")]
        public dynamic PeriodSearchOPR(string FromDate, string Todate, int CmpID, string Status,int ContraTc, string GMT)
        {
            return _repoWrapper.Billing.PeriodSearchOPR(FromDate, Todate, CmpID, Status, ContraTc, GMT);
        }

        [HttpGet("PeriodSearchOPRs/{FromDate}/{Todate}/{CmpID}/{Status}/{Tc}/{GMT}")]
        public dynamic PeriodSearchOPRs(string FromDate, string Todate, int CmpID, string Status, int Tc, string GMT)
        {
            return _repoWrapper.Billing.PeriodSearchOPRs(FromDate, Todate, CmpID, Status, Tc, GMT);
        }


        [HttpGet("PatientNameSearch/{CmpID}/{Status}/{ContraTc}/{GMT}/{Name}")]
        public dynamic PatientNameSearch(int CmpID, string Status, int ContraTc, string GMT, string Name)
        {
            return _repoWrapper.Billing.PatientNameSearch(CmpID, Status, ContraTc, GMT, Name);
        }


        [HttpGet("PatientNameSearchs/{CmpID}/{Status}/{Tc}/{GMT}/{Name}")]
        public dynamic PatientNameSearchs(int CmpID, string Status, int Tc, string GMT, string Name)
        {
            return _repoWrapper.Billing.PatientNameSearchs(CmpID, Status, Tc, GMT, Name);
        }

        [HttpGet("BillNoSearch/{CmpID}/{Status}/{ContraTc}/{GMT}/{BillNo}")]
        public dynamic BillNoSearch(int CmpID, string Status, int ContraTc, string GMT, string BillNo)
        {
            return _repoWrapper.Billing.BillNoSearch(CmpID, Status, ContraTc, GMT, BillNo);
        }

        [HttpGet("BillNoSearchs/{CmpID}/{Status}/{Tc}/{GMT}/{BillNo}")]
        public dynamic BillNoSearchs(int CmpID, string Status, int Tc, string GMT, string BillNo)
        {
            return _repoWrapper.Billing.BillNoSearchs(CmpID, Status, Tc, GMT, BillNo);
        }

        [HttpGet("ReturningBillNoDetails/{BillNo}/{CmpID}/{StoreID}")]
        public dynamic ReturningBillNoDetails(string BillNo, int CmpID, int StoreID)
        {

            return _repoWrapper.Billing.ReturningBillNoDetails(BillNo, CmpID, StoreID);
        }


        [HttpGet("ClosedReturningBillNoDetails/{BillNo}/{CmpID}/{StoreID}/{GMT}")]
        public dynamic ClosedReturningBillNoDetails(string BillNo, int CmpID, int StoreID,string GMT)
        {

            return _repoWrapper.Billing.ClosedReturningBillNoDetails(BillNo, CmpID, StoreID, GMT);
        }

        [HttpPost("AddOutPatientBillingReturns/{GMT}")]
        public dynamic AddOutPatientBillingReturns([FromBody] BillingReturns BillingReturnDetails, string GMT)
        {


            if (BillingReturnDetails.paymenttrans.Count > 0)
            {
                int? RecContraID = _repoWrapper.Common.GettingReceiptTcID(BillingReturnDetails.TC, BillingReturnDetails.Cmpid);

                if (RecContraID == null)
                {
                    return new
                    {
                        Success = false,
                        Message = "Receipt Running Number Does'nt Mapped in Transaction Table"
                    };
                }

                BillingReturnDetails.ReceiptRunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), BillingReturnDetails.Cmpid, "GetRunningNo");

                if (BillingReturnDetails.ReceiptRunningNo == "Running Number Does'nt Exist")
                {
                    return new
                    {
                        Success = false,
                        Message = "Receipt Running Number Does'nt Exist"
                    };
                }
            }
            BillingReturnDetails.RunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(BillingReturnDetails.TC, BillingReturnDetails.Cmpid, "GetRunningNo");

            if (BillingReturnDetails.RunningNo == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }


            return _repoWrapper.Billing.AddOutPatientBillingReturns(BillingReturnDetails,GMT);
        }


        [HttpGet("CurrentDateSearchCreditBill/{SelectedDate}/{CmpID}/{Tc}/{GMT}")]
        public dynamic CurrentDateSearchCreditBill(DateTime SelectedDate, int CmpID,int Tc, string GMT)
        {

            return _repoWrapper.Billing.CurrentDateSearchCreditBill(SelectedDate, CmpID, Tc, GMT);
        }

        [HttpGet("PeriodSearchCreditBill/{FromDate}/{ToDate}/{CmpID}/{Tc}/{GMT}")]
        public dynamic PeriodSearchCreditBill(DateTime FromDate,DateTime ToDate, int CmpID, int Tc, string GMT)
        {

            return _repoWrapper.Billing.PeriodSearchCreditBill(FromDate, ToDate, CmpID, Tc, GMT);
        }


        [HttpGet("GetCreditItemDetails/{MedicalBillID}/{CmpID}/{TC}/{GMT}")]
        public dynamic GetCreditItemDetails(int MedicalBillID, int CmpID, int TC, string GMT)
        {
            return _repoWrapper.Billing.GetCreditItemDetails(MedicalBillID, CmpID, TC, GMT);
        }

    }
}
