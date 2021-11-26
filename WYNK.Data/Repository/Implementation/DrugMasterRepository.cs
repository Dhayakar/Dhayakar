
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;

namespace WYNK.Data.Repository.Implementation
{
    class DrugMasterRepository : RepositoryBase<DrugMaster>, IDrugMasterRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
       

        public DrugMasterRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic AddDrug(DrugMaster AddDrug, int? cmpid)
        {
            try
            {
                var drugMaster = new Drug_Master();

                drugMaster.Brand = AddDrug.drugMaster.Brand;
                drugMaster.Manufacturer = AddDrug.drugMaster.Manufacturer;
                drugMaster.UOM = AddDrug.drugMaster.UOM;
                drugMaster.DrugGroup = AddDrug.drugMaster.DrugGroup;
                drugMaster.GenericName = AddDrug.drugMaster.GenericName;
                drugMaster.Rate = AddDrug.drugMaster.Rate;
                drugMaster.HSNNo = AddDrug.drugMaster.HSNNo;
                drugMaster.TaxID = AddDrug.drugMaster.TaxID;
                drugMaster.IsActive = true;
                drugMaster.CreatedBy = AddDrug.drugMaster.CreatedBy;
                drugMaster.Cmpid = AddDrug.drugMaster.Cmpid;
                drugMaster.CreatedUTC =DateTime.UtcNow;
                drugMaster.DrugCategory = AddDrug.drugMaster.DrugCategory;
                drugMaster.DrugTracker = AddDrug.drugMaster.DrugTracker;
                drugMaster.Power = AddDrug.drugMaster.Power;
                drugMaster.ModelNo = AddDrug.drugMaster.ModelNo;
                drugMaster.Aconstant = AddDrug.drugMaster.Aconstant;
                drugMaster.OpticDia = AddDrug.drugMaster.OpticDia;
                drugMaster.Length = AddDrug.drugMaster.Length;
                drugMaster.DrugComposition = AddDrug.drugMaster.DrugComposition;
                drugMaster.DrugSubDescription = AddDrug.drugMaster.DrugSubDescription;

                WYNKContext.DrugMaster.Add(drugMaster);

                string cmpname = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault();
                string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == AddDrug.drugMaster.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                string userid = Convert.ToString(AddDrug.drugMaster.CreatedBy);
                ErrorLog oErrorLogs = new ErrorLog();
                oErrorLogs.WriteErrorLogTitle(cmpname, "Drug Master", "User name :", username, "User ID :", userid, "Mode : Add");
                object names = drugMaster;
                oErrorLogs.WriteErrorLogArray("DrugMaster", names);


                WYNKContext.SaveChanges();

                if (WYNKContext.SaveChanges() >= 0)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                }
                 
                return new
                {
                    Success = true,
                    Message = "Saved successfully",
                    Id = drugMaster.ID,
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
                Message = "Something Went Wrong"
            };

        }


