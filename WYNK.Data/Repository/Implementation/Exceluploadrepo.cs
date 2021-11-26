
using System;
using System.Collections.Generic;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class Exceluploadrepo : RepositoryBase<ExcelViewModel>, IExceluploadrepo
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;

        public Exceluploadrepo(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic GetStatusDrugStock(DrugStockExcel DrugList, int CmpId)
        {

            var Drugmaster = WYNKContext.DrugMaster.ToList();

            var DrugDatas = (from druglist in DrugList.DrugStockExcelimports
                             let DrugTracker = Enum.GetName(typeof(TrackingType), WYNKContext.DrugMaster.Where(x => x.ID == druglist.DrugID).Select(x => x.DrugTracker).FirstOrDefault())
                             select new
                             {
                                 DrugID = druglist.DrugID,
                                 Brand = Drugmaster.Where(x => x.ID == druglist.DrugID).Select(x => x.Brand).FirstOrDefault(),
                                 Generic = WYNKContext.DrugGroup.Where(x => x.ID == Drugmaster.Where(y => y.ID == druglist.DrugID).Select(y => y.GenericName).FirstOrDefault()).Select(x => x.Description).FirstOrDefault(),
                                 BatchSerial = druglist.BatchSerial,
                                 Date = DrugTracker != "None" ? druglist.Date != null ? Convert.ToDateTime(druglist.Date).AddDays(1) : druglist.Date : null,
                                 Quantity = druglist.Quantity,
                                 Remarks = RemarksCheck(DrugTracker, druglist.Quantity, Convert.ToDateTime(druglist.Date).AddDays(1)),
                                 Status = StatusCheck(DrugTracker, druglist.Quantity, Convert.ToDateTime(druglist.Date).AddDays(1)),
                             }).ToList();

            if (DrugDatas.Count > 0)
            {
                return new
                {
                    Success = true,
                    Message = "Invalid Records",
                    DrugDatas = DrugDatas,
                };
            }
            else
            {
                return new
                {
                    Success = false,
                    Message = "No valid Records",
                };
            }

        }

        public string RemarksCheck(string DrugTracker, decimal Quantity, DateTime ExpiryDate)
        {
            string Remarks = "";
            if (DrugTracker == "SerialNumberBased")
            {
                var IsExpiryValid = ExpiryDate < DateTime.Now ? false : true;
                var IsQuantityValid = Quantity <= 0 || Quantity > 1 ? false : true;
                if (!IsExpiryValid)
                {
                    Remarks = "Invalid Expiry Date";
                }
                if (!IsQuantityValid)
                {
                    if (Remarks == "")
                    {
                        Remarks = "Invalid Qty";
                    }
                    else
                    {
                        Remarks += " ,Invalid Qty";
                    }
                }
            }
            else if (DrugTracker == "BatchNumberBased")
            {
                var IsExpiryValid = ExpiryDate < DateTime.Now ? false : true;
                var IsQuantityValid = Quantity <= 0 ? false : true;
                if (!IsExpiryValid)
                {
                    Remarks = "Invalid Expiry Date";
                }
                if (!IsQuantityValid)
                {
                    if (Remarks == "")
                    {
                        Remarks = "Invalid Qty";
                    }
                    else
                    {
                        Remarks += ", Invalid Qty";
                    }
                }
            }
            else
            {
                var IsQuantityValid = Quantity <= 0 ? false : true;
                if (!IsQuantityValid)
                {
                    Remarks = "Invalid Qty";
                }
            }
            return Remarks;
        }

        public string StatusCheck(string DrugTracker, decimal Quantity, DateTime ExpiryDate)
        {
            string status = "";
            if (DrugTracker == "SerialNumberBased")
            {
                var IsExpiryValid = ExpiryDate < DateTime.Now ? false : true;
                var IsQuantityValid = Quantity <= 0 || Quantity > 1 ? false : true;
                if (IsExpiryValid && IsQuantityValid)
                {
                    status = "Valid";
                }
                else
                {
                    status = "Invalid";
                }
            }
            else if (DrugTracker == "BatchNumberBased")
            {
                var IsExpiryValid = ExpiryDate < DateTime.Now ? false : true;
                var IsQuantityValid = Quantity <= 0 ? false : true;
                if (IsExpiryValid && IsQuantityValid)
                {
                    status = "Valid";
                }
                else
                {
                    status = "Invalid";
                }
            }
            else
            {
                var IsQuantityValid = Quantity <= 0 ? false : true;
                if (IsQuantityValid)
                {
                    status = "Valid";
                }
                else
                {
                    status = "Invalid";
                }
            }
            return status;
        }

        public dynamic UpdateDrugExceldata(ExcelViewModel UpdateExceldatas, int CmpId, int Createdby)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                if (UpdateExceldatas.DrugExcelimport.Count > 0)
                {
                    IList<DrugExcelimport> DuplicateDrugs = new List<DrugExcelimport>();

                    int Uploaded = 0;
                    int Duplicate = 0;
                    int Error = 0;


                    string cmpname = CMPSContext.Company.Where(x => x.CmpID == CmpId).Select(x => x.CompanyName).FirstOrDefault();
                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == Createdby).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    string userid = Convert.ToString(Createdby);
                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(cmpname, "Drug Excel Upload", "User name :", username, "User ID :", userid, "Mode : Add");

                    foreach (var Drug in UpdateExceldatas.DrugExcelimport.ToList())
                    {


                        try
                        {

                            int? IsManufacturer = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == Drug.Manufacturer && x.ParentTag == "Brand").Select(x => (int?)x.OLMID).FirstOrDefault();

                            /*Manufacturer Insertion and assign to IsManufacturer*/
                            if (IsManufacturer == null)
                            {
                                var onelinemaster = new OneLine_Masters();
                                onelinemaster.ParentDescription = Drug.Manufacturer;
                                onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Brand").Select(x => x.OLMID).FirstOrDefault();
                                onelinemaster.ParentTag = "Brand";
                                onelinemaster.IsDeleted = false;
                                onelinemaster.IsActive = true;
                                onelinemaster.CreatedUTC = DateTime.UtcNow;
                                onelinemaster.CreatedBy = Createdby;
                                CMPSContext.OneLineMaster.Add(onelinemaster);
                                CMPSContext.SaveChanges();

                                IsManufacturer = onelinemaster.OLMID;
                            }


                            int? IsUOM = CMPSContext.uommaster.Where(x => x.Description == Drug.UOM).Select(x => (int?)x.id).FirstOrDefault();

                            /*UOM Insertion and assign to IsManufacturer*/
                            if (IsUOM == null)
                            {
                                var UOms = new UOM_Master();
                                UOms.Description = Drug.UOM;
                                UOms.CreatedUTC = DateTime.UtcNow;
                                UOms.CreatedBy = Createdby;
                                CMPSContext.uommaster.Add(UOms);
                                CMPSContext.SaveChanges();
                            }


                            int? IsGenericName = WYNKContext.DrugGroup.Where(x => x.Description == Drug.GenericName && x.Cmpid == CmpId).Select(x => (int?)x.ID).FirstOrDefault();

                            /*GenericName Insertion*/
                            if (IsGenericName == null)
                            {
                                int? IsDrugGroup = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == Drug.DrugGroup).Select(x => (int?)x.OLMID).FirstOrDefault();

                                if (IsDrugGroup == null)
                                {
                                    var onelinemaster = new OneLine_Masters();
                                    onelinemaster.ParentDescription = Drug.DrugGroup;
                                    onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Form").Select(x => x.OLMID).FirstOrDefault();
                                    onelinemaster.ParentTag = "Form";
                                    onelinemaster.IsDeleted = false;
                                    onelinemaster.IsActive = true;
                                    onelinemaster.CreatedUTC = DateTime.UtcNow;
                                    onelinemaster.CreatedBy = Createdby;
                                    CMPSContext.OneLineMaster.Add(onelinemaster);
                                    CMPSContext.SaveChanges();

                                    var DrugGroup = new Drug_Group();
                                    DrugGroup.Description = Drug.GenericName;
                                    DrugGroup.DrugFormID = onelinemaster.OLMID;
                                    DrugGroup.IsStepDown = false;
                                    DrugGroup.IsDeleted = false;
                                    DrugGroup.RetestInterval = 10;
                                    DrugGroup.RetestCriticalInterval = 20;
                                    DrugGroup.CreatedUTC = DateTime.UtcNow;
                                    DrugGroup.CreatedBy = Createdby;
                                    DrugGroup.Cmpid = CmpId;
                                    WYNKContext.DrugGroup.Add(DrugGroup);
                                    CMPSContext.SaveChanges();
                                    IsGenericName = DrugGroup.ID;
                                }
                                else
                                {
                                    var DrugGroups = new Drug_Group();
                                    DrugGroups.Description = Drug.GenericName;
                                    DrugGroups.DrugFormID = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "Form" && x.ParentDescription == Drug.DrugGroup).Select(x => x.OLMID).FirstOrDefault();
                                    DrugGroups.IsStepDown = false;
                                    DrugGroups.IsDeleted = false;
                                    DrugGroups.RetestInterval = 10;
                                    DrugGroups.RetestCriticalInterval = 20;
                                    DrugGroups.CreatedUTC = DateTime.UtcNow;
                                    DrugGroups.CreatedBy = Createdby;
                                    DrugGroups.Cmpid = CmpId;
                                    WYNKContext.DrugGroup.Add(DrugGroups);
                                    WYNKContext.SaveChanges();
                                    IsGenericName = DrugGroups.ID;
                                }
                            }


                            /*Checking Brand and GenericName*/
                            var IsDrugFound = WYNKContext.DrugMaster.Where(x => x.Brand == Drug.Brand && x.GenericName == IsGenericName && x.Cmpid == CmpId).FirstOrDefault();




                            /*Drug Not && Insert into DrugMaster */
                            if (IsDrugFound == null)
                            {
                                var Drugmaster = new Drug_Master();
                                Drugmaster.Brand = Drug.Brand;
                                Drugmaster.Manufacturer = Convert.ToInt32(IsManufacturer);
                                Drugmaster.GenericName = Convert.ToInt32(IsGenericName);
                                Drugmaster.UOM = Drug.UOM;
                                Drugmaster.DrugGroup = Drug.DrugGroup;
                                Drugmaster.Rate = Convert.ToDecimal(Drug.Rate);
                                Drugmaster.HSNNo = Drug.HSNO;
                                Drugmaster.DrugSubDescription = Drug.DrugSubDescription;
                                Drugmaster.Aconstant = Drug.AConstant;
                                Drugmaster.OpticDia = Drug.OpticDia;
                                Drugmaster.ModelNo = Drug.ModelNo;
                                Drugmaster.Length = Drug.Length;
                                Drugmaster.DrugComposition = Drug.DrugComposition;

                                if (Drug.DrugCategory == "IMPLANTDRUG")
                                {
                                    Drugmaster.DrugCategory = 1;

                                }
                                else if (Drug.DrugCategory == "NONIMPLANTDRUG")
                                {
                                    Drugmaster.DrugCategory = 2;
                                }
                                else
                                {
                                    Drugmaster.DrugCategory = 3;
                                }

                                if (Drug.DrugTracker == "SERIAL NUMBER BASED")
                                {
                                    Drugmaster.DrugTracker = 0;

                                }
                                else if (Drug.DrugTracker == "BATCH NUMBER BASED")
                                {
                                    Drugmaster.DrugTracker = 1;
                                }
                                else
                                {
                                    Drugmaster.DrugTracker = 2;
                                }

                                Drugmaster.IsActive = true;
                                Drugmaster.IsDeleted = false;
                                Drugmaster.Cmpid = CmpId;
                                Drugmaster.CreatedUTC = DateTime.UtcNow;
                                Drugmaster.CreatedBy = Createdby;

                                WYNKContext.DrugMaster.Add(Drugmaster);

                                object names = Drugmaster;
                                oErrorLogs.WriteErrorLogArray("DrugMaster", names);

                                WYNKContext.SaveChanges();


                                Drug.Status = "Uploaded";
                                Uploaded = Uploaded + 1;
                            }
                            /*Drug Already in DrugMaster and Logging in Loggers as Duplicate*/
                            else
                            {
                                DuplicateDrugs.Add(Drug);

                                object names = Drug;
                                oErrorLogs.WriteErrorLogArray("Duplicate DrugMaster Record", names);
                                Drug.Status = "Duplicate";
                                Duplicate = Duplicate + 1;
                            }

                        }

                        catch (Exception ex)
                        {
                            object names = Drug;
                            oErrorLogs.ErrorLogArrayDrug("Error DrugMaster Record", ex.InnerException.Message.ToString(), names);
                            Drug.Status = "Error";
                            Error = Error + 1;
                            continue;
                        }


                    }
                    dbContextTransaction.Commit();
                    return new
                    {
                        Success = true,
                        Message = "Saved successfully",
                        DrugDetails = UpdateExceldatas.DrugExcelimport.ToList(),
                        Uploaded = Uploaded,
                        Duplicate = Duplicate,
                        Error = Error,
                    };

                }
                else
                {
                    return new
                    {
                        Success = false,
                        Message = "No Files To Upload",
                    };

                }
            }
        }



        public dynamic UpdateStrabismusdata(ExcelViewModel UpdateExceldatass, int CmpId, int Createdby)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                if (UpdateExceldatass.Strabismusimport.Count > 0)
                {
                    IList<Strabismusimport> DuplicateDrugs = new List<Strabismusimport>();

                    int Uploaded = 0;
                    int Duplicate = 0;
                    int Error = 0;


                    string cmpname = CMPSContext.Company.Where(x => x.CmpID == CmpId).Select(x => x.CompanyName).FirstOrDefault();
                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == Createdby).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    string userid = Convert.ToString(Createdby);
                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(cmpname, "Strabismus Upload", "User name :", username, "User ID :", userid, "Mode : Add");

                    foreach (var Drug in UpdateExceldatass.Strabismusimport.ToList())
                    {


                        try
                        {
                            var sqm = new Squint_ExtnMaster();
                            sqm.ParentDescription = Drug.ParentDescription;
                            sqm.ParentID = 1;
                            sqm.ParentTag = Drug.ParentTag;
                            sqm.IsActive = true;
                            sqm.IsDeleted = false;
                            sqm.CmpID = CmpId;
                            sqm.CreatedBy = Createdby;
                            sqm.CreatedUTC = DateTime.UtcNow;
                            WYNKContext.SquintExtnMaster.AddRange(sqm);
                            WYNKContext.SaveChanges();

                        }

                        catch (Exception ex)
                        {
                            object names = Drug;
                            oErrorLogs.ErrorLogArrayDrug("Error Record", ex.InnerException.Message.ToString(), names);
                            Drug.Status = "Error";
                            Error = Error + 1;
                            continue;
                        }


                    }
                    dbContextTransaction.Commit();
                    return new
                    {
                        Success = true,
                        Message = "Saved successfully",
                        DrugDetails = UpdateExceldatass.Strabismusimport.ToList(),
                        Uploaded = Uploaded,
                        Duplicate = Duplicate,
                        Error = Error,
                    };

                }
                else
                {
                    return new
                    {
                        Success = false,
                        Message = "No Files To Upload",
                    };

                }
            }
        }




        public static DateTime CreateDateFromTime(int year, int month, int day, DateTime time)
        {
            return new DateTime(year, month, day, time.Hour, time.Minute, 0);
        }

        public dynamic UpdateExceldata(ExcelViewModel UpdateExceldata, int CmpId)
        {
            var Newmodel = new ExcelViewModel();
            Newmodel.ErrorExcelimport = new List<ErrorRegistrationExcel>();
            var registrationMaster = new Registration_Master();
            registrationMaster.CMPID = CmpId;
            var Regdata = UpdateExceldata.Excelimport;

            int Uploaded = 0;
            int Duplicate = 0;
            int Error = 0;

            if (Regdata.Count > 0)
            {

                foreach (var item in Regdata)
                {
                    try
                    {
                        var uindata = WYNKContext.Registration.Where(x => x.OLDUIN == item.UIN).FirstOrDefault();
                        if (uindata != null)
                        {
                            var Errordata = new ErrorRegistrationExcel();
                            Errordata.Address1 = item.Address1;
                            Errordata.UIN = item.UIN;
                            Errordata.Name = item.Name;
                            Errordata.MiddleName = item.MiddleName;
                            Errordata.LastName = item.LastName;
                            Errordata.DateofRegistration = item.DateofRegistration;
                            Errordata.DateofBirth = item.DateofBirth;
                            Errordata.Phone = item.Phone;
                            Errordata.Fees = item.Fees;
                            Errordata.Status = "Duplicate UIN Data";
                            Newmodel.ErrorExcelimport.Add(Errordata);
                            Duplicate = Duplicate + 1;
                        }
                        else
                        {

                            var closedvalue = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Closed" && x.ParentTag == "RegStatus").Select(x => x.OLMID).FirstOrDefault();
                            var openvaluevalue = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New" && x.ParentTag == "TOV").Select(x => x.OLMID).FirstOrDefault();


                            var rn = CMPSContext.NumberControl.Where(x => x.CmpID == CmpId && x.Description == "Registration").FirstOrDefault();
                            if (rn.RunningNumber == 0)
                            {
                                registrationMaster.UIN = rn.Prefix + 1;
                                rn.RunningNumber = 1;

                            }
                            else
                            {
                                var num = rn.RunningNumber + 1;
                                rn.RunningNumber = num;
                                registrationMaster.UIN = rn.Prefix + num;

                            }
                            registrationMaster.OLDUIN = item.UIN;
                            registrationMaster.Name = item.Name;
                            registrationMaster.LastName = item.LastName;
                            registrationMaster.MiddleName = item.MiddleName;

                            DateTime time = DateTime.Now;
                            var dd = item.DateofRegistration.Split(new string[] { "-" }, 3, StringSplitOptions.None)[0];
                            var mm = item.DateofRegistration.Split(new string[] { "-" }, 3, StringSplitOptions.None)[1];
                            var yy = item.DateofRegistration.Split(new string[] { "-" }, 3, StringSplitOptions.None)[2];

                            var ddd = item.DateofBirth.Split(new string[] { "-" }, 3, StringSplitOptions.None)[0];
                            var dmm = item.DateofBirth.Split(new string[] { "-" }, 3, StringSplitOptions.None)[1];
                            var dyy = item.DateofBirth.Split(new string[] { "-" }, 3, StringSplitOptions.None)[2];

                            registrationMaster.DateofRegistration = CreateDateFromTime(Convert.ToInt32(yy), Convert.ToInt32(mm), Convert.ToInt32(dd), time);
                            registrationMaster.DateofBirth = CreateDateFromTime(Convert.ToInt32(dyy), Convert.ToInt32(dmm), Convert.ToInt32(ddd), time);
                            registrationMaster.Gender = item.Gender;
                            registrationMaster.Address1 = item.Address1;
                            registrationMaster.Address3 = null;
                            registrationMaster.LocationID = 0;
                            registrationMaster.FatherHusbandName = null;
                            registrationMaster.AadharNumber = null;
                            registrationMaster.Occupation = null;

                            var phonenumber = item.Phone.All(char.IsDigit);
                            if (phonenumber == true)
                            {
                                registrationMaster.Phone = item.Phone;
                                registrationMaster.EmailID = null;
                                registrationMaster.SourceofReferralID = 0;
                                registrationMaster.ReferralName = null;
                                registrationMaster.IsDeleted = false;
                                registrationMaster.CreatedUTC = DateTime.UtcNow;
                                registrationMaster.UpdatedBy = 0;
                                registrationMaster.Insurance = false;
                                registrationMaster.CreatedBy = 1;
                                registrationMaster.PhotoPath = null;
                                registrationMaster.BiometricPath = null;
                                WYNKContext.Registration.AddRange(registrationMaster);
                                WYNKContext.SaveChanges();

                                var registrationtran = new RegistrationTran_Master();
                                registrationtran.UIN = registrationMaster.UIN;
                                registrationtran.CmpID = CmpId;
                                registrationtran.DateofVisit = registrationMaster.DateofRegistration;
                                registrationtran.TypeofVisit = 0;
                                registrationtran.Status = Convert.ToInt32(closedvalue);
                                registrationtran.AssignedDate = null;
                                registrationtran.DoctorID = 0;
                                registrationtran.StatusDateTime = registrationMaster.DateofRegistration;
                                registrationtran.Remarks = null;
                                registrationtran.FollowupDate = null;
                                registrationtran.IsDeleted = false;
                                registrationtran.CreatedUTC = DateTime.UtcNow;
                                registrationtran.CreatedBy = registrationMaster.CreatedBy;
                                registrationtran.PatientVisitType = Convert.ToString(openvaluevalue);
                                WYNKContext.RegistrationTran.AddRange(registrationtran);
                                WYNKContext.SaveChanges();
                                Uploaded = Uploaded + 1;
                            }
                            else
                            {
                                var Newdata = new ErrorRegistrationExcel();
                                Newdata.Address1 = item.Address1;
                                Newdata.UIN = item.UIN;
                                Newdata.Name = item.Name;
                                Newdata.MiddleName = item.MiddleName;
                                Newdata.Gender = item.Gender;
                                Newdata.Fees = item.Fees;
                                Newdata.LastName = item.LastName;
                                Newdata.DateofRegistration = item.DateofRegistration;
                                Newdata.DateofBirth = item.DateofBirth;
                                Newdata.Phone = item.Phone;
                                Newdata.Fees = item.Fees;
                                Newdata.Status = "Invalid Mobile Number, String Included";
                                Newmodel.ErrorExcelimport.Add(Newdata);
                                Error = Error + 1;
                            }
                            CMPSContext.NumberControl.UpdateRange(rn);
                            CMPSContext.SaveChanges();
                        }

                    }
                    catch (Exception ex)
                    {
                        var Newdata = new ErrorRegistrationExcel();
                        Newdata.Address1 = item.Address1;
                        Newdata.UIN = item.UIN;
                        Newdata.Name = item.Name;
                        Newdata.MiddleName = item.MiddleName;
                        Newdata.Gender = item.Gender;
                        Newdata.Fees = item.Fees;
                        Newdata.LastName = item.LastName;
                        Newdata.DateofRegistration = item.DateofRegistration;
                        Newdata.DateofBirth = item.DateofBirth;
                        Newdata.Phone = item.Phone;
                        Newdata.Fees = item.Fees;

                        if (ex.InnerException != null)
                        {
                            Newdata.Status = ex.InnerException.Message;
                        }
                        else
                        {
                            Newdata.Status = ex.Message;
                        }

                        Newmodel.ErrorExcelimport.Add(Newdata);
                        Error = Error + 1;
                        continue;

                    }

                }

                return new
                {
                    Success = true,
                    Message = "Saved Successfully",
                    UINData = Newmodel.ErrorExcelimport.ToList(),
                    Uploaded = Uploaded,
                    Error = Duplicate + Error,
                };

            }
            else
            {
                return new
                {
                   
                    Success = false,
                    Message = "No Files To Upload",
                };

            }




        }

        public dynamic SubmitDrugStock(DrugStockExcel DrugList, int CmpId, int UserID, int TC, DateTime Dates)
        {
            var StoreID = Convert.ToInt32(DrugList.StoreId);
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var drugMaster = WYNKContext.DrugMaster.ToList();
                    var uomMaster = CMPSContext.uommaster.ToList();

                    var Date = DateTime.Now;
                    var Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date && x.CMPID == CmpId && x.IsActive == true).Select(x => x.ID).FirstOrDefault());

                    var stockmas = AddBilling.AddstockMaster1(DrugList.RunningNo, Dates, StoreID, null, Convert.ToInt32(DrugList.VendorID), TC, CMPSContext.TransactionType.Where(x => x.TransactionID == TC).Select(x => x.Rec_Issue_type).FirstOrDefault(), CmpId, UserID, Fyear);
                    WYNKContext.StockMaster.Add(stockmas);

                    WYNKContext.SaveChanges();

                    foreach (var Drug in DrugList.DrugStockExcelimports.ToList())
                    {


                        var DrugTrackerId = WYNKContext.DrugMaster.Where(x => x.ID == Drug.DrugID).Select(x => x.DrugTracker).FirstOrDefault();
                        var res = Enum.GetName(typeof(TrackingType), DrugTrackerId);


                        if (res == "SerialNumberBased")
                        {
                            var itemSerial = WYNKContext.ItemSerial.Where(x => x.ItemID == Drug.DrugID && x.StoreID == StoreID && x.SerialNo == Drug.BatchSerial && x.IssueNo == null && x.IssueDate == null && x.IssueTC == null).FirstOrDefault();
                            if (itemSerial == null)
                            {
                                var SerialItem = new ItemSerial();
                                SerialItem.ItemID = Drug.DrugID;
                                SerialItem.SerialNo = Drug.BatchSerial;
                                SerialItem.GRNNo = stockmas.DocumentNumber;
                                SerialItem.TC = TC;
                                SerialItem.CreatedUTC = DateTime.UtcNow;
                                SerialItem.CreatedBy = UserID;
                                SerialItem.ExpiryDate = Convert.ToDateTime(Drug.Date);
                                SerialItem.StoreID = StoreID;
                                SerialItem.IssueDate = null;
                                SerialItem.cmpID = CmpId;
                                WYNKContext.ItemSerial.AddRange(SerialItem);


                                var IsStockTran = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == Drug.DrugID).FirstOrDefault();

                                if (IsStockTran == null)
                                {
                                    var StkTran = new StockTran();
                                    StkTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    StkTran.SMID = stockmas.RandomUniqueID;
                                    StkTran.ItemID = Drug.DrugID;
                                    var Qty = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == Drug.DrugID).Select(x => x.ItemQty).FirstOrDefault();
                                    StkTran.ItemQty = Qty != null ? Qty + 1 : 1;
                                    StkTran.UOMID = uomMaster.Where(UOM => UOM.Description == drugMaster.Where(x => x.ID == Drug.DrugID).Select(x => x.UOM).FirstOrDefault()).Select(UOM => UOM.id).FirstOrDefault();
                                    StkTran.ItemRate = drugMaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Rate).FirstOrDefault();
                                    StkTran.ItemValue = StkTran.ItemQty * StkTran.ItemRate;
                                    StkTran.CreatedUTC = DateTime.UtcNow;
                                    StkTran.CreatedBy = UserID;
                                    WYNKContext.StockTran.AddRange(StkTran);
                                    WYNKContext.SaveChanges();
                                }
                                else
                                {
                                    var StkTran = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == Drug.DrugID).FirstOrDefault();
                                    StkTran.ItemQty = StkTran.ItemQty != 0 ? StkTran.ItemQty + Drug.Quantity : Drug.Quantity;
                                    WYNKContext.StockTran.UpdateRange(StkTran);
                                    WYNKContext.SaveChanges();
                                }

                                var ItemBalance = WYNKContext.ItemBalance.Where(x => x.ItemID == Drug.DrugID && x.FYear == Convert.ToInt32(Fyear) && x.StoreID == StoreID && x.CmpID == CmpId).FirstOrDefault();

                                if (ItemBalance == null)
                                {
                                    var CurrentMonth = DateTime.Now.Month;

                                    var ItemBalance1 = new ItemBalance();

                                    switch (CurrentMonth)
                                    {

                                        case 1:
                                            ItemBalance1.Rec01 = 1;
                                            break;
                                        case 2:
                                            ItemBalance1.Rec02 = 1;
                                            break;
                                        case 3:
                                            ItemBalance1.Rec03 = 1;
                                            break;
                                        case 4:
                                            ItemBalance1.Rec04 = 1;
                                            break;
                                        case 5:
                                            ItemBalance1.Rec05 = 1;
                                            break;
                                        case 6:
                                            ItemBalance1.Rec06 = 1;
                                            break;
                                        case 7:
                                            ItemBalance1.Rec07 = 1;
                                            break;
                                        case 8:
                                            ItemBalance1.Rec08 = 1;
                                            break;
                                        case 9:
                                            ItemBalance1.Rec09 = 1;
                                            break;
                                        case 10:
                                            ItemBalance1.Rec10 = 1;
                                            break;
                                        case 11:
                                            ItemBalance1.Rec11 = 1;
                                            break;
                                        case 12:
                                            ItemBalance1.Rec12 = 1;
                                            break;
                                    }
                                    ItemBalance1.ItemID = Drug.DrugID;
                                    ItemBalance1.Iss01 = 0;
                                    ItemBalance1.Iss02 = 0;
                                    ItemBalance1.Iss03 = 0;
                                    ItemBalance1.Iss04 = 0;
                                    ItemBalance1.Iss05 = 0;
                                    ItemBalance1.Iss06 = 0;
                                    ItemBalance1.Iss07 = 0;
                                    ItemBalance1.Iss08 = 0;
                                    ItemBalance1.Iss09 = 0;
                                    ItemBalance1.Iss10 = 0;
                                    ItemBalance1.Iss11 = 0;
                                    ItemBalance1.Iss12 = 0;
                                    ItemBalance1.UOMID = uomMaster.Where(UOM => UOM.Description == drugMaster.Where(x => x.ID == Drug.DrugID).Select(x => x.UOM).FirstOrDefault()).Select(UOM => UOM.id).FirstOrDefault();
                                    ItemBalance1.FYear = Convert.ToInt32(Fyear);
                                    ItemBalance1.OpeningBalance = 0;
                                    ItemBalance1.StoreID = StoreID;
                                    ItemBalance1.ClosingBalance = 1;
                                    ItemBalance1.CreatedBy = UserID;
                                    ItemBalance1.CreatedUTC = DateTime.UtcNow;
                                    ItemBalance1.CmpID = CmpId;
                                    WYNKContext.ItemBalance.Add(ItemBalance1);
                                    WYNKContext.SaveChanges();

                                }
                                else
                                {
                                    var CurrentMonth = DateTime.Now.Month;
                                    switch (CurrentMonth)
                                    {
                                        case 1:
                                            ItemBalance.Rec01 = ItemBalance.Rec01 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 2:
                                            ItemBalance.Rec02 = ItemBalance.Rec02 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 3:
                                            ItemBalance.Rec03 = ItemBalance.Rec03 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 4:
                                            ItemBalance.Rec04 = ItemBalance.Rec04 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 5:
                                            ItemBalance.Rec05 = ItemBalance.Rec05 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 6:
                                            ItemBalance.Rec06 = ItemBalance.Rec06 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 7:
                                            ItemBalance.Rec07 = ItemBalance.Rec07 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 8:
                                            ItemBalance.Rec08 = ItemBalance.Rec08 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 9:
                                            ItemBalance.Rec09 = ItemBalance.Rec09 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 10:
                                            ItemBalance.Rec10 = ItemBalance.Rec10 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 11:
                                            ItemBalance.Rec11 = ItemBalance.Rec11 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 12:
                                            ItemBalance.Rec12 = ItemBalance.Rec12 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                    }
                                }


                                WYNKContext.SaveChanges();
                                Drug.Status = "Uploaded";
                            }
                            else
                            {
                                Drug.Status = "Duplicate";
                                Drug.Remarks = "Serial No Already Existing";
                            }
                        }
                        else if (res == "BatchNumberBased")
                        {
                            var BatchDetail = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == Drug.BatchSerial && x.ItemID == Drug.DrugID && x.StoreID == StoreID && x.ItemBatchExpiry == Convert.ToDateTime(Drug.Date).Date).FirstOrDefault();
                            if (BatchDetail == null)
                            {
                                var Batch = new ItemBatch();
                                Batch.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                Batch.ItemID = Drug.DrugID;
                                Batch.ItemBatchNumber = Drug.BatchSerial;
                                Batch.ItemBatchQty = Drug.Quantity;
                                Batch.ItemBatchissueQty = 0;
                                Batch.ItemBatchBalnceQty = Drug.Quantity;
                                Batch.StoreID = StoreID;
                                Batch.CreatedBy = UserID;
                                Batch.cmpID = CmpId;
                                Batch.CreatedUTC = DateTime.UtcNow;
                                Batch.ItemBatchExpiry = Convert.ToDateTime(Drug.Date);
                                WYNKContext.ItemBatch.AddRange(Batch);
                                WYNKContext.SaveChanges();


                                var IsStockTran = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == Drug.DrugID).FirstOrDefault();

                                if (IsStockTran == null)
                                {
                                    var StkTran = new StockTran();
                                    StkTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    StkTran.SMID = stockmas.RandomUniqueID;
                                    StkTran.ItemID = Drug.DrugID;
                                    var Qty = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == Drug.DrugID).Select(x => x.ItemQty).FirstOrDefault();
                                    StkTran.ItemQty = Qty != 0 ? Qty + Drug.Quantity : Drug.Quantity;
                                    StkTran.UOMID = uomMaster.Where(UOM => UOM.Description == drugMaster.Where(x => x.ID == Drug.DrugID).Select(x => x.UOM).FirstOrDefault()).Select(UOM => UOM.id).FirstOrDefault();
                                    StkTran.ItemRate = drugMaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Rate).FirstOrDefault();
                                    StkTran.ItemValue = StkTran.ItemQty * StkTran.ItemRate;
                                    StkTran.CreatedUTC = DateTime.UtcNow;
                                    StkTran.CreatedBy = UserID;
                                    WYNKContext.StockTran.AddRange(StkTran);
                                    WYNKContext.SaveChanges();


                                    var BatchTrans = new ItemBatchTrans();
                                    BatchTrans.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    BatchTrans.ItemBatchID = Batch.RandomUniqueID;
                                    BatchTrans.TC = TC;
                                    BatchTrans.STID = stockmas.RandomUniqueID;
                                    BatchTrans.SMID = stockmas.RandomUniqueID;
                                    BatchTrans.ItemID = Drug.DrugID;
                                    BatchTrans.ItemBatchNumber = Drug.BatchSerial;
                                    BatchTrans.ItemBatchTransactedQty = Drug.Quantity;
                                    BatchTrans.UOMID = uomMaster.Where(UOM => UOM.Description == drugMaster.Where(x => x.ID == Drug.DrugID).Select(x => x.UOM).FirstOrDefault()).Select(UOM => UOM.id).FirstOrDefault();
                                    BatchTrans.CreatedUTC = DateTime.UtcNow;
                                    BatchTrans.CreatedBy = UserID;
                                    BatchTrans.cmpID = CmpId;
                                    BatchTrans.ItemBatchExpiry = Convert.ToDateTime(Drug.Date);
                                    WYNKContext.ItemBatchTrans.AddRange(BatchTrans);
                                    WYNKContext.SaveChanges();


                                }
                                else
                                {
                                    var StkTran = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == Drug.DrugID).FirstOrDefault();
                                    StkTran.ItemQty = StkTran.ItemQty != 0 ? StkTran.ItemQty + Drug.Quantity : Drug.Quantity;
                                    WYNKContext.StockTran.UpdateRange(StkTran);
                                    WYNKContext.SaveChanges();


                                    var BatchTrans = new ItemBatchTrans();
                                    BatchTrans.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    BatchTrans.ItemBatchID = Batch.RandomUniqueID;
                                    BatchTrans.TC = TC;
                                    BatchTrans.STID = stockmas.RandomUniqueID;
                                    BatchTrans.SMID = stockmas.RandomUniqueID;
                                    BatchTrans.ItemID = Drug.DrugID;
                                    BatchTrans.ItemBatchNumber = Drug.BatchSerial;
                                    BatchTrans.ItemBatchTransactedQty = Drug.Quantity;
                                    BatchTrans.UOMID = uomMaster.Where(UOM => UOM.Description == drugMaster.Where(x => x.ID == Drug.DrugID).Select(x => x.UOM).FirstOrDefault()).Select(UOM => UOM.id).FirstOrDefault();
                                    BatchTrans.CreatedUTC = DateTime.UtcNow;
                                    BatchTrans.CreatedBy = UserID;
                                    BatchTrans.cmpID = CmpId;
                                    BatchTrans.ItemBatchExpiry = Convert.ToDateTime(Drug.Date);
                                    WYNKContext.ItemBatchTrans.AddRange(BatchTrans);
                                    WYNKContext.SaveChanges();

                                }

                                Drug.Status = "Uploaded";
                            }
                            else
                            {
                                BatchDetail.ItemBatchQty = BatchDetail.ItemBatchQty + Drug.Quantity;
                                BatchDetail.ItemBatchBalnceQty = BatchDetail.ItemBatchBalnceQty + Drug.Quantity;
                                BatchDetail.UpdatedBy = UserID;
                                BatchDetail.UpdatedUTC = DateTime.UtcNow;
                                WYNKContext.ItemBatch.UpdateRange(BatchDetail);
                                WYNKContext.SaveChanges();


                                var StkTran = new StockTran();
                                StkTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                StkTran.SMID = stockmas.RandomUniqueID;
                                StkTran.ItemID = Drug.DrugID;
                                var Qty = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == Drug.DrugID).Select(x => x.ItemQty).FirstOrDefault();
                                StkTran.ItemQty = Qty != 0 ? Qty + Drug.Quantity : Drug.Quantity;
                                StkTran.UOMID = uomMaster.Where(UOM => UOM.Description == drugMaster.Where(x => x.ID == Drug.DrugID).Select(x => x.UOM).FirstOrDefault()).Select(UOM => UOM.id).FirstOrDefault();
                                StkTran.ItemRate = drugMaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Rate).FirstOrDefault();
                                StkTran.ItemValue = StkTran.ItemQty * StkTran.ItemRate;
                                StkTran.CreatedUTC = DateTime.UtcNow;
                                StkTran.CreatedBy = UserID;
                                WYNKContext.StockTran.AddRange(StkTran);
                                WYNKContext.SaveChanges();


                                var BatchTrans = new ItemBatchTrans();
                                BatchTrans.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                BatchTrans.ItemBatchID = BatchDetail.RandomUniqueID;
                                BatchTrans.TC = TC;
                                BatchTrans.STID = stockmas.RandomUniqueID;
                                BatchTrans.SMID = stockmas.RandomUniqueID;
                                BatchTrans.ItemID = Drug.DrugID;
                                BatchTrans.ItemBatchNumber = Drug.BatchSerial;
                                BatchTrans.ItemBatchTransactedQty = Drug.Quantity;
                                BatchTrans.UOMID = uomMaster.Where(UOM => UOM.Description == drugMaster.Where(x => x.ID == Drug.DrugID).Select(x => x.UOM).FirstOrDefault()).Select(UOM => UOM.id).FirstOrDefault();
                                BatchTrans.CreatedUTC = DateTime.UtcNow;
                                BatchTrans.CreatedBy = UserID;
                                BatchTrans.cmpID = CmpId;
                                BatchTrans.ItemBatchExpiry = Convert.ToDateTime(Drug.Date);
                                WYNKContext.ItemBatchTrans.AddRange(BatchTrans);
                                WYNKContext.SaveChanges();

                                Drug.Status = "Uploaded";
                            }
                            var ItemBalance = WYNKContext.ItemBalance.Where(x => x.ItemID == Drug.DrugID && x.FYear == Convert.ToInt32(Fyear) && x.StoreID == StoreID && x.CmpID == CmpId).FirstOrDefault();
                            if (ItemBalance == null)
                            {
                                var CurrentMonth = DateTime.Now.Month;

                                var ItemBalance1 = new ItemBalance();

                                switch (CurrentMonth)
                                {

                                    case 1:
                                        ItemBalance1.Rec01 = Drug.Quantity;
                                        break;
                                    case 2:
                                        ItemBalance1.Rec02 = Drug.Quantity;
                                        break;
                                    case 3:
                                        ItemBalance1.Rec03 = Drug.Quantity;
                                        break;
                                    case 4:
                                        ItemBalance1.Rec04 = Drug.Quantity;
                                        break;
                                    case 5:
                                        ItemBalance1.Rec05 = Drug.Quantity;
                                        break;
                                    case 6:
                                        ItemBalance1.Rec06 = Drug.Quantity;
                                        break;
                                    case 7:
                                        ItemBalance1.Rec07 = Drug.Quantity;
                                        break;
                                    case 8:
                                        ItemBalance1.Rec08 = Drug.Quantity;
                                        break;
                                    case 9:
                                        ItemBalance1.Rec09 = Drug.Quantity;
                                        break;
                                    case 10:
                                        ItemBalance1.Rec10 = Drug.Quantity;
                                        break;
                                    case 11:
                                        ItemBalance1.Rec11 = Drug.Quantity;
                                        break;
                                    case 12:
                                        ItemBalance1.Rec12 = Drug.Quantity;
                                        break;
                                }

                                ItemBalance1.ItemID = Drug.DrugID;
                                ItemBalance1.Iss01 = 0;
                                ItemBalance1.Iss02 = 0;
                                ItemBalance1.Iss03 = 0;
                                ItemBalance1.Iss04 = 0;
                                ItemBalance1.Iss05 = 0;
                                ItemBalance1.Iss06 = 0;
                                ItemBalance1.Iss07 = 0;
                                ItemBalance1.Iss08 = 0;
                                ItemBalance1.Iss09 = 0;
                                ItemBalance1.Iss10 = 0;
                                ItemBalance1.Iss11 = 0;
                                ItemBalance1.Iss12 = 0;
                                ItemBalance1.UOMID = uomMaster.Where(UOM => UOM.Description == drugMaster.Where(x => x.ID == Drug.DrugID).Select(x => x.UOM).FirstOrDefault()).Select(UOM => UOM.id).FirstOrDefault();
                                ItemBalance1.FYear = Convert.ToInt32(Fyear);
                                ItemBalance1.OpeningBalance = 0;
                                ItemBalance1.StoreID = StoreID;
                                ItemBalance1.ClosingBalance = Drug.Quantity;
                                ItemBalance1.CreatedBy = UserID;
                                ItemBalance1.CreatedUTC = DateTime.UtcNow;
                                ItemBalance1.CmpID = CmpId;
                                WYNKContext.ItemBalance.Add(ItemBalance1);
                                WYNKContext.SaveChanges();
                            }
                            else
                            {
                                var CurrentMonth = DateTime.Now.Month;

                                switch (CurrentMonth)
                                {
                                    case 1:
                                        ItemBalance.Rec01 = ItemBalance.Rec01 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 2:
                                        ItemBalance.Rec02 = ItemBalance.Rec02 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 3:
                                        ItemBalance.Rec03 = ItemBalance.Rec03 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 4:
                                        ItemBalance.Rec04 = ItemBalance.Rec04 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 5:
                                        ItemBalance.Rec05 = ItemBalance.Rec05 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 6:
                                        ItemBalance.Rec06 = ItemBalance.Rec06 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 7:
                                        ItemBalance.Rec07 = ItemBalance.Rec07 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 8:
                                        ItemBalance.Rec08 = ItemBalance.Rec08 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 9:
                                        ItemBalance.Rec09 = ItemBalance.Rec09 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 10:
                                        ItemBalance.Rec10 = ItemBalance.Rec10 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 11:
                                        ItemBalance.Rec11 = ItemBalance.Rec11 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 12:
                                        ItemBalance.Rec12 = ItemBalance.Rec12 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                }
                                WYNKContext.SaveChanges();
                            }
                        }
                        else
                        {
                            var ItemBalance = WYNKContext.ItemBalance.Where(x => x.ItemID == Drug.DrugID && x.FYear == Convert.ToInt32(Fyear) && x.StoreID == StoreID && x.CmpID == CmpId).FirstOrDefault();
                            if (ItemBalance == null)
                            {
                                var CurrentMonth = DateTime.Now.Month;

                                var ItemBalance1 = new ItemBalance();

                                switch (CurrentMonth)
                                {

                                    case 1:
                                        ItemBalance1.Rec01 = Drug.Quantity;
                                        break;
                                    case 2:
                                        ItemBalance1.Rec02 = Drug.Quantity;
                                        break;
                                    case 3:
                                        ItemBalance1.Rec03 = Drug.Quantity;
                                        break;
                                    case 4:
                                        ItemBalance1.Rec04 = Drug.Quantity;
                                        break;
                                    case 5:
                                        ItemBalance1.Rec05 = Drug.Quantity;
                                        break;
                                    case 6:
                                        ItemBalance1.Rec06 = Drug.Quantity;
                                        break;
                                    case 7:
                                        ItemBalance1.Rec07 = Drug.Quantity;
                                        break;
                                    case 8:
                                        ItemBalance1.Rec08 = Drug.Quantity;
                                        break;
                                    case 9:
                                        ItemBalance1.Rec09 = Drug.Quantity;
                                        break;
                                    case 10:
                                        ItemBalance1.Rec10 = Drug.Quantity;
                                        break;
                                    case 11:
                                        ItemBalance1.Rec11 = Drug.Quantity;
                                        break;
                                    case 12:
                                        ItemBalance1.Rec12 = Drug.Quantity;
                                        break;
                                }

                                ItemBalance1.ItemID = Drug.DrugID;
                                ItemBalance1.Iss01 = 0;
                                ItemBalance1.Iss02 = 0;
                                ItemBalance1.Iss03 = 0;
                                ItemBalance1.Iss04 = 0;
                                ItemBalance1.Iss05 = 0;
                                ItemBalance1.Iss06 = 0;
                                ItemBalance1.Iss07 = 0;
                                ItemBalance1.Iss08 = 0;
                                ItemBalance1.Iss09 = 0;
                                ItemBalance1.Iss10 = 0;
                                ItemBalance1.Iss11 = 0;
                                ItemBalance1.Iss12 = 0;
                                ItemBalance1.UOMID = uomMaster.Where(UOM => UOM.Description == drugMaster.Where(x => x.ID == Drug.DrugID).Select(x => x.UOM).FirstOrDefault()).Select(UOM => UOM.id).FirstOrDefault();
                                ItemBalance1.FYear = Convert.ToInt32(Fyear);
                                ItemBalance1.OpeningBalance = 0;
                                ItemBalance1.StoreID = StoreID;
                                ItemBalance1.ClosingBalance = Drug.Quantity;
                                ItemBalance1.CreatedBy = UserID;
                                ItemBalance1.CreatedUTC = DateTime.UtcNow;
                                ItemBalance1.CmpID = CmpId;
                                WYNKContext.ItemBalance.Add(ItemBalance1);
                                WYNKContext.SaveChanges();

                                var IsStockTran = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == Drug.DrugID).FirstOrDefault();

                                if (IsStockTran == null)
                                {

                                    var StkTran = new StockTran();
                                    StkTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    StkTran.SMID = stockmas.RandomUniqueID;
                                    StkTran.ItemID = Drug.DrugID;
                                    var Qty = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == Drug.DrugID).Select(x => x.ItemQty).FirstOrDefault();
                                    StkTran.ItemQty = Drug.Quantity;
                                    StkTran.UOMID = uomMaster.Where(UOM => UOM.Description == drugMaster.Where(x => x.ID == Drug.DrugID).Select(x => x.UOM).FirstOrDefault()).Select(UOM => UOM.id).FirstOrDefault();
                                    StkTran.ItemRate = drugMaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Rate).FirstOrDefault();
                                    StkTran.ItemValue = StkTran.ItemQty * StkTran.ItemRate;
                                    StkTran.CreatedUTC = DateTime.UtcNow;
                                    StkTran.CreatedBy = UserID;
                                    WYNKContext.StockTran.AddRange(StkTran);
                                    WYNKContext.SaveChanges();
                                }
                                else
                                {
                                    var StkTran = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == Drug.DrugID).FirstOrDefault();
                                    StkTran.ItemQty = StkTran.ItemQty != null ? StkTran.ItemQty + Drug.Quantity : Drug.Quantity;
                                    WYNKContext.StockTran.UpdateRange(StkTran);
                                    WYNKContext.SaveChanges();
                                }

                                Drug.Status = "Uploaded";
                            }
                            else
                            {
                                var CurrentMonth = DateTime.Now.Month;

                                switch (CurrentMonth)
                                {
                                    case 1:
                                        ItemBalance.Rec01 = ItemBalance.Rec01 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 2:
                                        ItemBalance.Rec02 = ItemBalance.Rec02 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 3:
                                        ItemBalance.Rec03 = ItemBalance.Rec03 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 4:
                                        ItemBalance.Rec04 = ItemBalance.Rec04 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 5:
                                        ItemBalance.Rec05 = ItemBalance.Rec05 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 6:
                                        ItemBalance.Rec06 = ItemBalance.Rec06 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 7:
                                        ItemBalance.Rec07 = ItemBalance.Rec07 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 8:
                                        ItemBalance.Rec08 = ItemBalance.Rec08 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 9:
                                        ItemBalance.Rec09 = ItemBalance.Rec09 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 10:
                                        ItemBalance.Rec10 = ItemBalance.Rec10 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 11:
                                        ItemBalance.Rec11 = ItemBalance.Rec11 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 12:
                                        ItemBalance.Rec12 = ItemBalance.Rec12 + Drug.Quantity;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Drug.Quantity;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                }


                                var IsStockTran = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == Drug.DrugID).FirstOrDefault();

                                if (IsStockTran == null)
                                {

                                    var StkTran = new StockTran();
                                    StkTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    StkTran.SMID = stockmas.RandomUniqueID;
                                    StkTran.ItemID = Drug.DrugID;
                                    var Qty = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == Drug.DrugID).Select(x => x.ItemQty).FirstOrDefault();
                                    StkTran.ItemQty = Qty != null ? Qty + Drug.Quantity : Drug.Quantity;
                                    StkTran.UOMID = uomMaster.Where(UOM => UOM.Description == drugMaster.Where(x => x.ID == Drug.DrugID).Select(x => x.UOM).FirstOrDefault()).Select(UOM => UOM.id).FirstOrDefault();
                                    StkTran.ItemRate = drugMaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Rate).FirstOrDefault();
                                    StkTran.ItemValue = StkTran.ItemQty * StkTran.ItemRate;
                                    StkTran.CreatedUTC = DateTime.UtcNow;
                                    StkTran.CreatedBy = UserID;
                                    WYNKContext.StockTran.AddRange(StkTran);
                                    WYNKContext.SaveChanges();

                                }
                                else
                                {
                                    var StkTran = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == Drug.DrugID).FirstOrDefault();
                                    StkTran.ItemQty = StkTran.ItemQty != 0 ? StkTran.ItemQty + Drug.Quantity : Drug.Quantity;
                                    WYNKContext.StockTran.UpdateRange(StkTran);
                                    WYNKContext.SaveChanges();
                                }
                                Drug.Status = "Uploaded";
                            }
                        }
                    }


                    var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                    var RunningNumber = commonRepos.GenerateRunningCtrlNoo(TC, CmpId, "GetRunningNo");


                    if (RunningNumber == DrugList.RunningNo)
                    {
                        commonRepos.GenerateRunningCtrlNoo(TC, CmpId, "UpdateRunningNo");
                    }
                    else
                    {
                        var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(TC, CmpId, "UpdateRunningNo");

                        var StockMasters = WYNKContext.StockMaster.Where(x => x.SMID == stockmas.SMID && x.TransactionID == TC).FirstOrDefault();
                        StockMasters.DocumentNumber = GetRunningNumber;
                        WYNKContext.StockMaster.UpdateRange(StockMasters);
                    }

                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

                    return new { Success = true, Message = "Data Uploaded", DrugData = DrugList.DrugStockExcelimports };

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error", ex.InnerException.Message.ToString());
                    string msg = ex.InnerException.Message;
                    return new { Success = false, Message = msg, grn = DrugList.RunningNo };
                }


            }
        }

        public dynamic GetStatusOpticalStock(OpticalStockExcel OpticalList, int CmpId)
        {
            var Lensmaster = WYNKContext.Lensmaster.ToList();
            var LensTran = WYNKContext.Lenstrans.ToList();
            var BrandMaster = WYNKContext.Brand.ToList();

            var OpticalDatas = (from Opticallist in OpticalList.OpticalStockExcelimports
                                select new
                                {
                                    OpticalID = Opticallist.OpticalID,
                                    LensType = Lensmaster.Where(lm => lm.RandomUniqueID == LensTran.Where(lt => lt.ID == Opticallist.OpticalID).Select(lt => lt.LMID).FirstOrDefault()).Select(lm => lm.LensType).FirstOrDefault(),
                                    Brand = BrandMaster.Where(Bm => Bm.ID == LensTran.Where(lt => lt.ID == Opticallist.OpticalID).Select(lt => lt.Brand).FirstOrDefault()).Select(Bm => Bm.Description).FirstOrDefault(),

                                    Model = LensTran.Where(lt => lt.ID == Opticallist.OpticalID).Select(lt => lt.Model).FirstOrDefault(),
                                    Quantity = Opticallist.Quantity,
                                    Remarks = Opticallist.Quantity < 0 || Opticallist.Quantity == 0 ? "InValid Data" : " ",
                                    Status = Opticallist.Quantity < 0 || Opticallist.Quantity == 0 ? "InValid" : "Valid",
                                }).ToList();






            if (OpticalList.OpticalStockExcelimports.Count > 0)
            {
                return new
                {
                    Success = true,
                    OpticalDatas = OpticalDatas,
                };
            }
            else
            {
                return new
                {
                    Success = false,
                };
            }
        }

        public dynamic SubmitOpticalStock(OpticalStockExcel OpticList, int CmpId, int UserID, int TC, DateTime Dates)
        {

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var Date = DateTime.Now;
                    var Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date && x.CMPID == CmpId && x.IsActive == true).Select(x => x.ID).FirstOrDefault());

                    var OptcialStockMaster = new OpticalStockMaster();
                    OptcialStockMaster.TransactionID = TC;
                    OptcialStockMaster.CMPID = CmpId;
                    OptcialStockMaster.DocumentNumber = OpticList.RunningNo;
                    OptcialStockMaster.DocumentDate = Dates;
                    OptcialStockMaster.OpticalOrderID = "";
                    OptcialStockMaster.StoreID = Convert.ToInt32(OpticList.StoreId);
                    OptcialStockMaster.TransactionType = CMPSContext.TransactionType.Where(x => x.TransactionID == TC).Select(x => x.Rec_Issue_type).FirstOrDefault();
                    OptcialStockMaster.VendorID = Convert.ToInt32(OpticList.VendorID);
                    OptcialStockMaster.DepartmentID = 0;
                    OptcialStockMaster.TotalPOValue = 0;
                    OptcialStockMaster.IsCancelled = false;
                    OptcialStockMaster.IsDeleted = false;
                    OptcialStockMaster.CreatedUTC = DateTime.UtcNow;
                    OptcialStockMaster.CreatedBy = UserID;
                    OptcialStockMaster.Fyear = Fyear;
                    WYNKContext.OpticalStockMaster.Add(OptcialStockMaster);
                    WYNKContext.SaveChanges();




                    foreach (var Optical in OpticList.OpticalStockExcelimports.ToList())
                    {
                        var OPticalBalance = WYNKContext.OpticalBalance.Where(x => x.LTID == Optical.OpticalID && x.FYID == Convert.ToInt32(Fyear) && x.StoreID == Convert.ToInt32(OpticList.StoreId) && x.CmpID == CmpId).FirstOrDefault();

                        if (OPticalBalance == null)
                        {
                            var CurrentMonth = DateTime.Now.Month;

                            var OpticBal = new OpticalBalance();
                            switch (CurrentMonth)
                            {

                                case 1:
                                    OpticBal.REC01 = Optical.Quantity;
                                    break;
                                case 2:
                                    OpticBal.REC02 = Optical.Quantity;
                                    break;
                                case 3:
                                    OpticBal.REC03 = Optical.Quantity;
                                    break;
                                case 4:
                                    OpticBal.REC04 = Optical.Quantity;
                                    break;
                                case 5:
                                    OpticBal.REC05 = Optical.Quantity;
                                    break;
                                case 6:
                                    OpticBal.REC06 = Optical.Quantity;
                                    break;
                                case 7:
                                    OpticBal.REC07 = Optical.Quantity;
                                    break;
                                case 8:
                                    OpticBal.REC08 = Optical.Quantity;
                                    break;
                                case 9:
                                    OpticBal.REC09 = Optical.Quantity;
                                    break;
                                case 10:
                                    OpticBal.REC10 = Optical.Quantity;
                                    break;
                                case 11:
                                    OpticBal.REC11 = Optical.Quantity;
                                    break;
                                case 12:
                                    OpticBal.REC12 = Optical.Quantity;
                                    break;
                            }
                            OpticBal.ISS01 = 0;
                            OpticBal.ISS02 = 0;
                            OpticBal.ISS03 = 0;
                            OpticBal.ISS04 = 0;
                            OpticBal.ISS05 = 0;
                            OpticBal.ISS06 = 0;
                            OpticBal.ISS07 = 0;
                            OpticBal.ISS08 = 0;
                            OpticBal.ISS09 = 0;
                            OpticBal.ISS10 = 0;
                            OpticBal.ISS11 = 0;
                            OpticBal.ISS12 = 0;
                            OpticBal.LTID = Optical.OpticalID;
                            OpticBal.OpeningBalance = 0;
                            OpticBal.ClosingBalance = Optical.Quantity;
                            OpticBal.FYID = Convert.ToInt32(Fyear);
                            OpticBal.StoreID = Convert.ToInt32(OpticList.StoreId);
                            OpticBal.UOMID = Convert.ToInt32(WYNKContext.Lenstrans.Where(x => x.ID == Optical.OpticalID).Select(x => x.UOMID).FirstOrDefault());
                            OpticBal.CreatedUTC = DateTime.UtcNow;
                            OpticBal.CreatedBy = UserID;
                            OpticBal.CmpID = CmpId;

                            WYNKContext.OpticalBalance.Add(OpticBal);
                            WYNKContext.SaveChanges();





                            var Optcialstocktran = WYNKContext.OpticalStockTran.Where(opTran => opTran.RandomUniqueID == OptcialStockMaster.RandomUniqueID && opTran.LMIDID == Optical.OpticalID).FirstOrDefault();

                            if (Optcialstocktran == null)
                            {
                                var OpticStoTran = new OpticalStockTran();
                                OpticStoTran.RandomUniqueID = OptcialStockMaster.RandomUniqueID;
                                OpticStoTran.LMIDID = Optical.OpticalID;
                                OpticStoTran.POID = 0;
                                OpticStoTran.ItemQty = Optical.Quantity;
                                OpticStoTran.UOMID = WYNKContext.Lenstrans.Where(x => x.ID == Optical.OpticalID).Select(x => x.UOMID).FirstOrDefault();
                                OpticStoTran.ItemRate = WYNKContext.Lenstrans.Where(x => x.ID == Optical.OpticalID).Select(x => x.Prize).FirstOrDefault();
                                OpticStoTran.IsDeleted = false;
                                OpticStoTran.CreatedUTC = DateTime.UtcNow;
                                OpticStoTran.CreatedBy = UserID;
                                WYNKContext.OpticalStockTran.Add(OpticStoTran);
                                WYNKContext.SaveChanges();
                            }
                            else
                            {
                                Optcialstocktran.ItemQty = Optcialstocktran.ItemQty + Optical.Quantity;
                                WYNKContext.OpticalStockTran.UpdateRange(Optcialstocktran);
                                WYNKContext.SaveChanges();
                            }

                        }
                        else
                        {

                            var CurrentMonth = DateTime.Now.Month;

                            switch (CurrentMonth)
                            {

                                case 1:
                                    OPticalBalance.REC01 = OPticalBalance.REC01 + Optical.Quantity;
                                    OPticalBalance.ClosingBalance = OPticalBalance.ClosingBalance + Optical.Quantity;
                                    break;
                                case 2:
                                    OPticalBalance.REC02 = OPticalBalance.REC02 + Optical.Quantity;
                                    OPticalBalance.ClosingBalance = OPticalBalance.ClosingBalance + Optical.Quantity;
                                    break;
                                case 3:
                                    OPticalBalance.REC03 = OPticalBalance.REC03 + Optical.Quantity;
                                    OPticalBalance.ClosingBalance = OPticalBalance.ClosingBalance + Optical.Quantity;
                                    break;
                                case 4:
                                    OPticalBalance.REC04 = OPticalBalance.REC04 + Optical.Quantity;
                                    OPticalBalance.ClosingBalance = OPticalBalance.ClosingBalance + Optical.Quantity;
                                    break;
                                case 5:
                                    OPticalBalance.REC05 = OPticalBalance.REC05 + Optical.Quantity;
                                    OPticalBalance.ClosingBalance = OPticalBalance.ClosingBalance + Optical.Quantity;
                                    break;
                                case 6:
                                    OPticalBalance.REC06 = OPticalBalance.REC06 + Optical.Quantity;
                                    OPticalBalance.ClosingBalance = OPticalBalance.ClosingBalance + Optical.Quantity;
                                    break;
                                case 7:
                                    OPticalBalance.REC07 = OPticalBalance.REC07 + Optical.Quantity;
                                    OPticalBalance.ClosingBalance = OPticalBalance.ClosingBalance + Optical.Quantity;
                                    break;
                                case 8:
                                    OPticalBalance.REC08 = OPticalBalance.REC08 + Optical.Quantity;
                                    OPticalBalance.ClosingBalance = OPticalBalance.ClosingBalance + Optical.Quantity;
                                    break;
                                case 9:
                                    OPticalBalance.REC09 = OPticalBalance.REC09 + Optical.Quantity;
                                    OPticalBalance.ClosingBalance = OPticalBalance.ClosingBalance + Optical.Quantity;
                                    break;
                                case 10:
                                    OPticalBalance.REC10 = OPticalBalance.REC10 + Optical.Quantity;
                                    OPticalBalance.ClosingBalance = OPticalBalance.ClosingBalance + Optical.Quantity;
                                    break;
                                case 11:
                                    OPticalBalance.REC11 = OPticalBalance.REC11 + Optical.Quantity;
                                    OPticalBalance.ClosingBalance = OPticalBalance.ClosingBalance + Optical.Quantity;
                                    break;
                                case 12:
                                    OPticalBalance.REC12 = OPticalBalance.REC12 + Optical.Quantity;
                                    OPticalBalance.ClosingBalance = OPticalBalance.ClosingBalance + Optical.Quantity;
                                    break;
                            }


                            WYNKContext.OpticalBalance.UpdateRange(OPticalBalance);
                            WYNKContext.SaveChanges();

                            var Optcialstocktran = WYNKContext.OpticalStockTran.Where(opTran => opTran.RandomUniqueID == OptcialStockMaster.RandomUniqueID && opTran.LMIDID == Optical.OpticalID).FirstOrDefault();

                            if (Optcialstocktran == null)
                            {
                                var OpticStoTran = new OpticalStockTran();
                                OpticStoTran.RandomUniqueID = OptcialStockMaster.RandomUniqueID;
                                OpticStoTran.LMIDID = Optical.OpticalID;
                                OpticStoTran.POID = 0;
                                OpticStoTran.ItemQty = Optical.Quantity;
                                OpticStoTran.UOMID = WYNKContext.Lenstrans.Where(x => x.ID == Optical.OpticalID).Select(x => x.UOMID).FirstOrDefault();
                                OpticStoTran.ItemRate = WYNKContext.Lenstrans.Where(x => x.ID == Optical.OpticalID).Select(x => x.Prize).FirstOrDefault();
                                OpticStoTran.IsDeleted = false;
                                OpticStoTran.CreatedUTC = DateTime.UtcNow;
                                OpticStoTran.CreatedBy = UserID;
                                WYNKContext.OpticalStockTran.Add(OpticStoTran);
                                WYNKContext.SaveChanges();
                            }
                            else
                            {
                                Optcialstocktran.ItemQty = Optcialstocktran.ItemQty + Optical.Quantity;
                                WYNKContext.OpticalStockTran.UpdateRange(Optcialstocktran);
                                WYNKContext.SaveChanges();
                            }

                        }


                        Optical.Status = "Uploaded";
                    }


                    var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                    var RunningNumber = commonRepos.GenerateRunningCtrlNoo(TC, CmpId, "GetRunningNo");


                    if (RunningNumber == OpticList.RunningNo)
                    {
                        commonRepos.GenerateRunningCtrlNoo(TC, CmpId, "UpdateRunningNo");
                    }
                    else
                    {
                        var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(TC, CmpId, "UpdateRunningNo");

                        var StockMasters = WYNKContext.OpticalStockMaster.Where(x => x.SMID == OptcialStockMaster.SMID && x.TransactionID == TC).FirstOrDefault();
                        StockMasters.DocumentNumber = GetRunningNumber;
                        WYNKContext.OpticalStockMaster.UpdateRange(StockMasters);
                    }

                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

                    return new { Success = true, Message = "Data Uploaded", OPticData = OpticList.OpticalStockExcelimports };




                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error", ex.InnerException.Message.ToString());
                    string msg = ex.InnerException.Message;
                    return new { Success = false, Message = msg, grn = OpticList.RunningNo };
                }
            }
        }

        public dynamic GettingDrugIdsDesc(int CmpId)
        {
            try
            {
                var res = (from drug in WYNKContext.DrugMaster.Where(x => x.Cmpid == CmpId)
                           select new
                           {
                               DrugID = drug.ID,
                               Brand = drug.Brand,
                               BatchSerial = "varchar",
                               Date = "varchar",
                               Quantity = "number",
                           }).ToList();


                return new
                {
                    Success = true,
                    res = res,
                };
            }
            catch (Exception)
            {

                return new
                {
                    Success = false,
                };
            }

        }

        public dynamic VendorDrugCheck(DrugStockExcel DrugList, int CmpId, int VendorID)
        {

            try
            {
                var res = (from drug in DrugList.DrugStockExcelimports
                           let IsMapped = WYNKContext.ItemVendorMapping.Where(x => x.ItemID == drug.DrugID && x.VendorID == VendorID && x.IsActive == true).Select(x => x.IsActive).FirstOrDefault()
                           select new DrugStockExcelimport
                           {
                               DrugID = drug.DrugID,
                               Brand = drug.Brand,
                               BatchSerial = drug.BatchSerial,
                               Date = drug.Date,
                               Generic = drug.Generic,
                               Quantity = drug.Quantity,
                               Remarks = IsMapped != true ? "Item not Mapped" : null,
                               Status = IsMapped != true ? "Invalid" : "Valid",
                           }).ToList();

                return new
                {
                    Success = true,
                    res = res,
                };

            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                };
            }
        }


        public dynamic GetExistingRecord(int ICD, string Surgery)
        {
            var ICDSpecialityCode = WYNKContext.ICDSpecialityCode.ToList();
            var ICDMaster = WYNKContext.ICDMaster.ToList();
            var IOProcedureTemplate = WYNKContext.IOProcedureTemplate.ToList();
            var IOTemplateTran = WYNKContext.IOTemplateTran.ToList();

            var result = new ExcelViewModel();

            result.GetOTNotesValues = (from ICDSpecial in ICDSpecialityCode.Where(x => x.ID == ICD)
                                       join IOP in IOProcedureTemplate.Where(x => x.IsActive == true && x.SurgeryDescription == Surgery) on ICDSpecial.ID equals IOP.ICDSpecialityCode
                                       join IOT in IOTemplateTran.Where(x => x.IsActive == true) on IOP.ID equals IOT.IOTemplateID

                                       select new GetOTNotesValues
                                       {
                                           OTNotes = IOT.OTNotesDescription,
                                           UserInput = IOT.UserInputType,
                                           InputValue = IOT.InputValue,
                                       }).ToList();
            return result;
        }

        public dynamic UpdateSurgeryExceldata(ExcelViewModel UpdateSurgeryExceldata, int CmpId, int Createdby, int Icd, string surgery, int popup)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                if (UpdateSurgeryExceldata.SurgeryDetailsExcelImport.Count > 0)
                {
                    int Uploaded = 0;
                    int Error = 0;
                    ErrorLog oErrorLogs = new ErrorLog();

                    if (popup == 1)
                    {
                        var Description = WYNKContext.IOProcedureTemplate.Where(x => x.SurgeryDescription == surgery).ToList();
                        foreach (var i in Description)
                        {
                            var IsSoftDel = WYNKContext.IOTemplateTran.Where(x => x.IOTemplateID == i.ID && x.IsActive == true).ToList();
                            if (IsSoftDel != null)
                            {
                                IsSoftDel.All(x => { x.IsActive = false; return true; });
                                WYNKContext.IOTemplateTran.UpdateRange(IsSoftDel);
                                WYNKContext.SaveChanges();
                            }
                            var IsSoftDelPR = WYNKContext.IOProcedureTemplate.Where(x => x.ID == i.ID && x.IsActive == true).ToList();
                            if (IsSoftDelPR != null)
                            {
                                IsSoftDelPR.All(x => { x.IsActive = false; return true; });
                                WYNKContext.IOProcedureTemplate.UpdateRange(IsSoftDelPR);
                                WYNKContext.SaveChanges();
                            }
                        }

                        foreach (var IOT in UpdateSurgeryExceldata.SurgeryDetailsExcelImport.ToList())
                        {
                            var IOPT = new IOProcedureTemplate();
                            IOPT.ICDSpecialityCode = WYNKContext.ICDSpecialityCode.Where(x => x.ID == Icd).Select(x => x.ID).FirstOrDefault();
                            IOPT.SurgeryDescription = surgery;
                            IOPT.IsActive = true;
                            IOPT.CreatedUTC = DateTime.UtcNow;
                            IOPT.CreatedBy = Createdby;
                            WYNKContext.IOProcedureTemplate.Add(IOPT);
                            WYNKContext.SaveChanges();


                            var IOTEMPT = new IOTemplateTran();
                            IOTEMPT.IOTemplateID = IOPT.ID;
                            IOTEMPT.OTNotesDescription = IOT.OTNotes;
                            IOTEMPT.UserInputType = IOT.UserInput;
                            IOTEMPT.InputValue = IOT.InputValue;
                            IOTEMPT.IsActive = true;
                            IOTEMPT.CreatedUTC = DateTime.UtcNow;
                            IOTEMPT.CreatedBy = Createdby;
                            WYNKContext.IOTemplateTran.Add(IOTEMPT);
                            IOT.Status = "Uploaded";
                            Uploaded = Uploaded + 1;
                            WYNKContext.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                        return new
                        {
                            Success = true,
                            Message = "Saved successfully",
                            SurgeryDetails = UpdateSurgeryExceldata.SurgeryDetailsExcelImport.ToList(),
                            Uploaded = Uploaded,
                            Error = Error,
                        };
                    }
                    else
                    {
                        //var Description = WYNKContext.IOProcedureTemplate.Where(x => x.SurgeryDescription == surgery).Select(x => x.ID).FirstOrDefault();
                        foreach (var IOT in UpdateSurgeryExceldata.SurgeryDetailsExcelImport.ToList())
                        {
                            var IOPT = new IOProcedureTemplate();
                            IOPT.ICDSpecialityCode = WYNKContext.ICDSpecialityCode.Where(x => x.ID == Icd).Select(x => x.ID).FirstOrDefault();
                            IOPT.SurgeryDescription = surgery;
                            IOPT.IsActive = true;
                            IOPT.CreatedUTC = DateTime.UtcNow;
                            IOPT.CreatedBy = Createdby;
                            WYNKContext.IOProcedureTemplate.Add(IOPT);
                            WYNKContext.SaveChanges();


                            var IOTEMPT = new IOTemplateTran();
                            IOTEMPT.IOTemplateID = IOPT.ID;
                            IOTEMPT.OTNotesDescription = IOT.OTNotes;
                            IOTEMPT.UserInputType = IOT.UserInput;
                            IOTEMPT.InputValue = IOT.InputValue;
                            IOTEMPT.IsActive = true;
                            IOTEMPT.CreatedUTC = DateTime.UtcNow;
                            IOTEMPT.CreatedBy = Createdby;
                            WYNKContext.IOTemplateTran.Add(IOTEMPT);
                            IOT.Status = "Uploaded";
                            Uploaded = Uploaded + 1;
                            WYNKContext.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                        return new
                        {
                            Success = true,
                            Message = "Saved successfully",
                            SurgeryDetails = UpdateSurgeryExceldata.SurgeryDetailsExcelImport.ToList(),
                            Uploaded = Uploaded,
                            Error = Error,
                        };
                    }
                }
                else
                {
                    return new
                    {
                        Success = false,
                        Message = "No Files To Upload",
                    };

                }
            }
        }
    }
}














