using System;
using System.Collections.Generic;
using System.Text;
namespace WYNK.Data.Model.ViewModel
{
    public class OfferViewM
    {
        public Offer Offer { get; set; }
        public OfferTran OfferTran { get; set; }

        public ICollection<getModel> getModel { get; set; }
        public ICollection<oneplusmodels> oneplusmodels { get; set; }
        public ICollection<ModelItems> ModelItems { get; set; }
        public ICollection<ModelItems2> ModelItems2 { get; set; }
        public ICollection<oneplusOfferItems> oneplusOfferItems { get; set; }

    }

    public class getModel
    {
        public string Models { get; set; }
        public string LMID { get; set; }
        public int LTID { get; set; }
        public int BrandID { get; set; }
        public string BrandName { get; set; }
    }

    public class ModelItems2
    {

        public string Models { get; set; }
        public int LTID { get; set; }
        public int LMID { get; set; }
        public string OfferType { get; set; }
    }

    public class ModelItems
    {

        public string Models { get; set; }
        public int LTID { get; set; }
        public int LMID { get; set; }
    }



    public class oneplusmodels
    {
        public int OP_LTID { get; set; }
        public string OP_LMID { get; set; }
        public string OP_Model { get; set; }
    }



    public class oneplusOfferItems
    {

        public int LMID { get; set; }
        public int LTID { get; set; }
        public string Models { get; set; }
        public int OfferLMID { get; set; }
        public int OfferLTID { get; set; }
        public string OfferType { get; set; }
        public string oneplusModel { get; set; }

    }
}
