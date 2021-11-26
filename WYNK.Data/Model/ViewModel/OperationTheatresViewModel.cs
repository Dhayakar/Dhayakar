
using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class OperationTheatres
    {
        public OperationTheatre OperationTheatre { get; set; }

        public ICollection<OperationExtension> OperationExtension { get; set; }
    }
}






