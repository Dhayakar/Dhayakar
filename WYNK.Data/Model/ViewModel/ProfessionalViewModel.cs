using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class ProfessionalView
    {
        public ICollection<put> put { get; set; }
        public ICollection<ProfesDetails> ProfesDetails { get; set; }
        public ICollection<getDetails> getDetails { get; set; }
        public ICollection<details> details { get; set; }
        public ProTax ProTax { get; set; }
        public ProTaxTran ProTaxTran { get; set; }

        public ICollection<getdate> getdate { get; set; }

    }

    public class ProfesDetails
    {
        public int ID { get; set; }
        public decimal From { get; set; }
        public decimal To { get; set; }
        public string Frequency { get; set; }
        public string Frequency1 { get; set; }
        public decimal PTAmount { get; set; }
        public int CreatedBy { get; set; }
        public DateTime WithEffectFrom { get; set; }
        public string StateID { get; set; }
        public int StateID1 { get; set; }
        public Boolean IsActive { get; set; }

    }

    public class getDetails
    {
        public int ID1 { get; set; }
        public decimal From1 { get; set; }
        public decimal To1 { get; set; }
        public string Frequency1 { get; set; }
        public string Frequency11 { get; set; }
        public decimal PTAmount1 { get; set; }
        public DateTime WithEffectFrom1 { get; set; }
        public string StateID1 { get; set; }
        public int StateID11 { get; set; }


    }

    public class put
    {
        public DateTime WithEffectFrom { get; set; }
    }

    public class details
    {

        public int ID { get; set; }
        public int PTID { get; set; }
        public decimal From { get; set; }
        public decimal To { get; set; }
        public decimal PTAmount { get; set; }
        public string Frequency { get; set; }
        public string Frequency1 { get; set; }
        public DateTime WithEffectFrom { get; set; }
        public string StateID { get; set; }
        public int StateID1 { get; set; }
        public int IDs { get; set; }


    }
    public class getdate
    {
        public int StateIDS { get; set; }
    }

}
