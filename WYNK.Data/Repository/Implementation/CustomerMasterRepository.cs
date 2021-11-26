
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;
//check

namespace WYNK.Data.Repository.Implementation
{
    class CustomerMasterRepository : RepositoryBase<CustomerMasterViewModel>, ICustomerMasterRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public CustomerMasterRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic SubmitCustomer(CustomerMasterViewModel CustomerDetails)
        {
            try
            {
                if (CustomerDetails.Customermaster.UIN != null)
                {
                    var SearchUin = WYNKContext.CustomerMaster.Where(x => x.UIN == CustomerDetails.Customermaster.UIN && x.IsDeleted == false).Select(x => x.UIN).FirstOrDefault();
                    if (SearchUin != null)
                    {
                        return new
                        {
                            Success = false,
                            Message = "Already Registered Customer"
                        };
                    }
                }
                var Customer = new CustomerMaster();
                Customer.CmpID = CustomerDetails.Customermaster.CmpID;
                Customer.UIN = CustomerDetails.Customermaster.UIN;
                Customer.Name = CustomerDetails.Customermaster.Name;
                Customer.MiddleName = CustomerDetails.Customermaster.MiddleName;
                Customer.LastName = CustomerDetails.Customermaster.LastName;
                Customer.Address1 = CustomerDetails.Customermaster.Address1;
                Customer.Address2 = CustomerDetails.Customermaster.Address2;
                Customer.Address3 = CustomerDetails.Customermaster.Address3;
                Customer.Location = CustomerDetails.Customermaster.Location;
                Customer.GSTNo = CustomerDetails.Customermaster.GSTNo;
                Customer.MobileNo = CustomerDetails.Customermaster.MobileNo;
                Customer.PhoneNo = CustomerDetails.Customermaster.PhoneNo;
                Customer.ContactPerson = CustomerDetails.Customermaster.ContactPerson;
                Customer.IsDeleted =false;
                Customer.CreatedBy= CustomerDetails.Customermaster.CreatedBy;
                Customer.CreatedUTC = DateTime.UtcNow;
                WYNKContext.CustomerMaster.Add(Customer);


                string cmpname = CMPSContext.Company.Where(x => x.CmpID == CustomerDetails.Customermaster.CmpID).Select(x => x.CompanyName).FirstOrDefault();
                string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == CustomerDetails.Customermaster.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                string userid = Convert.ToString(CustomerDetails.Customermaster.CreatedBy);
                ErrorLog oErrorLogs = new ErrorLog();
                oErrorLogs.WriteErrorLogTitle(cmpname, "CUSTOMER MASTER", "User name :", username, "User ID :", userid, "Mode : Submit");

                object names = Customer;
                oErrorLogs.WriteErrorLogArray("CustomerMaster", names);


                WYNKContext.SaveChanges();

                if(WYNKContext.SaveChanges() >= 0)
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
                oErrorLog.WriteErrorLog("Error :",ex.InnerException.Message.ToString());
            }
            return new
            {
                Success = false,
                Message = "Something Went Wrong"
            };
        }

        public dynamic UpdateCustomerMaster(CustomerMasterViewModel CustomerDetails, int ID)
        {

            try
            {
                var Customer = WYNKContext.CustomerMaster.Where(x=>x.ID == ID).FirstOrDefault();
                Customer.UIN = CustomerDetails.Customermaster.UIN;
                Customer.Name = CustomerDetails.Customermaster.Name;
                Customer.MiddleName = CustomerDetails.Customermaster.MiddleName;
                Customer.LastName = CustomerDetails.Customermaster.LastName;
                Customer.Address1 = CustomerDetails.Customermaster.Address1;
                Customer.Address2 = CustomerDetails.Customermaster.Address2;
                Customer.Address3 = CustomerDetails.Customermaster.Address3;
                Customer.Location = CustomerDetails.Customermaster.Location;
                Customer.GSTNo = CustomerDetails.Customermaster.GSTNo;
                Customer.MobileNo = CustomerDetails.Customermaster.MobileNo;
                Customer.PhoneNo = CustomerDetails.Customermaster.PhoneNo;
                Customer.ContactPerson = CustomerDetails.Customermaster.ContactPerson;
                Customer.IsDeleted = false;
                Customer.UpdatedBy = CustomerDetails.Customermaster.UpdatedBy;
                Customer.UpdatedUTC = DateTime.UtcNow;
                WYNKContext.CustomerMaster.Update(Customer);


                string cmpname = CMPSContext.Company.Where(x => x.CmpID == CustomerDetails.Customermaster.CmpID).Select(x => x.CompanyName).FirstOrDefault();
                string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == CustomerDetails.Customermaster.UpdatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                string userid = Convert.ToString(CustomerDetails.Customermaster.UpdatedBy);
                ErrorLog oErrorLogs = new ErrorLog();
                oErrorLogs.WriteErrorLogTitle(cmpname, "CUSTOMER MASTER", "User name :", username, "User ID :", userid, "Mode : Update");
                object names = Customer;
                oErrorLogs.WriteErrorLogArray("CustomerMaster", names);


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

        public dynamic DeleteCustomerMaster(int ID, int CMPID, int USERID)
        {
            try
            {
                var Customer = WYNKContext.CustomerMaster.Where(x => x.ID == ID).FirstOrDefault();
                if (Customer != null)
                {
                    Customer.IsDeleted = true;
                    WYNKContext.CustomerMaster.Update(Customer);

                    string cmpname = CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.CompanyName).FirstOrDefault();
                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == USERID).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    string userid = Convert.ToString(USERID);
                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(cmpname, "CUSTOMER MASTER", "User name :", username, "User ID :", userid, "Mode : Delete");
                    object names = Customer;
                    oErrorLogs.WriteErrorLogArray("CustomerMaster", names);

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













