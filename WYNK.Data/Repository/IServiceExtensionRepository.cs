using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IServiceExtensionRepository : IRepositoryBase<ServiceExtensionViewModel>
    {

        ServiceExtensionViewModel GetInpatientBilleddtls(int cmpid); 
        ServiceExtensionViewModel GetOutpatientBilleddtls(int cmpid);
       // ServiceExtensionViewModel GetServicedtls(int cmpid);
        
    }

}


