using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class Financialview
    {
        public Financial Fes { get; set; }
        public FinancialStatus Fts { get; set; }
        public string Tag { get; set; }
        public ICollection<FinancialDetails> FinancialDetails { get; set; }

    }



    public class FinancialDetails
    {

        public int ID { get; set; }
        public string FYDescription { get; set; }
        public int FYID { get; set; }
        // public string Tag { get; set; }
        public DateTime FYFrom { get; set; }
        public DateTime FYTo { get; set; }
        public Int16 FYAccYear { get; set; }
        public DateTime CreatedUTC { get; set; }
        public string IsActive { get; set; }
        public string FYStatus { get; set; }
        public string FYStatus1 { get; set; }
        public DateTime DateTime { get; set; }
        public string ClosedBy { get; set; }
    }



}
