using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{ 
    public class InsuranceVsMiddlemanViewModel
    {
        public Insurance Insurance { get; set; }
        public InsuranceVsMiddlemen InsuranceVsMiddleman { get; set; }
        public ICollection<GetMiddleman> GetMiddleman { get; set; }
        public ICollection<GetInsurance> GetInsurance { get; set; }
        public ICollection<InsertIvsM> InsertIvsM { get; set; }
        public ICollection<GetINvsMiddleman> GetINvsMiddleman { get; set; }
        public ICollection<SampleGetINvsMiddleman> SampleGetINvsMiddleman { get; set; }
        public ICollection<SampleGetINvsMiddlemantotal> SampleGetINvsMiddlemantotal { get; set; }
        public ICollection<OriginalSampleGetINvsMiddlemantotal> OriginalSampleGetINvsMiddlemantotal { get; set; }


        public ICollection<UpdateIvsM> UpdateIvsM { get; set; }
        public ICollection<temp1> temp1 { get; set; }
        

    }

    public class temp1
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int ID { get; set; }
        public int IvsMID { get; set; }
        public bool checkeda { get; set; }
        public bool CB { get; set; }

    }
        public class UpdateIvsM
    {

        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int IvsMID { get; set; }


    }

    public class SampleGetINvsMiddleman
    {
        
        public int ID { get; set; }
        

    }


    public class SampleGetINvsMiddlemantotal
    {
        public string Name { get; set; }
        public decimal amount { get; set; }
        public int ID { get; set; }
        public int IvsMID { get; set; }
        public bool status { get; set; }


    }


    public class OriginalSampleGetINvsMiddlemantotal
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int ID { get; set; }
        public int IvsMID { get; set; }
        public bool checkeda { get; set; }
        public bool CB { get; set; }
    }


    public class GetINvsMiddleman
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public bool CB { get; set; }

    }
    public class InsertIvsM
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
      
    }

    public class GetInsurance
    {
        
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public bool CB { get; set; }
        
    }

        public class GetMiddleman
        {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string City { get; set; }
        public int Pincode { get; set; }
        public string InsuranceCategory { get; set; }
        public bool IsActive { get; set; }

        }



}
