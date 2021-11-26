using Microsoft.AspNetCore.Http;

using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IEmployeeMasterrepository : IRepositoryBase<Employee_master>
    {

        dynamic AddEmployee(Employee_master EmployeeAdd);
        dynamic DeleteEmployee(int? EmployeeID);
        dynamic UpdateEmployee(Employee_master EmployeeMaster, int? EmployeeID);
        dynamic DeleteEmployeebank(int? EmployeeID);
        dynamic GetDegree(int id);
        dynamic GetSpecialization(string name);
        dynamic UploadImage(IFormFile file, int Empid);
        dynamic Getemployeeimage(int ID);

        dynamic GetlocationDetails(int id);

    }
}


