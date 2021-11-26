using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class StockViewmodel
    {
        public ICollection<StockDetails> StockDetails { get; set; }
        public ICollection<get> get { get; set; }
        public ICollection<StockDetails1> StockDetails1 { get; set; }
        public ICollection<StockDetails2> StockDetails2 { get; set; }
        public string Manufacturer { get; set; }
        public string BrandName { get; set; }


    }

    public class StockDetails
    {
        public string BrandName { get; set; }
        public decimal PhysicalStock { get; set; }


    }

    public class StockDetails1
    {
        public string BrandName { get; set; }
        public decimal PhysicalStock { get; set; }
        public decimal ConsumableStock { get; set; }
        public decimal ExpiredStock { get; set; }
        public string UOM { get; set; }



    }

    public class get
    {
        public string BrandName { get; set; }
        public decimal PhysicalStock { get; set; }
        public string UOM { get; set; }


    }
    public class StockDetails2
    {
        public string BrandName { get; set; }
        public string BatchNumber { get; set; }
        public decimal BatchQty { get; set; }
        public decimal? BatchIssueQty { get; set; }
        public decimal BatchBalanceQty { get; set; }
        public DateTime BatchExpiry { get; set; }
    }

}

