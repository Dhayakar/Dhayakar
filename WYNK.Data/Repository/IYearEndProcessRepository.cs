using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IYearEndProcessRepository : IRepositoryBase<YearEndProcessViewModel>
    {
        YearEndProcessViewModel GetFinancialYearDetail(int cmpid);
        dynamic GetFinancialID(int ID, int cmpid);
        dynamic GetFinancialYearItemBalance(string ID, int cmpid, int FFYID, int TFYID);
        dynamic Submit(int cmpid, int FFYID, int TFYID, string Store, int Createdby);
        dynamic SubmitOptical(int cmpid, int FFYID, int TFYID, string Store, int Createdby);
        dynamic GetOpticaLFinancialYearItemBalance(string ID, int cmpid, int FFYID, int TFYID);


    }

}


