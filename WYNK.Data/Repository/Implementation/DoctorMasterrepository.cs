using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;


namespace WYNK.Data.Repository.Implementation
{
    class DoctorMasterrepository : RepositoryBase<DoctorMaster>, IDoctorMasterRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public DoctorMasterrepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;
        }
        public dynamic UpdateDoctorMaster(DoctorMaster AddDoctor)
        {
            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {
                var oCMpname = CMPSContext.Company.Where(x => x.CmpID == AddDoctor.doctorMaster.CMPID).Select(x => x.CompanyName).FirstOrDefault();

                try
                {
                    int cmd = 0;
                    string RefTag = "D";
                    var roleid = 0;

                    var EmailExist = CMPSContext.DoctorMaster.Where(x => x.EmailID == AddDoctor.doctorMaster.EmailID && x.CMPID == AddDoctor.doctorMaster.CMPID).FirstOrDefault();

                    if(EmailExist != null) 
                    {
                        return new
                        {
                            Success = false,
                            Message = "Email already Exists"
                        };
                    }

                    if (AddDoctor.Specialitytrans.Count() == 1)
                    {
                        foreach (var item in AddDoctor.Specialitytrans.ToList())
                        {
                            var Optometrist = CMPSContext.OneLineMaster.Where(x => x.OLMID == item.OLMID).Select(x => x.ParentDescription).FirstOrDefault();

                            if (Optometrist == "Optometrist")
                            {
                                cmd = 0;
                                RefTag = "O";


                            }
                            else if (Optometrist == "Vision")
                            {
                                cmd = 0;
                                RefTag = "V";

                            }
                            else
                            {
                                cmd = Convert.ToInt16(CMPSContext.DoctorMaster.Where(x => x.RegistrationNumber == AddDoctor.doctorMaster.RegistrationNumber).Select(x => x.DoctorID).FirstOrDefault());
                            }

                        }
                    }
                    else
                    {
                        cmd = Convert.ToInt16(CMPSContext.DoctorMaster.Where(x => x.RegistrationNumber == AddDoctor.doctorMaster.RegistrationNumber).Select(x => x.DoctorID).FirstOrDefault());

                    }

                    if (cmd == 0)
                    {
                        var doctorMaster = new Doctor_Master();
                        doctorMaster.Firstname = AddDoctor.doctorMaster.Firstname;

                        if (AddDoctor.doctorMaster.MiddleName == null)
                        {
                            doctorMaster.MiddleName = " ";
                        }
                        else
                        {
                            doctorMaster.MiddleName = AddDoctor.doctorMaster.MiddleName;
                        }

                        doctorMaster.LastName = AddDoctor.doctorMaster.LastName;

                        doctorMaster.Title = AddDoctor.doctorMaster.Title;
                        doctorMaster.Gender = AddDoctor.doctorMaster.Gender;
                        doctorMaster.DateofBirth = AddDoctor.doctorMaster.DateofBirth;
                        doctorMaster.Address1 = AddDoctor.doctorMaster.Address1;
                        doctorMaster.Address2 = AddDoctor.doctorMaster.Address2;
                        doctorMaster.Address3 = AddDoctor.doctorMaster.Address3;
                        doctorMaster.LocationID = AddDoctor.doctorMaster.LocationID;
                        doctorMaster.Phone1 = AddDoctor.doctorMaster.Phone1;

                        if (RefTag == "O")
                        {
                            roleid = CMPSContext.Role.Where(x => x.RoleDescription == "Optometrist").Select(x => x.RoleID).FirstOrDefault();
                        }
                        else if (RefTag == "V")
                        {
                            roleid = CMPSContext.Role.Where(x => x.RoleDescription == "Vision").Select(x => x.RoleID).FirstOrDefault();
                        }
                        else
                        {
                            roleid = CMPSContext.Role.Where(x => x.RoleDescription == "Doctor").Select(x => x.RoleID).FirstOrDefault();
                        }

                        doctorMaster.RoleID = roleid;
                        doctorMaster.StaffIdentification = RefTag;

                        doctorMaster.Phone2 = AddDoctor.doctorMaster.Phone2;
                        doctorMaster.RegistrationNumber = AddDoctor.doctorMaster.RegistrationNumber;
                        doctorMaster.EngagementType = AddDoctor.doctorMaster.EngagementType;
                        var CMpname = CMPSContext.Company.Where(x => x.CmpID == AddDoctor.doctorMaster.CMPID).Select(x => x.CompanyName).FirstOrDefault();
                        var Doctorfullname = doctorMaster.Firstname + doctorMaster.MiddleName + doctorMaster.LastName;
                        try
                        {
                            doctorMaster.EmailID = AddDoctor.doctorMaster.EmailID;

                        }
                        catch (Exception ex)
                        {
                            ErrorLog oErrorLog = new ErrorLog();
                            oErrorLog.WriteErrorLog("Error :", "Mail Service Problem");
                            return new
                            {
                                Success = false,
                                Message = "Mail Service Problem"
                            };
                        }
                        doctorMaster.CMPID = AddDoctor.doctorMaster.CMPID;
                        doctorMaster.CreatedBy = AddDoctor.doctorMaster.CreatedBy;
                        doctorMaster.IsActive = true;
                        doctorMaster.IsDeleted = false;
                        doctorMaster.CreatedUTC = DateTime.UtcNow;
                        int doctorid = CMPSContext.DoctorMaster.OrderByDescending(x => x.DoctorID).Select(x => x.DoctorID).FirstOrDefault();
                        doctorMaster.DoctorID = doctorid + 1;
                        CMPSContext.DoctorMaster.Add(doctorMaster);

                        string cmpname = CMPSContext.Company.Where(x => x.CmpID == AddDoctor.doctorMaster.CMPID).Select(x => x.CompanyName).FirstOrDefault();
                        string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == AddDoctor.doctorMaster.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                        string userid = Convert.ToString(AddDoctor.doctorMaster.CreatedBy);
                        ErrorLog oErrorLogs = new ErrorLog();
                        oErrorLogs.WriteErrorLogTitle(cmpname, "Doctor Master", "User name :", username, "User ID :", userid, "Mode : Add");

                        object names = doctorMaster;
                        oErrorLogs.WriteErrorLogArray("DoctorMaster", names);

                        CMPSContext.SaveChanges();
                        if (AddDoctor.Specialitytrans.Count() > 0)
                        {
                            foreach (var item in AddDoctor.Specialitytrans.ToList())
                            {
                                int getdoctorid = doctorMaster.DoctorID;
                                var spee = new Speciality_master();
                                spee.OLMID = item.OLMID;
                                spee.CreatedUTC = DateTime.UtcNow;
                                spee.DoctorID = getdoctorid;
                                spee.CreatedBy = AddDoctor.doctorMaster.CreatedBy;
                                spee.IsActive = true;
                                CMPSContext.DoctorSpeciality.AddRange(spee);

                                ErrorLog oErrorLogstran = new ErrorLog();
                                object namestr = spee;
                                oErrorLogstran.WriteErrorLogArray("DoctorSpeciality", namestr);
                            }
                        }
                        CMPSContext.SaveChanges();

                        if (CMPSContext.SaveChanges() >= 0)
                        {
                            ErrorLog oErrorLog = new ErrorLog();
                            oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                        }


                        dbContextTransaction.Commit();
                        return new
                        {
                            id = doctorMaster.DoctorID,
                            Success = true,
                            Message = "Saved successfully"
                        };
                    }

                    else
                    {
                        return new
                        {
                            Success = false,
                            Message = "Registration Number already Exists"
                        };
                    }

                }
                catch (Exception ex)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                    dbContextTransaction.Rollback();
                    return new
                    {
                        Success = false,
                    };
                }

            }
        }
        public dynamic DeleteDoctor(int? DoctorID, int? DoctorSpecialityID)
        {
            var cmb = CMPSContext.DoctorSpeciality.Where(x => x.DoctorID == DoctorID && x.DoctorSpecialityID == DoctorSpecialityID).ToList();
            if (cmb != null)
            {
                cmb.All(x => { x.IsActive = false; return true; });
                CMPSContext.DoctorSpeciality.UpdateRange(cmb);
                CMPSContext.SaveChanges();
            }

            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,
                        Message = "Delete successfully"
                    };
            }
            catch (Exception ex)
            {
                ErrorLog oErrorLog = new ErrorLog();
                oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
            }
            return new
            {
                Success = false,
            };
        }

        public dynamic DeleteDoctor1(int? DoctorID)
        {
            var cmb = CMPSContext.DoctorMaster.Where(x => x.DoctorID == DoctorID).ToList();
            if (cmb != null)
            {
                cmb.All(x => { x.IsDeleted = true; x.IsActive = false; return true; });
                CMPSContext.DoctorMaster.UpdateRange(cmb);

            }
            var userid = CMPSContext.Users.Where(x => x.ReferenceID == DoctorID).Select(x => x.Userid).FirstOrDefault();
            if (userid != 0)
            {
                var userids = CMPSContext.Users.Where(x => x.ReferenceID == DoctorID).FirstOrDefault();
                userids.Isactive = false;
                CMPSContext.Users.UpdateRange(userids);
            }
            var usersvsroleID = CMPSContext.UserVsRole.Where(x => x.UserID == userid).ToList();
            if (usersvsroleID != null)
            {
                usersvsroleID.All(x => { x.IsDeleted = true; return true; });
                CMPSContext.UserVsRole.UpdateRange(usersvsroleID);
            }
            CMPSContext.SaveChanges();
            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = "Delete successfully"
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
            };
        }

        public dynamic UpDateDocMas(DoctorMaster DoctorMaster, int? DoctorID)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var EmailExist = CMPSContext.DoctorMaster.Where(x => x.DoctorID != DoctorID &&  x.EmailID == DoctorMaster.doctorMaster.EmailID).FirstOrDefault();

                    if (EmailExist != null)
                    {
                        return new
                        {
                            Success = false,
                            Message = "Email already Exists"
                        };
                    }
                    var cmd = CMPSContext.DoctorMaster.Where(x => x.DoctorID == DoctorID && x.RegistrationNumber == DoctorMaster.doctorMaster.RegistrationNumber).ToList();

                    if (cmd.Count == 1)
                    {
                        string RefTag = "D";
                        var roleid = 0;
                        if (DoctorMaster.Specialitytrans.Count() == 1)
                        {
                            foreach (var item in DoctorMaster.Specialitytrans.ToList())
                            {
                                var Optometrist = CMPSContext.OneLineMaster.Where(x => x.OLMID == item.OLMID).Select(x => x.ParentDescription).FirstOrDefault();

                                if (Optometrist == "Optometrist")
                                {
                                    RefTag = "O";
                                }
                                else if (Optometrist == "Vision")
                                {
                                    RefTag = "V";
                                }

                            }
                        }

                        var doctorMaster = new Doctor_Master();
                        var speciality = new Speciality_master();
                        doctorMaster = CMPSContext.DoctorMaster.Where(x => x.DoctorID == DoctorID).FirstOrDefault();
                        doctorMaster.Firstname = DoctorMaster.doctorMaster.Firstname;
                        doctorMaster.LastName = DoctorMaster.doctorMaster.LastName;
                        doctorMaster.MiddleName = DoctorMaster.doctorMaster.MiddleName;
                        doctorMaster.Title = DoctorMaster.doctorMaster.Title;
                        doctorMaster.Gender = DoctorMaster.doctorMaster.Gender;
                        doctorMaster.DateofBirth = DoctorMaster.doctorMaster.DateofBirth;
                        doctorMaster.Address1 = DoctorMaster.doctorMaster.Address1;
                        doctorMaster.Address2 = DoctorMaster.doctorMaster.Address2;
                        doctorMaster.Address3 = DoctorMaster.doctorMaster.Address3;
                        doctorMaster.LocationID = DoctorMaster.doctorMaster.LocationID;
                        if (RefTag == "O")
                        {
                            roleid = CMPSContext.Role.Where(x => x.RoleDescription == "Optometrist").Select(x => x.RoleID).FirstOrDefault();
                        }
                        else if (RefTag == "V")
                        {
                            roleid = CMPSContext.Role.Where(x => x.RoleDescription == "Vision").Select(x => x.RoleID).FirstOrDefault();
                        }
                        else
                        {
                            roleid = CMPSContext.Role.Where(x => x.RoleDescription == "Doctor").Select(x => x.RoleID).FirstOrDefault();
                        }
                        doctorMaster.RoleID = roleid;
                        doctorMaster.StaffIdentification = RefTag;
                        doctorMaster.Phone1 = DoctorMaster.doctorMaster.Phone1;
                        doctorMaster.Phone2 = DoctorMaster.doctorMaster.Phone2;
                       
                        doctorMaster.RegistrationNumber = DoctorMaster.doctorMaster.RegistrationNumber;
                        doctorMaster.EngagementType = DoctorMaster.doctorMaster.EngagementType;
                        doctorMaster.EmailID = DoctorMaster.doctorMaster.EmailID;
                        doctorMaster.IsActive = DoctorMaster.doctorMaster.IsActive;
                        doctorMaster.IsDeleted = false;
                        doctorMaster.UpdatedUTC = DateTime.UtcNow;
                        doctorMaster.UpdatedBy = DoctorMaster.doctorMaster.UpdatedBy;
                        CMPSContext.DoctorMaster.UpdateRange(doctorMaster);

                        string cmpname = CMPSContext.Company.Where(x => x.CmpID == DoctorMaster.doctorMaster.CMPID).Select(x => x.CompanyName).FirstOrDefault();
                        string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == DoctorMaster.doctorMaster.UpdatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                        string userid = Convert.ToString(DoctorMaster.doctorMaster.UpdatedBy);
                        ErrorLog oErrorLogs = new ErrorLog();
                        oErrorLogs.WriteErrorLogTitle(cmpname, "Doctor Master", "User name :", username, "User ID :", userid, "Mode : Edit");

                        object names = doctorMaster;
                        oErrorLogs.WriteErrorLogArray("DoctorMaster", names);

                        var masters = CMPSContext.DoctorSpeciality.Where(x => x.DoctorID == DoctorID);
                        if (masters != null)
                        {

                            CMPSContext.DoctorSpeciality.RemoveRange(CMPSContext.DoctorSpeciality.Where(x => x.DoctorID == DoctorID).ToList());
                            CMPSContext.SaveChanges();
                        }
                        if (DoctorMaster.Specialitytrans.Count() > 0)
                        {
                            foreach (var item in DoctorMaster.Specialitytrans.ToList())
                            {
                                int getdoctorid = doctorMaster.DoctorID;
                                var spee = new Speciality_master();
                                spee.OLMID = item.OLMID;
                                spee.CreatedUTC = DateTime.UtcNow;
                                spee.DoctorID = getdoctorid;
                                spee.CreatedBy = Convert.ToInt32(DoctorMaster.doctorMaster.UpdatedBy);
                                spee.IsActive = true;
                                CMPSContext.DoctorSpeciality.AddRange(spee);
                                ErrorLog oErrorLogstran = new ErrorLog();
                                object namestr = spee;
                                oErrorLogstran.WriteErrorLogArray("DoctorSpeciality", namestr);
                            }
                            CMPSContext.SaveChanges();
                        }

                    }

                    else
                    {
                        var cmd1 = CMPSContext.DoctorMaster.Where(x => x.RegistrationNumber == DoctorMaster.doctorMaster.RegistrationNumber).ToList();

                        if (cmd1.Count != 1)
                        {

                            string RefTag = "D";
                            var roleid = 0;
                            if (DoctorMaster.Specialitytrans.Count() == 1)
                            {
                                foreach (var item in DoctorMaster.Specialitytrans.ToList())
                                {
                                    var Optometrist = CMPSContext.OneLineMaster.Where(x => x.OLMID == item.OLMID).Select(x => x.ParentDescription).FirstOrDefault();

                                    if (Optometrist == "Optometrist")
                                    {
                                        RefTag = "O";
                                    }
                                    else if (Optometrist == "Vision")
                                    {
                                        RefTag = "V";
                                    }

                                }
                            }
                            var doctorMaster = new Doctor_Master();
                            var speciality = new Speciality_master();
                            doctorMaster = CMPSContext.DoctorMaster.Where(x => x.DoctorID == DoctorID).FirstOrDefault();
                            doctorMaster.Firstname = DoctorMaster.doctorMaster.Firstname;
                            doctorMaster.LastName = DoctorMaster.doctorMaster.LastName;
                            doctorMaster.MiddleName = DoctorMaster.doctorMaster.MiddleName;
                            doctorMaster.Title = DoctorMaster.doctorMaster.Title;
                            doctorMaster.Gender = DoctorMaster.doctorMaster.Gender;
                            doctorMaster.DateofBirth = DoctorMaster.doctorMaster.DateofBirth;
                            doctorMaster.Address1 = DoctorMaster.doctorMaster.Address1;
                            doctorMaster.Address2 = DoctorMaster.doctorMaster.Address2;
                            doctorMaster.Address3 = DoctorMaster.doctorMaster.Address3;
                            doctorMaster.LocationID = DoctorMaster.doctorMaster.LocationID;
                            if (RefTag == "O")
                            {
                                roleid = CMPSContext.Role.Where(x => x.RoleDescription == "Optometrist").Select(x => x.RoleID).FirstOrDefault();
                            }
                            else if (RefTag == "V")
                            {
                                roleid = CMPSContext.Role.Where(x => x.RoleDescription == "Vision").Select(x => x.RoleID).FirstOrDefault();
                            }
                            else
                            {
                                roleid = CMPSContext.Role.Where(x => x.RoleDescription == "Doctor").Select(x => x.RoleID).FirstOrDefault();
                            }
                            doctorMaster.RoleID = roleid;
                            doctorMaster.StaffIdentification = RefTag;
                            doctorMaster.Phone1 = DoctorMaster.doctorMaster.Phone1;
                            doctorMaster.Phone2 = DoctorMaster.doctorMaster.Phone2;                           
                            doctorMaster.RegistrationNumber = DoctorMaster.doctorMaster.RegistrationNumber;
                            doctorMaster.EngagementType = DoctorMaster.doctorMaster.EngagementType;
                            doctorMaster.EmailID = DoctorMaster.doctorMaster.EmailID;
                            doctorMaster.IsDeleted = false;
                            doctorMaster.UpdatedBy = DoctorMaster.doctorMaster.UpdatedBy;
                            doctorMaster.UpdatedUTC = DateTime.UtcNow;
                            CMPSContext.DoctorMaster.UpdateRange(doctorMaster);

                            string cmpname = CMPSContext.Company.Where(x => x.CmpID == DoctorMaster.doctorMaster.CMPID).Select(x => x.CompanyName).FirstOrDefault();
                            string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == DoctorMaster.doctorMaster.UpdatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                            string userid = Convert.ToString(DoctorMaster.doctorMaster.UpdatedBy);
                            ErrorLog oErrorLogs = new ErrorLog();
                            oErrorLogs.WriteErrorLogTitle(cmpname, "Doctor Master", "User name :", username, "User ID :", userid, "Mode : Update");

                            object names = doctorMaster;                            
                            oErrorLogs.WriteErrorLogArray("DoctorMaster", names);


                            var masters = CMPSContext.DoctorSpeciality.Where(x => x.DoctorID == DoctorID);
                            if (masters != null)
                            {

                                CMPSContext.DoctorSpeciality.RemoveRange(CMPSContext.DoctorSpeciality.Where(x => x.DoctorID == DoctorID).ToList());
                                CMPSContext.SaveChanges();
                            }
                            if (DoctorMaster.Specialitytrans.Count() > 0)
                            {
                                foreach (var item in DoctorMaster.Specialitytrans.ToList())
                                {
                                    int getdoctorid = doctorMaster.DoctorID;
                                    var spee = new Speciality_master();
                                    spee.OLMID = item.OLMID;
                                    spee.CreatedUTC = DateTime.UtcNow;
                                    spee.DoctorID = getdoctorid;
                                    spee.CreatedBy = Convert.ToInt32(DoctorMaster.doctorMaster.UpdatedBy);
                                    spee.IsActive = true;
                                    CMPSContext.DoctorSpeciality.AddRange(spee);
                                    ErrorLog oErrorLogstran = new ErrorLog();
                                    object namestr = spee;
                                    oErrorLogstran.WriteErrorLogArray("DoctorSpeciality", namestr);
                                }
                                CMPSContext.SaveChanges();
                            }
                        }

                        else
                        {
                            return new
                            {
                                Success = false,
                                Message = "Registration Number already Exists"
                            };
                        }

                    }



                    CMPSContext.SaveChanges();

                    if (CMPSContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Updated Successfully");
                    }

                    dbContextTransaction.Commit();

                    return new
                    {
                        Success = true,
                        Message = "Saved successfully"
                    };
                }

                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                    return new
                    {
                        Success = false,
                    };
                }

            }
        }
        public dynamic AddSpeciality(DoctorMaster AddSpecial)
        {
            try
            {
                var ole = new OneLine_Masters();

                ole.ParentDescription = AddSpecial.OneLineMaster.ParentDescription;
                ole.ParentTag = "ST";
                ole.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "ST").Select(x => x.ParentID).FirstOrDefault();
                ole.CreatedBy = AddSpecial.OneLineMaster.CreatedBy;
                ole.IsActive = true;
                ole.IsDeleted = false;
                ole.CreatedUTC = DateTime.UtcNow;
                CMPSContext.OneLineMaster.Add(ole);
                CMPSContext.SaveChanges();
                return new
                {
                    Success = true,
                    Message = "saved successfully"
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                };
            }
     


        }

        public dynamic DoctorAssignDetails(int? DoctorID)
        {

            var Assigned = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Assigned" && x.ParentTag == "RegStatus").Select(x => x.OLMID).FirstOrDefault();
            var Reassigned = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Reassigned" && x.ParentTag == "RegStatus").Select(x => x.OLMID).FirstOrDefault();

            var DoctorAssignedStatus = WYNKContext.RegistrationTran.Where(x => x.DoctorID == DoctorID && (x.Status == Assigned || x.Status == Reassigned)).FirstOrDefault();

            if (DoctorAssignedStatus != null)
            {
                return new
                {
                    Success = false,
                    Message = "Patients Assigned"
                };
            }

            return new
            {
                Success = true,
                Message = "No Patients Assigned"
            };
        }

        public DoctorMaster GetlocationDetails(int id)
        {
            var locDetails = new DoctorMaster();

            var v = CMPSContext.LocationMaster.Where(x => x.ID == id).Select(x => x.ParentDescription).FirstOrDefault();
            var stateid = CMPSContext.LocationMaster.Where(x => x.ParentDescription == v).Select(x => x.ParentID).FirstOrDefault();
            locDetails.ParentDescriptionstate = CMPSContext.LocationMaster.Where(x => x.ID == stateid).Select(x => x.ParentDescription).FirstOrDefault();

            var countryid = CMPSContext.LocationMaster.Where(x => x.ParentDescription == locDetails.ParentDescriptionstate).Select(x => x.ParentID).FirstOrDefault();
            locDetails.ParentDescriptioncountry = CMPSContext.LocationMaster.Where(x => x.ID == countryid).Select(x => x.ParentDescription).FirstOrDefault();

            return locDetails;

        }

        public dynamic UploadImage(IFormFile file, string docid)
        {

            var currentDir = Directory.GetCurrentDirectory();
            if (!Directory.Exists(currentDir + "/DoctorImages/"))
                Directory.CreateDirectory(currentDir + "/DoctorImages/");
            var fileName = $"{docid}{Path.GetExtension(file.FileName)}";
            var path = $"{currentDir}/DoctorImages/{fileName}";

            if ((File.Exists(path)))
                File.Delete(path);


            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
                var photo = CMPSContext.DoctorMaster.Where(x => x.DoctorID == Convert.ToInt32(docid)).FirstOrDefault();
                photo.Photopath = fileName;

                CMPSContext.Entry(photo).State = EntityState.Modified;

                var tt = CMPSContext.SaveChanges() > 0;
                return tt;

            }

        }

        public dynamic DeleteDocImage(string docid)
        {
            var currentDir = Directory.GetCurrentDirectory();
            if (!Directory.Exists(currentDir + "/DoctorImages/"))
                Directory.CreateDirectory(currentDir + "/DoctorImages/");
            var fileName = $"{docid}{".png"}";
            var path = $"{currentDir}/DoctorImages/{fileName}";
            if ((File.Exists(path)))
                File.Delete(path);
            var photo = CMPSContext.DoctorMaster.Where(x => x.DoctorID == Convert.ToInt32(docid)).FirstOrDefault();
            photo.Photopath = null;
            CMPSContext.Entry(photo).State = EntityState.Modified;
            var tt = CMPSContext.SaveChanges() > 0;
            return tt;
        }

        public dynamic Getpatientimage(string regNo)
        {
            var reg = new List<string>();
            var regs = CMPSContext.DoctorMaster.Where(x => x.DoctorID == Convert.ToInt32(regNo)).Select(x => x.Photopath).FirstOrDefault();
            if (regs != null)
            {
                var osfn = regs;
                var osfi = "/DoctorImages/";
                var currentDir = Directory.GetCurrentDirectory();
                string path = currentDir + osfi + osfn;
                if ((File.Exists(path)))
                {
                    string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                    reg.Add("data:image/png;base64," + imageData);
                }
                else
                {

                }
            }
            else
            {
            }
            return reg;
        }
        public dynamic UpdateDoctorVisitingHours(DoctorMaster Doctorvisisting)
        {
            var data = new DoctorMaster();
            if (Doctorvisisting.DoctorGridconsultingnewcolumns.Count != 0)
            {
                var docid = Doctorvisisting.DoctorGridconsultingnewcolumns.Select(x => x.DoctorID).FirstOrDefault();
                var deletedata = CMPSContext.DoctorExtension.Where(x => x.DoctorID == docid).ToList();
                CMPSContext.DoctorExtension.RemoveRange(deletedata);
                CMPSContext.SaveChanges();
                foreach (var ssitem in Doctorvisisting.DoctorGridconsultingnewcolumns)
                {
                    var id = CMPSContext.DoctorExtension.OrderByDescending(x => x.ID).Select(x => x.ID).FirstOrDefault();
                    var Extension = new DoctorExtension();
                    if (id == 0)
                    {
                        Extension.ID = 1;
                    }
                    else
                    {
                        Extension.ID = id + 1;
                    }
                    Extension.CMPID = Convert.ToInt32(Doctorvisisting.CPMID);
                    Extension.CreatedBy = Convert.ToInt32(Doctorvisisting.USERID);
                    Extension.DoctorID = Convert.ToInt32(ssitem.DoctorID);
                    Extension.FSFromTime = Convert.ToDateTime(ssitem.firstsessionstart).TimeOfDay;
                    Extension.FSToTime = Convert.ToDateTime(ssitem.firstsessionend).TimeOfDay;
                    Extension.SSFromTime = Convert.ToDateTime(ssitem.Secondsessionstart).TimeOfDay;
                    Extension.SSToTime = Convert.ToDateTime(ssitem.fsecondsessionend).TimeOfDay;
                    Extension.CreatedUTC = DateTime.UtcNow;
                    Extension.Day = ssitem.Days;
                    Extension.IsDeleted = false;
                    Extension.MaxFSPatients = Convert.ToInt32(ssitem.Maxpatientsmorning);
                    Extension.MaxSSPatient = Convert.ToInt32(ssitem.Maxpatientecening);
                    CMPSContext.DoctorExtension.AddRange(Extension);
                    CMPSContext.SaveChanges();
                }
            }
            else
            {
                foreach (var item in Doctorvisisting.DoctorGridconsulting)
                {
                    var fulldata = CMPSContext.DoctorExtension.Where(x => x.DoctorID == Convert.ToInt32(item.DoctorID) && x.Day == item.Docday).ToList();
                    if (fulldata.Count != 0)
                    {
                        var deletedata = CMPSContext.DoctorExtension.Where(x => x.DoctorID == Convert.ToInt32(item.DoctorID) && x.Day == item.Docday).ToList();
                        CMPSContext.DoctorExtension.RemoveRange(deletedata);
                        CMPSContext.SaveChanges();
                    }

                    var id = CMPSContext.DoctorExtension.OrderByDescending(x => x.ID).Select(x => x.ID).FirstOrDefault();
                    var Extension = new DoctorExtension();
                    if (id == 0)
                    {
                        Extension.ID = 1;
                    }
                    else
                    {
                        Extension.ID = id + 1;
                    }
                    Extension.CMPID = Convert.ToInt32(Doctorvisisting.CPMID);
                    Extension.CreatedBy = Convert.ToInt32(Doctorvisisting.USERID);
                    Extension.DoctorID = Convert.ToInt32(item.DoctorID);
                    Extension.FSFromTime = Convert.ToDateTime(item.FFtime + ':' + item.FTTime).TimeOfDay;
                    Extension.FSToTime = Convert.ToDateTime(item.SSFtime + ':' + item.SSSTime).TimeOfDay;
                    Extension.SSFromTime = Convert.ToDateTime(item.EveningFFtime + ':' + item.EveningFTTime).TimeOfDay;
                    Extension.SSToTime = Convert.ToDateTime(item.EveningSSFtime + ':' + item.EveningSSSTime).TimeOfDay;
                    Extension.CreatedUTC = DateTime.UtcNow;
                    Extension.Day = item.Docday;
                    Extension.IsDeleted = false;
                    if(item.Maxfpatient != "")
                    {
                        Extension.MaxFSPatients = Convert.ToInt32(item.Maxfpatient);
                    }
                    else
                    {
                        Extension.MaxFSPatients = 0;
                    }
                    if (item.Maxtpatient != "")
                    {
                        Extension.MaxSSPatient = Convert.ToInt32(item.Maxtpatient);
                    }
                    else
                    {
                        Extension.MaxSSPatient = 0;
                    }

                    CMPSContext.DoctorExtension.AddRange(Extension);
                    CMPSContext.SaveChanges();
                }
            }


            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                {
                    return new
                    {
                        Success = true,
                        MessageTemplate = "Saved Successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                MessageTemplate = "Something Wrong",
            };
        }

        public dynamic Getdoctorhours(int CMPID, int Doctorid)
        {
            var data = new DoctorMaster();
            data.DoctorGridconsulting = new List<DoctorGridconsulting>();
            data.Doctorbreakupconsulting = (from ex in CMPSContext.DoctorExtension.OrderByDescending(x => x.CreatedUTC).Where(x => x.DoctorID == Doctorid && x.CMPID == CMPID && x.IsDeleted == false)
                                            select new Doctorbreakupconsulting
                                            {
                                                ExtensionID = ex.ID,
                                                Days = ex.Day,
                                                firstname = CMPSContext.DoctorMaster.Where(x => x.DoctorID == ex.DoctorID && x.CMPID == CMPID).Select(x => x.Firstname).FirstOrDefault(),
                                                Lastname = CMPSContext.DoctorMaster.Where(x => x.DoctorID == ex.DoctorID && x.CMPID == CMPID).Select(x => x.LastName).FirstOrDefault(),
                                                DoctorID = ex.DoctorID,
                                                firstsessionstart = ex.FSFromTime.ToString(@"hh\:mm"),
                                                firstsessionend = ex.FSToTime.ToString(@"hh\:mm"),
                                                Secondsessionstart = ex.SSFromTime.ToString(@"hh\:mm"),
                                                fsecondsessionend = ex.SSToTime.ToString(@"hh\:mm"),
                                                Maxpatientsmorning = ex.MaxFSPatients,
                                                Maxpatientecening = ex.MaxSSPatient,
                                            }).ToList();
            return data;
        }

        public dynamic DeleteDoctorpastdata(int CMPID, int DOCID, int EXTENSIONID)
        {
            var data = new DoctorMaster();
            var Custid = CMPSContext.DoctorExtension.Where(x => x.ID == EXTENSIONID && x.CMPID == CMPID && x.DoctorID == DOCID).FirstOrDefault();
            Custid.IsDeleted = true;
            CMPSContext.DoctorExtension.UpdateRange(Custid);
            CMPSContext.SaveChanges();

            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                {
                    return new
                    {
                        success = true,
                    };
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                success = false,
            };


        }

        public dynamic AddEngage(DoctorMaster EngagementType)
        {
            try
            {
                var ole = new OneLine_Masters();

                ole.ParentDescription = EngagementType.Engages.EngagementType;
                ole.ParentTag = "ET";
                ole.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "ET").Select(x => x.ParentID).FirstOrDefault();
                ole.CreatedBy = EngagementType.Engages.USERID;
                ole.IsActive = true;
                ole.IsDeleted = false;
                ole.CreatedUTC = DateTime.UtcNow;
                CMPSContext.OneLineMaster.Add(ole);
                CMPSContext.SaveChanges();

                return new
                {
                    Success = true,
                    Message = "saved successfully"
                };

            }
            catch (Exception)
            {
                return new
                {
                    success = false,
                };
            }
        }


        public dynamic Getalldoctorssearch(string docid)
        {
            var data = new doctorsearchviemodel();
            var LocationMaster = CMPSContext.LocationMaster.ToList();
            data.Doctorsearchpatients = (from doc in CMPSContext.DoctorMaster.Where(x => x.CMPID == Convert.ToInt32(docid) && x.IsDeleted == false)
                                         join ds in CMPSContext.DoctorSpeciality on doc.DoctorID equals ds.DoctorID
                                         join om in CMPSContext.OneLineMaster on ds.OLMID equals om.OLMID

                                         select new Doctorsearchpatients
                                         {
                                             DoctorID = doc.DoctorID,
                                             DoctorName = String.Concat(doc.Firstname + " " + (doc.MiddleName != null ? doc.MiddleName : " ") + " " + doc.LastName),
                                             FirstName = doc.Firstname,
                                             MiddleName = doc.MiddleName,
                                             LastName = doc.LastName,
                                             Title = doc.Title,
                                             DateofBirth = doc.DateofBirth,
                                             Gender = doc.Gender,
                                             LocationID = doc.LocationID != null ? doc.LocationID.ToString() : null,
                                             City = doc.LocationID != null ? Convert.ToString(LocationMaster.Where(x => x.ID == Convert.ToInt16(doc.LocationID)).Select(x => x.ParentID).FirstOrDefault()) : null,
                                             Address1 = doc.Address1,
                                             Address2 = doc.Address2,
                                             Address3 = doc.Address3,
                                             Designation = doc.Designation,
                                             RegistrationNumber = doc.RegistrationNumber,
                                             EngagementType = CMPSContext.OneLineMaster.Where(x => x.OLMID == Convert.ToInt64(doc.EngagementType)).Select(x => x.ParentDescription).FirstOrDefault(),
                                             Phone1 = doc.Phone1,
                                             Phone2 = doc.Phone2,
                                             EmailID = doc.EmailID,
                                             Speciality = CMPSContext.OneLineMaster.Where(x => x.OLMID == ds.OLMID).Select(x => x.ParentDescription).FirstOrDefault(),
                                             DoctorSpecialityID = CMPSContext.DoctorSpeciality.Where(x => x.OLMID == ds.OLMID && x.DoctorID == doc.DoctorID).Select(x => x.DoctorSpecialityID).FirstOrDefault(),
                                             Status = doc.IsActive,
                                         }
                ).Distinct().ToList();
            return data;
        }
     

    }
}














