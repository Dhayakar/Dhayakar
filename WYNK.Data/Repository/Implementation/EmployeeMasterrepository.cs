using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Data.Repository.Implementation
{
    class EmployeeMasterrepository : RepositoryBase<Employee_master>, IEmployeeMasterrepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
       

        public EmployeeMasterrepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;
            
        }



        public dynamic AddEmployee(Employee_master EmployeeAdd)
        {

            var emp = new Employee_Master();




            try
            {
                if (EmployeeAdd.employeeMaster.AadhaarNo != null)
                {
                    var Aadhar = Convert.ToInt32(CMPSContext.Employee.Where(x => x.AadhaarNo == EmployeeAdd.employeeMaster.AadhaarNo).Select(x => x.EmployeeID).FirstOrDefault());

                    if (Aadhar != 0)
                    {
                        return new
                        {
                            Success = false,
                            Message = "Aadhar Number already Exists"
                        };

                    }
                }

                if (EmployeeAdd.employeeMaster.PANNo != null)
                {

                    var Pan = Convert.ToInt32(CMPSContext.Employee.Where(x => x.PANNo == EmployeeAdd.employeeMaster.PANNo).Select(x => x.EmployeeID).FirstOrDefault());

                    if (Pan != 0)
                    {
                        return new
                        {
                            Success = false,
                            Message = "Pan Number already Exists"
                        };
                    }


                }


                emp.Title = EmployeeAdd.employeeMaster.Title;
                emp.FirstName = EmployeeAdd.employeeMaster.FirstName;
                emp.MiddleName = EmployeeAdd.employeeMaster.MiddleName;
                emp.LastName = EmployeeAdd.employeeMaster.LastName;
                emp.Dateofbirth = EmployeeAdd.employeeMaster.Dateofbirth.AddDays(1);
                emp.Gender = EmployeeAdd.employeeMaster.Gender;
                emp.PhysicallyChallenged = EmployeeAdd.employeeMaster.PhysicallyChallenged;
                emp.MaritalStatus = EmployeeAdd.employeeMaster.MaritalStatus;
                emp.Category = EmployeeAdd.employeeMaster.Category;
                emp.DeptCode = EmployeeAdd.employeeMaster.DeptCode;
                emp.AadhaarNo = EmployeeAdd.employeeMaster.AadhaarNo;
                emp.PANNo = EmployeeAdd.employeeMaster.PANNo;
                emp.FatherHusbandName = EmployeeAdd.employeeMaster.FatherHusbandName;
                emp.DOJ = EmployeeAdd.employeeMaster.DOJ.AddDays(1);
                emp.Designation = EmployeeAdd.employeeMaster.Designation;
                emp.EmergencyContactName = EmployeeAdd.employeeMaster.EmergencyContactName;
                emp.EmergencyContactNo = EmployeeAdd.employeeMaster.EmergencyContactNo;
                emp.BloodGroup = EmployeeAdd.employeeMaster.BloodGroup;
                emp.IsActive = true;
                emp.CMPID = EmployeeAdd.employeeMaster.CMPID;
                emp.CreatedUTC = DateTime.UtcNow;
                emp.CreatedBy = EmployeeAdd.employeeMaster.CreatedBy;
                CMPSContext.Employee.Add(emp);
                CMPSContext.SaveChanges();


                var empcomm = new EmployeeCommunication();

                int empid2 = emp.EmployeeID;
                empcomm.EmpID = empid2;
                empcomm.CmpID = EmployeeAdd.EmployeeCommunication.CmpID;
                empcomm.PresentAddress1 = EmployeeAdd.EmployeeCommunication.PresentAddress1;
                empcomm.PresentAddress2 = EmployeeAdd.EmployeeCommunication.PresentAddress2;
                empcomm.PresentLocationID = EmployeeAdd.EmployeeCommunication.PresentLocationID;
                empcomm.PresentLandmark = EmployeeAdd.EmployeeCommunication.PresentLandmark;
                empcomm.PermanentAddress1 = EmployeeAdd.EmployeeCommunication.PermanentAddress1;
                empcomm.PermanentAddress2 = EmployeeAdd.EmployeeCommunication.PermanentAddress2;
                empcomm.PermanentLocationID = EmployeeAdd.EmployeeCommunication.PermanentLocationID;
                empcomm.PermanentLandmark = EmployeeAdd.EmployeeCommunication.PermanentLandmark;
                empcomm.Phone = EmployeeAdd.EmployeeCommunication.Phone;
                empcomm.MobileNumber1 = EmployeeAdd.EmployeeCommunication.MobileNumber1;
                empcomm.MobileNumber2 = EmployeeAdd.EmployeeCommunication.MobileNumber2;
                empcomm.EmailID = EmployeeAdd.EmployeeCommunication.EmailID;
                empcomm.AlternateEmailID = EmployeeAdd.EmployeeCommunication.AlternateEmailID;
                empcomm.CreatedBy = emp.CreatedBy;
                empcomm.CreatedUTC = DateTime.UtcNow;
                CMPSContext.EmployeeCommunication.Add(empcomm);
                CMPSContext.SaveChanges();

                int EmpID = CMPSContext.Employee.OrderByDescending(x => x.EmployeeID).Select(x => x.EmployeeID).FirstOrDefault();

                var empstat = new EmployeeStatutory();
                int empid = emp.EmployeeID;
                empstat.EmpID = empid;
                empstat.CmpID = EmployeeAdd.EmployeeStatutory.CmpID;
                empstat.IsPFEligible = EmployeeAdd.EmployeeStatutory.IsPFEligible;
                empstat.PFNumber = EmployeeAdd.EmployeeStatutory.PFNumber;
                if (EmployeeAdd.EmployeeStatutory.PFCommencementDate != null)
                {
                    empstat.PFCommencementDate = EmployeeAdd.EmployeeStatutory.PFCommencementDate.Value.AddDays(1);
                }
                empstat.UANNo = EmployeeAdd.EmployeeStatutory.UANNo;
                empstat.VPFPercentage = EmployeeAdd.EmployeeStatutory.VPFPercentage;
                empstat.IsESIEligible = EmployeeAdd.EmployeeStatutory.IsESIEligible;
                empstat.ESINumber = EmployeeAdd.EmployeeStatutory.ESINumber;
                if (EmployeeAdd.EmployeeStatutory.ESICommencementDate != null)
                {
                    empstat.ESICommencementDate = EmployeeAdd.EmployeeStatutory.ESICommencementDate.Value.AddDays(1);
                }
                empstat.IsProfessionalTaxEligible = EmployeeAdd.EmployeeStatutory.IsProfessionalTaxEligible;
                empstat.GratuityNo = EmployeeAdd.EmployeeStatutory.GratuityNo;
                empstat.IsDeleted = false;
                empstat.CreatedBy = emp.CreatedBy;
                empstat.CreatedUTC = DateTime.UtcNow;
                CMPSContext.EmployeeStatutory.Add(empstat);
                CMPSContext.SaveChanges();

                if (EmployeeAdd.EmployeeBank.AccountHoldersName != null)
                {
                    var empbank = new EmployeeBank();

                    int empid1 = emp.EmployeeID;
                    empbank.EmpID = empid1;
                    empbank.CmpID = EmployeeAdd.EmployeeBank.CmpID;
                    empbank.BankName = EmployeeAdd.EmployeeBank.BankName;
                    empbank.BankBranch = EmployeeAdd.EmployeeBank.BankBranch;
                    empbank.AccountHoldersName = EmployeeAdd.EmployeeBank.AccountHoldersName;
                    empbank.AccountType = EmployeeAdd.EmployeeBank.AccountType;
                    empbank.AccountNo = EmployeeAdd.EmployeeBank.AccountNo;
                    empbank.IFSCCode = EmployeeAdd.EmployeeBank.IFSCCode;
                    empbank.IsDeleted = false;
                    empbank.CreatedBy = emp.CreatedBy;
                    empbank.CreatedUTC = DateTime.UtcNow;
                    CMPSContext.EmployeeBank.Add(empbank);
                    CMPSContext.SaveChanges();
                }

                if (EmployeeAdd.EmployeeExperience != null)
                {
                    foreach (var item in EmployeeAdd.EmployeeExperience.ToList())

                    {

                        var empexp = new EmployeeExperience();

                        int empid4 = emp.EmployeeID;
                        empexp.EmpID = empid4;
                        empexp.Organisation = item.Organisation;
                        empexp.FromDate = item.FromDate.AddDays(1);
                        empexp.ToDate = item.ToDate.AddDays(1);
                        empexp.Designation = item.Designation;
                        empexp.CreatedBy = emp.CreatedBy;
                        empexp.CreatedUTC = DateTime.UtcNow;
                        WYNKContext.Experience.AddRange(empexp);
                        WYNKContext.SaveChanges();

                    }
                }

                if (EmployeeAdd.empqua != null)
                {
                    foreach (var item in EmployeeAdd.empqua.ToList())

                    {

                        var equa = new EmployeeQualification();

                        equa.EmpID = emp.EmployeeID;
                        equa.Cmpid = emp.CMPID;

                        if (equa.FromDate != null)
                        {
                            equa.FromDate = item.fromdate.AddDays(1);
                        }
                        if (equa.ToDate != null)
                        {
                            equa.ToDate = item.todate.AddDays(1);
                        }
                        equa.YearofPass = item.yearofpassing;
                        equa.QualExtCode = item.qid;
                        equa.UniversityExtCode = item.uid;
                        equa.IsDeleted = false;
                        equa.Percentage = item.percentageofmarks;
                        equa.CreatedBy = emp.CreatedBy;
                        equa.CreatedUTC = DateTime.UtcNow;
                        WYNKContext.EmployeeQualification.AddRange(equa);
                        WYNKContext.SaveChanges();

                    }
                }


                if (CMPSContext.SaveChanges() >= 0 || WYNKContext.SaveChanges() >= 0)

                    return new
                    {
                        Success = true,
                        Message = "Saved successfully",
                        Empid = emp.EmployeeID,
                    };
            }

            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = "Something Went Wrong"
            };


        }

        public dynamic UploadImage(IFormFile file, int Empid)
        {

            var currentDir = Directory.GetCurrentDirectory();
            if (!Directory.Exists(currentDir + "/EmployeeImages/"))
                Directory.CreateDirectory(currentDir + "/EmployeeImages/");
            var fileName = $"{Empid}{Path.GetExtension(file.FileName)}";
            var path = $"{currentDir}/EmployeeImages/{fileName}";

            if ((File.Exists(path)))
                File.Delete(path);


            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
                var photo = CMPSContext.Employee.Where(x => x.EmployeeID == Empid).FirstOrDefault();
                photo.PhotoPath = fileName;

                CMPSContext.Entry(photo).State = EntityState.Modified;

                var tt = CMPSContext.SaveChanges() > 0;
                return tt;

            }

        }

        public dynamic Getemployeeimage(int ID)
        {

            var reg = new RegistrationMasterViewModel();

            var regs = CMPSContext.Employee.Where(x => x.EmployeeID == ID).Select(x => x.PhotoPath).FirstOrDefault();

            if (regs != null)
            {
                string path = "F:/13feb2020/WYNK.Services/EmployeeImages/" + regs;
                string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                string source = imageData;
                string base64 = source.Substring(source.IndexOf(',') + 1);
                byte[] data = Convert.FromBase64String(base64);
                reg.ProductImage = imageData;
            }
            else
            {

            }
            return reg;


        }

        public dynamic DeleteEmployee(int? EmployeeID)
        {


            var cmb = CMPSContext.Employee.Where(x => x.EmployeeID == EmployeeID).ToList();
            if (cmb != null)
            {
                cmb.All(x => { x.IsDeleted = true; x.IsActive = false; return true; });
                CMPSContext.Employee.UpdateRange(cmb);

            }

            var userid = CMPSContext.Users.Where(x => x.ReferenceID == EmployeeID).Select(x => x.Userid).FirstOrDefault();

            if (userid != 0)
            {
                var userids = CMPSContext.Users.Where(x => x.ReferenceID == EmployeeID).FirstOrDefault();
                userids.Isactive = false;
                CMPSContext.Users.UpdateRange(userids);
            }



            var usersvsroleID = CMPSContext.UserVsRole.Where(x => x.UserID == userid).ToList();

            if (usersvsroleID != null)
            {
                usersvsroleID.All(x => { x.IsDeleted = true; return true; });
                CMPSContext.UserVsRole.UpdateRange(usersvsroleID);
            }


            var empstatut = CMPSContext.EmployeeStatutory.Where(x => x.EmpID == EmployeeID).FirstOrDefault();
            if (empstatut != null)
            {
                empstatut.IsDeleted = true;
                CMPSContext.EmployeeStatutory.UpdateRange(empstatut);

            }

            var empban = CMPSContext.EmployeeBank.Where(x => x.EmpID == EmployeeID).FirstOrDefault();
            if (empban != null)
            {
                empban.IsDeleted = true;
                CMPSContext.EmployeeBank.UpdateRange(empban);

            }

            var empcommuni = CMPSContext.EmployeeCommunication.Where(x => x.EmpID == EmployeeID).FirstOrDefault();
            if (empcommuni != null)
            {
                empcommuni.IsDeleted = true;
                CMPSContext.EmployeeCommunication.UpdateRange(empcommuni);

            }

            var empexp = WYNKContext.Experience.Where(x => x.EmpID == EmployeeID).FirstOrDefault();
            if (empexp != null)
            {
                empexp.IsDeleted = true;
                WYNKContext.Experience.UpdateRange(empexp);

            }

            var empqualif = WYNKContext.EmployeeQualification.Where(x => x.EmpID == EmployeeID).FirstOrDefault();
            if (empqualif != null)
            {
                empqualif.IsDeleted = true;
                WYNKContext.EmployeeQualification.UpdateRange(empqualif);

            }

            try
            {
                if (CMPSContext.SaveChanges() >= 0 || WYNKContext.SaveChanges() >= 0)
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
                Message = "Some data are Missing"
            };
        }
        public dynamic UpdateEmployee(Employee_master EmployeeMaster, int? EmployeeID)
        {
            var emp = new Employee_Master();

            var Aanumber = CMPSContext.Employee.Where(x => x.EmployeeID == EmployeeID && x.AadhaarNo == EmployeeMaster.employeeMaster.AadhaarNo && x.PANNo == EmployeeMaster.employeeMaster.PANNo).ToList();

            try
            {

                if (Aanumber.Count == 1)
                {

                    emp = CMPSContext.Employee.Where(x => x.EmployeeID == EmployeeID).FirstOrDefault();

                    emp.Title = EmployeeMaster.employeeMaster.Title;
                    emp.FirstName = EmployeeMaster.employeeMaster.FirstName;
                    emp.MiddleName = EmployeeMaster.employeeMaster.MiddleName;
                    emp.LastName = EmployeeMaster.employeeMaster.LastName;
                    if (emp.Dateofbirth != EmployeeMaster.employeeMaster.Dateofbirth)
                    {
                        emp.Dateofbirth = EmployeeMaster.employeeMaster.Dateofbirth.AddDays(1);
                    }

                    emp.Gender = EmployeeMaster.employeeMaster.Gender;
                    emp.PhysicallyChallenged = EmployeeMaster.employeeMaster.PhysicallyChallenged;
                    emp.MaritalStatus = EmployeeMaster.employeeMaster.MaritalStatus;
                    emp.Category = EmployeeMaster.employeeMaster.Category;
                    emp.DeptCode = EmployeeMaster.employeeMaster.DeptCode;
                    emp.AadhaarNo = EmployeeMaster.employeeMaster.AadhaarNo;
                    emp.PANNo = EmployeeMaster.employeeMaster.PANNo;
                    emp.FatherHusbandName = EmployeeMaster.employeeMaster.FatherHusbandName;
                    emp.ReasonForResignation = EmployeeMaster.employeeMaster.ReasonForResignation;

                    if (EmployeeMaster.employeeMaster.DOR != null)
                    {
                        emp.DOR = EmployeeMaster.employeeMaster.DOR.Value.AddDays(1);
                    }
                    emp.Designation = EmployeeMaster.employeeMaster.Designation;
                    emp.EmergencyContactName = EmployeeMaster.employeeMaster.EmergencyContactName;
                    emp.EmergencyContactNo = EmployeeMaster.employeeMaster.EmergencyContactNo;
                    emp.BloodGroup = EmployeeMaster.employeeMaster.BloodGroup;
                    emp.IsActive = EmployeeMaster.employeeMaster.IsActive;
                    emp.UpdatedUTC = DateTime.UtcNow;
                    emp.UpdatedBy = EmployeeMaster.employeeMaster.UpdatedBy;
                    CMPSContext.Employee.UpdateRange(emp);
                    CMPSContext.SaveChanges();

                    var empcommu = new EmployeeCommunication();

                    empcommu = CMPSContext.EmployeeCommunication.Where(x => x.EmpID == EmployeeID).FirstOrDefault();

                    empcommu.PresentAddress1 = EmployeeMaster.EmployeeCommunication.PresentAddress1;
                    empcommu.PresentAddress2 = EmployeeMaster.EmployeeCommunication.PresentAddress2;
                    empcommu.PresentLocationID = EmployeeMaster.EmployeeCommunication.PresentLocationID;
                    empcommu.PresentLandmark = EmployeeMaster.EmployeeCommunication.PresentLandmark;
                    empcommu.PermanentAddress1 = EmployeeMaster.EmployeeCommunication.PermanentAddress1;
                    empcommu.PermanentAddress2 = EmployeeMaster.EmployeeCommunication.PermanentAddress2;
                    empcommu.PermanentLocationID = EmployeeMaster.EmployeeCommunication.PermanentLocationID;
                    empcommu.PermanentLandmark = EmployeeMaster.EmployeeCommunication.PermanentLandmark;
                    empcommu.Phone = EmployeeMaster.EmployeeCommunication.Phone;
                    empcommu.MobileNumber1 = EmployeeMaster.EmployeeCommunication.MobileNumber1;
                    empcommu.MobileNumber2 = EmployeeMaster.EmployeeCommunication.MobileNumber2;
                    empcommu.EmailID = EmployeeMaster.EmployeeCommunication.EmailID;
                    empcommu.AlternateEmailID = EmployeeMaster.EmployeeCommunication.AlternateEmailID;
                    empcommu.UpdatedBy = emp.UpdatedBy;
                    empcommu.UpdatedUTC = DateTime.UtcNow;
                    CMPSContext.EmployeeCommunication.UpdateRange(empcommu);
                    CMPSContext.SaveChanges();


                    var empst = new EmployeeStatutory();

                    empst = CMPSContext.EmployeeStatutory.Where(x => x.EmpID == EmployeeID).FirstOrDefault();


                    empst.IsPFEligible = EmployeeMaster.EmployeeStatutory.IsPFEligible;
                    empst.PFNumber = EmployeeMaster.EmployeeStatutory.PFNumber;

                    if (empst.PFCommencementDate != EmployeeMaster.EmployeeStatutory.PFCommencementDate)
                    {
                        empst.PFCommencementDate = EmployeeMaster.EmployeeStatutory.PFCommencementDate.Value.AddDays(1);
                    }

                    empst.UANNo = EmployeeMaster.EmployeeStatutory.UANNo;
                    empst.VPFPercentage = EmployeeMaster.EmployeeStatutory.VPFPercentage;
                    empst.IsESIEligible = EmployeeMaster.EmployeeStatutory.IsESIEligible;
                    empst.ESINumber = EmployeeMaster.EmployeeStatutory.ESINumber;
                    if (empst.ESICommencementDate != EmployeeMaster.EmployeeStatutory.ESICommencementDate)
                    {
                        empst.ESICommencementDate = EmployeeMaster.EmployeeStatutory.ESICommencementDate.Value.AddDays(1);
                    }
                    empst.IsProfessionalTaxEligible = EmployeeMaster.EmployeeStatutory.IsProfessionalTaxEligible;
                    empst.GratuityNo = EmployeeMaster.EmployeeStatutory.GratuityNo;
                    empst.CmpID = EmployeeMaster.EmployeeStatutory.CmpID;
                    empst.EmpID = emp.EmployeeID;
                    empst.UpdatedBy = emp.UpdatedBy;
                    empst.UpdatedUTC = DateTime.UtcNow;
                    CMPSContext.EmployeeStatutory.UpdateRange(empst);
                    CMPSContext.SaveChanges();


                    var v = CMPSContext.EmployeeBank.Where(x => x.EmpID == EmployeeID).LastOrDefault();
                    if (v != null)
                    {
                        if (v.IsDeleted == false)
                        {
                            var empba = new EmployeeBank();

                            empba = CMPSContext.EmployeeBank.Where(x => x.EmpID == EmployeeID).LastOrDefault();

                            empba.BankName = EmployeeMaster.EmployeeBank.BankName;
                            empba.BankBranch = EmployeeMaster.EmployeeBank.BankBranch;
                            empba.AccountHoldersName = EmployeeMaster.EmployeeBank.AccountHoldersName;
                            empba.AccountType = EmployeeMaster.EmployeeBank.AccountType;
                            empba.AccountNo = EmployeeMaster.EmployeeBank.AccountNo;
                            empba.IFSCCode = EmployeeMaster.EmployeeBank.IFSCCode;
                            empba.CmpID = EmployeeMaster.EmployeeBank.CmpID;
                            empba.EmpID = emp.EmployeeID;
                            empba.UpdatedBy = emp.UpdatedBy;
                            empba.UpdatedUTC = DateTime.UtcNow;
                            CMPSContext.EmployeeBank.UpdateRange(empba);
                            CMPSContext.SaveChanges();
                        }
                    }


                    var vs = CMPSContext.EmployeeBank.Where(x => x.EmpID == EmployeeID).LastOrDefault();

                    if (vs != null)
                    {
                        if (vs.IsDeleted != false)
                        {
                            var empbankup = new EmployeeBank();

                            int empidd = vs.EmpID;
                            empbankup.EmpID = empidd;
                            empbankup.CmpID = EmployeeMaster.EmployeeBank.CmpID;
                            empbankup.BankName = EmployeeMaster.EmployeeBank.BankName;
                            empbankup.BankBranch = EmployeeMaster.EmployeeBank.BankBranch;
                            empbankup.AccountHoldersName = EmployeeMaster.EmployeeBank.AccountHoldersName;
                            empbankup.AccountType = EmployeeMaster.EmployeeBank.AccountType;
                            empbankup.AccountNo = EmployeeMaster.EmployeeBank.AccountNo;
                            empbankup.IFSCCode = EmployeeMaster.EmployeeBank.IFSCCode;
                            empbankup.IsDeleted = false;
                            empbankup.CreatedBy = EmployeeMaster.EmployeeBank.CreatedBy;
                            CMPSContext.EmployeeBank.Add(empbankup);
                            CMPSContext.SaveChanges();
                        }
                    }



                    var vv = CMPSContext.EmployeeBank.Where(x => x.EmpID == EmployeeID).LastOrDefault();

                    if (vv == null)
                    {
                        if (EmployeeMaster.EmployeeBank.AccountType != null)
                        {
                            var empbankup = new EmployeeBank();

                            empbankup.EmpID = emp.EmployeeID;
                            empbankup.CmpID = EmployeeMaster.EmployeeBank.CmpID;
                            empbankup.BankName = EmployeeMaster.EmployeeBank.BankName;
                            empbankup.BankBranch = EmployeeMaster.EmployeeBank.BankBranch;
                            empbankup.AccountHoldersName = EmployeeMaster.EmployeeBank.AccountHoldersName;
                            empbankup.AccountType = EmployeeMaster.EmployeeBank.AccountType;
                            empbankup.AccountNo = EmployeeMaster.EmployeeBank.AccountNo;
                            empbankup.IFSCCode = EmployeeMaster.EmployeeBank.IFSCCode;
                            empbankup.IsDeleted = false;
                            empbankup.CreatedBy = EmployeeMaster.EmployeeBank.CreatedBy;
                            CMPSContext.EmployeeBank.Add(empbankup);
                            CMPSContext.SaveChanges();
                        }


                    }

                    if (EmployeeMaster.EmployeeExperience != null)

                    {

                        var ids = WYNKContext.Experience.Where(x => x.EmpID == EmployeeID).ToList();

                        foreach (var item in ids.ToList())
                        {


                            if (ids != null && ids.Count != 0)

                            {

                                ids.All(x => { x.EmpID = item.EmpID; return true; });
                                var idsss = WYNKContext.Experience.Where(x => x.EmpID == EmployeeID).ToList();
                                if (idsss.Count != 0)
                                {
                                    WYNKContext.Experience.RemoveRange(ids);
                                    WYNKContext.SaveChanges();
                                }

                            }
                        }

                        var ids1 = WYNKContext.Experience.Where(x => x.EmpID == EmployeeID).ToList();

                        if (ids1.Count == 0)
                        {
                            foreach (var item in EmployeeMaster.EmployeeExperience.ToList())
                            {
                                var S = new EmployeeExperience();

                                int empid4 = emp.EmployeeID;
                                S.EmpID = empid4;
                                S.Organisation = item.Organisation;
                                S.FromDate = item.FromDate.AddDays(1);
                                S.ToDate = item.ToDate.AddDays(1);
                                S.Designation = item.Designation;
                                S.CreatedBy = item.CreatedBy;
                                WYNKContext.Experience.AddRange(S);
                                WYNKContext.SaveChanges();
                            }
                        }
                    }


                    if (EmployeeMaster.empqua1 != null)
                    {
                        var idss = WYNKContext.EmployeeQualification.Where(x => x.EmpID == EmployeeID).ToList();

                        foreach (var item in idss.ToList())

                        {

                            if (idss != null && idss.Count != 0)
                            {

                                idss.All(x => { x.EmpID = item.EmpID; return true; });
                                var idssss = WYNKContext.EmployeeQualification.Where(u => u.EmpID == EmployeeID).ToList();
                                if (idssss.Count != 0)
                                {
                                    WYNKContext.EmployeeQualification.RemoveRange(idssss);
                                    WYNKContext.SaveChanges();
                                }


                            }
                        }

                        var ids2 = WYNKContext.EmployeeQualification.Where(x => x.EmpID == EmployeeID).ToList();

                        if (ids2.Count == 0)
                        {
                            foreach (var item in EmployeeMaster.empqua1.ToList())
                            {

                                var SS = new EmployeeQualification();

                                SS.EmpID = emp.EmployeeID;
                                SS.Cmpid = emp.CMPID;
                                if (SS.FromDate != null)
                                {
                                    SS.FromDate = item.fromdate.AddDays(1);
                                }
                                if (SS.ToDate != null)
                                {
                                    SS.ToDate = item.todate.AddDays(1);
                                }
                                SS.YearofPass = item.yearofpassing;
                                SS.QualExtCode = item.qid;
                                SS.UniversityExtCode = item.uid;
                                SS.IsDeleted = false;
                                SS.Percentage = item.percentageofmarks;
                                SS.CreatedBy = item.createby;
                                SS.CreatedUTC = DateTime.UtcNow;
                                WYNKContext.EmployeeQualification.AddRange(SS);
                                WYNKContext.SaveChanges();
                            }
                        }



                    }

                }


                else
                {
                    var res = Convert.ToInt32(CMPSContext.Employee.Where(x => x.AadhaarNo == EmployeeMaster.employeeMaster.AadhaarNo && x.EmployeeID != EmployeeID).Select(x => x.EmployeeID).FirstOrDefault());
                    if (res != 0)
                    {
                        return new
                        {
                            Success = false,
                            Message = "Aadhar Number Already Exist"
                        };

                    }
                    else
                    {
                        var Aanumbno = Convert.ToInt32(CMPSContext.Employee.Where(x => x.PANNo == EmployeeMaster.employeeMaster.PANNo && x.EmployeeID != EmployeeID).Select(x => x.EmployeeID).FirstOrDefault());

                        if (Aanumbno != 0)
                        {
                            return new
                            {
                                Success = false,
                                Message = "Pan Number Already Exist"
                            };
                        }


                        else
                        {

                            emp = CMPSContext.Employee.Where(x => x.EmployeeID == EmployeeID).FirstOrDefault();

                            emp.Title = EmployeeMaster.employeeMaster.Title;
                            emp.FirstName = EmployeeMaster.employeeMaster.FirstName;
                            emp.MiddleName = EmployeeMaster.employeeMaster.MiddleName;
                            emp.LastName = EmployeeMaster.employeeMaster.LastName;
                            if (emp.Dateofbirth != EmployeeMaster.employeeMaster.Dateofbirth)
                            {
                                emp.Dateofbirth = EmployeeMaster.employeeMaster.Dateofbirth.AddDays(1);
                            }

                            emp.Gender = EmployeeMaster.employeeMaster.Gender;
                            emp.PhysicallyChallenged = EmployeeMaster.employeeMaster.PhysicallyChallenged;
                            emp.MaritalStatus = EmployeeMaster.employeeMaster.MaritalStatus;
                            emp.Category = EmployeeMaster.employeeMaster.Category;
                            emp.DeptCode = EmployeeMaster.employeeMaster.DeptCode;
                            emp.AadhaarNo = EmployeeMaster.employeeMaster.AadhaarNo;
                            emp.PANNo = EmployeeMaster.employeeMaster.PANNo;
                            emp.FatherHusbandName = EmployeeMaster.employeeMaster.FatherHusbandName;
                            emp.ReasonForResignation = EmployeeMaster.employeeMaster.ReasonForResignation;

                            if (EmployeeMaster.employeeMaster.DOR != null)
                            {
                                emp.DOR = EmployeeMaster.employeeMaster.DOR.Value.AddDays(1);
                            }
                            emp.Designation = EmployeeMaster.employeeMaster.Designation;
                            emp.EmergencyContactName = EmployeeMaster.employeeMaster.EmergencyContactName;
                            emp.EmergencyContactNo = EmployeeMaster.employeeMaster.EmergencyContactNo;
                            emp.BloodGroup = EmployeeMaster.employeeMaster.BloodGroup;
                            emp.IsActive = EmployeeMaster.employeeMaster.IsActive;
                            emp.UpdatedUTC = DateTime.UtcNow;
                            emp.UpdatedBy = EmployeeMaster.employeeMaster.UpdatedBy;
                            CMPSContext.Employee.UpdateRange(emp);
                            CMPSContext.SaveChanges();

                            var empcommu = new EmployeeCommunication();

                            empcommu = CMPSContext.EmployeeCommunication.Where(x => x.EmpID == EmployeeID).FirstOrDefault();

                            empcommu.PresentAddress1 = EmployeeMaster.EmployeeCommunication.PresentAddress1;
                            empcommu.PresentAddress2 = EmployeeMaster.EmployeeCommunication.PresentAddress2;
                            empcommu.PresentLocationID = EmployeeMaster.EmployeeCommunication.PresentLocationID;
                            empcommu.PresentLandmark = EmployeeMaster.EmployeeCommunication.PresentLandmark;
                            empcommu.PermanentAddress1 = EmployeeMaster.EmployeeCommunication.PermanentAddress1;
                            empcommu.PermanentAddress2 = EmployeeMaster.EmployeeCommunication.PermanentAddress2;
                            empcommu.PermanentLocationID = EmployeeMaster.EmployeeCommunication.PermanentLocationID;
                            empcommu.PermanentLandmark = EmployeeMaster.EmployeeCommunication.PermanentLandmark;
                            empcommu.Phone = EmployeeMaster.EmployeeCommunication.Phone;
                            empcommu.MobileNumber1 = EmployeeMaster.EmployeeCommunication.MobileNumber1;
                            empcommu.MobileNumber2 = EmployeeMaster.EmployeeCommunication.MobileNumber2;
                            empcommu.EmailID = EmployeeMaster.EmployeeCommunication.EmailID;
                            empcommu.AlternateEmailID = EmployeeMaster.EmployeeCommunication.AlternateEmailID;
                            empcommu.UpdatedBy = emp.UpdatedBy;
                            empcommu.UpdatedUTC = DateTime.UtcNow;
                            CMPSContext.EmployeeCommunication.UpdateRange(empcommu);
                            CMPSContext.SaveChanges();


                            var empst = new EmployeeStatutory();

                            empst = CMPSContext.EmployeeStatutory.Where(x => x.EmpID == EmployeeID).FirstOrDefault();


                            empst.IsPFEligible = EmployeeMaster.EmployeeStatutory.IsPFEligible;
                            empst.PFNumber = EmployeeMaster.EmployeeStatutory.PFNumber;

                            if (empst.PFCommencementDate != EmployeeMaster.EmployeeStatutory.PFCommencementDate)
                            {
                                empst.PFCommencementDate = EmployeeMaster.EmployeeStatutory.PFCommencementDate.Value.AddDays(1);
                            }

                            empst.UANNo = EmployeeMaster.EmployeeStatutory.UANNo;
                            empst.VPFPercentage = EmployeeMaster.EmployeeStatutory.VPFPercentage;
                            empst.IsESIEligible = EmployeeMaster.EmployeeStatutory.IsESIEligible;
                            empst.ESINumber = EmployeeMaster.EmployeeStatutory.ESINumber;
                            if (empst.ESICommencementDate != EmployeeMaster.EmployeeStatutory.ESICommencementDate)
                            {
                                empst.ESICommencementDate = EmployeeMaster.EmployeeStatutory.ESICommencementDate.Value.AddDays(1);
                            }
                            empst.IsProfessionalTaxEligible = EmployeeMaster.EmployeeStatutory.IsProfessionalTaxEligible;
                            empst.GratuityNo = EmployeeMaster.EmployeeStatutory.GratuityNo;
                            empst.CmpID = EmployeeMaster.EmployeeStatutory.CmpID;
                            empst.EmpID = emp.EmployeeID;
                            empst.UpdatedBy = emp.UpdatedBy;
                            empst.UpdatedUTC = DateTime.UtcNow;
                            CMPSContext.EmployeeStatutory.UpdateRange(empst);
                            CMPSContext.SaveChanges();


                            var v = CMPSContext.EmployeeBank.Where(x => x.EmpID == EmployeeID).LastOrDefault();
                            if (v != null)
                            {
                                if (v.IsDeleted == false)
                                {
                                    var empba = new EmployeeBank();

                                    empba = CMPSContext.EmployeeBank.Where(x => x.EmpID == EmployeeID).LastOrDefault();

                                    empba.BankName = EmployeeMaster.EmployeeBank.BankName;
                                    empba.BankBranch = EmployeeMaster.EmployeeBank.BankBranch;
                                    empba.AccountHoldersName = EmployeeMaster.EmployeeBank.AccountHoldersName;
                                    empba.AccountType = EmployeeMaster.EmployeeBank.AccountType;
                                    empba.AccountNo = EmployeeMaster.EmployeeBank.AccountNo;
                                    empba.IFSCCode = EmployeeMaster.EmployeeBank.IFSCCode;
                                    empba.CmpID = EmployeeMaster.EmployeeBank.CmpID;
                                    empba.EmpID = emp.EmployeeID;
                                    empba.UpdatedBy = emp.UpdatedBy;
                                    empba.UpdatedUTC = DateTime.UtcNow;
                                    CMPSContext.EmployeeBank.UpdateRange(empba);
                                    CMPSContext.SaveChanges();
                                }
                            }


                            var vs = CMPSContext.EmployeeBank.Where(x => x.EmpID == EmployeeID).LastOrDefault();

                            if (vs != null)
                            {
                                if (vs.IsDeleted != false)
                                {
                                    var empbankup = new EmployeeBank();

                                    int empidd = vs.EmpID;
                                    empbankup.EmpID = empidd;
                                    empbankup.CmpID = EmployeeMaster.EmployeeBank.CmpID;
                                    empbankup.BankName = EmployeeMaster.EmployeeBank.BankName;
                                    empbankup.BankBranch = EmployeeMaster.EmployeeBank.BankBranch;
                                    empbankup.AccountHoldersName = EmployeeMaster.EmployeeBank.AccountHoldersName;
                                    empbankup.AccountType = EmployeeMaster.EmployeeBank.AccountType;
                                    empbankup.AccountNo = EmployeeMaster.EmployeeBank.AccountNo;
                                    empbankup.IFSCCode = EmployeeMaster.EmployeeBank.IFSCCode;
                                    empbankup.IsDeleted = false;
                                    empbankup.CreatedBy = EmployeeMaster.EmployeeBank.CreatedBy;
                                    CMPSContext.EmployeeBank.Add(empbankup);
                                    CMPSContext.SaveChanges();
                                }
                            }



                            var vv = CMPSContext.EmployeeBank.Where(x => x.EmpID == EmployeeID).LastOrDefault();

                            if (vv == null)
                            {
                                if (EmployeeMaster.EmployeeBank.AccountType != null)
                                {
                                    var empbankup = new EmployeeBank();

                                    empbankup.EmpID = emp.EmployeeID;
                                    empbankup.CmpID = EmployeeMaster.EmployeeBank.CmpID;
                                    empbankup.BankName = EmployeeMaster.EmployeeBank.BankName;
                                    empbankup.BankBranch = EmployeeMaster.EmployeeBank.BankBranch;
                                    empbankup.AccountHoldersName = EmployeeMaster.EmployeeBank.AccountHoldersName;
                                    empbankup.AccountType = EmployeeMaster.EmployeeBank.AccountType;
                                    empbankup.AccountNo = EmployeeMaster.EmployeeBank.AccountNo;
                                    empbankup.IFSCCode = EmployeeMaster.EmployeeBank.IFSCCode;
                                    empbankup.IsDeleted = false;
                                    empbankup.CreatedBy = EmployeeMaster.EmployeeBank.CreatedBy;
                                    CMPSContext.EmployeeBank.Add(empbankup);
                                    CMPSContext.SaveChanges();
                                }


                            }

                            if (EmployeeMaster.EmployeeExperience != null)

                            {

                                var ids = WYNKContext.Experience.Where(x => x.EmpID == EmployeeID).ToList();

                                foreach (var item in ids.ToList())
                                {


                                    if (ids != null && ids.Count != 0)

                                    {

                                        ids.All(x => { x.EmpID = item.EmpID; return true; });
                                        var idsss = WYNKContext.Experience.Where(x => x.EmpID == EmployeeID).ToList();
                                        if (idsss.Count != 0)
                                        {
                                            WYNKContext.Experience.RemoveRange(ids);
                                            WYNKContext.SaveChanges();
                                        }

                                    }
                                }

                                var ids1 = WYNKContext.Experience.Where(x => x.EmpID == EmployeeID).ToList();

                                if (ids1.Count == 0)
                                {
                                    foreach (var item in EmployeeMaster.EmployeeExperience.ToList())
                                    {
                                        var S = new EmployeeExperience();

                                        int empid4 = emp.EmployeeID;
                                        S.EmpID = empid4;
                                        S.Organisation = item.Organisation;
                                        S.FromDate = item.FromDate.AddDays(1);
                                        S.ToDate = item.ToDate.AddDays(1);
                                        S.Designation = item.Designation;
                                        S.CreatedBy = item.CreatedBy;
                                        WYNKContext.Experience.AddRange(S);
                                        WYNKContext.SaveChanges();
                                    }
                                }
                            }


                            if (EmployeeMaster.empqua1 != null)
                            {
                                var idss = WYNKContext.EmployeeQualification.Where(x => x.EmpID == EmployeeID).ToList();

                                foreach (var item in idss.ToList())

                                {

                                    if (idss != null && idss.Count != 0)
                                    {

                                        idss.All(x => { x.EmpID = item.EmpID; return true; });
                                        var idssss = WYNKContext.EmployeeQualification.Where(u => u.EmpID == EmployeeID).ToList();
                                        if (idssss.Count != 0)
                                        {
                                            WYNKContext.EmployeeQualification.RemoveRange(idssss);
                                            WYNKContext.SaveChanges();
                                        }


                                    }
                                }

                                var ids2 = WYNKContext.EmployeeQualification.Where(x => x.EmpID == EmployeeID).ToList();

                                if (ids2.Count == 0)
                                {
                                    foreach (var item in EmployeeMaster.empqua1.ToList())
                                    {

                                        var SS = new EmployeeQualification();

                                        SS.EmpID = emp.EmployeeID;
                                        SS.Cmpid = emp.CMPID;
                                        if (SS.FromDate != null)
                                        {
                                            SS.FromDate = item.fromdate.AddDays(1);
                                        }
                                        if (SS.ToDate != null)
                                        {
                                            SS.ToDate = item.todate.AddDays(1);
                                        }
                                        SS.YearofPass = item.yearofpassing;
                                        SS.QualExtCode = item.qid;
                                        SS.UniversityExtCode = item.uid;
                                        SS.IsDeleted = false;
                                        SS.Percentage = item.percentageofmarks;
                                        SS.CreatedBy = item.createby;
                                        SS.CreatedUTC = DateTime.UtcNow;
                                        WYNKContext.EmployeeQualification.AddRange(SS);
                                        WYNKContext.SaveChanges();
                                    }
                                }



                            }

                        }

                    }
                }



                if (CMPSContext.SaveChanges() >= 0 || WYNKContext.SaveChanges() >= 0)

                    return new
                    {
                        Success = true,
                        Message = "Saved successfully",
                    };

                else
                {

                }

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = "Something Went Wrong"
            };


        }
        public dynamic DeleteEmployeebank(int? EmployeeID)
        {



            var empexp = CMPSContext.EmployeeBank.Where(x => x.EmpID == EmployeeID).LastOrDefault();
            if (empexp != null)
            {
                empexp.IsDeleted = true;
                CMPSContext.EmployeeBank.UpdateRange(empexp);

            }

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
                Message = "Some data are Missing"
            };
        }
        public dynamic GetDegree(int id)
        {


            var degreeDetails = new Employee_master();
            degreeDetails.qualificationexten = new List<qualificationexten>();

            degreeDetails.qualificationexten = (from D in WYNKContext.QualificationExt.Where(u => u.QualCode == Convert.ToInt32(id) && u.IsActive == true).GroupBy(x => x.QualExtDescription)
                                                select new qualificationexten
                                                {
                                                    ID = D.Key,
                                                    QualExtDescription = D.Select(x => x.QualExtDescription).FirstOrDefault(),                                                   
                                                }).ToList();



            return degreeDetails;
        }
        public dynamic GetSpecialization(string name)
        {
            var specialDetails = new Employee_master();
            specialDetails.qualificationexten1 = new List<qualificationexten1>();

            specialDetails.qualificationexten1 = (from DD in WYNKContext.QualificationExt.Where(u => u.QualExtDescription == name && u.IsActive == true)
                                                  select new qualificationexten1
                                                  {
                                                      QualExtAnsDescription = DD.QualExtAnsDescription,
                                                      QID = DD.ID,
                                                  }).ToList();


            return specialDetails;
        }
        public dynamic GetlocationDetails(int id)
        {

            var locDetails = new Employee_master();

            var v = CMPSContext.LocationMaster.Where(x => x.ID == id).Select(x => x.ParentDescription).FirstOrDefault();
            var stateid = CMPSContext.LocationMaster.Where(x => x.ParentDescription == v).Select(x => x.ParentID).FirstOrDefault();
            locDetails.ParentDescriptionstatee = CMPSContext.LocationMaster.Where(x => x.ID == stateid).Select(x => x.ParentDescription).FirstOrDefault();

            var countryid = CMPSContext.LocationMaster.Where(x => x.ParentDescription == locDetails.ParentDescriptionstatee).Select(x => x.ParentID).FirstOrDefault();
            locDetails.ParentDescriptioncountryy = CMPSContext.LocationMaster.Where(x => x.ID == countryid).Select(x => x.ParentDescription).FirstOrDefault();

            return locDetails;

        }










    }


}











