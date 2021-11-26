using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
  public  class TransactionTypeViewM
    {
        public TransactionType TransactionType { get; set; }
        public ICollection<getContraDet> getContraDet { get; set; }
    }

    public class getContraDet
    {
        public int TransactionID { get; set; }
        public string Description { get; set; }

    }



    }
