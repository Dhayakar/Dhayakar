using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;
using Microsoft.AspNetCore.Http;

namespace WYNK.Data.Repository
{
    public interface ISlitLampRepository:IRepositoryBase<SlitLampViewM>
    {
        dynamic GetDesc();
        dynamic GetDescfi();
        dynamic GetDesctab();
        dynamic GetSubDesc(int ID);
        dynamic GetSubDescfi(int ID);
        dynamic GetaddDescfi(int ID);
        dynamic GetfullDesc(int ID);
        dynamic GetadDesc(int ID);
        dynamic InsertDesc(SlitLampViewM SlitLamp);
        dynamic UpdateDesc(SlitLampViewM SlitLamp, int ID);
        dynamic Deletedes(SlitLampViewM SlitLamp, int ID);
        dynamic Deletedess(SlitLampViewM SlitLamp, int ID);
        dynamic Deletedesss(SlitLampViewM SlitLamp, int ID);
        dynamic UpdateSubDesc(SlitLampViewM SlitLamp, int ID);
        dynamic UpdateaddDesc(SlitLampViewM SlitLamp, int ID);
        dynamic InsertSubdesc(SlitLampViewM SlitLamp);
        dynamic InsertAddesc(SlitLampViewM SlitLamp);
    }
}
