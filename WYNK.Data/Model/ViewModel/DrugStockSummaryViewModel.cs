using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
  public  class DrugStockSummaryDataView
    {


        public ICollection<Summaryy> Summary { get; set; }//Ledgers
        public ICollection<DrugSerial> DrugSerial { get; set; }
        public ICollection<DrugBatch> DrugBatch { get; set; }
        public ICollection<Ledgers> Ledgers { get; set; }
        public ICollection<BrandArrays> BrandArrays { get; set; }
        public ICollection<StoreArrays> StoreArrays { get; set; }
        public ICollection<Companycommu> Companycommu { get; set; }
        public ICollection<Stocksummaryarrayd> Stocksummaryarrayd { get; set; }
        public ICollection<OpticalStocksummaryarray> OpticalStocksummaryarray { get; set; }
        public decimal openstock { get; set; }
        public decimal closestock { get; set; }

        public ICollection<BrandArray> BrandArray { get; set; }
        public ICollection<StoreArray> StoreArray { get; set; }
        public ICollection<Opticalstockledgerd> Opticalstockledgerd { get; set; }
        public ICollection<OpticalstockledgerId> OpticalstockledgerId { get; set; }
        public ICollection<Receiptd> Receiptd { get; set; }
        public ICollection<Issued> Issued { get; set; }
        public ICollection<Companycommd> Companycommd { get; set; }
    }
    public class Stocksummaryarrayd
    {
        public string CmpName { get; set; }
        public string Type { get; set; }
        public string Store { get; set; }
        public string Brand { get; set; }
        public string UOM { get; set; }
        public string Sph { get; set; }
        public string Cyl { get; set; }
        public string Axis { get; set; }
        public string Add { get; set; }
        public string Fshpae { get; set; }
        public string Ftype { get; set; }
        public string Fwidth { get; set; }
        public string Fstyle { get; set; }
        public string Color { get; set; }
        public decimal Openingstock { get; set; }
        public int ID { get; set; }
        public int Rec01 { get; set; }
        public int Rec02 { get; set; }
        public int Rec03 { get; set; }
        public int Rec04 { get; set; }
        public int Rec05 { get; set; }
        public int Rec06 { get; set; }
        public int Rec07 { get; set; }
        public int Rec08 { get; set; }
        public int Rec09 { get; set; }
        public int Rec10 { get; set; }
        public int Rec11 { get; set; }
        public int Rec12 { get; set; }
        public int Iss01 { get; set; }
        public int Iss02 { get; set; }
        public int Iss03 { get; set; }
        public int Iss04 { get; set; }
        public int Iss05 { get; set; }
        public int Iss06 { get; set; }
        public int Iss07 { get; set; }
        public int Iss08 { get; set; }
        public int Iss09 { get; set; }
        public int Iss10 { get; set; }
        public int Iss11 { get; set; }
        public int Iss12 { get; set; }
        public int LTID { get; set; }
        public int StoreID { get; set; }
        public int CmpID { get; set; }
    }
    public class Ledgers
    {
        public string CmpName { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string DocumentNo { get; set; }//CmpName
        public string DocumentType { get; set; }
        public string Store { get; set; }
        public string Brand { get; set; }
        public string UOM { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal? Receipt { get; set; }
        public decimal Issue { get; set; }
        public decimal Balance { get; set; }
        public decimal ClosingBalance { get; set; }//Receipt


    }
    public class Summaryy
    {
        public string CmpName { get; set; }
        public string Store { get; set; }
        public string Brand { get; set; }
        public string UOM { get; set; } 
        public string GenericName { get; set; }
        public string DrugGroup { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal Receipt { get; set; }
        public decimal Issue { get; set; }
        public decimal ClosingBalance { get; set; }
        public int? DrugCtgy { get; set; }
        public int DrugID { get; set; }
    }

    public class DrugSerial
    {

        public string Brand { get; set; }
        public string GenericName { get; set; }
        public string grnno { get; set; }
        public string SerialNo { get; set; }
        public DateTime? ExpiryDt { get; set; }
        public DateTime? grndt { get; set; }
        public string trans { get; set; }

    }

    public class DrugBatch
    {

        public string Brand { get; set; }
        public string GenericName { get; set; }
        public string BatchNo { get; set; }
        public decimal? BatchQty { get; set; }
        public DateTime? ExpiryDt { get; set; }
        public string grnno { get; set; }
        public DateTime grndt { get; set; }
        public string trans { get; set; }

    }

    public class Companycommd
    {
        public string Companyname { get; set; }
        public string Address { get; set; }
        public string Phoneno { get; set; }
        public string Web { get; set; }
        public string Location { get; set; }
    }

    public class StoreArrayd
    {
        public string StoreID { get; set; }
        public string StoreName { get; set; }
    }
    public class BrandArrayd
    {
        public string BrandID { get; set; }
        public string BrandName { get; set; }

    }

    public class Opticalstockledgerd
    {
        public string CmpName { get; set; }
        public string DocumentNo { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string DocumentType { get; set; }
        public string Type { get; set; }
        public int TypeID { get; set; }
        public string Store { get; set; }
        public int StoreID { get; set; }
        public string Brand { get; set; }
        public int BrandID { get; set; }
        public string UOM { get; set; }
        public string Color { get; set; }
        public decimal Receipt { get; set; }
        public decimal Openingstock { get; set; }
        public decimal Closingstock { get; set; }
        public int LTID { get; set; }
        public string Sphh { get; set; }
        public string Cyll { get; set; }
        public string Axiss { get; set; }
        public string Addd { get; set; }
        public string Fshpaee { get; set; }
        public string Ftypee { get; set; }
        public string Fwidthh { get; set; }
        public string Fstylee { get; set; }
        public int ID { get; set; }
        public int CmpID { get; set; }
    }
    public class OpticalstockledgerId
    {
        public string CmpName { get; set; }
        public string DocumentNo { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string DocumentType { get; set; }
        public string Type { get; set; }
        public int TypeID { get; set; }
        public string Store { get; set; }
        public int StoreID { get; set; }
        public string Brand { get; set; }
        public int BrandID { get; set; }
        public string UOM { get; set; }
        public string Color { get; set; }
        public decimal Issue { get; set; }
        public decimal Openingstock { get; set; }
        public decimal Closingstock { get; set; }
        public int ID { get; set; }
        public int LTID { get; set; }
        public int CmpID { get; set; }
        public string Sphh { get; set; }
        public string Cyll { get; set; }
        public string Axiss { get; set; }
        public string Addd { get; set; }
        public string Fshpaee { get; set; }
        public string Ftypee { get; set; }
        public string Fwidthh { get; set; }
        public string Fstylee { get; set; }
    }

    public class Receiptd
    {
        public string CmpName { get; set; }
        public int CmpID { get; set; }
        public string DocumentNo { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string DocumentType { get; set; }
        public string Type { get; set; }
        public int TypeID { get; set; }
        public string Store { get; set; }
        public int StoreID { get; set; }
        public string Brand { get; set; }
        public int BrandID { get; set; }
        public string UOM { get; set; }
        public string Color { get; set; }
        public decimal Recept { get; set; }
        public decimal Issue { get; set; }
        public int ID { get; set; }
        public int Rec01 { get; set; }
        public int Rec02 { get; set; }
        public int Rec03 { get; set; }
        public int Rec04 { get; set; }
        public int Rec05 { get; set; }
        public int Rec06 { get; set; }
        public int Rec07 { get; set; }
        public int Rec08 { get; set; }
        public int Rec09 { get; set; }
        public int Rec10 { get; set; }
        public int Rec11 { get; set; }
        public int Rec12 { get; set; }
        public int Iss01 { get; set; }
        public int Iss02 { get; set; }
        public int Iss03 { get; set; }
        public int Iss04 { get; set; }
        public int Iss05 { get; set; }
        public int Iss06 { get; set; }
        public int Iss07 { get; set; }
        public int Iss08 { get; set; }
        public int Iss09 { get; set; }
        public int Iss10 { get; set; }
        public int Iss11 { get; set; }
        public int Iss12 { get; set; }
        public int LTID { get; set; }
        public decimal Openingstock { get; set; }
        public string Sphh { get; set; }
        public string Cyll { get; set; }
        public string Axiss { get; set; }
        public string Addd { get; set; }
        public string Fshpaee { get; set; }
        public string Ftypee { get; set; }
        public string Fwidthh { get; set; }
        public string Fstylee { get; set; }
    }
    public class Issued
    {
        public string CmpName { get; set; }
        public string DocumentNo { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string DocumentType { get; set; }
        public string Type { get; set; }
        public int TypeID { get; set; }
        public string Store { get; set; }
        public int StoreID { get; set; }
        public string Brand { get; set; }
        public int BrandID { get; set; }
        public string UOM { get; set; }
        public string Color { get; set; }
        public decimal Receipt { get; set; }
        public decimal Isue { get; set; }
        public int Rec01 { get; set; }
        public int Rec02 { get; set; }
        public int Rec03 { get; set; }
        public int Rec04 { get; set; }
        public int Rec05 { get; set; }
        public int Rec06 { get; set; }
        public int Rec07 { get; set; }
        public int Rec08 { get; set; }
        public int Rec09 { get; set; }
        public int Rec10 { get; set; }
        public int Rec11 { get; set; }
        public int Rec12 { get; set; }
        public int Iss01 { get; set; }
        public int Iss02 { get; set; }
        public int Iss03 { get; set; }
        public int Iss04 { get; set; }
        public int Iss05 { get; set; }
        public int Iss06 { get; set; }
        public int Iss07 { get; set; }
        public int Iss08 { get; set; }
        public int Iss09 { get; set; }
        public int Iss10 { get; set; }
        public int Iss11 { get; set; }
        public int Iss12 { get; set; }
        public int LTID { get; set; }
        public decimal Openingstock { get; set; }
        public int ID { get; set; }
        public int CmpID { get; set; }
        public string Sphh { get; set; }
        public string Cyll { get; set; }
        public string Axiss { get; set; }
        public string Addd { get; set; }
        public string Fshpaee { get; set; }
        public string Ftypee { get; set; }
        public string Fwidthh { get; set; }
        public string Fstylee { get; set; }

    }

}
