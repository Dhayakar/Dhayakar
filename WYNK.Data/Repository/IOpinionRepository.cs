using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IOpinionRepository : IRepositoryBase<Opinionmasterref>
    {
        dynamic Insertopinion(Opinionmasterref Addopinion);


        Opinionmasterref getopiniondetailssataus();
    }
}