using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;
using Microsoft.AspNetCore.Http;

namespace WYNK.Data.Repository
{
    public interface IOfferRepository:IRepositoryBase<OfferViewM>
    {
        dynamic Insertdata(OfferViewM offerviewm);
        OfferViewM GetModels(int ID);
        dynamic getOneplusmodelvalue(int ID);
    }

}
