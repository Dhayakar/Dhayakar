using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class PurchaseOrdDetView
    {

        public PurchaseOrderTrans PurchaseOrderTrans { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public Vendor_Masters Vendor_Masters { get; set; }
        public Drug_Master drugMaster { get; set; }

        public ICollection<getPODet> getPODet { get; set; }
        public ICollection<getPODet1> getPODet1 { get; set; }
        public ICollection<getP_Det> getP_Det { get; set; }
    }

    public class getPODet
    {
        public string Po_No { get; set; }
        public DateTime Po_Date { get; set; }

        public string drugName { get; set; }

        public decimal drugQuantity { get; set; }

        public decimal drugRcvdQuantity { get; set; }

        public string UOM { get; set; }

        public decimal? itemRate { get; set; }

        public decimal itemValue { get; set; }

        public decimal discountPercentage { get; set; }

        public decimal discountAmount { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal GSTAmount { get; set; }

        public string VendorName { get; set; }
        public string Quatation_No { get; set; }

    }

    public class getP_Det
    {
        public string P_Po_No { get; set; }
        public DateTime P_Po_Date { get; set; }

        public string V_Name { get; set; }
        public string Q_No { get; set; }



    }

    public class getPODet1
    {
        // public Int64 Po_id { get; set; }
        public string Po_No1 { get; set; }
        public DateTime Po_Date1 { get; set; }

        public string drugName1 { get; set; }

        public decimal drugQuantity1 { get; set; }

        public decimal drugRcvdQuantity1 { get; set; }

        public decimal PendingQuantity1 { get; set; }

        public string UOM1 { get; set; }

        public decimal? itemRate1 { get; set; }

        public decimal itemValue1 { get; set; }

        public decimal discountPercentage1 { get; set; }

        public decimal discountAmount1 { get; set; }
        public decimal GSTPercentage1 { get; set; }
        public decimal GSTAmount1 { get; set; }

        public decimal ItemValue2 { get; set; }

        public decimal discountAmount2 { get; set; }
        public decimal GSTAmount2 { get; set; }



    }






}
