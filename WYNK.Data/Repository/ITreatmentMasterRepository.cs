using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface ITreatmentMasterRepository:IRepositoryBase<TreatmentMasterDataView>
    {
        dynamic InsertTreatment(TreatmentMasterDataView Con);
        TreatmentMasterDataView PopupSearch();
        dynamic UpdateTratment(TreatmentMasterDataView Cons);
        dynamic DeleteTreatment(TreatmentMasterDataView Cons1,int ID);
    }
}
