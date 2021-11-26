using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class InvestigationBilling
    {
        public Registration_Master Registration { get; set; }
        public InvestigationPrescription InvestigationPrescription { get; set; }
        public ICollection<InvestigationPrescriptionTran> InvestigationPrescriptionTran { get; set; }
        public ICollection<InvestigationImages> INV { get; set; }
        public ICollection<InvestigationTran> InvTran { get; set; }
        public InvestigationImages Investigation { get; set; }
        public InvestigationBillMaster InvestigationBillMaster { get; set; }
        public ICollection<InvestigationBillTran> InvestigationBillTran { get; set; }
        public ICollection<PatientBillDetails> PatientBillDetails { get; set; }//PayDetailss
        public ICollection<BillDetails> BillDetails { get; set; }
        public ICollection<TaxDetails> TaxDetails { get; set; }
        public ICollection<Payment_Master> PaymentMaster { get; set; }
        public ICollection<InvestDetails> InvestDetails { get; set; }
        public ICollection<InvestDetailsp> InvestDetailsp { get; set; }
        public ICollection<PayDetailss> PayDetailss { get; set; }
        public ICollection<ReBillingDetails> ReBillingDetails { get; set; }
        public ICollection<PayDetailssp> PayDetailssp { get; set; }
        public ICollection<GetTaxDetails> GetTaxDetails { get; set; }

        public int page { get; set; }//PayDetailssp
        public string CAddress { get; set; }
        public string Cphone { get; set; }

        public string Cweb { get; set; }
        public string Cname { get; set; }

        public int rpage { get; set; }//ReBillingDetails
        public string rCAddress { get; set; }
        public string rCphone { get; set; }
        public string rCweb { get; set; }
        public string rCname { get; set; }

    }



}

public class ReBillingDetails

{
    public string UIN { get; set; }
    public int Oid { get; set; }
    public int? ridd { get; set; }
    public string Name { get; set; }
    public string BillNo { get; set; }
    public DateTime? BillDate { get; set; }

}

public class InvestDetails

{
    public string Description { get; set; }
    public Decimal? value { get; set; }
    public Decimal? discount { get; set; }
    public Decimal? dvalue { get; set; }
    public Decimal grosamount { get; set; }
    public Decimal? gst { get; set; }
    public Decimal? gstvalue { get; set; }
    public Decimal? cess { get; set; }
    public Decimal? cessvalue { get; set; }
    public Decimal? addcess { get; set; }
    public Decimal? addcessvalue { get; set; }
    public Decimal? total { get; set; }
    public string TaxDescription { get; set; }
    public string CessDescription { get; set; }
    public string AddcessDescription { get; set; }

}


public class InvestDetailsp

{
    public string Descriptionp { get; set; }
    public Decimal? valuep { get; set; }
    public Decimal? discountp { get; set; }
    public Decimal? dvaluep { get; set; }
    public Decimal grosamountp { get; set; }
    public Decimal? gstp { get; set; }
    public Decimal? gstvaluep { get; set; }
    public Decimal? cessp { get; set; }
    public Decimal? cessvaluep { get; set; }
    public Decimal? addcessp { get; set; }
    public Decimal? addcessvaluep { get; set; }
    public Decimal? totalp { get; set; }

    public string TaxDescriptionp { get; set; }
    public string CessDescriptionp { get; set; }
    public string AddcessDescriptionp { get; set; }


}



public class PayDetailss

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

public class PayDetailssp

{
    public string paymodep { get; set; }
    public string instnop { get; set; }
    public DateTime? instdtp { get; set; }
    public string bnamep { get; set; }
    public string branchp { get; set; }
    public DateTime? expiryp { get; set; }
    public Decimal amountp { get; set; }

}


public class PatientBillDetails
{
    public int? rid { get; set; }
    public string ipid { get; set; }
    public string PrescNo { get; set; }
    public DateTime PrescribedDate { get; set; }
    public string PrescribedBy { get; set; }
    public string Remarks { get; set; }



}

public class BillDetails
{
            public string Investigation { get; set; }
        public string ipid { get; set; }
        public int iptid { get; set; }
        public int invid { get; set; }
        public Decimal? Amount { get; set; }
        public Decimal Discount { get; set; }
        public Decimal DiscountAmount { get; set; }
        public Decimal? GrossAmount { get; set; }    
        public Decimal? GSTAmount { get; set; } 
        public Decimal? CESSAmount { get; set; }
        public Decimal? AdditionalCESSAmount { get; set; }
        public Decimal? TotalCost { get; set; }




        //////////////////////tax//////////////////////////
         public string TaxDescription { get; set; }
    public int? TaxID { get; set; }
    public string CESSDescription { get; set; }
        public string AdditionalCESSDescription { get; set; }
        public Int16? GST { get; set; }
        public decimal? CESS { get; set; }
        public decimal? AdditionalCESS { get; set; }
        public decimal? CGSTPercentage { get; set; }
        public decimal? SGSTPercentage { get; set; }
        public decimal? IGSTPercentage { get; set; }


}


public class TaxDetails
{
    public Decimal? cess { get; set; }
    public Decimal? addcess { get; set; }
}