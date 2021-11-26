using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class OpticalStockLedgerDataView
    {
        public ICollection<BrandArray> BrandArray { get; set; }
        public ICollection<StoreArray> StoreArray { get; set; }
        public ICollection<Opticalstockledger> Opticalstockledger { get; set; }
        public ICollection<OpticalstockledgerI> OpticalstockledgerI { get; set; }
        public ICollection<Receipt> Receipt { get; set; }
        public ICollection<Issue> Issue { get; set; }
        public ICollection<Companycomm> Companycomm { get; set; }
    }


    public class Companycomm
    {
        public string Companyname { get; set; }
        public string Address { get; set; }
        public string Phoneno { get; set; }
        public string Web { get; set; }
        public string Location { get; set; }
    }

    public class StoreArray
    {
        public string StoreID { get; set; }
        public string StoreName { get; set; }
    }
    public class BrandArray
    {
        public string BrandID { get; set; }
        public string BrandName { get; set; }

    }

    public class Opticalstockledger
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
    public class OpticalstockledgerI
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

    public class Receipt
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
        public int REC01 { get; set; }
        public int REC02 { get; set; }
        public int REC03 { get; set; }
        public int REC04 { get; set; }
        public int REC05 { get; set; }
        public int REC06 { get; set; }
        public int REC07 { get; set; }
        public int REC08 { get; set; }
        public int REC09 { get; set; }
        public int REC10 { get; set; }
        public int REC11 { get; set; }
        public int REC12 { get; set; }
        public int ISS01 { get; set; }
        public int ISS02 { get; set; }
        public int ISS03 { get; set; }
        public int ISS04 { get; set; }
        public int ISS05 { get; set; }
        public int ISS06 { get; set; }
        public int ISS07 { get; set; }
        public int ISS08 { get; set; }
        public int ISS09 { get; set; }
        public int ISS10 { get; set; }
        public int ISS11 { get; set; }
        public int ISS12 { get; set; }
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
    public class Issue
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
        public int REC01 { get; set; }
        public int REC02 { get; set; }
        public int REC03 { get; set; }
        public int REC04 { get; set; }
        public int REC05 { get; set; }
        public int REC06 { get; set; }
        public int REC07 { get; set; }
        public int REC08 { get; set; }
        public int REC09 { get; set; }
        public int REC10 { get; set; }
        public int REC11 { get; set; }
        public int REC12 { get; set; }
        public int ISS01 { get; set; }
        public int ISS02 { get; set; }
        public int ISS03 { get; set; }
        public int ISS04 { get; set; }
        public int ISS05 { get; set; }
        public int ISS06 { get; set; }
        public int ISS07 { get; set; }
        public int ISS08 { get; set; }
        public int ISS09 { get; set; }
        public int ISS10 { get; set; }
        public int ISS11 { get; set; }
        public int ISS12 { get; set; }
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
