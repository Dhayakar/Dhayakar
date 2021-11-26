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
    class FindingsRepository : RepositoryBase<Findings>, IFindingsRepository
    {

        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;



        public FindingsRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic docnames(string uin)
        {
            var v = new Findings();
            v.Finding = new Finding();
            var patient = WYNKContext.PatientAssign.ToList();
            var doctormas = CMPSContext.DoctorMaster.ToList();

            var RE = WYNKContext.RegistrationTran.Where(x => x.UIN == uin).Select(x => x.RegistrationTranID).LastOrDefault();
            var DRID = WYNKContext.PatientAssign.Where(x => x.RegistrationTranID == RE).ToList();




            foreach (var item in DRID.ToList())
            {

                var d = WYNKContext.PatientAssign.Where(x => x.ID == item.ID).Select(x => x.DoctorID).FirstOrDefault();

                v.Registerationnos = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.StaffIdentification).FirstOrDefault();

                if (v.Registerationnos == "D")
                {
                    v.Docnames = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.LastName).FirstOrDefault();
                    v.Docnamesf = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.Firstname).FirstOrDefault();
                    v.Docnamesm = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.MiddleName).FirstOrDefault();
                    v.Regnos = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.RegistrationNumber).FirstOrDefault();
                }
                if (v.Registerationnos == "O")
                {
                    v.OptoNames = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.LastName).FirstOrDefault();
                    v.OptoNamesf = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.Firstname).FirstOrDefault();
                    v.OptoNamesm = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.MiddleName).FirstOrDefault();
                }

                if (v.Registerationnos == "V")
                {
                    v.VCNames = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.LastName).FirstOrDefault();
                }

            }




            try
            {

                return new
                {
                    Doctorname = v.Docnames,
                    Doctornamef = v.Docnamesf,
                    Doctornamem = v.Docnamesm,
                    Registerationno = v.Regnos,
                    Optometristname = v.OptoNames,
                    Optometristnamef = v.OptoNamesf,
                    Optometristnamem = v.OptoNamesm,
                    vcname = v.VCNames,

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


        public Findings GetcmpDetails(int id)

        {
            var fnd = new Findings();
            fnd.TonometryTran = new List<TonometryTran>();
            fnd.Cnamegl = CMPSContext.Company.Where(x => x.CmpID == id).Select(x => x.CompanyName).FirstOrDefault();

            return fnd;

        }


        public Findings GetallPatientDetails(string UIN, int CompanyID, string gmt)

        {
            var fnd = new Findings();


            var fnds = WYNKContext.Findings.ToList();
            var docmas = CMPSContext.DoctorMaster.ToList();
            var cmp = CMPSContext.Company.ToList();
            var user = CMPSContext.Users.ToList();
            var regssf = WYNKContext.Findings.Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).LastOrDefault();
            var reiddf = WYNKContext.Findings.Where(x => x.RegistrationTranID == regssf).Select(x => x.ID).LastOrDefault();
            var crby = WYNKContext.Findings.Where(x => x.ID == reiddf).Select(x => x.CreatedBy).LastOrDefault();
            var usnam = CMPSContext.Users.Where(x => x.Userid == crby).Select(x => x.Username).LastOrDefault();
            var ussnam = CMPSContext.DoctorMaster.Where(x => x.EmailID == usnam).Select(x => x.LastName).LastOrDefault();

            TimeSpan ts = TimeSpan.Parse(gmt);
            int cmpid = CMPSContext.Company.Where(c => c.CmpID == CompanyID).Select(c => c.ParentID).FirstOrDefault();
            if (cmpid == 0)
            {
                cmpid = CompanyID;
            }
            var grp = (from FN in fnds.Where(u => u.UIN == UIN).OrderByDescending(x => x.CreatedUTC)
                       join us in user on
                       FN.CreatedBy equals us.Userid
                       join doc in docmas on
                       us.Username equals doc.EmailID
                       join cm in cmp.Where(x => x.ParentID == cmpid || x.CmpID == cmpid) on FN.CmpID equals cm.CmpID
                       select new
                       {
                           fiid = FN.RandomUniqueID,
                           rid = FN.RegistrationTranID,
                           FiDate = FN.CreatedUTC.Add(ts),
                           Treatment = FN.TreatmentAdvice,
                           Rdate = FN.ReviewDate,
                           DocName = doc.Firstname + " " + doc.MiddleName + " " + doc.LastName,
                           CmpName = cm.CompanyName + "-" + cm.Address1,
                       }).ToList();

            var grpadm = (from FN in fnds.Where(u => u.UIN == UIN).OrderByDescending(x => x.CreatedUTC)
                          join us in user on
                          FN.CreatedBy equals us.Userid
                          join cm in cmp.Where(x => x.ParentID == cmpid || x.CmpID == cmpid) on
                          us.Username equals cm.EmailID
                          select new
                          {
                              fiid = FN.RandomUniqueID,
                              rid = FN.RegistrationTranID,
                              FiDate = FN.CreatedUTC.Add(ts),
                              Treatment = FN.TreatmentAdvice,
                              Rdate = FN.ReviewDate,
                              DocName = cm.ContactPerson,
                              CmpName = cm.CompanyName + "-" + cm.Address1,
                          }).ToList();

            var grpres = grp.Concat(grpadm);


            fnd.LastRecData = (from FN in grpres.OrderByDescending(x => x.FiDate).GroupBy(x => x.rid)

                               select new LastRecData
                               {
                                   rid = FN.Select(x => x.rid).FirstOrDefault(),
                                   fiid = FN.Select(x => x.fiid).FirstOrDefault(),
                                   FiDate = FN.Select(x => x.FiDate).FirstOrDefault(),
                                   Treatment = FN.Select(x => x.Treatment).FirstOrDefault(),
                                   Rdate = FN.Select(x => x.Rdate).FirstOrDefault(),
                                   DocName = FN.Select(x => x.DocName).FirstOrDefault(),
                                   CmpName = FN.Select(x => x.CmpName).FirstOrDefault(),
                               }).ToList();


            return fnd;
        }


        public Findings GetregPatientDetails(string UIN, int CompanyID, string GMT)

        {
            var fnd = new Findings();



            var cmp = CMPSContext.Company.ToList();
            var regssf = WYNKContext.Findings.Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).LastOrDefault();
            var reiddf = WYNKContext.Findings.Where(x => x.RegistrationTranID == regssf).Select(x => x.ID).LastOrDefault();
            var crby = WYNKContext.Findings.Where(x => x.ID == reiddf).Select(x => x.CreatedBy).LastOrDefault();
            var usnam = CMPSContext.Users.Where(x => x.Userid == crby).Select(x => x.Username).LastOrDefault();
            var ussnam = CMPSContext.DoctorMaster.Where(x => x.EmailID == usnam).Select(x => x.LastName).LastOrDefault();
            var registration = WYNKContext.Registration.ToList();
            var registrationtran = WYNKContext.RegistrationTran.ToList();
            TimeSpan ts = TimeSpan.Parse(GMT);
            int cmpid = CMPSContext.Company.Where(c => c.CmpID == CompanyID).Select(c => c.ParentID).FirstOrDefault();
            if (cmpid == 0)
            {
                cmpid = CompanyID;
            }

            fnd.RegDet = (from reg in registration.Where(u => u.UIN == UIN)
                          join rt in registrationtran.OrderByDescending(x => x.CreatedUTC) on
                          reg.UIN equals rt.UIN

                          join cm in cmp.Where(x => x.ParentID == cmpid || x.CmpID == cmpid) on reg.CMPID equals cm.CmpID
                          select new RegDet
                          {
                              rid = rt.RegistrationTranID,
                              visitdt = rt.DateofVisit.Add(ts),
                              CmpName = cm.CompanyName + "-" + cm.Address1,
                          }).ToList();



            return fnd;
        }

        public Findings Getcustvalues(int RID, string GMT)

        {
            var fnd = new Findings();

            var cidd = WYNKContext.Findings.Where(x => x.RegistrationTranID == RID).Select(x => x.CmpID).LastOrDefault();

            fnd.CAddress = CMPSContext.Company.Where(x => x.CmpID == cidd).Select(x => x.Address1).FirstOrDefault();

            fnd.Cphone = CMPSContext.Company.Where(x => x.CmpID == cidd).Select(x => x.Phone1).FirstOrDefault();

            fnd.Cweb = CMPSContext.Company.Where(x => x.CmpID == cidd).Select(x => x.Website).FirstOrDefault();

            fnd.Cname = CMPSContext.Company.Where(x => x.CmpID == cidd).Select(x => x.CompanyName).FirstOrDefault();
            TimeSpan ts = TimeSpan.Parse(GMT);

            var onelinemaster = CMPSContext.OneLineMaster.AsNoTracking().ToList();
            var specialitymaster = WYNKContext.SpecialityMaster.AsNoTracking().ToList();
            var DC = CMPSContext.DoctorMaster.AsNoTracking().ToList();
            var oneline = CMPSContext.OneLineMaster.AsNoTracking().Where(x => x.ParentTag == "OcularComplaints").ToList();
            var Ocl = WYNKContext.OcularComplaints.AsNoTracking().Where(x => x.RegistrationTranID == RID && x.IsDeleted == false).ToList();
            var tono = WYNKContext.Tonometry.AsNoTracking().ToList();
            fnd.ComplaintsDetails = Ocl.OrderByDescending(x => x.CreatedUTC).Select(s => new ComplaintsDetails
            {
                ID = s.ID,
                Description = oneline.Where(x => x.OLMID == s.Description).Select(x => x.ParentDescription).FirstOrDefault(),
                Remarks = s.Remarks,
                ISOD = s.ISOD,
                ISOU = s.ISOU,
                ISOS = s.ISOS,
                Fromdate = s.FromUTC,
            }).ToList();


            var id = WYNKContext.Findings.AsNoTracking().Where(x => x.RegistrationTranID == RID).OrderByDescending(x => x.CreatedUTC).Select(x => x.RandomUniqueID).FirstOrDefault();


            var alid = WYNKContext.PatientGeneral.AsNoTracking().Where(x => x.RegistrationTranID == RID).OrderByDescending(x => x.CreatedUTC).Select(x => x.RunningID).FirstOrDefault();

            fnd.allergych = (from REF in WYNKContext.PatientGeneral.AsNoTracking().Where(u => u.RunningID == alid && u.IsDeleted == false)
                             select new allergych
                             {
                                 allergy = REF.Allergy,
                                 date = REF.CreatedUTC.Add(ts),

                             }).ToList();

            fnd.tonometrydetailsch = (from REF in WYNKContext.TonometryTran.AsNoTracking().Where(u => u.RegistrationtranID == RID && u.IsDeleted == false)
                                      select new tonometrydetailsch
                                      {
                                          ID = REF.ID,
                                          RegistrationtranID = REF.RegistrationtranID,
                                          UIN = REF.UIN,
                                          CmpID = REF.CmpID,
                                          VisitDatetime = REF.VisitDatetime,
                                          TonometryID = REF.TonometryID,
                                          Tonometryname = tono.Where(x => x.ID == REF.TonometryID).Select(x => x.Description).FirstOrDefault(),
                                          BOD = REF.BOD,
                                          BOS = REF.BOS,
                                          AOD = REF.AOD,
                                          AOS = REF.AOS,
                                          Time = REF.Time,
                                          Dilation = REF.Dilation.ToString(),
                                          Staffname = DC.Where(x => x.DoctorID == REF.StaffID).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                          StaffID = REF.StaffID,
                                          RemovedReasons = REF.RemovedReasons,
                                          RemovedBy = REF.RemovedBy,
                                          Removedname = DC.Where(x => x.DoctorID == REF.RemovedBy).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                          RemovedDatetime = REF.RemovedDatetime,
                                          drug = REF.Dropsname,
                                      }).ToList();

            fnd.odpach = WYNKContext.Glaucoma.AsNoTracking().Where(x => x.FindingsId == id).Select(x => x.pachymetryod).FirstOrDefault();
            fnd.ospach = WYNKContext.Glaucoma.AsNoTracking().Where(x => x.FindingsId == id).Select(x => x.pachymetryos).FirstOrDefault();


            fnd.opticprescription = (from op in WYNKContext.OpticalPrescription.AsNoTracking().Where(x => x.RegistrationTranID == RID)

                                     select new opticprescription
                                     {
                                         Type = op.Type,
                                         Ocular = op.Ocular,
                                         DistSph = op.DistSph,
                                         NearCyl = op.NearCyl,
                                         PinAxis = op.PinAxis,
                                         Add = op.Add != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(op.Add)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                         Remarks = op.Remarks,
                                         PD = op.PD,
                                         MPDOD = op.MPDOD,
                                         MPDOS = op.MPDOS,
                                     }).ToList();

            var slitlamp = WYNKContext.SlitLamp.AsNoTracking().ToList();
            var slitlampmaster = WYNKContext.SlitLampMaster.AsNoTracking().ToList();
            var funduss = WYNKContext.Fundus.AsNoTracking().ToList();
            var fundusmaster = WYNKContext.FundusMaster.AsNoTracking().ToList();
            var diagniosis = WYNKContext.Diagnosis.AsNoTracking().ToList();
            var eyelid = WYNKContext.EyelidDisorder.AsNoTracking().ToList();
            var orbdis = WYNKContext.OrbitalDisorders.AsNoTracking().ToList();

            fnd.elidhisc = (from REF in WYNKContext.Findings.AsNoTracking().Where(x => x.RandomUniqueID == id)
                            join EL in eyelid
                            on REF.RandomUniqueID equals EL.FindingsID


                            select new elidhisc
                            {
                                UpperRE = EL.UpperRE,
                                UpperLE = EL.UpperLE,
                                LowerRE = EL.LowerRE,
                                LowerLE = EL.LowerLE,
                                BothRE = EL.BothRE,
                                BothLE = EL.BothLE,
                                PosLidRE = EL.PosLidRE,
                                PosLidLE = EL.PosLidLE,
                                LashesRE = EL.LashesRE,
                                LashesLE = EL.LashesLE,
                                IntDstRE = EL.IntDstRE,
                                IntDstLE = EL.IntDstLE,
                                LPSRE = EL.LPSRE,
                                LPSLE = EL.LPSLE,
                                POSRE = EL.POSRE,
                                POSLE = EL.POSLE,

                            }).ToList();

            fnd.orbitalcushis = (from REF in WYNKContext.Findings.AsNoTracking().Where(x => x.RandomUniqueID == id)
                                 join EL in orbdis
                                 on REF.RandomUniqueID equals EL.FindingsID


                                 select new orbitalcushis
                                 {

                                     PositionofGlobeRE = onelinemaster.Where(x => x.OLMID == EL.PositionofGlobeRE).Select(x => x.ParentDescription).FirstOrDefault(),
                                     PositionofGlobeLE = onelinemaster.Where(x => x.OLMID == EL.PositionofGlobeLE).Select(x => x.ParentDescription).FirstOrDefault(),
                                     BaseReadingRE = EL.BaseReadingRE,
                                     BaseReadingLE = EL.BaseReadingLE,
                                     ReadingsfortheeyeRE = EL.ReadingsfortheeyeRE,
                                     ReadingsfortheeyeLE = EL.ReadingsfortheeyeLE,
                                     FingerInsinuationTestRE = EL.FingerInsinuationTestRE,
                                     FingerInsinuationTestLE = EL.FingerInsinuationTestLE,
                                     LocationRE = onelinemaster.Where(x => x.OLMID == EL.LocationRE).Select(x => x.ParentDescription).FirstOrDefault(),
                                     LocationLE = onelinemaster.Where(x => x.OLMID == EL.LocationLE).Select(x => x.ParentDescription).FirstOrDefault(),
                                     ShapeRE = onelinemaster.Where(x => x.OLMID == EL.ShapeRE).Select(x => x.ParentDescription).FirstOrDefault(),
                                     ShapeLE = onelinemaster.Where(x => x.OLMID == EL.ShapeLE).Select(x => x.ParentDescription).FirstOrDefault(),
                                     TextureRE = onelinemaster.Where(x => x.OLMID == EL.TextureRE).Select(x => x.ParentDescription).FirstOrDefault(),
                                     TextureLE = onelinemaster.Where(x => x.OLMID == EL.TextureLE).Select(x => x.ParentDescription).FirstOrDefault(),
                                     SizeRE = onelinemaster.Where(x => x.OLMID == EL.SizeRE).Select(x => x.ParentDescription).FirstOrDefault(),
                                     SizeLE = onelinemaster.Where(x => x.OLMID == EL.SizeLE).Select(x => x.ParentDescription).FirstOrDefault(),
                                     Remarks = EL.Remarks,
                                     GradeofThyroidEyeDiseaseRE = onelinemaster.Where(x => x.OLMID == EL.GradeofThyroidEyeDiseaseRE).Select(x => x.ParentDescription).FirstOrDefault(),
                                     GradeofThyroidEyeDiseaseLE = onelinemaster.Where(x => x.OLMID == EL.GradeofThyroidEyeDiseaseLE).Select(x => x.ParentDescription).FirstOrDefault(),
                                     LocationTextRE = EL.LocationTextRE,
                                     LocationTextLE = EL.LocationTextLE,
                                     ShapeTextRE = EL.ShapeTextRE,
                                     ShapeTextLE = EL.ShapeTextLE,
                                     TextureTextRE = EL.TextureTextRE,
                                     TextureTextLE = EL.TextureTextLE,
                                     SizeTextRE = EL.SizeTextRE,
                                     SizeTextLE = EL.SizeTextLE,
                                     SizeTextREVer = EL.SizeTextREVer,
                                     SizeTextLEVer = EL.SizeTextLEVer,
                                     FingerInsinuationRE = onelinemaster.Where(x => x.OLMID == EL.SizeRE).Select(x => x.ParentDescription).FirstOrDefault(),
                                     FingerInsinuationLE = onelinemaster.Where(x => x.OLMID == EL.SizeLE).Select(x => x.ParentDescription).FirstOrDefault(),
                                 }).ToList();

            fnd.Slitlatesthich = (from REF in WYNKContext.Findings.AsNoTracking().Where(x => x.RandomUniqueID == id)
                                  join SLT in slitlamp.Where(x => x.BasicDescriptionRE == "" && x.BasicDescriptionLE == "")
                                  on REF.RandomUniqueID equals SLT.FindingID


                                  select new Slitlatesthich
                                  {
                                      slitlampname = slitlampmaster.Where(x => x.ID == SLT.SlitLampID).Select(x => x.ParentDescription).FirstOrDefault(),
                                      slitlamplinename = slitlampmaster.Where(x => x.ID == SLT.SlitLampLineItemID).Select(x => x.ParentDescription).FirstOrDefault(),
                                      propertyname = slitlampmaster.Where(x => x.ID == SLT.SlitProperty).Select(x => x.ParentDescription).FirstOrDefault(),
                                      description = SLT.Description,
                                      OdLat = SLT.IsOD == true ? true : false,
                                      OsLat = SLT.IsOS == true ? true : false,
                                      OuLat = SLT.IsOU == true ? true : false,

                                  }).ToList();
            fnd.Slitlatesthichn = (from REF in WYNKContext.Findings.AsNoTracking().Where(x => x.RandomUniqueID == id)
                                   join SLT in slitlamp.Where(x => x.BasicDescriptionRE != "" || x.BasicDescriptionLE != "")
                                   on REF.RandomUniqueID equals SLT.FindingID


                                   select new Slitlatesthichn
                                   {
                                       slit = slitlampmaster.Where(x => x.ID == SLT.SlitLampID).Select(x => x.ParentDescription).FirstOrDefault(),
                                       redesc = SLT.BasicDescriptionRE,
                                       ledesc = SLT.BasicDescriptionLE,
                                       description = SLT.Description,


                                   }).ToList();

            fnd.Slitlatesthichnn = (from REF in WYNKContext.Findings.AsNoTracking().Where(x => x.RandomUniqueID == id)
                                    join SLT in slitlamp.Where(x => x.BasicDescriptionRE != "" || x.BasicDescriptionLE != "" || x.Description != "")
                                    on REF.RandomUniqueID equals SLT.FindingID


                                    select new Slitlatesthichnn
                                    {
                                        redesc = SLT.BasicDescriptionRE,
                                        ledesc = SLT.BasicDescriptionLE,
                                        description = SLT.Description,


                                    }).ToList();


            fnd.Funlatesthich = (from REF in WYNKContext.Findings.AsNoTracking().Where(x => x.RandomUniqueID == id)
                                 join SLT in funduss.Where(x => x.BasicDescriptionRE == "" && x.BasicDescriptionLE == "")
                                 on REF.RandomUniqueID equals SLT.FindingID


                                 select new Funlatesthich
                                 {
                                     fundusname = fundusmaster.Where(x => x.ID == SLT.FundusID).Select(x => x.ParentDescription).FirstOrDefault(),
                                     funduslinename = fundusmaster.Where(x => x.ID == SLT.FundusLineItemID).Select(x => x.ParentDescription).FirstOrDefault(),
                                     propertyname = fundusmaster.Where(x => x.ID == SLT.FundusProperty).Select(x => x.ParentDescription).FirstOrDefault(),
                                     description = SLT.Description,
                                     OdLat = SLT.IsOD == true ? true : false,
                                     OsLat = SLT.IsOS == true ? true : false,
                                     OuLat = SLT.IsOU == true ? true : false,

                                 }).ToList();

            fnd.Funlatesthichn = (from REF in WYNKContext.Findings.AsNoTracking().Where(x => x.RandomUniqueID == id)
                                  join SLT in funduss.Where(x => x.BasicDescriptionRE != "" || x.BasicDescriptionLE != "")
                                  on REF.RandomUniqueID equals SLT.FindingID


                                  select new Funlatesthichn
                                  {
                                      fundus = fundusmaster.Where(x => x.ID == SLT.FundusID).Select(x => x.ParentDescription).FirstOrDefault(),
                                      redesc = SLT.BasicDescriptionRE,
                                      ledesc = SLT.BasicDescriptionLE,
                                      description = SLT.Description,

                                  }).ToList();

            fnd.Funlatesthichnn = (from REF in WYNKContext.Findings.AsNoTracking().Where(x => x.RandomUniqueID == id)
                                   join SLT in funduss.Where(x => x.BasicDescriptionRE != "" || x.BasicDescriptionLE != "" || x.Description != "")
                                   on REF.RandomUniqueID equals SLT.FindingID


                                   select new Funlatesthichnn
                                   {
                                       redesc = SLT.BasicDescriptionRE,
                                       ledesc = SLT.BasicDescriptionLE,
                                       description = SLT.Description,

                                   }).ToList();


            fnd.Diaglatehich = (from FND in WYNKContext.Findings.AsNoTracking().Where(u => u.RandomUniqueID == id)
                                join DIG in diagniosis
                                on FND.RandomUniqueID equals DIG.FindingsID
                                join OLM in specialitymaster.Where(u => u.IsActive == true && u.IsDeleted == false && u.CmpID == cidd)
                                on DIG.DiagnosisId equals OLM.ID



                                select new Diaglatehich
                                {
                                    IcdCode2 = DIG.DiagnosisId,
                                    IcdCodess2 = OLM.Code,
                                    IcdDesc2 = OLM.ParentDescription,
                                    Desc = DIG.Description,
                                    Odd2 = DIG.IsOD == true ? true : false,
                                    Oss2 = DIG.IsOS == true ? true : false,
                                    Ouu2 = DIG.IsOU == true ? true : false,

                                }).ToList();

            var invpres = WYNKContext.InvestigationPrescription.AsNoTracking().ToList();
            var invprestran = WYNKContext.InvestigationPrescriptionTran.AsNoTracking().ToList();
            var speciality = WYNKContext.ICDSpecialityCode.AsNoTracking().ToList();
            var services = WYNKContext.Services.AsNoTracking().ToList();

            var invid = WYNKContext.InvestigationPrescription.AsNoTracking().Where(x => x.RegistrationTranID == RID).OrderByDescending(x => x.CreatedUTC).Select(x => x.RandomUniqueID).FirstOrDefault();


            fnd.invprehis = (from IP in invpres.Where(u => u.RandomUniqueID == invid)
                             join IPT in invprestran
                             on IP.RandomUniqueID equals IPT.IPID
                             join OLM in services
                             on IPT.InvestigationID equals OLM.ID
                             join SP in speciality.Where(u => u.IsActive == true)
                             on IPT.SpecialityID equals SP.ID



                             select new invprehis
                             {
                                 speciality = SP.SpecialityDescription,
                                 testdesc = OLM.Description,
                                 remarks = IP.Remarks,
                                 //amount = OLM.Amount,

                             }).ToList();

            var med = WYNKContext.MedicalPrescription.AsNoTracking().ToList();
            var medtran = WYNKContext.MedicalPrescriptionTran.AsNoTracking().ToList();
            var drug = WYNKContext.DrugMaster.AsNoTracking().ToList();
            var drugrp = WYNKContext.DrugGroup.AsNoTracking().ToList();
            var mdid = WYNKContext.MedicalPrescription.AsNoTracking().Where(x => x.RegistrationTranID == RID).OrderByDescending(x => x.CreatedUTC).Select(x => x.MedicalPrescriptionID).FirstOrDefault();


            fnd.MedDatas = (from MP in med.Where(x => x.MedicalPrescriptionID == mdid)
                            join MPT in medtran
                            on MP.RandomUniqueID equals MPT.MedicalPrescriptionID


                            select new MedDatas
                            {
                                DrugName = MPT.Brand,
                                Eye = MPT.Eye,
                                Dose = MPT.Dossage,
                                Freq = MPT.Frequency,
                                Foodd = MPT.Food,
                                Tdays = Convert.ToInt32(MPT.Days),
                                Quant = MPT.Quantity,
                            }).ToList();


            var OLMM = CMPSContext.OneLineMaster.AsNoTracking().Where(x => x.ParentTag == "SystemicCondition").ToList();
            var PH = WYNKContext.PatientHistory.AsNoTracking().Where(x => x.IsActive == true && x.RegistrationTranID == RID).ToList();
            DateTime now = DateTime.Now;
            fnd.PatientHistorys = PH.Select(s => new PatientHistoryDetails
            {
                ID = s.ID,
                Description = OLMM.Where(x => x.OLMID == s.Description).Select(x => x.ParentDescription).FirstOrDefault(),
                FromUTC = s.FromUTC,
                Duration = MonthDifference(Convert.ToDateTime(s.FromUTC), now) + " Months"



            }).ToList();

            var PACExam = WYNKContext.PACExamination.AsNoTracking().ToList();
            fnd.getPACExamDetails = (from PACE in PACExam.Where(x => x.RegistrationTranID == RID)


                                     select new getPACExamDetails
                                     {
                                         PACExamID = PACE.PACExamID,
                                         PulseHeartRate = PACE.PulseHeartRate,
                                         Respiration = PACE.Respiration,
                                         BloodPressure = PACE.BloodPressure,
                                         temperature = PACE.temperature,
                                         tempunit = PACE.TempUnit,
                                         Weight = PACE.Weight,
                                         Height = PACE.Height,
                                         BMI = PACE.BMI,
                                     }).ToList();

            var bh = WYNKContext.BirthHistory.AsNoTracking().ToList();

            fnd.getbhDetails = (from b in bh.Where(x => x.RegistrationTranID == RID)


                                select new getbhDetails
                                {
                                    delivery = b.DeliveryType,
                                    weight = b.Weight,
                                    unit = b.Unit,
                                    vac = b.Vaccination,
                                    gad = b.GrowthDev,
                                    transplant = b.BloodTransplant,
                                    therapy = b.OxygenTherapy,
                                    illness = b.criticalillness,
                                }).ToList();


            var docmass = CMPSContext.DoctorMaster.AsNoTracking().ToList();
            var syfddt = WYNKContext.SYRINGEFDDT.ToList();

            fnd.SchirmerTestcus = (from sc in WYNKContext.SchirmerTest.AsNoTracking().Where(x => x.RID == RID && x.IsActive == true)

                                   select new SchirmerTestcus
                                   {

                                       VisitDatetime = sc.VisitDatetime,
                                       Ocular = sc.Ocular,
                                       Time = sc.Time,
                                       Tearsecretion = sc.Tearsecretion,
                                       Examinedbyname = docmass.Where(x => x.DoctorID == sc.Examinedby).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                       Remarks = sc.Remarks,

                                   }).ToList();

            var specialitymas = WYNKContext.SpecialityMaster.ToList();


            fnd.syringehis = (from sy in syfddt.Where(x => x.REGTRANID == RID && x.IsActive == true)
                              join olm in specialitymas.Where(x => x.ParentTag == "SYRINGING" && x.IsActive == true) on sy.FDDTSYRINGEID equals olm.ID

                              select new syringehis
                              {

                                  desc = olm.ParentDescription,
                                  eye = sy.Eye,
                                  regurgitation = sy.REGURGITATION,
                                  fluid = sy.FLUID,
                                  stop = sy.Stop,
                                  docname = docmass.Where(x => x.DoctorID == sy.DoctorID).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                  remarks = sy.REMARKS,
                                  vdate = sy.VISITDATE.Add(ts),

                              }).ToList();

            fnd.fddthis = (from sy in syfddt.Where(x => x.REGTRANID == RID && x.IsActive == true)
                           join olm in specialitymas.Where(x => x.ParentTag == "FDDT" && x.IsActive == true) on sy.FDDTSYRINGEID equals olm.ID

                           select new fddthis
                           {
                               desc = olm.ParentDescription,
                               eye = sy.Eye,
                               docname = docmass.Where(x => x.DoctorID == sy.DoctorID).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                               remarks = sy.REMARKS,
                               vdate = sy.VISITDATE.Add(ts),

                           }).ToList();





            var glaucomaeval = WYNKContext.GlaucomaEvaluation.AsNoTracking().ToList();

            fnd.glvalch = (from GE in glaucomaeval.Where(x => x.RID == RID && x.IsActive == true)


                           select new glvalch
                           {
                               GDate = GE.Date,
                               Drugs = GE.GlaucomaDrugs,
                               hvf = GE.HVF,
                               oct = GE.OCT,
                               intervention = GE.Intervention,
                               status = GE.StableProgression,

                           }).ToList();

            var fnext = WYNKContext.FindingsExt.AsNoTracking().ToList();
            var icspec = WYNKContext.ICDSpecialityCode.AsNoTracking().ToList();
            var icd = WYNKContext.ICDMaster.AsNoTracking().ToList();
            var squintextn = WYNKContext.SquintExt.AsNoTracking().ToList();
            var squintextnmaster = WYNKContext.SquintExtnMaster.AsNoTracking().ToList();
            var slitlampextn = WYNKContext.SlitlampExtn.AsNoTracking().ToList();

            fnd.Surlatestch = (from FND in WYNKContext.Findings.AsNoTracking().Where(x => x.RandomUniqueID == id)
                               join FE in fnext
                               on FND.RandomUniqueID equals FE.FindingsID
                               join ISP in icspec
                               on FE.ICDSpecialityid equals ISP.ID


                               select new Surlatestch
                               {
                                   ICDSpec = ISP.SpecialityDescription,
                                   IcdDesc = icd.Where(x => x.ICDCODE == FE.ICDCode).Select(x => x.ICDDESCRIPTION).FirstOrDefault(),
                                   OdSur = FE.IsOD == true ? true : false,
                                   OsSur = FE.IsOS == true ? true : false,
                                   OuSur = FE.IsOU == true ? true : false,
                                   sdate = FND.CreatedUTC.Date,

                               }).ToList();

            fnd.ocurechisch = (from FND in WYNKContext.Findings.AsNoTracking().Where(x => x.RandomUniqueID == id)
                               join SLE in slitlampextn
                               on FND.RandomUniqueID equals SLE.FindingsID


                               select new ocurechisch
                               {
                                   HorMeasurementod1 = SLE.HorMeasurementod1,
                                   VerMeasurementod1 = SLE.VerMeasurementod1,
                                   HorMeasurementod2 = SLE.HorMeasurementod2,
                                   VerMeasurementod2 = SLE.VerMeasurementod2,
                                   HorMeasurementod3 = SLE.HorMeasurementod3,
                                   VerMeasurementod3 = SLE.VerMeasurementod3,
                                   HorMeasurementod4 = SLE.HorMeasurementod4,
                                   VerMeasurementod4 = SLE.VerMeasurementod4,
                                   HorMeasurementod5 = SLE.HorMeasurementod5,
                                   VerMeasurementod5 = SLE.VerMeasurementod5,
                                   HorMeasurementod6 = SLE.HorMeasurementod6,
                                   VerMeasurementod6 = SLE.VerMeasurementod6,

                                   HorMeasurementos1 = SLE.HorMeasurementos1,
                                   VerMeasurementos1 = SLE.VerMeasurementos1,
                                   HorMeasurementos2 = SLE.HorMeasurementos2,
                                   VerMeasurementos2 = SLE.VerMeasurementos2,
                                   HorMeasurementos3 = SLE.HorMeasurementos3,
                                   VerMeasurementos3 = SLE.VerMeasurementos3,
                                   HorMeasurementos4 = SLE.HorMeasurementos4,
                                   VerMeasurementos4 = SLE.VerMeasurementos4,
                                   HorMeasurementos5 = SLE.HorMeasurementos5,
                                   VerMeasurementos5 = SLE.VerMeasurementos5,
                                   HorMeasurementos6 = SLE.HorMeasurementos6,
                                   VerMeasurementos6 = SLE.VerMeasurementos6,

                                   OcularMovementod1 = squintextnmaster.Where(x => x.ID == SLE.OcularMovementod1).Select(x => x.ParentDescription).FirstOrDefault(),
                                   OcularMovementod2 = squintextnmaster.Where(x => x.ID == SLE.OcularMovementod2).Select(x => x.ParentDescription).FirstOrDefault(),
                                   OcularMovementod3 = squintextnmaster.Where(x => x.ID == SLE.OcularMovementod3).Select(x => x.ParentDescription).FirstOrDefault(),
                                   OcularMovementod4 = squintextnmaster.Where(x => x.ID == SLE.OcularMovementod4).Select(x => x.ParentDescription).FirstOrDefault(),
                                   OcularMovementod5 = squintextnmaster.Where(x => x.ID == SLE.OcularMovementod5).Select(x => x.ParentDescription).FirstOrDefault(),
                                   OcularMovementod6 = squintextnmaster.Where(x => x.ID == SLE.OcularMovementod6).Select(x => x.ParentDescription).FirstOrDefault(),
                                   OcularMovementod7 = squintextnmaster.Where(x => x.ID == SLE.OcularMovementod7).Select(x => x.ParentDescription).FirstOrDefault(),
                                   OcularMovementod8 = squintextnmaster.Where(x => x.ID == SLE.OcularMovementod8).Select(x => x.ParentDescription).FirstOrDefault(),

                                   OcularMovementos1 = squintextnmaster.Where(x => x.ID == SLE.OcularMovementos1).Select(x => x.ParentDescription).FirstOrDefault(),
                                   OcularMovementos2 = squintextnmaster.Where(x => x.ID == SLE.OcularMovementos2).Select(x => x.ParentDescription).FirstOrDefault(),
                                   OcularMovementos3 = squintextnmaster.Where(x => x.ID == SLE.OcularMovementos3).Select(x => x.ParentDescription).FirstOrDefault(),
                                   OcularMovementos4 = squintextnmaster.Where(x => x.ID == SLE.OcularMovementos4).Select(x => x.ParentDescription).FirstOrDefault(),
                                   OcularMovementos5 = squintextnmaster.Where(x => x.ID == SLE.OcularMovementos5).Select(x => x.ParentDescription).FirstOrDefault(),
                                   OcularMovementos6 = squintextnmaster.Where(x => x.ID == SLE.OcularMovementos6).Select(x => x.ParentDescription).FirstOrDefault(),
                                   OcularMovementos7 = squintextnmaster.Where(x => x.ID == SLE.OcularMovementos7).Select(x => x.ParentDescription).FirstOrDefault(),
                                   OcularMovementos8 = squintextnmaster.Where(x => x.ID == SLE.OcularMovementos8).Select(x => x.ParentDescription).FirstOrDefault(),

                               }).ToList();

            fnd.squinthischis = (from FND in WYNKContext.Findings.AsNoTracking().Where(x => x.RandomUniqueID == id)
                                 join SEM in squintextn
                                 on FND.RandomUniqueID equals SEM.FindingsID


                                 select new squinthischis
                                 {


                                     AngleKappa = squintextnmaster.Where(x => x.ID == SEM.AngleKappa).Select(x => x.ParentDescription).FirstOrDefault(),
                                     Patterns = squintextnmaster.Where(x => x.ID == SEM.Patterns).Select(x => x.ParentDescription).FirstOrDefault(),
                                     ACAMethod = squintextnmaster.Where(x => x.ID == SEM.ACAMethod).Select(x => x.ParentDescription).FirstOrDefault(),
                                     ACAValue = squintextnmaster.Where(x => x.ID == SEM.ACAValue).Select(x => x.ParentDescription).FirstOrDefault(),
                                     WFDTDistance = squintextnmaster.Where(x => x.ID == SEM.WFDTDistance).Select(x => x.ParentDescription).FirstOrDefault(),
                                     WFDTNear = squintextnmaster.Where(x => x.ID == SEM.WFDTNear).Select(x => x.ParentDescription).FirstOrDefault(),
                                     StreopsisMethod = squintextnmaster.Where(x => x.ID == SEM.StreopsisMethod).Select(x => x.ParentDescription).FirstOrDefault(),
                                     StreopsisValue = squintextnmaster.Where(x => x.ID == SEM.StreopsisValue).Select(x => x.ParentDescription).FirstOrDefault(),

                                     ARC = squintextnmaster.Where(x => x.ID == SEM.ARC).Select(x => x.ParentDescription).FirstOrDefault(),

                                     PBCTDisHor = squintextnmaster.Where(x => x.ID == SEM.PBCTDisHor).Select(x => x.ParentDescription).FirstOrDefault(),
                                     PBCTDisVer = squintextnmaster.Where(x => x.ID == SEM.PBCTDisVer).Select(x => x.ParentDescription).FirstOrDefault(),
                                     PBCTNearHor = squintextnmaster.Where(x => x.ID == SEM.PBCTNearHor).Select(x => x.ParentDescription).FirstOrDefault(),
                                     PBCTNearVer = squintextnmaster.Where(x => x.ID == SEM.PBCTNearVer).Select(x => x.ParentDescription).FirstOrDefault(),
                                     ModKrimHor = squintextnmaster.Where(x => x.ID == SEM.ModKrimHor).Select(x => x.ParentDescription).FirstOrDefault(),
                                     ModKrimVer = squintextnmaster.Where(x => x.ID == SEM.ModKrimVer).Select(x => x.ParentDescription).FirstOrDefault(),
                                     PriDevHor = squintextnmaster.Where(x => x.ID == SEM.PriDevHor).Select(x => x.ParentDescription).FirstOrDefault(),

                                     PriDevVer = squintextnmaster.Where(x => x.ID == SEM.PriDevVer).Select(x => x.ParentDescription).FirstOrDefault(),
                                     SecDevHor = squintextnmaster.Where(x => x.ID == SEM.SecDevHor).Select(x => x.ParentDescription).FirstOrDefault(),
                                     SecDevVer = squintextnmaster.Where(x => x.ID == SEM.SecDevVer).Select(x => x.ParentDescription).FirstOrDefault(),
                                     Amplitude = squintextnmaster.Where(x => x.ID == SEM.Amplitude).Select(x => x.ParentDescription).FirstOrDefault(),
                                     Frequency = squintextnmaster.Where(x => x.ID == SEM.Frequency).Select(x => x.ParentDescription).FirstOrDefault(),
                                     Type = squintextnmaster.Where(x => x.ID == SEM.Type).Select(x => x.ParentDescription).FirstOrDefault(),
                                     Pursuit = SEM.Pursuit,
                                     Saccade = SEM.Saccade,

                                     ABHeadPos = squintextnmaster.Where(x => x.ID == SEM.ABHeadPos).Select(x => x.ParentDescription).FirstOrDefault(),
                                     FreqOnConvergence = squintextnmaster.Where(x => x.ID == SEM.FreqOnConvergence).Select(x => x.ParentDescription).FirstOrDefault(),
                                     Occlusion = squintextnmaster.Where(x => x.ID == SEM.Occlusion).Select(x => x.ParentDescription).FirstOrDefault(),
                                     Oscillopsia = squintextnmaster.Where(x => x.ID == SEM.Oscillopsia).Select(x => x.ParentDescription).FirstOrDefault(),

                                     PBCTDisHorValue = SEM.PBCTDisHorValue,
                                     PBCTDisVerValue = SEM.PBCTDisVerValue,
                                     PBCTNearHorValue = SEM.PBCTNearHorValue,
                                     PBCTNearVerValue = SEM.PBCTNearVerValue,
                                     ModKrimHorValue = SEM.ModKrimHorValue,
                                     ModKrimVerValue = SEM.ModKrimVerValue,

                                     PriDevHorValue = SEM.PriDevHorValue,
                                     PriDevVerValue = SEM.PriDevVerValue,
                                     SecDevHorValue = SEM.SecDevHorValue,
                                     SecDevVerValue = SEM.SecDevVerValue,

                                     ConjugateDissociated = SEM.ConjugateDissociated,
                                     AssHeadPosture = SEM.AssHeadPosture,
                                     SquintBasicExam = SEM.SquintBasicExam,
                                     SpecialTest = SEM.SpecialTest,

                                     VF1 = squintextnmaster.Where(x => x.ID == SEM.VF1).Select(x => x.ParentDescription).FirstOrDefault(),
                                     VF2 = squintextnmaster.Where(x => x.ID == SEM.VF2).Select(x => x.ParentDescription).FirstOrDefault(),
                                     VF3 = squintextnmaster.Where(x => x.ID == SEM.VF3).Select(x => x.ParentDescription).FirstOrDefault(),
                                     VF4 = squintextnmaster.Where(x => x.ID == SEM.VF4).Select(x => x.ParentDescription).FirstOrDefault(),

                                     VF1Value = SEM.VF1Value,
                                     VF2Value = SEM.VF2Value,
                                     VF3Value = SEM.VF3Value,
                                     VF4Value = SEM.VF4Value,
                                 }).ToList();

            fnd.treatment = WYNKContext.Findings.AsNoTracking().Where(x => x.RandomUniqueID == id).Select(x => x.TreatmentAdvice).FirstOrDefault();
            fnd.pfc = WYNKContext.Findings.AsNoTracking().Where(x => x.RandomUniqueID == id).Select(x => x.PFC).FirstOrDefault();

            var user = CMPSContext.Users.AsNoTracking().ToList();
            var docmas = CMPSContext.DoctorMaster.AsNoTracking().ToList();
            var userrole = CMPSContext.UserVsRole.AsNoTracking().ToList();
            var vs = WYNKContext.VISUALACUITY.AsNoTracking().ToList();
            var pgp = WYNKContext.PGP.AsNoTracking().ToList();
            var Rec = WYNKContext.REFRACTIONEXT.AsNoTracking().ToList();
            var TID = WYNKContext.Refraction.AsNoTracking().Where(u => u.RegistrationTranID == RID).GroupBy(x => x.RegistrationTranID).ToList();

            foreach (var i in TID.ToList())
            {
                var rid = i.Key;

                var qry = from va in vs.Where(u => u.RegistrationTranID == rid && u.SubCategory != 0)
                          join use in user on
                          va.CreatedBy equals use.Userid
                          join doc in docmas on use.Username equals doc.EmailID
                          join usr in userrole on
                          use.Username equals usr.UserName

                          select new
                          {
                              VDate = va.Date.Add(ts),
                              Name = doc.Firstname + " " + doc.MiddleName + " " + doc.LastName,
                              Role = usr.RoleDescription,
                              VDescription = va.Description != null ? va.Description : string.Empty,
                              SubCategory = onelinemaster.Where(x => x.OLMID == va.SubCategory).Select(x => x.ParentDescription).FirstOrDefault(),
                              VOcular = va.Ocular != null ? va.Ocular : string.Empty,
                              DistSph = va.DistSph != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.DistSph)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                              NearCyl = va.NearCyl != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.NearCyl)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                              VPowerGlass = va.PowerGlass != "" ? va.PowerGlass : string.Empty,
                              NVDESC = va.N_V_DESC != "" ? va.N_V_DESC : string.Empty,
                              PinAxis = va.PinAxis != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.PinAxis)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                              createdby = va.CreatedBy,
                              Ocularr = va.Ocular,
                              rid = va.RegistrationTranID,
                              ChartType = va.ChartType != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.ChartType)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                              Remarks = va.Remarks != "" ? va.Remarks : string.Empty,
                          };


                var qryAdmin = from va in vs.Where(u => u.RegistrationTranID == rid && u.SubCategory != 0)
                               join use in user on
                               va.CreatedBy equals use.Userid
                               join userole in userrole on
                                use.Userid equals userole.UserID

                               select new
                               {
                                   VDate = va.Date.Add(ts),
                                   Name = use.Username,
                                   Role = userole.RoleDescription,
                                   VDescription = va.Description != null ? va.Description : string.Empty,
                                   SubCategory = onelinemaster.Where(x => x.OLMID == va.SubCategory).Select(x => x.ParentDescription).FirstOrDefault(),
                                   VOcular = va.Ocular != null ? va.Ocular : string.Empty,
                                   DistSph = va.DistSph != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.DistSph)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                   NearCyl = va.NearCyl != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.NearCyl)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                   VPowerGlass = va.PowerGlass != "" ? va.PowerGlass : string.Empty,
                                   NVDESC = va.N_V_DESC != "" ? va.N_V_DESC : string.Empty,
                                   PinAxis = va.PinAxis != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.PinAxis)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                   createdby = va.CreatedBy,
                                   Ocularr = va.Ocular,
                                   rid = va.RegistrationTranID,
                                   ChartType = va.ChartType != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.ChartType)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                   Remarks = va.Remarks != "" ? va.Remarks : string.Empty,
                               };


                var grpres = qry.Concat(qryAdmin).ToList();

                if (grpres.Count != 0)
                {

                    fnd.Refractioninfolach = (from res in grpres.GroupBy(x => x.createdby)
                                              select new Refractioninfolach
                                              {
                                                  Description = res.Select(x => x.VDescription).FirstOrDefault(),
                                                  SubCategory = res.Where(x => x.Ocularr == "OD").Select(x => x.SubCategory).FirstOrDefault(),
                                                  SubCatgry = res.Where(x => x.Ocularr == "OS").Select(x => x.SubCategory).FirstOrDefault(),
                                                  DistSph = res.Where(x => x.Ocularr == "OD").Select(x => x.DistSph).FirstOrDefault(),
                                                  NearCyl = res.Where(x => x.Ocularr == "OD").Select(x => x.NearCyl).FirstOrDefault(),
                                                  PinAxis = res.Where(x => x.Ocularr == "OD").Select(x => x.PinAxis).FirstOrDefault(),
                                                  N_V_DESC = res.Where(x => x.Ocularr == "OD").Select(x => x.NVDESC).FirstOrDefault(),
                                                  PowerGlass = res.Where(x => x.Ocularr == "OD").Select(x => x.VPowerGlass).FirstOrDefault(),
                                                  DistSphh = res.Where(x => x.Ocularr == "OS").Select(x => x.DistSph).FirstOrDefault(),
                                                  NearCyll = res.Where(x => x.Ocularr == "OS").Select(x => x.NearCyl).FirstOrDefault(),
                                                  PinAxiss = res.Where(x => x.Ocularr == "OS").Select(x => x.PinAxis).FirstOrDefault(),
                                                  N_V_DESCC = res.Where(x => x.Ocularr == "OS").Select(x => x.NVDESC).FirstOrDefault(),
                                                  PowerGlasss = res.Where(x => x.Ocularr == "OS").Select(x => x.VPowerGlass).FirstOrDefault(),
                                                  Name = res.Select(x => x.Name).FirstOrDefault(),
                                                  Role = res.Select(x => x.Role).FirstOrDefault(),
                                                  Date = res.Select(x => x.VDate).FirstOrDefault(),
                                                  ChartType = res.Select(x => x.ChartType).LastOrDefault(),
                                                  Remarks = res.Select(x => x.Remarks).LastOrDefault(),

                                              }).ToList();





                }

            }

            fnd.Historyvisualacuitypaediatric = new List<Historyvisualacuitypaediatric>();



            foreach (var i in TID.ToList())
            {
                var rid = i.Key;

                var qry = from va in vs.Where(u => u.RegistrationTranID == rid && u.SubCategory == 0)
                          join use in user on
                          va.CreatedBy equals use.Userid
                          join doc in docmas on use.Username equals doc.EmailID
                          join usr in userrole on
                           use.Userid equals usr.UserID

                          select new
                          {
                              VDate = va.Date.Add(ts),
                              Name = doc.Firstname + " " + doc.MiddleName + " " + doc.LastName,
                              Role = usr.RoleDescription,
                              VDescription = va.Description != null ? va.Description : string.Empty,
                              VOcular = va.Ocular != null ? va.Ocular : string.Empty,
                              Central = va.Central != false ? va.Central : false,
                              Steady = va.Steady != false ? va.Steady : false,
                              Maintained = va.Maintained != false ? va.Maintained : false,
                              Uncentral = va.Uncentral != false ? va.Uncentral : false,
                              Unsteady = va.Unsteady != false ? va.Unsteady : false,
                              Unmaintained = va.Unmaintained != false ? va.Unmaintained : false,
                              CentralOS = va.Central != false ? va.Central : false,
                              SteadyOS = va.Steady != false ? va.Steady : false,
                              MaintainedOS = va.Maintained != false ? va.Maintained : false,
                              UncentralOS = va.Uncentral != false ? va.Uncentral : false,
                              UnsteadyOS = va.Unsteady != false ? va.Unsteady : false,
                              UnmaintainedOS = va.Unmaintained != false ? va.Unmaintained : false,
                              createdby = va.CreatedBy,
                              Ocularr = va.Ocular,
                              rid = va.RegistrationTranID,
                              ChartType = va.ChartType != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.ChartType)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                              Remarks = va.Remarks != "" ? va.Remarks : string.Empty,
                          };

                var qryAdmin = from va in vs.Where(u => u.RegistrationTranID == rid && u.SubCategory == 0)
                               join use in user on
                               va.CreatedBy equals use.Userid
                               join userole in userrole on
                               use.Userid equals userole.UserID

                               select new
                               {
                                   VDate = va.Date.Add(ts),
                                   Name = use.Username,
                                   Role = userole.RoleDescription,
                                   VDescription = va.Description != null ? va.Description : string.Empty,
                                   VOcular = va.Ocular != null ? va.Ocular : string.Empty,
                                   Central = va.Central != false ? va.Central : false,
                                   Steady = va.Steady != false ? va.Steady : false,
                                   Maintained = va.Maintained != false ? va.Maintained : false,
                                   Uncentral = va.Uncentral != false ? va.Uncentral : false,
                                   Unsteady = va.Unsteady != false ? va.Unsteady : false,
                                   Unmaintained = va.Unmaintained != false ? va.Unmaintained : false,
                                   CentralOS = va.Central != false ? va.Central : false,
                                   SteadyOS = va.Steady != false ? va.Steady : false,
                                   MaintainedOS = va.Maintained != false ? va.Maintained : false,
                                   UncentralOS = va.Uncentral != false ? va.Uncentral : false,
                                   UnsteadyOS = va.Unsteady != false ? va.Unsteady : false,
                                   UnmaintainedOS = va.Unmaintained != false ? va.Unmaintained : false,
                                   createdby = va.CreatedBy,
                                   Ocularr = va.Ocular,
                                   rid = va.RegistrationTranID,
                                   ChartType = va.ChartType != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.ChartType)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                   Remarks = va.Remarks != "" ? va.Remarks : string.Empty,
                               };




                var grpres = qry.Count() > 0 ? qry.ToList() : qryAdmin.ToList();

                if (grpres.Count != 0)
                {

                    fnd.Historyvisualacuitypaediatric1 = (from res in grpres.GroupBy(x => x.createdby)
                                                          select new Historyvisualacuitypaediatric1
                                                          {
                                                              Description = res.Select(x => x.VDescription).LastOrDefault(),
                                                              Central = res.Where(x => x.Ocularr == "OD").Select(x => x.Central).LastOrDefault(),
                                                              Steady = res.Where(x => x.Ocularr == "OD").Select(x => x.Steady).LastOrDefault(),
                                                              Maintained = res.Where(x => x.Ocularr == "OD").Select(x => x.Maintained).LastOrDefault(),
                                                              Uncentral = res.Where(x => x.Ocularr == "OD").Select(x => x.Uncentral).LastOrDefault(),
                                                              Unsteady = res.Where(x => x.Ocularr == "OD").Select(x => x.Unsteady).LastOrDefault(),
                                                              Unmaintained = res.Where(x => x.Ocularr == "OD").Select(x => x.Unmaintained).LastOrDefault(),
                                                              CentralOS = res.Where(x => x.Ocularr == "OS").Select(x => x.CentralOS).LastOrDefault(),
                                                              SteadyOS = res.Where(x => x.Ocularr == "OS").Select(x => x.SteadyOS).LastOrDefault(),
                                                              MaintainedOS = res.Where(x => x.Ocularr == "OS").Select(x => x.MaintainedOS).LastOrDefault(),
                                                              UncentralOS = res.Where(x => x.Ocularr == "OS").Select(x => x.UncentralOS).LastOrDefault(),
                                                              UnsteadyOS = res.Where(x => x.Ocularr == "OS").Select(x => x.UnsteadyOS).LastOrDefault(),
                                                              UnmaintainedOS = res.Where(x => x.Ocularr == "OS").Select(x => x.UnmaintainedOS).LastOrDefault(),
                                                              Name = res.Select(x => x.Name).LastOrDefault(),
                                                              Role = res.Select(x => x.Role).LastOrDefault(),
                                                              Date = res.Select(x => x.VDate).LastOrDefault(),
                                                              ChartType = res.Select(x => x.ChartType).LastOrDefault(),
                                                              Remarks = res.Select(x => x.Remarks).LastOrDefault(),
                                                          }).ToList();



                    foreach (var itm in fnd.Historyvisualacuitypaediatric1.ToList())
                    {
                        var Hisv = new Historyvisualacuitypaediatric();

                        Hisv.Description = itm.Description;
                        Hisv.Central = itm.Central == true ? true : false;
                        Hisv.Steady = itm.Steady == true ? true : false;
                        Hisv.Maintained = itm.Maintained == true ? true : false;
                        Hisv.Uncentral = itm.Uncentral == true ? true : false;
                        Hisv.Unsteady = itm.Unsteady == true ? true : false;
                        Hisv.Unmaintained = itm.Unmaintained == true ? true : false;
                        Hisv.CentralOS = itm.CentralOS == true ? true : false;
                        Hisv.SteadyOS = itm.SteadyOS == true ? true : false;
                        Hisv.MaintainedOS = itm.MaintainedOS == true ? true : false;
                        Hisv.UncentralOS = itm.UncentralOS == true ? true : false;
                        Hisv.UnsteadyOS = itm.UnsteadyOS == true ? true : false;
                        Hisv.UnmaintainedOS = itm.UnmaintainedOS == true ? true : false;
                        Hisv.Name = itm.Name;
                        Hisv.Role = itm.Role;
                        Hisv.Date = itm.Date;
                        Hisv.Remarks = itm.Remarks;
                        Hisv.ChartType = itm.ChartType;
                        fnd.Historyvisualacuitypaediatric.Add(Hisv);

                    }

                }

            }




            foreach (var i in TID.ToList())
            {
                var rid = i.Key;

                var qry = from va in WYNKContext.PGP.Where(u => u.RegistrationTranID == rid)
                          join us in user on
                          va.CreatedBy equals us.Userid
                          join doc in docmas on
                          us.Username equals doc.EmailID
                          join usr in userrole on
                          us.Username equals usr.UserName

                          select new
                          {
                              VDate = va.Date.Add(ts),
                              Name = doc.Firstname + " " + doc.MiddleName + " " + doc.LastName,
                              Role = usr.RoleDescription,
                              Descriptionn = va.Description,
                              Ocularr = va.Ocular,
                              DistSphh = va.DistSph,
                              NearCyll = va.NearCyl,
                              PinAxiss = va.PinAxis,
                              Addd = va.Add,
                              Detailss = va.Details,
                              createdby = va.CreatedBy,
                          };



                var qryAdmin = from va in pgp.Where(u => u.RegistrationTranID == rid)
                               join use in user on
                               va.CreatedBy equals use.Userid
                               join userole in userrole on
                                use.Userid equals userole.UserID

                               select new
                               {

                                   VDate = va.Date.Add(ts),
                                   Name = use.Username,
                                   Role = userole.RoleDescription,
                                   Descriptionn = va.Description,
                                   Ocularr = va.Ocular,
                                   DistSphh = va.DistSph,
                                   NearCyll = va.NearCyl,
                                   PinAxiss = va.PinAxis,
                                   Addd = va.Add,
                                   Detailss = va.Details,
                                   createdby = va.CreatedBy,

                               };


                var grpres = qry.Concat(qryAdmin).ToList();

                if (grpres.Count != 0)
                {
                    fnd.PGPHch = (from res in grpres.GroupBy(x => x.createdby)
                                  select new PGPHch
                                  {
                                      Description = res.Select(x => x.Descriptionn).FirstOrDefault(),
                                      DistSph = res.Where(x => x.Ocularr == "OD").Select(x => x.DistSphh).FirstOrDefault(),
                                      NearCyl = res.Where(x => x.Ocularr == "OD").Select(x => x.NearCyll).FirstOrDefault(),
                                      PinAxis = res.Where(x => x.Ocularr == "OD").Select(x => x.PinAxiss).FirstOrDefault(),
                                      Add = res.Where(x => x.Ocularr == "OD").Select(x => x.Addd).FirstOrDefault(),
                                      DistSph1 = res.Where(x => x.Ocularr == "OS").Select(x => x.DistSphh).FirstOrDefault(),
                                      NearCyl1 = res.Where(x => x.Ocularr == "OS").Select(x => x.NearCyll).FirstOrDefault(),
                                      PinAxis1 = res.Where(x => x.Ocularr == "OS").Select(x => x.PinAxiss).FirstOrDefault(),
                                      Add1 = res.Where(x => x.Ocularr == "OS").Select(x => x.Addd).FirstOrDefault(),
                                      Name = res.Select(x => x.Name).FirstOrDefault(),
                                      Role = res.Select(x => x.Role).FirstOrDefault(),
                                      Date = res.Select(x => x.VDate).FirstOrDefault(),
                                      Details = res.Select(x => x.Detailss).FirstOrDefault(),
                                  }).ToList();


                }




            }

            List<REFHch> REFHch = new List<REFHch>();


            foreach (var i in TID.ToList())
            {
                var rid = i.Key;

                var qry = from va in WYNKContext.REFRACTIONEXT.Where(u => u.RegistrationTranID == rid)
                          join us in user on
                          va.CreatedBy equals us.Userid
                          join doc in docmas on
                          us.Username equals doc.EmailID
                          join usr in userrole on
                          us.Username equals usr.UserName

                          select new
                          {
                              VDate = va.Date.Add(ts),
                              Name = doc.Firstname + " " + doc.MiddleName + " " + doc.LastName,
                              Role = usr.RoleDescription,
                              Descriptionn = va.Description,
                              Ocularr = va.Ocular,
                              createdby = va.CreatedBy,
                              disp = va.DistSph,
                              near = va.NearCyl,
                              pin = va.PinAxis,
                              typee = va.Type,
                              SubCategory = onelinemaster.Where(x => x.OLMID == va.SubCategory).Select(x => x.ParentDescription).FirstOrDefault(),
                          };

                var qryAdmin = from va in Rec.Where(u => u.RegistrationTranID == rid)
                               join use in user on
                               va.CreatedBy equals use.Userid
                               join userole in userrole on
                                use.Userid equals userole.UserID

                               select new
                               {
                                   VDate = va.Date.Add(ts),
                                   Name = use.Username,
                                   Role = userole.RoleDescription,
                                   Descriptionn = va.Description,
                                   Ocularr = va.Ocular,
                                   createdby = va.CreatedBy,
                                   disp = va.DistSph,
                                   near = va.NearCyl,
                                   pin = va.PinAxis,
                                   typee = va.Type,
                                   SubCategory = onelinemaster.Where(x => x.OLMID == va.SubCategory).Select(x => x.ParentDescription).FirstOrDefault(),
                               };

                var grpres = qry.Concat(qryAdmin).ToList();

                if (grpres.Count != 0)
                {
                    fnd.REFHch = (from res in grpres.GroupBy(item => new { item.SubCategory, item.createdby })

                                  select new REFHch
                                  {
                                      Description = res.Where(x => x.Ocularr == "OD").Select(x => x.Descriptionn).FirstOrDefault(),
                                      Name = res.Select(x => x.Name).FirstOrDefault(),
                                      Role = res.Select(x => x.Role).FirstOrDefault(),
                                      Date = res.Where(x => x.Ocularr == "OD").Select(x => x.VDate).FirstOrDefault(),
                                      SubCategory = res.Where(x => x.Ocularr == "OD").Select(x => x.SubCategory).FirstOrDefault(),
                                      SubCatgry = res.Where(x => x.Ocularr == "OS").Select(x => x.SubCategory).FirstOrDefault(),
                                      DistSph = res.Where(x => x.Ocularr == "OD" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.disp).FirstOrDefault(),
                                      NearCyl = res.Where(x => x.Ocularr == "OD" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.near).FirstOrDefault(),
                                      PinAxis = res.Where(x => x.Ocularr == "OD" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.pin).FirstOrDefault(),
                                      DistSph1 = res.Where(x => x.Ocularr == "OS" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.disp).FirstOrDefault(),
                                      NearCyl1 = res.Where(x => x.Ocularr == "OS" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.near).FirstOrDefault(),
                                      PinAxis1 = res.Where(x => x.Ocularr == "OS" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.pin).FirstOrDefault(),
                                      DistSph2 = res.Where(x => x.Ocularr == "OD" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.disp).FirstOrDefault(),
                                      NearCyl2 = res.Where(x => x.Ocularr == "OD" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.near).FirstOrDefault(),
                                      PinAxis2 = res.Where(x => x.Ocularr == "OD" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.pin).FirstOrDefault(),
                                      DistSph3 = res.Where(x => x.Ocularr == "OS" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.disp).FirstOrDefault(),
                                      NearCyl3 = res.Where(x => x.Ocularr == "OS" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.near).FirstOrDefault(),
                                      PinAxis3 = res.Where(x => x.Ocularr == "OS" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.pin).FirstOrDefault(),
                                  }).ToList();



                }




            }



            fnd.Amsler = (from res in WYNKContext.Refraction.Where(x => x.RegistrationTranID == RID && x.Description == "Amsler").OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.CreatedBy)
                          select new Amslermas
                          {
                              Description = res.Select(x => x.Description).FirstOrDefault(),
                              ID = res.Where(x => x.Ocular == "OD").Select(x => x.ID).FirstOrDefault(),
                              A_n_OD = res.Where(x => x.Ocular == "OD").Select(x => x.A_n_OD).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OD").Select(x => x.A_n_OD).FirstOrDefault() : false,
                              A_abn_OD = res.Where(x => x.Ocular == "OD").Select(x => x.A_abn_OD).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OD").Select(x => x.A_abn_OD).FirstOrDefault() : false,
                              Desc_Text = res.Where(x => x.Ocular == "OD").Select(x => x.Desc_Text).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.Desc_Text).FirstOrDefault() : string.Empty,
                              Ocular = res.Where(x => x.Ocular == "OD").Select(x => x.Ocular).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.Ocular).FirstOrDefault() : "OD",
                              IDOS = res.Where(x => x.Ocular == "OS").Select(x => x.ID).FirstOrDefault(),
                              A_n_OS = res.Where(x => x.Ocular == "OS").Select(x => x.A_n_OS).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OS").Select(x => x.A_n_OS).FirstOrDefault() : false,
                              A_abn_OS = res.Where(x => x.Ocular == "OS").Select(x => x.A_abn_OS).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OS").Select(x => x.A_abn_OS).FirstOrDefault() : false,
                              Desc_TextOS = res.Where(x => x.Ocular == "OS").Select(x => x.Desc_Text).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.Desc_Text).FirstOrDefault() : string.Empty,
                              OcularOS = res.Where(x => x.Ocular == "OS").Select(x => x.Ocular).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.Ocular).FirstOrDefault() : "OS",
                              CreatedUTc = res.Select(x => x.CreatedUTC).FirstOrDefault(),
                              Subcategory = res.Select(x => x.SubCategory).FirstOrDefault() != null ? res.Select(x => x.SubCategory).FirstOrDefault() : 0,
                          }).ToList();





            return fnd;






        }
        private static int MonthDifference(DateTime lValue, DateTime rValue)
        {
            return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
        }
        public Findings Getpastvalues(string id)

        {
            var fnds = new Findings();


            var idd = WYNKContext.Findings.Where(x => x.RandomUniqueID == id).Select(x => x.RegistrationTranID).LastOrDefault();

            var cidd = WYNKContext.Findings.Where(x => x.RandomUniqueID == id).Select(x => x.CmpID).LastOrDefault();

            fnds.CAddress = CMPSContext.Company.Where(x => x.CmpID == cidd).Select(x => x.Address1).FirstOrDefault();

            fnds.Cphone = CMPSContext.Company.Where(x => x.CmpID == cidd).Select(x => x.Phone1).FirstOrDefault();

            fnds.Cweb = CMPSContext.Company.Where(x => x.CmpID == cidd).Select(x => x.Website).FirstOrDefault();

            fnds.Cname = CMPSContext.Company.Where(x => x.CmpID == cidd).Select(x => x.CompanyName).FirstOrDefault();



            if (fnds.Finding == null)
            {

                fnds.Finding = new Finding();

            }
            var onelinemaster = CMPSContext.OneLineMaster.ToList();
            var specialitymaster = WYNKContext.SpecialityMaster.ToList();
            var slitlamp = WYNKContext.SlitLamp.ToList();
            var findingsss = WYNKContext.Findings.ToList();
            var diagniosis = WYNKContext.Diagnosis.ToList();
            var funduss = WYNKContext.Fundus.ToList();
            var slitlampmaster = WYNKContext.SlitLampMaster.ToList();
            var fundusmaster = WYNKContext.FundusMaster.ToList();

            fnds.elidhis = (from EL in WYNKContext.EyelidDisorder.Where(u => u.FindingsID == id)

                            select new elidhis
                            {

                                UpperRE = EL.UpperRE,
                                UpperLE = EL.UpperLE,
                                LowerRE = EL.LowerRE,
                                LowerLE = EL.LowerLE,
                                BothRE = EL.BothRE,
                                BothLE = EL.BothLE,
                                PosLidRE = EL.PosLidRE,
                                PosLidLE = EL.PosLidLE,
                                LashesRE = EL.LashesRE,
                                LashesLE = EL.LashesLE,
                                IntDstRE = EL.IntDstRE,
                                IntDstLE = EL.IntDstLE,
                                LPSRE = EL.LPSRE,
                                LPSLE = EL.LPSLE,
                                POSRE = EL.POSRE,
                                POSLE = EL.POSLE,

                            }).ToList();


            fnds.goniohis = (from GL in WYNKContext.Glaucoma.Where(u => u.FindingsId == id)

                             select new goniohis
                             {

                                 goniood1 = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(GL.GaunioscopyODG1)).Select(x => x.ParentDescription).FirstOrDefault(),
                                 goniood2 = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(GL.GaunioscopyODG2)).Select(x => x.ParentDescription).FirstOrDefault(),
                                 goniood3 = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(GL.GaunioscopyODG3)).Select(x => x.ParentDescription).FirstOrDefault(),
                                 goniood4 = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(GL.GaunioscopyODG4)).Select(x => x.ParentDescription).FirstOrDefault(),
                                 gonioos1 = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(GL.GaunioscopyOSG1)).Select(x => x.ParentDescription).FirstOrDefault(),
                                 gonioos2 = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(GL.GaunioscopyOSG2)).Select(x => x.ParentDescription).FirstOrDefault(),
                                 gonioos3 = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(GL.GaunioscopyOSG3)).Select(x => x.ParentDescription).FirstOrDefault(),
                                 gonioos4 = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(GL.GaunioscopyOSG4)).Select(x => x.ParentDescription).FirstOrDefault(),

                             }).ToList();




            fnds.Refractioninfola = (from REF in WYNKContext.Findings.Where(u => u.RandomUniqueID == id)
                                     join OLM in onelinemaster
                                     on REF.Category equals OLM.OLMID
                                     select new Refractioninfola
                                     {

                                         SubCategory = OLM.ParentDescription,
                                         SubCategoryos = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(REF.Categoryos) && x.IsActive == true).Select(x => x.ParentDescription).FirstOrDefault(),
                                         DistSph = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(REF.DistSphod)).Select(x => x.ParentDescription).FirstOrDefault(),
                                         NearCyl = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(REF.NearCylod)).Select(x => x.ParentDescription).FirstOrDefault(),
                                         PinAxis = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(REF.PinAxisod)).Select(x => x.ParentDescription).FirstOrDefault(),
                                         N_V_DESC = REF.N_V_DESCod,
                                         pgod = REF.PgSphOD,
                                         DistSphs = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(REF.DistSphos)).Select(x => x.ParentDescription).FirstOrDefault(),
                                         NearCyls = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(REF.NearCylos)).Select(x => x.ParentDescription).FirstOrDefault(),
                                         PinAxiss = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(REF.PinAxisos)).Select(x => x.ParentDescription).FirstOrDefault(),
                                         N_V_DESCs = REF.N_V_DESCos,
                                         pgos = REF.PgSphOS,
                                     }).ToList();

            fnds.PGPH = (from REF in WYNKContext.Findings.Where(u => u.RandomUniqueID == id)

                         select new PGPH
                         {

                             Description = REF.PgDtls,
                             sph = REF.PgSphOD,
                             cyl = REF.PgCylOD,
                             pin = REF.PgAxisOD,
                             add = REF.PgAddOD,
                             sphs = REF.PgSphOS,
                             cyls = REF.PgCylOS,
                             pins = REF.PgAxisOS,
                             adds = REF.PgAddOS,
                         }).ToList();


            fnds.REFH = (from REF in WYNKContext.Findings.Where(u => u.RandomUniqueID == id)

                         select new REFH
                         {



                             Category = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(REF.Instrument)).Select(x => x.ParentDescription).FirstOrDefault(),
                             sph = REF.RDySphOD,
                             cyl = REF.RDyCylOD,
                             axis = REF.RDyAxisOD,
                             sphwt = REF.RWtSphOD,
                             cylwt = REF.RWtCylOD,
                             axiswt = REF.RWtAxisOD,
                             sphs = REF.RDySphOS,
                             cyls = REF.RDyCylOS,
                             axiss = REF.RDyAxisOS,
                             sphswt = REF.RWtSphOS,
                             cylswt = REF.RWtCylOS,
                             axisswt = REF.RWtAxisOS,
                         }).ToList();



            fnds.SlitDesch = (from REF in findingsss.Where(x => x.RandomUniqueID == id)
                              join SLT in slitlamp
                              on REF.RandomUniqueID equals SLT.FindingID
                              join OLM in onelinemaster
                              on SLT.SlitLampLineItemID equals OLM.OLMID



                              select new SlitDesch
                              {
                                  Descc = SLT.Description,
                                  slitid = SLT.SlitLampID,
                              }).ToList();

            fnds.FunDesch = (from FND in findingsss.Where(x => x.RandomUniqueID == id)
                             join FUN in funduss
                             on FND.RandomUniqueID equals FUN.FindingID
                             join OLM in onelinemaster
                             on FUN.FundusLineItemID equals OLM.OLMID

                             select new FunDesch
                             {
                                 Descf = FUN.Description,
                                 fnid = FUN.FundusID,


                             }).ToList();



            fnds.NCT = (from REF in WYNKContext.Findings.Where(u => u.RandomUniqueID == id)

                        select new NCT
                        {

                            Iolnctbdod = REF.IOLNCTbdOD,
                            Iolnctbdos = REF.IOLNCTbdOS,
                            Iolnctadod = REF.IOLNCTOD,
                            Iolnctados = REF.IOLNCTOS,
                        }).ToList();

            fnds.AT = (from REF in WYNKContext.Findings.Where(u => u.RandomUniqueID == id)

                       select new AT
                       {

                           Iolatbdod = REF.IOLATbdOD,
                           Iolatbdos = REF.IOLATbdOS,
                           Iolatadod = REF.IOLATOD,
                           Iolatados = REF.IOLATOS,
                       }).ToList();



            fnds.IOPhi = (from REF in WYNKContext.Refraction.Where(u => u.RegistrationTranID == idd && u.Description == "Intra Ocular Pressure" && u.Ocular == "OD")

                          select new IOPhi
                          {

                              Iolnct = REF.Iolnct,

                          }).ToList();

            fnds.IOPOshi = (from REF in WYNKContext.Refraction.Where(u => u.RegistrationTranID == idd && u.Description == "Intra Ocular Pressure" && u.Ocular == "OS")

                            select new IOPOshi
                            {

                                IolnctOs = REF.Iolnct,

                            }).ToList();



            fnds.IOPOdBdh = (from REF in WYNKContext.Refraction.Where(u => u.RegistrationTranID == idd && u.Description == "Intra Ocular Pressure" && u.Ocular == "OD")

                             select new IOPOdBdh
                             {

                                 IolnctOdBd = REF.Iolnctbd,

                             }).ToList();

            fnds.IOPOsBdh = (from REF in WYNKContext.Refraction.Where(u => u.RegistrationTranID == idd && u.Description == "Intra Ocular Pressure" && u.Ocular == "OS")

                             select new IOPOsBdh
                             {

                                 IolnctOsBd = REF.Iolnctbd,

                             }).ToList();



            fnds.Slitlatesthi = (from REF in findingsss.Where(x => x.RandomUniqueID == id)
                                 join SLT in slitlamp
                                 on REF.RandomUniqueID equals SLT.FindingID


                                 select new Slitlatesthi
                                 {
                                     slitlampname = slitlampmaster.Where(x => x.ID == SLT.SlitLampID).Select(x => x.ParentDescription).FirstOrDefault(),
                                     slitlamplinename = slitlampmaster.Where(x => x.ID == SLT.SlitLampLineItemID).Select(x => x.ParentDescription).FirstOrDefault(),
                                     propertyname = slitlampmaster.Where(x => x.ID == SLT.SlitProperty).Select(x => x.ParentDescription).FirstOrDefault(),
                                     description = SLT.Description,
                                     OdLat = SLT.IsOD == true ? true : false,
                                     OsLat = SLT.IsOS == true ? true : false,
                                     OuLat = SLT.IsOU == true ? true : false,

                                 }).ToList();

            fnds.Slitlatesthin = (from REF in findingsss.Where(x => x.RandomUniqueID == id)
                                  join SLT in slitlamp
                                  on REF.RandomUniqueID equals SLT.FindingID


                                  select new Slitlatesthin
                                  {
                                      slitname = slitlampmaster.Where(x => x.ID == SLT.SlitLampID).Select(x => x.ParentDescription).FirstOrDefault(),
                                      redesc = SLT.BasicDescriptionRE,
                                      ledesc = SLT.BasicDescriptionLE,
                                      remarks = SLT.Description,


                                  }).ToList();







            fnds.Funlatesthi = (from REF in findingsss.Where(x => x.RandomUniqueID == id)
                                join SLT in funduss
                                on REF.RandomUniqueID equals SLT.FindingID


                                select new Funlatesthi
                                {
                                    fundusname = fundusmaster.Where(x => x.ID == SLT.FundusID).Select(x => x.ParentDescription).FirstOrDefault(),
                                    funduslinename = fundusmaster.Where(x => x.ID == SLT.FundusLineItemID).Select(x => x.ParentDescription).FirstOrDefault(),
                                    propertyname = fundusmaster.Where(x => x.ID == SLT.FundusProperty).Select(x => x.ParentDescription).FirstOrDefault(),
                                    description = SLT.Description,
                                    OdLat = SLT.IsOD == true ? true : false,
                                    OsLat = SLT.IsOS == true ? true : false,
                                    OuLat = SLT.IsOU == true ? true : false,

                                }).ToList();

            fnds.Funlatesthin = (from REF in findingsss.Where(x => x.RandomUniqueID == id)
                                 join SLT in funduss
                                 on REF.RandomUniqueID equals SLT.FindingID


                                 select new Funlatesthin
                                 {
                                     fundus = fundusmaster.Where(x => x.ID == SLT.FundusID).Select(x => x.ParentDescription).FirstOrDefault(),
                                     redesc = SLT.BasicDescriptionRE,
                                     ledesc = SLT.BasicDescriptionLE,
                                     remarks = SLT.Description,


                                 }).ToList();


            fnds.Diaglatehi = (from FND in findingsss.Where(u => u.RandomUniqueID == id)
                               join DIG in diagniosis
                               on FND.RandomUniqueID equals DIG.FindingsID
                               join OLM in specialitymaster.Where(u => u.IsActive == true && u.IsDeleted == false && u.CmpID == cidd)
                               on DIG.DiagnosisId equals OLM.ID



                               select new Diaglatehi
                               {
                                   IcdCode2 = DIG.DiagnosisId,
                                   IcdCodess2 = OLM.Code,
                                   IcdDesc2 = OLM.ParentDescription,
                                   Desc = DIG.Description,
                                   Odd2 = DIG.IsOD == true ? true : false,
                                   Oss2 = DIG.IsOS == true ? true : false,
                                   Ouu2 = DIG.IsOU == true ? true : false,

                               }).ToList();

            var fnext = WYNKContext.FindingsExt.ToList();
            var icspec = WYNKContext.ICDSpecialityCode.ToList();
            var icd = WYNKContext.ICDMaster.ToList();
            var use = CMPSContext.Users.ToList();
            var docmas = CMPSContext.DoctorMaster.ToList();


            fnds.Surlatesthi = (from FND in findingsss.Where(x => x.RandomUniqueID == id)
                                join FE in fnext
                                on FND.RandomUniqueID equals FE.FindingsID
                                join ISP in icspec
                                on FE.ICDSpecialityid equals ISP.ID
                                join US in use
                                on FND.CreatedBy equals US.Userid
                                join DOC in docmas
                                on US.Emailid equals DOC.EmailID

                                select new Surlatesthi
                                {
                                    ICDSpec = ISP.SpecialityDescription,
                                    IcdDesc = icd.Where(x => x.ICDCODE == FE.ICDCode).Select(x => x.ICDDESCRIPTION).FirstOrDefault(),
                                    OdSur = FE.IsOD == true ? true : false,
                                    OsSur = FE.IsOS == true ? true : false,
                                    OuSur = FE.IsOU == true ? true : false,
                                    dname = DOC.LastName,
                                    sdate = FND.CreatedUTC.Date,

                                }).ToList();

            return fnds;
        }




        public Findings GetPatientDetails(string UIN, int CompanyID)

        {
            var findings = new Findings();
            findings.INV = new List<Investigation>();
            findings.UploadInvestigationPrescription = new List<UploadInvestigationPrescription>();
            findings.Investigation = new InvestigationImages();
            findings.InvestigationExtn = new InvestigationExtn();
            findings.TonometryTran = new List<TonometryTran>();
            findings.InvestigationTran = new InvestigationTran();
            findings.Glaucoma = new Glaucoma();
            findings.SlitlampExtn = new SlitlampExtn();
            findings.Slitinfo = new List<Slitinfo>();
            findings.DIA = new List<Diagnosis>();
            findings.SLT = new List<SlitLamp>();
            findings.Finding = new Finding();
            findings.Registration = new Registration_Master();
            findings.Slitlatest = new List<Slitlatest>();
            findings.OneLineMaster = new OneLine_Masters();
            findings.SpecialityMaster = new SpecialityMaster();
            findings.Patientinfo = new List<Patientinfo>();
            findings.Fundus = new List<Fundus>();
            findings.MPT = new List<MedicalPrescriptionTran>();
            findings.MedicalPrescription = new MedicalPrescription();
            findings.MedicalPrescriptionTran = new MedicalPrescriptionTran();
            findings.DrugInfo = new List<DrugInfo>();
            findings.UploadMedicalPrescription = new List<UploadMedicalPrescription>();
            findings.InvImg = new List<InvImg>();
            findings.FindingsExt = new List<FindingsExt>();
            findings.Surlatest = new List<Surlatest>();
            findings.SquintMaster = new SquintMaster();
            findings.SquintTran = new List<SquintTran>();
            findings.SQI = new List<SquintImage>();
            findings.SlitLampImage = new List<SlitLampImage>();
            findings.EyelidImage = new List<EyelidImage>();
            findings.FundusImage = new List<FundusImage>();
            findings.InvImgs = new List<InvImgs>();
            findings.InvImgos = new List<InvImgos>();
            findings.InvImgsslod = new List<InvImgsslod>();
            findings.InvImgsslode = new List<InvImgsslode>();
            findings.InvImgsslos = new List<InvImgsslos>();
            findings.InvImgsslose = new List<InvImgsslose>();
            findings.InvImgsfnod = new List<InvImgsfnod>();
            findings.InvImgsfnos = new List<InvImgsfnos>();
            findings.InvImgsglod = new List<InvImgsglod>();
            findings.InvImgsglos = new List<InvImgsglos>();
            findings.InvImgsvfod = new List<InvImgsvfod>();
            findings.InvImgsvfos = new List<InvImgsvfos>();
            findings.GlaucomaImage = new List<GlaucomaImage>();
            findings.VisualField = new List<VisualField>();
            findings.GlaucomaEvaluation = new List<GlaucomaEvaluation>();
            findings.InvestigationPrescription = new InvestigationPrescription();
            findings.EyelidDisorder = new EyelidDisorder();
            findings.OrbitalDisorders = new OrbitalDisorders();
            findings.SquintExt = new SquintExt();
            findings.SquintExtnMaster = new Squint_ExtnMaster();
            findings.InvestigationPrescriptionTran = new List<InvestigationPrescriptionTran>();
            findings.SchirmerTest = new List<SchirmerTest>();
            findings.DryEyes = new DryEyes();
            findings.Registration = WYNKContext.Registration.FirstOrDefault(x => x.UIN == UIN);
            findings.Finding = WYNKContext.Findings.LastOrDefault(x => x.UIN == UIN);
            if (findings.Finding == null)
            {

                findings.Finding = new Finding();

            }

            var regs = WYNKContext.Refraction.Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).LastOrDefault();
            var reid = WYNKContext.Refraction.Where(x => x.RegistrationTranID == regs).Select(x => x.ID).LastOrDefault();

            var regsf = WYNKContext.Findings.Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).LastOrDefault();
            var reidf = WYNKContext.Findings.Where(x => x.RegistrationTranID == regsf).Select(x => x.ID).LastOrDefault();

            findings.Fdt = WYNKContext.Findings.Where(x => x.ID == reidf).Select(x => x.CreatedUTC.Date).LastOrDefault();

            var regids = WYNKContext.RegistrationTran.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.RegistrationTranID).FirstOrDefault();
            findings.Docfee = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == regids).Select(x => x.ConsulationFees).FirstOrDefault();


            var REFRACTION = WYNKContext.Refraction.ToList();
            var fnd = WYNKContext.Findings.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();
            var sqm = WYNKContext.SquintMaster.ToList();
            var sqt = WYNKContext.SquintTran.ToList();
            var use = CMPSContext.Users.ToList();
            var docmas = CMPSContext.DoctorMaster.ToList();

            var vid = WYNKContext.VISUALACUITY.Where(x => x.RegistrationTranID == regs && x.RefTag == "O").Select(x => x.CreatedBy).LastOrDefault();
            var uid = CMPSContext.Users.Where(x => x.Userid == vid).Select(x => x.Emailid).FirstOrDefault();
            findings.Dname = CMPSContext.DoctorMaster.Where(x => x.EmailID == uid).Select(x => x.LastName).FirstOrDefault();


            var RID = WYNKContext.SquintTran.Where(w => w.RID == WYNKContext.RegistrationTran.Where(q => q.UIN == WYNKContext.Registration.Where(u => u.UIN == UIN).Select(x => x.UIN).FirstOrDefault()).OrderByDescending(s => s.CreatedUTC).Select(s => s.RegistrationTranID).FirstOrDefault()).OrderByDescending(t => t.CreatedUTC).Select(a => a.RID).FirstOrDefault();

            findings.SquintTraninfo = (from sq in WYNKContext.SquintTran.Where(x => x.RID == RID && x.IsActive == true)

                                       select new SquintTraninfo
                                       {

                                           IsDVOD = sq.IsDVOD,
                                           IsDVOS = sq.IsDVOS,
                                           IsDVOU = sq.IsDVOU,
                                           OD = sq.IsDVOD,
                                           OS = sq.IsDVOS,
                                           OU = sq.IsDVOU,
                                           Date = sq.Date,
                                           SquintDiagnosisDescription = sq.SquintDiagnosisDescription,
                                           SquintType = sq.SquintType,
                                           ID = sq.ID,
                                           IsActive = sq.IsActive
                                       }).ToList();

            var docmass = CMPSContext.DoctorMaster.ToList();
            var SCRID = WYNKContext.SchirmerTest.Where(w => w.RID == WYNKContext.RegistrationTran.Where(q => q.UIN == WYNKContext.Registration.Where(u => u.UIN == UIN).Select(x => x.UIN).FirstOrDefault()).OrderByDescending(s => s.CreatedUTC).Select(s => s.RegistrationTranID).FirstOrDefault()).OrderByDescending(t => t.CreatedUTC).Select(a => a.RID).FirstOrDefault();

            findings.SchirmerTesthis = (from sc in WYNKContext.SchirmerTest.Where(x => x.RID == SCRID && x.IsActive == true)

                                        select new SchirmerTesthis
                                        {

                                            VisitDatetime = sc.VisitDatetime,
                                            Ocular = sc.Ocular,
                                            Time = sc.Time,
                                            Tearsecretion = sc.Tearsecretion,
                                            Examinedby = sc.Examinedby,
                                            Examinedbyname = docmass.Where(x => x.DoctorID == sc.Examinedby).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                            Remarks = sc.Remarks,
                                            ID = sc.ID,
                                            IsActive = sc.IsActive
                                        }).ToList();



            var gleval = WYNKContext.GlaucomaEvaluation.ToList();
            var GERID = WYNKContext.GlaucomaEvaluation.Where(w => w.RID == WYNKContext.RegistrationTran.Where(q => q.UIN == WYNKContext.Registration.Where(u => u.UIN == UIN).Select(x => x.UIN).FirstOrDefault()).OrderByDescending(s => s.CreatedUTC).Select(s => s.RegistrationTranID).FirstOrDefault()).OrderByDescending(t => t.CreatedUTC).Select(a => a.RID).FirstOrDefault();


            findings.GElatest = (from GE in WYNKContext.GlaucomaEvaluation.Where(x => x.RID == GERID && x.IsActive == true)


                                 select new GElatest
                                 {
                                     date = GE.Date,
                                     regat = GE.REGAT,
                                     legat = GE.LEGAT,
                                     time = GE.Time,
                                     reoptic = GE.REoptic,
                                     leoptic = GE.LEoptic,
                                     gdrugs = GE.GlaucomaDrugs,
                                     hvf = GE.HVF,
                                     oct = GE.OCT,
                                     interven = GE.Intervention,
                                     stapro = GE.StableProgression,
                                     ID = GE.ID,
                                     IsActive = GE.IsActive
                                 }).ToList();




            var groups = WYNKContext.SquintImage.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.Eye);

            findings.Imagedatasq = (from IN in groups
                                    select new Imagedatasq
                                    {
                                        idd = IN.Key,
                                        cont = IN.Count(),
                                        Descs = IN.Select(X => X.SquintODImagePath).FirstOrDefault(),
                                        ImDescs = IN.Select(X => X.Eye).FirstOrDefault(),
                                    }).ToList();


            if (findings.Imagedatasq != null)
            {


                var rest = findings.Imagedatasq.Sum(x => x.cont);
                findings.stringArray = new string[rest];

                var list1 = new List<samplelists>();

                foreach (var item1 in findings.Imagedatasq.ToList())

                {

                    var ress = item1.cont;

                    var list2 = new samplelists();
                    list2.Uin = ress;
                    list1.Add(list2);

                    var cv = list1.Sum(x => x.Uin);

                    var cvv = cv - item1.cont;

                    for (var inde = 0; inde < item1.cont; inde++)
                    {

                        var testStr = item1.Descs + '/' + UIN + inde + ".png";
                        var path = testStr;

                        if ((File.Exists(path)))
                        {

                            string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                            string source = imageData;
                            if (findings.stringArray[inde] != null)
                            {
                                findings.stringArray[inde + cvv] = imageData;

                            }
                            else
                            {
                                findings.stringArray[inde] = imageData;

                            }


                            var list4 = new InvImgs();
                            list4.Desccm = item1.ImDescs;
                            list4.imgdt = "data:image/png;base64," + imageData;
                            findings.InvImgs.Add(list4);


                        }
                    }


                }


                findings.InvImgress = (from IN in findings.InvImgs.GroupBy(x => x.Desccm)
                                       select new InvImgress
                                       {
                                           Desccmre = IN.Key,
                                           imgdtre = IN.Select(x => x.imgdt).ToList(),

                                       }).ToList();


            }

            else
            {
            }





            findings.Imagedatasqs = (from IN in groups
                                     select new Imagedatasqs
                                     {
                                         idd = IN.Key,
                                         cont = IN.Count(),
                                         Descs = IN.Select(X => X.SquintOSImagePath).FirstOrDefault(),
                                         ImDescs = IN.Select(X => X.Eye).FirstOrDefault(),
                                     }).ToList();


            if (findings.Imagedatasqs != null)
            {


                var rest = findings.Imagedatasqs.Sum(x => x.cont);
                findings.stringArrays = new string[rest];

                var list1 = new List<samplelistos>();

                foreach (var item1 in findings.Imagedatasqs.ToList())

                {

                    var ress = item1.cont;

                    var list2 = new samplelistos();
                    list2.Uin = ress;
                    list1.Add(list2);

                    var cv = list1.Sum(x => x.Uin);

                    var cvv = cv - item1.cont;

                    for (var inde = 0; inde < item1.cont; inde++)
                    {

                        var testStr = item1.Descs + '/' + UIN + inde + ".png";
                        var path = testStr;

                        if ((File.Exists(path)))
                        {

                            string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                            string source = imageData;
                            if (findings.stringArrays[inde] != null)
                            {
                                findings.stringArrays[inde + cvv] = imageData;

                            }
                            else
                            {
                                findings.stringArrays[inde] = imageData;

                            }


                            var list4 = new InvImgos();
                            list4.Desccm = item1.ImDescs;
                            list4.imgdt = "data:image/png;base64," + imageData;
                            findings.InvImgos.Add(list4);


                        }
                    }


                }


                findings.InvImgressos = (from IN in findings.InvImgos.GroupBy(x => x.Desccm)
                                         select new InvImgressos
                                         {
                                             Desccmre = IN.Key,
                                             imgdtre = IN.Select(x => x.imgdt).ToList(),

                                         }).ToList();


            }

            else
            {
            }










            var groupsslod = WYNKContext.SlitLampImage.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.SlitLampODImagePath);

            findings.Imagedataslod = (from IN in groupsslod
                                      select new Imagedataslod
                                      {
                                          idd = IN.Key,
                                          cont = IN.Count(),
                                          Descs = IN.Select(X => X.SlitLampODImagePath).FirstOrDefault(),
                                          ImDescs = IN.Select(X => X.Eye).FirstOrDefault(),
                                          dttms = IN.Select(X => X.CreatedUTC).FirstOrDefault(),
                                      }).ToList();


            if (findings.Imagedataslod != null || findings.Imagedataslod.Count != 0)
            {


                var rest = findings.Imagedataslod.Sum(x => x.cont);
                findings.stringArrayslod = new string[rest];

                var list1 = new List<samplelistsslod>();

                foreach (var item1 in findings.Imagedataslod.ToList())

                {

                    var ress = item1.cont;

                    var list2 = new samplelistsslod();
                    list2.Uin = ress;
                    list1.Add(list2);

                    var cv = list1.Sum(x => x.Uin);

                    var cvv = cv - item1.cont;

                    for (var inde = 0; inde < item1.cont; inde++)
                    {

                        var sldid = WYNKContext.SlitLampImage.Where(x => x.UIN == UIN).Select(x => x.ID).ToList();

                        for (var indes = 0; indes < sldid.Count; indes++)
                        {
                            var aa = sldid[indes];


                            var testStr = item1.Descs + '/' + UIN + inde + aa + ".png";
                            var path = testStr;

                            if ((File.Exists(path)))
                            {

                                string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                                string source = imageData;
                                if (findings.stringArrayslod[inde] != null)
                                {
                                    findings.stringArrayslod[inde + cvv] = imageData;

                                }
                                else
                                {
                                    findings.stringArrayslod[inde] = imageData;

                                }


                                var list4 = new InvImgsslod();
                                list4.Desccm = item1.ImDescs;
                                list4.dttm = item1.dttms;
                                list4.imgdt = "data:image/png;base64," + imageData;
                                findings.InvImgsslod.Add(list4);
                            }




                        }

                    }
                }


                findings.InvImgressslod = (from IN in findings.InvImgsslod.GroupBy(x => x.Desccm)
                                           select new InvImgressslod
                                           {
                                               Desccmre = IN.Key,
                                               imgdtre = IN.Select(x => x.imgdt).ToList(),
                                               imgdttm = IN.Select(x => x.dttm).ToList(),
                                           }).ToList();


            }

            else
            {
            }



            var groupsslos = WYNKContext.SlitLampImage.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.SlitLampOSImagePath);



            findings.Imagedataslos = (from IN in groupsslos
                                      select new Imagedataslos
                                      {
                                          idd = IN.Key,
                                          cont = IN.Count(),
                                          Descs = IN.Select(X => X.SlitLampOSImagePath).FirstOrDefault(),
                                          ImDescs = IN.Select(X => X.Eye).FirstOrDefault(),
                                          dttms = IN.Select(X => X.CreatedUTC).FirstOrDefault(),

                                      }).ToList();


            if (findings.Imagedataslos != null || findings.Imagedataslos.Count != 0)
            {


                var rest = findings.Imagedataslos.Sum(x => x.cont);
                findings.stringArrayslos = new string[rest];

                var list1 = new List<samplelistsslos>();

                foreach (var item1 in findings.Imagedataslos.ToList())

                {

                    var ress = item1.cont;

                    var list2 = new samplelistsslos();
                    list2.Uin = ress;
                    list1.Add(list2);

                    var cv = list1.Sum(x => x.Uin);

                    var cvv = cv - item1.cont;

                    for (var inde = 0; inde < item1.cont; inde++)
                    {

                        var slsid = WYNKContext.SlitLampImage.Where(x => x.UIN == UIN).Select(x => x.ID).ToList();

                        for (var indes = 0; indes < slsid.Count; indes++)
                        {
                            var slos = slsid[indes];

                            var testStr = item1.Descs + '/' + UIN + inde + slos + ".png";
                            var path = testStr;

                            if ((File.Exists(path)))
                            {

                                string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                                string source = imageData;
                                if (findings.stringArrayslos[inde] != null)
                                {
                                    findings.stringArrayslos[inde + cvv] = imageData;

                                }
                                else
                                {
                                    findings.stringArrayslos[inde] = imageData;

                                }


                                var list4 = new InvImgsslos();
                                list4.Desccm = item1.ImDescs;
                                list4.dttm = item1.dttms;
                                list4.imgdt = "data:image/png;base64," + imageData;
                                findings.InvImgsslos.Add(list4);
                            }




                        }
                    }
                }

                findings.InvImgressslos = (from IN in findings.InvImgsslos.GroupBy(x => x.Desccm)
                                           select new InvImgressslos
                                           {
                                               Desccmre = IN.Key,
                                               imgdtre = IN.Select(x => x.imgdt).ToList(),
                                               imgdttm = IN.Select(x => x.dttm).ToList(),
                                           }).ToList();


            }

            else
            {
            }









            var groupsslode = WYNKContext.EyelidImage.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.ODImagePath);

            findings.Imagedataslode = (from IN in groupsslode
                                       select new Imagedataslode
                                       {
                                           idd = IN.Key,
                                           cont = IN.Count(),
                                           Descs = IN.Select(X => X.ODImagePath).FirstOrDefault(),
                                           ImDescs = IN.Select(X => X.Eye).FirstOrDefault(),
                                           dttms = IN.Select(X => X.CreatedUTC).FirstOrDefault(),
                                       }).ToList();


            if (findings.Imagedataslode != null || findings.Imagedataslode.Count != 0)
            {


                var rest = findings.Imagedataslode.Sum(x => x.cont);
                findings.stringArrayslode = new string[rest];

                var list1 = new List<samplelistsslode>();

                foreach (var item1 in findings.Imagedataslode.ToList())

                {

                    var ress = item1.cont;

                    var list2 = new samplelistsslode();
                    list2.Uin = ress;
                    list1.Add(list2);

                    var cv = list1.Sum(x => x.Uin);

                    var cvv = cv - item1.cont;

                    for (var inde = 0; inde < item1.cont; inde++)
                    {

                        var sldid = WYNKContext.EyelidImage.Where(x => x.UIN == UIN).Select(x => x.ID).ToList();

                        for (var indes = 0; indes < sldid.Count; indes++)
                        {
                            var aa = sldid[indes];


                            var testStr = item1.Descs + '/' + UIN + inde + aa + ".png";
                            var path = testStr;

                            if ((File.Exists(path)))
                            {

                                string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                                string source = imageData;
                                if (findings.stringArrayslode[inde] != null)
                                {
                                    findings.stringArrayslode[inde + cvv] = imageData;

                                }
                                else
                                {
                                    findings.stringArrayslode[inde] = imageData;

                                }


                                var list4 = new InvImgsslode();
                                list4.Desccm = item1.ImDescs;
                                list4.dttm = item1.dttms;
                                list4.imgdt = "data:image/png;base64," + imageData;
                                findings.InvImgsslode.Add(list4);
                            }




                        }

                    }
                }


                findings.InvImgressslode = (from IN in findings.InvImgsslode.GroupBy(x => x.Desccm)
                                            select new InvImgressslode
                                            {
                                                Desccmre = IN.Key,
                                                imgdtre = IN.Select(x => x.imgdt).ToList(),
                                                imgdttm = IN.Select(x => x.dttm).ToList(),
                                            }).ToList();


            }

            else
            {
            }




            var groupsslose = WYNKContext.EyelidImage.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.OSImagePath);



            findings.Imagedataslose = (from IN in groupsslose
                                       select new Imagedataslose
                                       {
                                           idd = IN.Key,
                                           cont = IN.Count(),
                                           Descs = IN.Select(X => X.OSImagePath).FirstOrDefault(),
                                           ImDescs = IN.Select(X => X.Eye).FirstOrDefault(),
                                           dttms = IN.Select(X => X.CreatedUTC).FirstOrDefault(),

                                       }).ToList();


            if (findings.Imagedataslose != null || findings.Imagedataslose.Count != 0)
            {


                var rest = findings.Imagedataslose.Sum(x => x.cont);
                findings.stringArrayslose = new string[rest];

                var list1 = new List<samplelistsslose>();

                foreach (var item1 in findings.Imagedataslose.ToList())

                {

                    var ress = item1.cont;

                    var list2 = new samplelistsslose();
                    list2.Uin = ress;
                    list1.Add(list2);

                    var cv = list1.Sum(x => x.Uin);

                    var cvv = cv - item1.cont;

                    for (var inde = 0; inde < item1.cont; inde++)
                    {

                        var slsid = WYNKContext.EyelidImage.Where(x => x.UIN == UIN).Select(x => x.ID).ToList();

                        for (var indes = 0; indes < slsid.Count; indes++)
                        {
                            var slos = slsid[indes];

                            var testStr = item1.Descs + '/' + UIN + inde + slos + ".png";
                            var path = testStr;

                            if ((File.Exists(path)))
                            {

                                string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                                string source = imageData;
                                if (findings.stringArrayslose[inde] != null)
                                {
                                    findings.stringArrayslose[inde + cvv] = imageData;

                                }
                                else
                                {
                                    findings.stringArrayslose[inde] = imageData;

                                }


                                var list4 = new InvImgsslose();
                                list4.Desccm = item1.ImDescs;
                                list4.dttm = item1.dttms;
                                list4.imgdt = "data:image/png;base64," + imageData;
                                findings.InvImgsslose.Add(list4);
                            }




                        }
                    }
                }

                findings.InvImgressslose = (from IN in findings.InvImgsslose.GroupBy(x => x.Desccm)
                                            select new InvImgressslose
                                            {
                                                Desccmre = IN.Key,
                                                imgdtre = IN.Select(x => x.imgdt).ToList(),
                                                imgdttm = IN.Select(x => x.dttm).ToList(),
                                            }).ToList();


            }

            else
            {
            }
















            var groupsfnod = WYNKContext.FundusImage.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.FundusODImagePath);

            findings.Imagedatafnod = (from IN in groupsfnod
                                      select new Imagedatafnod
                                      {
                                          idd = IN.Key,
                                          cont = IN.Count(),
                                          Descs = IN.Select(X => X.FundusODImagePath).FirstOrDefault(),
                                          ImDescs = IN.Select(X => X.Eye).FirstOrDefault(),
                                          dttms = IN.Select(X => X.CreatedUTC).FirstOrDefault(),
                                      }).ToList();


            if (findings.Imagedatafnod != null || findings.Imagedatafnod.Count != 0)
            {


                var rest = findings.Imagedatafnod.Sum(x => x.cont);
                findings.stringArrayfnod = new string[rest];

                var list1 = new List<samplelistsfnod>();

                foreach (var item1 in findings.Imagedatafnod.ToList())

                {

                    var ress = item1.cont;

                    var list2 = new samplelistsfnod();
                    list2.Uin = ress;
                    list1.Add(list2);

                    var cv = list1.Sum(x => x.Uin);

                    var cvv = cv - item1.cont;

                    for (var inde = 0; inde < item1.cont; inde++)
                    {
                        var fndid = WYNKContext.FundusImage.Where(x => x.UIN == UIN).Select(x => x.ID).ToList();

                        for (var indes = 0; indes < fndid.Count; indes++)
                        {
                            var fnod = fndid[indes];


                            var testStr = item1.Descs + '/' + UIN + inde + fnod + ".png";
                            var path = testStr;

                            if ((File.Exists(path)))
                            {

                                string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                                string source = imageData;
                                if (findings.stringArrayfnod[inde] != null)
                                {
                                    findings.stringArrayfnod[inde + cvv] = imageData;

                                }
                                else
                                {
                                    findings.stringArrayfnod[inde] = imageData;

                                }


                                var list4 = new InvImgsfnod();
                                list4.Desccm = item1.ImDescs;
                                list4.dttm = item1.dttms;
                                list4.imgdt = "data:image/png;base64," + imageData;
                                findings.InvImgsfnod.Add(list4);
                            }




                        }
                    }
                }

                findings.InvImgressfnod = (from IN in findings.InvImgsfnod.GroupBy(x => x.Desccm)
                                           select new InvImgressfnod
                                           {
                                               Desccmre = IN.Key,
                                               imgdtre = IN.Select(x => x.imgdt).ToList(),
                                               imgdttm = IN.Select(x => x.dttm).ToList(),

                                           }).ToList();


            }

            else
            {
            }


            var groupsfnos = WYNKContext.FundusImage.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.FundusOSImagePath);


            findings.Imagedatafnos = (from IN in groupsfnos
                                      select new Imagedatafnos
                                      {
                                          idd = IN.Key,
                                          cont = IN.Count(),
                                          Descs = IN.Select(X => X.FundusOSImagePath).FirstOrDefault(),
                                          ImDescs = IN.Select(X => X.Eye).FirstOrDefault(),
                                          dttms = IN.Select(X => X.CreatedUTC).FirstOrDefault(),
                                      }).ToList();


            if (findings.Imagedatafnos != null || findings.Imagedatafnos.Count != 0)
            {


                var rest = findings.Imagedatafnos.Sum(x => x.cont);
                findings.stringArrayfnos = new string[rest];

                var list1 = new List<samplelistsfnos>();

                foreach (var item1 in findings.Imagedatafnos.ToList())

                {

                    var ress = item1.cont;

                    var list2 = new samplelistsfnos();
                    list2.Uin = ress;
                    list1.Add(list2);

                    var cv = list1.Sum(x => x.Uin);

                    var cvv = cv - item1.cont;

                    for (var inde = 0; inde < item1.cont; inde++)
                    {
                        var fnsid = WYNKContext.FundusImage.Where(x => x.UIN == UIN).Select(x => x.ID).ToList();

                        for (var indes = 0; indes < fnsid.Count; indes++)
                        {
                            var fnos = fnsid[indes];

                            var testStr = item1.Descs + '/' + UIN + inde + fnos + ".png";
                            var path = testStr;

                            if ((File.Exists(path)))
                            {

                                string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                                string source = imageData;
                                if (findings.stringArrayfnos[inde] != null)
                                {
                                    findings.stringArrayfnos[inde + cvv] = imageData;

                                }
                                else
                                {
                                    findings.stringArrayfnos[inde] = imageData;

                                }


                                var list4 = new InvImgsfnos();
                                list4.Desccm = item1.ImDescs;
                                list4.dttm = item1.dttms;
                                list4.imgdt = "data:image/png;base64," + imageData;
                                findings.InvImgsfnos.Add(list4);
                            }




                        }
                    }
                }

                findings.InvImgressfnos = (from IN in findings.InvImgsfnos.GroupBy(x => x.Desccm)
                                           select new InvImgressfnos
                                           {
                                               Desccmre = IN.Key,
                                               imgdtre = IN.Select(x => x.imgdt).ToList(),
                                               imgdttm = IN.Select(x => x.dttm).ToList(),
                                           }).ToList();


            }

            else
            {
            }





            var groupsglod = WYNKContext.GlaucomaImage.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.Eye);

            findings.Imagedataglod = (from IN in groupsglod
                                      select new Imagedataglod
                                      {
                                          idd = IN.Key,
                                          cont = IN.Count(),
                                          Descs = IN.Select(X => X.GlaucomaODImagePath).FirstOrDefault(),
                                          ImDescs = IN.Select(X => X.Eye).FirstOrDefault(),
                                      }).ToList();


            if (findings.Imagedataglod != null || findings.Imagedataglod.Count != 0)
            {


                var rest = findings.Imagedataglod.Sum(x => x.cont);
                findings.stringArrayglod = new string[rest];

                var list1 = new List<samplelistsglod>();

                foreach (var item1 in findings.Imagedataglod.ToList())

                {

                    var ress = item1.cont;

                    var list2 = new samplelistsglod();
                    list2.Uin = ress;
                    list1.Add(list2);

                    var cv = list1.Sum(x => x.Uin);

                    var cvv = cv - item1.cont;

                    for (var inde = 0; inde < item1.cont; inde++)
                    {

                        var testStr = item1.Descs + '/' + UIN + inde + ".png";
                        var path = testStr;

                        if ((File.Exists(path)))
                        {

                            string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                            string source = imageData;
                            if (findings.stringArrayglod[inde] != null)
                            {
                                findings.stringArrayglod[inde + cvv] = imageData;

                            }
                            else
                            {
                                findings.stringArrayglod[inde] = imageData;

                            }


                            var list4 = new InvImgsglod();
                            list4.Desccm = item1.ImDescs;
                            list4.imgdt = "data:image/png;base64," + imageData;
                            findings.InvImgsglod.Add(list4);
                        }




                    }
                }


                findings.InvImgressglod = (from IN in findings.InvImgsglod.GroupBy(x => x.Desccm)
                                           select new InvImgressglod
                                           {
                                               Desccmre = IN.Key,
                                               imgdtre = IN.Select(x => x.imgdt).ToList(),

                                           }).ToList();


            }

            else
            {
            }




            findings.Imagedataglos = (from IN in groupsglod
                                      select new Imagedataglos
                                      {
                                          idd = IN.Key,
                                          cont = IN.Count(),
                                          Descs = IN.Select(X => X.GlaucomaOSImagePath).FirstOrDefault(),
                                          ImDescs = IN.Select(X => X.Eye).FirstOrDefault(),
                                      }).ToList();


            if (findings.Imagedataglos != null || findings.Imagedataglos.Count != 0)
            {


                var rest = findings.Imagedataglos.Sum(x => x.cont);
                findings.stringArrayglos = new string[rest];

                var list1 = new List<samplelistsglos>();

                foreach (var item1 in findings.Imagedataglos.ToList())

                {

                    var ress = item1.cont;

                    var list2 = new samplelistsglos();
                    list2.Uin = ress;
                    list1.Add(list2);

                    var cv = list1.Sum(x => x.Uin);

                    var cvv = cv - item1.cont;

                    for (var inde = 0; inde < item1.cont; inde++)
                    {

                        var testStr = item1.Descs + '/' + UIN + inde + ".png";
                        var path = testStr;

                        if ((File.Exists(path)))
                        {

                            string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                            string source = imageData;
                            if (findings.stringArrayglos[inde] != null)
                            {
                                findings.stringArrayglos[inde + cvv] = imageData;

                            }
                            else
                            {
                                findings.stringArrayglos[inde] = imageData;

                            }


                            var list4 = new InvImgsglos();
                            list4.Desccm = item1.ImDescs;
                            list4.imgdt = "data:image/png;base64," + imageData;
                            findings.InvImgsglos.Add(list4);
                        }




                    }
                }


                findings.InvImgressglos = (from IN in findings.InvImgsglos.GroupBy(x => x.Desccm)
                                           select new InvImgressglos
                                           {
                                               Desccmre = IN.Key,
                                               imgdtre = IN.Select(x => x.imgdt).ToList(),

                                           }).ToList();


            }

            else
            {
            }








            var groupsvfod = WYNKContext.VisualField.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.VFODImagePath);

            findings.Imagedatavfod = (from IN in groupsvfod
                                      select new Imagedatavfod
                                      {
                                          idd = IN.Key,
                                          cont = IN.Count(),
                                          Descs = IN.Select(X => X.VFODImagePath).FirstOrDefault(),
                                          ImDescs = IN.Select(X => X.Eye).FirstOrDefault(),
                                          dttms = IN.Select(X => X.CreatedUTC).FirstOrDefault(),
                                      }).ToList();


            if (findings.Imagedatavfod != null || findings.Imagedatavfod.Count != 0)
            {


                var rest = findings.Imagedatavfod.Sum(x => x.cont);
                findings.stringArrayvfod = new string[rest];

                var list1 = new List<samplelistsvfod>();

                foreach (var item1 in findings.Imagedatavfod.ToList())

                {

                    var ress = item1.cont;

                    var list2 = new samplelistsvfod();
                    list2.Uin = ress;
                    list1.Add(list2);

                    var cv = list1.Sum(x => x.Uin);

                    var cvv = cv - item1.cont;

                    for (var inde = 0; inde < item1.cont; inde++)
                    {
                        var vfdid = WYNKContext.VisualField.Where(x => x.UIN == UIN).Select(x => x.ID).ToList();

                        for (var indes = 0; indes < vfdid.Count; indes++)
                        {
                            var vfod = vfdid[indes];


                            var testStr = item1.Descs + '/' + UIN + inde + vfod + ".png";
                            var path = testStr;

                            if ((File.Exists(path)))
                            {

                                string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                                string source = imageData;
                                if (findings.stringArrayvfod[inde] != null)
                                {
                                    findings.stringArrayvfod[inde + cvv] = imageData;

                                }
                                else
                                {
                                    findings.stringArrayvfod[inde] = imageData;

                                }


                                var list4 = new InvImgsvfod();
                                list4.Desccm = item1.ImDescs;
                                list4.dttm = item1.dttms;
                                list4.imgdt = "data:image/png;base64," + imageData;
                                findings.InvImgsvfod.Add(list4);
                            }




                        }
                    }
                }

                findings.InvImgressvfod = (from IN in findings.InvImgsvfod.GroupBy(x => x.Desccm)
                                           select new InvImgressvfod
                                           {
                                               Desccmre = IN.Key,
                                               imgdtre = IN.Select(x => x.imgdt).ToList(),
                                               imgdttm = IN.Select(x => x.dttm).ToList(),

                                           }).ToList();


            }

            else
            {
            }



            var groupsvfos = WYNKContext.VisualField.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.VFOSImagePath);



            findings.Imagedatavfos = (from IN in groupsvfos
                                      select new Imagedatavfos
                                      {
                                          idd = IN.Key,
                                          cont = IN.Count(),
                                          Descs = IN.Select(X => X.VFOSImagePath).FirstOrDefault(),
                                          ImDescs = IN.Select(X => X.Eye).FirstOrDefault(),
                                          dttms = IN.Select(X => X.CreatedUTC).FirstOrDefault(),
                                      }).ToList();


            if (findings.Imagedatavfos != null || findings.Imagedatavfos.Count != 0)
            {


                var rest = findings.Imagedatavfos.Sum(x => x.cont);
                findings.stringArrayvfos = new string[rest];

                var list1 = new List<samplelistsvfos>();

                foreach (var item1 in findings.Imagedatavfos.ToList())

                {

                    var ress = item1.cont;

                    var list2 = new samplelistsvfos();
                    list2.Uin = ress;
                    list1.Add(list2);

                    var cv = list1.Sum(x => x.Uin);

                    var cvv = cv - item1.cont;

                    for (var inde = 0; inde < item1.cont; inde++)
                    {
                        var vfsid = WYNKContext.VisualField.Where(x => x.UIN == UIN).Select(x => x.ID).ToList();

                        for (var indes = 0; indes < vfsid.Count; indes++)
                        {
                            var vfos = vfsid[indes];

                            var testStr = item1.Descs + '/' + UIN + inde + vfos + ".png";
                            var path = testStr;

                            if ((File.Exists(path)))
                            {

                                string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                                string source = imageData;
                                if (findings.stringArrayvfos[inde] != null)
                                {
                                    findings.stringArrayvfos[inde + cvv] = imageData;

                                }
                                else
                                {
                                    findings.stringArrayvfos[inde] = imageData;

                                }


                                var list4 = new InvImgsvfos();
                                list4.Desccm = item1.ImDescs;
                                list4.dttm = item1.dttms;
                                list4.imgdt = "data:image/png;base64," + imageData;
                                findings.InvImgsvfos.Add(list4);
                            }




                        }
                    }
                }

                findings.InvImgressvfos = (from IN in findings.InvImgsvfos.GroupBy(x => x.Desccm)
                                           select new InvImgressvfos
                                           {
                                               Desccmre = IN.Key,
                                               imgdtre = IN.Select(x => x.imgdt).ToList(),
                                               imgdttm = IN.Select(x => x.dttm).ToList(),

                                           }).ToList();


            }

            else
            {
            }


            var reg = WYNKContext.Findings.Where(x => x.UIN == UIN && x.CmpID == CompanyID).OrderByDescending(x => x.CreatedUTC).Select(x => x.RegistrationTranID).FirstOrDefault();

            var reg1 = WYNKContext.Findings.Where(x => x.RegistrationTranID == reg).OrderByDescending(x => x.CreatedUTC).Select(x => x.RandomUniqueID).FirstOrDefault();

            findings.SlitlampExtn = WYNKContext.SlitlampExtn.Where(x => x.FindingsID == reg1).FirstOrDefault();
            findings.Glaucoma = WYNKContext.Glaucoma.Where(x => x.FindingsId == reg1).FirstOrDefault();

            findings.EyelidDisorder = WYNKContext.EyelidDisorder.LastOrDefault(x => x.FindingsID == reg1);

            if (findings.EyelidDisorder == null)
            {

                findings.EyelidDisorder = new EyelidDisorder();

            }
            findings.Dreyhis = (from REF in WYNKContext.DryEyes.Where(u => u.FindingsID == reg1)


                                select new Dreyhis
                                {
                                    NorTMH = REF.NorTMH,
                                    NorTMHtxt = REF.NorTMHtxt,
                                    DryTMH = REF.DryTMH,
                                    DryTMHtxt = REF.DryTMHtxt,
                                    TBUT = REF.TBUT,
                                    TBUTtxt = REF.TBUTtxt,
                                    NiTBUT = REF.NiTBUT,
                                    NiTBUTtxt = REF.NiTBUTtxt,


                                }).ToList();


            findings.Goniosco = (from REF in WYNKContext.Glaucoma.Where(u => u.FindingsId == reg1)


                                 select new Goniosco
                                 {
                                     goniood1 = REF.GaunioscopyODG1,
                                     goniood2 = REF.GaunioscopyODG2,
                                     goniood3 = REF.GaunioscopyODG3,
                                     goniood4 = REF.GaunioscopyODG4,
                                     gonioos1 = REF.GaunioscopyOSG1,
                                     gonioos2 = REF.GaunioscopyOSG2,
                                     gonioos3 = REF.GaunioscopyOSG3,
                                     gonioos4 = REF.GaunioscopyOSG4,

                                     goniood11 = onelinemaster.Where(x => x.OLMID == REF.GaunioscopyODG1).Select(x => x.ParentDescription).FirstOrDefault(),
                                     goniood21 = onelinemaster.Where(x => x.OLMID == REF.GaunioscopyODG2).Select(x => x.ParentDescription).FirstOrDefault(),
                                     goniood31 = onelinemaster.Where(x => x.OLMID == REF.GaunioscopyODG3).Select(x => x.ParentDescription).FirstOrDefault(),
                                     goniood41 = onelinemaster.Where(x => x.OLMID == REF.GaunioscopyODG4).Select(x => x.ParentDescription).FirstOrDefault(),
                                     gonioos11 = onelinemaster.Where(x => x.OLMID == REF.GaunioscopyOSG1).Select(x => x.ParentDescription).FirstOrDefault(),
                                     gonioos21 = onelinemaster.Where(x => x.OLMID == REF.GaunioscopyOSG2).Select(x => x.ParentDescription).FirstOrDefault(),
                                     gonioos31 = onelinemaster.Where(x => x.OLMID == REF.GaunioscopyOSG3).Select(x => x.ParentDescription).FirstOrDefault(),
                                     gonioos41 = onelinemaster.Where(x => x.OLMID == REF.GaunioscopyOSG4).Select(x => x.ParentDescription).FirstOrDefault(),


                                 }).ToList();

            findings.ocurechis = (from REF in WYNKContext.SlitlampExtn.Where(u => u.FindingsID == reg1)


                                  select new ocurechis
                                  {
                                      HorMeasurementod1 = REF.HorMeasurementod1,
                                      VerMeasurementod1 = REF.VerMeasurementod1,
                                      OcularMovementod1 = REF.OcularMovementod1,
                                      HorMeasurementod2 = REF.HorMeasurementod2,
                                      VerMeasurementod2 = REF.VerMeasurementod2,
                                      OcularMovementod2 = REF.OcularMovementod2,
                                      HorMeasurementod3 = REF.HorMeasurementod3,
                                      VerMeasurementod3 = REF.VerMeasurementod3,
                                      OcularMovementod3 = REF.OcularMovementod3,

                                      HorMeasurementod4 = REF.HorMeasurementod4,
                                      VerMeasurementod4 = REF.VerMeasurementod4,
                                      OcularMovementod4 = REF.OcularMovementod4,
                                      HorMeasurementod5 = REF.HorMeasurementod5,
                                      VerMeasurementod5 = REF.VerMeasurementod5,
                                      OcularMovementod5 = REF.OcularMovementod5,
                                      HorMeasurementod6 = REF.HorMeasurementod6,
                                      VerMeasurementod6 = REF.VerMeasurementod6,
                                      OcularMovementod6 = REF.OcularMovementod6,
                                      OcularMovementod7 = REF.OcularMovementod7,
                                      OcularMovementod8 = REF.OcularMovementod8,

                                      HorMeasurementos1 = REF.HorMeasurementos1,
                                      VerMeasurementos1 = REF.VerMeasurementos1,
                                      OcularMovementos1 = REF.OcularMovementos1,
                                      HorMeasurementos2 = REF.HorMeasurementos2,
                                      VerMeasurementos2 = REF.VerMeasurementos2,
                                      OcularMovementos2 = REF.OcularMovementos2,
                                      HorMeasurementos3 = REF.HorMeasurementos3,
                                      VerMeasurementos3 = REF.VerMeasurementos3,
                                      OcularMovementos3 = REF.OcularMovementos3,

                                      HorMeasurementos4 = REF.HorMeasurementos4,
                                      VerMeasurementos4 = REF.VerMeasurementos4,
                                      OcularMovementos4 = REF.OcularMovementos4,
                                      HorMeasurementos5 = REF.HorMeasurementos5,
                                      VerMeasurementos5 = REF.VerMeasurementos5,
                                      OcularMovementos5 = REF.OcularMovementos5,
                                      HorMeasurementos6 = REF.HorMeasurementos6,
                                      VerMeasurementos6 = REF.VerMeasurementos6,
                                      OcularMovementos6 = REF.OcularMovementos6,
                                      OcularMovementos7 = REF.OcularMovementos7,
                                      OcularMovementos8 = REF.OcularMovementos8,


                                  }).ToList();

            findings.squinthis = (from REF in WYNKContext.SquintExt.Where(u => u.FindingsID == reg1)


                                  select new squinthis
                                  {
                                      AngleKappa = REF.AngleKappa,
                                      Patterns = REF.Patterns,
                                      ACAMethod = REF.ACAMethod,
                                      ACAValue = REF.ACAValue,
                                      WFDTDistance = REF.WFDTDistance,
                                      WFDTNear = REF.WFDTNear,
                                      StreopsisMethod = REF.StreopsisMethod,
                                      StreopsisValue = REF.StreopsisValue,
                                      ARC = REF.ARC,

                                      PBCTDisHor = REF.PBCTDisHor,
                                      PBCTDisHorValue = REF.PBCTDisHorValue,
                                      PBCTDisVer = REF.PBCTDisVer,
                                      PBCTDisVerValue = REF.PBCTDisVerValue,
                                      PBCTNearHor = REF.PBCTNearHor,
                                      PBCTNearHorValue = REF.PBCTNearHorValue,
                                      PBCTNearVer = REF.PBCTNearVer,
                                      PBCTNearVerValue = REF.PBCTNearVerValue,
                                      ModKrimHor = REF.ModKrimHor,
                                      ModKrimHorValue = REF.ModKrimHorValue,
                                      ModKrimVer = REF.ModKrimVer,

                                      ModKrimVerValue = REF.ModKrimVerValue,
                                      PriDevHor = REF.PriDevHor,
                                      PriDevHorValue = REF.PriDevHorValue,
                                      PriDevVer = REF.PriDevVer,
                                      PriDevVerValue = REF.PriDevVerValue,
                                      SecDevHor = REF.SecDevHor,
                                      SecDevHorValue = REF.SecDevHorValue,
                                      SecDevVer = REF.SecDevVer,
                                      SecDevVerValue = REF.SecDevVerValue,

                                      Amplitude = REF.Amplitude,
                                      Frequency = REF.Frequency,
                                      Type = REF.Type,
                                      Pursuit = REF.Pursuit,
                                      Saccade = REF.Saccade,
                                      ConjugateDissociated = REF.ConjugateDissociated,
                                      ABHeadPos = REF.ABHeadPos,
                                      FreqOnConvergence = REF.FreqOnConvergence,
                                      Occlusion = REF.Occlusion,
                                      Oscillopsia = REF.Oscillopsia,
                                      AssHeadPosture = REF.AssHeadPosture,
                                      SquintBasicExam = REF.SquintBasicExam,
                                      SpecialTest = REF.SpecialTest,

                                      VF1 = REF.VF1,
                                      VF1Value = REF.VF1Value,
                                      VF2 = REF.VF2,
                                      VF2Value = REF.VF2Value,
                                      VF3 = REF.VF3,
                                      VF3Value = REF.VF3Value,
                                      VF4 = REF.VF4,
                                      VF4Value = REF.VF4Value,


                                  }).ToList();

            findings.orbitalhis = (from REF in WYNKContext.OrbitalDisorders.Where(u => u.FindingsID == reg1)


                                   select new orbitalhis
                                   {

                                       PositionofGlobeRE = REF.PositionofGlobeRE,
                                       PositionofGlobeLE = REF.PositionofGlobeLE,
                                       BaseReadingRE = REF.BaseReadingRE,
                                       BaseReadingLE = REF.BaseReadingLE,
                                       ReadingsfortheeyeRE = REF.ReadingsfortheeyeRE,
                                       ReadingsfortheeyeLE = REF.ReadingsfortheeyeLE,
                                       FingerInsinuationTestRE = REF.FingerInsinuationTestRE,
                                       FingerInsinuationTestLE = REF.FingerInsinuationTestLE,
                                       LocationRE = REF.LocationRE,
                                       LocationLE = REF.LocationLE,
                                       ShapeRE = REF.ShapeRE,
                                       ShapeLE = REF.ShapeLE,
                                       TextureRE = REF.TextureRE,
                                       TextureLE = REF.TextureLE,
                                       SizeRE = REF.SizeRE,
                                       SizeLE = REF.SizeLE,
                                       Remarks = REF.Remarks,
                                       GradeofThyroidEyeDiseaseRE = REF.GradeofThyroidEyeDiseaseRE,
                                       GradeofThyroidEyeDiseaseLE = REF.GradeofThyroidEyeDiseaseLE,
                                       LocationTextRE = REF.LocationTextRE,
                                       LocationTextLE = REF.LocationTextLE,
                                       ShapeTextRE = REF.ShapeTextRE,
                                       ShapeTextLE = REF.ShapeTextLE,
                                       TextureTextRE = REF.TextureTextRE,
                                       TextureTextLE = REF.TextureTextLE,
                                       SizeTextRE = REF.SizeTextRE,
                                       SizeTextLE = REF.SizeTextLE,
                                       SizeTextREVer = REF.SizeTextREVer,
                                       SizeTextLEVer = REF.SizeTextLEVer,
                                       FingerInsinuationRE = REF.FingerInsinuationRE,
                                       FingerInsinuationLE = REF.FingerInsinuationLE,

                                   }).ToList();





            if (findings.SlitlampExtn == null)
            {
                findings.SlitlampExtn = new SlitlampExtn();

            }

            if (findings.Glaucoma == null)
            {
                findings.Glaucoma = new Glaucoma();

            }

            var slitlamp = WYNKContext.SlitLamp.ToList();
            var findingsss = WYNKContext.Findings.ToList();
            var onelinemasterss = CMPSContext.OneLineMaster.ToList();
            var specialitymaster = WYNKContext.SpecialityMaster.ToList();
            var diagniosis = WYNKContext.Diagnosis.ToList();
            var funduss = WYNKContext.Fundus.ToList();
            var fnext = WYNKContext.FindingsExt.ToList();
            var icspec = WYNKContext.ICDSpecialityCode.ToList();
            var icd = WYNKContext.ICDMaster.ToList();

            var slitlampmaster = WYNKContext.SlitLampMaster.ToList();

            var fundusmaster = WYNKContext.FundusMaster.ToList();




            findings.Slitlatest = (from REF in findingsss.Where(x => x.RandomUniqueID == reg1)
                                   join SLT in slitlamp
                                   on REF.RandomUniqueID equals SLT.FindingID
                                   join OLM in onelinemasterss
                                   on SLT.SlitLampLineItemID equals OLM.OLMID



                                   select new Slitlatest
                                   {
                                       Sname = onelinemasterss.Where(x => x.OLMID == SLT.SlitLampID).Select(x => x.ParentDescription).FirstOrDefault(),
                                       DescLat = OLM.ParentDescription,
                                       CodeLat = SLT.SlitLampLineItemID,
                                       OdLat = SLT.IsOD,
                                       OsLat = SLT.IsOS,
                                       OuLat = SLT.IsOU,

                                   }).ToList();



            findings.SlitDesc = (from REF in findingsss.Where(x => x.RandomUniqueID == reg1)
                                 join SLT in slitlamp
                                 on REF.RandomUniqueID equals SLT.FindingID
                                 join OLM in onelinemasterss
                                 on SLT.SlitLampLineItemID equals OLM.OLMID



                                 select new SlitDesc
                                 {
                                     Descc = SLT.Description,
                                     slitid = SLT.SlitLampID,
                                 }).ToList();


            findings.Slitlamphistory = (from REF in findingsss.Where(x => x.RandomUniqueID == reg1)
                                        join SLT in slitlamp.Where(x => x.BasicDescriptionLE == "" && x.BasicDescriptionRE == "")
                                        on REF.RandomUniqueID equals SLT.FindingID


                                        select new Slitlamphistory
                                        {
                                            slitlampname = slitlampmaster.Where(x => x.ID == SLT.SlitLampID).Select(x => x.ParentDescription).FirstOrDefault(),
                                            slitlamplinename = slitlampmaster.Where(x => x.ID == SLT.SlitLampLineItemID).Select(x => x.ParentDescription).FirstOrDefault(),
                                            propertyname = slitlampmaster.Where(x => x.ID == SLT.SlitProperty).Select(x => x.ParentDescription).FirstOrDefault(),
                                            slitlampid = SLT.SlitLampID,
                                            slitlamplineid = SLT.SlitLampLineItemID,
                                            propertyid = SLT.SlitProperty,
                                            description = SLT.Description,
                                            OdLat = SLT.IsOD,
                                            OsLat = SLT.IsOS,
                                            OuLat = SLT.IsOU,

                                        }).ToList();


            findings.Slitlamphistorynew = (from REF in findingsss.Where(x => x.RandomUniqueID == reg1)
                                           join SLT in slitlamp.Where(x => x.BasicDescriptionLE != "" || x.BasicDescriptionRE != "")
                                           on REF.RandomUniqueID equals SLT.FindingID


                                           select new Slitlamphistorynew
                                           {
                                               slitlampname = slitlampmaster.Where(x => x.ID == SLT.SlitLampID).Select(x => x.ParentDescription).FirstOrDefault(),
                                               slitlampid = SLT.SlitLampID,
                                               description = SLT.Description,
                                               redescription = SLT.BasicDescriptionRE,
                                               ledescription = SLT.BasicDescriptionLE,
                                               OdLat = SLT.IsOD,
                                               OsLat = SLT.IsOS,
                                               OuLat = SLT.IsOU,

                                           }).ToList();

            findings.Fundushistory = (from REF in findingsss.Where(x => x.RandomUniqueID == reg1)
                                      join SLT in funduss.Where(x => x.BasicDescriptionLE == "" && x.BasicDescriptionRE == "")
                                      on REF.RandomUniqueID equals SLT.FindingID


                                      select new Fundushistory
                                      {
                                          fundusname = fundusmaster.Where(x => x.ID == SLT.FundusID).Select(x => x.ParentDescription).FirstOrDefault(),
                                          funduslinename = fundusmaster.Where(x => x.ID == SLT.FundusLineItemID).Select(x => x.ParentDescription).FirstOrDefault(),
                                          propertyname = fundusmaster.Where(x => x.ID == SLT.FundusProperty).Select(x => x.ParentDescription).FirstOrDefault(),
                                          fundusid = SLT.FundusID,
                                          funduslineid = SLT.FundusLineItemID,
                                          propertyid = SLT.FundusProperty,
                                          description = SLT.Description,
                                          OdLat = SLT.IsOD,
                                          OsLat = SLT.IsOS,
                                          OuLat = SLT.IsOU,

                                      }).ToList();

            findings.Fundushistorynew = (from REF in findingsss.Where(x => x.RandomUniqueID == reg1)
                                         join SLT in funduss.Where(x => x.BasicDescriptionLE != "" || x.BasicDescriptionRE != "")
                                         on REF.RandomUniqueID equals SLT.FindingID


                                         select new Fundushistorynew
                                         {
                                             fundusname = fundusmaster.Where(x => x.ID == SLT.FundusID).Select(x => x.ParentDescription).FirstOrDefault(),
                                             fundusid = SLT.FundusID,
                                             redescription = SLT.BasicDescriptionRE,
                                             ledescription = SLT.BasicDescriptionLE,
                                             description = SLT.Description,
                                             OdLat = SLT.IsOD,
                                             OsLat = SLT.IsOS,
                                             OuLat = SLT.IsOU,

                                         }).ToList();






            findings.FunDesc = (from FND in findingsss.Where(x => x.RandomUniqueID == reg1)
                                join FUN in funduss
                                on FND.RandomUniqueID equals FUN.FindingID
                                join OLM in onelinemasterss
                                on FUN.FundusLineItemID equals OLM.OLMID

                                select new FunDesc
                                {
                                    Descf = FUN.Description,
                                    fnid = FUN.FundusID,


                                }).ToList();



            findings.Diaglate = (from FND in findingsss.Where(u => u.RandomUniqueID == reg1)
                                 join DIG in diagniosis
                                 on FND.RandomUniqueID equals DIG.FindingsID
                                 join OLM in specialitymaster
                                 on DIG.DiagnosisId equals OLM.ID



                                 select new Diaglate
                                 {
                                     IcdCode2 = DIG.DiagnosisId,
                                     IcdCodess2 = OLM.Code,
                                     IcdDesc2 = OLM.ParentDescription,
                                     Desc2 = DIG.Description,
                                     Odd2 = DIG.IsOD,
                                     Oss2 = DIG.IsOS,
                                     Ouu2 = DIG.IsOU,

                                 }).ToList();


            findings.Surlatest = (from FND in findingsss.Where(x => x.RandomUniqueID == reg1)
                                  join FE in fnext
                                  on FND.RandomUniqueID equals FE.FindingsID
                                  join ISP in icspec
                                  on FE.ICDSpecialityid equals ISP.ID

                                  select new Surlatest
                                  {
                                      ICDSpec = ISP.SpecialityDescription,
                                      IcdDesc = icd.Where(x => x.ICDCODE == FE.ICDCode).Select(x => x.ICDDESCRIPTION).FirstOrDefault(),
                                      ICDcod = FE.ICDCode,
                                      speid = FE.ICDSpecialityid,
                                      OdSur = FE.IsOD,
                                      OsSur = FE.IsOS,
                                      OuSur = FE.IsOU,

                                  }).ToList();





            return findings;
        }




        public IEnumerable<Diag> GetdiaDetails(int CmpID)

        {
            return (from OLM in WYNKContext.SpecialityMaster.Where(u => u.ParentTag == "DIA" && u.IsDeleted == false && u.IsActive == true && u.CmpID == CmpID).OrderByDescending(x => x.ID)
                    select new Diag
                    {
                        Desc1 = OLM.ParentDescription,
                        Descp = OLM.ParentTag,
                        Code1 = OLM.ID,
                        IcdCode1 = OLM.Code,
                    }).ToList();
        }

        public dynamic UpdateTono(Findings Findings, string UIN, int DOCID, string cpname, string dcname)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var find = new Finding();

                    var regtid = WYNKContext.RegistrationTran.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.RegistrationTranID).FirstOrDefault();
                    var reg = Convert.ToInt32(WYNKContext.TonometryTran.Where(x => x.RegistrationtranID == regtid).Select(x => x.RegistrationtranID).LastOrDefault());
                    if (reg == 0)
                    {
                        if (Findings.TonometryTran != null)
                        {
                            if (Findings.TonometryTran.Count() > 0)
                            {
                                foreach (var item in Findings.TonometryTran.ToList())
                                {
                                    var TT = new TonometryTran();

                                    TT.CmpID = item.CmpID;
                                    TT.UIN = item.UIN;
                                    var rid = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.RegistrationTranID).LastOrDefault();
                                    TT.RegistrationtranID = rid;
                                    TT.VisitDatetime = item.VisitDatetime;
                                    TT.TonometryID = item.TonometryID;
                                    TT.BOD = item.BOD;
                                    TT.BOS = item.BOS;
                                    TT.AOD = item.AOD;
                                    TT.AOS = item.AOS;
                                    TT.Dilation = item.Dilation;
                                    TT.Time = item.Time;
                                    TT.StaffID = item.StaffID;
                                    TT.IsDeleted = item.IsDeleted;
                                    TT.RemovedReasons = item.RemovedReasons;
                                    TT.RemovedBy = item.RemovedBy;
                                    TT.RemovedDatetime = item.RemovedDatetime;
                                    TT.CreatedUTC = DateTime.UtcNow;
                                    TT.CreatedBy = item.CreatedBy;
                                    TT.Dropsname = item.Dropsname;
                                    WYNKContext.TonometryTran.AddRange(TT);

                                }
                            }
                        }
                    }

                    else
                    {
                        if (Findings.TonometryTran != null)
                        {
                            if (Findings.TonometryTran.Count() > 0)
                            {
                                foreach (var item in Findings.TonometryTran.ToList())
                                {
                                    var TT = new TonometryTran();
                                    var ID = item.ID;

                                    if (ID != 0)
                                    {

                                        TT = WYNKContext.TonometryTran.Where(x => x.ID == item.ID).FirstOrDefault();

                                        TT.CmpID = item.CmpID;
                                        TT.UIN = item.UIN;
                                        var rid = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.RegistrationTranID).LastOrDefault();
                                        TT.RegistrationtranID = rid;
                                        TT.VisitDatetime = item.VisitDatetime;
                                        TT.TonometryID = item.TonometryID;
                                        TT.BOD = item.BOD;
                                        TT.BOS = item.BOS;
                                        TT.AOD = item.AOD;
                                        TT.AOS = item.AOS;
                                        TT.Dilation = item.Dilation;
                                        TT.Time = item.Time;
                                        TT.StaffID = item.StaffID;
                                        TT.IsDeleted = item.IsDeleted;
                                        TT.RemovedReasons = item.RemovedReasons;
                                        TT.RemovedBy = item.RemovedBy;
                                        TT.RemovedDatetime = item.RemovedDatetime;
                                        TT.UpdatedUTC = DateTime.UtcNow;
                                        TT.UpdatedBy = item.CreatedBy;
                                        TT.Dropsname = item.Dropsname;
                                        WYNKContext.TonometryTran.UpdateRange(TT);

                                    }
                                    else
                                    {
                                        TT.CmpID = item.CmpID;
                                        TT.UIN = item.UIN;
                                        var rid = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.RegistrationTranID).LastOrDefault();
                                        TT.RegistrationtranID = rid;
                                        TT.VisitDatetime = item.VisitDatetime;
                                        TT.TonometryID = item.TonometryID;
                                        TT.BOD = item.BOD;
                                        TT.BOS = item.BOS;
                                        TT.AOD = item.AOD;
                                        TT.AOS = item.AOS;
                                        TT.Dilation = item.Dilation;
                                        TT.Time = item.Time;
                                        TT.StaffID = item.StaffID;
                                        TT.IsDeleted = item.IsDeleted;
                                        TT.RemovedReasons = item.RemovedReasons;
                                        TT.RemovedBy = item.RemovedBy;
                                        TT.RemovedDatetime = item.RemovedDatetime;
                                        TT.CreatedUTC = DateTime.UtcNow;
                                        TT.CreatedBy = item.CreatedBy;
                                        TT.Dropsname = item.Dropsname;
                                        WYNKContext.TonometryTran.AddRange(TT);

                                    }
                                }
                            }
                        }

                    }

                    if (Findings.dilation == "Before")
                    {

                        var roledescription = Findings.Role;
                        var cmpids = Convert.ToInt32(Findings.CMPID);
                        var Regtranid = Convert.ToInt32(Findings.Registrationtranid);
                        var doctorid = Convert.ToInt32(Findings.Userid);
                        var usid = Convert.ToInt32(Findings.usid);
                        var utctimes = CMPSContext.Setup.Where(x => x.CMPID == cmpids).Select(x => x.UTCTime).FirstOrDefault();

                        TimeSpan utctime = TimeSpan.Parse(utctimes);

                        var CurDate = DateTime.UtcNow;
                        var times1 = CurDate.Hour + ":" + CurDate.Minute + ":" + CurDate.Second;
                        var times2 = utctime;
                        var fulltime = TimeSpan.Parse(times1) + times2;
                        DateTime time = DateTime.Today.Add(fulltime);
                        var curfullh = time.Hour;
                        var curfullm = time.Minute;
                        string displayTimes = curfullh + ":" + curfullm;

                        if (Findings.Role != "Doctor" && Findings.Role != "Optometrist")
                        {
                            var role = WYNKContext.Workflowmaster.Where(x => x.Description == "Before Dilation").Select(x => x.ID).FirstOrDefault();
                            var masters = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == true && x.Doctorid == 0 && x.status == true && x.Workflowmasterid != role).ToList();

                            if (masters != null && masters.Count != 0)
                            {

                                masters.All(x => { x.Checkouttime = displayTimes; x.status = false; return true; });
                                WYNKContext.Workflowtran.UpdateRange(masters);

                            }
                            var master = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == true && x.Doctorid == 0 && x.status == true && x.Workflowmasterid == role).ToList();
                            if (master != null && master.Count != 0)
                            {
                                master.All(x => { x.Checkintime = displayTimes; return true; });
                                WYNKContext.Workflowtran.UpdateRange(master);

                            }
                            else
                            {

                                var workflow = new Workflowtran();
                                workflow.Workflowmasterid = role;
                                workflow.Regtranid = Regtranid;
                                workflow.Reception = true;
                                workflow.Doctor = false;
                                workflow.Optometrist = false;
                                workflow.Checkintime = displayTimes;
                                workflow.Checkouttime = "";
                                workflow.CMPID = cmpids;
                                workflow.Date = DateTime.UtcNow.Date;
                                workflow.Createdutc = DateTime.UtcNow;
                                workflow.Createdby = usid;
                                workflow.status = true;
                                workflow.Doctorid = 0;
                                WYNKContext.Workflowtran.Add(workflow);
                                WYNKContext.SaveChanges();
                            }


                        }

                        else
                        {
                            var role = WYNKContext.Workflowmaster.Where(x => x.Description == "Before Dilation").Select(x => x.ID).FirstOrDefault();
                            var masters = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == false && x.Doctorid != 0 && x.status == true && x.Workflowmasterid != role).ToList();


                            if (masters != null && masters.Count != 0)
                            {

                                masters.All(x => { x.Checkouttime = displayTimes; x.status = false; return true; });
                                WYNKContext.Workflowtran.UpdateRange(masters);

                            }
                            var master = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == false && x.Doctorid != 0 && x.status == true && x.Workflowmasterid == role).ToList();
                            if (master != null && master.Count != 0)
                            {
                                master.All(x => { x.Checkintime = displayTimes; return true; });
                                WYNKContext.Workflowtran.UpdateRange(master);

                            }
                            else
                            {


                                var workflow = new Workflowtran();
                                workflow.Workflowmasterid = role;
                                workflow.Regtranid = Regtranid;
                                workflow.Reception = false;
                                if (Findings.Role == "Doctor")
                                {
                                    workflow.Doctor = true;
                                }
                                if (Findings.Role == "Optometrist")
                                {
                                    workflow.Optometrist = true;
                                }
                                workflow.Checkintime = displayTimes;
                                workflow.Checkouttime = "";
                                workflow.CMPID = cmpids;
                                workflow.Date = DateTime.UtcNow.Date;
                                workflow.Createdutc = DateTime.UtcNow;
                                workflow.Createdby = usid;
                                workflow.status = true;
                                workflow.Doctorid = doctorid;
                                WYNKContext.Workflowtran.Add(workflow);
                                WYNKContext.SaveChanges();
                            }


                        }



                    }

                    else
                    {
                        var roledescription = Findings.Role;
                        var cmpids = Convert.ToInt32(Findings.CMPID);
                        var Regtranid = Convert.ToInt32(Findings.Registrationtranid);
                        var doctorid = Convert.ToInt32(Findings.Userid);
                        var usid = Convert.ToInt32(Findings.usid);
                        var utctimes = CMPSContext.Setup.Where(x => x.CMPID == cmpids).Select(x => x.UTCTime).FirstOrDefault();

                        TimeSpan utctime = TimeSpan.Parse(utctimes);

                        var CurDate = DateTime.UtcNow;
                        var times1 = CurDate.Hour + ":" + CurDate.Minute + ":" + CurDate.Second;
                        var times2 = utctime;
                        var fulltime = TimeSpan.Parse(times1) + times2;
                        DateTime time = DateTime.Today.Add(fulltime);
                        var curfullh = time.Hour;
                        var curfullm = time.Minute;
                        string displayTimes = curfullh + ":" + curfullm;

                        if (Findings.Role != "Doctor" && Findings.Role != "Optometrist")
                        {
                            var role = WYNKContext.Workflowmaster.Where(x => x.Description == "After Dilation").Select(x => x.ID).FirstOrDefault();
                            var masters = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == true && x.Doctorid == 0 && x.status == true && x.Workflowmasterid != role).ToList();

                            if (masters != null && masters.Count != 0)
                            {

                                masters.All(x => { x.Checkouttime = displayTimes; x.status = false; return true; });
                                WYNKContext.Workflowtran.UpdateRange(masters);

                            }
                            var master = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == true && x.Doctorid == 0 && x.status == true && x.Workflowmasterid == role).ToList();
                            if (master != null && master.Count != 0)
                            {
                                master.All(x => { x.Checkintime = displayTimes; return true; });
                                WYNKContext.Workflowtran.UpdateRange(master);

                            }
                            else
                            {

                                var workflow = new Workflowtran();
                                workflow.Workflowmasterid = role;
                                workflow.Regtranid = Regtranid;
                                workflow.Reception = true;
                                workflow.Doctor = false;
                                workflow.Optometrist = false;
                                workflow.Checkintime = displayTimes;
                                workflow.Checkouttime = "";
                                workflow.CMPID = cmpids;
                                workflow.Date = DateTime.UtcNow.Date;
                                workflow.Createdutc = DateTime.UtcNow;
                                workflow.Createdby = usid;
                                workflow.status = true;
                                workflow.Doctorid = 0;
                                WYNKContext.Workflowtran.Add(workflow);
                                WYNKContext.SaveChanges();
                            }


                        }

                        else
                        {
                            var role = WYNKContext.Workflowmaster.Where(x => x.Description == "After Dilation").Select(x => x.ID).FirstOrDefault();
                            var masters = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == false && x.Doctorid != 0 && x.status == true && x.Workflowmasterid != role).ToList();


                            if (masters != null && masters.Count != 0)
                            {

                                masters.All(x => { x.Checkouttime = displayTimes; x.status = false; return true; });
                                WYNKContext.Workflowtran.UpdateRange(masters);

                            }
                            var master = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == false && x.Doctorid != 0 && x.status == true && x.Workflowmasterid == role).ToList();
                            if (master != null && master.Count != 0)
                            {
                                master.All(x => { x.Checkintime = displayTimes; return true; });
                                WYNKContext.Workflowtran.UpdateRange(master);

                            }
                            else
                            {


                                var workflow = new Workflowtran();
                                workflow.Workflowmasterid = role;
                                workflow.Regtranid = Regtranid;
                                workflow.Reception = false;
                                if (Findings.Role == "Doctor")
                                {
                                    workflow.Doctor = true;
                                }
                                if (Findings.Role == "Optometrist")
                                {
                                    workflow.Optometrist = true;
                                }
                                workflow.Checkintime = displayTimes;
                                workflow.Checkouttime = "";
                                workflow.CMPID = cmpids;
                                workflow.Date = DateTime.UtcNow.Date;
                                workflow.Createdutc = DateTime.UtcNow;
                                workflow.Createdby = usid;
                                workflow.status = true;
                                workflow.Doctorid = doctorid;
                                WYNKContext.Workflowtran.Add(workflow);
                                WYNKContext.SaveChanges();
                            }


                        }

                    }

                    dbContextTransaction.Commit();


                    if (WYNKContext.SaveChanges() >= 0)
                    {

                    }
                    return new
                    {

                        Success = true,
                        Message = CommonMessage.saved,
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
                Message = CommonMessage.Missing,
            };
        }

        public dynamic UpdateFindings(Findings Findings, string UIN, int DOCID, string cpname, string dcname)
        {




            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var find = new Finding();
                    var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Findings.Finding.UIN).Select(x => x.RegistrationTranID).LastOrDefault();
                    var docid = WYNKContext.PatientAssign.Where(x => x.RegistrationTranID == regid).Select(x => x.DoctorID).FirstOrDefault();

                    find.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                    find.CmpID = Findings.Finding.CmpID;
                    find.RegistrationTranID = regid;
                    find.UIN = Findings.Finding.UIN;

                    find.SlitLampODImagePath = Findings.Finding.SlitLampODImagePath;
                    find.SlitLampOSImagePath = Findings.Finding.SlitLampOSImagePath;
                    find.IOLNCTOD = Findings.Finding.IOLNCTOD;
                    find.IOLNCTOS = Findings.Finding.IOLNCTOS;
                    find.IOLATOD = Findings.Finding.IOLATOD;
                    find.IOLATOS = Findings.Finding.IOLATOS;

                    find.ProposedSurgeryPeriod = Findings.Finding.ProposedSurgeryPeriod;
                    find.TreatmentAdvice = Findings.Finding.TreatmentAdvice;
                    find.IsSurgeryAdviced = Findings.Finding.IsSurgeryAdviced;

                    var Adm = WYNKContext.Admission.Where(x => x.UIN == UIN && x.CMPID == Findings.Finding.CmpID && x.DischargeID == null).ToList();
                    if (Findings.Finding.Consultation != null && Adm.Count == 0)
                    {
                        find.Consultation = Findings.Finding.Consultation;
                    }
                    if (Findings.Finding.ReviewDate != null)
                    {

                        find.ReviewDate = Findings.Finding.ReviewDate.Value.Date.AddDays(1);
                    }

                    else
                    {
                        find.ReviewDate = null;
                    }
                    find.IsBilled = false;
                    find.CreatedBy = Findings.Finding.CreatedBy;
                    find.CreatedUTC = DateTime.UtcNow;
                    find.IOLNCTbdOD = Findings.Finding.IOLNCTbdOD;
                    find.IOLNCTbdOS = Findings.Finding.IOLNCTbdOS;
                    find.IOLATbdOD = Findings.Finding.IOLATbdOD;
                    find.IOLATbdOS = Findings.Finding.IOLATbdOS;
                    find.Category = Findings.Finding.Category;
                    find.Categoryos = Findings.Finding.Categoryos;
                    find.DistSphod = Findings.Finding.DistSphod;
                    find.NearCylod = Findings.Finding.NearCylod;
                    find.N_V_DESCod = Findings.Finding.N_V_DESCod;
                    find.PinAxisod = Findings.Finding.PinAxisod;
                    find.DistSphos = Findings.Finding.DistSphos;
                    find.NearCylos = Findings.Finding.NearCylos;
                    find.N_V_DESCos = Findings.Finding.N_V_DESCos;
                    find.PinAxisos = Findings.Finding.PinAxisos;

                    find.PgDtls = Findings.Finding.PgDtls;
                    find.PgSphOD = Findings.Finding.PgSphOD;
                    find.PgCylOD = Findings.Finding.PgCylOD;
                    find.PgAxisOD = Findings.Finding.PgAxisOD;
                    find.PgAddOD = Findings.Finding.PgAddOD;
                    find.PgSphOS = Findings.Finding.PgSphOS;
                    find.PgCylOS = Findings.Finding.PgCylOS;
                    find.PgAxisOS = Findings.Finding.PgAxisOS;
                    find.PgAddOS = Findings.Finding.PgAddOS;

                    find.Instrument = Findings.Finding.Instrument;
                    find.RDySphOD = Findings.Finding.RDySphOD;
                    find.RDyCylOD = Findings.Finding.RDyCylOD;
                    find.RDyAxisOD = Findings.Finding.RDyAxisOD;
                    find.RDySphOS = Findings.Finding.RDySphOS;
                    find.RDyCylOS = Findings.Finding.RDyCylOS;
                    find.RDyAxisOS = Findings.Finding.RDyAxisOS;
                    find.RWtSphOD = Findings.Finding.RWtSphOD;
                    find.RWtCylOD = Findings.Finding.RWtCylOD;
                    find.RWtAxisOD = Findings.Finding.RWtAxisOD;
                    find.RWtSphOS = Findings.Finding.RWtSphOS;
                    find.RWtCylOS = Findings.Finding.RWtCylOS;
                    find.RWtAxisOS = Findings.Finding.RWtAxisOS;

                    find.PGlassOD = Findings.Finding.PGlassOD;
                    find.PGlassOS = Findings.Finding.PGlassOS;

                    find.TimeNCT = Findings.Finding.TimeNCT;
                    find.TimeAT = Findings.Finding.TimeAT;
                    find.PFC = Findings.Finding.PFC;

                    WYNKContext.Findings.Add(find);

                    string userid = Convert.ToString(DOCID);
                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(cpname, "Findings", "User name :", dcname, "User ID :", userid, "Mode : Submit");

                    object names = find;
                    oErrorLogs.WriteErrorLogArray("Findings", names);

                    var fid = find.ID;
                    var usid = find.CreatedBy;
                    var cmid = find.CmpID;
                    var consult = find.Consultation;
                    var uid = find.UIN;
                    var ranid = find.RandomUniqueID;

                    if (Findings.FindingsExt.Count() > 0)
                    {
                        foreach (var item in Findings.FindingsExt.ToList())

                        {

                            var fe = new FindingsExt();
                            fe.ICDCode = item.ICDCode;
                            fe.FindingsID = ranid;
                            fe.ICDSpecialityid = item.ICDSpecialityid;
                            fe.IsOD = item.IsOD;
                            fe.IsOS = item.IsOS;
                            fe.IsOU = item.IsOU;
                            fe.SurgeryComplete = false;
                            fe.Isdeleted = false;
                            fe.CreatedUTC = DateTime.UtcNow;
                            fe.CreatedBy = usid;
                            WYNKContext.FindingsExt.AddRange(fe);
                            ErrorLog oErrorLogstran = new ErrorLog();
                            object namestr = fe;
                            oErrorLogstran.WriteErrorLogArray("FindingsExt", namestr);
                        }

                    }






                    var regtid = WYNKContext.RegistrationTran.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.RegistrationTranID).FirstOrDefault();

                    var Srid = Convert.ToInt32(WYNKContext.SquintTran.Where(x => x.RID == regtid).OrderByDescending(x => x.CreatedUTC).Select(r => r.RID).FirstOrDefault());

                    if (Srid == 0)
                    {

                        if (Findings.SquintTran.Count() > 0)
                        {
                            foreach (var item in Findings.SquintTran.ToList())
                            {
                                var ST = new SquintTran();

                                ST.RID = regtid;
                                ST.IsDVOD = item.IsDVOD;
                                ST.IsDVOS = item.IsDVOS;
                                ST.IsDVOU = item.IsDVOU;
                                ST.Date = DateTime.UtcNow;
                                ST.SquintType = item.SquintType;
                                ST.SquintDiagnosisDescription = item.SquintDiagnosisDescription;
                                ST.CreatedUTC = DateTime.UtcNow;
                                ST.CreatedBy = usid;
                                ST.IsActive = true;
                                WYNKContext.SquintTran.AddRange(ST);
                                ErrorLog oErrorLogstran = new ErrorLog();
                                object namestr = ST;
                                oErrorLogstran.WriteErrorLogArray("SquintTran", namestr);
                            }
                        }
                    }
                    else
                    {

                        if (Findings.SquintTran.Count() > 0)
                        {
                            foreach (var item in Findings.SquintTran.ToList())
                            {
                                var ST = new SquintTran();

                                if (item.ID != 0)
                                {
                                    ST = WYNKContext.SquintTran.Where(x => x.ID == item.ID).FirstOrDefault();
                                    ST.RID = regtid;
                                    ST.IsDVOD = item.IsDVOD;
                                    ST.IsDVOS = item.IsDVOS;
                                    ST.IsDVOU = item.IsDVOU;
                                    ST.Date = DateTime.UtcNow;
                                    ST.SquintType = item.SquintType;
                                    ST.SquintDiagnosisDescription = item.SquintDiagnosisDescription;
                                    ST.UpdatedUTC = DateTime.UtcNow;
                                    ST.UpdatedBy = usid;
                                    WYNKContext.SquintTran.UpdateRange(ST);
                                    ErrorLog oErrorLogstran = new ErrorLog();
                                    object namestr = ST;
                                    oErrorLogstran.WriteErrorLogArray("SquintTran", namestr);
                                }
                                else
                                {
                                    ST.RID = regtid;
                                    ST.IsDVOD = item.IsDVOD;
                                    ST.IsDVOS = item.IsDVOS;
                                    ST.IsDVOU = item.IsDVOU;
                                    ST.Date = DateTime.UtcNow;
                                    ST.SquintType = item.SquintType;
                                    ST.SquintDiagnosisDescription = item.SquintDiagnosisDescription;
                                    ST.CreatedUTC = DateTime.UtcNow;
                                    ST.CreatedBy = usid;
                                    ST.IsActive = true;
                                    WYNKContext.SquintTran.AddRange(ST);
                                    ErrorLog oErrorLogstran = new ErrorLog();
                                    object namestr = ST;
                                    oErrorLogstran.WriteErrorLogArray("SquintTran", namestr);
                                }
                            }
                        }

                    }

                    if (Findings.SquintTranDeletefi != null)
                    {
                        if (Findings.SquintTranDeletefi.Count > 0)
                        {
                            foreach (var item in Findings.SquintTranDeletefi.ToList())
                            {

                                var mas = WYNKContext.SquintTran.Where(x => x.ID == item.ID).FirstOrDefault();
                                mas.IsActive = false;
                                WYNKContext.Entry(mas).State = EntityState.Modified;
                                ErrorLog oErrorLogstran = new ErrorLog();
                                object namestr = mas;
                                oErrorLogstran.WriteErrorLogArray("SquintTran", namestr);
                            }
                        }
                    }

                    var Screid = Convert.ToInt32(WYNKContext.SchirmerTest.Where(x => x.RID == regtid).OrderByDescending(x => x.CreatedUTC).Select(r => r.RID).FirstOrDefault());

                    if (Screid == 0)
                    {

                        if (Findings.SchirmerTest.Count() > 0)
                        {
                            foreach (var item in Findings.SchirmerTest.ToList())
                            {
                                var SCT = new SchirmerTest();

                                SCT.CmpID = cmid;
                                SCT.RID = regtid;
                                SCT.VisitDatetime = item.VisitDatetime;
                                SCT.Ocular = item.Ocular;
                                SCT.Time = item.Time;
                                SCT.Tearsecretion = item.Tearsecretion;
                                SCT.Examinedby = item.Examinedby;
                                SCT.Remarks = item.Remarks;
                                SCT.CreatedUTC = DateTime.UtcNow;
                                SCT.CreatedBy = usid;
                                SCT.IsActive = true;
                                WYNKContext.SchirmerTest.AddRange(SCT);
                                ErrorLog oErrorLogstran = new ErrorLog();
                                object namestr = SCT;
                                oErrorLogstran.WriteErrorLogArray("SchirmerTest", namestr);
                            }
                        }
                    }
                    else
                    {

                        if (Findings.SchirmerTest.Count() > 0)
                        {
                            foreach (var item in Findings.SchirmerTest.ToList())
                            {
                                var SCT = new SchirmerTest();

                                if (item.ID != 0)
                                {
                                    SCT = WYNKContext.SchirmerTest.Where(x => x.ID == item.ID).FirstOrDefault();
                                    SCT.RID = regtid;
                                    SCT.CmpID = cmid;
                                    SCT.VisitDatetime = item.VisitDatetime;
                                    SCT.Ocular = item.Ocular;
                                    SCT.Time = item.Time;
                                    SCT.Tearsecretion = item.Tearsecretion;
                                    SCT.Examinedby = item.Examinedby;
                                    SCT.Remarks = item.Remarks;
                                    SCT.UpdatedUTC = DateTime.UtcNow;
                                    SCT.UpdatedBy = usid;
                                    WYNKContext.SchirmerTest.UpdateRange(SCT);
                                    ErrorLog oErrorLogstran = new ErrorLog();
                                    object namestr = SCT;
                                    oErrorLogstran.WriteErrorLogArray("SchirmerTest", namestr);
                                }
                                else
                                {
                                    SCT.CmpID = cmid;
                                    SCT.RID = regtid;
                                    SCT.VisitDatetime = item.VisitDatetime;
                                    SCT.Ocular = item.Ocular;
                                    SCT.Time = item.Time;
                                    SCT.Tearsecretion = item.Tearsecretion;
                                    SCT.Examinedby = item.Examinedby;
                                    SCT.CreatedUTC = DateTime.UtcNow;
                                    SCT.CreatedBy = usid;
                                    SCT.Remarks = item.Remarks;
                                    SCT.IsActive = true;
                                    WYNKContext.SchirmerTest.AddRange(SCT);
                                    ErrorLog oErrorLogstran = new ErrorLog();
                                    object namestr = SCT;
                                    oErrorLogstran.WriteErrorLogArray("SchirmerTest", namestr);
                                }
                            }
                        }

                    }

                    if (Findings.SchDeletefi != null)
                    {
                        if (Findings.SchDeletefi.Count > 0)
                        {
                            foreach (var item in Findings.SchDeletefi.ToList())
                            {

                                var mas = WYNKContext.SchirmerTest.Where(x => x.ID == item.ID).FirstOrDefault();
                                mas.IsActive = false;
                                WYNKContext.Entry(mas).State = EntityState.Modified;
                                ErrorLog oErrorLogstran = new ErrorLog();
                                object namestr = mas;
                                oErrorLogstran.WriteErrorLogArray("SchirmerTest", namestr);
                            }
                        }
                    }

                    var glreid = Convert.ToInt32(WYNKContext.GlaucomaEvaluation.Where(x => x.RID == regtid).OrderByDescending(x => x.CreatedUTC).Select(r => r.RID).FirstOrDefault());

                    if (glreid == 0)
                    {

                        if (Findings.GlaucomaEvaluation.Count() > 0)
                        {
                            foreach (var item in Findings.GlaucomaEvaluation.ToList())
                            {

                                var ge = new GlaucomaEvaluation();
                                ge.RID = regtid;
                                ge.Date = item.Date;
                                ge.REGAT = item.REGAT;
                                ge.LEGAT = item.LEGAT;
                                ge.Time = item.Time;
                                ge.REoptic = item.REoptic;
                                ge.LEoptic = item.LEoptic;
                                ge.GlaucomaDrugs = item.GlaucomaDrugs;
                                ge.HVF = item.HVF;
                                ge.OCT = item.OCT;
                                ge.Intervention = item.Intervention;
                                ge.StableProgression = item.StableProgression;
                                ge.CreatedUTC = DateTime.UtcNow;
                                ge.CreatedBy = usid;
                                ge.IsActive = true;
                                WYNKContext.GlaucomaEvaluation.AddRange(ge);
                                ErrorLog oErrorLogge = new ErrorLog();
                                object namege = ge;
                                oErrorLogge.WriteErrorLogArray("GlaucomaEvaluation", namege);
                            }
                        }
                    }
                    else
                    {

                        if (Findings.GlaucomaEvaluation.Count() > 0)
                        {
                            foreach (var item in Findings.GlaucomaEvaluation.ToList())
                            {
                                var ge = new GlaucomaEvaluation();

                                if (item.ID != 0)
                                {
                                    ge = WYNKContext.GlaucomaEvaluation.Where(x => x.ID == item.ID).FirstOrDefault();
                                    ge.RID = regtid;
                                    ge.Date = item.Date;
                                    ge.REGAT = item.REGAT;
                                    ge.LEGAT = item.LEGAT;
                                    ge.Time = item.Time;
                                    ge.REoptic = item.REoptic;
                                    ge.LEoptic = item.LEoptic;
                                    ge.GlaucomaDrugs = item.GlaucomaDrugs;
                                    ge.HVF = item.HVF;
                                    ge.OCT = item.OCT;
                                    ge.Intervention = item.Intervention;
                                    ge.StableProgression = item.StableProgression;
                                    ge.UpdatedUTC = DateTime.UtcNow;
                                    ge.UpdatedBy = usid;
                                    WYNKContext.GlaucomaEvaluation.UpdateRange(ge);
                                    ErrorLog oErrorLogge = new ErrorLog();
                                    object namege = ge;
                                    oErrorLogge.WriteErrorLogArray("GlaucomaEvaluation", namege);
                                }
                                else
                                {
                                    ge.RID = regtid;
                                    ge.Date = item.Date;
                                    ge.REGAT = item.REGAT;
                                    ge.LEGAT = item.LEGAT;
                                    ge.Time = item.Time;
                                    ge.REoptic = item.REoptic;
                                    ge.LEoptic = item.LEoptic;
                                    ge.GlaucomaDrugs = item.GlaucomaDrugs;
                                    ge.HVF = item.HVF;
                                    ge.OCT = item.OCT;
                                    ge.Intervention = item.Intervention;
                                    ge.StableProgression = item.StableProgression;
                                    ge.CreatedUTC = DateTime.UtcNow;
                                    ge.CreatedBy = usid;
                                    ge.IsActive = true;
                                    WYNKContext.GlaucomaEvaluation.AddRange(ge);
                                    ErrorLog oErrorLogge = new ErrorLog();
                                    object namege = ge;
                                    oErrorLogge.WriteErrorLogArray("GlaucomaEvaluation", namege);
                                }
                            }
                        }

                    }

                    if (Findings.GlaDeletefi != null)
                    {
                        if (Findings.GlaDeletefi.Count > 0)
                        {
                            foreach (var item in Findings.GlaDeletefi.ToList())
                            {

                                var mas = WYNKContext.GlaucomaEvaluation.Where(x => x.ID == item.ID).FirstOrDefault();
                                mas.IsActive = false;
                                WYNKContext.Entry(mas).State = EntityState.Modified;

                            }
                        }
                    }

                    var dye = new DryEyes();
                    dye.FindingsID = ranid;
                    dye.NorTMH = Findings.DryEyes.NorTMH;
                    dye.NorTMHtxt = Findings.DryEyes.NorTMHtxt;
                    dye.DryTMH = Findings.DryEyes.DryTMH;
                    dye.DryTMHtxt = Findings.DryEyes.DryTMHtxt;
                    dye.TBUT = Findings.DryEyes.TBUT;
                    dye.TBUTtxt = Findings.DryEyes.TBUTtxt;
                    dye.NiTBUT = Findings.DryEyes.NiTBUT;
                    dye.NiTBUTtxt = Findings.DryEyes.NiTBUTtxt;

                    dye.CreatedUTC = DateTime.UtcNow;
                    dye.CreatedBy = usid;
                    WYNKContext.DryEyes.Add(dye);

                    var sle = new SlitlampExtn();

                    sle.FindingsID = ranid;
                    sle.HorMeasurementod1 = Findings.SlitlampExtn.HorMeasurementod1;
                    sle.HorMeasurementod2 = Findings.SlitlampExtn.HorMeasurementod2;
                    sle.HorMeasurementod3 = Findings.SlitlampExtn.HorMeasurementod3;
                    sle.HorMeasurementod4 = Findings.SlitlampExtn.HorMeasurementod4;
                    sle.HorMeasurementod5 = Findings.SlitlampExtn.HorMeasurementod5;
                    sle.HorMeasurementod6 = Findings.SlitlampExtn.HorMeasurementod6;
                    sle.HorMeasurementos1 = Findings.SlitlampExtn.HorMeasurementos1;
                    sle.HorMeasurementos2 = Findings.SlitlampExtn.HorMeasurementos2;
                    sle.HorMeasurementos3 = Findings.SlitlampExtn.HorMeasurementos3;
                    sle.HorMeasurementos4 = Findings.SlitlampExtn.HorMeasurementos4;
                    sle.HorMeasurementos5 = Findings.SlitlampExtn.HorMeasurementos5;
                    sle.HorMeasurementos6 = Findings.SlitlampExtn.HorMeasurementos6;

                    sle.VerMeasurementod1 = Findings.SlitlampExtn.VerMeasurementod1;
                    sle.VerMeasurementod2 = Findings.SlitlampExtn.VerMeasurementod2;
                    sle.VerMeasurementod3 = Findings.SlitlampExtn.VerMeasurementod3;
                    sle.VerMeasurementod4 = Findings.SlitlampExtn.VerMeasurementod4;
                    sle.VerMeasurementod5 = Findings.SlitlampExtn.VerMeasurementod5;
                    sle.VerMeasurementod6 = Findings.SlitlampExtn.VerMeasurementod6;
                    sle.VerMeasurementos1 = Findings.SlitlampExtn.VerMeasurementos1;
                    sle.VerMeasurementos2 = Findings.SlitlampExtn.VerMeasurementos2;
                    sle.VerMeasurementos3 = Findings.SlitlampExtn.VerMeasurementos3;
                    sle.VerMeasurementos4 = Findings.SlitlampExtn.VerMeasurementos4;
                    sle.VerMeasurementos5 = Findings.SlitlampExtn.VerMeasurementos5;
                    sle.VerMeasurementos6 = Findings.SlitlampExtn.VerMeasurementos6;

                    sle.OcularMovementod1 = Findings.SlitlampExtn.OcularMovementod1;
                    sle.OcularMovementod2 = Findings.SlitlampExtn.OcularMovementod2;
                    sle.OcularMovementod3 = Findings.SlitlampExtn.OcularMovementod3;
                    sle.OcularMovementod4 = Findings.SlitlampExtn.OcularMovementod4;
                    sle.OcularMovementod5 = Findings.SlitlampExtn.OcularMovementod5;
                    sle.OcularMovementod6 = Findings.SlitlampExtn.OcularMovementod6;
                    sle.OcularMovementod7 = Findings.SlitlampExtn.OcularMovementod7;
                    sle.OcularMovementod8 = Findings.SlitlampExtn.OcularMovementod8;

                    sle.OcularMovementos1 = Findings.SlitlampExtn.OcularMovementos1;
                    sle.OcularMovementos2 = Findings.SlitlampExtn.OcularMovementos2;
                    sle.OcularMovementos3 = Findings.SlitlampExtn.OcularMovementos3;
                    sle.OcularMovementos4 = Findings.SlitlampExtn.OcularMovementos4;
                    sle.OcularMovementos5 = Findings.SlitlampExtn.OcularMovementos5;
                    sle.OcularMovementos6 = Findings.SlitlampExtn.OcularMovementos6;
                    sle.OcularMovementos7 = Findings.SlitlampExtn.OcularMovementos7;
                    sle.OcularMovementos8 = Findings.SlitlampExtn.OcularMovementos8;
                    sle.CreatedUTC = DateTime.UtcNow;
                    sle.CreatedBy = usid;
                    WYNKContext.SlitlampExtn.Add(sle);

                    ErrorLog oErrorLogsslext = new ErrorLog();
                    object nameslext = sle;
                    oErrorLogsslext.WriteErrorLogArray("SlitlampExtn", nameslext);

                    var glau = new Glaucoma();

                    glau.FindingsId = ranid;
                    glau.CmpId = cmid;
                    glau.GaunioscopyODG1 = Findings.Glaucoma.GaunioscopyODG1;
                    glau.GaunioscopyODG2 = Findings.Glaucoma.GaunioscopyODG2;
                    glau.GaunioscopyODG3 = Findings.Glaucoma.GaunioscopyODG3;
                    glau.GaunioscopyODG4 = Findings.Glaucoma.GaunioscopyODG4;
                    glau.GaunioscopyOSG1 = Findings.Glaucoma.GaunioscopyOSG1;
                    glau.GaunioscopyOSG2 = Findings.Glaucoma.GaunioscopyOSG2;
                    glau.GaunioscopyOSG3 = Findings.Glaucoma.GaunioscopyOSG3;
                    glau.GaunioscopyOSG4 = Findings.Glaucoma.GaunioscopyOSG4;
                    glau.pachymetryod = Findings.Glaucoma.pachymetryod;
                    glau.pachymetryos = Findings.Glaucoma.pachymetryos;
                    glau.Impression = Findings.Glaucoma.Impression;
                    glau.CreatedUTC = DateTime.UtcNow;
                    glau.CreatedBy = usid;

                    glau.GaunioscopyODD1 = Findings.Glaucoma.GaunioscopyODD1;
                    glau.GaunioscopyODD2 = Findings.Glaucoma.GaunioscopyODD2;
                    glau.GaunioscopyODD3 = Findings.Glaucoma.GaunioscopyODD3;
                    glau.GaunioscopyODD4 = Findings.Glaucoma.GaunioscopyODD4;
                    glau.GaunioscopyOSD1 = Findings.Glaucoma.GaunioscopyOSD1;
                    glau.GaunioscopyOSD2 = Findings.Glaucoma.GaunioscopyOSD2;
                    glau.GaunioscopyOSD3 = Findings.Glaucoma.GaunioscopyOSD3;
                    glau.GaunioscopyOSD4 = Findings.Glaucoma.GaunioscopyOSD4;
                    WYNKContext.Glaucoma.Add(glau);
                    ErrorLog oErrorLogsgl = new ErrorLog();
                    object nameslgl = glau;
                    oErrorLogsslext.WriteErrorLogArray("Glaucoma", nameslgl);

                    var squet = new SquintExt();
                    squet.FindingsID = ranid;
                    squet.CMPID = cmid;
                    squet.UIN = uid;
                    squet.VisitDate = DateTime.UtcNow;
                    squet.Doctorid = usid;
                    squet.VF1 = Findings.SquintExt.VF1;
                    squet.VF1Value = Findings.SquintExt.VF1Value;
                    squet.VF2 = Findings.SquintExt.VF2;
                    squet.VF2Value = Findings.SquintExt.VF2Value;
                    squet.VF3 = Findings.SquintExt.VF3;
                    squet.VF3Value = Findings.SquintExt.VF3Value;
                    squet.VF4 = Findings.SquintExt.VF4;
                    squet.VF4Value = Findings.SquintExt.VF4Value;
                    squet.AngleKappa = Findings.SquintExt.AngleKappa;
                    squet.Patterns = Findings.SquintExt.Patterns;
                    squet.ACAMethod = Findings.SquintExt.ACAMethod;
                    squet.ACAValue = Findings.SquintExt.ACAValue;
                    squet.WFDTDistance = Findings.SquintExt.WFDTDistance;
                    squet.WFDTNear = Findings.SquintExt.WFDTNear;
                    squet.StreopsisMethod = Findings.SquintExt.StreopsisMethod;
                    squet.StreopsisValue = Findings.SquintExt.StreopsisValue;
                    squet.ARC = Findings.SquintExt.ARC;
                    squet.PBCTDisHor = Findings.SquintExt.PBCTDisHor;
                    squet.PBCTDisHorValue = Findings.SquintExt.PBCTDisHorValue;
                    squet.PBCTDisVer = Findings.SquintExt.PBCTDisVer;
                    squet.PBCTDisVerValue = Findings.SquintExt.PBCTDisVerValue;
                    squet.PBCTNearHor = Findings.SquintExt.PBCTNearHor;
                    squet.PBCTNearHorValue = Findings.SquintExt.PBCTNearHorValue;
                    squet.PBCTNearVer = Findings.SquintExt.PBCTNearVer;
                    squet.PBCTNearVerValue = Findings.SquintExt.PBCTNearVerValue;
                    squet.ModKrimHor = Findings.SquintExt.ModKrimHor;
                    squet.ModKrimHorValue = Findings.SquintExt.ModKrimHorValue;
                    squet.ModKrimVer = Findings.SquintExt.ModKrimVer;
                    squet.ModKrimVerValue = Findings.SquintExt.ModKrimVerValue;
                    squet.PriDevHor = Findings.SquintExt.PriDevHor;
                    squet.PriDevHorValue = Findings.SquintExt.PriDevHorValue;
                    squet.PriDevVer = Findings.SquintExt.PriDevVer;
                    squet.PriDevVerValue = Findings.SquintExt.PriDevVerValue;
                    squet.SecDevHor = Findings.SquintExt.SecDevHor;
                    squet.SecDevHorValue = Findings.SquintExt.SecDevHorValue;
                    squet.SecDevVer = Findings.SquintExt.SecDevVer;
                    squet.SecDevVerValue = Findings.SquintExt.SecDevVerValue;
                    squet.Amplitude = Findings.SquintExt.Amplitude;
                    squet.Frequency = Findings.SquintExt.Frequency;
                    squet.Type = Findings.SquintExt.Type;
                    squet.Pursuit = Findings.SquintExt.Pursuit;
                    squet.Saccade = Findings.SquintExt.Saccade;
                    squet.ConjugateDissociated = Findings.SquintExt.ConjugateDissociated;
                    squet.ABHeadPos = Findings.SquintExt.ABHeadPos;
                    squet.FreqOnConvergence = Findings.SquintExt.FreqOnConvergence;
                    squet.Occlusion = Findings.SquintExt.Occlusion;
                    squet.Oscillopsia = Findings.SquintExt.Oscillopsia;
                    squet.AssHeadPosture = Findings.SquintExt.AssHeadPosture;
                    squet.SquintBasicExam = Findings.SquintExt.SquintBasicExam;
                    squet.SpecialTest = Findings.SquintExt.SpecialTest;
                    squet.IsActive = true;
                    squet.CreatedBy = usid;
                    squet.CreatedUTC = DateTime.UtcNow;
                    WYNKContext.SquintExt.Add(squet);

                    if (Findings.SQI.Count() > 0)
                    {
                        foreach (var items in Findings.SQI.ToList())
                        {

                            var sqi = new SquintImage();
                            sqi.SquintID = ranid;
                            sqi.UIN = UIN;
                            sqi.Eye = items.Eye;
                            sqi.UploadedOn = DateTime.Now.Date;
                            sqi.CreatedUTC = DateTime.Now;
                            sqi.CreatedBy = usid;
                            WYNKContext.SquintImage.Add(sqi);


                        }
                    }


                    if (Findings.SlitLampImage.Count() > 0)
                    {
                        foreach (var items in Findings.SlitLampImage.ToList())
                        {

                            var slm = new SlitLampImage();
                            slm.FindingsID = ranid;
                            slm.UIN = UIN;
                            slm.Eye = items.Eye;
                            slm.UploadedOn = DateTime.Now.Date;
                            slm.CreatedUTC = DateTime.Now;
                            slm.CreatedBy = usid;
                            WYNKContext.SlitLampImage.Add(slm);


                        }
                    }

                    if (Findings.EyelidImage.Count() > 0)
                    {
                        foreach (var items in Findings.EyelidImage.ToList())
                        {

                            var elm = new EyelidImage();
                            elm.FindingsID = ranid;
                            elm.UIN = UIN;
                            elm.Eye = items.Eye;
                            elm.UploadedOn = DateTime.Now.Date;
                            elm.CreatedUTC = DateTime.Now;
                            elm.CreatedBy = usid;
                            WYNKContext.EyelidImage.Add(elm);


                        }
                    }

                    if (Findings.FundusImage.Count() > 0)
                    {
                        foreach (var items in Findings.FundusImage.ToList())
                        {

                            var fn = new FundusImage();
                            fn.FindingsID = ranid;
                            fn.UIN = UIN;
                            fn.Eye = items.Eye;
                            fn.UploadedOn = DateTime.Now.Date;
                            fn.CreatedUTC = DateTime.Now;
                            fn.CreatedBy = usid;
                            WYNKContext.FundusImage.Add(fn);


                        }
                    }

                    if (Findings.GlaucomaImage.Count() > 0)
                    {
                        foreach (var items in Findings.GlaucomaImage.ToList())
                        {

                            var gla = new GlaucomaImage();
                            gla.FindingsID = ranid;
                            gla.UIN = UIN;
                            gla.Eye = items.Eye;
                            gla.UploadedOn = DateTime.Now.Date;
                            gla.CreatedUTC = DateTime.UtcNow;
                            gla.CreatedBy = usid;
                            WYNKContext.GlaucomaImage.Add(gla);

                        }
                    }

                    if (Findings.VisualField.Count() > 0)
                    {
                        foreach (var items in Findings.VisualField.ToList())
                        {

                            var vf = new VisualField();
                            vf.FindingsID = ranid;
                            vf.UIN = UIN;
                            vf.Eye = items.Eye;
                            vf.UploadedOn = DateTime.Now.Date;
                            vf.CreatedUTC = DateTime.Now;
                            vf.CreatedBy = usid;
                            WYNKContext.VisualField.Add(vf);

                        }
                    }







                    if (Findings.SLT.Count() > 0)
                    {


                        foreach (var item in Findings.SLT.ToList())

                        {
                            var Slit = new SlitLamp();

                            Slit.SlitLampID = item.SlitLampID;
                            Slit.SlitLampLineItemID = item.SlitLampLineItemID;
                            Slit.FindingID = ranid;
                            Slit.IsOD = item.IsOD;
                            Slit.IsOS = item.IsOS;
                            Slit.IsOU = item.IsOU;
                            Slit.Description = item.Description;
                            Slit.IsRemoved = false;
                            Slit.CreatedBy = usid;
                            Slit.CreatedUTC = DateTime.UtcNow;
                            Slit.SlitProperty = item.SlitProperty;
                            Slit.BasicDescriptionRE = item.BasicDescriptionRE;
                            Slit.BasicDescriptionLE = item.BasicDescriptionLE;

                            WYNKContext.SlitLamp.AddRange(Slit);

                            ErrorLog oErrorSlit = new ErrorLog();
                            object namSlit = Slit;
                            oErrorSlit.WriteErrorLogArray("SlitLamp", namSlit);


                        }
                    }







                    if (Findings.Fundus.Count() > 0)
                    {


                        foreach (var item in Findings.Fundus.ToList())

                        {
                            var fnd = new Fundus();

                            fnd.FundusID = item.FundusID;
                            fnd.FundusLineItemID = item.FundusLineItemID;
                            fnd.FindingID = ranid;
                            fnd.IsOD = item.IsOD;
                            fnd.IsOS = item.IsOS;
                            fnd.IsOU = item.IsOU;
                            fnd.Description = item.Description;
                            fnd.IsRemoved = false;
                            fnd.CreatedBy = usid;
                            fnd.CreatedUTC = DateTime.UtcNow;
                            fnd.FundusProperty = item.FundusProperty;
                            fnd.BasicDescriptionRE = item.BasicDescriptionRE;
                            fnd.BasicDescriptionLE = item.BasicDescriptionLE;
                            WYNKContext.Fundus.AddRange(fnd);
                            ErrorLog oErrorfnd = new ErrorLog();
                            object namfnd = fnd;
                            oErrorfnd.WriteErrorLogArray("Fundus", namfnd);


                        }
                    }


                    if (

                         Findings.EyelidDisorder.UpperRE != "" || Findings.EyelidDisorder.UpperLE != "" || Findings.EyelidDisorder.LowerRE != ""
                        || Findings.EyelidDisorder.LowerLE != "" || Findings.EyelidDisorder.BothRE != "" || Findings.EyelidDisorder.BothLE != ""
                        || Findings.EyelidDisorder.PosLidRE != "" || Findings.EyelidDisorder.PosLidLE != "" || Findings.EyelidDisorder.LashesRE != ""
                        || Findings.EyelidDisorder.LashesLE != "" || Findings.EyelidDisorder.IntDstRE != null || Findings.EyelidDisorder.IntDstLE != null
                        || Findings.EyelidDisorder.LPSRE != null || Findings.EyelidDisorder.LPSLE != null || Findings.EyelidDisorder.POSRE != ""
                        || Findings.EyelidDisorder.POSLE != "")
                    {
                        var eyelid = new EyelidDisorder();

                        eyelid.FindingsID = ranid;
                        eyelid.UpperRE = Findings.EyelidDisorder.UpperRE;
                        eyelid.UpperLE = Findings.EyelidDisorder.UpperLE;
                        eyelid.LowerRE = Findings.EyelidDisorder.LowerRE;
                        eyelid.LowerLE = Findings.EyelidDisorder.LowerLE;
                        eyelid.BothRE = Findings.EyelidDisorder.BothRE;
                        eyelid.BothLE = Findings.EyelidDisorder.BothLE;
                        eyelid.PosLidRE = Findings.EyelidDisorder.PosLidRE;
                        eyelid.PosLidLE = Findings.EyelidDisorder.PosLidLE;
                        eyelid.LashesRE = Findings.EyelidDisorder.LashesRE;
                        eyelid.LashesLE = Findings.EyelidDisorder.LashesLE;
                        eyelid.IntDstRE = Findings.EyelidDisorder.IntDstRE;
                        eyelid.IntDstLE = Findings.EyelidDisorder.IntDstLE;
                        eyelid.LPSRE = Findings.EyelidDisorder.LPSRE;
                        eyelid.LPSLE = Findings.EyelidDisorder.LPSLE;
                        eyelid.POSRE = Findings.EyelidDisorder.POSRE;
                        eyelid.POSLE = Findings.EyelidDisorder.POSLE;
                        eyelid.CreatedUTC = DateTime.UtcNow;
                        eyelid.CreatedBy = usid;

                        WYNKContext.EyelidDisorder.Add(eyelid);
                        ErrorLog oErrorLogsel = new ErrorLog();
                        object nameslel = eyelid;
                        oErrorLogsslext.WriteErrorLogArray("Eyelid Disorder", nameslel);

                    }

                    if (
Findings.OrbitalDisorders.PositionofGlobeLE != null || Findings.OrbitalDisorders.PositionofGlobeLE != null || Findings.OrbitalDisorders.BaseReadingRE != ""
|| Findings.OrbitalDisorders.BaseReadingLE != "" || Findings.OrbitalDisorders.ReadingsfortheeyeRE != "" || Findings.OrbitalDisorders.ReadingsfortheeyeLE != ""
|| Findings.OrbitalDisorders.FingerInsinuationTestRE != "" || Findings.OrbitalDisorders.FingerInsinuationTestRE != "" || Findings.OrbitalDisorders.LocationRE != null
|| Findings.OrbitalDisorders.LocationLE != null || Findings.OrbitalDisorders.ShapeLE != null || Findings.OrbitalDisorders.ShapeRE != null
|| Findings.OrbitalDisorders.TextureLE != null || Findings.OrbitalDisorders.TextureRE != null || Findings.OrbitalDisorders.SizeRE != null
|| Findings.OrbitalDisorders.SizeLE != null || Findings.OrbitalDisorders.Remarks != "" || Findings.OrbitalDisorders.GradeofThyroidEyeDiseaseRE != null || Findings.OrbitalDisorders.GradeofThyroidEyeDiseaseLE != null
|| Findings.OrbitalDisorders.LocationTextRE != "" || Findings.OrbitalDisorders.LocationTextLE != "" || Findings.OrbitalDisorders.ShapeTextRE != "" || Findings.OrbitalDisorders.ShapeTextLE != ""
|| Findings.OrbitalDisorders.TextureTextRE != "" || Findings.OrbitalDisorders.TextureTextLE != "" || Findings.OrbitalDisorders.SizeTextRE != "" || Findings.OrbitalDisorders.SizeTextLE != ""
|| Findings.OrbitalDisorders.SizeTextLEVer != "" || Findings.OrbitalDisorders.SizeTextREVer != "" || Findings.OrbitalDisorders.FingerInsinuationRE != null || Findings.OrbitalDisorders.FingerInsinuationLE != null)
                    {
                        var eyelid = new OrbitalDisorders();

                        eyelid.FindingsID = ranid;
                        eyelid.PositionofGlobeLE = Findings.OrbitalDisorders.PositionofGlobeLE;
                        eyelid.PositionofGlobeRE = Findings.OrbitalDisorders.PositionofGlobeRE;
                        eyelid.BaseReadingRE = Findings.OrbitalDisorders.BaseReadingRE;
                        eyelid.BaseReadingLE = Findings.OrbitalDisorders.BaseReadingLE;
                        eyelid.ReadingsfortheeyeRE = Findings.OrbitalDisorders.ReadingsfortheeyeRE;
                        eyelid.ReadingsfortheeyeLE = Findings.OrbitalDisorders.ReadingsfortheeyeLE;
                        eyelid.FingerInsinuationTestRE = Findings.OrbitalDisorders.FingerInsinuationTestRE;
                        eyelid.FingerInsinuationTestLE = Findings.OrbitalDisorders.FingerInsinuationTestLE;
                        eyelid.LocationLE = Findings.OrbitalDisorders.LocationLE;
                        eyelid.LocationRE = Findings.OrbitalDisorders.LocationRE;
                        eyelid.ShapeLE = Findings.OrbitalDisorders.ShapeLE;
                        eyelid.ShapeRE = Findings.OrbitalDisorders.ShapeRE;
                        eyelid.TextureLE = Findings.OrbitalDisorders.TextureLE;
                        eyelid.TextureRE = Findings.OrbitalDisorders.TextureRE;
                        eyelid.SizeLE = Findings.OrbitalDisorders.SizeLE;
                        eyelid.SizeRE = Findings.OrbitalDisorders.SizeRE;
                        eyelid.Remarks = Findings.OrbitalDisorders.Remarks;
                        eyelid.GradeofThyroidEyeDiseaseRE = Findings.OrbitalDisorders.GradeofThyroidEyeDiseaseRE;
                        eyelid.GradeofThyroidEyeDiseaseLE = Findings.OrbitalDisorders.GradeofThyroidEyeDiseaseLE;
                        eyelid.LocationTextRE = Findings.OrbitalDisorders.LocationTextRE;
                        eyelid.LocationTextLE = Findings.OrbitalDisorders.LocationTextLE;
                        eyelid.ShapeTextRE = Findings.OrbitalDisorders.ShapeTextRE;
                        eyelid.ShapeTextLE = Findings.OrbitalDisorders.ShapeTextLE;
                        eyelid.TextureTextRE = Findings.OrbitalDisorders.TextureTextRE;
                        eyelid.TextureTextLE = Findings.OrbitalDisorders.TextureTextLE;
                        eyelid.SizeTextRE = Findings.OrbitalDisorders.SizeTextRE;
                        eyelid.SizeTextLE = Findings.OrbitalDisorders.SizeTextLE;
                        eyelid.SizeTextLEVer = Findings.OrbitalDisorders.SizeTextLEVer;
                        eyelid.SizeTextREVer = Findings.OrbitalDisorders.SizeTextREVer;
                        eyelid.FingerInsinuationRE = Findings.OrbitalDisorders.FingerInsinuationRE;
                        eyelid.FingerInsinuationLE = Findings.OrbitalDisorders.FingerInsinuationLE;
                        eyelid.CreatedUTC = DateTime.UtcNow;
                        eyelid.CreatedBy = usid;

                        WYNKContext.OrbitalDisorders.Add(eyelid);
                        ErrorLog oErrorLogsel = new ErrorLog();
                        object nameslel = eyelid;
                        oErrorLogsslext.WriteErrorLogArray("Eyelid Disorder", nameslel);
                    }




                    if (Findings.DIA.Count() > 0)
                    {

                        foreach (var item in Findings.DIA.ToList())
                        {

                            var dia = new Diagnosis();
                            dia.FindingsID = ranid;
                            dia.DiagnosisId = item.DiagnosisId;
                            dia.Description = item.Description;
                            dia.IsOD = item.IsOD;
                            dia.IsOS = item.IsOS;
                            dia.IsOU = item.IsOU;
                            dia.CreatedUTC = DateTime.UtcNow;
                            dia.CreatedBy = usid;
                            WYNKContext.Diagnosis.Add(dia);
                            ErrorLog oErrordia = new ErrorLog();
                            object namdia = dia;
                            oErrordia.WriteErrorLogArray("Diagnosis", namdia);
                        }


                    }

                    var stat = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Closed" && x.ParentTag == "RegStatus").Select(x => x.OLMID).FirstOrDefault();

                    var master = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == regid).ToList();
                    if (master != null && stat != null)
                    {

                        master.All(x => { x.Status = stat; return true; });
                        WYNKContext.RegistrationTran.UpdateRange(master);
                    }

                    var Admission = WYNKContext.Admission.Where(x => x.UIN == UIN && x.CMPID == cmid && x.DischargeID == null).ToList();

                    if (Findings.Finding.Consultation != null && Admission.Count != 0)
                    {

                        var respa = (from pa in WYNKContext.PatientAccount.Where(x => x.UIN == UIN && x.InvoiceNumber == null && x.CMPID == cmid)
                                     select new
                                     {
                                         ret = pa.UIN.Count(),

                                     }).ToList();

                        var PAID = WYNKContext.PatientAccount.Where(x => x.UIN == UIN && x.InvoiceNumber == null && x.CMPID == cmid).Select(x => x.PAID).LastOrDefault();
                        var ServiceTypeID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Consultation").Select(x => x.ParentID).FirstOrDefault();
                        var invespa = new PatientAccount();

                        if (respa.Count == 0)
                        {

                            invespa.CMPID = cmid;
                            invespa.UIN = UIN;
                            invespa.TotalProductValue = Findings.Finding.Consultation;
                            invespa.TotalBillValue = Findings.Finding.Consultation;
                            invespa.CreatedUTC = DateTime.UtcNow;
                            invespa.CreatedBy = usid;
                            WYNKContext.PatientAccount.AddRange(invespa);
                            ErrorLog oErrorinvespa = new ErrorLog();
                            object naminvespa = invespa;
                            oErrorinvespa.WriteErrorLogArray("PatientAccount", naminvespa);

                        }

                        else
                        {

                            var masters1 = WYNKContext.PatientAccount.Where(x => x.UIN == UIN).LastOrDefault();
                            masters1.TotalProductValue += Findings.Finding.Consultation;
                            masters1.TotalBillValue += Findings.Finding.Consultation;
                            masters1.UpdatedUTC = DateTime.UtcNow;
                            masters1.UpdatedBy = usid;
                            WYNKContext.PatientAccount.UpdateRange(masters1);
                            ErrorLog oErrormasters1 = new ErrorLog();
                            object naminvesmasters1 = masters1;
                            oErrormasters1.WriteErrorLogArray("PatientAccount", naminvesmasters1);

                        }

                        var respa1 = WYNKContext.PatientAccountDetail.Where(x => x.PAID == PAID && x.ServiceTypeID == ServiceTypeID).ToList();

                        if (respa1.Count == 0)
                        {
                            var paid = invespa.PAID;
                            var patactdt = new PatientAccountDetail();
                            patactdt.PAID = paid;
                            patactdt.OLMID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Consultation").Select(x => x.OLMID).FirstOrDefault();
                            patactdt.ServiceTypeID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Consultation").Select(x => x.ParentID).FirstOrDefault();
                            patactdt.ServiceDescription = "Consultation";
                            patactdt.TotalProductValue = Findings.Finding.Consultation;
                            patactdt.TotalBillValue = Findings.Finding.Consultation;
                            patactdt.CreatedUTC = DateTime.UtcNow;
                            patactdt.CreatedBy = usid;
                            WYNKContext.PatientAccountDetail.AddRange(patactdt);
                            ErrorLog oErrorpatactdt = new ErrorLog();
                            object naminvespatactdt = patactdt;
                            oErrorpatactdt.WriteErrorLogArray("PatientAccountDetail", naminvespatactdt);
                        }
                        else
                        {
                            var masters = WYNKContext.PatientAccountDetail.Where(x => x.PAID == PAID && x.ServiceTypeID == ServiceTypeID).LastOrDefault();
                            masters.TotalProductValue += Findings.Finding.Consultation;
                            masters.TotalBillValue += Findings.Finding.Consultation;
                            masters.UpdatedUTC = DateTime.UtcNow;
                            masters.UpdatedBy = usid;
                            WYNKContext.PatientAccountDetail.UpdateRange(masters);

                            ErrorLog oErrormst = new ErrorLog();
                            object naminvesmst = masters;
                            oErrormst.WriteErrorLogArray("PatientAccountDetail", naminvesmst);

                        }

                    }

                    if (Findings.Finding.Consultation != null)
                    {

                        var conid = (from CS in WYNKContext.ConsultantionSummary.Where(x => x.DoctorID == DOCID && x.CmpID == cmid)
                                     select new
                                     {
                                         ret = CS.DoctorID,

                                     }).ToList();

                        var pid = CMPSContext.Company.Where(x => x.CmpID == cmid).Select(x => x.ParentID).FirstOrDefault();

                        var consum = new ConsultantionSummary();
                        if (conid.Count == 0)
                        {

                            consum.DoctorID = DOCID;
                            consum.CmpID = cmid;
                            consum.ParentID = pid;
                            consum.Date = DateTime.UtcNow;
                            consum.ConsultationCharges = Findings.Finding.Consultation;
                            consum.CreatedUTC = DateTime.UtcNow;
                            consum.CreatedBy = usid;
                            WYNKContext.ConsultantionSummary.AddRange(consum);
                            ErrorLog oErrorconsum = new ErrorLog();
                            object naminveconsum = consum;
                            oErrorconsum.WriteErrorLogArray("ConsultantionSummary", naminveconsum);

                        }

                        else
                        {

                            var masterss = WYNKContext.ConsultantionSummary.Where(x => x.DoctorID == DOCID && x.CmpID == cmid).LastOrDefault();
                            masterss.Date = DateTime.UtcNow;
                            masterss.ConsultationCharges += Findings.Finding.Consultation;
                            WYNKContext.ConsultantionSummary.UpdateRange(masterss);
                            ErrorLog oErrormasterss = new ErrorLog();
                            object naminvemasterss = masterss;
                            oErrormasterss.WriteErrorLogArray("ConsultationSummary", naminvemasterss);

                        }

                        var retid = WYNKContext.RegistrationTran.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.RegistrationTranID).FirstOrDefault();
                        var re = WYNKContext.Findings.Where(x => x.RegistrationTranID == regtid).ToList();

                        var revid = (from RS in WYNKContext.RevenueSummary.Where(x => x.DoctorID == DOCID && x.CmpID == cmid && x.Date.Date == DateTime.UtcNow.Date && x.SpecialityDesc == "Consultation Fee")
                                     select new
                                     {
                                         ret = RS.DoctorID,

                                     }).ToList();
                        var revenue = new RevenueSummary();
                        if (re.Count == 1)
                        {
                            if (revid.Count == 0)
                            {

                                revenue.DoctorID = DOCID;
                                revenue.CmpID = cmid;
                                revenue.SpecialityDesc = "Consultation Fee";
                                revenue.Date = DateTime.UtcNow.Date;
                                revenue.Numbers = 1;
                                revenue.Amount = Findings.Finding.Consultation;
                                revenue.CreatedUTC = DateTime.UtcNow;
                                revenue.CreatedBy = usid;
                                WYNKContext.RevenueSummary.AddRange(revenue);
                            }

                            else
                            {
                                var masterre = WYNKContext.RevenueSummary.Where(x => x.DoctorID == DOCID && x.CmpID == cmid && x.Date.Date == DateTime.Now.Date && x.SpecialityDesc == "Consultation Fee").LastOrDefault();
                                masterre.Numbers += 1;
                                masterre.Amount += Findings.Finding.Consultation;
                                masterre.UpdatedUTC = DateTime.UtcNow;
                                masterre.UpdatedBy = usid;
                                WYNKContext.RevenueSummary.UpdateRange(masterre);
                                WYNKContext.SaveChanges();
                            }
                        }
                    }


                    if (Findings.FDDTDescriptionDetails != null)
                    {
                        if (Findings.FDDTDescriptionDetails.Count() > 0)
                        {
                            Findings.FDDTDescriptionDetails = Findings.FDDTDescriptionDetails.Where(x => x.Status == "Added").ToList();
                            foreach (var item in Findings.FDDTDescriptionDetails.ToList())
                            {
                                var SYRINGEFDDT = new SYRINGEFDDT();
                                SYRINGEFDDT.UIN = item.UIN;
                                UIN = item.UIN;
                                SYRINGEFDDT.REGTRANID = item.REGTRANID;
                                SYRINGEFDDT.CMPID = item.CMPID;
                                SYRINGEFDDT.VISITDATE = item.VISITDATE;
                                SYRINGEFDDT.FDDTSYRINGEID = item.FDDTSYRINGEID;
                                SYRINGEFDDT.FDDTDESCRIPTION = item.FDDTDESCRIPTION;
                                SYRINGEFDDT.REGURGITATION = item.REGURGITATION;
                                SYRINGEFDDT.FLUID = item.FLUID;
                                SYRINGEFDDT.DoctorID = item.DoctorID;
                                SYRINGEFDDT.REMARKS = item.REMARKS;
                                SYRINGEFDDT.Eye = item.Eye;
                                SYRINGEFDDT.IsActive = true;
                                SYRINGEFDDT.CREATEDUTC = DateTime.UtcNow;
                                SYRINGEFDDT.CREATEDBY = item.CREATEDBY;
                                WYNKContext.SYRINGEFDDT.AddRange(SYRINGEFDDT);
                            }
                        }
                    }

                    if (Findings.SYRINGINGDescriptionDetails != null)
                    {
                        if (Findings.SYRINGINGDescriptionDetails.Count() > 0)
                        {
                            Findings.SYRINGINGDescriptionDetails = Findings.SYRINGINGDescriptionDetails.Where(x => x.Status == "Added").ToList();
                            foreach (var item in Findings.SYRINGINGDescriptionDetails.ToList())
                            {
                                var SYRINGEFDDT = new SYRINGEFDDT();
                                SYRINGEFDDT.UIN = item.UIN;
                                UIN = item.UIN;
                                SYRINGEFDDT.REGTRANID = item.REGTRANID;
                                SYRINGEFDDT.CMPID = item.CMPID;
                                SYRINGEFDDT.VISITDATE = item.VISITDATE;
                                SYRINGEFDDT.FDDTSYRINGEID = item.FDDTSYRINGEID;
                                SYRINGEFDDT.FDDTDESCRIPTION = item.FDDTDESCRIPTION;
                                SYRINGEFDDT.REGURGITATION = item.REGURGITATION;
                                SYRINGEFDDT.Stop = item.Stop;
                                SYRINGEFDDT.FLUID = item.FLUID;
                                SYRINGEFDDT.DoctorID = item.DoctorID;
                                SYRINGEFDDT.Eye = item.Eye;
                                SYRINGEFDDT.REMARKS = item.REMARKS;
                                SYRINGEFDDT.IsActive = true;
                                SYRINGEFDDT.CREATEDUTC = DateTime.UtcNow;
                                SYRINGEFDDT.CREATEDBY = item.CREATEDBY;
                                WYNKContext.SYRINGEFDDT.AddRange(SYRINGEFDDT);
                            }
                        }
                    }



                    var roledescription = Findings.Role;
                    var cmpids = Convert.ToInt32(Findings.CMPID);
                    var Regtranid = Convert.ToInt32(Findings.Registrationtranid);
                    var doctorid = Convert.ToInt32(Findings.Userid);
                    var usids = Convert.ToInt32(Findings.usid);
                    var utctimes = CMPSContext.Setup.Where(x => x.CMPID == cmpids).Select(x => x.UTCTime).FirstOrDefault();

                    TimeSpan utctime = TimeSpan.Parse(utctimes);

                    var CurDate = DateTime.UtcNow;
                    var times1 = CurDate.Hour + ":" + CurDate.Minute + ":" + CurDate.Second;
                    var times2 = utctime;
                    var fulltime = TimeSpan.Parse(times1) + times2;
                    DateTime time = DateTime.Today.Add(fulltime);
                    var curfullh = time.Hour;
                    var curfullm = time.Minute;
                    string displayTimes = curfullh + ":" + curfullm;

                    if (Findings.Role != "Doctor" && Findings.Role != "Optometrist")
                    {
                        var role = WYNKContext.Workflowmaster.Where(x => x.Description == "Findings").Select(x => x.ID).FirstOrDefault();
                        var masters = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == true && x.Doctorid == 0 && x.status == true && x.Workflowmasterid != role).ToList();

                        if (masters != null && masters.Count != 0)
                        {

                            masters.All(x => { x.Checkouttime = displayTimes; x.status = false; return true; });
                            WYNKContext.Workflowtran.UpdateRange(masters);

                        }
                        var masterb = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == true && x.Doctorid == 0 && x.status == true && x.Workflowmasterid == role).ToList();
                        if (masterb != null && masterb.Count != 0)
                        {
                            masterb.All(x => { x.Checkintime = displayTimes; return true; });
                            WYNKContext.Workflowtran.UpdateRange(masterb);

                        }
                        else
                        {

                            var workflow = new Workflowtran();
                            workflow.Workflowmasterid = role;
                            workflow.Regtranid = Regtranid;
                            workflow.Reception = true;
                            workflow.Doctor = false;
                            workflow.Optometrist = false;
                            workflow.Checkintime = displayTimes;
                            workflow.Checkouttime = "";
                            workflow.CMPID = cmpids;
                            workflow.Date = DateTime.UtcNow.Date;
                            workflow.Createdutc = DateTime.UtcNow;
                            workflow.Createdby = usids;
                            workflow.status = true;
                            workflow.Doctorid = 0;
                            WYNKContext.Workflowtran.Add(workflow);
                            WYNKContext.SaveChanges();
                        }


                    }

                    else
                    {
                        var role = WYNKContext.Workflowmaster.Where(x => x.Description == "Findings").Select(x => x.ID).FirstOrDefault();
                        var masters = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == false && x.Doctorid != 0 && x.status == true && x.Workflowmasterid != role).ToList();


                        if (masters != null && masters.Count != 0)
                        {

                            masters.All(x => { x.Checkouttime = displayTimes; x.status = false; return true; });
                            WYNKContext.Workflowtran.UpdateRange(masters);

                        }
                        var masterb = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == false && x.Doctorid != 0 && x.status == true && x.Workflowmasterid == role).ToList();
                        if (masterb != null && masterb.Count != 0)
                        {
                            masterb.All(x => { x.Checkintime = displayTimes; return true; });
                            WYNKContext.Workflowtran.UpdateRange(masterb);

                        }
                        else
                        {


                            var workflow = new Workflowtran();
                            workflow.Workflowmasterid = role;
                            workflow.Regtranid = Regtranid;
                            workflow.Reception = false;
                            if (Findings.Role == "Doctor")
                            {
                                workflow.Doctor = true;
                            }
                            if (Findings.Role == "Optometrist")
                            {
                                workflow.Optometrist = true;
                            }
                            workflow.Checkintime = displayTimes;
                            workflow.Checkouttime = "";
                            workflow.CMPID = cmpids;
                            workflow.Date = DateTime.UtcNow.Date;
                            workflow.Createdutc = DateTime.UtcNow;
                            workflow.Createdby = usids;
                            workflow.status = true;
                            workflow.Doctorid = doctorid;
                            WYNKContext.Workflowtran.Add(workflow);
                            WYNKContext.SaveChanges();
                        }


                    }


                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

                    if (WYNKContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }
                    return new
                    {

                        Uin = Findings.Finding.UIN,
                        Success = true,
                        Message = CommonMessage.saved,
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
                Message = CommonMessage.Missing,
            };

        }

        public dynamic UpdateDiagnosis(Findings Findings)
        {



            var olm = new SpecialityMaster();


            if (Findings.SpecialityMaster.ParentDescription != null)
            {


                var c = WYNKContext.SpecialityMaster.Where(x => x.IsActive == true && x.IsDeleted == false && x.CmpID == Findings.SpecialityMaster.CmpID && x.ParentTag == "DIA" && x.ParentDescription.Equals(Convert.ToString(Findings.SpecialityMaster.ParentDescription), StringComparison.OrdinalIgnoreCase)).Select(y => y.ID).FirstOrDefault();

                if (c != 0)
                {
                    return new
                    {
                        Success = false,
                        Message = "Description already Exists"
                    };

                }

            }

            if (Findings.SpecialityMaster.Code != null && Findings.SpecialityMaster.Code != "")
            {

                var d = WYNKContext.SpecialityMaster.Where(x => x.Code == Findings.SpecialityMaster.Code && x.IsActive == true && x.IsDeleted == false && x.CmpID == Findings.SpecialityMaster.CmpID && x.ParentTag == "DIA" && x.Code.Equals(Convert.ToString(Findings.SpecialityMaster.Code), StringComparison.OrdinalIgnoreCase)).Select(z => z.ID).FirstOrDefault();
                if (d != 0)
                {
                    return new
                    {
                        Success = false,
                        Message = "Code already Exists"
                    };

                }

            }



            olm.ParentDescription = Findings.SpecialityMaster.ParentDescription;
            olm.ParentID = 0;
            olm.ParentTag = "DIA";
            olm.IsActive = true;
            olm.IsDeleted = false;
            olm.CreatedUTC = DateTime.UtcNow;
            olm.CreatedBy = Findings.SpecialityMaster.CreatedBy;
            olm.Code = Findings.SpecialityMaster.Code;
            olm.CmpID = Findings.SpecialityMaster.CmpID;

            WYNKContext.SpecialityMaster.Add(olm);


            try
            {
                if (WYNKContext.SaveChanges() >= 0)
                    return new
                    {

                        Uin = Findings.Finding.UIN,
                        Success = true,
                        Message = CommonMessage.saved,
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = CommonMessage.Missing,
            };



        }







        public bool uploadFile(IFormFile files, string uin)
        {
            try
            {
                var currentDir = Directory.GetCurrentDirectory();

                var dt = Convert.ToDateTime(DateTime.Now.Date).ToString("dd-MM-yyyy");

                var res = Directory.CreateDirectory(currentDir + '/' + "/FndCaseSheet/" + '/' + dt);
                var fileName = $"{uin}{Path.GetExtension(files.FileName)}";
                var path = $"{currentDir}/FndCaseSheet/{dt}/{fileName}";

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    files.CopyTo(stream);
                    var fpath = WYNKContext.Findings.Where(x => x.UIN == uin).LastOrDefault();
                    fpath.FilePath = path;
                    WYNKContext.Entry(fpath).State = EntityState.Modified;
                    return WYNKContext.SaveChanges() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }





        }



        public bool UploadImage(IFormFile file, string uin)
        {
            try
            {
                var currentDir = Directory.GetCurrentDirectory();
                if (!Directory.Exists(currentDir + "/ODSlitImages/"))
                    Directory.CreateDirectory(currentDir + "/ODSlitImages/");
                var fileName = $"{uin}{Path.GetExtension(file.FileName)}";
                var path = $"{currentDir}/ODSlitImages/{fileName}";
                var pathf = $"{currentDir}";
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                    var slitod = WYNKContext.Findings.Where(x => x.UIN == uin).LastOrDefault();
                    slitod.SlitLampODImagePath = path;
                    WYNKContext.Entry(slitod).State = EntityState.Modified;
                    return WYNKContext.SaveChanges() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }





        }
        public bool UploadImage1(IFormFile file1, string uin)
        {
            try
            {
                var currentDir = Directory.GetCurrentDirectory();
                if (!Directory.Exists(currentDir + "/OSSlitImages/"))
                    Directory.CreateDirectory(currentDir + "/OSSlitImages/");
                var fileName1 = $"{uin}{Path.GetExtension(file1.FileName)}";
                var path1 = $"{currentDir}/OSSlitImages/{fileName1}";
                var pathf1 = $"{currentDir}";

                using (var stream1 = new FileStream(path1, FileMode.Create))
                {
                    file1.CopyTo(stream1);
                    var slitos = WYNKContext.Findings.Where(x => x.UIN == uin).LastOrDefault();
                    slitos.SlitLampOSImagePath = path1;
                    WYNKContext.Entry(slitos).State = EntityState.Modified;
                    return WYNKContext.SaveChanges() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UploadImage2(IFormFile file2, string uin)
        {
            try
            {
                var currentDir = Directory.GetCurrentDirectory();
                if (!Directory.Exists(currentDir + "/ODFundusImages/"))
                    Directory.CreateDirectory(currentDir + "/ODFundusImages/");
                var fileName2 = $"{uin}{Path.GetExtension(file2.FileName)}";
                var path2 = $"{currentDir}/ODFundusImages/{fileName2}";
                var pathf2 = $"{currentDir}";
                using (var stream1 = new FileStream(path2, FileMode.Create))
                {
                    file2.CopyTo(stream1);
                    var fndod = WYNKContext.Findings.Where(x => x.UIN == uin).LastOrDefault();
                    fndod.FundusODImagePath = path2;
                    WYNKContext.Entry(fndod).State = EntityState.Modified;
                    return WYNKContext.SaveChanges() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UploadImage3(IFormFile file3, string uin)
        {
            try
            {
                var currentDir = Directory.GetCurrentDirectory();
                if (!Directory.Exists(currentDir + "/OSFundusImages/"))
                    Directory.CreateDirectory(currentDir + "/OSFundusImages/");
                var fileName3 = $"{uin}{Path.GetExtension(file3.FileName)}";
                var path3 = $"{currentDir}/OSFundusImages/{fileName3}";
                var pathf3 = $"{currentDir}";
                using (var stream1 = new FileStream(path3, FileMode.Create))
                {
                    file3.CopyTo(stream1);
                    var fndos = WYNKContext.Findings.Where(x => x.UIN == uin).LastOrDefault();
                    fndos.FundusOSImagePath = path3;
                    WYNKContext.Entry(fndos).State = EntityState.Modified;
                    return WYNKContext.SaveChanges() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Findings GetSlitnewDetails(string UIN)

        {

            var findings1 = new Findings();


            var regg = WYNKContext.Findings.Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).LastOrDefault();

            var regg1 = WYNKContext.Findings.Where(x => x.RegistrationTranID == regg).OrderByDescending(x => x.CreatedUTC).Select(x => x.RandomUniqueID).FirstOrDefault();


            findings1.Fidt = WYNKContext.Findings.Where(x => x.RandomUniqueID == regg1).Select(x => x.CreatedUTC.Date).LastOrDefault();

            var slitlamp = WYNKContext.SlitLamp.ToList();
            var onelinemasterss = CMPSContext.OneLineMaster.ToList();
            var findingsss = WYNKContext.Findings.ToList();


            findings1.Slitlatest1 = (from REF in findingsss.Where(x => x.RandomUniqueID == regg1)
                                     join SLT in slitlamp
                                     on REF.RandomUniqueID equals SLT.FindingID
                                     join OLM in onelinemasterss
                                     on SLT.SlitLampLineItemID equals OLM.OLMID



                                     select new Slitlatest1
                                     {
                                         SlitDescLat1 = OLM.ParentTag,
                                         DescLat1 = OLM.ParentDescription,
                                         AdDescLat1 = SLT.Description,
                                         CodeLat1 = SLT.SlitLampLineItemID,
                                         OdLat1 = SLT.IsOD,
                                         OsLat1 = SLT.IsOS,
                                         OuLat1 = SLT.IsOU,

                                     }).ToList();



            return findings1;

        }


        public Findings GetFundnewDetails(string UIN)

        {

            var findingsf11 = new Findings();


            var reg = WYNKContext.Findings.Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).LastOrDefault();

            var reg1 = WYNKContext.Findings.Where(x => x.RegistrationTranID == reg).OrderByDescending(x => x.CreatedUTC).Select(x => x.RandomUniqueID).FirstOrDefault();

            var regg = WYNKContext.Refraction.Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).LastOrDefault();

            var regg1 = WYNKContext.Refraction.Where(x => x.RegistrationTranID == regg).Select(x => x.ID).LastOrDefault();
            findingsf11.Fidtt = WYNKContext.Findings.Where(x => x.RandomUniqueID == reg1).Select(x => x.CreatedUTC.Date).LastOrDefault();

            var findingsss = WYNKContext.Findings.ToList();
            var onelinemasterss = CMPSContext.OneLineMaster.ToList();
            var funduss = WYNKContext.Fundus.ToList();


            findingsf11.Funlatest1 = (from FND in findingsss.Where(x => x.RandomUniqueID == reg1)
                                      join FUN in funduss
                                      on FND.RandomUniqueID equals FUN.FindingID
                                      join OLM in onelinemasterss
                                      on FUN.FundusLineItemID equals OLM.OLMID

                                      select new Funlatest1
                                      {
                                          FundDescLat1 = OLM.ParentTag,

                                          DescFun1 = OLM.ParentDescription,
                                          AdDescFun1 = FUN.Description,
                                          CodeFun1 = FUN.FundusLineItemID,
                                          OdFun1 = FUN.IsOD,
                                          OsFun1 = FUN.IsOS,
                                          OuFun1 = FUN.IsOU,

                                      }).ToList();

            return findingsf11;

        }

        public Findings GetOneeyedDetails(string UIN, int cmpid)

        {

            var fnds = new Findings();


            fnds.OcularPro = (from RE in WYNKContext.RegistrationExtension.Where(x => x.UIN == UIN && x.CMPID == cmpid)


                              select new OcularPro
                              {
                                  AE = RE.Artificialeye,
                                  OD = RE.OD,
                                  OS = RE.OS,
                                  OU = RE.OU,

                              }).ToList();


            return fnds;

        }



        public dynamic Remove(Findings Findings, int ID)
        {
            var olm = new OneLine_Masters();

            var olmm = CMPSContext.OneLineMaster.Where(x => x.OLMID == ID).ToList();
            if (olmm != null)
            {

                olmm.All(x => { x.IsDeleted = true; return true; });
                CMPSContext.OneLineMaster.UpdateRange(olmm);
            }


            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                    return new
                    {

                        Success = true,
                        Message = "Deleted successfully"
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = CommonMessage.Missing,
            };



        }


        public dynamic Getpatientimage(string UIN)
        {

            var reg = new Findings();


            var regs = WYNKContext.Findings.Where(x => x.UIN == UIN).Select(x => x.UIN).LastOrDefault();

            if (regs != null)
            {
                var osfn = regs + ".png";
                var osfi = "/OSFundusImages/";
                var currentDir = Directory.GetCurrentDirectory();
                string path = currentDir + osfi + osfn;
                if ((File.Exists(path)))
                {
                    string imageData = Convert.ToBase64String(File.ReadAllBytes(path));

                    string source = imageData;
                    string base64 = source.Substring(source.IndexOf(',') + 1);
                    byte[] data = Convert.FromBase64String(base64);


                    reg.ProductImage = imageData;
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

        public dynamic Getpatientimagefnod(string UIN)
        {

            var reg = new Findings();


            var regs = WYNKContext.Findings.Where(x => x.UIN == UIN).Select(x => x.UIN).FirstOrDefault();

            if (regs != null)
            {
                var odfn = regs + ".png";

                var odfi = "/ODFundusImages/";
                var currentDir = Directory.GetCurrentDirectory();
                string path = currentDir + odfi + odfn;
                if ((File.Exists(path)))
                {
                    string imageData = Convert.ToBase64String(File.ReadAllBytes(path));

                    string source = imageData;
                    string base64 = source.Substring(source.IndexOf(',') + 1);
                    byte[] data = Convert.FromBase64String(base64);


                    reg.ProductImagefod = imageData;
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


        public dynamic Getpatientimageslod(string UIN)
        {

            var reg = new Findings();


            var regs = WYNKContext.Findings.Where(x => x.UIN == UIN).Select(x => x.UIN).FirstOrDefault();

            if (regs != null)
            {

                var ossl = regs + ".png";
                var odsi = "/ODSlitImages/";
                var currentDir = Directory.GetCurrentDirectory();
                string path = currentDir + odsi + ossl;

                if ((File.Exists(path)))
                {
                    string imageData = Convert.ToBase64String(File.ReadAllBytes(path));

                    string source = imageData;
                    string base64 = source.Substring(source.IndexOf(',') + 1);
                    byte[] data = Convert.FromBase64String(base64);


                    reg.ProductImagesod = imageData;
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


        public dynamic Getpatientimageslos(string UIN)
        {

            var reg = new Findings();


            var regs = WYNKContext.Findings.Where(x => x.UIN == UIN).Select(x => x.UIN).FirstOrDefault();

            if (regs != null)
            {
                var ossll = regs + ".png";
                var ossi = "/OSSlitImages/";
                var currentDir = Directory.GetCurrentDirectory();
                string path = currentDir + ossi + ossll;
                if ((File.Exists(path)))
                {
                    string imageData = Convert.ToBase64String(File.ReadAllBytes(path));

                    string source = imageData;
                    string base64 = source.Substring(source.IndexOf(',') + 1);
                    byte[] data = Convert.FromBase64String(base64);


                    reg.ProductImagesos = imageData;
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




        public dynamic Getpatientfile(string UIN)
        {

            var reg = new Findings();


            var regs = WYNKContext.Findings.Where(x => x.UIN == UIN).Select(x => x.UIN).LastOrDefault();

            if (regs != null)
            {

                var currentDirectory = System.IO.Directory.GetCurrentDirectory();
                string paths = WYNKContext.Findings.Where(x => x.UIN == UIN).Select(x => x.FilePath).LastOrDefault();
                if ((File.Exists(paths)))
                {
                    var file = Path.Combine(Path.Combine(paths));
                    return new FileStream(file, FileMode.Open, FileAccess.Read);
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



        public bool uploadImagsqd(IFormFile file1, string desc, string uin, string id)
        {
            var fnd = new Findings();
            fnd.SQI = new List<SquintImage>();
            try
            {
                var currentDir = Directory.GetCurrentDirectory();
                var dt = Convert.ToDateTime(DateTime.Now.Date).ToString("dd-MM-yyyy");

                var res = Directory.CreateDirectory(currentDir + '/' + uin + '/' + dt);
                var fileName1 = $"{desc}{Path.GetExtension(file1.FileName)}";
                var path1 = $"{currentDir}/{uin}/{dt}/{fileName1}";

                var pathh = $"{currentDir}/{uin}/{dt}";

                using (var stream1 = new FileStream(path1, FileMode.Create))
                {
                    file1.CopyTo(stream1);
                    var fid = WYNKContext.Findings.Where(x => x.UIN == id).Select(x => x.RandomUniqueID).LastOrDefault();
                    var opbio = WYNKContext.SquintImage.Where(x => x.SquintID == fid && x.Eye == "OD").ToList();
                    if (opbio.Count() > 0)
                    {
                        foreach (var item1 in opbio.ToList())
                        {
                            item1.SquintODImagePath = pathh;
                            WYNKContext.Entry(item1).State = EntityState.Modified;
                            WYNKContext.SaveChanges();
                        }

                    }

                    return WYNKContext.SaveChanges() > 0;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }






        public bool uploadImagslod(IFormFile file1, string desc, string uin, string id)
        {
            var fnd = new Findings();
            fnd.SlitLampImage = new List<SlitLampImage>();
            try
            {
                var slodid = WYNKContext.SlitLampImage.Where(x => x.UIN == id && x.Eye == "OD").Select(x => x.ID).LastOrDefault();


                var currentDir = Directory.GetCurrentDirectory();
                var dt = Convert.ToDateTime(DateTime.Now.Date).ToString("dd-MM-yyyy");

                var descsslod = desc + slodid;

                var res = Directory.CreateDirectory(currentDir + '/' + uin + '/' + dt);
                var fileName1 = $"{descsslod}{Path.GetExtension(file1.FileName)}";
                var path1 = $"{currentDir}/{uin}/{dt}/{fileName1}";

                var pathh = $"{currentDir}/{uin}/{dt}";

                using (var stream1 = new FileStream(path1, FileMode.Create))
                {
                    file1.CopyTo(stream1);
                    var fid = WYNKContext.Findings.Where(x => x.UIN == id).Select(x => x.RandomUniqueID).LastOrDefault();
                    var opbio = WYNKContext.SlitLampImage.Where(x => x.FindingsID == fid && x.Eye == "OD" && x.CreatedUTC.Date == DateTime.Now.Date && x.SlitLampODImagePath == null).ToList();
                    if (opbio.Count() > 0)
                    {
                        foreach (var item1 in opbio.ToList())
                        {
                            item1.SlitLampODImagePath = pathh;
                            WYNKContext.Entry(item1).State = EntityState.Modified;
                            WYNKContext.SaveChanges();
                        }

                    }

                    return WYNKContext.SaveChanges() > 0;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool uploadImagslos(IFormFile file1, string desc, string uin, string id)
        {
            var fnd = new Findings();
            fnd.SlitLampImage = new List<SlitLampImage>();
            try
            {
                var slosid = WYNKContext.SlitLampImage.Where(x => x.UIN == id && x.Eye == "OS").Select(x => x.ID).LastOrDefault();

                var currentDir = Directory.GetCurrentDirectory();
                var dt = Convert.ToDateTime(DateTime.Now.Date).ToString("dd-MM-yyyy");

                var descsslos = desc + slosid;

                var res = Directory.CreateDirectory(currentDir + '/' + uin + '/' + dt);
                var fileName1 = $"{descsslos}{Path.GetExtension(file1.FileName)}";
                var path1 = $"{currentDir}/{uin}/{dt}/{fileName1}";

                var pathh = $"{currentDir}/{uin}/{dt}";

                using (var stream1 = new FileStream(path1, FileMode.Create))
                {
                    file1.CopyTo(stream1);
                    var fid = WYNKContext.Findings.Where(x => x.UIN == id).Select(x => x.RandomUniqueID).LastOrDefault();
                    var opbio = WYNKContext.SlitLampImage.Where(x => x.FindingsID == fid && x.Eye == "OS" && x.CreatedUTC.Date == DateTime.Now.Date && x.SlitLampOSImagePath == null).ToList();
                    if (opbio.Count() > 0)
                    {
                        foreach (var item1 in opbio.ToList())
                        {
                            item1.SlitLampOSImagePath = pathh;
                            WYNKContext.Entry(item1).State = EntityState.Modified;
                            WYNKContext.SaveChanges();
                        }

                    }

                    return WYNKContext.SaveChanges() > 0;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool uploadImagelod(IFormFile file1, string desc, string uin, string id)
        {
            var fnd = new Findings();
            fnd.EyelidImage = new List<EyelidImage>();
            try
            {
                var slodid = WYNKContext.EyelidImage.Where(x => x.UIN == id && x.Eye == "OD").Select(x => x.ID).LastOrDefault();


                var currentDir = Directory.GetCurrentDirectory();
                var dt = Convert.ToDateTime(DateTime.Now.Date).ToString("dd-MM-yyyy");

                var descsslod = desc + slodid;

                var res = Directory.CreateDirectory(currentDir + '/' + uin + '/' + dt);
                var fileName1 = $"{descsslod}{Path.GetExtension(file1.FileName)}";
                var path1 = $"{currentDir}/{uin}/{dt}/{fileName1}";

                var pathh = $"{currentDir}/{uin}/{dt}";

                using (var stream1 = new FileStream(path1, FileMode.Create))
                {
                    file1.CopyTo(stream1);
                    var fid = WYNKContext.Findings.Where(x => x.UIN == id).Select(x => x.RandomUniqueID).LastOrDefault();
                    var opbio = WYNKContext.EyelidImage.Where(x => x.FindingsID == fid && x.Eye == "OD" && x.CreatedUTC.Date == DateTime.Now.Date && x.ODImagePath == null).ToList();
                    if (opbio.Count() > 0)
                    {
                        foreach (var item1 in opbio.ToList())
                        {
                            item1.ODImagePath = pathh;
                            WYNKContext.Entry(item1).State = EntityState.Modified;
                            WYNKContext.SaveChanges();
                        }

                    }

                    return WYNKContext.SaveChanges() > 0;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool uploadImagelos(IFormFile file1, string desc, string uin, string id)
        {
            var fnd = new Findings();
            fnd.EyelidImage = new List<EyelidImage>();
            try
            {
                var slosid = WYNKContext.EyelidImage.Where(x => x.UIN == id && x.Eye == "OS").Select(x => x.ID).LastOrDefault();

                var currentDir = Directory.GetCurrentDirectory();
                var dt = Convert.ToDateTime(DateTime.Now.Date).ToString("dd-MM-yyyy");

                var descsslos = desc + slosid;

                var res = Directory.CreateDirectory(currentDir + '/' + uin + '/' + dt);
                var fileName1 = $"{descsslos}{Path.GetExtension(file1.FileName)}";
                var path1 = $"{currentDir}/{uin}/{dt}/{fileName1}";

                var pathh = $"{currentDir}/{uin}/{dt}";

                using (var stream1 = new FileStream(path1, FileMode.Create))
                {
                    file1.CopyTo(stream1);
                    var fid = WYNKContext.Findings.Where(x => x.UIN == id).Select(x => x.RandomUniqueID).LastOrDefault();
                    var opbio = WYNKContext.EyelidImage.Where(x => x.FindingsID == fid && x.Eye == "OS" && x.CreatedUTC.Date == DateTime.Now.Date && x.OSImagePath == null).ToList();
                    if (opbio.Count() > 0)
                    {
                        foreach (var item1 in opbio.ToList())
                        {
                            item1.OSImagePath = pathh;
                            WYNKContext.Entry(item1).State = EntityState.Modified;
                            WYNKContext.SaveChanges();
                        }

                    }

                    return WYNKContext.SaveChanges() > 0;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool uploadImagfnod(IFormFile file1, string desc, string uin, string id)
        {
            var fnd = new Findings();
            fnd.FundusImage = new List<FundusImage>();
            try
            {
                var fnodid = WYNKContext.FundusImage.Where(x => x.UIN == id && x.Eye == "OD").Select(x => x.ID).LastOrDefault();

                var currentDir = Directory.GetCurrentDirectory();
                var dt = Convert.ToDateTime(DateTime.Now.Date).ToString("dd-MM-yyyy");

                var descsfnod = desc + fnodid;

                var res = Directory.CreateDirectory(currentDir + '/' + uin + '/' + dt);
                var fileName1 = $"{descsfnod}{Path.GetExtension(file1.FileName)}";
                var path1 = $"{currentDir}/{uin}/{dt}/{fileName1}";

                var pathh = $"{currentDir}/{uin}/{dt}";

                using (var stream1 = new FileStream(path1, FileMode.Create))
                {
                    file1.CopyTo(stream1);
                    var fid = WYNKContext.Findings.Where(x => x.UIN == id).Select(x => x.RandomUniqueID).LastOrDefault();
                    var opbio = WYNKContext.FundusImage.Where(x => x.FindingsID == fid && x.Eye == "OD" && x.CreatedUTC.Date == DateTime.Now.Date && x.FundusODImagePath == null).ToList();
                    if (opbio.Count() > 0)
                    {
                        foreach (var item1 in opbio.ToList())
                        {
                            item1.FundusODImagePath = pathh;
                            WYNKContext.Entry(item1).State = EntityState.Modified;
                            WYNKContext.SaveChanges();
                        }

                    }

                    return WYNKContext.SaveChanges() > 0;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }



        public bool uploadImagfnos(IFormFile file1, string desc, string uin, string id)
        {
            var fnd = new Findings();
            fnd.FundusImage = new List<FundusImage>();
            try
            {
                var fnosid = WYNKContext.FundusImage.Where(x => x.UIN == id && x.Eye == "OS").Select(x => x.ID).LastOrDefault();

                var currentDir = Directory.GetCurrentDirectory();
                var dt = Convert.ToDateTime(DateTime.Now.Date).ToString("dd-MM-yyyy");
                var descsfnos = desc + fnosid;

                var res = Directory.CreateDirectory(currentDir + '/' + uin + '/' + dt);
                var fileName1 = $"{descsfnos}{Path.GetExtension(file1.FileName)}";
                var path1 = $"{currentDir}/{uin}/{dt}/{fileName1}";

                var pathh = $"{currentDir}/{uin}/{dt}";

                using (var stream1 = new FileStream(path1, FileMode.Create))
                {
                    file1.CopyTo(stream1);
                    var fid = WYNKContext.Findings.Where(x => x.UIN == id).Select(x => x.RandomUniqueID).LastOrDefault();
                    var opbio = WYNKContext.FundusImage.Where(x => x.FindingsID == fid && x.Eye == "OS" && x.CreatedUTC.Date == DateTime.Now.Date && x.FundusOSImagePath == null).ToList();
                    if (opbio.Count() > 0)
                    {
                        foreach (var item1 in opbio.ToList())
                        {
                            item1.FundusOSImagePath = pathh;
                            WYNKContext.Entry(item1).State = EntityState.Modified;
                            WYNKContext.SaveChanges();
                        }

                    }

                    return WYNKContext.SaveChanges() > 0;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }




        public bool uploadImagvfod(IFormFile file1, string desc, string uin, string id)
        {
            var fnd = new Findings();
            fnd.VisualField = new List<VisualField>();
            try
            {
                var vfodid = WYNKContext.VisualField.Where(x => x.UIN == id && x.Eye == "OD").Select(x => x.ID).LastOrDefault();


                var currentDir = Directory.GetCurrentDirectory();
                var dt = Convert.ToDateTime(DateTime.Now.Date).ToString("dd-MM-yyyy");

                var descsvfod = desc + vfodid;

                var res = Directory.CreateDirectory(currentDir + '/' + uin + '/' + dt);
                var fileName1 = $"{descsvfod}{Path.GetExtension(file1.FileName)}";
                var path1 = $"{currentDir}/{uin}/{dt}/{fileName1}";

                var pathh = $"{currentDir}/{uin}/{dt}";

                using (var stream1 = new FileStream(path1, FileMode.Create))
                {
                    file1.CopyTo(stream1);
                    var fid = WYNKContext.Findings.Where(x => x.UIN == id).Select(x => x.RandomUniqueID).LastOrDefault();
                    var opbio = WYNKContext.VisualField.Where(x => x.FindingsID == fid && x.Eye == "OD" && x.CreatedUTC.Date == DateTime.Now.Date && x.VFODImagePath == null).ToList();
                    if (opbio.Count() > 0)
                    {
                        foreach (var item1 in opbio.ToList())
                        {
                            item1.VFODImagePath = pathh;
                            WYNKContext.Entry(item1).State = EntityState.Modified;
                            WYNKContext.SaveChanges();
                        }

                    }

                    return WYNKContext.SaveChanges() > 0;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool uploadImagvfos(IFormFile file1, string desc, string uin, string id)
        {
            var fnd = new Findings();
            fnd.VisualField = new List<VisualField>();
            try
            {
                var vfosid = WYNKContext.VisualField.Where(x => x.UIN == id && x.Eye == "OS").Select(x => x.ID).LastOrDefault();
                var currentDir = Directory.GetCurrentDirectory();
                var dt = Convert.ToDateTime(DateTime.Now.Date).ToString("dd-MM-yyyy");
                var descsvfos = desc + vfosid;

                var res = Directory.CreateDirectory(currentDir + '/' + uin + '/' + dt);
                var fileName1 = $"{descsvfos}{Path.GetExtension(file1.FileName)}";
                var path1 = $"{currentDir}/{uin}/{dt}/{fileName1}";

                var pathh = $"{currentDir}/{uin}/{dt}";

                using (var stream1 = new FileStream(path1, FileMode.Create))
                {
                    file1.CopyTo(stream1);
                    var fid = WYNKContext.Findings.Where(x => x.UIN == id).Select(x => x.RandomUniqueID).LastOrDefault();
                    var opbio = WYNKContext.VisualField.Where(x => x.FindingsID == fid && x.Eye == "OS" && x.CreatedUTC.Date == DateTime.Now.Date && x.VFOSImagePath == null).ToList();
                    if (opbio.Count() > 0)
                    {
                        foreach (var item1 in opbio.ToList())
                        {
                            item1.VFOSImagePath = pathh;
                            WYNKContext.Entry(item1).State = EntityState.Modified;
                            WYNKContext.SaveChanges();
                        }

                    }

                    return WYNKContext.SaveChanges() > 0;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }






        public bool uploadImagglod(IFormFile file1, string desc, string uin, string id)
        {
            var fnd = new Findings();
            fnd.GlaucomaImage = new List<GlaucomaImage>();
            try
            {
                var currentDir = Directory.GetCurrentDirectory();
                var dt = Convert.ToDateTime(DateTime.Now.Date).ToString("dd-MM-yyyy");

                var res = Directory.CreateDirectory(currentDir + '/' + uin + '/' + dt);
                var fileName1 = $"{desc}{Path.GetExtension(file1.FileName)}";
                var path1 = $"{currentDir}/{uin}/{dt}/{fileName1}";

                var pathh = $"{currentDir}/{uin}/{dt}";

                using (var stream1 = new FileStream(path1, FileMode.Create))
                {
                    file1.CopyTo(stream1);
                    var fid = WYNKContext.Findings.Where(x => x.UIN == id).Select(x => x.RandomUniqueID).LastOrDefault();
                    var opbio = WYNKContext.GlaucomaImage.Where(x => x.FindingsID == fid && x.Eye == "OD").ToList();
                    if (opbio.Count() > 0)
                    {
                        foreach (var item1 in opbio.ToList())
                        {
                            item1.GlaucomaODImagePath = pathh;
                            WYNKContext.Entry(item1).State = EntityState.Modified;
                            WYNKContext.SaveChanges();
                        }

                    }

                    return WYNKContext.SaveChanges() > 0;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool uploadImagglos(IFormFile file1, string desc, string uin, string id)
        {
            var fnd = new Findings();
            fnd.GlaucomaImage = new List<GlaucomaImage>();
            try
            {
                var currentDir = Directory.GetCurrentDirectory();
                var dt = Convert.ToDateTime(DateTime.Now.Date).ToString("dd-MM-yyyy");

                var res = Directory.CreateDirectory(currentDir + '/' + uin + '/' + dt);
                var fileName1 = $"{desc}{Path.GetExtension(file1.FileName)}";
                var path1 = $"{currentDir}/{uin}/{dt}/{fileName1}";

                var pathh = $"{currentDir}/{uin}/{dt}";

                using (var stream1 = new FileStream(path1, FileMode.Create))
                {
                    file1.CopyTo(stream1);
                    var fid = WYNKContext.Findings.Where(x => x.UIN == id).Select(x => x.RandomUniqueID).LastOrDefault();
                    var opbio = WYNKContext.GlaucomaImage.Where(x => x.FindingsID == fid && x.Eye == "OS").ToList();
                    if (opbio.Count() > 0)
                    {
                        foreach (var item1 in opbio.ToList())
                        {
                            item1.GlaucomaOSImagePath = pathh;
                            WYNKContext.Entry(item1).State = EntityState.Modified;
                            WYNKContext.SaveChanges();
                        }

                    }

                    return WYNKContext.SaveChanges() > 0;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }







        public bool uploadImagsqs(IFormFile file1, string desc, string uin, string id)
        {
            var fnd = new Findings();
            fnd.SQI = new List<SquintImage>();
            try
            {
                var currentDir = Directory.GetCurrentDirectory();
                var dt = Convert.ToDateTime(DateTime.Now.Date).ToString("dd-MM-yyyy");

                var res = Directory.CreateDirectory(currentDir + '/' + uin + '/' + dt);
                var fileName1 = $"{desc}{Path.GetExtension(file1.FileName)}";
                var path1 = $"{currentDir}/{uin}/{dt}/{fileName1}";

                var pathh = $"{currentDir}/{uin}/{dt}";

                using (var stream1 = new FileStream(path1, FileMode.Create))
                {
                    file1.CopyTo(stream1);
                    var fid = WYNKContext.Findings.Where(x => x.UIN == id).Select(x => x.RandomUniqueID).LastOrDefault();
                    var opbio = WYNKContext.SquintImage.Where(x => x.SquintID == fid && x.Eye == "OS").ToList();
                    if (opbio.Count() > 0)
                    {
                        foreach (var item1 in opbio.ToList())
                        {
                            item1.SquintOSImagePath = pathh;
                            WYNKContext.Entry(item1).State = EntityState.Modified;
                            WYNKContext.SaveChanges();
                        }

                    }

                    return WYNKContext.SaveChanges() > 0;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }


        public dynamic GetFDDtSyrDetails(string uin, string GMT, int Cmpid)
        {

            try
            {
                var Details = new PatientHistoryViewModel();
                Details.FDDTDescriptionDetails = new List<FDDTDescriptionDetail>();
                Details.SYRINGINGDescriptionDetails = new List<FDDTDescriptionDetail>();

                var DocMaster = CMPSContext.DoctorMaster.Where(x => x.CMPID == Cmpid).ToList();

                var FDDtIDs = WYNKContext.SpecialityMaster.Where(x => x.ParentTag == "FDDT").Select(x => x.ID).ToList();
                var SyrIDs = WYNKContext.SpecialityMaster.Where(x => x.ParentTag == "SYRINGING").Select(x => x.ID).ToList();


                Details.FDDTDescriptionDetails = (from SYRINGEFDDT in WYNKContext.SYRINGEFDDT.Where(x => x.UIN == uin && x.IsActive == true)
                                                  where FDDtIDs.Contains(SYRINGEFDDT.FDDTSYRINGEID)
                                                  select new FDDTDescriptionDetail
                                                  {
                                                      ID = SYRINGEFDDT.ID,
                                                      UIN = SYRINGEFDDT.UIN,
                                                      VISITDATE = SYRINGEFDDT.VISITDATE.Add(TimeSpan.Parse(GMT)),
                                                      FDDTSYRINGEID = SYRINGEFDDT.FDDTSYRINGEID,
                                                      FDDTDESCRIPTION = SYRINGEFDDT.FDDTDESCRIPTION,
                                                      Eye = SYRINGEFDDT.Eye,
                                                      Doctor = DocMaster.Where(x => x.DoctorID == SYRINGEFDDT.DoctorID).Select(x => String.Concat(x.Firstname + " " + (x.MiddleName != null ? x.MiddleName : " ") + " " + x.LastName)).FirstOrDefault(),
                                                      REMARKS = SYRINGEFDDT.REMARKS,
                                                      REGTRANID = SYRINGEFDDT.REGTRANID,
                                                      CMPID = SYRINGEFDDT.CMPID
                                                  }).OrderByDescending(x => x.VISITDATE).ToList();

                Details.SYRINGINGDescriptionDetails = (from SYRINGEFDDT in WYNKContext.SYRINGEFDDT.Where(x => x.UIN == uin && x.IsActive == true)
                                                       where SyrIDs.Contains(SYRINGEFDDT.FDDTSYRINGEID)
                                                       select new FDDTDescriptionDetail
                                                       {
                                                           ID = SYRINGEFDDT.ID,
                                                           UIN = SYRINGEFDDT.UIN,
                                                           VISITDATE = SYRINGEFDDT.VISITDATE.Add(TimeSpan.Parse(GMT)),
                                                           FDDTSYRINGEID = SYRINGEFDDT.FDDTSYRINGEID,
                                                           FDDTDESCRIPTION = SYRINGEFDDT.FDDTDESCRIPTION,
                                                           REGURGITATION = SYRINGEFDDT.REGURGITATION,
                                                           FLUID = SYRINGEFDDT.FLUID,
                                                           Stop = SYRINGEFDDT.Stop,
                                                           Eye = SYRINGEFDDT.Eye,
                                                           Doctor = DocMaster.Where(x => x.DoctorID == SYRINGEFDDT.DoctorID).Select(x => String.Concat(x.Firstname + " " + (x.MiddleName != null ? x.MiddleName : " ") + " " + x.LastName)).FirstOrDefault(),
                                                           REMARKS = SYRINGEFDDT.REMARKS,
                                                           REGTRANID = SYRINGEFDDT.REGTRANID,
                                                           CMPID = SYRINGEFDDT.CMPID
                                                       }).OrderByDescending(x => x.VISITDATE).ToList();
                return new
                {
                    Success = true,
                    data = Details,
                };

            }
            catch (Exception)
            {

                return new
                {
                    Success = false,
                };
            }

        }



        public dynamic DeleteFDDTSyringe(FddtSyringeRemovals FddtSyringeRemovalss, string uin, int ID, int Cmpid)
        {
            try
            {
                var SYRINGEFDDT = WYNKContext.SYRINGEFDDT.Where(x => x.ID == ID && x.UIN == uin && x.CMPID == Cmpid).FirstOrDefault();
                SYRINGEFDDT.IsActive = false;
                SYRINGEFDDT.RemovedDateTime = DateTime.UtcNow;
                SYRINGEFDDT.Removedby = FddtSyringeRemovalss.FddtSyringeRemoval.RemovedBy;
                SYRINGEFDDT.RemovalReasons = FddtSyringeRemovalss.FddtSyringeRemoval.RemovalReason;
                WYNKContext.SYRINGEFDDT.UpdateRange(SYRINGEFDDT);
                WYNKContext.SaveChanges();

                return new
                {
                    Success = true,
                };
            }
            catch (Exception)
            {
                return new
                {
                    Success = false,
                };
            }
        }


        public dynamic GetRemovedFDDtSyrDetails(string uin, string GMT, int CMPID)
        {

            try
            {
                var Details = new PatientHistoryViewModel();
                Details.FDDTDescriptionDetails = new List<FDDTDescriptionDetail>();
                Details.SYRINGINGDescriptionDetails = new List<FDDTDescriptionDetail>();

                var FDDtIDs = WYNKContext.SpecialityMaster.Where(x => x.ParentTag == "FDDT").Select(x => x.ID).ToList();
                var SyrIDs = WYNKContext.SpecialityMaster.Where(x => x.ParentTag == "SYRINGING").Select(x => x.ID).ToList();

                var Users = CMPSContext.Users.Where(x => x.CMPID == CMPID).ToList();
                var Docmast = CMPSContext.DoctorMaster.Where(x => x.CMPID == CMPID).ToList();



                Details.FDDTDescriptionDetails = (from SYRINGEFDDT in WYNKContext.SYRINGEFDDT.Where(x => x.UIN == uin && x.IsActive == false)
                                                  where FDDtIDs.Contains(SYRINGEFDDT.FDDTSYRINGEID)
                                                  select new FDDTDescriptionDetail
                                                  {
                                                      ID = SYRINGEFDDT.ID,
                                                      UIN = SYRINGEFDDT.UIN,
                                                      VISITDATE = SYRINGEFDDT.VISITDATE.Add(TimeSpan.Parse(GMT)),
                                                      FDDTSYRINGEID = SYRINGEFDDT.FDDTSYRINGEID,
                                                      FDDTDESCRIPTION = SYRINGEFDDT.FDDTDESCRIPTION,
                                                      Eye = SYRINGEFDDT.Eye,
                                                      Doctor = Docmast.Where(x => x.DoctorID == SYRINGEFDDT.DoctorID).Select(x => String.Concat(x.Firstname + " " + (x.MiddleName != null ? x.MiddleName : " ") + " " + x.LastName)).FirstOrDefault(),
                                                      RemovalReasons = SYRINGEFDDT.RemovalReasons,
                                                      Removedby = Docmast.Where(Doc => Doc.EmailID == Users.Where(x => x.Userid == SYRINGEFDDT.Removedby).Select(x => x.Emailid).FirstOrDefault()).Select(Doc => String.Concat(Doc.Firstname + " " + (Doc.MiddleName != null ? Doc.MiddleName : " ") + " " + Doc.LastName)).FirstOrDefault(),
                                                      RemovedDateTime = Convert.ToDateTime(SYRINGEFDDT.RemovedDateTime).Add(TimeSpan.Parse(GMT)),
                                                      REMARKS = SYRINGEFDDT.REMARKS,
                                                      REGTRANID = SYRINGEFDDT.REGTRANID,
                                                      CMPID = SYRINGEFDDT.CMPID
                                                  }).ToList();

                Details.SYRINGINGDescriptionDetails = (from SYRINGEFDDT in WYNKContext.SYRINGEFDDT.Where(x => x.UIN == uin && x.IsActive == false)
                                                       where SyrIDs.Contains(SYRINGEFDDT.FDDTSYRINGEID)
                                                       select new FDDTDescriptionDetail
                                                       {
                                                           ID = SYRINGEFDDT.ID,
                                                           UIN = SYRINGEFDDT.UIN,
                                                           VISITDATE = SYRINGEFDDT.VISITDATE.Add(TimeSpan.Parse(GMT)),
                                                           FDDTSYRINGEID = SYRINGEFDDT.FDDTSYRINGEID,
                                                           FDDTDESCRIPTION = SYRINGEFDDT.FDDTDESCRIPTION,
                                                           REGURGITATION = SYRINGEFDDT.REGURGITATION,
                                                           FLUID = SYRINGEFDDT.FLUID,
                                                           Stop = SYRINGEFDDT.Stop,
                                                           Eye = SYRINGEFDDT.Eye,
                                                           Doctor = Docmast.Where(x => x.DoctorID == SYRINGEFDDT.DoctorID).Select(x => String.Concat(x.Firstname + " " + (x.MiddleName != null ? x.MiddleName : " ") + " " + x.LastName)).FirstOrDefault(),
                                                           RemovalReasons = SYRINGEFDDT.RemovalReasons,
                                                           Removedby = Docmast.Where(Doc => Doc.EmailID == Users.Where(x => x.Userid == SYRINGEFDDT.Removedby).Select(x => x.Emailid).FirstOrDefault()).Select(Doc => String.Concat(Doc.Firstname + " " + (Doc.MiddleName != null ? Doc.MiddleName : " ") + " " + Doc.LastName)).FirstOrDefault(),
                                                           RemovedDateTime = Convert.ToDateTime(SYRINGEFDDT.RemovedDateTime).Add(TimeSpan.Parse(GMT)),
                                                           REMARKS = SYRINGEFDDT.REMARKS,
                                                           REGTRANID = SYRINGEFDDT.REGTRANID,
                                                           CMPID = SYRINGEFDDT.CMPID
                                                       }).ToList();


                if (Details.FDDTDescriptionDetails.Count > 0 || Details.SYRINGINGDescriptionDetails.Count > 0)
                {
                    return new
                    {
                        Success = "Data Found",
                        data = Details,
                    };

                }
                else
                {
                    return new
                    {
                        Success = "No Data Found",
                    };

                }



            }
            catch (Exception)
            {

                return new
                {
                    Success = false,
                };
            }

        }




    }
}