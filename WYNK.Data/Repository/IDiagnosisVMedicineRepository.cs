
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IDiagnosisVSMedicineRepository : IRepositoryBase<diagnosisvsmedicine>
    {

        diagnosisvsmedicine Getdruggvalues(int cmpid);
        diagnosisvsmedicine GetSelectedmeddetials(int id, int cmpid);
        diagnosisvsmedicine GetsubSelectedmeddetials(string id, int cmpid);
        dynamic Insertdiagmeddata(diagnosisvsmedicine DiagnosisVSMedicine);

    }
}
