using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class IOTemplateTran
    {
        [Key]
        public int ID { get; set; }
        public int IOTemplateID { get; set; }
        public string OTNotesDescription { get; set; }
        public string UserInputType { get; set; }
        public string InputValue { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}












