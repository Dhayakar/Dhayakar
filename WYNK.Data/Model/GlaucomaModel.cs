using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class Glaucoma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }
        public string FindingsId { get; set; }
        public int CmpId { get; set; }
        public int? GaunioscopyODG1 { get; set; }
        public int? GaunioscopyODG2 { get; set; }
        public int? GaunioscopyODG3 { get; set; }
        public int? GaunioscopyODG4 { get; set; }
        public int? GaunioscopyOSG1 { get; set; }
        public int? GaunioscopyOSG2 { get; set; }
        public int? GaunioscopyOSG3 { get; set; }
        public int? GaunioscopyOSG4 { get; set; }
        public string pachymetryod { get; set; }
        public string pachymetryos { get; set; }
        public string Impression { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public string GaunioscopyODD1 { get; set; }
        public string GaunioscopyODD2 { get; set; }
        public string GaunioscopyODD3 { get; set; }
        public string GaunioscopyODD4 { get; set; }
        public string GaunioscopyOSD1 { get; set; }
        public string GaunioscopyOSD2 { get; set; }
        public string GaunioscopyOSD3 { get; set; }
        public string GaunioscopyOSD4 { get; set; }


    }
}


