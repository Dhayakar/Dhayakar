using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class CustomerOrderViewModel
    {
        
        public int Cmpid { get; set; }
        public int Tc { get; set; }
        public string OrderDate { get; set; }
        public int CreatedBy { get; set; }

        public int CustomerId { get; set; }
        public string RefNo { get; set; }
        public DateTime? RefDate { get; set; }
        public DateTime? Deliverydate { get; set; }
        public string Remarks { get; set; }
        public string Fyear { get; set; }
        public string RunningNo { get; set; }
        public ICollection<opticprescription1> opticprescription1 { get; set; }
        public ICollection<OpticalPRESCRIPTION> FINALPRESCRIPTION { get; set; }
        public ICollection<Payment_Master> paymenttran { get; set; }

        public ICollection<CustomerItemOrder> CustomerItemOrders { get; set; }

        public CustomerOrder CustomerOrder { get; set; }
        public CustomerOrderTran CustomerOrderTran { get; set; }

        public string OrderNo { get; set; }
        public string CancelledReasons { get; set; }
        public string ReceiptRunningNo { get; set; }
        public string UIN { get; set; }
        public int? RegTranId { get; set; }
        
    }


    public class opticprescription1
    {
        public int? Type { get; set; }
        public string Ocular { get; set; }
        public string DistSph { get; set; }
        public string NearCyl { get; set; }
        public string PinAxis { get; set; }
        public string Add { get; set; }
        public string Remarks { get; set; }
        public string PD { get; set; }
        public string MPDOD { get; set; }
        public string MPDOS { get; set; }

    }


    public class OpticalPRESCRIPTION
    {
        public int ID { get; set; }
        public int IDOS { get; set; }
        public string Description { get; set; }
        public string Ocular { get; set; }
        public string OcularOS { get; set; }
        public string DistSph { get; set; }
        public string NearCyl { get; set; }
        public string PinAxis { get; set; }
        public string Add { get; set; }
        public string DistSphOS { get; set; }
        public string NearCylOS { get; set; }
        public string PinAxisOS { get; set; }
        public string AddOS { get; set; }
        public string DistSphNVOD { get; set; }
        public string AddNVOD { get; set; }
        public string DistSphNVOS { get; set; }
        public string AddNVOS { get; set; }
        public string Remarks { get; set; }
        public string PD { get; set; }
        public string MPDOD { get; set; }
        public string MPDOS { get; set; }
        public int DV { get; set; }
        public int NV { get; set; }
        public string DVName { get; set; }
        public string NVName { get; set; }
        public DateTime CreatedUTc { get; set; }
        public int? Subcategory { get; set; }
        public Boolean OD { get; set; }
        public Boolean OS { get; set; }
    }

    public class CustomerOrderedList
    {
        public string RefNo { get; set; }
        public DateTime? RefDate { get; set; }
        public string OrderNo { get; set; }
        public DateTime? OrderDate { get; set; }

        public string ReceiptNumber { get; set; }
        public DateTime? M_ReceiptNoDate { get; set; }
        public DateTime? Deliverydate { get; set; }
        public string Remarks { get; set; }

        public string CustomerName { get; set; }
        public string CustomerAddress1 { get; set; }
        public string CustomerAddress2 { get; set; }
        public string CustomerAddress3 { get; set; }
        public string CustomerMobileNo { get; set; }
        public ICollection<CustomerItemOrder> CustomerItemOrders { get; set; }
        public ICollection<Payment_Master> paymenttran { get; set; }
        public ICollection<Payment_Master> RefundDetails { get; set; }
        public List<string> OpticalPrescription { get; set; }
    }


    public class CustomerItemOrder
    {
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string LensOptions { get; set; }
        public string Index { get; set; }
        public string Color { get; set; }
        public string HSNNo { get; set; }
        public string UOM { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountAmount { get; set; }

        public decimal? GST { get; set; }
        public decimal? SGST { get; set; }
        public decimal? CGST { get; set; }


        public decimal? CESS { get; set; }
        public decimal? AddCess { get; set; }

        public string GSTDesc { get; set; }
        public string CESSDesc { get; set; }
        public string AddCessDesc { get; set; }


        public decimal? GSTValue { get; set; }
        public decimal? CESSValue { get; set; }
        public decimal? AddCessValue { get; set; }


        public decimal GrossAmount { get; set; }
        public decimal Amount { get; set; }
        public int LMID { get; set; }
        public int LTID { get; set; }


        public string Sph { get; set; }
        public string Cyl { get; set; }
        public string Axis { get; set; }
        public string Add { get; set; }
        public string Description { get; set; }
    }


    public class CustomerSubmit
    {

        public CustomerData CustomerDatas { get; set; }
    }
   
    public class CustomerData 
    {
        public string UIN { get; set; }
        public string FirstName { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string MobileNo { get; set; }


    }
}

