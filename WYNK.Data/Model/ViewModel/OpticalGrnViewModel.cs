using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data;

namespace WYNK.Data.Model.ViewModel
{
    public class OpticalGrnDataView
    {
        public ICollection<GetOpticalGrn> GetOpticalGrn { get; set; }
        public ICollection<GetOpticalGrntrns> GetOpticalGrntrns { get; set; }
        public OpticalStockMaster OpticalStockMaster { get; set; }
        public OpticalStockTran OpticalStockTran { get; set; }
        public ICollection<GetOpticalGrndetails> GetOpticalGrndetails { get; set; }
        public ICollection<GetOpticalGrntrnsstock> GetOpticalGrntrnsstock { get; set; }
        public ICollection<GetOpticalGrntrnsstockprint> GetOpticalGrntrnsstockprint { get; set; }
        public Decimal? TotalPOValue { get; set; }
        public string Suppliercity { get; set; }
        public string SupplierState { get; set; }
        public string Suppliercountry { get; set; }
        public string Supplierlocation { get; set; }
        public string Deliverylocation { get; set; }
        public string Deliverycity { get; set; }
        public string DeliveryState { get; set; }
        public string Deliverycountry { get; set; }
        public string companyAddress { get; set; }
        public string phone { get; set; }
        public string web { get; set; }
        public string Compnayname { get; set; }
        public string companyAddress1 { get; set; }
        public string companyAddress2 { get; set; }
        public string OpOrdernumber { get; set; }
        public DateTime OpOrderDate { get; set; }
        public string Refnumber { get; set; }
        public DateTime? RefDate { get; set; }
        public string Documentnumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string Suppliername { get; set; }
        public string Address { get; set; }
        public string Phoneno { get; set; }
        public string Taxno { get; set; }
        public string Location { get; set; }
        public string State { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierAddress1 { get; set; }
        public string GstNo { get; set; }
        public string PhoneNo { get; set; }
        public string DeliveryName { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryAddress1 { get; set; }

    }
    public class GetOpticalGrn
    {
        public string ID { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string RefNo { get; set; }
        public DateTime? RefDate { get; set; }
        public int VendorID { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public string GstNo { get; set; }
        public string PhoneNo { get; set; }
        public string SupplierAddress1 { get; set; }
        public string DeliveryName { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryAddress1 { get; set; }
        public string Supplierlocation { get; set; }
        public string Deliverylocation { get; set; }
        public DateTime CreateUtc { get; set; }


    }

    public class GetOpticalGrntrns
    {
        public string RandomUniqueID { get; set; }
        public int LTID { get; set; }
        public int UOMID { get; set; }
        public string UOMname { get; set; }
        public Decimal OrderedQty { get; set; }
        public Decimal ReceivedQty { get; set; }
        public Decimal Price { get; set; }
        public Decimal ActualQuantity { get; set; }
        public Decimal Value { get; set; }
        public Decimal? TotalCost { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Sph { get; set; }
        public string Cyl { get; set; }
        public string Axis { get; set; }
        public string Add { get; set; }
        public string Fshpae { get; set; }
        public string Ftype { get; set; }
        public string Fwidth { get; set; }
        public string Fstyle { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public Decimal? DiscountPercentage { get; set; }
        public Decimal? DiscountAmount { get; set; }

    }


    public class GetOpticalGrndetails
    {
        public string RandomUniqueID { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string RefNo { get; set; }
        public DateTime? RefDate { get; set; }
        public string GrnNumber { get; set; }
        public DateTime? GrnDate { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public string GstNo { get; set; }
        public string PhoneNo { get; set; }
        public string SupplierAddress1 { get; set; }
        public string Supplierlocation { get; set; }
        public string DeliveryName { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryAddress1 { get; set; }
        public string Deliverylocation { get; set; }
        public string store { get; set; }
        public string Termcondition { get; set; }
        public DateTime CreateUtc { get; set; }

    }

    public class GetOpticalGrntrnsstock
    {
        public string UOMname { get; set; }
        public Decimal? ItemQty { get; set; }
        public Decimal OrderedQty { get; set; }
        public Decimal PendingQty { get; set; }
        public Decimal? Itemrate { get; set; }
        public Decimal? Itemvalue { get; set; }
        public Decimal TotalPOValue { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public Decimal? DiscountPercentage { get; set; }
        public Decimal? DiscountAmount { get; set; }
        public Decimal? TotalCost { get; set; }
        public string Sph { get; set; }
        public string Cyl { get; set; }
        public string Axis { get; set; }
        public string Add { get; set; }
        public string Fshpae { get; set; }
        public string Ftype { get; set; }
        public string Fwidth { get; set; }
        public string Fstyle { get; set; }
    }

    public class GetOpticalGrntrnsstockprint
    {
        public string UOMname { get; set; }
        public Decimal? ItemQty { get; set; }
        public Decimal? Itemrate { get; set; }
        public Decimal? Itemvalue { get; set; }
        public Decimal TotalPOValue { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public Decimal? DiscountPercentage { get; set; }
        public Decimal? DiscountAmount { get; set; }
        public Decimal? TotalCost { get; set; }
        public string Sph { get; set; }
        public string Cyl { get; set; }
        public string Axis { get; set; }
        public string Add { get; set; }
        public string Fshpae { get; set; }
        public string Ftype { get; set; }
        public string Fwidth { get; set; }
        public string Fstyle { get; set; }

    }

}
