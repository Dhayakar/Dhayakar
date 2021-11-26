using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Common
{

    public sealed class GenerateNumber
    {
        public GeneratingNumberEntity GenerateRunning(GeneratingNumberEntity generatingNumber)
        {
            GeneratingNumberEntity generating = new GeneratingNumberEntity();

            return generating;
        }
    }


    public class GeneratingNumberEntity
    {
        public int VCID { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public bool Autonumber { get; set; }
        public Int64 RunningNumber { get; set; }
    }
}
