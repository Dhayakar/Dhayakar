
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NodaTime.TimeZones;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TimeZoneConverter;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    public class SetupRepository : RepositoryBase<SetupMasterViewModel>, ISetupMasterrepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public SetupRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }



        public dynamic InsertSetupdata(SetupMasterViewModel con)
        {
            var data = new SetupMasterViewModel();
            var Countrysetup = new SetupmasterClass();

            var cmpid = CMPSContext.Setup.Where(x => x.CMPID == Convert.ToInt32(con.cmpid)).FirstOrDefault();
            try
            {
                if (cmpid == null)
                {
                    Countrysetup.CMPID = Convert.ToInt32(con.cmpid);
                    Countrysetup.RoomCutOffTime = con.roomtime;
                    Countrysetup.Country = con.country;
                    Countrysetup.Symbol = con.currency.Trim();
                    Countrysetup.CreatedUTC = DateTime.UtcNow;
                    Countrysetup.CreatedBy = 32252;
                    Countrysetup.Pediatric = con.age;
                    Countrysetup.UTCTime = con.roomtime;
                    Countrysetup.Language = con.Language;
                    Countrysetup.Registrationfeeapplicable = Convert.ToBoolean(con.rfa);
                    Countrysetup.Insuranceapplicable = Convert.ToBoolean(con.cfa);
                    Countrysetup.IsNotification = Convert.ToBoolean(con.nfa);
                    Countrysetup.RegistrationDateManage = Convert.ToBoolean(con.rdm);
                    Countrysetup.GapInterval = con.GAPINTERVAL;
                    Countrysetup.FSFromTime = Convert.ToDateTime(con.FROM).TimeOfDay;
                    Countrysetup.FSToTime = Convert.ToDateTime(con.TO).TimeOfDay;
                    Countrysetup.SSFromTime = Convert.ToDateTime(con.SECFROM).TimeOfDay;
                    Countrysetup.SSToTime = Convert.ToDateTime(con.SECTO).TimeOfDay;
                    Countrysetup.BillingType = con.billtype;
                    Countrysetup.Procurement = con.procurement;
                    Countrysetup.Timezone = con.timezone;
                    Countrysetup.BufferTime = con.BufferTime;
                    Countrysetup.CheckoutTime = con.CheckoutTime;
                    CMPSContext.Setup.Add(Countrysetup);
                    CMPSContext.SaveChanges();
                }

                if (CMPSContext.SaveChanges() >= 0)

                    return new
                    {
                        Success = true,
                        Message = CommonMessage.saved,

                    };
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            return new
            {
                Success = false,
                Message = CommonMessage.Missing
            };

        }

        public bool TryGetCurrencySymbol(string ISOCurrencySymbol, out string symbol)
        {
            symbol = CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(c => !c.IsNeutralCulture)
                .Select(culture =>
                {
                    try
                    {
                        return new RegionInfo(culture.Name);
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(ri => ri != null && ri.ISOCurrencySymbol == ISOCurrencySymbol)
                .Select(ri => ri.DisplayName)
                .FirstOrDefault();
            return symbol != null;
        }

        public dynamic Getcountrycurrency(string countryvalue)
        {
            var data = new SetupMasterViewModel();
            data.Timezonedetailsdetauils = new List<Timezonedetailsdetauils>();
            var cntry = CMPSContext.Country.AsNoTracking().ToList();
            string currSymbol;
            var cname = TryGetCurrencySymbol(cntry.Where(x => x.ID == Convert.ToInt32(countryvalue)).Select(x => x.CountryCode).FirstOrDefault(), out currSymbol);
            if (cname != false)
            {
                try
                {
                    var source = TzdbDateTimeZoneSource.Default;
                    IEnumerable<string> windowsZoneIds = source.ZoneLocations
            .Where(x => x.CountryName == currSymbol)
            .Select(tz => source.WindowsMapping.MapZones
                .FirstOrDefault(x => x.TzdbIds.Contains(
                                     source.CanonicalIdMap.First(y => y.Value == tz.ZoneId).Key)))
            .Where(x => x != null)
            .Select(x => x.WindowsId)
            .Distinct();
                    data.Timezonedetailsdetauils = (from c in windowsZoneIds
                                                    select new Timezonedetailsdetauils()
                                                    {
                                                        Timezone = c,
                                                    }).ToList();
                    if (data.Timezonedetailsdetauils.Count() == 0)
                    {
                        IEnumerable<string> zoneIds = TzdbDateTimeZoneSource.Default.ZoneLocations
        .Where(x => x.CountryName == currSymbol)
        .Select(x => x.ZoneId);
                        data.Timezonedetailsdetauils = (from c in zoneIds
                                                        select new Timezonedetailsdetauils()
                                                        {
                                                            Timezone = c,
                                                        }).ToList();
                    }
                }
                catch (Exception eex)
                {
                    Console.WriteLine(eex.ToString());
                    IEnumerable<string> zoneIds = TzdbDateTimeZoneSource.Default.ZoneLocations
    .Where(x => x.CountryName == currSymbol)
    .Select(x => x.ZoneId);
                    data.Timezonedetailsdetauils = (from c in zoneIds
                                                    select new Timezonedetailsdetauils()
                                                    {
                                                        Timezone = c,
                                                    }).ToList();

                }

            }


            data.Currencycode = cntry.Where(x => x.ID == Convert.ToInt32(countryvalue)).Select(x => x.Currency).FirstOrDefault().Trim();
            var countryname = cntry.Where(x => x.ID == Convert.ToInt32(countryvalue)).Select(x => x.CountryCode).FirstOrDefault();
            data.Countrycode = countryname.Substring(0, 2).ToLower();
            return data;
        }

        public dynamic GetcountryTimesoffset(string countryvalue)
        {
            var data = new SetupMasterViewModel();
            TimeZoneInfo tZone = TimeZoneInfo.FindSystemTimeZoneById(countryvalue);
            var hh = tZone.BaseUtcOffset;
            var finff = hasSpecialChar(Convert.ToString(hh.Hours));
            if (finff == true)
            {
                var hhr = hh.Hours;
                var rem = Math.Abs(hhr);
                var vvh = Convert.ToString(rem).PadLeft(2, '0');
                data.hh = '-' + vvh;
                var dd = DateTime.Now.Date + hh;
                data.mm = dd.ToString("mm");
            }
            else
            {
                var dd = DateTime.Now.Date + hh;
                data.hh = dd.ToString("HH");
                data.mm = dd.ToString("mm");
            }


            return data;
        }

        public dynamic GetcountryTimesoffsetwitjstring(string countryvalue)
        {
            var data = new SetupMasterViewModel();
            var orgvalue = countryvalue.Replace('-', '/');
            TimeZoneInfo tzi = TZConvert.GetTimeZoneInfo(orgvalue);
            TimeZoneInfo tZone = TimeZoneInfo.FindSystemTimeZoneById(tzi.StandardName);
            var hh = tZone.BaseUtcOffset;
            var finff = hasSpecialChar(Convert.ToString(hh.Hours));
            if (finff == true)
            {
                var hhr = hh.Hours;
                var rem = Math.Abs(hhr);
                var vvh = Convert.ToString(rem).PadLeft(2, '0');
                data.hh = '-' + vvh;
                var dd = DateTime.Now.Date + hh;
                data.mm = dd.ToString("mm");
            }
            else
            {
                var dd = DateTime.Now.Date + hh;
                data.hh = dd.ToString("HH");
                data.mm = dd.ToString("mm");
            }


            return data;
        }


        public static bool hasSpecialChar(string input)
        {
            string specialChar = @"-";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }


        public dynamic uploadImag(IFormFile file, string CompanyID)
        {

            var sss = Char.IsLetter(CompanyID, 2);
            if (sss == true)
            {
                var ccmpid = CMPSContext.Company.Where(x => x.CompanyName == CompanyID).Select(x => x.CmpID).FirstOrDefault();

                try
                {
                    var currentDir = Directory.GetCurrentDirectory();
                    if (!Directory.Exists(currentDir + "/CompanyLogo/"))
                        Directory.CreateDirectory(currentDir + "/CompanyLogo/");
                    var fileName = $"{ccmpid}{Path.GetExtension(file.FileName)}";
                    var path = $"{currentDir}/CompanyLogo/{fileName}";

                    if ((File.Exists(path)))
                        File.Delete(path);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        var id = CMPSContext.Setup.Where(x => x.CMPID == ccmpid).Select(x => x.SetupID).FirstOrDefault();
                        var pat = CMPSContext.Setup.Where(x => x.SetupID == id).FirstOrDefault();
                        pat.LogoPath = path;
                        CMPSContext.Entry(pat).State = EntityState.Modified;
                        return CMPSContext.SaveChanges() > 0;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    var currentDir = Directory.GetCurrentDirectory();
                    if (!Directory.Exists(currentDir + "/CompanyLogo/"))
                        Directory.CreateDirectory(currentDir + "/CompanyLogo/");
                    var fileName = $"{CompanyID}{Path.GetExtension(file.FileName)}";
                    var path = $"{currentDir}/CompanyLogo/{fileName}";

                    if ((File.Exists(path)))
                        File.Delete(path);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        var id = CMPSContext.Setup.Where(x => x.CMPID == Convert.ToInt32(CompanyID)).Select(x => x.SetupID).FirstOrDefault();
                        var pat = CMPSContext.Setup.Where(x => x.SetupID == id).FirstOrDefault();
                        pat.LogoPath = path;
                        CMPSContext.Entry(pat).State = EntityState.Modified;
                        return CMPSContext.SaveChanges() > 0;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }



        }



        public dynamic getsetupdata(string Cmpid)
        {
            var data = new SetupMasterViewModel();
            var cmpdetails = CMPSContext.Company.AsNoTracking().ToList();
            var cmpid = cmpdetails.Where(x => x.CmpID == Convert.ToInt32(Cmpid)).Select(x => x.CmpID).ToList();
            var parenmtdet = CMPSContext.Company.Where(x => x.ParentID == Convert.ToInt32(Cmpid)).Select(x => x.CmpID).ToList();
            var jointdetails = parenmtdet.Concat(cmpid);
            data.SetupMasterFulldetailsS = new List<SetupMasterFulldetails>();
            var SetupMasterFulldetailsSss = (from cc in CMPSContext.Setup.Where(x => jointdetails.Contains(x.CMPID))
                                             select new
                                             {
                                                 cmp = cc.CMPID,
                                                 country = cc.Country,
                                                 age = cc.Pediatric,
                                                 roomtime = cc.UTCTime,
                                                 symbol = cc.Symbol,
                                                 fsfromtime = cc.FSFromTime,
                                                 fstotime = cc.FSToTime,
                                                 ssfromtime = cc.SSFromTime,
                                                 sstotime = cc.SSToTime,
                                                 time = cc.GapInterval,
                                                 cfa = cc.Insuranceapplicable,
                                                 nfa = cc.IsNotification,
                                                 rfa = cc.Registrationfeeapplicable,
                                                 rdm = cc.RegistrationDateManage,
                                                 btype = cc.BillingType,
                                                 procurement = cc.Procurement,
                                                 timezone = cc.Timezone,
                                                 CheckoutTime = cc.CheckoutTime,
                                                 BufferTime = cc.BufferTime,
                                             }).ToList();
            foreach (var item in SetupMasterFulldetailsSss)
            {
                var datas = new SetupMasterFulldetails();
                datas.age = item.age;
                datas.cmp = CMPSContext.Company.Where(x => x.CmpID == item.cmp).Select(x => x.CompanyName).FirstOrDefault();
                datas.country = CMPSContext.Country.Where(x => x.ID == Convert.ToInt32(item.country)).Select(x => x.CountryName).FirstOrDefault();
                datas.cfa = Convert.ToString(item.cfa).ToLower();
                datas.nfa = Convert.ToString(item.nfa).ToLower();
                datas.rfa = Convert.ToString(item.rfa).ToLower();
                datas.rdm = Convert.ToString(item.rdm).ToLower();
                datas.Roomtime = item.roomtime;
                datas.symbol = item.symbol;
                datas.billtype = item.btype;
                datas.procurement = item.procurement;
                datas.fsfrom =new DateTime(item.fsfromtime.Ticks).ToString("HH");
                datas.symbol = item.symbol;
                datas.fsfrom = new DateTime(item.fsfromtime.Ticks).ToString("HH");
                datas.fsto = new DateTime(item.fstotime.Ticks).ToString("HH");
                datas.ssfrom = new DateTime(item.ssfromtime.Ticks).ToString("HH");
                datas.ssto = new DateTime(item.sstotime.Ticks).ToString("HH");
                datas.fsfromm = new DateTime(item.fsfromtime.Ticks).ToString("mm");
                datas.fstom = new DateTime(item.fstotime.Ticks).ToString("mm");
                datas.ssfromm = new DateTime(item.ssfromtime.Ticks).ToString("mm");
                datas.sstom = new DateTime(item.sstotime.Ticks).ToString("mm");
                datas.gap = item.time;
                datas.timezone = item.timezone;
                datas.CheckoutTime = item.CheckoutTime;
                datas.BufferTime = item.BufferTime;
                var regs = CMPSContext.Setup.Where(x => x.CMPID == item.cmp).Select(x => x.LogoPath).FirstOrDefault();
                if (regs != null)
                {
                    var osfn = item.cmp + ".png";
                    var osfi = "/CompanyLogo/";
                    var currentDir = Directory.GetCurrentDirectory();
                    string path = currentDir + osfi + osfn;
                    if ((File.Exists(path)))
                    {
                        string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                        string source = imageData;
                        string base64 = source.Substring(source.IndexOf(',') + 1);
                        byte[] datasss = Convert.FromBase64String(base64);
                        datas.ProductImage = imageData;
                    }
                    else
                    {

                    }


                }
                data.SetupMasterFulldetailsS.Add(datas);
            }


            return data;
        }



        public dynamic UpdateSetupdata(SetupMasterViewModel con)
        {
            var data = new SetupMasterViewModel();
            var Countrysetup = new SetupmasterClass();
            var ccmpid = con.cmpid;
            var cmpid = CMPSContext.Setup.Where(x => x.CMPID == Convert.ToInt32(ccmpid)).FirstOrDefault();
            try
            {

                cmpid.UTCTime = con.roomtime;
                cmpid.Country = con.country;
                cmpid.Symbol = con.currency.Trim();
                cmpid.UpdatedUTC = DateTime.UtcNow;
                cmpid.UpdatedBy = 32252;
                cmpid.Pediatric = con.age;
                cmpid.Language = con.Language;
                cmpid.Registrationfeeapplicable = Convert.ToBoolean(con.rfa);
                cmpid.RegistrationDateManage = Convert.ToBoolean(con.rdm);
                cmpid.Insuranceapplicable = Convert.ToBoolean(con.cfa);
                cmpid.IsNotification = Convert.ToBoolean(con.nfa);
                cmpid.GapInterval = con.GAPINTERVAL;
                cmpid.FSFromTime = Convert.ToDateTime(con.FROM).TimeOfDay;
                cmpid.FSToTime = Convert.ToDateTime(con.TO).TimeOfDay;
                cmpid.SSFromTime = Convert.ToDateTime(con.SECFROM).TimeOfDay;
                cmpid.SSToTime = Convert.ToDateTime(con.SECTO).TimeOfDay;
                cmpid.BillingType = con.billtype;
                cmpid.Timezone = con.timezone;
                cmpid.Procurement = con.procurement;
                cmpid.BufferTime = con.BufferTime;
                cmpid.CheckoutTime = con.CheckoutTime;
                CMPSContext.Entry(cmpid).State = EntityState.Modified;
                CMPSContext.SaveChanges();

                if (CMPSContext.SaveChanges() >= 0)

                    return new
                    {
                        Success = true,
                        Message = CommonMessage.saved,

                    };
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            return new
            {
                Success = false,
                Message = CommonMessage.Missing
            };

        }


    }
}



