using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IOvertimeRepository : IRepositoryBase<OvertimeViewM>
    {
        dynamic insertOTDet(OvertimeViewM OTview);
        OvertimeViewM getEmployeeName();
        dynamic getAddHistoryDet(DateTime FromDate, DateTime Todate, int EmpId);
        dynamic getHistoryDet(DateTime FromDate, DateTime Todate, int EmpId);

        dynamic UpdateOvertimeDet(OvertimeViewM overtimeMaster, int ID, int O_Id);

        dynamic DeleteOTDet(int ID);

    }
}
