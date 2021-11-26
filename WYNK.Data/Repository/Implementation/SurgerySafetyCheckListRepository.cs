using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class SurgerySafetyCheckListRepository : RepositoryBase<SurgeryCheckListViewModel>, ISurgerySafetyCheckListRepository
    {
        

          private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
       

        public SurgerySafetyCheckListRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic GetSurgerySafetyChecklist(int CMPID)
        {
            var onelinemaster = CMPSContext.OneLineMaster.ToList();

            return (from e in WYNKContext.SurgerySafetyCheckList.Where(x => x.CmpID == CMPID && x.IsActive == true)
                    select new 
                    {
                      SSCID =e.SSCID,
                      Question =e.Question,
                      Questiontowhom=e.Questiontowhom,
                      SSCGroupDescription= onelinemaster.Where(x=>x.OLMID == e.SSCGroupDescription).Select(x=>x.ParentDescription).FirstOrDefault(),
                      DefaultValue=e.DefaultValue,
                      DefaultDescription = e.DefaultValue != null ?Enum.GetName(typeof(Answers), e.DefaultValue): " ",
                    }).ToList();

        }

        public dynamic DeleteSSCdetail(int CMPID,int id)
        {

            try
            {
                var SScDetail = WYNKContext.SurgerySafetyCheckList.Where(x => x.CmpID == CMPID && x.SSCID == id).FirstOrDefault();
                SScDetail.IsActive = false;
                SScDetail.IsDeleted = true;
                WYNKContext.SurgerySafetyCheckList.UpdateRange(SScDetail);
                WYNKContext.SaveChanges();
                return new
                {
                    Success = true,
                    Message = "Delete successfully"
                };
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

        public dynamic SubmitSurgerySafetyChecklist(SurgeryCheckListViewModel SubmitSurgerySafetyChecklist)
        {

            try
            {
                var SurgerySafetyCheckList = new SurgerySafetyCheckList();
                SurgerySafetyCheckList.SSCGroupDescription = SubmitSurgerySafetyChecklist.SurgerySafetyCheckList.SSCGroupDescription;
                SurgerySafetyCheckList.Question = SubmitSurgerySafetyChecklist.SurgerySafetyCheckList.Question;
                SurgerySafetyCheckList.Questiontowhom = SubmitSurgerySafetyChecklist.SurgerySafetyCheckList.Questiontowhom;
                SurgerySafetyCheckList.DefaultValue = SubmitSurgerySafetyChecklist.SurgerySafetyCheckList.DefaultValue;
                SurgerySafetyCheckList.CmpID = SubmitSurgerySafetyChecklist.SurgerySafetyCheckList.CmpID;
                SurgerySafetyCheckList.IsActive = true;
                SurgerySafetyCheckList.IsDeleted = false;
                SurgerySafetyCheckList.CreatedBy = SubmitSurgerySafetyChecklist.SurgerySafetyCheckList.CreatedBy;
                SurgerySafetyCheckList.CreatedUTC = DateTime.UtcNow;

                WYNKContext.SurgerySafetyCheckList.Add(SurgerySafetyCheckList);

                WYNKContext.SaveChanges();
                return new
                {
                    Success = true,
                    Message = "Saved successfully"
                };
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

        public dynamic UpdateSScDetails(SurgeryCheckListViewModel SubmitSurgerySafetyChecklist, int id)
        {
            try
            {
                var SurgerySafetyCheckList = WYNKContext.SurgerySafetyCheckList.Where(x => x.SSCID == id).FirstOrDefault();

                SurgerySafetyCheckList.SSCGroupDescription = SubmitSurgerySafetyChecklist.SurgerySafetyCheckList.SSCGroupDescription;
                SurgerySafetyCheckList.Question = SubmitSurgerySafetyChecklist.SurgerySafetyCheckList.Question;
                SurgerySafetyCheckList.Questiontowhom = SubmitSurgerySafetyChecklist.SurgerySafetyCheckList.Questiontowhom;
                SurgerySafetyCheckList.DefaultValue = SubmitSurgerySafetyChecklist.SurgerySafetyCheckList.DefaultValue;
                SurgerySafetyCheckList.IsActive = true;
                SurgerySafetyCheckList.IsDeleted = false;
                SurgerySafetyCheckList.UpdatedBy = SubmitSurgerySafetyChecklist.SurgerySafetyCheckList.CreatedBy;
                SurgerySafetyCheckList.UpdatedUTC = DateTime.UtcNow;

                WYNKContext.SurgerySafetyCheckList.Update(SurgerySafetyCheckList);

                WYNKContext.SaveChanges();

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
            return new
            {
                Success = false,
                Message = "Something Went Wrong"
            };
        }

        public dynamic PreviousSSCGroupdesc(int cMPID, int value)
        {
            try
            {
                var SScDetail = WYNKContext.SurgerySafetyCheckList.Where(x => x.CmpID == cMPID && x.SSCGroupDescription == value && x.IsActive == true).LastOrDefault();


                if(SScDetail != null)
                {
                    return new
                    {
                        Success = true,
                        QuestionToWhom = SScDetail.Questiontowhom,
                    };

                }

                else
                {
                    return new
                    {
                        Success = true,
                        QuestionToWhom = " ",
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
    }
}













