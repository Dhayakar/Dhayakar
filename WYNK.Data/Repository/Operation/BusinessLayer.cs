using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Implementation;

namespace WYNK.Data.Repository.Operation
{
    class BusinessLayer
    {

        public bool CheckStock(int quantity, int drugid, WYNKContext item)
        {
            var medicalprescription = new Medical_Prescription();
            bool Status = false;

            var itembal = item.ItemBalance;

            //var actstk = from res in itembal.whe


            //var actstk = JsonConvert.SerializeObject(itembalance);

            //var entireJson = JToken.Parse(actstk);

            //System.Diagnostics.Debug.WriteLine(entireJson["ItemID"].ToString());

            //var propertyList = entireJson.wh


            //var propertyList = (JObject)entireJson[];

            //foreach (var property in propertyList)
            //{
            //    var key = property.Key;
            //    var value = (bool)property.Value;

            //    //if (value)
            //    //{
            //    //    key.Dump();
            //    //}
            //}




            return Status;



        }


        public static ACtStock chestock(ItemBalance item)
        {
            var ACtStock = new ACtStock();
            if (item != null)
            {
                ACtStock.quantity = item.ClosingBalance;
            }
            //var ab = item.ToArray();

          
            //foreach (var i in item.ToArray())

            //{


            //    var medicalprescription = new Medical_Prescription();


            //    medicalprescription.Restock = i.ClosingBalance;




            //}

            return ACtStock;
        }


    }
}
