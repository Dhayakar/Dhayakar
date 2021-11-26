using System;
using System.Collections.Generic;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;

namespace WYNK.Data.Repository.Implementation
{
    class OcularComplaintsRepository : RepositoryBase<OcularComplaintsViewModel>, IOcularComplaintsRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;

        public OcularComplaintsRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic InsertOcularComplaints(OcularComplaintsViewModel InsertOcularComplaints)
        {
            if (InsertOcularComplaints.listofcomplaints.Count() > 0)
            {
                foreach (var item in InsertOcularComplaints.listofcomplaints.Where(x => x.ID == 0).ToList())
                {
                    var ocular = new OcularComplaints();
                    var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints"
                   && x.ParentDescription == item.Description).Select(x => x.OLMID).FirstOrDefault();

                    ocular.Description = Desc;
                    ocular.ISOD = item.ISOD;
                    ocular.ISOS = item.ISOS;
                    ocular.ISOU = item.ISOU;
                    ocular.CreatedUTC = DateTime.UtcNow;
                    ocular.UpdatedUTC = null;

                    if (item.Totalmonths == null)
                    {
                        ocular.FromUTC = null;
                    }
                    else
                    {
                        ocular.FromUTC = Convert.ToDateTime(item.Totalmonths);
                    }


                    ocular.CreatedBy = Convert.ToInt32(InsertOcularComplaints.Userid);
                    ocular.RegistrationTranID = item.TransID;
                    WYNKContext.OcularComplaints.Add(ocular);
                    WYNKContext.SaveChanges();
                }
                foreach (var item in InsertOcularComplaints.listofcomplaints.Where(x => x.ID != 0 && x.Actions == "CHANGE").ToList())
                {
                    var ocular = new OcularComplaints();
                    ocular = WYNKContext.OcularComplaints.Where(x => x.ID == item.ID).SingleOrDefault();
                    var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints"
                   && x.ParentDescription == item.Description).Select(x => x.OLMID).FirstOrDefault();
                    ocular.Description = Desc;
                    ocular.ISOD = item.ISOD;
                    ocular.ISOS = item.ISOS;
                    ocular.ISOU = item.ISOU;
                    ocular.UpdatedUTC = DateTime.UtcNow;
                    ocular.CreatedBy = Convert.ToInt32(InsertOcularComplaints.Userid);

                    WYNKContext.OcularComplaints.UpdateRange(ocular);
                    WYNKContext.SaveChanges();
                }
                foreach (var item in InsertOcularComplaints.listofcomplaints.Where(x => x.ID != 0 && x.Actions == "DROP").ToList())
                {
                    var ocularhistory = new OcularComplaintsHistory();
                    var ocular = new OcularComplaints();
                    ocular = WYNKContext.OcularComplaints.Where(x => x.ID == item.ID).FirstOrDefault();

                    var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints"
&& x.ParentDescription == item.Description).Select(x => x.OLMID).FirstOrDefault();


