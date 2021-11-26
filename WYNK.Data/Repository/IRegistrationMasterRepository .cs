using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IRegistrationMasterRepository : IRepositoryBase<RegistrationMasterViewModel>
    {
        dynamic InsertPatientAssign(RegistrationMasterViewModel PatientAssign, int userID);
        dynamic UpdateConBilling(RegistrationMasterViewModel ConBilling, int cmpPid, int DRID , int regtranid , int TransactionTypeid);
        dynamic InsertregMas(RegistrationMasterI RegistrationMaster, int userid, int TransactionTypeid, int ContraTransactionid);
        dynamic CancelPatientAssign(RegistrationMasterViewModel PatientAssign);
        dynamic UpdateregMas(RegistrationMasterI RegistrationMaster, string UIN, int userid);
        dynamic DeleteRegMas(string UIN);
        dynamic Getprint(int CMPID, string UIN, string Phase);
        dynamic Getconsultationbill(int CMPID, string UIN);
        dynamic Getreviewpatient( int CmpID, string PhoneNO, string Gender);
        dynamic GetpatientVoiceconsentdata(Consentdata Consentdata);
        dynamic AgeOrDboChange(string type, string value, string M_MMDD, int AgeN);
        dynamic InsertReview(RegistrationMasterI RegistrationMaster, string UIN, int Cmpid,int ContraTransactionid,int userid,decimal ConsultationFee);
        dynamic InsertReviewfromdashboard(RegistrationMasterI RegistrationMaster, string UIN, int Cmpid, int ContraTransactionid, int userid, decimal ConsultationFee);
        dynamic UpdateConsultationamount(COnsultationview Reaster, string UIN, int Cmpid, int ContraTransactionid, int userid, decimal ConsultationFee, int regtranid);
        RegistrationMasterViewModel GetPatientDetails(string ViewsStatusid, string RoleDescription, int userDoctorIDs, int CompanyID);
        RegistrationMasterViewModel GetSurgeryRevPatientDetails(int ViewsStatusid, string RoleDescription, int userDoctorIDs, int CompanyID);
        RegistrationMasterViewModel GetselectedPatientDetails(string ViewsStatusid, string RoleDescription, int userDoctorIDs, DateTime todaydates, int CompanyID);
        RegistrationMasterViewModel GetselectedsearchPatientDetails(string ViewsStatusid, string RoleDescription, int userDoctorIDs, string todaydates, int CompanyID);
        RegistrationMasterViewModel GetselectedsearchReviewPatientDetails(int ViewsStatusid, string RoleDescription, int userDoctorIDs, string todaydates, int CompanyID);
        RegistrationMasterViewModel GetfromtoPatientDetails(string ViewsStatusid, string RoleDescription, int userDoctorIDs, DateTime fromdate, DateTime todate, int Companyid);
        RegistrationMasterViewModel GetRevPatientDetails(int ViewsStatusid, string RoleDescription, int userDoctorIDs, int CompanyID);
        RegistrationMasterViewModel GetfromtoRevPatientDetails(int ViewsStatusid, string RoleDescription, int userDoctorIDs, DateTime fromdate, DateTime todate, int CompanyID);
        RegistrationMasterViewModel GetselectedRevPatientDetails(int ViewsStatusid, string RoleDescription, int userDoctorIDs, DateTime todaydates, int CompanyID);
        dynamic GetUsernamepassword(string username, string password);
        dynamic DeleteToken(string username);
        dynamic DeleteTokenbasaedonlink(string username);
        dynamic DeleteTokenbasaedonID(int username);
        

        RegistrationMasterViewModel Resetpassword(string emailid);
        RegistrationMasterViewModel Getusersaccess(int userid, string userroleid,int CID);
        RegistrationMasterViewModel GeMenuaccess(int Roletableid, int userroleID, string id);
        RegistrationMasterI getBusinessRule(int CmpID, string Activedata, string RegNO, int TransactiontypeID, DateTime RGDate);
        RegistrationMasterI GetDoctorConFee(int DoctorID, int userID, int CampID);
        
        dynamic GetMyPatientDetails(int CmpID);
        dynamic Getconfirmstatus(int CmpID);

        RegistrationMasterI getpayment(string UIN, string REPNo, int Cmpid);
        dynamic GetAppointmentDetails(int CmpID,string Userid);

        dynamic GetAppointmentPatientDetails(int CmpID, string ID);

        dynamic InsertAppointmentmembvers(string CMPID, RegistrationMasterViewModel APpointdata);
        dynamic InsertAppointmentCancelledbymembvers(string CMPID, RegistrationMasterViewModel APpointdata);
        
        dynamic GetAppointmentstatusPatientDetails(string CmpID, string Status, string RegNO);
        RegistrationMasterI GetRegistrationExtension(string RegNO, int CmpID);
        RegistrationMasterI GetServiceamount(int CmpID, int docid);
        RegistrationMasterI GetlocationDetails(int id);
        RegistrationMasterI GetPinCodeDetails(int location);
        RegistrationMasterI Getkindetail(string UIN);
        RegistrationMasterI Deletekin(int cmpid, string UIN, string Relationship);
        RegistrationMasterI ForeignNational();

        

        dynamic Insertnewpass(RegistrationMasterViewModel Insertnewpasss, int UIN);
        dynamic UploadImage(IFormFile file, string UIN);

        dynamic Uploadpatientvoice(IFormFile file, string UIN);
        dynamic UploadpatientVideo(IFormFile file, string UIN);

        
        dynamic Getpatientimage(string UIN);
        dynamic Getpatientvoice(string UIN);
        dynamic Getpatientvideo(string UIN);
        dynamic GetThumbimage(string UIN, int CMPID);
        dynamic GetReviewThumbimage(string UIN, int CMPID);
        dynamic GetSurgeryreviewThumbimage(string UIN, int CMPID);
        dynamic Deletedatabasedondate(int CMPID, int id, string userid);
        dynamic Getpaymentdetailsbasedontrainid(int CMPID, int Regtranid, string uin);
        dynamic SendOTP(int OTP,string Mobilnumber);
        dynamic Deleteselectedimage(string UIN, int CMPID, string Imagename, string status);
        dynamic CheckPatientAvailabliity(int Regtranid, int CMPID);
    }
}
