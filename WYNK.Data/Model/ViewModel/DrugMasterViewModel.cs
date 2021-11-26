using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class DrugMaster
    {
        public string CMPID { get; set; }

        public string DrugLanguageDescription { get; set; }
        public Drug_Master drugMaster { get; set; }
        public Drug_Group DrugGroup { get; set; }

        public ICollection<getTaxData> getTaxData { get; set; }

    }



    //public class UpdateDrugMaster
    //{
    //    public UpdateDrugClass drugMaster { get; set; }

    //}


    //public class UpdateDrugClass 
    //{
    //    public string Brand { get; set; }
    //    public int? Manufacturer { get; set; }
    //    public int GenericName { get; set; }
    //    public string DrugGroup { get; set; }
    //    public string UOM { get; set; }
    //    public decimal Rate { get; set; }
    //    public string HSNNo { get; set; }
    //    public decimal? CESSPercentage { get; set; }
    //    public decimal? AdditionalCESSPercentage { get; set; }
    //    public int? TaxID { get; set; }
    //    public int? VendorCode { get; set; }
    //    public Boolean IsActive { get; set; }
    //    public DateTime? UpdatedUTC { get; set; }
    //    public int? UpdatedBy { get; set; }
    //    public int? DrugCategory { get; set; }
    //    public string DrugSubDescription { get; set; }
    //    public string DrugComposition { get; set; }
    //    public string Power { get; set; }
    //    public string Aconstant { get; set; }
    //    public string ModelNo { get; set; }
    //    public string OpticDia { get; set; }
    //    public string Length { get; set; }
    //    public int? DrugTracker { get; set; }

    //}

    public class getTaxData
    {
        public Int16? GSTPercentage { get; set; }
        public decimal? CGSTPercentage { get; set; }
        public decimal? SGSTPercentage { get; set; }
        public Int16? IGSTPercentage { get; set; }

        public decimal? CESSPercentage { get; set; }
        public decimal? AdditionalCESSPercentage { get; set; }

    }




}

