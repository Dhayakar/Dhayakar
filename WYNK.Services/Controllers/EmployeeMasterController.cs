using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using WYNK.Data.Repository;
using WYNK.Data.Common;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class EmployeeMasterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public EmployeeMasterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpPost("AddEmployee")]
        public dynamic AddEmployee([FromBody] Employee_master EmployeeAdd)
        {
            return _repoWrapper.employee.AddEmployee(EmployeeAdd);
        }


        [HttpGet("Getemployeeimage/{ID}")]
        public dynamic Getemployeeimage(int ID)
        {
            return _repoWrapper.employee.Getemployeeimage(ID);
        }

        [HttpPost("uploadImage/{Empid}")]
        public bool UploadImage(int Empid)
        {
            var file = Request.Form.Files[0];
            return _repoWrapper.employee.UploadImage(file, Empid);
        }

        [HttpPost("DeleteEmployee/{EmployeeID}")]
        public dynamic DeleteEmployee(int? EmployeeID)
        {
            return _repoWrapper.employee.DeleteEmployee(EmployeeID);
        }

        [HttpPost("UpdateEmployee/{Employeeid}")]
        public dynamic UpdateEmployee([FromBody] Employee_master EmployeeMaster, int? Employeeid)
        {
            return _repoWrapper.employee.UpdateEmployee(EmployeeMaster, Employeeid);
        }



        [HttpPost("DeleteEmployeebank/{EmployeeID}")]
        public dynamic DeleteEmployeebank(int? EmployeeID)
        {
            return _repoWrapper.employee.DeleteEmployeebank(EmployeeID);
        }


        [HttpGet("GetDegree/{id}")]
        public dynamic GetDegree(int id)

        {
            return _repoWrapper.employee.GetDegree(id);
        }

        [HttpGet("GetSpecialization/{name}")]
        public dynamic GetSpecialization(string name)

        {
            return _repoWrapper.employee.GetSpecialization(name);
        }


        [HttpGet("GetlocationDetails/{id}")]
        public dynamic GetlocationDetails(int id)

        {
            return _repoWrapper.employee.GetlocationDetails(id);
        }

    }
}

