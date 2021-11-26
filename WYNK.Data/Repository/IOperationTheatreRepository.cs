using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IOperationTheatreRepository : IRepositoryBase<OperationTheatres>

    {
        dynamic Insertoperation(OperationTheatres Operation);
        dynamic Updateoperation(OperationTheatres Operationup, int ID);
        dynamic Deleteoperation(int ID);

    }

}



