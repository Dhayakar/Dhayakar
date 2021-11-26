
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class LocationController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public LocationController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("Getcountry")]
        public dynamic Getcountry()
        {
            return _repoWrapper.Loc.Getcountry();
        }

        [HttpPost("InsertCountry")]
        public dynamic InsertCountry([FromBody] LocationMasterViewModel country)
        {
            return _repoWrapper.Loc.InsertCountry(country);
        }

        [HttpGet("GetState/{ID}")]
        public dynamic GetState(int ID)
        {
            return _repoWrapper.Loc.GetState(ID);
        }

        [HttpPost("InsertState")]
        public dynamic InsertState([FromBody] LocationMasterViewModel state)
        {
            return _repoWrapper.Loc.InsertState(state);
        }

        [HttpGet("GetCity/{ID}")]
        public dynamic GetCity(int ID)
        {
            return _repoWrapper.Loc.GetCity(ID);
        }


        [HttpPost("InsertCity")]
        public dynamic InsertCity([FromBody] LocationMasterViewModel City)
        {
            return _repoWrapper.Loc.InsertCity(City);
        }

        [HttpGet("Getlocation/{ID}")]
        public dynamic Getlocation(int ID)
        {
            return _repoWrapper.Loc.Getlocation(ID);
        }


        [HttpPost("Insertlocation")]
        public dynamic Insertlocation([FromBody] LocationMasterViewModel Location)
        {
            return _repoWrapper.Loc.Insertlocation(Location);
        }


        [HttpGet("Getfulllocation/{ID}")]
        public dynamic Getfulllocation(int ID)
        {
            return _repoWrapper.Loc.Getfulllocation(ID);
        }

        [HttpPost("Updatelocation/{ID}")]
        public dynamic Updatelocation([FromBody] LocationMasterViewModel upLocation, int ID)
        {
            return _repoWrapper.Loc.Updatelocation(upLocation, ID);
        }

        [HttpPost("Updatecity/{ID}")]
        public dynamic Updatecity([FromBody] LocationMasterViewModel upcity, int ID)
        {
            return _repoWrapper.Loc.Updatecity(upcity, ID);
        }

        [HttpPost("UpdateState/{ID}")]
        public dynamic UpdateState([FromBody] LocationMasterViewModel upstate, int ID)
        {
            return _repoWrapper.Loc.UpdateState(upstate, ID);
        }

        [HttpPost("DeleteLocation/{ID}")]
        public dynamic DeleteLocation(int ID)
        {
            return _repoWrapper.Loc.DeleteLocation(ID);
        }

    }
}

