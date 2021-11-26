using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Common;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;
namespace WYNK.Data.Repository.Implementation
{
    class TaxMasterRepository : RepositoryBase<TaxMasterViewM>, ITaxMasterRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        
        

        public TaxMasterRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic insertTaxMaster(TaxMasterViewM taxMaster,int CMPID)
        {

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var M_taxMaster = new TaxMaster();
                    M_taxMaster.TaxDescription = taxMaster.taxMaster.TaxDescription;
                    M_taxMaster.GSTPercentage = taxMaster.taxMaster.GSTPercentage;
                    M_taxMaster.CGSTPercentage = taxMaster.taxMaster.CGSTPercentage;
                    M_taxMaster.SGSTPercentage = taxMaster.taxMaster.SGSTPercentage;
                    M_taxMaster.IGSTPercentage = taxMaster.taxMaster.IGSTPercentage;
                    M_taxMaster.CESSDescription = taxMaster.taxMaster.CESSDescription;
                    M_taxMaster.CESSPercentage = taxMaster.taxMaster.CESSPercentage;
                    M_taxMaster.AdditionalCESSDescription = taxMaster.taxMaster.AdditionalCESSDescription;
                    M_taxMaster.AdditionalCESSPercentage = taxMaster.taxMaster.AdditionalCESSPercentage;
                    M_taxMaster.CreatedBy = taxMaster.taxMaster.CreatedBy;
                    M_taxMaster.TaxGroupId = taxMaster.taxMaster.TaxGroupId;
                    M_taxMaster.IsActive = true;
                    M_taxMaster.istransact = true;
                    M_taxMaster.CreatedUTC = DateTime.UtcNow;
                    CMPSContext.TaxMaster.AddRange(M_taxMaster);

                    string cmpname = CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.CompanyName).FirstOrDefault();
                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == taxMaster.taxMaster.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    string userid = Convert.ToString(taxMaster.taxMaster.CreatedBy);
                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(cmpname, "TaxMaster", "User name :", username, "User ID :", Convert.ToString(userid), "Mode : Add");
                    object names = M_taxMaster;
                    oErrorLogs.WriteErrorLogArray("TaxMaster", names);

                    CMPSContext.SaveChanges();
                    if (CMPSContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }

                    dbContextTransaction.Commit();
                    return new
                    {
                        Success = true,
                        Message = "Saved successfully",
                    };
                }
                catch (Exception ex)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
                return new
                {
                    Success = false,
                    Message = CommonRepository.Missing
                };
               
            }
        
        
        
        }





        public dynamic updateTaxMaster(TaxMasterViewM taxMaster, int ID, int CMPID)
        {

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {

                    var M_taxMaster = new TaxMaster();
                    M_taxMaster = CMPSContext.TaxMaster.Where(x => x.ID == ID).FirstOrDefault();
                    M_taxMaster.TaxDescription = taxMaster.taxMaster.TaxDescription;
                    M_taxMaster.GSTPercentage = taxMaster.taxMaster.GSTPercentage;
                    M_taxMaster.CGSTPercentage = taxMaster.taxMaster.CGSTPercentage;
                    M_taxMaster.SGSTPercentage = taxMaster.taxMaster.SGSTPercentage;
                    M_taxMaster.IGSTPercentage = taxMaster.taxMaster.IGSTPercentage;
                    M_taxMaster.CESSDescription = taxMaster.taxMaster.CESSDescription;
                    M_taxMaster.CESSPercentage = taxMaster.taxMaster.CESSPercentage;
                    M_taxMaster.AdditionalCESSDescription = taxMaster.taxMaster.AdditionalCESSDescription;
                    M_taxMaster.AdditionalCESSPercentage = taxMaster.taxMaster.AdditionalCESSPercentage;
                    M_taxMaster.UpdatedBy = taxMaster.taxMaster.UpdatedBy;
                    M_taxMaster.TaxGroupId = taxMaster.taxMaster.TaxGroupId;
                    M_taxMaster.IsActive = taxMaster.taxMaster.IsActive;
                    M_taxMaster.UpdatedUTC = DateTime.UtcNow;
                    CMPSContext.TaxMaster.UpdateRange(M_taxMaster);

                    string cmpname = CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.CompanyName).FirstOrDefault();
                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == taxMaster.taxMaster.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    string userid = Convert.ToString(taxMaster.taxMaster.CreatedBy);
                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(cmpname, "TaxMaster", "User name :", username, "User ID :", Convert.ToString(userid), "Mode : update");
                    object names = M_taxMaster;
                    oErrorLogs.WriteErrorLogArray("TaxMaster", names);

                    CMPSContext.SaveChanges();
                    if (CMPSContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }

                    dbContextTransaction.Commit();
                    return new
                    {
                        Success = true,
                        Message = "Saved successfully",
                    };
                }
                catch (Exception ex)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
                return new
                {
                    Success = false,
                    Message = "Some data are Missing"
                };

            }



        }

    }
}
