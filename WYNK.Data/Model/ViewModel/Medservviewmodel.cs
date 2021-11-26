using System;
using System.Collections.Generic;
using System.Text;
namespace WYNK.Data.Model.ViewModel
{
    public class Medservviewmodel
    {
        public PatientAccount patientaccount { get; set; }
        public OneLine_Masters OneLineMaster { get; set; }
        public PatientAccountDetail PatientAccountDetail { get; set; }
        public ICollection<getDetail> getDetail { get; set; }
        public ICollection<getSummary> getSummary { get; set; }
        public ICollection<getserviceli> getserviceli { get; set; }
        public ICollection<getDetailss> getDetailss { get; set; }
        public ICollection<getsummaryy> getsummaryy { get; set; }


    }
}


public class getDetail
{

    public string services;
    public decimal amount;
    public string servicedes;
    public decimal? totalamount;
}

//getDetail

public class getSummary
{

    public string servicess;
    public decimal amountt;
    public string servicedess;
    public decimal? totalamountt;
}
public class getserviceli
{
    public string servic;
    public string servicedess;
    public decimal totalamountt;
}
public class getDetailss
{
    public string services;
    public decimal amount;
    public string servicedes;
    public decimal? totalamount;
    public DateTime datee;
}
public class getsummaryy
{
    public DateTime date;
    public string servicedess;
    public decimal? totalamountt;
}