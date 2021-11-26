using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Common;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;


namespace WYNK.Data.Repository.Implementation
{
    class OvertimeRepository : RepositoryBase<OvertimeViewM>, IOvertimeRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public OvertimeRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public OvertimeViewM getEmployeeName()
        {
            var getData = new OvertimeViewM();

            var M_Employee = CMPSContext.Employee.ToList();
            var M_EmployeeCommunication = CMPSContext.EmployeeCommunication.ToList();


            getData.OvertimeDet = new List<OvertimeDet>();

            getData.getEmployeeDet = (from M_Emp in M_Employee.Where(x => x.IsDeleted == false && x.DOR == null && x.IsActive == true)

                                      join M_EmpComm in M_EmployeeCommunication
                                      on M_Emp.EmployeeID equals M_EmpComm.EmpID

                                      select new getEmployeeDet
                                      {
                                          FirstName = M_Emp.FirstName,
                                          LastName = M_Emp.LastName,

                                          Designation = M_Emp.Designation,
                                          DOJ = M_Emp.DOJ,
                                          Gender = Enum.GetName(typeof(Genderr), M_Emp.Gender),
                                          PresentAddress = M_EmpComm.PresentAddress1,
                                          Emp_ID = M_Emp.EmployeeID,




                                      }).ToList();

            return getData;
        }


        public dynamic insertOTDet(OvertimeViewM OTview)
        {
            try
            {
                if (OTview.OvertimeDet.Count > 0)
                {
                    foreach (var item in OTview.OvertimeDet.ToList())
                    {
                        var getOT = new OverTime();

                        getOT.EmpID = OTview.OverTime.EmpID;
                        getOT.FromTime = item.FromTime;
                        getOT.ToTime = item.ToTime;
                        getOT.NoofHours = item.NoofHours;
                        getOT.OTDate = item.OTDate.AddDays(1);
                        getOT.OTperHour = item.OTperHour;
                        getOT.TotalOTAmount = item.TotalOTAmount;
                        getOT.CMPID = OTview.OverTime.CMPID;
                        getOT.CreatedUTC = DateTime.UtcNow;
                        getOT.CreatedBy = OTview.OverTime.CreatedBy;
                        WYNKContext.OverTime.AddRange(getOT);
                        WYNKContext.SaveChanges();

                    }

                }

                try
                {
                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,

                            Message = CommonRepository.saved

                        };
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = CommonRepository.Missing

            };
        }


        public dynamic getAddHistoryDet(DateTime FromDate, DateTime Todate, int EmpId)
        {
            var getData = new OvertimeViewM();

            var M_OT = WYNKContext.OverTime.ToList();
            var M_Emp = CMPSContext.Employee.ToList();
            var Fdate = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            var Tdate = Convert.ToDateTime(Todate).ToString("yyyy-MM-dd");


            getData.getAddHistoryDet = (from OT in M_OT.Where(x => Convert.ToDateTime(x.OTDate).Date >= Convert.ToDateTime(Fdate) && Convert.ToDateTime(x.OTDate).Date <= Convert.ToDateTime(Tdate)
                                       && x.IsDeleted == false)
                                        join Emp in M_Emp.Where(x => x.EmployeeID == EmpId)
                                        on OT.EmpID equals Emp.EmployeeID
                                        select new getAddHistoryDet
                                        {
                                            OTDate = OT.OTDate,
                                            FromTime = OT.FromTime,
                                            ToTime = OT.ToTime,
                                            NoofHours = OT.NoofHours,
                                            OTperHour = OT.OTperHour,
                                            TotalOTAmount = OT.TotalOTAmount
                                        }).ToList();

            return getData;
        }


        public dynamic getHistoryDet(DateTime FromDate, DateTime Todate, int EmpId)
        {
            var getData = new OvertimeViewM();
            var M_OT = WYNKContext.OverTime.ToList();
            var M_Emp = CMPSContext.Employee.ToList();

            var Fdate = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            var Tdate = Convert.ToDateTime(Todate).ToString("yyyy-MM-dd");

            getData.getHistoryDet = (from OT in M_OT.Where(x => Convert.ToDateTime(x.OTDate).Date >= Convert.ToDateTime(Fdate) && Convert.ToDateTime(x.OTDate).Date <= Convert.ToDateTime(Tdate)
                                       && x.IsDeleted == false)
                                     join Emp in M_Emp.Where(x => x.EmployeeID == EmpId)
                                     on OT.EmpID equals Emp.EmployeeID
                                     select new getHistoryDet
                                     {
                                         ID1 = OT.ID,
                                         OTDate1 = OT.OTDate,
                                         FromTime1 = OT.FromTime,
                                         ToTime1 = OT.ToTime,
                                         NoofHours1 = OT.NoofHours,
                                         OTperHour1 = OT.OTperHour,
                                         TotalOTAmount1 = OT.TotalOTAmount,
                                         EmpID1 = Emp.EmployeeID

                                     }).ToList();


            return getData;
        }

        public dynamic UpdateOvertimeDet(OvertimeViewM overtimeMaster, int ID, int O_Id)
        {

            try
            {
                var M_updateOTDet = new OverTime();
                M_updateOTDet = WYNKContext.OverTime.Where(x => x.EmpID == ID && x.ID == O_Id).FirstOrDefault();
                M_updateOTDet.OTDate = overtimeMaster.OverTime.OTDate;
                M_updateOTDet.FromTime = overtimeMaster.OverTime.FromTime;
                M_updateOTDet.ToTime = overtimeMaster.OverTime.ToTime;
                M_updateOTDet.NoofHours = overtimeMaster.OverTime.NoofHours;
                M_updateOTDet.OTperHour = overtimeMaster.OverTime.OTperHour;
                M_updateOTDet.TotalOTAmount = overtimeMaster.OverTime.TotalOTAmount;
                M_updateOTDet.UpdatedBy = overtimeMaster.OverTime.UpdatedBy;
                M_updateOTDet.UpdatedUTC = DateTime.UtcNow;

                WYNKContext.OverTime.UpdateRange(M_updateOTDet);



                try
                {
                    if (WYNKContext.SaveChanges() > 0)
                        return new
                        {
                            Success = true,
                            Message = "Updated successfully"
                        };
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = "Some data are Missing"
            };
        }


        public dynamic DeleteOTDet(int ID)
        {
            try
            {
                var OvertimeMaster = WYNKContext.OverTime.Where(x => x.ID == ID).ToList();

                if (OvertimeMaster != null)
                {
                    OvertimeMaster.All(x => { x.IsDeleted = true; return true; });
                    WYNKContext.OverTime.UpdateRange(OvertimeMaster);
                }

                try
                {
                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Message = CommonRepository.saved
                        };
                }

                catch (Exception ex)
                {
                    Console.Write(ex);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = CommonRepository.Missing
            };


        }

    }


}
