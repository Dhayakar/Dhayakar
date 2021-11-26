using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IPostoperativeRepository : IRepositoryBase<Postoperativeview>

    {

        dynamic Insertpostoperative(Postoperativeview postop);

        dynamic Updatepostoperative(Postoperativeview postopup, int ID);


    }

}
