using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class SquintExtnMasterRepository : RepositoryBase<SquintExtnMasterViewModel>, ISquintExtnMasterRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        
        public SquintExtnMasterRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;
            
        }



        public dynamic InsertSquintExtnM(SquintExtnMasterViewModel SQExtnM, int userroleID, int CmpID)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var Description = new Squint_ExtnMaster();
                    if (SQExtnM.MastersName == "Ocular Movements")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "ocularmovement" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "OCM";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }
                    else if (SQExtnM.MastersName == "Cover Test on Head Tilt")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "vfindings" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "VFI";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }
                    else if (SQExtnM.MastersName == "Angle kappa")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "anglekappa" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "anglekappa";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }
                    else if (SQExtnM.MastersName == "ACA method")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "acamethod" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "acamethod";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }
                    else if (SQExtnM.MastersName == "ACA value")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "acavalue" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "acavalue";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }
                    else if (SQExtnM.MastersName == "Worth Four Dot Test")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "wfdt" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "wfdt";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }
                    else if (SQExtnM.MastersName == "Stereopsis method")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Stereopsis method" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "Streopsis method";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }
                    else if (SQExtnM.MastersName == "Stereopsis value")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Stereopsis value" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "Streopsis value";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }
                    else if (SQExtnM.MastersName == "ARC")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "ARC" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "ARC";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }
                    else if (SQExtnM.MastersName == "Squint Types")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "PBCTMK" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "PBCTMK";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }
                    else if (SQExtnM.MastersName == "Amplitude")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Amplitude" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "Amplitude";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }
                    else if (SQExtnM.MastersName == "Frequency")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Frequency" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "Frequency";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }
                    else if (SQExtnM.MastersName == "Type")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Type" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "Type";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }
                    else if (SQExtnM.MastersName == "Pursuit")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Pursuit" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "Pursuit";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }
                    else if (SQExtnM.MastersName == "Saccade")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Saccade" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "Saccade";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }
                    else if (SQExtnM.MastersName == "Abnormal Head position")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Abnormal Head position" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "ABH";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }
                    else if (SQExtnM.MastersName == "Frequency on Convergence")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Frequency on Convergence" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "FOC";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }
                    else if (SQExtnM.MastersName == "Occulusion of One eye")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Occulusion of One eye" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "OOE";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }
                    else if (SQExtnM.MastersName == "Oscillopsia")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Oscillopsia" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "Oscillopsia";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }
                    else if (SQExtnM.MastersName == "Patterns of squint")
                    {

                        Description.ParentDescription = SQExtnM.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Patterns of squint" && x.CmpID == CmpID).Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "POS";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CmpID = CmpID;
                        WYNKContext.SquintExtnMaster.Add(Description);
                        WYNKContext.SaveChanges();
                    }

                    else
                    {
                        var specmas = new SpecialityMaster();
                        specmas.ParentDescription = SQExtnM.SpecialityMaster.ParentDescription;
                        specmas.ParentTag = SQExtnM.SpecialityMaster.ParentTag;
                        specmas.CreatedBy = userroleID;
                        specmas.CreatedUTC = DateTime.UtcNow;
                        specmas.IsActive = true;
                        specmas.IsDeleted = false;
                        specmas.CmpID = CmpID;
                        WYNKContext.SpecialityMaster.Add(specmas);
                        WYNKContext.SaveChanges();
                    }

                    string cmpname = CMPSContext.Company.Where(x => x.CmpID == CmpID).Select(x => x.CompanyName).FirstOrDefault();
                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == userroleID).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    ErrorLog oErrorLogs = new ErrorLog();
                    object namestr = Description;
                    oErrorLogs.WriteErrorLogTitle(cmpname, "Squint Extn Master", "User name :", username, "User ID :", Convert.ToString(userroleID), "Mode : Add");

                    if (WYNKContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }
                    dbContextTransaction.Commit();
                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Message = "Saved successfully"
                        };
                }
                catch (Exception ex)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
            }
            return new
            {
                Success = false,
                Message = "Some data are Missing"
            };
        }



        public dynamic UpdateSquintExtnM(SquintExtnMasterViewModel updateMaster, int ID, int userroleID, int CmpID)


        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var Description = new Squint_ExtnMaster();
                    Description = WYNKContext.SquintExtnMaster.Where(x => x.ID == ID && x.CmpID == CmpID).FirstOrDefault();


                    if (updateMaster.MastersName == "Ocular Movements")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "ocularmovement").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "OCM";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (updateMaster.MastersName == "Cover Test on Head Tilt")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "vfindings").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "VFI";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (updateMaster.MastersName == "Angle kappa")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "anglekappa").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "anglekappa";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (updateMaster.MastersName == "ACA method")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "acamethod").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "acamethod";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (updateMaster.MastersName == "ACA value")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "acavalue").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "acavalue";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (updateMaster.MastersName == "Worth Four Dot Test")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "wfdt").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "wfdt";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (updateMaster.MastersName == "Stereopsis method")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Stereopsis method").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "Streopsis method";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (updateMaster.MastersName == "Stereopsis value")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Stereopsis value").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "Streopsis value";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (updateMaster.MastersName == "ARC")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "ARC").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "ARC";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (updateMaster.MastersName == "Squint Types")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "PBCTMK").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "PBCTMK";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (updateMaster.MastersName == "Amplitude")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Amplitude").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "Amplitude";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (updateMaster.MastersName == "Frequency")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Frequency").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "Frequency";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (updateMaster.MastersName == "Type")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Type").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "Type";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (updateMaster.MastersName == "Pursuit")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Pursuit").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "Pursuit";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (updateMaster.MastersName == "Saccade")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Saccade").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "Saccade";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (updateMaster.MastersName == "Abnormal Head position")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Abnormal Head position").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "ABH";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (updateMaster.MastersName == "Frequency on Convergence")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Frequency on Convergence").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "FOC";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (updateMaster.MastersName == "Occulusion of One eye")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Occulusion of One eye").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "OOE";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (updateMaster.MastersName == "Oscillopsia")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Oscillopsia").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "Oscillopsia";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (updateMaster.MastersName == "Patterns of squint")
                    {
                        Description.ParentDescription = updateMaster.SquintExtnMaster.ParentDescription;
                        Description.ParentID = WYNKContext.SquintExtnMaster.Where(x => x.ParentDescription == "Patterns of squint").Select(x => x.ID).FirstOrDefault();
                        Description.ParentTag = "POS";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = updateMaster.SquintExtnMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    
                    else
                    {
                        var Descriptions = new SpecialityMaster();
                        Descriptions = WYNKContext.SpecialityMaster.Where(x => x.ID == ID && x.CmpID == CmpID).FirstOrDefault();
                        Descriptions.ParentDescription = updateMaster.SpecialityMaster.ParentDescription;
                        Descriptions.ParentTag = updateMaster.SpecialityMaster.ParentTag;
                        Descriptions.UpdatedBy = userroleID;
                        Descriptions.IsActive = updateMaster.SpecialityMaster.IsActive;
                        Descriptions.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Descriptions).State = EntityState.Modified;
                    }
                    
                    dbContextTransaction.Commit();
                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Message = "Update successfully"
                        };
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
            }
            return new
            {
                Success = false,
                Message = "Some data are Missing"
            };
        }



        public dynamic DeleteSQEM(SquintExtnMasterViewModel DeleteMaster, int ID, int CmpID)


        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var Description = new Squint_ExtnMaster();
                    Description = WYNKContext.SquintExtnMaster.Where(x => x.ID == ID && x.CmpID == CmpID).FirstOrDefault();

                    if (DeleteMaster.MastersName == "Ocular Movements")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (DeleteMaster.MastersName == "Cover Test on Head Tilt")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (DeleteMaster.MastersName == "Angle kappa")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (DeleteMaster.MastersName == "ACA method")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (DeleteMaster.MastersName == "ACA value")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (DeleteMaster.MastersName == "Worth Four Dot Test")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (DeleteMaster.MastersName == "Stereopsis method")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (DeleteMaster.MastersName == "Stereopsis value")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (DeleteMaster.MastersName == "ARC")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (DeleteMaster.MastersName == "Squint Types")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (DeleteMaster.MastersName == "Amplitude")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (DeleteMaster.MastersName == "Frequency")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (DeleteMaster.MastersName == "Type")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (DeleteMaster.MastersName == "Pursuit")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (DeleteMaster.MastersName == "Saccade")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (DeleteMaster.MastersName == "Abnormal Head position")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (DeleteMaster.MastersName == "Frequency on Convergence")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (DeleteMaster.MastersName == "Occulusion of One eye")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (DeleteMaster.MastersName == "Oscillopsia")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    else if (DeleteMaster.MastersName == "Patterns of squint")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                        WYNKContext.Entry(Description).State = EntityState.Modified;
                    }
                    
                     else
                    {
                        var Descriptions = new SpecialityMaster();
                        Descriptions = WYNKContext.SpecialityMaster.Where(x => x.ID == ID && x.CmpID == CmpID).FirstOrDefault();
                        Descriptions.IsDeleted = true;
                        Descriptions.IsActive = false;
                        WYNKContext.Entry(Descriptions).State = EntityState.Modified;
                    }

                    dbContextTransaction.Commit();
                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Message = "Delete successfully"
                        };
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
            }
            return new
            {
                Success = false,
                Message = "Some data are Missing"
            };
        }


        public SquintExtnMasterViewModel GetDetails(string MasterName, int CmpID)

        {
            var SQEXTM = new SquintExtnMasterViewModel();


            if (MasterName == "Cover Test on Head Tilt")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "VFI" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                select new SQEMaster
                                {
                                    ID = SQM.ID,
                                    Description = SQM.ParentDescription,
                                    IsActive = SQM.IsActive,
                                    IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                    PID = SQM.ParentID,
                                    PTag = SQM.ParentTag,
                                }).ToList();
            }
            else if (MasterName == "Ocular Movements")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "OCM" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PID = SQM.ParentID,
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }
            else if (MasterName == "Angle kappa")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "anglekappa" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PID = SQM.ParentID,
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }
            else if (MasterName == "ACA method")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "acamethod" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PID = SQM.ParentID,
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }
            else if (MasterName == "ACA value")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "acavalue" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PID = SQM.ParentID,
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }
            else if (MasterName == "Worth Four Dot Test")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "wfdt" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PID = SQM.ParentID,
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }
            else if (MasterName == "Stereopsis method")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "Streopsis method" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PID = SQM.ParentID,
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }
            else if (MasterName == "Stereopsis value")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "Streopsis value" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PID = SQM.ParentID,
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }
            else if (MasterName == "ARC")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "ARC" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PID = SQM.ParentID,
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }
            else if (MasterName == "Squint Types")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "PBCTMK" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PID = SQM.ParentID,
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }
            else if (MasterName == "Amplitude")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "Amplitude" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PID = SQM.ParentID,
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }
            else if (MasterName == "Frequency")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "Frequency" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PID = SQM.ParentID,
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }
            else if (MasterName == "Type")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "Type" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PID = SQM.ParentID,
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }
            else if (MasterName == "Pursuit")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "Pursuit" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PID = SQM.ParentID,
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }
            else if (MasterName == "Saccade")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "Saccade" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PID = SQM.ParentID,
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }
            else if (MasterName == "Abnormal Head position")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "ABH" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PID = SQM.ParentID,
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }
            else if (MasterName == "Frequency on Convergence")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "FOC" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PID = SQM.ParentID,
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }
            else if (MasterName == "Occulusion of One eye")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "OOE" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PID = SQM.ParentID,
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }
            else if (MasterName == "Oscillopsia")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "Oscillopsia" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PID = SQM.ParentID,
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }
            else if (MasterName == "Patterns of squint")
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SquintExtnMaster.Where(u => u.ParentTag == "POS" && u.IsDeleted == false && u.ParentID != 0 && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PID = SQM.ParentID,
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }

            else
            {
                SQEXTM.SQMaster = (from SQM in WYNKContext.SpecialityMaster.Where(u => u.ParentTag == MasterName && u.IsDeleted == false && u.CmpID == CmpID)
                                   select new SQEMaster
                                   {
                                       ID = SQM.ID,
                                       Description = SQM.ParentDescription,
                                       IsActive = SQM.IsActive,
                                       IsActive1 = Enum.GetName(typeof(ISactive), SQM.IsActive),
                                       PTag = SQM.ParentTag,
                                   }).ToList();
            }
            return SQEXTM;

        }


    }

}