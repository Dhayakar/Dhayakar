using Microsoft.EntityFrameworkCore;
using CommonMailService.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;
using System.Text.RegularExpressions;


namespace WYNK.Data.Repository.Implementation
{
    class OpticalPrescriptionRepository : RepositoryBase<OpticalPrescriptionview>, IOpticalPrescriptionRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;

        public OpticalPrescriptionRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic GetfinalopDetails(string Search, int id)
        {

            var registration = WYNKContext.Registration.ToList();
            var user = CMPSContext.Users.ToList();
            var docmas = CMPSContext.DoctorMaster.ToList();
            var userrole = CMPSContext.UserVsRole.ToList();
            var final = new OpticalPrescriptionview();
            final.opfinalprescri = new List<opfinalprescri>();



            var opticalpreadm =  (from REF in WYNKContext.OpticalPrescriptionmaster
                                  where REF.UIN.ToLower().Contains(Search) || REF.Phone.ToLower().Contains(Search)
                                  || REF.Name.ToLower().Contains(Search) || REF.LastName.ToLower().Contains(Search) && REF.CMPID == id

                                      join us in user.Where(x => x.CMPID == id) on

                                      REF.CreatedBy equals us.Userid

                                      join usr in userrole.Where(x => x.CMPID == id) on
                                      us.Username equals usr.UserName
                                      select new 
                                     {
                                        RegistrationTranID = REF.RegistrationID != null ? REF.RegistrationID : REF.CustomerMasterID,
                                        PrescriptionDate = REF.PrescriptionDate,
                                        Doctorname = us.Username,
                                        Role = usr.RoleDescription,
                                        UIN = REF.UIN != null ? REF.UIN : string.Empty,
                                        Name = REF.MiddleName == null ? REF.Name + " " + REF.LastName : REF.Name + " " + REF.MiddleName + " " + REF.LastName,
                                        Gender = REF.Gender != null ? REF.Gender : string.Empty,
                                        DateofBirth = REF.DateofBirth != null ? PasswordEncodeandDecode.ToAgeString(Convert.ToDateTime(REF.DateofBirth)) : string.Empty,
                                      }).ToList();

            var opfinalprescri = (from REF in WYNKContext.OpticalPrescriptionmaster
                                  where REF.UIN.ToLower().Contains(Search)|| REF.Phone.ToLower().Contains(Search)
                                  || REF.Name.ToLower().Contains(Search) || REF.LastName.ToLower().Contains(Search) && REF.CMPID == id
                                  join us in user.Where(x => x.CMPID == id) on
                                    REF.CreatedBy equals us.Userid
                                    join doc in docmas.Where(x => x.CMPID == id) on
                                    us.Username equals doc.EmailID
                                    join usr in userrole.Where(x => x.CMPID == id) on
                                    us.Username equals usr.UserName
                                    select new 
                                    {
                                        RegistrationTranID = REF.RegistrationID != null ? REF.RegistrationID : REF.CustomerMasterID,
                                        PrescriptionDate = REF.PrescriptionDate,
                                        Doctorname = doc.Firstname + " " + doc.MiddleName + " " + doc.LastName,
                                        Role = usr.RoleDescription,
                                        UIN = REF.UIN != null ? REF.UIN : string.Empty,
                                        Name = REF.MiddleName == null ? REF.Name + " " + REF.LastName : REF.Name + " " + REF.MiddleName + " " + REF.LastName,
                                        Gender = REF.Gender != null ? REF.Gender : string.Empty,
                                        DateofBirth = REF.DateofBirth!= null ? PasswordEncodeandDecode.ToAgeString(Convert.ToDateTime(REF.DateofBirth)) : string.Empty,
                                    }).ToList();

            var grpres = opfinalprescri.Count() > 0 ? opfinalprescri.ToList() : opticalpreadm.ToList();

            if (grpres.Count != 0)
            {

                final.opfinalprescri = (from REF in grpres

                                        select new opfinalprescri
                                        {

                                            RegistrationTranID = REF.RegistrationTranID,
                                            PrescriptionDate = REF.PrescriptionDate,
                                            Doctorname = REF.Doctorname,
                                            Role = REF.Role,
                                            UIN = REF.UIN,
                                            Name = REF.Name,
                                            Gender = REF.Gender,
                                            DateofBirth = REF.DateofBirth,
                                        }).ToList();

            }


            return final;

        }

