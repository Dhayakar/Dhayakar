using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class OpticalOrderTran
    {
        [Key]
        public int      ID                      { get; set; }
        public string    RandomUniqueID         { get; set; }
        public int      LTID                    { get; set; }
        public int      UOMID                   { get; set; }
        public Decimal  OrderedQty              { get; set; }
        public Decimal  ReceivedQty             { get; set; }
        public Decimal  Price                   { get; set; }
        public Decimal?  DiscountPercentage      { get; set; }
        public Decimal?  DiscountAmount          { get; set; }
        public Decimal?  GSTPercentage           { get; set; }
        public Decimal?  GSTTaxValue             { get; set; }
        public Decimal?  CGSTPercentage          { get; set; }
        public Decimal?  CGSTTaxValue            { get; set; }
        public Decimal?  SGSTPercentage          { get; set; }
        public Decimal?  SGSTTaxValue            { get; set; }
        public Decimal?  IGSTPercentage          { get; set; }
        public Decimal?  IGSTTaxValue            { get; set; }
        public Decimal?  CESSPercentage          { get; set; }
        public Decimal?  CESSAmount              { get; set; }
        public Decimal?  AdditionalCESSPercentage{ get; set; }
        public Decimal?  AddCESSPerAmt           { get; set; }
        public bool     IsCancelled             { get; set; }
        public int?      ContraOOID              { get; set; }
        public DateTime CreatedUTC              { get; set; }
        public DateTime? UpdatedUTC              { get; set; }
        public int      CreatedBy               { get; set; }
        public int?      UpdatedBy               { get; set; }









    }
}
