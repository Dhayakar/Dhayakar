
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{

    public interface IForm3cRepository : IRepositoryBase<Form3cViewModel>
    {
        dynamic getConsultationSummary(Form3cViewModel Form3cViewModel, string FromDate, string Todate, int companyid, string GMT);

    }
}
