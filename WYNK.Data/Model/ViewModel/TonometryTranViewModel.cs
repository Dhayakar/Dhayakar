using System;
using System.Collections.Generic;
using System.Text;


namespace WYNK.Data.Model.ViewModel
{
    public class TonometryTranViewmodel
    {
        public ICollection<TonometryTran> TonometryTran { get; set; }
        public ICollection<tonometrydetails> tonometrydetails { get; set; }

        public ICollection<tonometrydetailss> tonometrydetailss { get; set; }
    }



    public class tonometrydetails
    {
        public int ID { get; set; }
        public int CmpID { get; set; }
        public int RegistrationtranID { get; set; }
        public string UIN { get; set; }
        public string VisitDatetime { get; set; }
        public int TonometryID { get; set; }
        public string Tonometryname { get; set; }
        public string BOD { get; set; }
        public string BOS { get; set; }
        public string AOD { get; set; }
        public string AOS { get; set; }
        public string Dilation { get; set; }
        public string Time { get; set; }
        public int StaffID { get; set; }
        public string Staffname { get; set; }
        public bool IsDeleted { get; set; }
        public string RemovedReasons { get; set; }
        public int RemovedBy { get; set; }
        public string Removedname { get; set; }

        public DateTime? RemovedDatetime { get; set; }
        public string Dropsname { get; set; }

    }

   

    public class tonometrydetailss
    {
        public int ID { get; set; }
        public int CmpID { get; set; }
        public int RegistrationtranID { get; set; }
        public string UIN { get; set; }
        public string VisitDatetime { get; set; }
        public int TonometryID { get; set; }
        public string Tonometryname { get; set; }
        public string BOD { get; set; }
        public string BOS { get; set; }
        public string AOD { get; set; }
        public string AOS { get; set; }
        public string Dilation { get; set; }
        public string Time { get; set; }
        public int StaffID { get; set; }
        public string Staffname { get; set; }
        public bool IsDeleted { get; set; }
        public string RemovedReasons { get; set; }
        public int RemovedBy { get; set; }
        public string Removedname { get; set; }

        public DateTime? RemovedDatetime { get; set; }
        public string Dropsname { get; set; }

    }





}
