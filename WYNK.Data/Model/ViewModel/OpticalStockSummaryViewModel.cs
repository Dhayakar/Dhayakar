using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class OpticalStockSummaryDataView
    {

        public ICollection<BrandArrays> BrandArrays { get; set; }
        public ICollection<StoreArrays> StoreArrays { get; set; }
        public ICollection<Companycommu> Companycommu { get; set; }
        public ICollection<Stocksummaryarray> Stocksummaryarray { get; set; }
        public ICollection<OpticalStocksummaryarray> OpticalStocksummaryarray { get; set; }
    }

    public class Companycommu
    {
        public string Companyname { get; set; }
        public string Address { get; set; }
        public string Phoneno { get; set; }
        public string Web { get; set; }
        public string Location { get; set; }
    }
    public class StoreArrays
    {
        public string StoreID { get; set; }
        public string StoreName { get; set; }
    }
    public class BrandArrays
    {
        public string BrandID { get; set; }
        public string BrandName { get; set; }

    }
    public class Stocksummaryarray
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
        public int StoreID { get; set; }
        public int CmpID { get; set; }
    }
    public class OpticalStocksummaryarray
    {
        public string CmpName { get; set; }
        public int CmpID { get; set; }
        public string Type { get; set; }
        public string Store { get; set; }
        public string Brand { get; set; }
        public string UOM { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public decimal Receipt { get; set; }
        public decimal Issue { get; set; }
        public decimal Openingstock { get; set; }
        public decimal Closingstock { get; set; }
        public string Sph { get; set; }
        public string Cyl { get; set; }
        public string Axis { get; set; }
        public string Add { get; set; }
        public string Fshpae { get; set; }
        public string Ftype { get; set; }
        public string Fwidth { get; set; }
        public string Fstyle { get; set; }
        public int ID { get; set; }
        public int LTID { get; set; }
        public int StoreID { get; set; }
    }

}
