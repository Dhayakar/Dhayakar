using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class ReferralMasterRepository : RepositoryBase<Referral_Master>, IReferralMasterRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public ReferralMasterRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic UpdateRefMaster(Referral_Master ReferralMaster)
        {

            var RefsMasetr = new ReferralMasterS();

            RefsMasetr.REFERRAL_NAME = ReferralMaster.ReferralMaster.REFERRAL_NAME;
            RefsMasetr.REFERRAL_ADDRESS1 = ReferralMaster.ReferralMaster.REFERRAL_ADDRESS1;
            RefsMasetr.REFERRAL_ADDRESS2 = ReferralMaster.ReferralMaster.REFERRAL_ADDRESS2;
            RefsMasetr.REFERRAL_ADDRESS3 = ReferralMaster.ReferralMaster.REFERRAL_ADDRESS3;
            RefsMasetr.PHONE_NO = ReferralMaster.ReferralMaster.PHONE_NO;
            RefsMasetr.EMAIL_ID = ReferralMaster.ReferralMaster.EMAIL_ID;
            RefsMasetr.CONTACT_PERSON = ReferralMaster.ReferralMaster.CONTACT_PERSON;



            try
            {
                if (WYNKContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,
                        Message = "Speciality saved successfully"
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