                    ocular.Description = Desc;
                    ocular.ISOD = item.ISOD;
                    ocular.ISOS = item.ISOS;
                    ocular.ISOU = item.ISOU;
                    ocular.UpdatedUTC = DateTime.UtcNow;
                    ocular.CreatedBy = Convert.ToInt32(InsertOcularComplaints.Userid);
                    ocular.IsDeleted = true;
                    WYNKContext.OcularComplaints.UpdateRange(ocular);
                    WYNKContext.SaveChanges();
                }
                var itemss = InsertOcularComplaints.listofcomplaints;
                try
                {
                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Message = "Saved successfully",
                            Action = itemss,
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
            else
            {
                return new
                {
                    Success = false,
                    Message = "Some data are Missing"
                };
            }
        }
        public dynamic updateOcularComplaints(OcularComplaintsViewModel InsertOcularComplaints, int ID)
        {
            try
            {

                var ocular = new OcularComplaints();
                var OLm = new OneLine_Masters();
                ocular = WYNKContext.OcularComplaints.Where(x => x.ID == ID).SingleOrDefault();

                var item = InsertOcularComplaints.listofcomplaints;

                var IID = item.Select(x => x.ID).FirstOrDefault();
                var CID = WYNKContext.OcularComplaints.Where(x => x.ID == IID).Select(x => x.Description).FirstOrDefault();
                OLm = CMPSContext.OneLineMaster.Where(x => x.OLMID == CID).FirstOrDefault();
                string DDESC = item.Select(x => x.Description).FirstOrDefault();
                OLm.ParentDescription = DDESC;

                CMPSContext.OneLineMaster.UpdateRange(OLm);
                CMPSContext.SaveChanges();

                var disc = item.Select(x => x.Description).FirstOrDefault();

                var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints"
                && x.ParentDescription == disc).Select(x => x.OLMID).FirstOrDefault();
                ocular.Description = Desc;
                ocular.ISOD = item.Select(x => x.ISOD).FirstOrDefault();
                ocular.ISOS = item.Select(x => x.ISOS).FirstOrDefault();
                ocular.ISOU = item.Select(x => x.ISOU).FirstOrDefault();
                var tm = item.Select(x => x.Totalmonths).FirstOrDefault();

                if (tm == null)
                {
                    ocular.FromUTC = null;
                }
                else
                {
                    ocular.FromUTC = item.Select(x => Convert.ToDateTime(x.Totalmonths)).FirstOrDefault();
                }


                ocular.UpdatedUTC = DateTime.UtcNow;
                ocular.CreatedBy = Convert.ToInt32(InsertOcularComplaints.Userid);
                WYNKContext.OcularComplaints.UpdateRange(ocular);
                WYNKContext.SaveChanges();
                try
                {
                    if (CMPSContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Message = "update successfully"
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
        public dynamic Deleteocularcomplaints(int ID, string Reasons)
        {


            var M_ocularcomDel = new OcularComplaints();
            M_ocularcomDel = WYNKContext.OcularComplaints.Where(x => x.ID == ID && x.IsDeleted == false).SingleOrDefault();
            var ids = WYNKContext.OcularComplaints.Where(x => x.ID == ID).ToList();

            M_ocularcomDel.IsDeleted = true;
            M_ocularcomDel.Reasons = Reasons;



            WYNKContext.OcularComplaints.UpdateRange(M_ocularcomDel);

            try
            {
                if (WYNKContext.SaveChanges() >= 0)
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
                Message = "Some data are Missing"
            };
        }




        public dynamic InsertnewOcularComplaints(OcularComplaintsViewModel InsertOcularComplaints, string UIN)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {

                    var data = new OcularComplaintsViewModel();
                    var roledescription = InsertOcularComplaints.Role;
                    var cmpids = Convert.ToInt32(InsertOcularComplaints.CMPID);
                    var Regtranid = Convert.ToInt32(InsertOcularComplaints.Registrationtranid);
                    var doctorid = Convert.ToInt32(InsertOcularComplaints.Userid);
                    var usid = Convert.ToInt32(InsertOcularComplaints.usid);
                    var Companyid = InsertOcularComplaints.OcularComplaintsNew.Select(x => x.CMPID).FirstOrDefault();
                    var utctimes = CMPSContext.Setup.Where(x => x.CMPID == cmpids).Select(x => x.UTCTime).FirstOrDefault();
                    var OLMID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Assigned").Select(x => x.OLMID).FirstOrDefault();
                    var userroleids = InsertOcularComplaints.OcularComplaintsNew.Select(x => x.Doctoruserid).FirstOrDefault();

                    TimeSpan utctime = TimeSpan.Parse(utctimes);

                    var CurDate = DateTime.UtcNow;
                    var times1 = CurDate.Hour + ":" + CurDate.Minute + ":" + CurDate.Second;
                    var times2 = utctime;
                    var fulltime = TimeSpan.Parse(times1) + times2;
                    DateTime time = DateTime.Today.Add(fulltime);
                    var curfullh = time.Hour;
                    var curfullm = time.Minute;
                    string displayTimes = curfullh + ":" + curfullm;

                    if (InsertOcularComplaints.Role != "Doctor" && InsertOcularComplaints.Role != "Optometrist")
                    {
                        var role = WYNKContext.Workflowmaster.Where(x => x.Description == "Patient history").Select(x => x.ID).FirstOrDefault();
                        var masters = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == true && x.Doctorid == 0 && x.status == true && x.Workflowmasterid != role).ToList();

                        if (masters != null && masters.Count != 0)
                        {

                            masters.All(x => { x.Checkouttime = displayTimes; x.status = false; return true; });
                            WYNKContext.Workflowtran.UpdateRange(masters);

                        }
                        var master = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == true && x.Doctorid == 0 && x.status == true && x.Workflowmasterid == role).ToList();
                        if (master != null && master.Count != 0)
                        {
                            master.All(x => { x.Checkintime = displayTimes; return true; });
                            WYNKContext.Workflowtran.UpdateRange(master);

                        }
                        else
                        {

                            var workflow = new Workflowtran();
                            workflow.Workflowmasterid = role;
                            workflow.Regtranid = Regtranid;
                            workflow.Reception = true;
                            workflow.Doctor = false;
                            workflow.Optometrist = false;
                            workflow.Checkintime = displayTimes;
                            workflow.Checkouttime = "";
                            workflow.CMPID = cmpids;
                            workflow.Date = DateTime.UtcNow.Date;
                            workflow.Createdutc = DateTime.UtcNow;
                            workflow.Createdby = usid;
                            workflow.status = true;
                            workflow.Doctorid = 0;
                            WYNKContext.Workflowtran.Add(workflow);
                            WYNKContext.SaveChanges();
                        }


                    }

                    else
                    {
                        var role = WYNKContext.Workflowmaster.Where(x => x.Description == "Patient history").Select(x => x.ID).FirstOrDefault();
                        var masters = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == false && x.Doctorid != 0 && x.status == true && x.Workflowmasterid != role).ToList();


                        if (masters != null && masters.Count != 0)
                        {

                            masters.All(x => { x.Checkouttime = displayTimes; x.status = false; return true; });
                            WYNKContext.Workflowtran.UpdateRange(masters);

                        }
                        var master = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == false && x.Doctorid != 0 && x.status == true && x.Workflowmasterid == role).ToList();
                        if (master != null && master.Count != 0)
                        {
                            master.All(x => { x.Checkintime = displayTimes; return true; });
                            WYNKContext.Workflowtran.UpdateRange(master);

                        }
                        else
                        {


                            var workflow = new Workflowtran();
                            workflow.Workflowmasterid = role;
                            workflow.Regtranid = Regtranid;
                            workflow.Reception = false;
                            if (InsertOcularComplaints.Role == "Doctor")
                            {
                                workflow.Doctor = true;
                            }
                            if (InsertOcularComplaints.Role == "Optometrist")
                            {
                                workflow.Optometrist = true;
                            }
                            workflow.Checkintime = displayTimes;
                            workflow.Checkouttime = "";
                            workflow.CMPID = cmpids;
                            workflow.Date = DateTime.UtcNow.Date;
                            workflow.Createdutc = DateTime.UtcNow;
                            workflow.Createdby = usid;
                            workflow.status = true;
                            workflow.Doctorid = doctorid;
                            WYNKContext.Workflowtran.Add(workflow);
                            WYNKContext.SaveChanges();
                        }


                    }

                    if (roledescription == "Doctor")
                    {
                        var patientassign = WYNKContext.PatientAssign.OrderByDescending(x => x.CreatedUTC).Where(x => x.RegistrationTranID == Regtranid && x.DoctorID == doctorid && x.Patientallocatestatus == null).FirstOrDefault();

                        if (patientassign != null)
                        {
                            if (patientassign.DoctorID == doctorid)
                            {
                                var oneline = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints").ToList();
                                var Ocl = WYNKContext.OcularComplaints.ToList();
                                var listcmpid = InsertOcularComplaints.OcularComplaintsNew.Select(x => x.CMPID).FirstOrDefault();
                                var cmpid = Convert.ToInt32(listcmpid);
                                var regtranID = WYNKContext.RegistrationTran.Where(x => x.CmpID == cmpid && x.UIN == UIN).ToList();
                                var UINDETAILS = (from oc in Ocl
                                                  join rg in regtranID on oc.RegistrationTranID equals rg.RegistrationTranID
                                                  select new
                                                  {
                                                      Regid = oc.RegistrationTranID,
                                                  }).ToList();
                                if (UINDETAILS.Count != 0)
                                {

                                    foreach (var list in InsertOcularComplaints.OcularComplaintsNew)
                                    {
                                        var ocular = new OcularComplaints();
                                        if (list.Description != null)
                                        {
                                            var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints" && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                            ocular.Description = Desc;
                                        }
                                        else
                                        {
                                            var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "OcularComplaints").Select(x => x.OLMID).FirstOrDefault();
                                            ocular.Description = Desc;
                                        }
                                        ocular.ISOD = list.ISOD;
                                        ocular.ISOS = list.ISOS;
                                        ocular.ISOU = list.ISOU;
                                        ocular.CreatedUTC = DateTime.UtcNow;
                                        ocular.UpdatedUTC = null;
                                        ocular.Remarks = list.Remarks;
                                        if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                        {
                                            ocular.FromUTC = null;
                                        }
                                        else
                                        {
                                            ocular.FromUTC = Convert.ToDateTime(list.FromDate);
                                        }
                                        ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                        var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                        ocular.RegistrationTranID = tranid;
                                        WYNKContext.OcularComplaints.Add(ocular);
                                        WYNKContext.SaveChanges();
                                    }
                                    var regtranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                    var userroleid = InsertOcularComplaints.OcularComplaintsNew.Select(x => x.Doctoruserid).FirstOrDefault();

                                    var pgeneral = new PatientGeneralDatamodel();
                                    pgeneral.IsDeleted = false;
                                    if (InsertOcularComplaints.familyhistory != null)
                                    {
                                        pgeneral.FamilyHistory = InsertOcularComplaints.familyhistory;
                                    }
                                    else
                                    {
                                        pgeneral.FamilyHistory = "NULL";
                                    }

                                    if (InsertOcularComplaints.Allergy != null)
                                    {
                                        pgeneral.Allergy = InsertOcularComplaints.Allergy;
                                    }
                                    else
                                    {
                                        pgeneral.Allergy = "NULL";
                                    }

                                    pgeneral.RegistrationTranID = regtranid;
                                    pgeneral.UIN = UIN;
                                    pgeneral.VisitDate = DateTime.UtcNow;
                                    pgeneral.CreatedBy = Convert.ToInt32(userroleid);
                                    pgeneral.CreatedUTC = DateTime.UtcNow;
                                    pgeneral.CurrentMedication = "NULL";
                                    WYNKContext.PatientGeneral.Add(pgeneral);
                                    WYNKContext.SaveChanges();


                                    foreach (var list in InsertOcularComplaints.systemicconditionsNew)
                                    {
                                        var ocular = new PatientHistory();


                                        if (list.Description != null)
                                        {
                                            var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "SystemicCondition"
                                      && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                            ocular.Description = Desc;
                                        }
                                        else
                                        {
                                            var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "SystemicCondition").Select(x => x.OLMID).FirstOrDefault();
                                            ocular.Description = Desc;
                                        }
                                        ocular.IsActive = true;
                                        ocular.CreatedUTC = DateTime.UtcNow;
                                        ocular.UpdatedUTC = null;
                                        if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                        {
                                            ocular.FromUTC = null;
                                        }
                                        else
                                        {
                                            ocular.FromUTC = Convert.ToDateTime(list.FromDate);
                                        }
                                        ocular.UIN = UIN;
                                        ocular.Remarks = list.Remarks;
                                        ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                        var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                        ocular.RegistrationTranID = tranid;
                                        WYNKContext.PatientHistory.Add(ocular);
                                        WYNKContext.SaveChanges();
                                    }

                                }
                                else
                                {
                                    foreach (var list in InsertOcularComplaints.OcularComplaintsNew)
                                    {
                                        var ocular = new OcularComplaints();
                                        if (list.Description != null)
                                        {
                                            var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints" && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                            ocular.Description = Desc;
                                        }
                                        else
                                        {
                                            var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "OcularComplaints").Select(x => x.OLMID).FirstOrDefault();
                                            ocular.Description = Desc;
                                        }
                                        ocular.ISOD = list.ISOD;
                                        ocular.ISOS = list.ISOS;
                                        ocular.ISOU = list.ISOU;
                                        ocular.CreatedUTC = DateTime.UtcNow;
                                        ocular.UpdatedUTC = null;
                                        ocular.Remarks = list.Remarks;
                                        if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                        {
                                            ocular.FromUTC = null;
                                        }
                                        else

                                        {
                                            ocular.FromUTC = list.FromDate;
                                        }
                                        ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                        var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                        ocular.RegistrationTranID = tranid;
                                        WYNKContext.OcularComplaints.Add(ocular);
                                        WYNKContext.SaveChanges();
                                    }



                                    foreach (var list in InsertOcularComplaints.systemicconditionsNew)
                                    {
                                        var ocular = new PatientHistory();
                                        if (list.Description != null)
                                        {
                                            var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "SystemicCondition"
                                      && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                            ocular.Description = Desc;
                                        }
                                        else
                                        {
                                            var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "SystemicCondition").Select(x => x.OLMID).FirstOrDefault();
                                            ocular.Description = Desc;
                                        }
                                        ocular.CreatedUTC = DateTime.UtcNow;
                                        ocular.UpdatedUTC = null;
                                        ocular.IsActive = true;
                                        if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                        {
                                            ocular.FromUTC = null;
                                        }
                                        else
                                        {
                                            ocular.FromUTC = Convert.ToDateTime(list.FromDate);
                                        }
                                        ocular.UIN = UIN;
                                        ocular.Remarks = list.Remarks;
                                        ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                        var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                        ocular.RegistrationTranID = tranid;
                                        WYNKContext.PatientHistory.Add(ocular);
                                        WYNKContext.SaveChanges();
                                    }
                                    var regtranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                    var userroleid = InsertOcularComplaints.OcularComplaintsNew.Select(x => x.Doctoruserid).FirstOrDefault();

                                    var pgeneral = new PatientGeneralDatamodel();
                                    pgeneral.IsDeleted = false;
                                    if (InsertOcularComplaints.familyhistory != null)
                                    {
                                        pgeneral.FamilyHistory = InsertOcularComplaints.familyhistory;
                                    }
                                    else
                                    {
                                        pgeneral.FamilyHistory = "NULL";
                                    }

                                    if (InsertOcularComplaints.Allergy != null)
                                    {
                                        pgeneral.Allergy = InsertOcularComplaints.Allergy;
                                    }
                                    else
                                    {
                                        pgeneral.Allergy = "NULL";
                                    }

                                    pgeneral.RegistrationTranID = regtranid;
                                    pgeneral.UIN = UIN;
                                    pgeneral.VisitDate = DateTime.UtcNow;
                                    pgeneral.CreatedBy = Convert.ToInt32(userroleid);
                                    pgeneral.CurrentMedication = "NULL";
                                    pgeneral.CreatedUTC = DateTime.UtcNow;
                                    WYNKContext.PatientGeneral.Add(pgeneral);
                                    WYNKContext.SaveChanges();

                                }

                                if (InsertOcularComplaints.CurrentMedications != null)
                                {

                                    var PatCurrentMedication = WYNKContext.PatientCurrentMedication.Where(x => x.UIN == UIN);
                                    if (PatCurrentMedication != null)
                                    {
                                        WYNKContext.PatientCurrentMedication.RemoveRange(WYNKContext.PatientCurrentMedication.Where(x => x.UIN == UIN).ToList());
                                        WYNKContext.SaveChanges();
                                    }


                                    foreach (var item in InsertOcularComplaints.CurrentMedications.ToList())
                                    {
                                        var PatCurMedication = new PatientCurrentMedication();
                                        PatCurMedication.GenericDrugDescription = item.GenericDescription;
                                        PatCurMedication.RegistrationTranID = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                        PatCurMedication.UIN = UIN;
                                        PatCurMedication.VisitDate = DateTime.Now;
                                        if (item.Eye == "OD")
                                        {
                                            PatCurMedication.IsOD = true;
                                        }
                                        else if (item.Eye == "OS")
                                        {
                                            PatCurMedication.IsOS = true;
                                        }
                                        else if (item.Eye == "OU")
                                        {
                                            PatCurMedication.IsOU = true;
                                        }
                                        if (item.YearMonths == "")
                                        {
                                            PatCurMedication.Since = item.YearMonths != "" ? Convert.ToDateTime(item.YearMonths) : (DateTime?)null;
                                        }
                                        else if (item.YearMonths == " ")
                                        {
                                            PatCurMedication.Since = item.YearMonths != " " ? Convert.ToDateTime(item.YearMonths) : (DateTime?)null;
                                        }
                                        else
                                        {
                                            PatCurMedication.Since = Convert.ToDateTime(item.YearMonths);
                                        }
                                        PatCurMedication.ProgressStatusID = Convert.ToInt32(item.Status);
                                        PatCurMedication.Frequency = item.FrequencyID;
                                        PatCurMedication.PrescribedByDoctorName = item.PrescribedDoctor;
                                        PatCurMedication.Remarks = item.Remarks;
                                        PatCurMedication.Cmpid = cmpids;
                                        WYNKContext.PatientCurrentMedication.Add(PatCurMedication);
                                    }
                                    WYNKContext.SaveChanges();
                                }
                            }
                            else
                            {
                                var doctornmae = CMPSContext.DoctorMaster.Where(x => x.DoctorID == patientassign.DoctorID).Select(x => x.Firstname).FirstOrDefault();
                                var doctorlastnmae = CMPSContext.DoctorMaster.Where(x => x.DoctorID == patientassign.DoctorID).Select(x => x.LastName).FirstOrDefault();
                                data.Submitstatus = "Patient already undergone to " + doctornmae + " " + doctorlastnmae;
                                data.Messagestatus = "Patient already undergone";
                            }
                        }
                        else
                        {

                            var Patienttrandetails = WYNKContext.PatientAssign.Where(x => x.RegistrationTranID == Regtranid && x.Patientallocatestatus == "D").FirstOrDefault();
                            Patienttrandetails.DoctorID = Convert.ToInt32(doctorid);
                            Patienttrandetails.Patientallocatestatus = null;
                            Patienttrandetails.AssignedDate = DateTime.UtcNow + utctime;
                            WYNKContext.PatientAssign.UpdateRange(Patienttrandetails);
                            WYNKContext.SaveChanges();

                            var Regtrandetails = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == Regtranid).FirstOrDefault();
                            Regtrandetails.Status = OLMID;
                            Regtrandetails.DoctorID = Convert.ToInt32(doctorid);
                            Regtrandetails.AssignedDate = DateTime.UtcNow + utctime;
                            WYNKContext.RegistrationTran.UpdateRange(Regtrandetails);
                            WYNKContext.SaveChanges();

                            var oneline = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints").ToList();
                            var Ocl = WYNKContext.OcularComplaints.ToList();
                            var listcmpid = InsertOcularComplaints.OcularComplaintsNew.Select(x => x.CMPID).FirstOrDefault();
                            var cmpid = Convert.ToInt32(listcmpid);
                            var regtranID = WYNKContext.RegistrationTran.Where(x => x.CmpID == cmpid && x.UIN == UIN).ToList();
                            var UINDETAILS = (from oc in Ocl
                                              join rg in regtranID on oc.RegistrationTranID equals rg.RegistrationTranID
                                              select new
                                              {
                                                  Regid = oc.RegistrationTranID,
                                              }).ToList();
                            if (UINDETAILS.Count != 0)
                            {

                                foreach (var list in InsertOcularComplaints.OcularComplaintsNew)
                                {
                                    var ocular = new OcularComplaints();
                                    if (list.Description != null)
                                    {
                                        var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints" && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                        ocular.Description = Desc;
                                    }
                                    else
                                    {
                                        var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "OcularComplaints").Select(x => x.OLMID).FirstOrDefault();
                                        ocular.Description = Desc;
                                    }
                                    ocular.ISOD = list.ISOD;
                                    ocular.ISOS = list.ISOS;
                                    ocular.ISOU = list.ISOU;
                                    ocular.CreatedUTC = DateTime.UtcNow;
                                    ocular.Remarks = list.Remarks;
                                    ocular.UpdatedUTC = null;

                                    if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                    {
                                        ocular.FromUTC = null;
                                    }
                                    else
                                    {
                                        ocular.FromUTC = Convert.ToDateTime(list.FromDate);
                                    }
                                    ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                    var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                    ocular.RegistrationTranID = tranid;
                                    WYNKContext.OcularComplaints.Add(ocular);
                                    WYNKContext.SaveChanges();
                                }
                                var regtranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                var userroleid = InsertOcularComplaints.OcularComplaintsNew.Select(x => x.Doctoruserid).FirstOrDefault();

                                var pgeneral = new PatientGeneralDatamodel();
                                pgeneral.IsDeleted = false;
                                if (InsertOcularComplaints.familyhistory != null)
                                {
                                    pgeneral.FamilyHistory = InsertOcularComplaints.familyhistory;
                                }
                                else
                                {
                                    pgeneral.FamilyHistory = "NULL";
                                }

                                pgeneral.RegistrationTranID = regtranid;
                                pgeneral.UIN = UIN;
                                pgeneral.VisitDate = DateTime.UtcNow;
                                pgeneral.CreatedBy = Convert.ToInt32(userroleid);
                                pgeneral.CreatedUTC = DateTime.UtcNow;

                                if (InsertOcularComplaints.Allergy != null)
                                {
                                    pgeneral.Allergy = InsertOcularComplaints.Allergy;
                                }
                                else
                                {
                                    pgeneral.Allergy = "NULL";
                                }

                                pgeneral.CurrentMedication = "NULL";
                                WYNKContext.PatientGeneral.Add(pgeneral);
                                WYNKContext.SaveChanges();


                                foreach (var list in InsertOcularComplaints.systemicconditionsNew)
                                {
                                    var ocular = new PatientHistory();
                                    if (list.Description != null)
                                    {
                                        var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "SystemicCondition"
                                  && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                        ocular.Description = Desc;
                                    }
                                    else
                                    {
                                        var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "SystemicCondition").Select(x => x.OLMID).FirstOrDefault();
                                        ocular.Description = Desc;
                                    }
                                    ocular.IsActive = true;
                                    ocular.CreatedUTC = DateTime.UtcNow;
                                    ocular.UpdatedUTC = null;
                                    ocular.Remarks = list.Remarks;
                                    if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                    {
                                        ocular.FromUTC = null;
                                    }
                                    else
                                    {
                                        ocular.FromUTC = Convert.ToDateTime(list.FromDate);
                                    }
                                    ocular.UIN = UIN;
                                    ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                    var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                    ocular.RegistrationTranID = tranid;
                                    WYNKContext.PatientHistory.Add(ocular);
                                    WYNKContext.SaveChanges();
                                }

                            }
                            else
                            {
                                foreach (var list in InsertOcularComplaints.OcularComplaintsNew)
                                {
                                    var ocular = new OcularComplaints();
                                    if (list.Description != null)
                                    {
                                        var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints" && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                        ocular.Description = Desc;
                                    }
                                    else
                                    {
                                        var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "OcularComplaints").Select(x => x.OLMID).FirstOrDefault();
                                        ocular.Description = Desc;
                                    }
                                    ocular.ISOD = list.ISOD;
                                    ocular.ISOS = list.ISOS;
                                    ocular.ISOU = list.ISOU;
                                    ocular.Remarks = list.Remarks;
                                    ocular.CreatedUTC = DateTime.UtcNow;
                                    ocular.UpdatedUTC = null;

                                    if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                    {
                                        ocular.FromUTC = null;
                                    }
                                    else

                                    {
                                        ocular.FromUTC = list.FromDate;
                                    }
                                    ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                    var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                    ocular.RegistrationTranID = tranid;
                                    WYNKContext.OcularComplaints.Add(ocular);
                                    WYNKContext.SaveChanges();
                                }



                                foreach (var list in InsertOcularComplaints.systemicconditionsNew)
                                {
                                    var ocular = new PatientHistory();
                                    if (list.Description != null)
                                    {
                                        var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "SystemicCondition"
                                  && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                        ocular.Description = Desc;
                                    }
                                    else
                                    {
                                        var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "SystemicCondition").Select(x => x.OLMID).FirstOrDefault();
                                        ocular.Description = Desc;
                                    }

                                    ocular.CreatedUTC = DateTime.UtcNow;
                                    ocular.UpdatedUTC = null;
                                    ocular.IsActive = true;
                                    if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                    {
                                        ocular.FromUTC = null;
                                    }
                                    else
                                    {
                                        ocular.FromUTC = Convert.ToDateTime(list.FromDate);
                                    }
                                    ocular.UIN = UIN;
                                    ocular.Remarks = list.Remarks;
                                    ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                    var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                    ocular.RegistrationTranID = tranid;
                                    WYNKContext.PatientHistory.Add(ocular);
                                    WYNKContext.SaveChanges();
                                }
                                var regtranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                var userroleid = InsertOcularComplaints.OcularComplaintsNew.Select(x => x.Doctoruserid).FirstOrDefault();

                                var pgeneral = new PatientGeneralDatamodel();
                                pgeneral.IsDeleted = false;
                                if (InsertOcularComplaints.familyhistory != null)
                                {
                                    pgeneral.FamilyHistory = InsertOcularComplaints.familyhistory;
                                }
                                else
                                {
                                    pgeneral.FamilyHistory = "NULL";
                                }
                                pgeneral.RegistrationTranID = regtranid;
                                pgeneral.UIN = UIN;
                                pgeneral.VisitDate = DateTime.UtcNow;
                                pgeneral.CreatedBy = Convert.ToInt32(userroleid);
                                pgeneral.CurrentMedication = "NULL";

                                if (InsertOcularComplaints.Allergy != null)
                                {
                                    pgeneral.Allergy = InsertOcularComplaints.Allergy;
                                }
                                else
                                {
                                    pgeneral.Allergy = "NULL";
                                }

                                pgeneral.CreatedUTC = DateTime.UtcNow;
                                WYNKContext.PatientGeneral.Add(pgeneral);
                                WYNKContext.SaveChanges();

                            }

                            if (InsertOcularComplaints.CurrentMedications != null)
                            {

                                var PatCurrentMedication = WYNKContext.PatientCurrentMedication.Where(x => x.UIN == UIN);
                                if (PatCurrentMedication != null)
                                {
                                    WYNKContext.PatientCurrentMedication.RemoveRange(WYNKContext.PatientCurrentMedication.Where(x => x.UIN == UIN).ToList());
                                    WYNKContext.SaveChanges();
                                }


                                foreach (var item in InsertOcularComplaints.CurrentMedications.ToList())
                                {
                                    var PatCurMedication = new PatientCurrentMedication();
                                    PatCurMedication.GenericDrugDescription = item.GenericDescription;
                                    PatCurMedication.RegistrationTranID = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                    PatCurMedication.UIN = UIN;
                                    PatCurMedication.VisitDate = DateTime.Now;
                                    if (item.Eye == "OD")
                                    {
                                        PatCurMedication.IsOD = true;
                                    }
                                    else if (item.Eye == "OS")
                                    {
                                        PatCurMedication.IsOS = true;
                                    }
                                    else if (item.Eye == "OU")
                                    {
                                        PatCurMedication.IsOU = true;
                                    }
                                    if (item.YearMonths == "")
                                    {
                                        PatCurMedication.Since = item.YearMonths != "" ? Convert.ToDateTime(item.YearMonths) : (DateTime?)null;
                                    }
                                    else if (item.YearMonths == " ")
                                    {
                                        PatCurMedication.Since = item.YearMonths != " " ? Convert.ToDateTime(item.YearMonths) : (DateTime?)null;
                                    }
                                    else
                                    {
                                        PatCurMedication.Since = Convert.ToDateTime(item.YearMonths);
                                    }
                                    PatCurMedication.ProgressStatusID = Convert.ToInt32(item.Status);
                                    PatCurMedication.Frequency = item.FrequencyID;
                                    PatCurMedication.PrescribedByDoctorName = item.PrescribedDoctor;
                                    PatCurMedication.Remarks = item.Remarks;
                                    PatCurMedication.Cmpid = cmpids;
                                    WYNKContext.PatientCurrentMedication.Add(PatCurMedication);
                                }
                                WYNKContext.SaveChanges();
                            }


                        }

                    }
                    else if (roledescription == "Optometrist")
                    {
                        var patientassign = WYNKContext.PatientAssign.OrderByDescending(x => x.CreatedUTC).Where(x => x.RegistrationTranID == Regtranid && x.DoctorID == doctorid && x.Patientallocatestatus == null).FirstOrDefault();

                        if (patientassign != null)
                        {
                            if (patientassign.DoctorID == doctorid)
                            {
                                var oneline = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints").ToList();
                                var Ocl = WYNKContext.OcularComplaints.ToList();
                                var listcmpid = InsertOcularComplaints.OcularComplaintsNew.Select(x => x.CMPID).FirstOrDefault();
                                var cmpid = Convert.ToInt32(listcmpid);
                                var regtranID = WYNKContext.RegistrationTran.Where(x => x.CmpID == cmpid && x.UIN == UIN).ToList();
                                var UINDETAILS = (from oc in Ocl
                                                  join rg in regtranID on oc.RegistrationTranID equals rg.RegistrationTranID
                                                  select new
                                                  {
                                                      Regid = oc.RegistrationTranID,
                                                  }).ToList();
                                if (UINDETAILS.Count != 0)
                                {

                                    foreach (var list in InsertOcularComplaints.OcularComplaintsNew)
                                    {
                                        var ocular = new OcularComplaints();
                                        if (list.Description != null)
                                        {
                                            var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints" && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                            ocular.Description = Desc;
                                        }
                                        else
                                        {
                                            var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "OcularComplaints").Select(x => x.OLMID).FirstOrDefault();
                                            ocular.Description = Desc;
                                        }
                                        ocular.ISOD = list.ISOD;
                                        ocular.ISOS = list.ISOS;
                                        ocular.ISOU = list.ISOU;
                                        ocular.CreatedUTC = DateTime.UtcNow;
                                        ocular.Remarks = list.Remarks;
                                        ocular.UpdatedUTC = null;

                                        if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                        {
                                            ocular.FromUTC = null;
                                        }
                                        else
                                        {
                                            ocular.FromUTC = Convert.ToDateTime(list.FromDate);
                                        }
                                        ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                        var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                        ocular.RegistrationTranID = tranid;
                                        WYNKContext.OcularComplaints.Add(ocular);
                                        WYNKContext.SaveChanges();
                                    }
                                    var regtranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                    var userroleid = InsertOcularComplaints.OcularComplaintsNew.Select(x => x.Doctoruserid).FirstOrDefault();

                                    var pgeneral = new PatientGeneralDatamodel();
                                    pgeneral.IsDeleted = false;
                                    if (InsertOcularComplaints.familyhistory != null)
                                    {
                                        pgeneral.FamilyHistory = InsertOcularComplaints.familyhistory;
                                    }
                                    else
                                    {
                                        pgeneral.FamilyHistory = "NULL";
                                    }

                                    pgeneral.RegistrationTranID = regtranid;
                                    pgeneral.UIN = UIN;
                                    pgeneral.VisitDate = DateTime.UtcNow;
                                    pgeneral.CreatedBy = Convert.ToInt32(userroleid);
                                    pgeneral.CreatedUTC = DateTime.UtcNow;

                                    if (InsertOcularComplaints.Allergy != null)
                                    {
                                        pgeneral.Allergy = InsertOcularComplaints.Allergy;
                                    }
                                    else
                                    {
                                        pgeneral.Allergy = "NULL";
                                    }

                                    pgeneral.CurrentMedication = "NULL";
                                    WYNKContext.PatientGeneral.Add(pgeneral);
                                    WYNKContext.SaveChanges();


                                    foreach (var list in InsertOcularComplaints.systemicconditionsNew)
                                    {
                                        var ocular = new PatientHistory();
                                        if (list.Description != null)
                                        {
                                            var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "SystemicCondition"
                                      && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                            ocular.Description = Desc;
                                        }
                                        else
                                        {
                                            var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "SystemicCondition").Select(x => x.OLMID).FirstOrDefault();
                                            ocular.Description = Desc;
                                        }
                                        ocular.IsActive = true;
                                        ocular.CreatedUTC = DateTime.UtcNow;
                                        ocular.UpdatedUTC = null;
                                        ocular.Remarks = list.Remarks;
                                        if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                        {
                                            ocular.FromUTC = null;
                                        }
                                        else
                                        {
                                            ocular.FromUTC = Convert.ToDateTime(list.FromDate);
                                        }
                                        ocular.UIN = UIN;
                                        ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                        var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                        ocular.RegistrationTranID = tranid;
                                        WYNKContext.PatientHistory.Add(ocular);
                                        WYNKContext.SaveChanges();
                                    }

                                }
                                else
                                {
                                    foreach (var list in InsertOcularComplaints.OcularComplaintsNew)
                                    {
                                        var ocular = new OcularComplaints();
                                        if (list.Description != null)
                                        {
                                            var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints" && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                            ocular.Description = Desc;
                                        }
                                        else
                                        {
                                            var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "OcularComplaints").Select(x => x.OLMID).FirstOrDefault();
                                            ocular.Description = Desc;
                                        }
                                        ocular.ISOD = list.ISOD;
                                        ocular.ISOS = list.ISOS;
                                        ocular.ISOU = list.ISOU;
                                        ocular.CreatedUTC = DateTime.UtcNow;
                                        ocular.UpdatedUTC = null;
                                        ocular.Remarks = list.Remarks;
                                        if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                        {
                                            ocular.FromUTC = null;
                                        }
                                        else

                                        {
                                            ocular.FromUTC = list.FromDate;
                                        }
                                        ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                        var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                        ocular.RegistrationTranID = tranid;
                                        WYNKContext.OcularComplaints.Add(ocular);
                                        WYNKContext.SaveChanges();
                                    }



                                    foreach (var list in InsertOcularComplaints.systemicconditionsNew)
                                    {
                                        var ocular = new PatientHistory();
                                        if (list.Description != null)
                                        {
                                            var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "SystemicCondition"
                                      && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                            ocular.Description = Desc;
                                        }
                                        else
                                        {
                                            var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "SystemicCondition").Select(x => x.OLMID).FirstOrDefault();
                                            ocular.Description = Desc;
                                        }
                                        ocular.CreatedUTC = DateTime.UtcNow;
                                        ocular.UpdatedUTC = null;
                                        ocular.IsActive = true;
                                        if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                        {
                                            ocular.FromUTC = null;
                                        }
                                        else
                                        {
                                            ocular.FromUTC = Convert.ToDateTime(list.FromDate);
                                        }
                                        ocular.UIN = UIN;
                                        ocular.Remarks = list.Remarks;
                                        ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                        var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                        ocular.RegistrationTranID = tranid;
                                        WYNKContext.PatientHistory.Add(ocular);
                                        WYNKContext.SaveChanges();
                                    }
                                    var regtranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                    var userroleid = InsertOcularComplaints.OcularComplaintsNew.Select(x => x.Doctoruserid).FirstOrDefault();

                                    var pgeneral = new PatientGeneralDatamodel();
                                    pgeneral.IsDeleted = false;
                                    if (InsertOcularComplaints.familyhistory != null)
                                    {
                                        pgeneral.FamilyHistory = InsertOcularComplaints.familyhistory;
                                    }
                                    else
                                    {
                                        pgeneral.FamilyHistory = "NULL";
                                    }
                                    pgeneral.RegistrationTranID = regtranid;
                                    pgeneral.UIN = UIN;
                                    pgeneral.VisitDate = DateTime.UtcNow;
                                    pgeneral.CreatedBy = Convert.ToInt32(userroleid);
                                    pgeneral.CurrentMedication = "NULL";

                                    if (InsertOcularComplaints.Allergy != null)
                                    {
                                        pgeneral.Allergy = InsertOcularComplaints.Allergy;
                                    }
                                    else
                                    {
                                        pgeneral.Allergy = "NULL";
                                    }

                                    pgeneral.CreatedUTC = DateTime.UtcNow;
                                    WYNKContext.PatientGeneral.Add(pgeneral);
                                    WYNKContext.SaveChanges();

                                }

                                if (InsertOcularComplaints.CurrentMedications != null)
                                {

                                    var PatCurrentMedication = WYNKContext.PatientCurrentMedication.Where(x => x.UIN == UIN);
                                    if (PatCurrentMedication != null)
                                    {
                                        WYNKContext.PatientCurrentMedication.RemoveRange(WYNKContext.PatientCurrentMedication.Where(x => x.UIN == UIN).ToList());
                                        WYNKContext.SaveChanges();
                                    }


                                    foreach (var item in InsertOcularComplaints.CurrentMedications.ToList())
                                    {
                                        var PatCurMedication = new PatientCurrentMedication();
                                        PatCurMedication.GenericDrugDescription = item.GenericDescription;
                                        PatCurMedication.RegistrationTranID = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                        PatCurMedication.UIN = UIN;
                                        PatCurMedication.VisitDate = DateTime.Now;
                                        if (item.Eye == "OD")
                                        {
                                            PatCurMedication.IsOD = true;
                                        }
                                        else if (item.Eye == "OS")
                                        {
                                            PatCurMedication.IsOS = true;
                                        }
                                        else if (item.Eye == "OU")
                                        {
                                            PatCurMedication.IsOU = true;
                                        }
                                        if (item.YearMonths == "")
                                        {
                                            PatCurMedication.Since = item.YearMonths != "" ? Convert.ToDateTime(item.YearMonths) : (DateTime?)null;
                                        }
                                        else if (item.YearMonths == " ")
                                        {
                                            PatCurMedication.Since = item.YearMonths != " " ? Convert.ToDateTime(item.YearMonths) : (DateTime?)null;
                                        }
                                        else
                                        {
                                            PatCurMedication.Since = Convert.ToDateTime(item.YearMonths);
                                        }
                                        PatCurMedication.ProgressStatusID = Convert.ToInt32(item.Status);
                                        PatCurMedication.Frequency = item.FrequencyID;
                                        PatCurMedication.PrescribedByDoctorName = item.PrescribedDoctor;
                                        PatCurMedication.Remarks = item.Remarks;
                                        PatCurMedication.Cmpid = cmpids;
                                        WYNKContext.PatientCurrentMedication.Add(PatCurMedication);
                                    }
                                    WYNKContext.SaveChanges();
                                }
                            }
                            else
                            {
                                var doctornmae = CMPSContext.DoctorMaster.Where(x => x.DoctorID == patientassign.DoctorID).Select(x => x.Firstname).FirstOrDefault();
                                var doctorlastnmae = CMPSContext.DoctorMaster.Where(x => x.DoctorID == patientassign.DoctorID).Select(x => x.LastName).FirstOrDefault();
                                data.Submitstatus = "Patient already undergone to " + doctornmae + " " + doctorlastnmae;
                                data.Messagestatus = "Patient already undergone";
                            }
                        }
                        else
                        {

                            var Patienttrandetails = WYNKContext.PatientAssign.Where(x => x.RegistrationTranID == Regtranid && x.Patientallocatestatus == "O").FirstOrDefault();
                            Patienttrandetails.DoctorID = Convert.ToInt32(doctorid);
                            Patienttrandetails.Patientallocatestatus = null;
                            Patienttrandetails.AssignedDate = DateTime.UtcNow + utctime;
                            WYNKContext.PatientAssign.UpdateRange(Patienttrandetails);
                            WYNKContext.SaveChanges();

                            var oneline = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints").ToList();
                            var Ocl = WYNKContext.OcularComplaints.ToList();
                            var cmpid = cmpids;
                            var regtranID = WYNKContext.RegistrationTran.Where(x => x.CmpID == cmpid && x.UIN == UIN).ToList();
                            var UINDETAILS = (from oc in Ocl
                                              join rg in regtranID on oc.RegistrationTranID equals rg.RegistrationTranID
                                              select new
                                              {
                                                  Regid = oc.RegistrationTranID,
                                              }).ToList();
                            if (UINDETAILS.Count != 0)
                            {

                                foreach (var list in InsertOcularComplaints.OcularComplaintsNew)
                                {
                                    var ocular = new OcularComplaints();
                                    if (list.Description != null)
                                    {
                                        var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints" && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                        ocular.Description = Desc;
                                    }
                                    else
                                    {
                                        var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "OcularComplaints").Select(x => x.OLMID).FirstOrDefault();
                                        ocular.Description = Desc;
                                    }
                                    ocular.ISOD = list.ISOD;
                                    ocular.ISOS = list.ISOS;
                                    ocular.ISOU = list.ISOU;
                                    ocular.CreatedUTC = DateTime.UtcNow;
                                    ocular.Remarks = list.Remarks;
                                    ocular.UpdatedUTC = null;

                                    if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                    {
                                        ocular.FromUTC = null;
                                    }
                                    else
                                    {
                                        ocular.FromUTC = Convert.ToDateTime(list.FromDate);
                                    }
                                    ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                    var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                    ocular.RegistrationTranID = tranid;
                                    WYNKContext.OcularComplaints.Add(ocular);
                                    WYNKContext.SaveChanges();
                                }
                                var regtranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                var userroleid = InsertOcularComplaints.OcularComplaintsNew.Select(x => x.Doctoruserid).FirstOrDefault();

                                var pgeneral = new PatientGeneralDatamodel();
                                pgeneral.IsDeleted = false;
                                if (InsertOcularComplaints.familyhistory != null)
                                {
                                    pgeneral.FamilyHistory = InsertOcularComplaints.familyhistory;
                                }
                                else
                                {
                                    pgeneral.FamilyHistory = "NULL";
                                }

                                pgeneral.RegistrationTranID = regtranid;
                                pgeneral.UIN = UIN;
                                pgeneral.VisitDate = DateTime.UtcNow;
                                pgeneral.CreatedBy = Convert.ToInt32(userroleid);
                                pgeneral.CreatedUTC = DateTime.UtcNow;

                                if (InsertOcularComplaints.Allergy != null)
                                {
                                    pgeneral.Allergy = InsertOcularComplaints.Allergy;
                                }
                                else
                                {
                                    pgeneral.Allergy = "NULL";
                                }

                                pgeneral.CurrentMedication = "NULL";
                                WYNKContext.PatientGeneral.Add(pgeneral);
                                WYNKContext.SaveChanges();


                                foreach (var list in InsertOcularComplaints.systemicconditionsNew)
                                {
                                    var ocular = new PatientHistory();
                                    if (list.Description != null)
                                    {
                                        var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "SystemicCondition"
                                  && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                        ocular.Description = Desc;
                                    }
                                    else
                                    {
                                        var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "SystemicCondition").Select(x => x.OLMID).FirstOrDefault();
                                        ocular.Description = Desc;
                                    }
                                    ocular.IsActive = true;
                                    ocular.CreatedUTC = DateTime.UtcNow;
                                    ocular.UpdatedUTC = null;
                                    ocular.Remarks = list.Remarks;
                                    if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                    {
                                        ocular.FromUTC = null;
                                    }
                                    else
                                    {
                                        ocular.FromUTC = Convert.ToDateTime(list.FromDate);
                                    }
                                    ocular.UIN = UIN;
                                    ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                    var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                    ocular.RegistrationTranID = tranid;
                                    WYNKContext.PatientHistory.Add(ocular);
                                    WYNKContext.SaveChanges();
                                }

                            }
                            else
                            {
                                foreach (var list in InsertOcularComplaints.OcularComplaintsNew)
                                {
                                    var ocular = new OcularComplaints();
                                    if (list.Description != null)
                                    {
                                        var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints" && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                        ocular.Description = Desc;
                                    }
                                    else
                                    {
                                        var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "OcularComplaints").Select(x => x.OLMID).FirstOrDefault();
                                        ocular.Description = Desc;
                                    }
                                    ocular.ISOD = list.ISOD;
                                    ocular.ISOS = list.ISOS;
                                    ocular.ISOU = list.ISOU;
                                    ocular.CreatedUTC = DateTime.UtcNow;
                                    ocular.UpdatedUTC = null;
                                    ocular.Remarks = list.Remarks;
                                    if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                    {
                                        ocular.FromUTC = null;
                                    }
                                    else

                                    {
                                        ocular.FromUTC = list.FromDate;
                                    }
                                    ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                    var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                    ocular.RegistrationTranID = tranid;
                                    WYNKContext.OcularComplaints.Add(ocular);
                                    WYNKContext.SaveChanges();
                                }



                                foreach (var list in InsertOcularComplaints.systemicconditionsNew)
                                {
                                    var ocular = new PatientHistory();
                                    if (list.Description != null)
                                    {
                                        var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "SystemicCondition"
                                  && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                        ocular.Description = Desc;
                                    }
                                    else
                                    {
                                        var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "SystemicCondition").Select(x => x.OLMID).FirstOrDefault();
                                        ocular.Description = Desc;
                                    }
                                    ocular.CreatedUTC = DateTime.UtcNow;
                                    ocular.UpdatedUTC = null;
                                    ocular.IsActive = true;
                                    if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                    {
                                        ocular.FromUTC = null;
                                    }
                                    else
                                    {
                                        ocular.FromUTC = Convert.ToDateTime(list.FromDate);
                                    }
                                    ocular.UIN = UIN;
                                    ocular.Remarks = list.Remarks;
                                    ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                    var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                    ocular.RegistrationTranID = tranid;
                                    WYNKContext.PatientHistory.Add(ocular);
                                    WYNKContext.SaveChanges();
                                }
                                var regtranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                var userroleid = InsertOcularComplaints.OcularComplaintsNew.Select(x => x.Doctoruserid).FirstOrDefault();

                                var pgeneral = new PatientGeneralDatamodel();
                                pgeneral.IsDeleted = false;
                                if (InsertOcularComplaints.familyhistory != null)
                                {
                                    pgeneral.FamilyHistory = InsertOcularComplaints.familyhistory;
                                }
                                else
                                {
                                    pgeneral.FamilyHistory = "NULL";
                                }
                                pgeneral.RegistrationTranID = regtranid;
                                pgeneral.UIN = UIN;
                                pgeneral.VisitDate = DateTime.UtcNow;
                                pgeneral.CreatedBy = Convert.ToInt32(userroleid);
                                pgeneral.CurrentMedication = "NULL";

                                if (InsertOcularComplaints.Allergy != null)
                                {
                                    pgeneral.Allergy = InsertOcularComplaints.Allergy;
                                }
                                else
                                {
                                    pgeneral.Allergy = "NULL";
                                }

                                pgeneral.CreatedUTC = DateTime.UtcNow;
                                WYNKContext.PatientGeneral.Add(pgeneral);
                                WYNKContext.SaveChanges();

                            }

                            if (InsertOcularComplaints.CurrentMedications != null)
                            {

                                var PatCurrentMedication = WYNKContext.PatientCurrentMedication.Where(x => x.UIN == UIN);
                                if (PatCurrentMedication != null)
                                {
                                    WYNKContext.PatientCurrentMedication.RemoveRange(WYNKContext.PatientCurrentMedication.Where(x => x.UIN == UIN).ToList());
                                    WYNKContext.SaveChanges();
                                }


                                foreach (var item in InsertOcularComplaints.CurrentMedications.ToList())
                                {
                                    var PatCurMedication = new PatientCurrentMedication();
                                    PatCurMedication.GenericDrugDescription = item.GenericDescription;
                                    PatCurMedication.RegistrationTranID = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                    PatCurMedication.UIN = UIN;
                                    PatCurMedication.VisitDate = DateTime.Now;
                                    if (item.Eye == "OD")
                                    {
                                        PatCurMedication.IsOD = true;
                                    }
                                    else if (item.Eye == "OS")
                                    {
                                        PatCurMedication.IsOS = true;
                                    }
                                    else if (item.Eye == "OU")
                                    {
                                        PatCurMedication.IsOU = true;
                                    }
                                    if (item.YearMonths == "")
                                    {
                                        PatCurMedication.Since = item.YearMonths != "" ? Convert.ToDateTime(item.YearMonths) : (DateTime?)null;
                                    }
                                    else if (item.YearMonths == " ")
                                    {
                                        PatCurMedication.Since = item.YearMonths != " " ? Convert.ToDateTime(item.YearMonths) : (DateTime?)null;
                                    }
                                    else
                                    {
                                        PatCurMedication.Since = Convert.ToDateTime(item.YearMonths);
                                    }
                                    PatCurMedication.ProgressStatusID = Convert.ToInt32(item.Status);
                                    PatCurMedication.Frequency = item.FrequencyID;
                                    PatCurMedication.PrescribedByDoctorName = item.PrescribedDoctor;
                                    PatCurMedication.Remarks = item.Remarks;
                                    PatCurMedication.Cmpid = cmpids;
                                    WYNKContext.PatientCurrentMedication.Add(PatCurMedication);
                                }
                                WYNKContext.SaveChanges();
                            }


                        }

                    }
                    else
                    {

                        var oneline = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints").ToList();
                        var Ocl = WYNKContext.OcularComplaints.ToList();
                        var listcmpid = InsertOcularComplaints.OcularComplaintsNew.Select(x => x.CMPID).FirstOrDefault();
                        var cmpid = Convert.ToInt32(listcmpid);
                        var regtranID = WYNKContext.RegistrationTran.Where(x => x.CmpID == cmpid && x.UIN == UIN).ToList();
                        var UINDETAILS = (from oc in Ocl
                                          join rg in regtranID on oc.RegistrationTranID equals rg.RegistrationTranID
                                          select new
                                          {
                                              Regid = oc.RegistrationTranID,
                                          }).ToList();
                        if (UINDETAILS.Count != 0)
                        {
                            foreach (var list in InsertOcularComplaints.OcularComplaintsNew)
                            {
                                var ocular = new OcularComplaints();


                                if (list.Description != null)
                                {
                                    var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints" && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                    ocular.Description = Desc;
                                }
                                else
                                {
                                    var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "OcularComplaints").Select(x => x.OLMID).FirstOrDefault();
                                    ocular.Description = Desc;
                                }
                                ocular.ISOD = list.ISOD;
                                ocular.ISOS = list.ISOS;
                                ocular.ISOU = list.ISOU;
                                ocular.CreatedUTC = DateTime.UtcNow;
                                ocular.UpdatedUTC = null;
                                ocular.Remarks = list.Remarks;
                                if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                {
                                    ocular.FromUTC = null;
                                }
                                else
                                {
                                    ocular.FromUTC = Convert.ToDateTime(list.FromDate);
                                }
                                ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                ocular.RegistrationTranID = tranid;
                                WYNKContext.OcularComplaints.Add(ocular);
                                WYNKContext.SaveChanges();
                            }
                            var regtranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                            var userroleid = InsertOcularComplaints.OcularComplaintsNew.Select(x => x.Doctoruserid).FirstOrDefault();

                            var pgeneral = new PatientGeneralDatamodel();
                            pgeneral.IsDeleted = false;
                            if (InsertOcularComplaints.familyhistory != null)
                            {
                                pgeneral.FamilyHistory = InsertOcularComplaints.familyhistory;
                            }
                            else
                            {
                                pgeneral.FamilyHistory = "NULL";
                            }

                            pgeneral.RegistrationTranID = regtranid;
                            pgeneral.UIN = UIN;
                            pgeneral.VisitDate = DateTime.UtcNow;
                            pgeneral.CreatedBy = Convert.ToInt32(userroleid);
                            pgeneral.CreatedUTC = DateTime.UtcNow;

                            if (InsertOcularComplaints.Allergy != null)
                            {
                                pgeneral.Allergy = InsertOcularComplaints.Allergy;
                            }
                            else
                            {
                                pgeneral.Allergy = "NULL";
                            }

                            pgeneral.CurrentMedication = "NULL";
                            WYNKContext.PatientGeneral.Add(pgeneral);
                            WYNKContext.SaveChanges();


                            foreach (var list in InsertOcularComplaints.systemicconditionsNew)
                            {
                                var ocular = new PatientHistory();
                                if (list.Description != null)
                                {
                                    var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "SystemicCondition"
                              && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                    ocular.Description = Desc;
                                }
                                else
                                {
                                    var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "SystemicCondition").Select(x => x.OLMID).FirstOrDefault();
                                    ocular.Description = Desc;
                                }
                                ocular.IsActive = true;
                                ocular.CreatedUTC = DateTime.UtcNow;
                                ocular.UpdatedUTC = null;
                                ocular.Remarks = list.Remarks;
                                if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                {
                                    ocular.FromUTC = null;
                                }
                                else
                                {
                                    ocular.FromUTC = Convert.ToDateTime(list.FromDate);
                                }
                                ocular.UIN = UIN;
                                ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                ocular.RegistrationTranID = tranid;
                                WYNKContext.PatientHistory.Add(ocular);
                                WYNKContext.SaveChanges();
                            }

                        }
                        else
                        {
                            foreach (var list in InsertOcularComplaints.OcularComplaintsNew)
                            {
                                var ocular = new OcularComplaints();
                                if (list.Description != null)
                                {
                                    var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints" && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                    ocular.Description = Desc;
                                }
                                else
                                {
                                    var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "OcularComplaints").Select(x => x.OLMID).FirstOrDefault();
                                    ocular.Description = Desc;
                                }
                                ocular.ISOD = list.ISOD;
                                ocular.ISOS = list.ISOS;
                                ocular.ISOU = list.ISOU;
                                ocular.CreatedUTC = DateTime.UtcNow;
                                ocular.UpdatedUTC = null;
                                ocular.Remarks = list.Remarks;
                                if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                {
                                    ocular.FromUTC = null;
                                }
                                else

                                {
                                    ocular.FromUTC = list.FromDate;
                                }
                                ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                ocular.RegistrationTranID = tranid;
                                WYNKContext.OcularComplaints.Add(ocular);
                                WYNKContext.SaveChanges();
                            }



                            foreach (var list in InsertOcularComplaints.systemicconditionsNew)
                            {
                                var ocular = new PatientHistory();
                                if (list.Description != null)
                                {
                                    var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "SystemicCondition"
                              && x.ParentDescription == list.Description).Select(x => x.OLMID).FirstOrDefault();
                                    ocular.Description = Desc;
                                }
                                else
                                {
                                    var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == " " && x.ParentTag == "SystemicCondition").Select(x => x.OLMID).FirstOrDefault();
                                    ocular.Description = Desc;
                                }

                                ocular.CreatedUTC = DateTime.UtcNow;
                                ocular.UpdatedUTC = null;
                                ocular.IsActive = true;
                                if (list.FromDate == null || list.FromDate == Convert.ToDateTime("1/1/0001 00:00:00"))
                                {
                                    ocular.FromUTC = null;
                                }
                                else
                                {
                                    ocular.FromUTC = Convert.ToDateTime(list.FromDate);
                                }
                                ocular.UIN = UIN;
                                ocular.Remarks = list.Remarks;
                                ocular.CreatedBy = Convert.ToInt32(list.Doctoruserid);
                                var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                ocular.RegistrationTranID = tranid;
                                WYNKContext.PatientHistory.Add(ocular);
                                WYNKContext.SaveChanges();
                            }
                            var regtranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                            var userroleid = InsertOcularComplaints.OcularComplaintsNew.Select(x => x.Doctoruserid).FirstOrDefault();

                            var pgeneral = new PatientGeneralDatamodel();
                            pgeneral.IsDeleted = false;
                            if (InsertOcularComplaints.familyhistory != null)
                            {
                                pgeneral.FamilyHistory = InsertOcularComplaints.familyhistory;
                            }
                            else
                            {
                                pgeneral.FamilyHistory = "NULL";
                            }
                            pgeneral.RegistrationTranID = regtranid;
                            pgeneral.UIN = UIN;
                            pgeneral.VisitDate = DateTime.UtcNow;
                            pgeneral.CreatedBy = Convert.ToInt32(userroleid);
                            pgeneral.CurrentMedication = "NULL";

                            if (InsertOcularComplaints.Allergy != null)
                            {
                                pgeneral.Allergy = InsertOcularComplaints.Allergy;
                            }
                            else
                            {
                                pgeneral.Allergy = "NULL";
                            }

                            pgeneral.CreatedUTC = DateTime.UtcNow;
                            WYNKContext.PatientGeneral.Add(pgeneral);
                            WYNKContext.SaveChanges();

                        }

                        if (InsertOcularComplaints.CurrentMedications != null)
                        {

                            var PatCurrentMedication = WYNKContext.PatientCurrentMedication.Where(x => x.UIN == UIN);
                            if (PatCurrentMedication != null)
                            {
                                WYNKContext.PatientCurrentMedication.RemoveRange(WYNKContext.PatientCurrentMedication.Where(x => x.UIN == UIN).ToList());
                                WYNKContext.SaveChanges();
                            }


                            foreach (var item in InsertOcularComplaints.CurrentMedications.ToList())
                            {
                                var PatCurMedication = new PatientCurrentMedication();
                                PatCurMedication.GenericDrugDescription = item.GenericDescription;
                                PatCurMedication.RegistrationTranID = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                PatCurMedication.UIN = UIN;
                                PatCurMedication.VisitDate = DateTime.Now;
                                if (item.Eye == "OD")
                                {
                                    PatCurMedication.IsOD = true;
                                }
                                else if (item.Eye == "OS")
                                {
                                    PatCurMedication.IsOS = true;
                                }
                                else if (item.Eye == "OU")
                                {
                                    PatCurMedication.IsOU = true;
                                }
                                if (item.YearMonths == "")
                                {
                                    PatCurMedication.Since = item.YearMonths != "" ? Convert.ToDateTime(item.YearMonths) : (DateTime?)null;
                                }
                                else if (item.YearMonths == " ")
                                {
                                    PatCurMedication.Since = item.YearMonths != " " ? Convert.ToDateTime(item.YearMonths) : (DateTime?)null;
                                }
                                else
                                {
                                    PatCurMedication.Since = Convert.ToDateTime(item.YearMonths);
                                }
                                PatCurMedication.ProgressStatusID = Convert.ToInt32(item.Status);
                                PatCurMedication.Frequency = item.FrequencyID;
                                PatCurMedication.PrescribedByDoctorName = item.PrescribedDoctor;
                                PatCurMedication.Remarks = item.Remarks;
                                PatCurMedication.Cmpid = cmpids;
                                WYNKContext.PatientCurrentMedication.Add(PatCurMedication);
                            }
                            WYNKContext.SaveChanges();
                        }
                    }
                    if (WYNKContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Patient History Saved Successfully");
                        dbContextTransaction.Commit();
                    }
                    return new
                    {
                        Success = true
                    };

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                    Console.Write(ex);
                }
                return new
                {
                    Success = false
                };

            }
        }

        public dynamic DeletenewOcularComplaints(string UIN, string Description, string Reasons, int CMPID)
        {
            var data = new OcularComplaintsViewModel();
            try
            {
                var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == UIN && x.CmpID == CMPID).Select(x => x.RegistrationTranID).ToList();

                foreach (var firstitem in tranid)
                {
                    var olmid = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == Description && x.ParentTag == "OcularComplaints").Select(x => x.OLMID).ToList();

                    foreach (var item in olmid)
                    {
                        var oculrdata = WYNKContext.OcularComplaints.Where(x => x.RegistrationTranID == firstitem && x.Description == item).FirstOrDefault();
                        if (oculrdata != null)
                        {
                            oculrdata.Reasons = Reasons;
                            oculrdata.IsDeleted = true;
                            oculrdata.UpdatedUTC = DateTime.UtcNow;
                            WYNKContext.OcularComplaints.UpdateRange(oculrdata);
                            WYNKContext.SaveChanges();
                        }
                    }
                }


                if (WYNKContext.SaveChanges() >= 0)
                {
                    return new
                    {
                        Success = true
                    };
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false

            };
        }

        public dynamic Deletenewsystemicconditions(string UIN, string Description, string Reasons, int CMPID)
        {
            var data = new OcularComplaintsViewModel();
            try
            {
                

                var tranid = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == UIN && x.CmpID == CMPID).Select(x => x.RegistrationTranID).ToList();

                foreach (var firstitem in tranid)
                {
                    var olmid = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == Description && x.ParentTag == "SystemicCondition").Select(x => x.OLMID).ToList();

                    foreach (var item in olmid)
                    {
                        var oculrdata = WYNKContext.PatientHistory.Where(x => x.RegistrationTranID == firstitem && x.Description == item && x.UIN == UIN).FirstOrDefault();
                        if (oculrdata != null)
                        {
                            oculrdata.Reasons = Reasons;
                            oculrdata.IsActive = false;
                            oculrdata.UpdatedUTC = DateTime.UtcNow;
                            WYNKContext.PatientHistory.UpdateRange(oculrdata);
                            WYNKContext.SaveChanges();
                        }
                    }
                }
                if (WYNKContext.SaveChanges() >= 0)
                {
                    return new
                    {
                        Success = true
                    };
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false

            };
        }



        public dynamic GetDeletednewOcularComplaints(string UIN, int CMPID)
        {
            var data = new OcularComplaintsViewModel();
            var tranid = WYNKContext.RegistrationTran.Where(x => x.CmpID == CMPID && x.UIN == UIN).ToList();
            var Onelinedata = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints").ToList();
            data.GetDeletedOcularComplaintsNew = new List<GetDeletedOcularComplaintsNew>();

            data.GetDeletedOcularComplaintsNew = (from oc in WYNKContext.OcularComplaints.Where(x => x.IsDeleted == true)
                                                  join reg in tranid on oc.RegistrationTranID equals reg.RegistrationTranID
                                                  select new GetDeletedOcularComplaintsNew
                                                  {
                                                      ID = oc.ID,
                                                      Description = Onelinedata.Where(x => x.OLMID == oc.Description).Select(x => x.ParentDescription).FirstOrDefault(),
                                                      ISOD = oc.ISOD,
                                                      ISOS = oc.ISOS,
                                                      ISOU = oc.ISOU,
                                                      DeletedDate = oc.UpdatedUTC,
                                                      Reasons = oc.Reasons,

                                                  }).ToList();

            return data;

        }


        public dynamic GetDeletednewsystemic(string UIN, int CMPID)
        {
            var data = new OcularComplaintsViewModel();
            var tranid = WYNKContext.RegistrationTran.Where(x => x.CmpID == CMPID && x.UIN == UIN).ToList();
            var Onelinedata = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "SystemicCondition").ToList();
            data.GetDeletedOcularComplaintsNew = new List<GetDeletedOcularComplaintsNew>();

            data.GetDeletedOcularComplaintsNew = (from oc in WYNKContext.PatientHistory.Where(x => x.IsActive == false)
                                                  join reg in tranid on oc.RegistrationTranID equals reg.RegistrationTranID
                                                  select new GetDeletedOcularComplaintsNew
                                                  {
                                                      ID = oc.ID,
                                                      Description = Onelinedata.Where(x => x.OLMID == oc.Description).Select(x => x.ParentDescription).FirstOrDefault(),
                                                      DeletedDate = oc.UpdatedUTC,
                                                      Reasons = oc.Reasons,

                                                  }).ToList();

            return data;

        }


        public dynamic InsertOcularMaster(OcularComplaintsViewModel AddOculardata)
        {
            var onelinemaster = new OneLine_Masters();
            onelinemaster.ParentDescription = AddOculardata.OneLineMaster.ParentDescription;
            onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Ocular Complaints").Select(x => x.OLMID).FirstOrDefault();
            onelinemaster.ParentTag = "OcularComplaints";
            onelinemaster.CreatedBy = AddOculardata.OneLineMaster.CreatedBy;
            onelinemaster.CreatedUTC = DateTime.UtcNow;
            onelinemaster.IsActive = true;
            onelinemaster.IsDeleted = false;
            CMPSContext.OneLineMaster.AddRange(onelinemaster);
            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,

                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,

            };
        }


        public dynamic Insertsystemconditions(OcularComplaintsViewModel Addsystemicdata)
        {
            var onelinemaster = new OneLine_Masters();
            onelinemaster.ParentDescription = Addsystemicdata.OneLineMaster.ParentDescription;
            onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Systemic Condition").Select(x => x.OLMID).FirstOrDefault();
            onelinemaster.ParentTag = "SystemicCondition";
            onelinemaster.CreatedBy = Addsystemicdata.OneLineMaster.CreatedBy;
            onelinemaster.CreatedUTC = DateTime.UtcNow;
            onelinemaster.IsActive = true;
            onelinemaster.IsDeleted = false;
            CMPSContext.OneLineMaster.AddRange(onelinemaster);
            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,

                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,

            };
        }


        public dynamic UpdateOcularMaster(OcularComplaintsViewModel updateOculardata, int ID)
        {
            var onelinemaster = new OneLine_Masters();
            onelinemaster = CMPSContext.OneLineMaster.Where(x => x.OLMID == ID).FirstOrDefault();
            onelinemaster.ParentDescription = updateOculardata.OneLineMaster.ParentDescription;
            onelinemaster.IsActive = updateOculardata.OneLineMaster.IsActive;
            onelinemaster.UpdatedBy = updateOculardata.OneLineMaster.UpdatedBy;
            onelinemaster.UpdatedUTC = DateTime.UtcNow;
            CMPSContext.OneLineMaster.UpdateRange(onelinemaster);
            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,

            };

        }


        public dynamic UpdatesystemicMaster(OcularComplaintsViewModel updatesystemicdata, int ID)
        {
            var onelinemaster = new OneLine_Masters();
            onelinemaster = CMPSContext.OneLineMaster.Where(x => x.OLMID == ID).FirstOrDefault();
            onelinemaster.ParentDescription = updatesystemicdata.OneLineMaster.ParentDescription;
            onelinemaster.IsActive = updatesystemicdata.OneLineMaster.IsActive;
            onelinemaster.UpdatedBy = updatesystemicdata.OneLineMaster.UpdatedBy;
            onelinemaster.UpdatedUTC = DateTime.UtcNow;
            CMPSContext.OneLineMaster.UpdateRange(onelinemaster);
            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,

            };
        }

        public dynamic DeleteOcular(int ID)
        {
            var stoMas = CMPSContext.OneLineMaster.Where(x => x.OLMID == ID).ToList();

            if (stoMas != null)
            {
                stoMas.All(x => { x.IsDeleted = true; x.IsActive = false; return true; });
                CMPSContext.OneLineMaster.UpdateRange(stoMas);
            }
            try
            {
                if (CMPSContext.SaveChanges() >= 0)
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
            return new
            {
                Success = false,
                Message = CommonRepository.Missing
            };

        }

        public dynamic Deletesystemic(int ID)
        {
            var stoMas = CMPSContext.OneLineMaster.Where(x => x.OLMID == ID).ToList();

            if (stoMas != null)
            {
                stoMas.All(x => { x.IsDeleted = true; x.IsActive = false; return true; });
                CMPSContext.OneLineMaster.UpdateRange(stoMas);
            }
            try
            {
                if (CMPSContext.SaveChanges() >= 0)
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
            return new
            {
                Success = false,
                Message = CommonRepository.Missing
            };

        }

    }

}
