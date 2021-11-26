
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class PurchaseOrderCancellationPrintingRepository : RepositoryBase<PurchaseOrderCancellationPrinting>, IPurchaseOrderCancellationPrintingRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public PurchaseOrderCancellationPrintingRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }



    }
}













