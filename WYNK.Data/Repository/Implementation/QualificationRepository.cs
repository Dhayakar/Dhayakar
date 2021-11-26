
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
    class QualificationRepository : RepositoryBase<QualificationMasterViewModel>, IQualificationRepository
    {

        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
       

        public QualificationRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;
           
        }
        public dynamic AddValues(QualificationMasterViewModel add)
        {
            try
            {

                var QualificationMaster = new Qualification();

                QualificationMaster.Description = add.qualification.Description;
                QualificationMaster.IsActive = true;
                QualificationMaster.CreatedUTC = DateTime.UtcNow;
                QualificationMaster.CreatedBy = add.uid;
                WYNKContext.Qualification.AddRange(QualificationMaster);



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


        public dynamic Addvaluess(QualificationMasterViewModel add, int id)
        {
            try
            {
                var Qualificationn = WYNKContext.Qualification.ToList();
                var QualificationExt = WYNKContext.QualificationExt.ToList();


                var QualificationMasterrExt = new QualificationExt();
                QualificationMasterrExt.QualCode = add.qualificationExt.QualCode = id;
                QualificationMasterrExt.QualExtDescription = add.qualificationExt.QualExtDescription;
                QualificationMasterrExt.QualExtAnsDescription = add.qualificationExt.QualExtAnsDescription;
                QualificationMasterrExt.IsActive = true;
                QualificationMasterrExt.CreatedUTC = DateTime.UtcNow;
                QualificationMasterrExt.CreatedBy = add.uid;



                WYNKContext.QualificationExt.AddRange(QualificationMasterrExt);

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
        public dynamic AddvaluessS(QualificationMasterViewModel addd, int id)
        {
            var Qualificationn = WYNKContext.Qualification.ToList();
            var QualificationExt = WYNKContext.QualificationExt.ToList();


            var QualificationMasterrExt = new QualificationExt();
            QualificationMasterrExt.QualCode = id;
            QualificationMasterrExt.QualExtDescription = addd.qualificationExt.QualExtDescription;
            QualificationMasterrExt.QualExtAnsDescription = addd.qualificationExt.QualExtAnsDescription;
            QualificationMasterrExt.IsActive = true;
            QualificationMasterrExt.CreatedUTC = DateTime.UtcNow;
            QualificationMasterrExt.CreatedBy = addd.uid;



            WYNKContext.QualificationExt.AddRange(QualificationMasterrExt);

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
            return new
            {
                Success = false,
                Message = "Some data are Missing"



            };


        }


        public dynamic getDetails()
        {

            var get = new QualificationMasterViewModel();

            var Qualification = WYNKContext.Qualification.ToList();
            var QualificationExt = WYNKContext.QualificationExt.ToList().Where(x => x.IsActive == true);

            get.getdetail = (from M_Qualification in Qualification
                             join M_QualificationExt in QualificationExt
                             on M_Qualification.ID equals M_QualificationExt.QualCode
                             select new QualificationMasterViewModel.getdetails
                             {
                                 Description = M_Qualification.Description,
                                 QualExtDesc = M_QualificationExt.QualExtDescription,
                                 QualExtAns = M_QualificationExt.QualExtAnsDescription,
                                 idd = M_Qualification.ID.ToString(),
                                 IsActive = M_Qualification.IsActive,

                             }).ToList();

            return get;
        }

        public dynamic Update(QualificationMasterViewModel upd, int id)
        {


            var QualificationMaster = new Qualification();
            QualificationMaster = WYNKContext.Qualification.Where(x => x.ID == id).FirstOrDefault();
            QualificationMaster.IsActive = upd.qualification.IsActive;
            QualificationMaster.CreatedUTC = DateTime.UtcNow;
            QualificationMaster.CreatedBy = upd.uid;
            WYNKContext.Qualification.UpdateRange(QualificationMaster);
            WYNKContext.SaveChanges();



            try
            {
                if (WYNKContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,
                        Message = "Updated successfully"



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



        public dynamic Delete(QualificationMasterViewModel del, string id, int idd)
        {
            var qualifymaster = WYNKContext.QualificationExt.Where(x => x.QualExtAnsDescription == id).ToList();
            var qualifymaster1 = WYNKContext.Qualification.Where(x => x.ID == idd).ToList();
            if (qualifymaster != null)
            {
                qualifymaster.All(x => { x.IsActive = false; return true; });
                WYNKContext.QualificationExt.UpdateRange(qualifymaster);

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

        public dynamic getDetailsdegree(int id)
        {
            var degreeDetails = new QualificationMasterViewModel();

            degreeDetails.getdetail = (from D in WYNKContext.QualificationExt.Where(u => u.QualCode == id && u.IsActive == true).GroupBy(x => x.QualExtDescription)
                                       select new QualificationMasterViewModel.getdetails
                                       {

                                           QualExtDesc = D.Select(x => x.QualExtDescription).FirstOrDefault(),

                                       }).ToList();


            return degreeDetails;

        }
        public dynamic getDetailsspec(string val)
        {
            var spec = new QualificationMasterViewModel();

            spec.getdetail = (from D in WYNKContext.QualificationExt.Where(u => u.QualExtDescription == val && u.IsActive == true)

                              select new QualificationMasterViewModel.getdetails
                              {
                                  QualExtAns = D.QualExtAnsDescription,
                                  ide = D.ID
                              }).ToList();
            return spec;
        }
        public dynamic getDetailsAll()
        {
            var Qualifydetails = new QualificationMasterViewModel();
            Qualifydetails.getdetail = (from D in WYNKContext.Qualification.GroupBy(x => x.CreatedUTC)
                                        select new QualificationMasterViewModel.getdetails
                                        {

                                            Description = D.Select(x => (x.Description)).FirstOrDefault(),
                                        }).ToList();

            Qualifydetails.getdetailss = (from D in WYNKContext.Qualification

                                          select new QualificationMasterViewModel.getdetails1
                                          {
                                              Description = D.Description,
                                              idd = D.ID
                                          }).ToList();

            return Qualifydetails;



        }


    }

}

