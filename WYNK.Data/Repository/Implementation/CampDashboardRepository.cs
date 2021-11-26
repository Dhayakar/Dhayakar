
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Implementation;
using WYNK.Helpers;

namespace WYNK.Data.Repository
{
    class CampDashboardRepository : RepositoryBase<CampDashboardViewModel>, ICampDashboardRepository
    {

        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public CampDashboardRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public  dynamic GetPeriodSearch(DateTime FromDate,  int Cmpid)
        {
            try
            {

                var From = FromDate.ToString("MM/yyyy");
                var res = (from Camp in WYNKContext.CampPatientFootfall.Where(x => Convert.ToDateTime(x.Date.ToString("MM/yyyy")) >= Convert.ToDateTime(From) && Convert.ToDateTime(x.Date.ToString("MM/yyyy")) <= Convert.ToDateTime(From) && x.CmpID == Cmpid)
                           group Camp by Camp.CampID  into CampRes
                           select new
                           {
                               CampPatFootFallID = CampRes.Select(x => x.RandomUniqueID).ToList(),
                               CampDescription = WYNKContext.CAMP.Where(x => x.CampID == CampRes.Key).Select(x => x.CampName).FirstOrDefault(),
                               PatientsScreened = CampRes.Select(x => x.PatientCount).Sum(),
                               SurgeryAdvised = CampRes.Select(x => x.SurgeryAdvisedPatient).Sum(),
                               SurgeryUnderwent = CampRes.Select(x => x.SurgeryUnderwentPatient).Sum()
                           }).ToList();

                return new
                {
                    Success = true,
                    res = res,
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


        public dynamic GetCampSearch(CampDashboardViewModel campDashboardView, int Cmpid)
        {
            try
            {
                var CampSearchDetail = new CampDashboardViewModel();

                var Icds = WYNKContext.ICDSpecialityCode.ToList();

                var CampPatientFootfallTran = (from CampPatientFootfallTrans in WYNKContext.CampPatientFootfallTran
                                               where campDashboardView.CampPatFootFallIDs.Contains(CampPatientFootfallTrans.CAMPFFId)
                                               select CampPatientFootfallTrans).ToList();

                CampSearchDetail.CampSearchDetails = (from CampFoot in CampPatientFootfallTran
                                                      group CampFoot by CampFoot.SpecialityID into CampFootTranRes
                                                      select new CampSearchDetail
                                                      {
                                                          SurgeryCount = CampFootTranRes.Count(),
                                                          SurgeryDescription = Icds.Where(ic => ic.ID == CampFootTranRes.Key).Select(ic => ic.SpecialityDescription).FirstOrDefault(),
                                                          CampFttranRandomUniqueID = CampFootTranRes.Select(x => x.RandomUniqueID).ToList(),
                                                      }).ToList();

                return new
                {
                    Success = true,
                    res = CampSearchDetail.CampSearchDetails,
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

        public dynamic GetDoctorBreakup(CampDashboardViewModel campDashboardView,int CMPID)
        {
            try
            {
                var CampPatientFootDoctor = (from CampFootDoc in WYNKContext.CampPatientFootDoctor
                                             where campDashboardView.CampFttranRandomUniqueIDs.Contains(CampFootDoc.CampFRTID)
                                             select CampFootDoc).ToList();

                var DocMast = CMPSContext.DoctorMaster.Where(x => x.CMPID == CMPID).ToList();

                var res = (from CampFootDoc in CampPatientFootDoctor
                           where campDashboardView.CampFttranRandomUniqueIDs.Contains(CampFootDoc.CampFRTID)
                           group CampFootDoc by CampFootDoc.DoctorID into CampFootDocRes
                           select new
                           {
                               DoctorCount = CampFootDocRes.Count(),
                               DoctorName = DocMast.Where(x => x.DoctorID == CampFootDocRes.Key).Select(x => String.Concat(x.Firstname + " " + (x.MiddleName != null ? x.MiddleName : " ") + " " + x.LastName)).FirstOrDefault(),
                               SurgeryIDs = CampFootDocRes.Select(x => x.SurgeryID).ToList(),
                           }).ToList();


                return new
                {
                    Success = true,
                    res = res,
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

        public dynamic GetPatientBreakup(CampDashboardViewModel campDashboardView, int Cmpid)
        {
            try
            {
                var SurgerysList = (from Surgery in WYNKContext.Surgery.Where(x=>x.CMPID == Cmpid)
                                    where campDashboardView.SurgeryIDs.Contains(Surgery.RandomUniqueID)
                                    select Surgery).ToList();



                var res = (from SurgeryList in SurgerysList
                           select new
                           {
                               PatientName = WYNKContext.Registration.Where(x => x.CMPID == Cmpid && x.UIN == SurgeryList.UIN).Select(x => String.Concat(x.Name + " " + (x.MiddleName != null ? x.MiddleName : " ") + " " + x.LastName)).FirstOrDefault(),
                               ClinicVisitDate = WYNKContext.Registration.Where(x => x.CMPID == Cmpid && x.UIN == SurgeryList.UIN).Select(x => x.DateofRegistration).FirstOrDefault(),
                               CampVisitDate = WYNKContext.CampRegistration.Where(x => x.CMPID == Cmpid && x.CampUIN == SurgeryList.UIN).Select(x => x.DateofRegistration).FirstOrDefault(),
                               SurgeryDate = SurgeryList.DateofSurgery,
                               Age = PasswordEncodeandDecode.ToAgeString(WYNKContext.Registration.Where(x => x.CMPID == Cmpid && x.UIN == SurgeryList.UIN).Select(x => x.DateofBirth).FirstOrDefault())
                           }).ToList();


                return new
                {
                    Success = true,
                    res = res,
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
    }
}
