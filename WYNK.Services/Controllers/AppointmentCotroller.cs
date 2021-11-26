
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class AppointmentController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public AppointmentController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpPost("InsertData")]
        public dynamic InsertData([FromBody] AppointmentView Appo)
        {
            return _repoWrapper.AppointMent.InsertData(Appo);
        }


        [HttpPost("uploadImag/{CompanyID}")]
        public bool uploadImag(string CompanyID)
        {
            var file = Request.Form.Files[0];
            return _repoWrapper.AppointMent.uploadImag(file, CompanyID);
        }


        [HttpPost("uploadfileformats/{phonenumber}/{firstname}/{lastname}/{CMPID}")]
        public dynamic uploadfileformats(string phonenumber, string firstname, string lastname, string CMPID)
        {
            var file = Request.Form.Files[0];
            return _repoWrapper.AppointMent.uploadfileformats(file, phonenumber, firstname, lastname, CMPID);
        }



        [HttpPost("uploadProfilefile/{Userid}/{CMPID}")]
        public dynamic uploadProfilefile(string Userid, string CMPID)
        {
            var file = Request.Form.Files[0];
            return _repoWrapper.AppointMent.uploadProfilefile(file, Userid, CMPID);
        }




        [HttpPost("Insertpatientappointmentassign/{userID}")]
        public dynamic Insertpatientappointmentassign([FromBody] RegistrationMasterViewModel InsertPatientAssign, int userID)
        {
            return _repoWrapper.AppointMent.Insertpatientappointmentassign(InsertPatientAssign, userID);
        }


        [HttpGet("getlistofdays/{CmpID}/{date}/{HH}/{MM}/{userid}")]
        public dynamic getlistofdays(int CmpID, string date, string HH, string MM, int userid)

        {
            return _repoWrapper.AppointMent.getlistofdays(CmpID, date, HH, MM, userid);
        }
      

        [HttpGet("GetAppointmentDetails/{CmpID}/{Userid}")]
        public dynamic GetAppointmentDetails(int CmpID, int Userid)

        {
            return _repoWrapper.AppointMent.GetAppointmentDetails(CmpID, Userid);
        }


        [HttpGet("getlistofpatients/{Phonenumber}")]
        public dynamic getlistofpatients(string Phonenumber)

        {
            return _repoWrapper.AppointMent.getlistofpatients(Phonenumber);
        }


        [HttpGet("Getdoctorappointments/{CmpID}/{Userdoctorid}/{date}")]
        public dynamic Getdoctorappointments(int CmpID, string Userdoctorid, string date)

        {
            return _repoWrapper.AppointMent.Getdoctorappointments(CmpID, Userdoctorid, date);
        }


        [HttpGet("Getpaymentid/{CmpID}/{Phonenumber}/{Userdoctorid}/{comapnyid}/{phone}")]
        public dynamic Getpaymentid(string CmpID, string Phonenumber, string Userdoctorid, int comapnyid, string phone)

        {
            return _repoWrapper.AppointMent.Getpaymentid(CmpID, Phonenumber, Userdoctorid, comapnyid, phone);
        }

        [HttpGet("Bookappointmentforpatients/{CmpID}/{Userdoctorid}/{date}/{panme}/{phone}/{email}/{time}/{gender}/{age}/{Lname}/{fees}")]
        public dynamic Bookappointmentforpatients(int CmpID, int Userdoctorid, string date, string panme, string phone, string email, string time, string gender, int age, string Lname, decimal fees)

        {
            return _repoWrapper.AppointMent.Bookappointmentforpatients(CmpID, Userdoctorid, date, panme, phone, email, time, gender, age,Lname,fees);
        }

        [HttpGet("BookappointmentforpatientsInsideappointment/{CmpID}/{Userdoctorid}/{date}/{panme}/{phone}/{Address}/{time}/{gender}/{age}/{apptreason}/{lname}/{fees}")]
        public dynamic BookappointmentforpatientsInsideappointment(int CmpID, int Userdoctorid, string date, string panme,
            string phone, string Address, string time, string gender, string age, string apptreason, string lname, decimal fees)

        {
            return _repoWrapper.AppointMent.BookappointmentforpatientsInsideappointment(CmpID, Userdoctorid, date, panme,
                phone, Address, time, gender, age, apptreason, lname,fees);
        }

        [HttpGet("validatepaymentid/{Paymentid}")]
        public dynamic validatepaymentid(string Paymentid)

        {
            return _repoWrapper.AppointMent.validatepaymentid(Paymentid);
        }


        [HttpGet("GetdoctorImage/{CmpID}/{Userdoctorid}")]
        public dynamic GetdoctorImage(int CmpID, string Userdoctorid)

        {
            return _repoWrapper.AppointMent.GetdoctorImage(CmpID, Userdoctorid);
        }

        [HttpGet("GetappointmentPatientsbasedonRUID/{CmpID}/{Userdoctorid}/{date}/{time}")]
        public dynamic GetappointmentPatientsbasedonRUID(int CmpID, string Userdoctorid, string date, string time)

        {
            return _repoWrapper.AppointMent.GetappointmentPatientsbasedonRUID(CmpID, Userdoctorid,date, time);
        }

        

        [HttpGet("getDoctorlistofDays/{CmpID}/{Dcotorvalue}/{reqdate}")]
        public dynamic getDoctorlistofDays(int CmpID, string Dcotorvalue, string reqdate)

        {
            return _repoWrapper.AppointMent.getDoctorlistofDays(CmpID, Dcotorvalue, reqdate);
        }


        [HttpGet("GetAppointmentDetailsforreschedules/{CmpID}/{Dcotorvalue}")]
        public dynamic GetAppointmentDetailsforreschedules(int CmpID, string Dcotorvalue)

        {
            return _repoWrapper.AppointMent.GetAppointmentDetailsforreschedules(CmpID, Dcotorvalue);
        }

        [HttpPost("Cancelappointment/{userID}")]
        public dynamic Cancelappointment([FromBody] RegistrationMasterViewModel InsertPatientAssignappreshdules, int userID)
        {
            return _repoWrapper.AppointMent.Cancelappointment(InsertPatientAssignappreshdules, userID);
        }

        [HttpPost("Insertpatientappointmentassignreschedulespatients/{userID}")]
        public dynamic Insertpatientappointmentassignreschedulespatients([FromBody] RegistrationMasterViewModel InsertPatientAssignappreshdules, int userID)
        {
            return _repoWrapper.AppointMent.Insertpatientappointmentassignreschedulespatients(InsertPatientAssignappreshdules, userID);
        }

        [HttpGet("GetappointmentPatients/{CmpID}")]
        public dynamic GetappointmentPatients(int CmpID)

        {
            return _repoWrapper.AppointMent.GetappointmentPatients(CmpID);
        }
        [HttpGet("ReschedulePatient/{CmpID}/{doc}/{date}/{time}/{fees}/{uin}/{catg}")]
        public dynamic ReschedulePatient(int CmpID, int doc, string date, string time, decimal? fees, string uin, string catg)
        {
            return _repoWrapper.AppointMent.ReschedulePatient(CmpID,doc,date,time,fees, uin,catg);
        }


        [HttpGet("CancelledPatient/{CmpID}/{uin}/{reasons}")]
        public dynamic CancelledPatient(int CmpID, string uin, string reasons)
        {
            return _repoWrapper.AppointMent.CancelledPatient(CmpID, uin, reasons);
        }

    }
}