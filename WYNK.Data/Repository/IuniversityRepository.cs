using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Common;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IuniversityRepository : IRepositoryBase<UniversityViewModel>
    {
        dynamic getDetailsAll();
        dynamic getDetailscollege(int id);
        dynamic getDetailsLoc();
        dynamic Addvalues(UniversityViewModel add,int id,int companyid);
        dynamic Addvalues1(UniversityViewModel add, int id,int id1);
        dynamic getDetailsuniv();
        dynamic Delete(UniversityViewModel del, string id);
    }
}
