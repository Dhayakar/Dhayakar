using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class PostoperativeRepository : RepositoryBase<Postoperativeview>, IPostoperativeRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public PostoperativeRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic Insertpostoperative(Postoperativeview postop)

        {

            var postoperative = new Postoperative();

            postoperative.SurgeryID = postop.PostOperative.SurgeryID;
            postoperative.PostOperativeDate = postop.PostOperative.PostOperativeDate.AddDays(1);
            postoperative.ComplicationDetails = postop.PostOperative.ComplicationDetails;
            postoperative.TreatmentAdvive = postop.PostOperative.TreatmentAdvive;

            if (postop.PostOperative.ReviewDate != null)
            {
                postoperative.ReviewDate = postop.PostOperative.ReviewDate.Value.AddDays(1);
            }

            postoperative.CreatedBy = postop.PostOperative.CreatedBy;

            WYNKContext.PostOperative.AddRange(postoperative);

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



        public dynamic Updatepostoperative(Postoperativeview postopup, int ID)
        {

            var Postopup = new Postoperative();

            Postopup = WYNKContext.PostOperative.Where(x => x.ID == ID).FirstOrDefault();

            Postopup.PostOperativeDate = postopup.PostOperative.PostOperativeDate;
            Postopup.ComplicationDetails = postopup.PostOperative.ComplicationDetails;
            Postopup.TreatmentAdvive = postopup.PostOperative.TreatmentAdvive;
            Postopup.ReviewDate = postopup.PostOperative.ReviewDate.Value.AddDays(1);
            Postopup.UpdatedUTC = DateTime.UtcNow;
            Postopup.UpdatedBy = postopup.PostOperative.UpdatedBy;
            WYNKContext.PostOperative.UpdateRange(Postopup);
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

    }

}

