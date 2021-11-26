using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class InterDepartmentTransfer
    {
        public ICollection<StoreDetail> StoreDetails { get; set; }
    }

    public class InterDepartmentDrugDetails
    {
        public ICollection<DrugDetails> DrugDetails { get; set; }
    }

    public class InterDepartmentTransferDetails
    {
        public ICollection<InterDepartmentTransferDetail> InterDepartmentTransferDetail { get; set; }
    }

    public class InterDepartmentTransferDetail
    {
        public string stockTransNo { get; set; }
        public string Date { get; set; }
        public DateTime created { get; set; }
        public string FromStore { get; set; }
        public string ToStore { get; set; }
    }

    public class SubmitTransferdetails
    {
        public int SupplierStoreID { get; set; }
        public int ReceiverStoreID { get; set; }
        public int Cmpid { get; set; }
        public int TransactionId { get; set; }
        public string RunningNoStock { get; set; }
        public int  CreatedBy { get; set; }
        public string FYear { get; set; }
        public string GivenDate { get; set; }
        public ICollection<interTransfer> interTransfers { get; set; }
        public ICollection<InSufficientDrug> InSufficientDrugs { get; set; }
        public ICollection<SerialDetail> InSufficientSerials { get; set; }
        public ICollection<OtherDrugs> InSufficientOtherDrugs { get; set; }
    }



    public class interTransfer
    {
        public int ID { get; set; }
        public string Brand { get; set; }
        public string GenericName { get; set; }
        public string UOM { get; set; }
        public decimal Quantity { get; set; }
        public bool? IsSerial { get; set; }
        public ICollection<BatchInfo> BatchDetail { get; set; }
        public ICollection<SerialInfo> SelectedList { get; set; }
        //ICollection<int> SelectedList
    }


    public class DrugDetails
    {
        public int DrugID { get; set; }
        public string Drug { get; set; }
        public string DrugGroup { get; set; }
        public string UOM { get; set; }
    }

    public class StoreDetail
    {
        public int ID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Location { get; set; }
        public string PhoneNo { get; set; }
    }


    public class DrugDetails1
    {
        public decimal? Quantity { get; set; }
        public string Brand { get; set; }
        public string GenericName { get; set; }
        public string UOM { get; set; }

        public bool? IsSerial { get; set; }

        public ICollection<SerialDetailInfo> SerialDetails { get; set; }
        public ICollection<BatchDetailInfo> BatchDetails { get; set; }
    }



    public class SerialDetailInfo
    {
        public string Brand { get; set; }
        public string SerialNo { get; set; }
    }


    public class BatchDetailInfo
    {
        public string Batch { get; set; }
        public DateTime Expiry { get; set; }
        public int qty { get; set; }
    }
    public class StockTransferDetails
    {
        public string StockTransferNo { get; set; }
        public DateTime StockTransferDate { get; set; }
        public object CompanyDetails { get; set; }
        public ICollection<Supplierdetail> Supplierdetail { get; set; }
        public ICollection<Receiverdetail> Receiverdetail { get; set; }
        public ICollection<DrugDetails1> DrugDetails { get; set; }
    }

    public class StockTransferDetails1
    {
        public string StockTransferNo { get; set; }
        public string RefNo { get; set; }
        public string StockTransferDate { get; set; }
        public DateTime? RefDate { get; set; }
        public object CompanyDetails { get; set; }
        public ICollection<Supplierdetail> Supplierdetail { get; set; }
        public ICollection<Receiverdetail> Receiverdetail { get; set; }
        public ICollection<DrugDetails1> DrugDetails { get; set; }
        public string TransportMode { get; set; }
        public string TransportationNo { get; set; }
        public DateTime? Transportationdate { get; set; }
        public string TransportationName { get; set; }
        public decimal? TransportationCharges { get; set; }
        public string Remarks { get; set; }
        public string RandomUniqueId { get; set; }
    }


    public class Supplierdetail
    {
        public string SupplierStore { get; set; }
        public string SupplierAddress1 { get; set; }
        public string SupplierAddress2 { get; set; }
        public string SupplierLocation { get; set; }
        public string SupplierPhoneNo { get; set; }
    }

    public class Receiverdetail
    {
        public string ReceiverStore { get; set; }
        public string ReceiverAddress1 { get; set; }
        public string ReceiverAddress2 { get; set; }
        public string ReceiverLocation { get; set; }
        public string ReceiverPhoneNo { get; set; }
    }


    public class GetCompanyIds
    {
        public ICollection<CompanyIDSs> CompanyIDSs { get; set; }
    }

    public class CompanyIDSs
    {
        public int CompanyIDS { get; set; }
    }

    public class SubmitInterBanchdetails
    {
        public int SupplierStoreID { get; set; }
        public int ReceiverStoreID { get; set; }
        public int Cmpid { get; set; }
        public int TransactionId { get; set; }
        public string RunningNoStock { get; set; }
        public int CreatedBy { get; set; }
        public string Issuedate { get; set; }
        public DateTime? RefDate { get; set; }
        public string RefNo { get; set; }
        public string Remarks { get; set; }
        public string Transportation { get; set; }
        public string TransportationNo { get; set; }
        public string TransportationName { get; set; }
        public decimal? TransportationCharges { get; set; }
        public DateTime? Transportationdate { get; set; }
        public ICollection<interTransfer> interTransfers { get; set; }
        public ICollection<InSufficientDrug> InSufficientDrugs { get; set; }
        public ICollection<SerialDetail> InSufficientSerials { get; set; }
        public ICollection<OtherDrugs> InSufficientOtherDrugs { get; set; }
    }

    public class GetItemListDetails
    {
        public int TransactionId { get; set; }
        public string Status { get; set; }
        public ICollection<int> SelectedBranches { get; set; }
    }

    public class IssueStatusDetails
    {
        public string Status { get; set; }
        public string RandomUniqueId { get; set; }
        public int CreatedBy { get; set; }
    }

}
