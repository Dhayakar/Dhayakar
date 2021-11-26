using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public UserRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic GetWorkflodata(string res1)
        {
            var AccessWorkflodata = new ModuleMaster();

            AccessWorkflodata.Parentmoduledescription = CMPSContext.ModuleMaster.Where(x => x.ModuleDescription == res1 && x.Parentmoduledescription != null)
                .Select(x => x.Parentmoduledescription).FirstOrDefault();

            return AccessWorkflodata;

        }


        public dynamic GetModuletransactiondetails(string ModuleDescription, int CompanyID)
        {
            var AccessPrivilegesDetailss = new Rolevsaccesscontrol();

            var moduledesc = CMPSContext.ModuleMaster.Where(x => x.Parentmoduledescription == ModuleDescription).Select(x => x.ModuleDescription).FirstOrDefault();
            AccessPrivilegesDetailss.transactionid = CMPSContext.NumberControl.Where(x => x.Description == moduledesc).Select(x => x.TransactionID).FirstOrDefault();
            AccessPrivilegesDetailss.RecPayContra = CMPSContext.TransactionType.Where(x => x.TransactionID == AccessPrivilegesDetailss.transactionid).Select(x => x.RecPayContra).FirstOrDefault();

            return AccessPrivilegesDetailss;

        }

        public dynamic GetModuletransactiondetailsstring(string ModuleDescription, string suffix, int CompanyID)
        {
            var AccessPrivilegesDetailss = new Rolevsaccesscontrol();

            var dd = ModuleDescription + '/' + suffix;

            var moduledesc = CMPSContext.ModuleMaster.Where(x => x.Parentmoduledescription == dd).Select(x => x.ModuleDescription).FirstOrDefault();
            AccessPrivilegesDetailss.transactionid = CMPSContext.NumberControl.Where(x => x.Description == moduledesc).Select(x => x.TransactionID).FirstOrDefault();
            AccessPrivilegesDetailss.RecPayContra = CMPSContext.TransactionType.Where(x => x.TransactionID == AccessPrivilegesDetailss.transactionid).Select(x => x.RecPayContra).FirstOrDefault();

            return AccessPrivilegesDetailss;

        }

        public dynamic Getconsulttranid(string ModuleDescription, int CompanyID)
        {
            var AccessPrivilegesDetailss = new Rolevsaccesscontrol();


            AccessPrivilegesDetailss.transactionid = CMPSContext.NumberControl.Where(x => x.Description == ModuleDescription && x.CmpID == CompanyID).Select(x => x.TransactionID).FirstOrDefault();

            return AccessPrivilegesDetailss;

        }



        public dynamic GetRoledetails(string roletext, int CompanyID)
        {
            var user = new Users();
            var AccessPrivilegesDetailss = new Rolevsaccesscontrol();

            if (roletext == "MS")
            {

                var res = (from cm in CMPSContext.Users.Where(x => x.CMPID == CompanyID && x.Isactive == true)
                           join qw in CMPSContext.DoctorMaster.Where(x => x.IsActive == true) on cm.Username equals qw.EmailID
                           select qw.EmailID).ToList();

                AccessPrivilegesDetailss.AccessNames = (from Dm in CMPSContext.DoctorMaster.Where(x => x.CMPID == Convert.ToInt32(CompanyID) && x.IsActive == true)
                                                        where !res.Contains(Dm.EmailID)
                                                        select new AccessNames
                                                        {
                                                            Roleids = Dm.DoctorID,
                                                            Roledescriptions = getnames(Dm.Firstname, Dm.LastName),
                                                        }).ToList();

            }
            else

            {

                var res = (from cm in CMPSContext.Users.Where(x => x.CMPID == CompanyID && x.Isactive == true)
                           join qw in CMPSContext.EmployeeCommunication on cm.Username equals qw.EmailID
                           select qw.EmailID).ToList();

                AccessPrivilegesDetailss.AccessNames = (from ee in CMPSContext.EmployeeCommunication
                                                        join Dm in CMPSContext.Employee.Where(x => x.CMPID == Convert.ToInt32(CompanyID)
                                                        && x.IsActive == true) on ee.EmpID equals Dm.EmployeeID
                                                        join ees in CMPSContext.EmployeeCommunication on Dm.EmployeeID equals ees.EmpID
                                                        where !res.Contains(ees.EmailID)
                                                        select new AccessNames
                                                        {
                                                            Roleids = Dm.EmployeeID,
                                                            Roledescriptions = getnames(Dm.FirstName, Dm.LastName),
                                                        }).ToList();

            }
            return AccessPrivilegesDetailss;
        }


        public string getnames(string fname, string lname)
        {
            var nname = "";

            nname = fname + " " + lname;

            return nname;
        }

        public dynamic Postinternaluserdetails(User AddEmp)
        {

            var users = new User_Master();

            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {

                try
                {

                    if (AddEmp.Inetrnaluserdetails.password == "MS")
                    {
                        var Docdetails = CMPSContext.DoctorMaster.Where(x => x.DoctorID == Convert.ToInt32(AddEmp.Inetrnaluserdetails.Userrole)
                       && x.CMPID == Convert.ToInt32(AddEmp.Inetrnaluserdetails.companyid)).FirstOrDefault();
                        if (Docdetails != null)
                        {
                            var user = new Users();
                            user.CMPID = Convert.ToInt32(AddEmp.Inetrnaluserdetails.companyid);
                            user.Username = Docdetails.EmailID;
                            user.Password = PasswordEncodeandDecode.EncodePasswordToBase64(AddEmp.Inetrnaluserdetails.Confirmpassword);
                            if (AddEmp.Inetrnaluserdetails.AccessRole == "A")
                            {
                                user.Useraccess = "A";
                                user.Usertype = "MS";
                                user.ReferenceID = CMPSContext.Role.Where(x => x.RoleDescription == "Admin").Select(x => x.RoleID).FirstOrDefault();
                                user.ReferenceTag = "A";
                            }
                            else
                            {
                                user.Useraccess = Docdetails.StaffIdentification;
                                user.Usertype = "MS";
                                user.ReferenceID = Convert.ToInt32(AddEmp.Inetrnaluserdetails.Baserole);
                                user.ReferenceTag = Docdetails.StaffIdentification;
                            }
                            user.Emailid = Docdetails.EmailID;
                            user.Isactive = true;
                            user.Createdutc = DateTime.Now.Date;
                            user.Createdby = Convert.ToInt32(AddEmp.Inetrnaluserdetails.Userid);
                            CMPSContext.Users.Add(user);
                            CMPSContext.SaveChanges();

                            var uservsrole = new User_Role();
                            var useriss = CMPSContext.Users.Select(x => x.Userid).LastOrDefault();
                            uservsrole.UserID = useriss;
                            uservsrole.CMPID = Convert.ToInt32(AddEmp.Inetrnaluserdetails.companyid);
                            uservsrole.UserName = Docdetails.EmailID;
                            uservsrole.RoleID = Convert.ToInt32(AddEmp.Inetrnaluserdetails.Baserole);
                            uservsrole.RoleDescription = CMPSContext.Role.Where(x => x.RoleID == uservsrole.RoleID).Select(x => x.RoleDescription).FirstOrDefault();
                            uservsrole.IsDeleted = false;
                            uservsrole.CreatedUTC = DateTime.Now.Date;
                            uservsrole.CreatedBy = Convert.ToInt32(AddEmp.Inetrnaluserdetails.Userid);
                            CMPSContext.UserVsRole.Add(uservsrole);
                            CMPSContext.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                        else
                        {
                            AddEmp.Check = "No Mail ID";
                        }
                    }
                    else
                    {
                        var Docdetails = CMPSContext.Employee.Where(x => x.EmployeeID == Convert.ToInt32(AddEmp.Inetrnaluserdetails.Userrole)
                         && x.CMPID == Convert.ToInt32(AddEmp.Inetrnaluserdetails.companyid)).FirstOrDefault();
                        var emaildid = CMPSContext.EmployeeCommunication.Where(x => x.EmpID == Docdetails.EmployeeID).Select(x => x.EmailID).FirstOrDefault();

                        if (Docdetails != null)
                        {
                            var user = new Users();
                            user.CMPID = Convert.ToInt32(AddEmp.Inetrnaluserdetails.companyid);
                            user.Username = emaildid;
                            user.Password = PasswordEncodeandDecode.EncodePasswordToBase64(AddEmp.Inetrnaluserdetails.Confirmpassword);
                            if (AddEmp.Inetrnaluserdetails.AccessRole == "A")
                            {
                                user.Useraccess = "A";
                                user.Usertype = "NMS";
                                user.ReferenceID = CMPSContext.Role.Where(x => x.RoleDescription == "Admin").Select(x => x.RoleID).FirstOrDefault();
                                user.ReferenceTag = "A";
                            }
                            else
                            {
                                user.Useraccess = "E";
                                user.Usertype = "NMS";
                                user.ReferenceID = Convert.ToInt32(AddEmp.Inetrnaluserdetails.Baserole);
                                user.ReferenceTag = "E";
                            }
                            user.Emailid = emaildid;
                            user.Isactive = true;
                            user.Createdutc = DateTime.Now.Date;
                            user.Createdby = Convert.ToInt32(AddEmp.Inetrnaluserdetails.Userid);
                            CMPSContext.Users.Add(user);
                            CMPSContext.SaveChanges();

                            var uservsrole = new User_Role();
                            var useriss = CMPSContext.Users.Select(x => x.Userid).LastOrDefault();
                            uservsrole.UserID = useriss;
                            uservsrole.CMPID = Convert.ToInt32(AddEmp.Inetrnaluserdetails.companyid);
                            uservsrole.UserName = emaildid;
                            uservsrole.RoleID = Convert.ToInt32(AddEmp.Inetrnaluserdetails.Baserole);
                            uservsrole.RoleDescription = CMPSContext.Role.Where(x => x.RoleID == uservsrole.RoleID).Select(x => x.RoleDescription).FirstOrDefault();
                            uservsrole.IsDeleted = false;
                            uservsrole.CreatedUTC = DateTime.Now.Date;
                            uservsrole.CreatedBy = Convert.ToInt32(AddEmp.Inetrnaluserdetails.Userid);
                            CMPSContext.UserVsRole.Add(uservsrole);
                            CMPSContext.SaveChanges();
                            dbContextTransaction.Commit();
                        }


                    }
                    if (CMPSContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Message = "Saved successfully",
                            Check = AddEmp.Check,
                        };
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
                return new
                {
                    Success = false,
                    Message = "some data are missing"
                };

            }



        }

        public dynamic updateUserDet(User User, int ID, Boolean IsActive, int DoctorID)
        {

            try
            {

                var users = new Users();

                users = CMPSContext.Users.Where(x => x.Userid == ID).FirstOrDefault();

                users.Isactive = IsActive;
                users.Updatedby = DoctorID;
                users.Updatedutc = DateTime.UtcNow;


                CMPSContext.Users.UpdateRange(users);

                try
                {
                    if (CMPSContext.SaveChanges() > 0)
                        return new
                        {
                            Success = true,
                            Message = "update successfully"
                        };
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = "Some data are Missing"
            };

        }




        public dynamic PostExternaluserdetails(User AddEmp)
        {

            var users = new User_Master();

            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {

                try
                {
                    if (AddEmp.Inetrnaluserdetails.Baserole == "Doctor")
                    {

                        dbContextTransaction.Commit();

                    }
                    else if (AddEmp.Inetrnaluserdetails.Baserole == "Nurse")
                    {

                    }
                    else if (AddEmp.Inetrnaluserdetails.Baserole == "Chief Nurse")
                    {

                    }
                    else if (AddEmp.Inetrnaluserdetails.Baserole == "Reception")
                    {

                        dbContextTransaction.Commit();
                    }
                    else if (AddEmp.Inetrnaluserdetails.Baserole == "Optometrist")
                    {

                        dbContextTransaction.Commit();
                    }
                    else if (AddEmp.Inetrnaluserdetails.Baserole == "Employee")
                    {

                        dbContextTransaction.Commit();
                    }

                    if (CMPSContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Message = "Saved successfully",
                        };
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
                return new
                {
                    Success = false,
                    Message = "some data are missing"
                };
            }
        }


    }



}







