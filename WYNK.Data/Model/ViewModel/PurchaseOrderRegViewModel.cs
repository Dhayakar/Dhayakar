using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class PurchaseOrderRegView
    {
        public PurchaseOrderTrans PurchaseOrderTrans { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public Vendor_Masters Vendor_Masters { get; set; }
        public Drug_Master drugMaster { get; set; }
        public OneLine_Masters OneLine_Masters { get; set; }


        public ICollection<getPO_Reg> getPO_Reg { get; set; }

        public ICollection<getPO_Reg1> getPO_Reg1 { get; set; }
    }


    public class getPO_Reg
    {
        public string PoNo { get; set; }
        public DateTime PoDate { get; set; }
        public string Q_No { get; set; }
        public DateTime? Q_Date { get; set; }
        public string VendorName { get; set; }
        public string Dlvry_Name { get; set; }
        public string Dlvry_Addrss { get; set; }
        public string Location { get; set; }
        public decimal Gross_Prod_val { get; set; }
        public decimal Tot_Dis_val { get; set; }
        public decimal Tot_Tax_val { get; set; }
        public decimal Tot_CGSTTax_val { get; set; }
        public decimal Tot_SGSTTax_val { get; set; }
        public decimal Tot_Po_val { get; set; }
        public string PO_status { get; set; }
        public string Parent_PO_No { get; set; }
        public DateTime Parent_PO_Date { get; set; }



    }


    public class getPO_Reg1
    {
        public string PoNo1 { get; set; }
        public DateTime PoDate1 { get; set; }
        public string Parent_No1 { get; set; }
        public DateTime? Parent_Date1 { get; set; }
        public string VendorName1 { get; set; }
        public string Dlvry_Name1 { get; set; }
        public string Dlvry_Addrss1 { get; set; }
        public string Location1 { get; set; }
        public decimal Gross_Prod_val1 { get; set; }
        public decimal Tot_Dis_val1 { get; set; }
        public decimal Tot_Tax_val1 { get; set; }
        public decimal Tot_CGSTTax_val1 { get; set; }
        public decimal Tot_SGSTTax_val1 { get; set; }
        public decimal Tot_Po_val1 { get; set; }
        public string PO_status1 { get; set; }
        public string Parent_PO_No1 { get; set; }
        public DateTime Parent_PO_Date1 { get; set; }



    }






}
