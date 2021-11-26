using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class Rolevsaccesscontrol
    {

        public User_Master UserMasterAccess { get; set; }
        public Modelmastername Modelmastername { get; set; }
        public ICollection<RoleVsModuleaccess> RoleVsModule { get; set; }

        public ICollection<updateModuleMasterget> UpdateModuleMaster { get; set; }
        public ICollection<ModuleNameGet> ModuleNameGet { get; set; }
        public ICollection<ModuleMasterget> ModuleMasterget { get; set; }
        public ICollection<NonModuleMastergetuseraccess> NonModuleMastergetuseraccess { get; set; }
        public ICollection<ModuleMasterdetails> ModuleMasterdetails{ get; set; }
        public ICollection<ModuleMastergetuseraccess> ModuleMastergetuseraccess { get; set; }
        public ICollection<AccessNames> AccessNames { get; set; }
        public ICollection<CrudOperations> CrudOperations { get; set; }
        public int? transactionid { get; set; }

        public int? RecPayContra { get; set; }
        public int Roleid { get; set; }
        public string CmpID { get; set; }
        public int userdoctorid { get; set; }
    }
}

public class CrudOperations
{

    public string Add { get; set; }
    public string Edit { get; set; }
    public string Query { get; set; }
    public string Delete { get; set; }
    public string Print { get; set; }
    public string All { get; set; }


}


public class AccessNames
{

    public int Roleids { get; set; }
    public string Roledescriptions { get; set; }

}

public class ModuleNameGet
{

    public string RoleDesc { get; set; }
    public string UserName { get; set; }


}

public class updateModuleMasterget
{
    public string Desc { get; set; }
    public bool All { get; set; }
    public bool Add { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
    public bool Export { get; set; }
    public bool Update { get; set; }
    public string ModType { get; set; }

}

public class ModuleMasterget
{

    public string Desc { get; set; }
    public int Code { get; set; }

    public bool Add { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
    public bool Export { get; set; }

    public bool Print { get; set; }

}


public class ModuleMasterdetails
{

    public string Desc { get; set; }
    public int Code { get; set; }

    public bool checkStatusall { get; set; }
    public bool checkStatusadd { get; set; }
    public bool checkStatusedit { get; set; }
    public bool checkStatusdelete { get; set; }
    public bool checkStatusquery { get; set; }

    public bool checkStatusprint { get; set; }

}

public class ModuleMastergetuseraccess
{

    public string Desc { get; set; }
    public int Code { get; set; }

    public bool checkStatusall { get; set; }
    public bool checkStatusadd { get; set; }
    public bool checkStatusedit { get; set; }
    public bool checkStatusdelete { get; set; }
    public bool checkStatusquery { get; set; }

    public bool checkStatusprint { get; set; }

}


public class NonModuleMastergetuseraccess
{

    public string Desc { get; set; }
    public int Code { get; set; }

    public bool checkStatusall { get; set; }
    public bool checkStatusadd { get; set; }
    public bool checkStatusedit { get; set; }
    public bool checkStatusdelete { get; set; }
    public bool checkStatusquery { get; set; }

    public bool checkStatusprint { get; set; }

}