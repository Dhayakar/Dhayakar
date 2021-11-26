
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class AllergyController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public AllergyController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpPost("Insertallergy")]
        public dynamic Insertallergy([FromBody] AllergyViewmodel AddType)
        {
            return _repoWrapper.Allergy.Insertallergy(AddType);
        }

        [HttpGet("allergyrecord")]
        public dynamic allergyrecord()
        {
            return _repoWrapper.Allergy.allergyrecord();
        }

        [HttpPost("updateallergy/{IDT}")]
        public dynamic updateallergy([FromBody] AllergyViewmodel UpType, int IDT)
        {
            return _repoWrapper.Allergy.updateallergy(UpType, IDT);
        }

        [HttpPost("Deleteallergy/{IDT}")]
        public dynamic Deleteallergy(int IDT)
        {
            return _repoWrapper.Allergy.Deleteallergy(IDT);
        }

        [HttpPost("InsertDescription")]
        public dynamic InsertDescription([FromBody] AllergyViewmodel AddDescription)
        {
            return _repoWrapper.Allergy.InsertDescription(AddDescription);
        }


        [HttpGet("Descriptionrecord/{DescID}")]
        public dynamic Descriptionrecord(int DescID)

        {
            return _repoWrapper.Allergy.Descriptionrecord(DescID);
        }

        [HttpPost("updateDescription/{IDD}")]
        public dynamic updateDescription([FromBody] AllergyViewmodel UpDescription, int IDD)
        {
            return _repoWrapper.Allergy.updateDescription(UpDescription, IDD);
        }

        [HttpPost("DeleteDescription/{IDD}")]
        public dynamic DeleteDescription(int IDD)
        {
            return _repoWrapper.Allergy.DeleteDescription(IDD);
        }


    }
}