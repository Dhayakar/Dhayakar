using System;

using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IGstTaxSummaryRepository : IRepositoryBase<GstTaxSummaryViewM>
    {

        GstTaxSummaryViewM getTaxSummary(DateTime Date, int CompanyID);
    }
}
