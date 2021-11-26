using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class OpinionRepository : RepositoryBase<Opinionmasterref>, IOpinionRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public OpinionRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }




        public dynamic Insertopinion(Opinionmasterref Addopinion)
        {



            var Opinion = new OpinionMaster();
            var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addopinion.UIN).Select(x => x.RegistrationTranID).FirstOrDefault();

            Opinion.RegistrationTranID = regid;
            Opinion.ContraOpionionID = 3;
            Opinion.FromDoctor = 4;
            Opinion.ToDoctor = Addopinion.OpinionMaster.ToDoctor;
            Opinion.OpinionDescription = Addopinion.OpinionMaster.OpinionDescription;
            Opinion.OpinionStatus = 9;
            Opinion.OpinionTriggeredFrom = "Refraction";
            Opinion.CreatedBy = 4;
            Opinion.UpdatedBy = 4;
            Opinion.IsDeleted = false;
            Opinion.CreatedUTC = DateTime.UtcNow;
            Opinion.UpdatedUTC = DateTime.UtcNow;
            WYNKContext.Opinion.Add(Opinion);


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





        public Opinionmasterref getopiniondetailssataus()

        {

            var registration = WYNKContext.Registration.ToList();
            var registrationtran = WYNKContext.RegistrationTran.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();
            var doctormaster = CMPSContext.DoctorMaster.ToList();
            var opinion = WYNKContext.Opinion.ToList();

            var opinions  = new Opinionmasterref();
            opinions.OPINIONDETAILS = (from rt in registrationtran
                                       join op in opinion on rt.RegistrationTranID equals op.RegistrationTranID
                                       join dm in doctormaster on op.FromDoctor equals dm.DoctorID   
                                       join rm in registration on rt.UIN equals rm.UIN
                                       join om in onelinemaster on rt.Status equals om.OLMID
                                       where op.ToDoctor == dm.DoctorID

                                       select new OPINIONLIST
                                       {
                                           OPUIN =  rt.UIN,
                                           PName = rm.Name,
                                           OSex = rm.Gender,
                                           DOV = rm.DateofRegistration,
                                           FODate = rt.FollowupDate,
                                           OStatus = om.ParentDescription,
                                           DOCTORID = doctormaster.Where(x =>x.DoctorID == op.ToDoctor).Select(x =>x.LastName).FirstOrDefault(),
                                           sentdate = op.CreatedUTC,
                                           respondeddate =  DateTime.Now,
                                       }).ToList();

            return opinions;
        }















    }
}














