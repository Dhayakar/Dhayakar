using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class SlitlampExtn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }
        public string FindingsID { get; set; }
        public string HorMeasurementod1 { get; set; }
        public string VerMeasurementod1 { get; set; }
        public int? OcularMovementod1 { get; set; }
        public string HorMeasurementod2 { get; set; }
        public string VerMeasurementod2 { get; set; }
        public int? OcularMovementod2 { get; set; }
        public string HorMeasurementod3 { get; set; }
        public string VerMeasurementod3 { get; set; }
        public int? OcularMovementod3 { get; set; }
        public string HorMeasurementod4 { get; set; }
        public string VerMeasurementod4 { get; set; }
        public int? OcularMovementod4 { get; set; }
        public string HorMeasurementod5 { get; set; }
        public string VerMeasurementod5 { get; set; }
        public int? OcularMovementod5 { get; set; }
        public string HorMeasurementod6 { get; set; }
        public string VerMeasurementod6 { get; set; }
        public int? OcularMovementod6 { get; set; }
        public int? OcularMovementod7 { get; set; }
        public int? OcularMovementod8 { get; set; }
        public string HorMeasurementos1 { get; set; }
        public string VerMeasurementos1 { get; set; }
        public int? OcularMovementos1 { get; set; }
        public string HorMeasurementos2 { get; set; }
        public string VerMeasurementos2 { get; set; }
        public int? OcularMovementos2 { get; set; }
        public string HorMeasurementos3 { get; set; }
        public string VerMeasurementos3 { get; set; }
        public int? OcularMovementos3 { get; set; }
        public string HorMeasurementos4 { get; set; }
        public string VerMeasurementos4 { get; set; }
        public int? OcularMovementos4 { get; set; }
        public string HorMeasurementos5 { get; set; }
        public string VerMeasurementos5 { get; set; }
        public int? OcularMovementos5 { get; set; }
        public string HorMeasurementos6 { get; set; }
        public string VerMeasurementos6 { get; set; }
        public int? OcularMovementos6 { get; set; }
        public int? OcularMovementos7 { get; set; }
        public int? OcularMovementos8 { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedUTC { get; set; }


    }
}


