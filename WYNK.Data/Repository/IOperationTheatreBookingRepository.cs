
using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IOperationTheatreBookingRepository : IRepositoryBase<OperationTheatreBooking_Viewmodel>

    {
        dynamic GetDetails(string uin);
        dynamic GetDetailsuin(int companyid);
        dynamic BookingDetails(OperationTheatreBooking_Viewmodel opb, string uin, int companyid, int operationid, int userid);
        dynamic GetOperationname(int otid);
        dynamic GetDetails();
    }
}
