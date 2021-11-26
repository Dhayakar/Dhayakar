using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{

    public class FinalBillingMaster
    {
        public Payment_Master payment { get; set; }
        public ICollection<patientDtls> patientDtls { get; set; }//PayDetailsspp
        public ICollection<PayDetailsspp> PayDetailsspp { get; set; }
        public ICollection<patientDtlsp> patientDtlsp { get; set; }
        public ICollection<ServiceDtlsprintp> ServiceDtlsprintp { get; set; }
        public ICollection<patientDtlsprint> patientDtlsprint { get; set; }
        public ICollection<BillingDtls> BillingDtls { get; set; }
        public ICollection<ServiceDtls> ServiceDtls { get; set; }
        public ICollection<ServiceDtlsprint> ServiceDtlsprint { get; set; }
        public ICollection<ItmDtls> ItmDtls { get; set; }
        public ICollection<Payment_Master> PaymentMaster { get; set; }
        public PatientAccount PatientAccount { get; set; }
        public ICollection<PatientAccountDetail> PatientAccountDetail { get; set; }
        public ICollection<PayDetailss> PayDetailss { get; set; }
        public ICollection<ReprintServiceDtlsprintNew> ReprintServiceDtlsprintNew { get; set; }
        public ICollection<ServiceDtlsprintNew> ServiceDtlsprintNewss { get; set; }
        public string Companyname { get; set; }
        public decimal Totalamt { get; set; }
        public string Cmpaddress { get; set; }
        public string Cmplocation { get; set; }
        public string Cmpphno { get; set; }
        public string Cmpgstno { get; set; }
        public string ReceiptRunningNo { get; set; }
        public string Lensused { get; set; }
        public string Procedure { get; set; }
        

        public decimal? Totalcostyprint { get; set; }
        public decimal? totaldicountprint { get; set; }
        public decimal? totaltaxprint { get; set; }
        public decimal? totalnetamount { get; set; }

        public DateTime Admdate { get; set; }
        public string Admnumber { get; set; }

        public DateTime? Invoicedate { get; set; }

        public decimal? totalcostamount { get; set; }
        public decimal? minustotalnetamount { get; set; }

        public string InVoiceNumber { get; set; }
        public ICollection<PackageBillingList> PackageBillingList { get; set; }
        public ICollection<unPackageBillingList> unPackageBillingLists { get; set; }
        public ICollection<PackageBillingListtotal> PackageBillingListtotals { get; set; }
    }


    public class PackageBillingList
    {
        public string Description { get; set; }
        public Decimal? TotalCost { get; set; }
        public Decimal? Amount { get; set; }
        public decimal? Discount { get; set; }

        public decimal? TempDiscount { get; set; }
        public decimal? GSTAmount { get; set; }        
        //public ICollection<unPackageBillingList> unPackageBillingList { get; set; }
        public ICollection<PackageBillingListtotal> PackageBillingListtotal { get; set; }
    }


    public class PackageBillingListtotal
    {
        public string Description { get; set; }
        public string ServiceDescription { get; set; }
        public Decimal? Amount { get; set; }

        public Decimal? Dummyamout { get; set; }
        public DateTime? Date { get; set; }
    }

    public class unPackageBillingList
    {
        public string Description { get; set; }
        public string ServiceDescription { get; set; }
        public Decimal? Amount { get; set; }
        public Decimal? Dummyamout { get; set; }
        public DateTime? Date { get; set; }
    }

    public class Packagebillingprint
    {
        public int Description { get; set; }
        public string uin { get; set; }
        public string billno { get; set; }
        public DateTime? billdt { get; set; }
        public Decimal? total { get; set; }
        public string name { get; set; }
        public string age { get; set; }
        public string gender { get; set; }
        public string address { get; set; }
        public string phoneno { get; set; }

    }


    public class PayDetailsspp

    {
        public string paymode { get; set; }
        public string instno { get; set; }
        public DateTime? instdt { get; set; }
        public string bname { get; set; }
        public string branch { get; set; }
        public DateTime? expiry { get; set; }
        public Decimal amount { get; set; }
        public Decimal tamount { get; set; }
        public string bilnum { get; set; }
        public DateTime? bildtt { get; set; }
    }
    public class patientDtls
    {
        public int paid { get; set; }
        public string uin { get; set; }
        public string name { get; set; }
        public string age { get; set; }
        public string gender { get; set; }
        public string address { get; set; }
        public string phoneno { get; set; }
        public string billtype { get; set; }
        public string billno { get; set; }
        public DateTime? billdate { get; set; }

    }

    public class patientDtlsp
    {
        public int paid { get; set; }
        public string uin { get; set; }
        public string billno { get; set; }
        public DateTime? billdt { get; set; }
        public Decimal? total { get; set; }
        public string name { get; set; }
        public string age { get; set; }
        public string gender { get; set; }
        public string address { get; set; }
        public string phoneno { get; set; }

    }

    public class patientDtlsprint
    {
        public int paid { get; set; }
        public string uin { get; set; }
        public string name { get; set; }
        public string age { get; set; }
        public string gender { get; set; }
        public string address { get; set; }
        public string phoneno { get; set; }

    }

    public class BillingDtls
    {
        public int paid { get; set; }
        public string uin { get; set; }
        public string name { get; set; }
        public string age { get; set; }
        public string gender { get; set; }
        public string address { get; set; }
        public string phoneno { get; set; }

    }

    public class ServiceDtls
    {
        public int padid { get; set; }
        public int TaxID { get; set; }
        public int paccdtltaxid { get; set; }
        public int Qty { get; set; }
        public int Price { get; set; }
        public string ServiceDescription { get; set; }
        public string Lensused { get; set; }
        public string Procedure { get; set; }
        public string Description { get; set; }
        public Decimal? Amount { get; set; }
        public Decimal? Discount { get; set; }
        public Decimal? DummyDiscount { get; set; }
        public Decimal? DiscountAmount { get; set; }
        public string TaxDescription { get; set; }
        public string CESSDescription { get; set; }
        public string AdditionalCESSDescription { get; set; }
        public Decimal? GST { get; set; }
        public Decimal? CESS { get; set; }
        public Decimal? AdditionalCESS { get; set; }
        public Decimal? GSTAmount { get; set; }
        public Decimal? CESSAmount { get; set; }
        public Decimal? AdditionalCESSAmount { get; set; }

        public Decimal? CGST { get; set; }
        public Decimal? SGST { get; set; }

        public string COUNTRY { get; set; }

        public DateTime? Surgdate { get; set; }
        public Decimal? TotalCost { get; set; }
        //public ICollection<ServiceDtlsprintNew> ServiceDtlsprintNew { get; set; }
        public ICollection<PackageBillingList> PackageBillingLists { get; set; }

    }


    public class ReprintServiceDtlsprintNew
    {
        public string Billno { get; set; }
        public DateTime? billdate { get; set; }
        public Decimal? Amount { get; set; }
    }

    public class ServiceDtlsprintNew
    {
        public string ServiceDescription { get; set; }
        public string Description { get; set; }
        public Decimal? Discount { get; set; }
        public DateTime? Date { get; set; }
    }

 public class ServiceDtlsprint
    {
        public int padid { get; set; }
        public string ServiceDescription { get; set; }
        public string Description { get; set; }
        public Decimal? Amount { get; set; }
        public Decimal? Discount { get; set; }
        public Decimal? DiscountAmount { get; set; }
        public string TaxDescription { get; set; }
        public string CESSDescription { get; set; }
        public string AdditionalCESSDescription { get; set; }
        public Decimal? GST { get; set; }
        public Decimal? CESS { get; set; }
        public Decimal? AdditionalCESS { get; set; }
        public Decimal? GSTAmount { get; set; }
        public Decimal? CESSAmount { get; set; }
        public Decimal? AdditionalCESSAmount { get; set; }
        public Decimal? TotalCost { get; set; }

        public ICollection<ServiceDtlsprintNew> ServiceDtlsprintNew { get; set; }

    }

    public class ServiceDtlsprintp
    {
        public int padid { get; set; }
        public string ServiceDescription { get; set; }
        public string Description { get; set; }
        public Decimal? Amount { get; set; }
        public Decimal? Discount { get; set; }
        public Decimal? DiscountAmount { get; set; }
        public string TaxDescription { get; set; }
        public string CESSDescription { get; set; }
        public string AdditionalCESSDescription { get; set; }
        public Decimal? GST { get; set; }
        public Decimal? CESS { get; set; }
        public Decimal? AdditionalCESS { get; set; }
        public Decimal? GSTAmount { get; set; }
        public Decimal? CESSAmount { get; set; }
        public Decimal? AdditionalCESSAmount { get; set; }
        public Decimal? TotalCost { get; set; }

    }

    public class ItmDtls
    {
        public int paccdtltaxid { get; set; }
        public int TaxID { get; set; }
        public string Description { get; set; }
        public string Lensused { get; set; }
        public Decimal? Qty { get; set; }
        public Decimal? Price { get; set; }
        public Decimal? Amount { get; set; }
        public Decimal? Discount { get; set; }
        public Decimal? DiscountAmount { get; set; }
        public string TaxDescription { get; set; }
        public string CESSDescription { get; set; }
        public string AdditionalCESSDescription { get; set; }
        public Decimal? GST { get; set; }
        public Decimal? CESS { get; set; }
        public Decimal? AdditionalCESS { get; set; }
        public Decimal? GSTAmount { get; set; }
        public Decimal? CESSAmount { get; set; }
        public Decimal? AdditionalCESSAmount { get; set; }
        public Decimal? TotalCost { get; set; }

    }
}
