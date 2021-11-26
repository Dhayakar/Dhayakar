using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IReferralMasterRepository : IRepositoryBase<Referral_Master>
    {

        dynamic UpdateRefMaster(Referral_Master ReferralMaster);

    }
}
