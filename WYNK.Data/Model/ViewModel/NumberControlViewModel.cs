
using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class NumberControlViewModel
    {

        public Number_Control NumberControl { get; set; }
        
        public int TransactionID { get; set; }
        public ICollection<NumColgrid> NumColgrid { get; set; }
    }
}
public class NumColgrid
{
    public int VCID { get; set; }
    public int TransactionID { get; set; }
    public int DepartmentID { get; set; }
    public int CmpID { get; set; }
    public int Location { get; set; }
    public string Prefix { get; set; }
    public string Suffix { get; set; }
    public string Description { get; set; }
    public string Autonumber { get; set; }
    public Int64 RunningNumber { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public string IsActive { get; set; }
    public string IsDeleted { get; set; }
    public DateTime CreatedUTC { get; set; }
    public DateTime? UpdatedUTC { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }


}