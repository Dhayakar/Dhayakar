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
    class OperationTheatreBookingRepository : RepositoryBase<OperationTheatreBooking_Viewmodel>, IOperationTheatreBookingRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        
        public object HRS_FS_04 { get; private set; }
       

        public OperationTheatreBookingRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }
        public dynamic GetDetails(string uin)
        {
            var surgeryassignedd = WYNKContext.SurgeryAssigned.ToList();
            var surgeryAssignedtran = WYNKContext.SurgeryAssignedTran.ToList();
            var reg = WYNKContext.Registration.ToList();
            var findings = WYNKContext.Findings.ToList();
            var operationtheatre = WYNKContext.OperationTheatre.ToList();
            var operationtheatreextension = WYNKContext.OperationTheatreExtension.ToList();
            var icdcode = WYNKContext.ICDSpecialityCode.ToList();
            var FindingsExt = WYNKContext.FindingsExt.ToList();
            var OTB = new OperationTheatreBooking_Viewmodel();


            OTB.regDetails = (from REG in reg.Where(x => x.UIN == uin)
                              select new registrationDetails
                              {

                                  name = REG.Name,
                                  gender = REG.Gender,
                                  age = PasswordEncodeandDecode.ToAgeString(REG.DateofBirth),
                              }).ToList();

            OTB.surgerydetails = (from surgeryassigned in surgeryassignedd.Where(x => x.UIN == uin).OrderByDescending(x => x.CreatedUTC)

                                  select new surgery
                                  {
                                      fromtime = surgeryassigned.FromTime,
                                      totime = surgeryassigned.ToTime,
                                      surgerydate = surgeryassigned.SurgeryDate
                                  }).ToList();

           
            OTB.surgerynamedetails = (from surgeryassigned in surgeryassignedd.Where(x => x.UIN == uin)
                                      join tran in surgeryAssignedtran
                                      on surgeryassigned.RandomUniqueID equals tran.SAID
                                      select new surgeryname
                                      {


                                      }).ToList();




            return OTB;





        }
        public dynamic GetDetailsuin(int companyid)
        {

            var reg = WYNKContext.Registration.ToList();
            var surgeryAssigned = WYNKContext.SurgeryAssigned.ToList();
            var OTB = new OperationTheatreBooking_Viewmodel();


            OTB.OperationtheatrebookingDetails = (from SurgeryAssignedd in surgeryAssigned
                                                  join REG in reg
                                                  on SurgeryAssignedd.UIN equals REG.UIN
                                                  select new Operationtheatrebooking
                                                  {
                                                      uin = SurgeryAssignedd.UIN,
                                                      name = REG.Name,
                                                      gender = REG.Gender,
                                                      age = PasswordEncodeandDecode.ToAgeString(REG.DateofBirth),
                                                  }).ToList();


            OTB.OperationtheatrebookingDetails1 = (from res in OTB.OperationtheatrebookingDetails.GroupBy(x => x.uin)
                                                   select new Operationtheatrebooking1
                                                   {
                                                       uin1 = res.Key,
                                                       name1 = res.Select(x => x.name).FirstOrDefault(),
                                                       age1 = res.Select(x => x.age).FirstOrDefault(),
                                                       gender1 = res.Select(x => x.gender).FirstOrDefault()
                                                   }).ToList();

            return OTB;





        }
        public dynamic BookingDetails(OperationTheatreBooking_Viewmodel opb, string uin, int companyid, int operationid, int userid)

        {

            var operationbooking = new OperationTheatreBooking();
            var operationbookingstatus = new OperationTheatre_Booking_Status();
            operationbooking.CMPID = companyid;
            operationbooking.UIN = uin;
            operationbooking.OPID = operationid;
            operationbooking.OTRequiredDate = opb.OperationBookingg.OTRequiredDate.AddDays(1);
            operationbooking.FromTime = opb.OperationBookingg.FromTime;
            operationbooking.ToTime = opb.OperationBookingg.ToTime;
            operationbooking.SurgeryType = opb.OperationBookingg.SurgeryType;
            operationbooking.OTBookedBy = opb.OperationBookingg.OTBookedBy;
            operationbooking.OTBookedOn = opb.OperationBookingg.OTBookedOn;
            operationbooking.CreatedUTC = DateTime.UtcNow;
            operationbooking.CreatedBy = userid;
            WYNKContext.OperationTheatreBooking.AddRange(operationbooking);
            WYNKContext.SaveChanges();

            string sub = opb.OperationBookingg.FromTime.Substring(0, 2);
            string subto = opb.OperationBookingg.ToTime.Substring(0, 2);
            string a = "HRS_FS_";
            string b = "HRS_SS_";

            string c = a + sub;

            string cto = b + subto;

            var names = typeof(OperationTheatre_Booking_Status).GetProperties()
                       .Select(property => property.Name).ToList();

            var Narray = typeof(OperationTheatre_Booking_Status).GetProperties()
                       .Select(property => property.Name).ToArray();
            var zarray = Array.FindIndex(Narray, x => x == c);

            var ztoarray = Array.FindIndex(Narray, x => x == cto);
            var liBooking = new List<BookingHRSDet>();


            foreach (var sedonlist in names)
            {
                var index = names.IndexOf(sedonlist);

                if (index >= zarray && index <= ztoarray)
                {
                    var seclist = new BookingHRSDet();
                    var ssvalue = Narray.GetValue(index);
                    seclist.HRValues = Convert.ToString(ssvalue);
                    liBooking.Add(seclist);
                }
            }

            var lasrecords = liBooking.TakeLast(2).ToList();

            var lastfirstrecord = lasrecords.Select(x => x.HRValues).FirstOrDefault();
            var lastLastrecord = lasrecords.Select(x => x.HRValues).LastOrDefault();


            foreach (var item in liBooking.Select(x => x.HRValues).ToList())
            {

                var opee = new OperationTheatre_Booking_Status();



                opee.HRS_FS_00 = Todata("HRS_FS_00", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_00 = Todata("HRS_SS_00", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_01 = Todata("HRS_FS_01", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_01 = Todata("HRS_SS_01", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_02 = Todata("HRS_FS_02", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_02 = Todata("HRS_SS_02", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_03 = Todata("HRS_SS_03", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_03 = Todata("HRS_SS_03", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_04 = Todata("HRS_FS_04", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_04 = Todata("HRS_SS_04", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_05 = Todata("HRS_FS_05", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_05 = Todata("HRS_SS_05", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_06 = Todata("HRS_SS_06", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_06 = Todata("HRS_FS_00", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_07 = Todata("HRS_FS_07", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_07 = Todata("HRS_SS_07", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_08 = Todata("HRS_FS_08", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_08 = Todata("HRS_SS_08", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_09 = Todata("HRS_FS_09", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_09 = Todata("HRS_SS_09", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_10 = Todata("HRS_FS_10", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_10 = Todata("HRS_SS_10", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_11 = Todata("HRS_FS_11", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_11 = Todata("HRS_SS_11", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_12 = Todata("HRS_FS_12", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_12 = Todata("HRS_SS_12", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_13 = Todata("HRS_FS_13", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_13 = Todata("HRS_SS_13", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_14 = Todata("HRS_FS_14", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_14 = Todata("HRS_SS_14", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_15 = Todata("HRS_FS_15", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_15 = Todata("HRS_SS_15", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_16 = Todata("HRS_FS_16", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_16 = Todata("HRS_SS_16", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_17 = Todata("HRS_FS_17", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_17 = Todata("HRS_SS_17", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_18 = Todata("HRS_FS_18", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_18 = Todata("HRS_SS_18", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_19 = Todata("HRS_FS_19", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_19 = Todata("HRS_SS_19", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_20 = Todata("HRS_FS_20", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_20 = Todata("HRS_SS_20", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_21 = Todata("HRS_FS_21", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_21 = Todata("HRS_SS_21", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_22 = Todata("HRS_FS_22", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_22 = Todata("HRS_SS_22", item, opb.OperationBookingg.ToTime);

                opee.HRS_FS_23 = Todata("HRS_FS_23", item, opb.OperationBookingg.FromTime);
                opee.HRS_SS_23 = Todata("HRS_SS_23", item, opb.OperationBookingg.ToTime);

                WYNKContext.OperationTheatreBookingStatus.Add(opee);



            }



            try
            {
                if (WYNKContext.SaveChanges() >= 0)
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
                Message = "Some data are Missing"



            };


        }



        public dynamic Todata(string DBDATA, string itemdata, string originaltime)
        {
            var Returnoriginaltimes = "0";

            var timstamp = Convert.ToDateTime(originaltime);
            var realtimess = timstamp.ToString("HH");
            var realtime = Convert.ToInt32(timstamp.ToString("mm"));

            string sub = realtimess;
            string a = "HRS_FS_";
            string b = "HRS_SS_";
            string c = a + sub;
            string d = b + sub;



            if (realtime <= Convert.ToInt32("30") && DBDATA == c)
            {

                Returnoriginaltimes = originaltime;

            }

            else if (DBDATA == d)
            {
                Returnoriginaltimes = originaltime;
            }
            else
            {
                Returnoriginaltimes = "0";
            }


            return Returnoriginaltimes;
        }





        public dynamic GetOperationname(int spid)
        {
            var optExt = WYNKContext.OperationTheatreExtension.ToList().Where(x => x.SpecialityDescription == spid.ToString());
            var operationtheatre = WYNKContext.OperationTheatre.ToList();
            var OTB1 = new OperationTheatreBooking_Viewmodel();

            OTB1.operationnamee = (from operationtheatreExtension in optExt
                                   join OPT in operationtheatre
                                   on operationtheatreExtension.OTID equals OPT.OTID
                                   select new operationname
                                   {
                                       operationtheatrename = OPT.OTDescription,
                                       ID = OPT.OTID
                                   }).ToList();
            OTB1.operationnamee1 = (from res in OTB1.operationnamee.GroupBy(x => x.ID)

                                    select new operationname1
                                    {
                                        ID1 = res.Key,
                                        operationtheatrename1 = res.Select(x => x.operationtheatrename).FirstOrDefault()
                                    }).ToList();
            return OTB1;
        }
        public dynamic GetDetails()
        {
            var opb = WYNKContext.OperationTheatreBooking.ToList();
            var opb1 = new OperationTheatreBooking_Viewmodel();

            opb1.operationBookingDetail = (from OPB in opb

                                           select new operationBookingDet
                                           {
                                               uin = OPB.UIN,
                                               OTReqDate = OPB.OTRequiredDate,
                                               FromTime = OPB.FromTime,
                                               ToTime = OPB.ToTime,
                                               SurgeryType = OPB.SurgeryType,
                                           }).ToList();
            return opb1;

        }
    }
}
