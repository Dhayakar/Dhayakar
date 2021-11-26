
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IAllergyrepository : IRepositoryBase<AllergyViewmodel>
    {
        dynamic Insertallergy(AllergyViewmodel AddType);
        dynamic allergyrecord();
        dynamic updateallergy(AllergyViewmodel UpType, int IDT);
        dynamic Deleteallergy(int IDT);
        dynamic InsertDescription(AllergyViewmodel AddDescription);
        dynamic Descriptionrecord(int DescID);
        dynamic updateDescription(AllergyViewmodel UpDescription, int IDD);
        dynamic DeleteDescription(int IDD);

    }
}
