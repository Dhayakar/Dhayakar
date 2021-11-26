using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class IndentViewModel
    {
        public ICollection<DrugssDetailss> DrugssDetailsss { get; set; }

        //public OTIndentModel Indentmodel { get; set; }
        public string Userid { get; set; }
        public IndentTotalData Indentdetails { get; set; }
        public string transactionid { get; set; }
        public ICollection<Gridadata> GridDeatils { get; set; }

        
        public string Statuss { get; set; }
        public string indentnumbers { get; set; }
        public string message { get; set; }
    }



    public class DrugssDetailss
    {
        public int ID { get; set; }
        public string Brand { get; set; }
        public string Manufacture { get; set; }
        public string Drugdroup { get; set; }
        public string Generic { get; set; }

        public DateTime OTRAISEDDATE { get; set; }
        public string UOM { get; set; }




    }


    public class Gridadata
    {
        public string Manufacture { get; set; }
        public string UOM { get; set; }
        public string Generic { get; set; }
        public string Brand { get; set; }
        public int IndentQty { get; set; }
        public string Drugdroup { get; set; }
   // public string OTRAISEDDATE { get; set; }

    }

    public class IndentTotalData
    {
        public string OperationtheatreID { get; set; }
        public string OTRaisedBY { get; set; }
        public string OTIndentdate { get; set; }
        public string Storename { get; set; }


    }
}
