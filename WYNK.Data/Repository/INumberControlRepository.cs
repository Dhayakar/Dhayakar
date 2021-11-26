using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface INumberControlRepository : IRepositoryBase<NumberControlViewModel>

    {
        
            dynamic InsertNum (NumberControlViewModel numberControl);
            dynamic UpdateNum(NumberControlViewModel numberControl, int VCID);
            dynamic DeleteNum(NumberControlViewModel numberControl, int VCID);
            NumberControlViewModel getNumberControl(int CmpID,int Description);
            NumberControlViewModel getgridNC(int CmpID);


    }

}


