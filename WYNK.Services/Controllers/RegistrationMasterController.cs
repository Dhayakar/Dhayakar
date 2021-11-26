using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using WYNK.Data.Repository;
using WYNK.Data.Common;
using WYNK.Data.Model.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class RegistrationMasterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public RegistrationMasterController(IRepositoryWrapper repoWrapper, IConfiguration config)
        {
            _repoWrapper = repoWrapper;

        }


        [HttpGet("GetMyPatientDetails/{CmpID}")]
        public dynamic GetMyPatientDetails(int CmpID)

        {
            return _repoWrapper.RegistrationMaster.GetMyPatientDetails(CmpID);
        }


        [HttpGet("Getconfirmstatus/{CmpID}")]
        public dynamic Getconfirmstatus(int CmpID)

        {
            return _repoWrapper.RegistrationMaster.Getconfirmstatus(CmpID);
        }


        [HttpGet("getpayment/{UIN}/{REPNo}/{Cmpid}")]
        public RegistrationMasterI getpayment(string UIN,string REPNo, int Cmpid)
        {
            return _repoWrapper.RegistrationMaster.getpayment(UIN, REPNo,Cmpid);
        }


        [HttpGet("GetAppointmentDetails/{CmpID}/{Userid}")]
        public dynamic GetAppointmentDetails(int CmpID,string Userid)

        {
            return _repoWrapper.RegistrationMaster.GetAppointmentDetails(CmpID, Userid);
        }
        [HttpGet("GetAppointmentPatientDetails/{CmpID}/{ID}")]
        public dynamic GetAppointmentPatientDetails(int CmpID,string ID)

        {
            return _repoWrapper.RegistrationMaster.GetAppointmentPatientDetails(CmpID,ID);
        }

        //InsertAppointmentmembvers


        [HttpPost("InsertAppointmentmembvers/{CMPID}")]
        public dynamic InsertAppointmentmembvers(string CMPID,[FromBody] RegistrationMasterViewModel APpointdata)
        {
            return _repoWrapper.RegistrationMaster.InsertAppointmentmembvers(CMPID, APpointdata);
        }


        [HttpPost("InsertAppointmentCancelledbymembvers/{CMPID}")]
        public dynamic InsertAppointmentCancelledbymembvers(string CMPID, [FromBody] RegistrationMasterViewModel APpointdata)
        {
            return _repoWrapper.RegistrationMaster.InsertAppointmentCancelledbymembvers(CMPID, APpointdata);
        }

        



        [HttpGet("GetAppointmentstatusPatientDetails/{CmpID}/{Status}/{RegNO}")]
        public dynamic GetAppointmentstatusPatientDetails(string CmpID , string Status, string RegNO)

        {
            return _repoWrapper.RegistrationMaster.GetAppointmentstatusPatientDetails(CmpID, Status, RegNO);
        }

        [HttpGet("GetRegistrationExtension/{RegNO}/{CmpID}")]
        public RegistrationMasterI GetRegistrationExtension( string RegNO, int CmpID)

        {
            return _repoWrapper.RegistrationMaster.GetRegistrationExtension( RegNO, CmpID);
        }

        [HttpGet("GetServiceamount/{CmpID}/{docid}")]
        public RegistrationMasterI GetServiceamount(int CmpID, int docid)

        {
            return _repoWrapper.RegistrationMaster.GetServiceamount(CmpID, docid);
        }

        [HttpGet("getBusinessRule/{CmpID}/{Activedata}/{RegNO}/{TransactiontypeID}/{RGDate}")]
        public RegistrationMasterI getBusinessRule(int CmpID, string Activedata, string RegNO, int TransactiontypeID, DateTime RGDate)

        {
            return _repoWrapper.RegistrationMaster.getBusinessRule(CmpID, Activedata, RegNO, TransactiontypeID, RGDate);
        }

        [HttpGet("GetDoctorConFee/{DoctorID}/{userID}/{CampID}")]
        public RegistrationMasterI GetDoctorConFee(int DoctorID,int userID,int CampID)

        {
            return _repoWrapper.RegistrationMaster.GetDoctorConFee(DoctorID, userID, CampID);
        }


        [HttpGet("DeleteToken/{username}")]
        public dynamic DeleteToken(string username)
        {

            return _repoWrapper.RegistrationMaster.DeleteToken(username);
        }
        [HttpGet("DeleteTokenbasaedonlink/{username}")]
        public dynamic DeleteTokenbasaedonlink(string username)
        {

            return _repoWrapper.RegistrationMaster.DeleteTokenbasaedonlink(username);
        }

        [HttpGet("DeleteTokenbasaedonID/{username}")]
        public dynamic DeleteTokenbasaedonID(int username)
        {

            return _repoWrapper.RegistrationMaster.DeleteTokenbasaedonID(username);
        }

        

        [HttpGet("GetUsernamepassword/{username}/{password}")]
        public dynamic GetUsernamepassword(string username, string password)
        {
            return _repoWrapper.RegistrationMaster.GetUsernamepassword(username, password);
        }

        [HttpGet("GetlocationDetails/{id}")]
        public RegistrationMasterI GetlocationDetails(int id)
        {
            Console.WriteLine("Controller Method Started");
            return _repoWrapper.RegistrationMaster.GetlocationDetails(id);
        }
        [HttpGet("GetPinCodeDetails/{location}")]
        public RegistrationMasterI GetPinCodeDetails(int location)

        {
            return _repoWrapper.RegistrationMaster.GetPinCodeDetails(location);
        }
        

        [HttpGet("GeMenuaccess/{Roletableid}/{userroleID}/{id}")]
        public RegistrationMasterViewModel GeMenuaccess(int Roletableid, int userroleID, string id)
        {
            return _repoWrapper.RegistrationMaster.GeMenuaccess(Roletableid, userroleID, id);
        }

        [HttpGet("Getkindetail/{UIN}")]
        public RegistrationMasterI Getkindetail(string UIN)
        {
            return _repoWrapper.RegistrationMaster.Getkindetail(UIN);
        }

        [HttpGet("SendOTP/{OTP}/{Mobilnumber}")]
        public dynamic SendOTP(int OTP, string Mobilnumber)
        {
            return _repoWrapper.RegistrationMaster.SendOTP(OTP, Mobilnumber);
        }

       // GetENterOTP


        [HttpGet("Deletekin/{CompanyID}/{UIN}/{Relationship}")]
        public RegistrationMasterI Deletekin(int CompanyID, string UIN, string Relationship)
        {
            return _repoWrapper.RegistrationMaster.Deletekin(CompanyID, UIN, Relationship);
        }
        [HttpGet("ForeignNational")]
        public RegistrationMasterI ForeignNational()
        {
            return _repoWrapper.RegistrationMaster.ForeignNational();
        }
        [HttpPost("InsertPatientAssign/{userID}")]
        public dynamic InsertPatientAssign([FromBody] RegistrationMasterViewModel InsertPatientAssign, int userID)
        {
            return _repoWrapper.RegistrationMaster.InsertPatientAssign(InsertPatientAssign, userID);
        }



        [HttpPost("CancelPatientAssign")]
        public dynamic CancelPatientAssign([FromBody] RegistrationMasterViewModel CancelPatientAssign)
        {
            return _repoWrapper.RegistrationMaster.CancelPatientAssign(CancelPatientAssign);
        }

        [HttpGet("ageOrDboChange/{type}/{value}/{M_MMDD}/{AgeN}")]
        public dynamic AgeOrDboChange(string type, string value, string M_MMDD, int AgeN)
        {
            return _repoWrapper.RegistrationMaster.AgeOrDboChange(type, value, M_MMDD, AgeN);
        }
        [HttpGet("Getreviewpatient/{CmpID}/{PhoneNO}/{Gender}")]
        public dynamic Getreviewpatient(int CmpID,string PhoneNO, string Gender)
        {
            return _repoWrapper.RegistrationMaster.Getreviewpatient(CmpID, PhoneNO, Gender);
        }
        

        [HttpPost("InsertregMas/{CompanyID}/{userid}/{TransactionTypeid}/{ContraTransactionid}")]
        public dynamic InsertregMas([FromBody] RegistrationMasterI RegistrationMaster, int CompanyID, int userid, int TransactionTypeid,int ContraTransactionid)
        {
            string generatenumber = _repoWrapper.Common.GenerateRunningCtrlNoo(TransactionTypeid, CompanyID,"GetRunningNo");
            string generatenumber1 = _repoWrapper.Common.GenerateRunningCtrlNoo(ContraTransactionid, CompanyID, "GetRunningNo");

            if (TransactionTypeid == 0)
            {
                var Errordata = _repoWrapper.Common.ErrorList("TransactionTypeid=null", "Registration", CompanyID, userid);

                return new
                {
                    Success = false,
                    Message = "TransactionTypeid Does'nt Exist"
                };
            }


            if (generatenumber == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }
            else if (generatenumber1 == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Receipt Number Does'nt Exist"
                };
            }
            RegistrationMaster.RegistrationMaster.UIN = generatenumber;
            RegistrationMaster.ContraTransactionid =generatenumber1;
            return _repoWrapper.RegistrationMaster.InsertregMas(RegistrationMaster, userid, TransactionTypeid , ContraTransactionid);
        }

        [HttpPost("Insertnewpass/{UIN}")]
        public dynamic Insertnewpass([FromBody] RegistrationMasterViewModel Insertnewpasss, int UIN)
        {
            return _repoWrapper.RegistrationMaster.Insertnewpass(Insertnewpasss, UIN);
        }



        [HttpPost("InsertReview/{UIN}/{Cmpid}/{ContraTransactionid}/{userid}/{ConsultationFee}")]
        public dynamic InsertReview([FromBody] RegistrationMasterI RegistrationMaster, string UIN, int Cmpid,int ContraTransactionid,int userid,decimal ConsultationFee)
        {


            if (ContraTransactionid == 0)
            {
                var Errordata = _repoWrapper.Common.ErrorList("TransactionTypeid=null", "Registration", Cmpid, userid);

                return new
                {
                    Success = false,
                    Message = "TransactionTypeid Does'nt Exist"
                };
            }

            if (ConsultationFee != 0 && ConsultationFee != null) {
                string generatenumber1 = _repoWrapper.Common.GenerateRunningCtrlNoo(ContraTransactionid, Cmpid, "GetRunningNo");
                if (generatenumber1 == "Running Number Does'nt Exist")
                {
                    return new
                    {
                        Success = false,
                        Message = "Running Number Does'nt Exist"
                    };
                }

                RegistrationMaster.ContraTransactionid = generatenumber1;
            }
            return _repoWrapper.RegistrationMaster.InsertReview(RegistrationMaster, UIN, Cmpid, ContraTransactionid, userid,  ConsultationFee);
        }

        [HttpPost("UpdateConsultationamount/{UIN}/{Cmpid}/{ContraTransactionid}/{userid}/{ConsultationFee}/{regtranid}")]
        public dynamic UpdateConsultationamount([FromBody] COnsultationview Reaster, string UIN, int Cmpid, int ContraTransactionid, int userid, decimal ConsultationFee,int regtranid)
        {
            return _repoWrapper.RegistrationMaster.UpdateConsultationamount(Reaster, UIN, Cmpid, ContraTransactionid, userid, ConsultationFee, regtranid);
        }

        [HttpPost("InsertReviewfromdashboard/{UIN}/{Cmpid}/{ContraTransactionid}/{userid}/{ConsultationFee}")]
        public dynamic InsertReviewfromdashboard([FromBody] RegistrationMasterI RegistrationMaster, string UIN, int Cmpid, int ContraTransactionid, int userid, decimal ConsultationFee)
        {


            if (ContraTransactionid == 0)
            {
                var Errordata = _repoWrapper.Common.ErrorList("TransactionTypeid=null", "Registration", Cmpid, userid);

                return new
                {
                    Success = false,
                    Message = "TransactionTypeid Does'nt Exist"
                };
            }

            if (ConsultationFee != 0 && ConsultationFee != null)
            {
                string generatenumber1 = _repoWrapper.Common.GenerateRunningCtrlNoo(ContraTransactionid, Cmpid, "GetRunningNo");
                if (generatenumber1 == "Running Number Does'nt Exist")
                {
                    return new
                    {
                        Success = false,
                        Message = "Running Number Does'nt Exist"
                    };
                }

                RegistrationMaster.ContraTransactionid = generatenumber1;
            }
            return _repoWrapper.RegistrationMaster.InsertReviewfromdashboard(RegistrationMaster, UIN, Cmpid, ContraTransactionid, userid, ConsultationFee);
        }

        [HttpPost("UpdateregMas/{UIN}/{userid}")]
        public dynamic UpDateCmpMas([FromBody] RegistrationMasterI RegistrationMaster, string UIN, int userid)
        {
            return _repoWrapper.RegistrationMaster.UpdateregMas(RegistrationMaster, UIN, userid);
        }




        [HttpPost("GetpatientVoiceconsentdata")]
        public dynamic GetpatientVoiceconsentdata([FromBody] Consentdata Consentdata)
        {
            return _repoWrapper.RegistrationMaster.GetpatientVoiceconsentdata(Consentdata);
        }


        


        [HttpDelete("DeleteregMas/{UIN}")]
        public dynamic DeleteRegMas(string UIN)
        {
            return _repoWrapper.RegistrationMaster.DeleteRegMas(UIN);
        }

        [HttpGet("GetPatientDetails/{ViewsStatusid}/{RoleDescription}/{userDoctorIDs}/{CompanyID}")]
        public RegistrationMasterViewModel GetPatientDetails(string ViewsStatusid, string RoleDescription, int userDoctorIDs, int CompanyID)
        {
            return _repoWrapper.RegistrationMaster.GetPatientDetails(ViewsStatusid, RoleDescription, userDoctorIDs, CompanyID);
        }


        [HttpGet("GetselectedPatientDetails/{ViewsStatusid}/{RoleDescription}/{userDoctorIDs}/{todaydates}/{CompanyID}")]
        public RegistrationMasterViewModel GetselectedPatientDetails(string ViewsStatusid, string RoleDescription, int userDoctorIDs, DateTime todaydates, int CompanyID)
        {
            return _repoWrapper.RegistrationMaster.GetselectedPatientDetails(ViewsStatusid, RoleDescription, userDoctorIDs, todaydates, CompanyID);
        }


        [HttpGet("GetselectedsearchPatientDetails/{ViewsStatusid}/{RoleDescription}/{userDoctorIDs}/{todaydates}/{CompanyID}")]
        public RegistrationMasterViewModel GetselectedsearchPatientDetails(string ViewsStatusid, string RoleDescription, int userDoctorIDs, string todaydates, int CompanyID)
        {
            return _repoWrapper.RegistrationMaster.GetselectedsearchPatientDetails(ViewsStatusid, RoleDescription, userDoctorIDs, todaydates, CompanyID);
        }


        [HttpGet("GetselectedsearchReviewPatientDetails/{ViewsStatusid}/{RoleDescription}/{userDoctorIDs}/{todaydates}/{CompanyID}")]
        public RegistrationMasterViewModel GetselectedsearchReviewPatientDetails(int ViewsStatusid, string RoleDescription, int userDoctorIDs, string todaydates, int CompanyID)
        {
            return _repoWrapper.RegistrationMaster.GetselectedsearchReviewPatientDetails(ViewsStatusid, RoleDescription, userDoctorIDs, todaydates, CompanyID);
        }

        [HttpGet("GetfromtoPatientDetails/{ViewsStatusid}/{RoleDescription}/{userDoctorIDs}/{fromdate}/{todate}/{Companyid}")]
        public RegistrationMasterViewModel GetfromtoPatientDetails(string ViewsStatusid, string RoleDescription, int userDoctorIDs, DateTime fromdate, DateTime todate, int Companyid)
        {
            return _repoWrapper.RegistrationMaster.GetfromtoPatientDetails(ViewsStatusid, RoleDescription, userDoctorIDs, fromdate, todate, Companyid);
        }





        [HttpGet("GetRevPatientDetails/{ViewsStatusid}/{RoleDescription}/{userDoctorIDs}/{CompanyID}")]
        public RegistrationMasterViewModel GetRevPatientDetails(int ViewsStatusid, string RoleDescription, int userDoctorIDs, int CompanyID)
        {
            return _repoWrapper.RegistrationMaster.GetRevPatientDetails(ViewsStatusid, RoleDescription, userDoctorIDs, CompanyID);
        }






        [HttpGet("GetselectedRevPatientDetails/{ViewsStatusid}/{RoleDescription}/{userDoctorIDs}/{todaydates}/{CompanyID}")]
        public RegistrationMasterViewModel GetselectedRevPatientDetails(int ViewsStatusid,
            string RoleDescription, int userDoctorIDs, DateTime todaydates, int CompanyID)
        {
            return _repoWrapper.RegistrationMaster.GetselectedRevPatientDetails(ViewsStatusid, RoleDescription, userDoctorIDs, todaydates, CompanyID);
        }



        [HttpGet("GetfromtoRevPatientDetails/{ViewsStatusid}/{RoleDescription}/{userDoctorIDs}/{fromdate}/{todate}/{CompanyID}")]
        public RegistrationMasterViewModel GetfromtoRevPatientDetails(int ViewsStatusid,
            string RoleDescription, int userDoctorIDs, DateTime fromdate, DateTime todate, int CompanyID)
        {
            return _repoWrapper.RegistrationMaster.GetfromtoRevPatientDetails(ViewsStatusid, RoleDescription, userDoctorIDs, fromdate, todate, CompanyID);
        }



        [HttpPost("uploadImage/{UIN}")]
        public bool UploadImage(string UIN)
        {
            var file = Request.Form.Files[0];
            return _repoWrapper.RegistrationMaster.UploadImage(file, UIN);
        }


        [HttpPost("Uploadpatientvoice/{UIN}")]
        public bool Uploadpatientvoice(string UIN)
        {
            var file = Request.Form.Files[0];
            return _repoWrapper.RegistrationMaster.Uploadpatientvoice(file, UIN);
        }


        [HttpPost("UploadpatientVideo/{UIN}")]
        public bool UploadpatientVideo(string UIN)
        {
            var file = Request.Form.Files[0];
            return _repoWrapper.RegistrationMaster.UploadpatientVideo(file, UIN);
        }


        

        [HttpGet("Getpatientimage/{UIN}")]
        public dynamic Getpatientimage(string UIN)
        {
            return _repoWrapper.RegistrationMaster.Getpatientimage(UIN);
        }

        [HttpGet("Getpatientvoice/{UIN}")]
        public dynamic Getpatientvoice(string UIN)
        {
            return _repoWrapper.RegistrationMaster.Getpatientvoice(UIN);
        }
        [HttpGet("Getpatientvideo/{UIN}")]
        public dynamic Getpatientvideo(string UIN)
        {
            return _repoWrapper.RegistrationMaster.Getpatientvideo(UIN);
        }



        [HttpGet("GetThumbimage/{UIN}/{CMPID}")]
        public dynamic GetThumbimage(string UIN, int CMPID)
        {
            return _repoWrapper.RegistrationMaster.GetThumbimage(UIN, CMPID);
        }

        [HttpGet("GetReviewThumbimage/{UIN}/{CMPID}")]
        public dynamic GetReviewThumbimage(string UIN, int CMPID)
        {
            return _repoWrapper.RegistrationMaster.GetReviewThumbimage(UIN, CMPID);
        }

        [HttpGet("GetSurgeryreviewThumbimage/{UIN}/{CMPID}")]
        public dynamic GetSurgeryreviewThumbimage(string UIN, int CMPID)
        {
            return _repoWrapper.RegistrationMaster.GetSurgeryreviewThumbimage(UIN, CMPID);
        }


        [HttpGet("Resetpassword/{emailid}")]
        public RegistrationMasterViewModel Resetpassword(string emailid)
        {
            return _repoWrapper.RegistrationMaster.Resetpassword(emailid);
        }


        [HttpGet("Getusersaccess/{userid}/{userroleid}/{CID}")]
        public RegistrationMasterViewModel Getusersaccess(int userid, string userroleid,int CID)
        {
            return _repoWrapper.RegistrationMaster.Getusersaccess(userid, userroleid, CID);
        }


        [HttpGet("GetSurgeryRevPatientDetails/{ViewsStatusid}/{RoleDescription}/{userDoctorIDs}/{CompanyID}")]
        public RegistrationMasterViewModel GetSurgeryRevPatientDetails(int ViewsStatusid, string RoleDescription, int userDoctorIDs, int CompanyID)
        {
            return _repoWrapper.RegistrationMaster.GetSurgeryRevPatientDetails(ViewsStatusid, RoleDescription, userDoctorIDs, CompanyID);
        }

        [HttpGet("Getprint/{CMPID}/{UIN}/{Phase}")]
        public dynamic Getprint(int CMPID, string UIN, string Phase)
        {
            return _repoWrapper.RegistrationMaster.Getprint(CMPID, UIN, Phase);
        }
        [HttpGet("Getconsultationbill/{CMPID}/{UIN}")]
        public dynamic Getconsultationbill(int CMPID, string UIN)
        {
            return _repoWrapper.RegistrationMaster.Getconsultationbill( CMPID, UIN);
        }


        [HttpGet("Deleteselectedimage/{UIN}/{CMPID}/{Imagename}/{status}")]
        public dynamic Deleteselectedimage(string UIN, int CMPID, string Imagename, string status)
        {
            return _repoWrapper.RegistrationMaster.Deleteselectedimage(UIN, CMPID, Imagename, status);
        }


        [HttpGet("CheckPatientAvailabliity/{Regtranid}/{CMPID}")]
        public dynamic CheckPatientAvailabliity(int Regtranid, int CMPID)
        { 
            return _repoWrapper.RegistrationMaster.CheckPatientAvailabliity(Regtranid, CMPID);
        }
        [HttpGet("Getpaymentdetailsbasedontrainid/{CMPID}/{Regtranid}/{uin}")]
        public dynamic Getpaymentdetailsbasedontrainid( int CMPID, int Regtranid, string uin)
        {
            return _repoWrapper.RegistrationMaster.Getpaymentdetailsbasedontrainid(CMPID, Regtranid,uin);
        }

        [HttpGet("Deletedatabasedondate/{CMPID}/{id}/{userid}")]
        public dynamic Deletedatabasedondate(int CMPID, int id, string userid)
        {
            return _repoWrapper.RegistrationMaster.Deletedatabasedondate(CMPID, id, userid);
        }

        


        /// //Testing data


        [HttpGet("GetTestdata")]
        public dynamic GetTestdata()
        {
            return "Testingserverdata";
        }



        //////////////////consultation payment////////////////////
       
        [HttpPost("UpdateConBilling/{cmpPid}/{DRID}/{regtranid}/{TransactionTypeid}")]
        public dynamic UpdateConBilling([FromBody] RegistrationMasterViewModel UpdateConBilling, int cmpPid,int DRID,int regtranid, int TransactionTypeid)
        {
            String ib = _repoWrapper.Common.GenerateRunningCtrlNoo(TransactionTypeid, cmpPid, "GetRunningNo");

            if (ib == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }
            UpdateConBilling.InVoiceNumber = ib;
            return _repoWrapper.RegistrationMaster.UpdateConBilling(UpdateConBilling, cmpPid, DRID, regtranid , TransactionTypeid);
        }


    }
}
