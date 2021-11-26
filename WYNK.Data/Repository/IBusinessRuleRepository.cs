using System;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IBusinessRuleRepository : IRepositoryBase<BusinessRuleViewModel>
    {

        dynamic InsertBusinessRule(BusinessRuleViewModel BusinessRule,int CmpID, int BRID);

        BusinessRuleViewModel GetBRMDescription(int Cmpid, int Descriptionid, DateTime EFDate);
       
    }
}
