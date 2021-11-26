using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class InvestigationIPBilling
    {
        public Registration_Master Registration { get; set; }
        public InvestigationPrescription InvestigationPrescription { get; set; }
        public ICollection<GetRegDetails> GetRegDetails { get; set; }
        public ICollection<GetTaxDetails> GetTaxDetails { get; set; }
        
        /// <summary>
        /// //////////unbilled////////////////////////
        /// </summary>  
        public InvestigationImages Investigation { get; set; }
        public ICollection<InvestigationImages> INV { get; set; }
        public ICollection<BillDetailsIP> BillDetailsIP { get; set; }
        public ICollection<Payment_Master> PaymentMaster { get; set; }
        public ICollection<PatientBillDetailsIP> PatientBillDetailsIP { get; set; }//GetRegDetailsop
        public ICollection<GetRegDetailsop> GetRegDetailsop { get; set; }



        //////////////////////submit///////////////////////////////
        public InvestigationBillMaster InvestigationBillMaster { get; set; }
        public ICollection<InvestigationBillTran> InvestigationBillTran { get; set; }


    }

    public class GetTaxDetails
    {
        
        public int TaxID { get; set; }
        public string TaxDescription { get; set; }
        public string CESSDescription { get; set; }
        public string AdditionalCESSDescription { get; set; }
        public Int16? GSTPercentage { get; set; }

        public decimal? CESSPercentage { get; set; }
        public decimal? AdditionalCESSPercentage { get; set; }
        public decimal? CGSTPercentage { get; set; }
        public decimal? SGSTPercentage { get; set; }
        public decimal? IGSTPercentage { get; set; }


        
        
        
        
        

    }
    public class GetRegDetails
    {
        public string UIN { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public DateTime DateofBirth { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }

    }

    public class GetRegDetailsop
    {
        public string UIN { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public DateTime DateofBirth { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }

    }
    public class PatientBillDetailsIP
    {
        public int? rid { get; set; }
        public string ipid { get; set; }
        public string PrescNo { get; set; }
        public DateTime PrescribedDate { get; set; }
        public string PrescribedBy { get; set; }
        public string Remarks { get; set; }
        public string INVPRESNO { get; set; }
        

    }

    public class BillDetailsIP
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
        public int TaxID { get; set; }


        //////////////////////tax//////////////////////////
        public string TaxDescription { get; set; }
        public string CESSDescription { get; set; }
        public string AdditionalCESSDescription { get; set; }
        public Int16? GST { get; set; }
        public decimal? CESS { get; set; }
        public decimal? AdditionalCESS { get; set; }
        public decimal? CGSTPercentage { get; set; }
        public decimal? SGSTPercentage { get; set; }
        public decimal? IGSTPercentage { get; set; }


    }
}

