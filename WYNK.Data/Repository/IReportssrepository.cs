using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IReportssrepository : IRepositoryBase<Vreportss>
    {

        Vreportss Todaysearch(string fromDate, string toDate, int CompanyID);
        Vreportss Getnewvisits(string date, string Newcount, int CompanyID);
        Vreportss Getreviewvisits(string date, string Newcount, int CompanyID);
        Vreportss Getsurgreviewvisits(string date, string Newcount, int CompanyID);

        Vreportss GetBranchwisedetails(string date, int CompanyID);

        Vreportss Getnewgeneralocular(DateTime date, string Generalocular, string cmpid, string CMPbranch);
        Vreportss GetnewgeneralNormal(DateTime date, string Generalocular, string cmpid, string CMPbranch);
        Vreportss GetnewpediatricNormal(DateTime date, string Generalocular, string cmpid, string CMPbranch);
        Vreportss Getnewpediatricocular(DateTime date, string Generalocular, string cmpid, string CMPbranch);
        Vreportss Getnewtotal(DateTime date, string Generalocular, string cmpid, string CMPbranch);


        Vreportss GetReviewgeneralNormal(DateTime date, string Generalocular, string cmpid, string CMPbranch);
        Vreportss GetReviewgeneralocular(DateTime date, string Generalocular, string cmpid, string CMPbranch);
        Vreportss GetReviewpediatricnormal(DateTime date, string Generalocular, string cmpid, string CMPbranch);
        Vreportss GetReviewpediatricocular(DateTime date, string Generalocular, string cmpid, string CMPbranch);
        

        Vreportss Getviewtotal(DateTime date, string Generalocular, string cmpid, string CMPbranch);

        Vreportss GetnewCumulativetotal(DateTime date, string Generalocular, string cmpid, string CMPbranch);

    }
}
