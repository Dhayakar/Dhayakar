using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class TaxMasterViewM
    {
        public TaxMaster taxMaster { get; set; }
        public ICollection<getTaxDet> getTaxDet { get; set; }

    }

    public class getTaxDet
    {
        public int ID { get; set; }
        public string TaxDescription { get; set; }
        public Int16? GSTPercentage { get; set; }
        public decimal? CGSTPercentage { get; set; }
        public decimal? SGSTPercentage { get; set; }
        public Int16? IGSTPercentage { get; set; }
        public string TaxGroupId { get; set; }
        public bool IsActive { get; set; }
    }






}
