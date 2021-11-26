
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;

namespace WYNK.Data.Repository.Implementation
{
    class DonorRepository : RepositoryBase<DonorViewModel>, IDonorRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public DonorRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic SubmitDonor(DonorViewModel DonorDetails)
        {
            try
            {          
                var Donor = new Donor();
                Donor.CmpID = DonorDetails.Donor.CmpID;
                Donor.Name = DonorDetails.Donor.Name;
                Donor.DonorType = DonorDetails.Donor.DonorType;
                Donor.MiddleName = DonorDetails.Donor.MiddleName;
                Donor.LastName = DonorDetails.Donor.LastName;
                Donor.Address1 = DonorDetails.Donor.Address1;
                Donor.Address2 = DonorDetails.Donor.Address2;
                Donor.Address3 = DonorDetails.Donor.Address3;
                Donor.Location = DonorDetails.Donor.Location;
                Donor.GSTNo = DonorDetails.Donor.GSTNo;
                Donor.MobileNo = DonorDetails.Donor.MobileNo;
                Donor.PhoneNo = DonorDetails.Donor.PhoneNo;
                Donor.ContactPerson = DonorDetails.Donor.ContactPerson;
                Donor.IsDeleted = false;
                Donor.CreatedBy = DonorDetails.Donor.CreatedBy;
                Donor.CreatedUTC = DateTime.UtcNow;
                WYNKContext.Donor.Add(Donor);



                string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == DonorDetails.Donor.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                string userid = Convert.ToString(DonorDetails.Donor.CreatedBy);
                ErrorLog oErrorLogs = new ErrorLog();
                oErrorLogs.WriteErrorLogTitle(DonorDetails.Companyname, "Donor", "User name :", username, "User ID :", userid, "Mode : Submit");

                object names = Donor;
                oErrorLogs.WriteErrorLogArray("Donor", names);


                WYNKContext.SaveChanges();

                if (WYNKContext.SaveChanges() >= 0)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    return new
                    {
                        Success = true,
                        Message = "Save successfully"
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorLog oErrorLog = new ErrorLog();
                oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
            }
            return new
            {
                Success = false,
                Message = "Something Went Wrong"
            };
        }

        public dynamic UpdateDonor(DonorViewModel DonorDetails, int ID)
        {

            try
            {
                var Donor = WYNKContext.Donor.Where(x => x.ID == ID).FirstOrDefault();
                Donor.Name = DonorDetails.Donor.Name;
                Donor.DonorType = DonorDetails.Donor.DonorType;
                Donor.MiddleName = DonorDetails.Donor.MiddleName;
                Donor.LastName = DonorDetails.Donor.LastName;
                Donor.Address1 = DonorDetails.Donor.Address1;
                Donor.Address2 = DonorDetails.Donor.Address2;
                Donor.Address3 = DonorDetails.Donor.Address3;
                Donor.Location = DonorDetails.Donor.Location;
                Donor.GSTNo = DonorDetails.Donor.GSTNo;
                Donor.MobileNo = DonorDetails.Donor.MobileNo;
                Donor.PhoneNo = DonorDetails.Donor.PhoneNo;
                Donor.ContactPerson = DonorDetails.Donor.ContactPerson;
                Donor.IsDeleted = false;
                Donor.UpdatedBy = DonorDetails.Donor.UpdatedBy;
                Donor.UpdatedUTC = DateTime.UtcNow;
                WYNKContext.Donor.Update(Donor);


               
                string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == DonorDetails.Donor.UpdatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                string userid = Convert.ToString(DonorDetails.Donor.UpdatedBy);
                ErrorLog oErrorLogs = new ErrorLog();
                oErrorLogs.WriteErrorLogTitle(DonorDetails.Companyname, "Donor", "User name :", username, "User ID :", userid, "Mode : Update");
                object names = Donor;
                oErrorLogs.WriteErrorLogArray("Donor", names);


                WYNKContext.SaveChanges();

                if (WYNKContext.SaveChanges() >= 0)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Information :", "Update Successfully");
                    return new
                    {
                        Success = true,
                        Message = "Save successfully"
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorLog oErrorLog = new ErrorLog();
                oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());

            }
            return new
            {
                Success = false,
                Message = "Something Went Wrong"
            };

        }

        public dynamic DeleteDonor(int ID, int CMPID, int USERID , string Companyname)
        {
            try
            {
                var Donor = WYNKContext.Donor.Where(x => x.ID == ID).FirstOrDefault();
                if (Donor != null)
                {
                    Donor.IsDeleted = true;
                    WYNKContext.Donor.Update(Donor);

                   
                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == USERID).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    string userid = Convert.ToString(USERID);
                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(Companyname, "Donor", "User name :", username, "User ID :", userid, "Mode : Delete");
                    object names = Donor;
                    oErrorLogs.WriteErrorLogArray("Donor", names);

                    WYNKContext.SaveChanges();

                }
                if (WYNKContext.SaveChanges() >= 0)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Information :", "Delete Successfully");
                    return new
                    {
                        Success = true,
                        Message = "Save successfully"
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorLog oErrorLog = new ErrorLog();
                oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
            }
            return new
            {
                Success = false,
                Message = "Something Went Wrong"
            };

        }


    }
}