        public dynamic GetopticalDetails(int RegID)
        {
            var OPDetails = new OpticalPrescriptionview();
            var OneLineMaster = CMPSContext.OneLineMaster.ToList();
            OPDetails.opticprescription = new List<opticprescription>();

            OPDetails.opticprescription = (from op in WYNKContext.OpticalPrescription.Where(x => x.RegistrationTranID == RegID || x.CustomerMasterID == RegID)

                                           select new opticprescription
                                           {
                                               Type = op.Type,
                                               Ocular = op.Ocular,
                                               DistSph = op.DistSph,
                                               NearCyl = op.NearCyl,
                                               PinAxis = op.PinAxis,
                                               Add = op.Add != "" ? OneLineMaster.Where(x => x.OLMID == Convert.ToInt32(op.Add)).Select(d => d.ParentDescription).FirstOrDefault() : string.Empty,
                                               Remarks = op.Remarks,
                                               PD = op.PD,
                                               MPDOD = op.MPDOD,
                                               MPDOS = op.MPDOS,
                                           }).ToList();



            return OPDetails;
        }
        public dynamic Getfinopprint(int rid, int CMPID, string Time)

        {

            TimeSpan ts = TimeSpan.Parse(Time);

            var registration = WYNKContext.Registration.ToList();
            var opticalprescriptionDetailsprint = new OpticalPrescriptionview();
            var doc = CMPSContext.Users.ToList();
            var re = WYNKContext.OpticalPrescription.ToList();
            var Employee = CMPSContext.Employee.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();
            opticalprescriptionDetailsprint.opticprescriptionprint = new List<opticprescriptionprint>();

            var TID = WYNKContext.OpticalPrescription.Where(u => u.RegistrationTranID == rid).Select(x => x.UIN).LastOrDefault();
            var CID = WYNKContext.OpticalPrescription.Where(u => u.CustomerMasterID == rid).Select(x => x.CustomerMasterID).LastOrDefault();

            opticalprescriptionDetailsprint.UIN = WYNKContext.OpticalPrescription.Where(x => x.UIN == TID).Select(x => x.UIN).FirstOrDefault();
            opticalprescriptionDetailsprint.Date = WYNKContext.OpticalPrescription.Where(x => x.UIN == TID).Select(x => x.CreatedUTC.Add(ts)).FirstOrDefault() != null ? WYNKContext.OpticalPrescription.Where(x => x.UIN == TID).Select(x => x.CreatedUTC.Add(ts)).FirstOrDefault() : WYNKContext.OpticalPrescriptionmaster.Where(x => x.CustomerMasterID == CID).Select(x => x.CreatedUTC.Add(ts)).FirstOrDefault();
            opticalprescriptionDetailsprint.Name = registration.Where(x => x.UIN == TID).Select(x => x.Name + " " + x.MiddleName + " " + x.LastName).FirstOrDefault() != null ? registration.Where(x => x.UIN == TID).Select(x => x.Name + " " + x.MiddleName + " " + x.LastName).FirstOrDefault() : WYNKContext.OpticalPrescriptionmaster.Where(x => x.CustomerMasterID == CID).Select(x => x.Name + " " + x.MiddleName + " " + x.LastName).FirstOrDefault();
            opticalprescriptionDetailsprint.Age = PasswordEncodeandDecode.ToAgeString(WYNKContext.Registration.Where(x => x.UIN == TID).Select(x => x.DateofBirth).FirstOrDefault());
            opticalprescriptionDetailsprint.Gender = WYNKContext.Registration.Where(x => x.UIN == TID).Select(x => x.Gender).FirstOrDefault();
            opticalprescriptionDetailsprint.Description = WYNKContext.PatientGeneral.Where(x => x.UIN == TID).Select(x => x.Allergy).FirstOrDefault();
            opticalprescriptionDetailsprint.Address = CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.Address1 != null ? x.Address1 : string.Empty).FirstOrDefault();
            opticalprescriptionDetailsprint.Address1 = CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.Address2 != null ? x.Address2 : string.Empty).FirstOrDefault();
            opticalprescriptionDetailsprint.Address2 = CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.Address3 != null ? x.Address3 : string.Empty).FirstOrDefault();
            opticalprescriptionDetailsprint.phone = CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.Phone1).FirstOrDefault();
            opticalprescriptionDetailsprint.web = CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.Website).FirstOrDefault();
            opticalprescriptionDetailsprint.Compnayname = CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.CompanyName).FirstOrDefault();

            var d = WYNKContext.PatientAssign.Where(x => x.RegistrationTranID == rid).Select(x => x.DoctorID).FirstOrDefault();

            var v = doc.Where(x => x.Userid == re.Where(c => c.RegistrationTranID == rid).Select(g => g.CreatedBy).FirstOrDefault()).Select(x => x.Username).FirstOrDefault();
            var f = CMPSContext.DoctorMaster.Where(x => x.EmailID == v).Select(x => x.DoctorID).FirstOrDefault();

            if (d != 0)
            {
                opticalprescriptionDetailsprint.docname = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault();
                opticalprescriptionDetailsprint.docreg = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.RegistrationNumber).FirstOrDefault();

            }
            else
            {
                if (f != 0)
                {
                    opticalprescriptionDetailsprint.docname = CMPSContext.DoctorMaster.Where(x => x.DoctorID == f).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault();
                    opticalprescriptionDetailsprint.docreg = CMPSContext.DoctorMaster.Where(x => x.DoctorID == f).Select(x => x.RegistrationNumber).FirstOrDefault();
                }
                else {
                    opticalprescriptionDetailsprint.docname = Employee.Where(r => r.CreatedBy == re.Where(x => x.CustomerMasterID == CID).Select(x => x.CreatedBy).FirstOrDefault()).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();
                }

            }




            opticalprescriptionDetailsprint.opticprescriptionprint = (from op in WYNKContext.OpticalPrescription.Where(x => x.RegistrationTranID == rid || x.CustomerMasterID == rid)

                                                                      select new opticprescriptionprint
                                                                      {
                                                                          Type = op.Type,
                                                                          Ocular = op.Ocular,
                                                                          DistSph = op.DistSph,
                                                                          NearCyl = op.NearCyl,
                                                                          PinAxis = op.PinAxis,
                                                                          Add = onelinemaster.Where(x => x.OLMID == Convert.ToInt64(op.Add)).Select(x => x.ParentDescription).FirstOrDefault(),
                                                                          Remarks = op.Remarks,
                                                                          PD = op.PD,
                                                                          MPDOD = op.MPDOD,
                                                                          MPDOS = op.MPDOS

                                                                      }).ToList();





            try
            {

                return new
                {
                    uin = opticalprescriptionDetailsprint.UIN,
                    pdate = opticalprescriptionDetailsprint.Date,
                    name = opticalprescriptionDetailsprint.Name,
                    age = opticalprescriptionDetailsprint.Age,
                    gender = opticalprescriptionDetailsprint.Gender,
                    address = opticalprescriptionDetailsprint.Address,
                    address1 = opticalprescriptionDetailsprint.Address1,
                    address2 = opticalprescriptionDetailsprint.Address2,
                    ph = opticalprescriptionDetailsprint.phone,
                    webb = opticalprescriptionDetailsprint.web,
                    company = opticalprescriptionDetailsprint.Compnayname,
                    patientdesc = opticalprescriptionDetailsprint.Description,
                    Doctorname = opticalprescriptionDetailsprint.docname,
                    DoctorReg = opticalprescriptionDetailsprint.docreg,
                    opticprescriptiondetails = opticalprescriptionDetailsprint.opticprescriptionprint.ToList(),

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
        public dynamic GetUINDetails(int cid)
        {

            var medicalprescription = new OpticalPrescriptionview();

            var re = WYNKContext.OpticalPrescription.ToList();
            var reg = WYNKContext.Registration.ToList();


            var meddt = (from MP in re.Where(x => x.CMPID == cid).GroupBy(x => x.UIN)


                         select new
                         {
                             uin = MP.Select(x => x.UIN).FirstOrDefault(),

                         }).ToList();


            var hisoptical = (from M in meddt
                              join R in reg on M.uin equals R.UIN


                              select new
                              {
                                  Uin = R.UIN,
                                  name = R.Name + " " + R.MiddleName + " " + R.LastName,
                                  gender = R.Gender,
                                  age = PasswordEncodeandDecode.ToAgeString(R.DateofBirth),
                                  addr1 = R.Address1,
                                  addr2 = R.Address2,
                                  addr3 = R.Address3,
                                  phone = R.Phone,

                              }).ToList();


            return hisoptical;
        }
        public dynamic GetHistoryDetails(string UIN, int rid)
        {

            var medicalprescription = new OpticalPrescriptionview();

            var re = WYNKContext.OpticalPrescription.ToList();
            var user = CMPSContext.Users.ToList();
            var docmas = CMPSContext.DoctorMaster.ToList();
            var userrole = CMPSContext.UserVsRole.ToList();

            var MedBills = (from MP in re.Where(u => u.UIN == UIN && u.CMPID == rid).OrderByDescending(x => x.CreatedUTC)
                            join us in user on
                            MP.CreatedBy equals us.Userid
                            join doc in docmas on
                            us.Username equals doc.EmailID
                            join usr in userrole on
                            us.Username equals usr.UserName
                            group MP by new

                            {

                                RID = MP.RegistrationTranID,
                                Pdate = MP.CreatedUTC.Date.ToString("dd-MMM-yyyy"),
                                DName = doc.Firstname + " " + doc.MiddleName + " " + doc.LastName,
                                Role = usr.RoleDescription,

                            } into grp

                            select new
                            {
                                ID = grp.Key.RID,
                                Date = grp.Key.Pdate,
                                Name = grp.Key.DName,
                                role = grp.Key.Role,

                            }).ToList();



            return MedBills;
        }














    }

}









