using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
  public  class Dmmeterial
    {
        public string ReciptNo { get; set; }
        public string BatchList1 { get; set; }
        public DateTime RecipDate { get; set; }
        public string VendorN { get; set; }
        public string Adress1 { get; set; }
        public string Adress2 { get; set; }
        public string Location { get; set; }
        public string GSTNO { get; set; }
        public string MobileNo { get; set; }
        public int Vendorid { get; set; }
        public string Remarks { get; set; }



    }
}
