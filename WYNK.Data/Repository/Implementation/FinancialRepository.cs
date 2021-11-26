
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    public class FinancialRepository : RepositoryBase<Financialview>, IFinancialRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public FinancialRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }
        public Financialview GetFpartDetail(int cmpid)
        {
            var ddetail = new Financialview();

            ddetail.FinancialDetails = (from get in WYNKContext.FinancialYear.Where(x =>x.CMPID == cmpid && x.IsActive == true)
                                        select new FinancialDetails
                                        {
                                            ID = get.ID,
                                            FYDescription = get.FYDescription,
                                            FYFrom = get.FYFrom,
                                            FYTo = get.FYTo,
                                            FYAccYear = get.FYAccYear,
                                            CreatedUTC = get.CreatedUTC,
                                            IsActive = Getbooleanvalue(get.IsActive),

                                        }).ToList();
            return ddetail;
        }




        public static string Getbooleanvalue(Boolean valuess)
        {
            var d = "";

            if (valuess == true)
            {
                d = "Active";
            }
            else
            {
                d = "In-Active";
            }


            return d;
        }


        public Financialview GetStsDetail(int M_ID, int docotorid)
        {

            var pt1 = CMPSContext.Users.Where(x => x.Userid == docotorid).Select(x => x.Userid).FirstOrDefault();
            var Reftag = CMPSContext.Users.Where(x => x.Userid == pt1).Select(x => x.ReferenceTag).FirstOrDefault();

            var Sdetail = new Financialview();

            Sdetail.Tag = Reftag;
            Sdetail.FinancialDetails = (from get in WYNKContext.FinancialYearStatus.OrderByDescending(x =>x.DateTime).Where(x => x.FYID == M_ID)
                                        select new FinancialDetails
                                        {

                                           
                                            FYStatus = Enum.GetName(typeof(Status), get.FYStatus),
                                            ID = get.ID,
                                            FYID = get.FYID,
                                            FYStatus1 = get.FYStatus.ToString(),
                                           

                                        }).ToList();
            return Sdetail;
        }
   
        public Financialview GetStatusDetails(int docotorid, int FYD)
        {
            var fyr = WYNKContext.FinancialYearStatus.ToList();
            var fr = WYNKContext.FinancialYear.ToList();
            var pt = CMPSContext.Users.Where(x => x.Userid == docotorid).Select(x => x.Username).FirstOrDefault();

            var usrs = CMPSContext.Users.ToList();
            var Gsdetail = new Financialview();
            Gsdetail.FinancialDetails = (from
                                         fir in fr.Where(x => x.ID == FYD)
                                         join
                                         fyrs in fyr
                                         on fir.ID equals
                                         fyrs.FYID
                                         select new FinancialDetails
                                         {
                                             DateTime = fyrs.DateTime,                                             
                                             FYStatus = Enum.GetName(typeof(Status), fyrs.FYStatus),
                                             ClosedBy = pt,
                                             FYDescription = fir.FYDescription,

                                         }).ToList();
            return Gsdetail;
        }

        public dynamic InsertFart(Financialview Fe)
        {


            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {


                    var FYID =  WYNKContext.FinancialYear.Where(x => x.CMPID == Fe.Fes.CMPID && x.FYAccYear == Fe.Fes.FYAccYear).Select(x => x.ID).FirstOrDefault();
                    if (FYID != 0)
                    {
                        return new
                        {
                            Success = false,
                            Message = "Financial Year already exists"
                        };
                    }


                    var Fpart = new Financial();
                    Fpart.FYDescription = Fe.Fes.FYDescription;
                    Fpart.FYFrom = Fe.Fes.FYFrom.AddDays(1);
                    Fpart.FYTo = Fe.Fes.FYTo.AddDays(1);
                    Fpart.FYAccYear = Fe.Fes.FYAccYear;
                    Fpart.CreatedUTC = DateTime.UtcNow;
                    Fpart.IsActive = true;
                    Fpart.CreatedBy = Fe.Fes.CreatedBy;
                    Fpart.CMPID = Fe.Fes.CMPID;
                    WYNKContext.FinancialYear.Add(Fpart);
                    WYNKContext.SaveChanges();

                    var Finid = WYNKContext.FinancialYear.Where(x=>x.CMPID == Fe.Fes.CMPID).OrderByDescending(x => x.ID).Select(x =>x.ID).FirstOrDefault();

                    var DD = Fpart.ID;
                    var fins = new FinancialStatus();
                    fins.ClosedBy = Fe.Fts.ClosedBy;
                    fins.FYStatus = 1;
                    fins.FYID = Finid;
                    fins.DateTime = DateTime.UtcNow;
                    WYNKContext.FinancialYearStatus.AddRange(fins);
                    WYNKContext.SaveChanges();

                    dbContextTransaction.Commit();

                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Message = "Saved Successfully"
                        };
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                    dbContextTransaction.Rollback();
                }
                return new
                {
                    Success = false,
                    Message = "Some data are Missing"

                };

            }
    
        }

        public dynamic UpdateFart(Financialview Fe1, int ID, int FID)
        {
            var Fpart1 = new Financial();
            Fpart1 = WYNKContext.FinancialYear.Where(x => x.ID == ID).FirstOrDefault();
            Fpart1.FYDescription = Fe1.Fes.FYDescription;
            Fpart1.IsActive = Fe1.Fes.IsActive;
            Fpart1.UpdatedBy = Fe1.Fes.UpdatedBy;
            Fpart1.UpdatedUTC = DateTime.UtcNow;

            WYNKContext.FinancialYear.UpdateRange(Fpart1);
            WYNKContext.SaveChanges();


            var DD = Fpart1.ID;
            var fins = new FinancialStatus();
            fins.ClosedBy = Fe1.Fts.ClosedBy;
            fins.FYStatus = Fe1.Fts.FYStatus;
            fins.FYID = DD;
            fins.DateTime = DateTime.UtcNow;

            WYNKContext.FinancialYearStatus.AddRange(fins);
            WYNKContext.SaveChanges();

            try
            {
                if (WYNKContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = "Updated Successfully"
                    };

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Succes = false,
                Message = "Some data are Missing"
            };

        }

    }



}





