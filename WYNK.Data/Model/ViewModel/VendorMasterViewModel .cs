
using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class VendorMasterViewModel
    {

        public Vendor_Masters VendorMasters { get; set; }
        public DrugMaster Drugmaster { get; set; }
        public ICollection<Vendordetials> Vendordetials { get; set; }
        public ICollection<NONVendordetials> NONVendordetials { get; set; }
        public ICollection<NONdetials> nONdetials { get; set; }
        public string ParentDescriptioncity { get; set; }
        public string ParentDescriptionstate { get; set; }
        public string ParentDescriptioncountry { get; set; }
        public ICollection<IOtemvendormappingdetails> otemvendormappingdetails { get; set; }
        public ICollection<NONItemdetials> NONItemdetials { get; set; }
        public ICollection<VendorDetail> VendorDetails { get; set; }
        public ICollection<unVendorDetail> unvendordetails { get; set; }
        public ICollection<unitemDetail> unitemDetails { get; set; }
        public ICollection<ItemDetail> ItemDetails { get; set; }
        public ICollection<Itemdetials> itemdetials { get; set; }
        public int Code { get; set; }
        public string CompanyID { get; set; }
        public string UserID { get; set; }
        
    }



    public class unitemDetail
    {
        public string Description { get; set; }
        public Boolean Select { get; set; }

    }

    public class unVendorDetail
    {
        public string Description { get; set; }
        public Boolean Select { get; set; }

    }

    public class VendorDetail
    {
        public string Description { get; set; }
        public Boolean Select { get; set; }

    }

    public class ItemDetail
    {
        public string Description { get; set; }
        public Boolean Select { get; set; }

    }

    public class Vendordetials
    {
        public string Itemdescription { get; set; }
        public Boolean Itemselect { get; set; }

    }


    public class IOtemvendormappingdetails
    {
        public string Itemdescription { get; set; }
        public Boolean Itemselect { get; set; }

    }

    public class NONVendordetials
    {
        public string Itemdescription { get; set; }
        public Boolean Itemselect { get; set; }

    }
    public class NONdetials
    {
        public string Itemdescription { get; set; }
        public Boolean Itemselect { get; set; }

    }
    public class Itemdetials
    {
        public string Itemdescription { get; set; }
        public Boolean Vendorselect { get; set; }

    }
    public class NONItemdetials
    {
        public string Itemdescription { get; set; }
        public Boolean Vendorselect { get; set; }

    }

}





