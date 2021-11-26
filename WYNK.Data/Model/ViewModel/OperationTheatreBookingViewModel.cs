using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
   public  class OperationTheatreBooking_Viewmodel
    {       
        public ICollection<Operationtheatrebooking> OperationtheatrebookingDetails { get; set; }
        public ICollection<Operationtheatrebooking1> OperationtheatrebookingDetails1 { get; set; }
        public ICollection<registrationDetails> regDetails { get; set; }
        public ICollection<surgeon> surgeonname { get; set; }
        public ICollection<surgery> surgerydetails { get; set; }
        public ICollection<Findingsdetails> Findingsdetailss { get; set; }
        public ICollection<surgeryname> surgerynamedetails { get; set; }
        public ICollection<operationname> operationnamee { get; set; }
        public ICollection<operationname1> operationnamee1 { get; set; }
        public ICollection<operationBookingDet> operationBookingDetail { get; set; }
        public OperationTheatreBooking OperationBookingg { get; set; }
        public BookingHRSDet BookingHRSDet { get; set; }
        public string HRS_FS;
    }

    public class BookingHRSDet
    {

        public string HRValues;

    }


    public class operationBookingDet
    {
        public string uin;
        public DateTime OTReqDate;
        public string FromTime;
        public string ToTime;
        public string SurgeryType;
    }
    public class surgery
    {
        public string surgerytype;
        public string fromtime;
        public string totime;
        public DateTime surgerydate;

    }
    public class surgeon
    {
        public string name;
    }
    public class Operationtheatrebooking
    {
        public string uin;
        public string name;
        public string age;
        public string gender;
    }
    public class Operationtheatrebooking1
    {
        public string uin1;
        public string name1;
        public string age1;
        public string gender1;
    }
    public class registrationDetails
    {
        public string name;
        public string age;
        public DateTime dob;
        public string gender;
        public DateTime surgerydate;
        public string surgerytype;
        public string fromtime;
        public string totime;
        public string uin;


    }
    public class Findingsdetails
    {
        public string name;
        public DateTime date;
    }
    public class surgeryname
    {
        
        public string description;
        public int id;
    }
    public class operationname
    {
        public string operationtheatrename;
        public int ID;
    }
    public class operationname1
    {
        public string operationtheatrename1;
        public int ID1;
    }
}
