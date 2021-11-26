using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IProfRepository : IRepositoryBase<ProfessionalView>
    {
        dynamic InsertPros(ProfessionalView pos);
        ProfessionalView GetDetails();
        ProfessionalView getData();
        dynamic deleteProfes(int? ID);
        dynamic Gettrans(int ID);
        dynamic UpdatePart(ProfessionalView De1, int ID,int? pfID);
     
    }
}