        public dynamic uploadImagedrug(IFormFile file, string M_Brand)
        {
                var currentDir = Directory.GetCurrentDirectory();
                if (!Directory.Exists(currentDir + "/DrugImage/"))
                    Directory.CreateDirectory(currentDir + "/DrugImage/");
                var fileName = $"{M_Brand}{Path.GetExtension(file.FileName)}";
                var path = $"{currentDir}/DrugImage/{fileName}";

                if ((File.Exists(path)))
                    File.Delete(path);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                    var id = WYNKContext.DrugMaster.Where(x => x.Brand == M_Brand).Select(x => x.ID).FirstOrDefault();
                    var pat = WYNKContext.DrugMaster.Where(x => x.ID == id).FirstOrDefault();
                    pat.DrugImagePath = path;
                    WYNKContext.Entry(pat).State = EntityState.Modified;
                    return WYNKContext.SaveChanges() > 0;
                }
        }


        public dynamic GetDrugimage(string Brand)
        {

            var reg = new Drug_Master();


            var regs = WYNKContext.DrugMaster.Where(x => x.Brand == Brand).Select(x => x.DrugImagePath).FirstOrDefault();

            if (regs != null)
            {
                var osfn = regs;
                string path = osfn;
              
                if ((File.Exists(path)))
                {
                    string imageData = Convert.ToBase64String(File.ReadAllBytes(path));

                    string source = imageData;
                    string base64 = source.Substring(source.IndexOf(',') + 1);
                    byte[] data = Convert.FromBase64String(base64);


                    reg.DrugImagePath = imageData;
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






        public dynamic UpdateDrug(DrugMaster DrugMaster, int? ID,int? cmpid)
        {
            try
            {
                var drugMaster = new Drug_Master();

                drugMaster = WYNKContext.DrugMaster.Where(x => x.ID == ID).FirstOrDefault();

                drugMaster.Brand = DrugMaster.drugMaster.Brand;
                drugMaster.Manufacturer = DrugMaster.drugMaster.Manufacturer;
                drugMaster.UOM = DrugMaster.drugMaster.UOM;
                drugMaster.GenericName = DrugMaster.drugMaster.GenericName;
                drugMaster.DrugGroup = DrugMaster.drugMaster.DrugGroup;
                drugMaster.Rate = DrugMaster.drugMaster.Rate;
                drugMaster.HSNNo = DrugMaster.drugMaster.HSNNo;
                drugMaster.TaxID = DrugMaster.drugMaster.TaxID;
                drugMaster.IsActive = DrugMaster.drugMaster.IsActive;
                drugMaster.UpdatedBy = DrugMaster.drugMaster.UpdatedBy;
                drugMaster.DrugCategory = DrugMaster.drugMaster.DrugCategory;
                drugMaster.DrugTracker = DrugMaster.drugMaster.DrugTracker;
                drugMaster.Power = DrugMaster.drugMaster.Power;
                drugMaster.Aconstant = DrugMaster.drugMaster.Aconstant;
                drugMaster.ModelNo = DrugMaster.drugMaster.ModelNo;
                drugMaster.OpticDia = DrugMaster.drugMaster.OpticDia;
                drugMaster.Length = DrugMaster.drugMaster.Length;
                drugMaster.DrugComposition = DrugMaster.drugMaster.DrugComposition;
                drugMaster.DrugSubDescription = DrugMaster.drugMaster.DrugSubDescription;
                drugMaster.Cmpid = DrugMaster.drugMaster.Cmpid;
                drugMaster.UpdatedUTC = DateTime.UtcNow;
                WYNKContext.DrugMaster.UpdateRange(drugMaster);


                string cmpname = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault();
                string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == DrugMaster.drugMaster.UpdatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                string userid = Convert.ToString(DrugMaster.drugMaster.UpdatedBy);
                ErrorLog oErrorLogs = new ErrorLog();
                oErrorLogs.WriteErrorLogTitle(cmpname, "Drug Master", "User name :", username, "User ID :", userid, "Mode : Edit");
                object names = drugMaster;
                oErrorLogs.WriteErrorLogArray("DrugMaster", names);

                WYNKContext.SaveChanges();

                if (WYNKContext.SaveChanges() >= 0)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                }



                if (WYNKContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = "Saved successfully",
                        Id = drugMaster.ID,
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
                Message = "Something Went Wrong"
            };
        }


        public dynamic AddDrugGroup(DrugMaster AddDrugGroup)
        {
            try
            {
                var drugGroupMaster = new Drug_Group();
                drugGroupMaster.Description = AddDrugGroup.DrugGroup.Description;
                drugGroupMaster.DrugFormID = AddDrugGroup.DrugGroup.DrugFormID;
                drugGroupMaster.RetestInterval = AddDrugGroup.DrugGroup.RetestInterval;
                drugGroupMaster.RetestCriticalInterval = AddDrugGroup.DrugGroup.RetestCriticalInterval;
                drugGroupMaster.SideEffects = AddDrugGroup.DrugGroup.SideEffects;
                drugGroupMaster.Precautions = AddDrugGroup.DrugGroup.Precautions;
                drugGroupMaster.Overdose = AddDrugGroup.DrugGroup.Overdose;
                drugGroupMaster.IsStepDown = AddDrugGroup.DrugGroup.IsStepDown;
                drugGroupMaster.CreatedUTC = DateTime.UtcNow;
                drugGroupMaster.CreatedBy = AddDrugGroup.DrugGroup.CreatedBy;
                drugGroupMaster.Cmpid = AddDrugGroup.DrugGroup.Cmpid;

                var drugformDesc = CMPSContext.OneLineMaster.Where(x => x.OLMID == drugGroupMaster.DrugFormID).Select(x => x.ParentDescription).FirstOrDefault();
                WYNKContext.DrugGroup.Add(drugGroupMaster);
                WYNKContext.SaveChanges();
             
                return new
                {
                    Success = true,
                    Message = "Saved successfully",
                    Id = Convert.ToString(drugGroupMaster.ID),
                    drugformDesc = drugformDesc
                };
           
            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                    Message = "Something Went Wrong"
                };
            }
        }

        public dynamic getTaxValues(int ID)
        {

            var taxDet = new DrugMaster();

            taxDet.getTaxData = (from tax in CMPSContext.TaxMaster.Where(x => x.ID == ID)

                                 select new getTaxData
                                 {
                                     GSTPercentage = tax.GSTPercentage,
                                     CGSTPercentage = tax.CGSTPercentage,
                                     SGSTPercentage = tax.SGSTPercentage,
                                     IGSTPercentage = tax.IGSTPercentage,
                                     CESSPercentage = tax.CESSPercentage,
                                     AdditionalCESSPercentage = tax.AdditionalCESSPercentage

                                 }).ToList();



            return taxDet;


        }
        public dynamic DrugGroupFormDesc(int Value)
        {
            try
            {
                var DrugFormId = WYNKContext.DrugGroup.Where(x => x.ID == Value).Select(x => x.DrugFormID).FirstOrDefault();

                var sideeffects = WYNKContext.DrugGroup.Where(x => x.ID == Value).Select(x => x.SideEffects).FirstOrDefault();
                var precaution = WYNKContext.DrugGroup.Where(x => x.ID == Value).Select(x => x.Precautions).FirstOrDefault();
                var overdose = WYNKContext.DrugGroup.Where(x => x.ID == Value).Select(x => x.Overdose).FirstOrDefault();


                var drugformDesc = CMPSContext.OneLineMaster.Where(x => x.OLMID == DrugFormId).Select(x => x.ParentDescription).FirstOrDefault();


                return new
                {
                    res = drugformDesc,
                    M_sideEffect = sideeffects,
                    M_Precaution = precaution,
                    M_overdose = overdose,
                    Success = true,
                    Message = "Data Found"

                };

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = "data are Missing"
            };
        }

        public dynamic deleteDrug(int? iD)
        {


            var drugMaster = WYNKContext.DrugMaster.Where(x => x.ID == iD).ToList();
            if (drugMaster != null)
            {
                drugMaster.All(x => { x.IsDeleted = true; x.IsActive = false; return true; });
                WYNKContext.DrugMaster.UpdateRange(drugMaster);
                WYNKContext.SaveChanges();
            }

            try
            {
                if (WYNKContext.SaveChanges() >= 0)
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
                Message = "Something Went Wrong"
            };
        }






        public dynamic AddUOM(string UOM, int DoctorID)
        {
            var uom = new UOM_Master();
            uom.Description = UOM;
            uom.CreatedUTC = DateTime.UtcNow;
            uom.CreatedBy = DoctorID;
            CMPSContext.uommaster.Add(uom);
            CMPSContext.SaveChanges();
            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                    return new
                    {
                        Id = uom.Description,
                        Success = true,
                        Message = "Saved successfully"
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



        public dynamic updatedata(DrugMaster UpdateDrugGroup, int ID)
        {
            try
            {
                var drugMaster = new Drug_Group();
                drugMaster = WYNKContext.DrugGroup.Where(x => x.ID == ID && x.Cmpid == UpdateDrugGroup.DrugGroup.Cmpid).FirstOrDefault();
                drugMaster.Description = UpdateDrugGroup.DrugGroup.Description;
                drugMaster.DrugFormID = UpdateDrugGroup.DrugGroup.DrugFormID;
                drugMaster.RetestInterval = UpdateDrugGroup.DrugGroup.RetestInterval;
                drugMaster.RetestCriticalInterval = UpdateDrugGroup.DrugGroup.RetestCriticalInterval;
                drugMaster.SideEffects = UpdateDrugGroup.DrugGroup.SideEffects;
                drugMaster.Precautions = UpdateDrugGroup.DrugGroup.Precautions;
                drugMaster.Overdose = UpdateDrugGroup.DrugGroup.Overdose;
                drugMaster.IsStepDown = UpdateDrugGroup.DrugGroup.IsStepDown;
                drugMaster.UpdatedUTC = DateTime.UtcNow;
                drugMaster.UpdatedBy = UpdateDrugGroup.DrugGroup.UpdatedBy;
                WYNKContext.DrugGroup.UpdateRange(drugMaster);
                WYNKContext.SaveChanges();
                return new
                {
                    Success = true,
                    Message = "Updated successfully",
                    Id = drugMaster.ID,
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                    Message = "Something went Wrong"
                };
            }
   
        }


        public dynamic DeleteGenericMedicine(int ID)
        {
            var DrugGroup = new Drug_Group();
            DrugGroup = WYNKContext.DrugGroup.Where(x => x.ID == ID).FirstOrDefault();
            DrugGroup.IsDeleted = true;
            WYNKContext.Entry(DrugGroup).State = EntityState.Modified;
            WYNKContext.SaveChanges();
            try
            {
                if (WYNKContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = "Saved successfully"
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

        public dynamic DeleteDrugGroup(int ID)
        {
            try
            {
                var DG = WYNKContext.DrugGroup.Where(x => x.ID == ID).ToList();
                if (DG != null)
                {
                    DG.All(x => { x.IsDeleted = true; return true; });
                    WYNKContext.DrugGroup.UpdateRange(DG);
                    WYNKContext.SaveChanges();
                }

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
                return new
                {
                    Success = false,
                    Message = "Some data are Missing"
                };
            }
          
        }
    }
}













