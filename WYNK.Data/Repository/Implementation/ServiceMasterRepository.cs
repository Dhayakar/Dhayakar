using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class ServiceMasterRepository : RepositoryBase<ServicesViewModel>, IServiceMasterRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;

        public ServiceMasterRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;
        }
    
        public dynamic Saveservicemasterdata(ServicesViewModel BMI)
        {
            var Data = new ServicesViewModel();
            var Servicesdata = BMI.ServicesGridData;
            try
            {
                var parid = WYNKContext.ServiceMaster.Where(x => x.parentDescription == Convert.ToString(BMI.Parentid)).OrderByDescending(x => x.CreatedUTC).Select(r => r.parentDescription).FirstOrDefault();
                if (parid == null)
                {
                    /////////////////////////////////////////ServicesGridDataExcel//////////////////////////////////////////////////////////
                    if (BMI.ServicesGridDataExcel.Count() > 0)
                    {
                        foreach (var item in BMI.ServicesGridDataExcel.ToList())
                        {

                            int ID = WYNKContext.ServiceMaster.Where(pay => pay.ChildDescription == item.childid
                            && pay.InsuranceID == Convert.ToInt32(item.insuranceid)
                            && pay.DoctorID == Convert.ToInt32(item.docid)
                            && pay.Icdcode == item.Icdcode
                            && pay.RoomID == Convert.ToInt32(item.roomid)
                            && pay.VisitType == item.VisitType).Select(c => c.ID).FirstOrDefault();

                            if (ID == 0)
                            {
                                var ser = new ServiceMaster();
                                ser.ChildDescription = item.childid;
                                ser.parentDescription = Convert.ToString(BMI.Parentid);
                                ser.Tag = Convert.ToString(BMI.Stag);
                                ser.DoctorID = Convert.ToInt32(item.docid == "" ? "0" : item.docid);
                                ser.InsuranceID = Convert.ToInt32(item.insuranceid);
                                ser.RoomID = Convert.ToInt32(item.roomid == "" ? "0" : item.roomid);
                                ser.VisitType = item.VisitType;
                                ser.Icdcode = Convert.ToString(item.Icdcode);
                                ser.IcdSpecialityCode = Convert.ToInt32(item.IcdSpecialityCode);
                                ser.DiscountPercentage = Convert.ToInt32(item.percentage);
                                ser.AmountEligible = Convert.ToDecimal(item.eligibleamt);
                                ser.ServiceCharge = Convert.ToDecimal(item.serviceamt);
                                ser.TotalAmount = Convert.ToDecimal(item.netamount);
                                ser.CreatedUTC = DateTime.UtcNow;
                                ser.CMPID = BMI.cmpid;
                                ser.CreatedBy = BMI.ROLEID;
                                ser.IsActive = true;
                                WYNKContext.ServiceMaster.AddRange(ser);
                                WYNKContext.SaveChanges();
                                item.Status = "Uploaded";
                            }
                            else
                            {
                                item.Status = "Duplicate";
                            }



                        }
                    }
                    //////////////////////////////////////////ServicesGridData/////////////////////////////////////////////////////////
                    if (BMI.ServicesGridData.Count() > 0)
                    {
                        foreach (var item in BMI.ServicesGridData.ToList())
                        {
                            var ser = new ServiceMaster();
                            ser.ChildDescription = item.childid;
                            ser.parentDescription = Convert.ToString(BMI.Parentid);
                            ser.Tag = Convert.ToString(BMI.Stag);
                            ser.DoctorID = Convert.ToInt32(item.docid == "" ? "0" : item.docid);
                            ser.InsuranceID = Convert.ToInt32(item.insuranceid);
                            ser.RoomID = Convert.ToInt32(item.roomid == "" ? "0" : item.roomid);
                            ser.VisitType = item.VisitType;
                            ser.Icdcode = Convert.ToString(item.Icdcode);
                            ser.IcdSpecialityCode = Convert.ToInt32(item.IcdSpecialityCode);
                            ser.DiscountPercentage = Convert.ToInt32(item.percentage);
                            ser.AmountEligible = Convert.ToDecimal(item.eligibleamt);
                            ser.ServiceCharge = Convert.ToDecimal(item.serviceamt);
                            ser.TotalAmount = Convert.ToDecimal(item.netamount);
                            ser.CreatedUTC = DateTime.UtcNow;
                            ser.CMPID = BMI.cmpid;
                            ser.CreatedBy = BMI.ROLEID;
                            ser.IsActive = true;
                            WYNKContext.ServiceMaster.AddRange(ser);
                        }
                    }
                }
                else
                {

                    /////////////////////////////////////////ServicesGridDataExcel//////////////////////////////////////////////////////////
                    if (BMI.ServicesGridDataExcel.Count() > 0)
                    {
                        foreach (var item in BMI.ServicesGridDataExcel.ToList())
                        {

                            int ID = WYNKContext.ServiceMaster.Where(pay => pay.ChildDescription == item.childid
                         && pay.InsuranceID == Convert.ToInt32(item.insuranceid)
                         && pay.DoctorID == Convert.ToInt32(item.docid)
                         && pay.Icdcode == item.Icdcode
                         && pay.RoomID == Convert.ToInt32(item.roomid)
                         && pay.VisitType == item.VisitType).Select(c => c.ID).FirstOrDefault();

                            if (ID == 0)
                            {
                                var fd = new ServiceMaster();
                                if (item.ID != 0)
                                {
                                    fd = WYNKContext.ServiceMaster.Where(x => x.ID == item.ID).FirstOrDefault();
                                    fd.ChildDescription = item.childid;
                                    fd.DoctorID = Convert.ToInt32(item.docid);
                                    fd.InsuranceID = Convert.ToInt32(item.insuranceid);
                                    fd.RoomID = Convert.ToInt32(item.roomid);
                                    fd.VisitType = Convert.ToInt32(item.VisitType);
                                    fd.DiscountPercentage = Convert.ToInt32(item.percentage);
                                    fd.AmountEligible = Convert.ToDecimal(item.eligibleamt);
                                    fd.ServiceCharge = Convert.ToDecimal(item.serviceamt);
                                    fd.Icdcode = Convert.ToString(item.Icdcode);
                                    fd.IcdSpecialityCode = Convert.ToInt32(item.IcdSpecialityCode);
                                    fd.TotalAmount = Convert.ToDecimal(item.netamount);
                                    fd.UpdatedUTC = DateTime.UtcNow;
                                    fd.UpdatedBy = BMI.ROLEID;
                                    fd.IsActive = true;
                                    WYNKContext.ServiceMaster.UpdateRange(fd);
                                    WYNKContext.SaveChanges();
                                    item.Status = "Uploaded";
                                }
                                else
                                {
                                    fd.parentDescription = Convert.ToString(BMI.Parentid);
                                    fd.ChildDescription = item.childid;
                                    fd.Tag = Convert.ToString(BMI.Stag);
                                    fd.DoctorID = Convert.ToInt32(item.docid);
                                    fd.InsuranceID = Convert.ToInt32(item.insuranceid);
                                    fd.RoomID = Convert.ToInt32(item.roomid);
                                    fd.VisitType = Convert.ToInt32(item.VisitType);
                                    fd.DiscountPercentage = Convert.ToInt32(item.percentage);
                                    fd.AmountEligible = Convert.ToDecimal(item.eligibleamt);
                                    fd.ServiceCharge = Convert.ToDecimal(item.serviceamt);
                                    fd.Icdcode = Convert.ToString(item.Icdcode);
                                    fd.IcdSpecialityCode = Convert.ToInt32(item.IcdSpecialityCode);
                                    fd.TotalAmount = Convert.ToDecimal(item.netamount);
                                    fd.CreatedUTC = DateTime.UtcNow;
                                    fd.CreatedBy = BMI.ROLEID;
                                    fd.CMPID = BMI.cmpid;
                                    fd.IsActive = true;
                                    WYNKContext.ServiceMaster.AddRange(fd);
                                    WYNKContext.SaveChanges();
                                    item.Status = "Uploaded";
                                }
                            }
                            else
                            {
                                item.Status = "Duplicate";
                            }
                        }
                    }

                    if (BMI.ServicesGridData.Count() > 0)
                    {
                        foreach (var item in BMI.ServicesGridData.ToList())
                        {
                            var fd = new ServiceMaster();
                            if (item.ID != 0)
                            {
                                fd = WYNKContext.ServiceMaster.Where(x => x.ID == item.ID).FirstOrDefault();
                                fd.ChildDescription = item.childid;
                                fd.DoctorID = Convert.ToInt32(item.docid);
                                fd.InsuranceID = Convert.ToInt32(item.insuranceid);
                                fd.RoomID = Convert.ToInt32(item.roomid);
                                fd.VisitType = Convert.ToInt32(item.VisitType);
                                fd.DiscountPercentage = Convert.ToInt32(item.percentage);
                                fd.AmountEligible = Convert.ToDecimal(item.eligibleamt);
                                fd.ServiceCharge = Convert.ToDecimal(item.serviceamt);
                                fd.Icdcode = Convert.ToString(item.Icdcode);
                                fd.IcdSpecialityCode = Convert.ToInt32(item.IcdSpecialityCode);
                                fd.TotalAmount = Convert.ToDecimal(item.netamount);
                                fd.UpdatedUTC = DateTime.UtcNow;
                                fd.UpdatedBy = BMI.ROLEID;
                                fd.IsActive = true;
                                WYNKContext.ServiceMaster.UpdateRange(fd);
                            }
                            else
                            {
                                fd.parentDescription = Convert.ToString(BMI.Parentid);
                                fd.ChildDescription = item.childid;
                                fd.Tag = Convert.ToString(BMI.Stag);
                                fd.DoctorID = Convert.ToInt32(item.docid);
                                fd.InsuranceID = Convert.ToInt32(item.insuranceid);
                                fd.RoomID = Convert.ToInt32(item.roomid);
                                fd.VisitType = Convert.ToInt32(item.VisitType);
                                fd.DiscountPercentage = Convert.ToInt32(item.percentage);
                                fd.AmountEligible = Convert.ToDecimal(item.eligibleamt);
                                fd.ServiceCharge = Convert.ToDecimal(item.serviceamt);
                                fd.Icdcode = Convert.ToString(item.Icdcode);
                                fd.IcdSpecialityCode = Convert.ToInt32(item.IcdSpecialityCode);
                                fd.TotalAmount = Convert.ToDecimal(item.netamount);
                                fd.CreatedUTC = DateTime.UtcNow;
                                fd.CreatedBy = BMI.ROLEID;
                                fd.CMPID = BMI.cmpid;
                                fd.IsActive = true;
                                WYNKContext.ServiceMaster.AddRange(fd);
                            }
                        }
                    }

                }

                if (BMI.SchDelete != null)
                {
                    if (BMI.SchDelete.Count > 0)
                    {
                        foreach (var item in BMI.SchDelete.ToList())
                        {

                            var mas = WYNKContext.ServiceMaster.Where(x => x.ID == item.ID).FirstOrDefault();
                            mas.IsActive = false;
                            WYNKContext.Entry(mas).State = EntityState.Modified;
                        }
                    }
                }


                WYNKContext.SaveChanges();
                if(WYNKContext.SaveChanges() >= 0)
                {
                    return new
                    {
                        Success = true,
                        Message = "Saved",
                        ServicesGridDataExcel = BMI.ServicesGridDataExcel.ToList()
                    };
                }
            }

            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = "Something Went Wrong"
            };
                
        }


        public dynamic Deleteservicemasterdata(string Pid, string childid, string docid)
        {
            try
            {
                var pdata = WYNKContext.ServiceMaster.Where(x => x.parentDescription == Pid && x.ChildDescription == childid && x.DoctorID == Convert.ToInt32(docid)).FirstOrDefault();
                WYNKContext.ServiceMaster.RemoveRange(pdata);
                WYNKContext.SaveChanges();
                if(WYNKContext.SaveChanges() >= 0)
                {
                    return new
                    {
                        Success = true,                        
                    };
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = "Something Went Wrong"
            };   
        }





        public dynamic InsertRoleVsService(ServicesViewModel AddRoleVsService, int CMPID, int userroleID, int RoleID)
        {
            try
            {

                var RoleVsServices = WYNKContext.RoleVsService.Where(x => x.RoleID == RoleID).ToList();
                if (RoleVsServices.Count != 0)
                {
                    WYNKContext.RoleVsService.RemoveRange(RoleVsServices);
                    WYNKContext.SaveChanges();
                }

                foreach (var item in AddRoleVsService.ServicesMap.ToList())
                {
                    var RoleVsService1 = new RoleVsService();

                    RoleVsService1.ServiceID = item.ServiceID;
                    RoleVsService1.RoleID = item.RoleID;
                    RoleVsService1.CMPID = CMPID;
                    RoleVsService1.CreatedBy = userroleID;
                    RoleVsService1.CreatedUTC = DateTime.UtcNow;
                    WYNKContext.RoleVsService.AddRange(RoleVsService1);
                }
                WYNKContext.SaveChanges();
                return new
                {
                    Success = true,
                    Message = "Saved successfully",
                };
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


        public ServicesViewModel GetServiceDetails(int RoleName, int CMPID)
        {
            var RoleVsServiceModel = new ServicesViewModel();
            var RoleVsService = WYNKContext.RoleVsService.Where(x => x.RoleID == RoleName && x.CMPID == CMPID).ToList();
            var Services = WYNKContext.Services.Where(x => x.ParentID == 0 && x.CMPID == CMPID).ToList();
            var Orgsample = (from g in RoleVsService
                             select new
                             {
                                 Id = g.ID,
                                 ServiceID = g.ServiceID,
                                // RoleID = g.RoleID,
                                 CB = true,
                             }).ToList();

            var Orgdataq = (from d in Services
                            where RoleVsService.All(a => a.ServiceID != d.ID)
                            select new
                            {
                                Id = d.ID,
                                ServiceID = d.ID,
                               // RoleID = d.RoleID,
                                CB = false,
                            }).ToList();
            var mergeddata = Orgsample.Concat(Orgdataq);

            RoleVsServiceModel.GetRoleVsServiceschecked = (from oo in Orgsample
                                        select new GetRoleVsServiceschecked
                                        {
                                           
                                            ServiceID = oo.ServiceID,
                                            RoleID = RoleName,
                                            checkeds = oo.CB,

                                        }).ToList();
            RoleVsServiceModel.GetRoleVsServices = (from gg in mergeddata
                                                                      select new GetRoleVsServices
                                                                      {
                                                                          ID = gg.Id,
                                                                          ServiceID = gg.ServiceID,
                                                                          ServicesName = Services.Where(x=>x.ID == gg.ServiceID).Select(c=>c.Description).FirstOrDefault(),
                                                                          RoleID = RoleName,
                                                                          checkeds = gg.CB,
                                                                      }).ToList();


            return RoleVsServiceModel;
        }

    }
}
