using Microsoft.EntityFrameworkCore;
using CommonMailService.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Common;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class UserVsRoleRepository : RepositoryBase<UserVsRoleViewModel>, IUsersVsRoleRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public UserVsRoleRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;


        }

        public UserVsRoleViewModel GetUsersDetails(int CmpID,int RoleID)
        {
            var UserDetails = new UserVsRoleViewModel();

            var DoctorMaster = CMPSContext.DoctorMaster.ToList();
            var Users = CMPSContext.Users.Where(x => x.Isactive == true && x.CMPID == CmpID);
            var UserVsRole = CMPSContext.UserVsRole.Where(x => x.IsDeleted == false && x.CMPID == CmpID && x.RoleID==RoleID);
            var Employee = CMPSContext.Employee.ToList();
            var EmployeeCommunication = CMPSContext.EmployeeCommunication.ToList();   
            var DoctorDetails1 = (from DR in DoctorMaster.Where(x => x.IsActive == true && x.IsDeleted == false && x.CMPID == CmpID)
                                            join UVR in UserVsRole on DR.EmailID equals UVR.UserName
                                          
                                  select new 
                                            {
                                                userId = UVR.UserID,
                                                Title = DR.Title,
                                                Firstname = DR.Firstname,
                                                LastName = DR.LastName,
                                                MiddleName = DR.MiddleName,
                                                Phone1 = DR.Phone1,
                                                EmailID = DR.EmailID,
                                      Status = true,
                                  }).ToList();
            var DoctorDetails = (from DR in DoctorMaster.Where(x => x.IsActive == true && x.IsDeleted == false && x.CMPID == CmpID)
                                 join UR in Users on DR.EmailID equals UR.Username
                                 where DoctorDetails1.All(a => a.userId != UR.Userid)
                                 select new
                                 {
                                     userId = UR.Userid,
                                     Title = DR.Title,
                                     Firstname = DR.Firstname,
                                     LastName = DR.LastName,
                                     MiddleName = DR.MiddleName,
                                     Phone1 = DR.Phone1,
                                     EmailID = DR.EmailID,
                                     Status = false,
                                 }).ToList();

            var mergeddata = DoctorDetails1.Concat(DoctorDetails);

            UserDetails.GetDoctorDetails = (from gg in mergeddata
                              
                                 select new GetDoctorDetails
                                 {
                                     userId = gg.userId,
                                     Title = gg.Title,
                                     Firstname = gg.Firstname,
                                     LastName = gg.LastName,
                                     MiddleName = gg.MiddleName,
                                     Phone1 = gg.Phone1,
                                     EmailID = gg.EmailID,
                                     Status = gg.Status,
                                 }).ToList();

            UserDetails.GetDoctorDetails1 = (from gg in DoctorDetails1

                                             select new GetDoctorDetails1
                                            {
                                                userId = gg.userId,
                                                Title = gg.Title,
                                                Firstname = gg.Firstname,
                                                LastName = gg.LastName,
                                                MiddleName = gg.MiddleName,
                                                Phone1 = gg.Phone1,
                                                EmailID = gg.EmailID,
                                                Status = gg.Status,
                                            }).ToList();




            var EmployeeDetails1 = (from EMP in Employee.Where(x => x.IsActive == true && x.IsDeleted == false && x.CMPID == CmpID)
                                    join EMPC in EmployeeCommunication on EMP.EmployeeID equals EMPC.EmpID
                                    join UVR in UserVsRole on EMPC.EmailID equals UVR.UserName

                                  select new
                                  {
                                      userId = UVR.UserID,
                                      Title = EMP.Title,
                                      Firstname = EMP.FirstName,
                                      LastName = EMP.LastName,
                                      MiddleName = EMP.MiddleName,
                                      EmailID = EMPC.EmailID,
                                      Phone1 = EMPC.MobileNumber1,
                                      Status = true,
                                  }).ToList();
            var EmployeeDetails = (from EMP in Employee.Where(x => x.IsActive == true && x.IsDeleted == false && x.CMPID == CmpID)
                                   join EMPC in EmployeeCommunication on EMP.EmployeeID equals EMPC.EmpID
                                   join UR in Users on EMPC.EmailID equals UR.Username
                                 where EmployeeDetails1.All(a => a.userId != UR.Userid)
                                 select new
                                 {
                                     userId = UR.Userid,
                                     Title = EMP.Title,
                                     Firstname = EMP.FirstName,
                                     LastName = EMP.LastName,
                                     MiddleName = EMP.MiddleName,
                                     EmailID = EMPC.EmailID,
                                     Phone1 = EMPC.MobileNumber1,
                                     Status = false,
                                 }).ToList();



            var mergeddata1 = EmployeeDetails1.Concat(EmployeeDetails);



            UserDetails.GetEmployeeDetails = (from E in mergeddata1
                                              select new GetEmployeeDetails
                                              {
                                               userId = E.userId,
                                               Title = E.Title,
                                               Firstname = E.Firstname,
                                               LastName = E.LastName,
                                               MiddleName = E.MiddleName,
                                               EmailID= E.EmailID,
                                               Phone1 = E.Phone1,
                                               Status=E.Status,
                                              }).ToList();

            UserDetails.GetEmployeeDetails1 = (from Ee in EmployeeDetails1
                                               select new GetEmployeeDetails1
                                               {
                                                  userId = Ee.userId,
                                                  Title = Ee.Title,
                                                  Firstname = Ee.Firstname,
                                                  LastName = Ee.LastName,
                                                  MiddleName = Ee.MiddleName,
                                                  EmailID = Ee.EmailID,
                                                  Phone1 = Ee.Phone1,
                                                  Status = Ee.Status,
                                               }).ToList();

            return UserDetails;
        }



        public dynamic InsertUserVsRole(UserVsRoleViewModel UserVsRoleInsert, int RoleID, int userroleID,int CmpID,string Dtag)
        {
            try
            {         
                var UserVsRole = new User_Role ();
                var UserVsRole1 = CMPSContext.UserVsRole.Where(x => x.RoleID == RoleID && x.ReferenceTag=="M").ToList();
                var UserVsRoleE = CMPSContext.UserVsRole.Where(x => x.RoleID == RoleID && x.ReferenceTag == "E").ToList();

            
                
                    if (Dtag !="null")
                    {
                        if (UserVsRole1.Count != 0)
                        {
                            CMPSContext.UserVsRole.RemoveRange(UserVsRole1);
                            CMPSContext.SaveChanges();
                        }
                        foreach (var item in UserVsRoleInsert.GetDoctorDetails1.ToList())
                        {
                            var UVR = new User_Role();
                            UVR.UserID = item.userId;
                            UVR.CMPID = CmpID;
                            UVR.RoleID = RoleID;
                            UVR.UserName = item.EmailID;
                            UVR.RoleDescription = CMPSContext.Role.Where(x => x.RoleID == RoleID).Select(x => x.RoleDescription).FirstOrDefault();
                            UVR.CreatedUTC = DateTime.UtcNow;
                            UVR.CreatedBy = userroleID;
                            UVR.IsDeleted = false;
                            UVR.ReferenceTag = "M";
                            CMPSContext.UserVsRole.AddRange(UVR);
                        }
                    }
                    else
                    {
                        if (UserVsRoleE.Count != 0)
                        {
                            CMPSContext.UserVsRole.RemoveRange(UserVsRoleE);
                            CMPSContext.SaveChanges();
                        }
                        foreach (var item in UserVsRoleInsert.GetEmployeeDetails1.ToList())
                        {
                            var UVR = new User_Role();
                            UVR.UserID = item.userId;
                            UVR.CMPID = CmpID;
                            UVR.RoleID = RoleID;
                            UVR.UserName = item.EmailID;
                            UVR.RoleDescription = CMPSContext.Role.Where(x => x.RoleID == RoleID).Select(x => x.RoleDescription).FirstOrDefault();
                            UVR.CreatedUTC = DateTime.UtcNow;
                            UVR.CreatedBy = userroleID;
                            UVR.IsDeleted = false;
                            UVR.ReferenceTag = "E";
                            CMPSContext.UserVsRole.AddRange(UVR);
                        }
                    }
                
                CMPSContext.SaveChanges();
                return new
                {
                    Success = true,
                    Message = "Saved successfully",
                };
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



    }
}
