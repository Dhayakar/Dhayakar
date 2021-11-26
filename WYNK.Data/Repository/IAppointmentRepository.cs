using Microsoft.AspNetCore.Http;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Data.Repository
{
    public interface IAppointmentRepository : IRepositoryBase<AppointmentView>
    {        
        dynamic InsertData(AppointmentView Appo);
        dynamic Insertpatientappointmentassign(RegistrationMasterViewModel PatientAssign, int userID);
        dynamic uploadImag(IFormFile file, string CompanyID);
        dynamic uploadfileformats(IFormFile file, string phonenumber, string firstname, string lastname, string CMPID);
        dynamic uploadProfilefile(IFormFile file, string Userid, string CMPID);
        dynamic getlistofdays(int CmpID, string date, string HH, string MM, int userid);
        dynamic GetAppointmentDetails(int CmpID, int Userid);
        dynamic GetdoctorImage(int CmpID, string Userdoctorid);
        dynamic getlistofpatients(string Phonenumber);
        dynamic Getdoctorappointments(int CmpID, string Userdoctorid, string date);
        dynamic Getpaymentid(string CmpID, string Phonenumber, string Userdoctorid, int comapnyid, string phone);
        dynamic Bookappointmentforpatients(int CmpID, int Userdoctorid, string date, string panme, string phone, string email, string time, string gender, int age,string Lname, decimal fees);
        dynamic BookappointmentforpatientsInsideappointment(int CmpID, int Userdoctorid, string date, string panme, string phone, string Address, string time, string gender, string age, string apptreason, string lname, decimal fees);
        dynamic validatepaymentid(string Paymentid);
        dynamic getDoctorlistofDays(int CmpID, string Dcotorvalue, string reqdate);
        dynamic Cancelappointment(RegistrationMasterViewModel InsertPatientAssignappreshdules, int userID);
        dynamic GetAppointmentDetailsforreschedules(int CmpID, string Dcotorvalue);
        dynamic Insertpatientappointmentassignreschedulespatients(RegistrationMasterViewModel InsertPatientAssignappreshdules, int userID);
        dynamic GetappointmentPatients(int CmpID);
        dynamic GetappointmentPatientsbasedonRUID(int CmpID, string Userdoctorid ,string date, string time);
        dynamic ReschedulePatient(int CmpID, int doc, string date, string time, decimal? fees, string uin, string catg);
        dynamic CancelledPatient(int CmpID, string uin, string reasons);
    }
}
