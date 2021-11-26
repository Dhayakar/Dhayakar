using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface ISurgeryRepository : IRepositoryBase<SurgeryViewModel>

    {
        dynamic InsertSurgeryMas(SurgeryViewModel SurgeryMaster);
        dynamic InsertIntraoperative(SurgeryViewModel Intraoperative);
        dynamic InsertPreoperative(SurgeryViewModel Preoperative);
        dynamic UpdateSurgeryMas(SurgeryViewModel UpdateSurgeryMaster, int M_AdmId, int M_surId);


    }

}


