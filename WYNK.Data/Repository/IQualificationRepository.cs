using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Common;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IQualificationRepository : IRepositoryBase<QualificationMasterViewModel>
    {
        dynamic AddValues(QualificationMasterViewModel add);
        dynamic getDetails();
        dynamic Update(QualificationMasterViewModel upd, int id);
        dynamic Delete(QualificationMasterViewModel del, string id,int idd);
        dynamic getDetailsdegree(int id);
        dynamic getDetailsspec(string id);
        dynamic getDetailsAll();
        dynamic Addvaluess(QualificationMasterViewModel add,int id);
        dynamic AddvaluessS(QualificationMasterViewModel addd, int id);
       
    }
}
