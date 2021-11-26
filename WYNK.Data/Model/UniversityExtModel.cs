using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WYNK.Data.Model
{
   public  class UniversityExt
    {

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UniversityExtCode { get; set; }
    public string UniversityExtDescription { get; set; }
    public int UniversityCode{ get; set; }
    public int Locationid { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedUTC { get; set; }
    public DateTime? UpdatedUTC { get; set; }
    public int CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }



 

    }
}
