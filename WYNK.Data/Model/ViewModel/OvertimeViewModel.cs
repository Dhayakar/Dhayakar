using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{

    public  class OvertimeViewM
    {
        public OverTime OverTime { get; set; }
        public ICollection<getEmployeeDet> getEmployeeDet { get; set; }
        public ICollection<OvertimeDet> OvertimeDet { get; set; }
        public ICollection<getAddHistoryDet> getAddHistoryDet { get; set; }
        public ICollection<getHistoryDet> getHistoryDet { get; set; }
    }

    public class getEmployeeDet
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Designation { get; set; }

        public DateTime DOJ { get; set; }
        public string Gender { get; set; }

        public int Emp_ID { get; set; }

        public string PresentAddress { get; set; }

    }



    public class OvertimeDet
    {
        public int EmpID { get; set; }
        public DateTime OTDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public decimal NoofHours { get; set; }
        public decimal OTperHour { get; set; }
        public decimal TotalOTAmount { get; set; }

    }

    public class getAddHistoryDet
    {
        public int EmpID { get; set; }
        public DateTime OTDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public decimal NoofHours { get; set; }
        public decimal OTperHour { get; set; }
        public decimal? TotalOTAmount { get; set; }

    }

    public class getHistoryDet
    {
        public Int16 ID1 { get; set; }
        public int EmpID1 { get; set; }
        public DateTime OTDate1 { get; set; }
        public string FromTime1 { get; set; }
        public string ToTime1 { get; set; }
        public decimal NoofHours1 { get; set; }
        public decimal OTperHour1 { get; set; }
        public decimal? TotalOTAmount1 { get; set; }

    }



}
