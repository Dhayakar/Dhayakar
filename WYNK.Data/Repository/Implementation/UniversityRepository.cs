using Microsoft.EntityFrameworkCore;
using CommonMailService.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class UniversityRepository : RepositoryBase<UniversityViewModel>, IuniversityRepository
    {

        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        
        public UniversityRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;
            
        }

        public dynamic getDetailsAll()
        {
            var Universitydetails = new UniversityViewModel();
            Universitydetails.getdetail = (from D in WYNKContext.University.GroupBy(x => x.CreatedUTC)
                                           select new getdetails
                                           {

                                               universityDescription = D.Select(x => (x.UniversityDescription)).FirstOrDefault(),
                                           }).ToList();

            Universitydetails.getdetails1 = (from D in WYNKContext.University

                                             select new getdetailss
                                             {
                                                 universityDescription1 = D.UniversityDescription,
                                                 id1 = D.UniversityCode
                                             }).ToList();

            return Universitydetails;



        }
        public dynamic getDetailscollege(int id)
        {
            var collegedetails = new UniversityViewModel();

            collegedetails.getdetailscol = (from D in WYNKContext.UniversityExt.Where(u => u.UniversityCode == id && u.IsActive == true).GroupBy(x => x.UniversityExtDescription)
                                            select new getdetailscollege
                                            {

                                                college = D.Key
                                            }).ToList();


            return collegedetails;

        }
        public dynamic getDetailsLoc()
        {
            var loc = new UniversityViewModel();

            var location = CMPSContext.LocationMaster.ToList();

            loc.getdetailsloc = (from loc1 in location.Where(x => x.ParentTag == "City")

                                 select new getdetailsloc
                                 {
                                     location = loc1.ParentDescription,
                                     id = loc1.ID
                                 }).ToList();
            return loc;

        }
        public dynamic Addvalues(UniversityViewModel add, int id, int companyid)
        {

            try
            {
                var U_university = new University();
                U_university.CMPID = companyid;
                U_university.UniversityDescription = add.University_M.UniversityDescription;
                U_university.LocationId = id;
                U_university.IsActive = true;
                U_university.CreatedUTC = DateTime.UtcNow;
                U_university.CreatedBy = add.uid;
                WYNKContext.University.AddRange(U_university);

                try
                {
                    if (WYNKContext.SaveChanges() > 0)
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
        public dynamic Addvalues1(UniversityViewModel add, int id, int id1)
        {
            try
            {

                var U_universityExt = new UniversityExt();
                U_universityExt.UniversityExtDescription = add.UniversityExt_M.UniversityExtDescription;
                U_universityExt.UniversityCode = id1;
                U_universityExt.Locationid = id;
                U_universityExt.IsActive = true;
                U_universityExt.CreatedUTC = DateTime.UtcNow;
                U_universityExt.CreatedBy = add.uid;
                WYNKContext.UniversityExt.AddRange(U_universityExt);
                try
                {
                    if (WYNKContext.SaveChanges() > 0)
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
        public dynamic getDetailsuniv()
        {
            var univ = new UniversityViewModel();

            var university = WYNKContext.University.ToList();
            var college = WYNKContext.UniversityExt.ToList().Where(x => x.IsActive == true);
            var location = CMPSContext.LocationMaster.ToList();

            univ.getdetails = (from UNIV in university
                               join COL in college
                               on UNIV.UniversityCode equals COL.UniversityCode


                               select new getdetailsuniversity
                               {

                                   location = location.Where(x => x.ID == COL.Locationid).Select(x => x.ParentDescription).FirstOrDefault(),
                                   university = UNIV.UniversityDescription,
                                   college = COL.UniversityExtDescription

                               }).ToList();


            return univ;

        }
        public dynamic Delete(UniversityViewModel del, string id)
        {
            var Universitymaster = WYNKContext.UniversityExt.Where(x => x.UniversityExtDescription == id).ToList();

            if (Universitymaster != null)
            {
                Universitymaster.All(x => { x.IsActive = false; return true; });
                WYNKContext.UniversityExt.UpdateRange(Universitymaster);

            }

            try
            {
                if (WYNKContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = CommonRepository.saved
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = CommonRepository.Missing
            };

        }
    }
}
