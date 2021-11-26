using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
   public class ExcelViewModel
    {

        public ICollection<RegistrationExcel> Excelimport { get; set; }
        public ICollection<ErrorRegistrationExcel> ErrorExcelimport { get; set; }
        public ICollection<DrugExcelimport> DrugExcelimport { get; set; }
        public ICollection<Strabismusimport> Strabismusimport { get; set; }
        public ICollection<SurgeryDetailsExcelImport> SurgeryDetailsExcelImport { get; set; }
        public ICollection<GetOTNotesValues> GetOTNotesValues { get; set; }


    }



    public class DrugStockExcel
    {
        public int? StoreId { get; set; }
        public int? VendorID { get; set; }
        public ICollection<DrugStockExcelimport> DrugStockExcelimports { get; set; }
        public string RunningNo { get; set; }
    }



    public class OpticalStockExcel
    {
        public int? StoreId { get; set; }
        public int? VendorID { get; set; }
        public ICollection<OptcialStockExcelimport> OpticalStockExcelimports { get; set; }
        public string RunningNo { get; set; }
    }

    public class OptcialStockExcelimport 
    {
        public int OpticalID { get; set; }
        public string Brand { get; set; }
        public string LensOptions { get; set; }
        public string LensType { get; set; }
        public string Model { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public int Quantity { get; set; }



    }

    public class ErrorRegistrationExcel
    {
        public string UIN { get; set; }
        public string DateofRegistration { get; set; }

        public string Status { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DateofBirth { get; set; }
        public string Gender { get; set; }
        public string Address1 { get; set; }
        public string Phone { get; set; }
        public int Fees { get; set; }
    }

    public class RegistrationExcel
    {
        public string UIN { get; set; }
        public string DateofRegistration { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DateofBirth { get; set; }
        public string Gender { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public int Fees { get; set; }    
    }


    public class DrugExcelimport
    {
        public int ID { get; set; }
        public string Brand { get; set; }
        public string GenericName { get; set; }
        public string Manufacturer { get; set; }
        public string UOM { get; set; }
        public string DrugGroup { get; set; }
        public string Rate { get; set; }
        public string DrugCategory { get; set; }
        public string DrugTracker { get; set; }
        public string HSNO { get; set; }
        public string DrugSubDescription { get; set; }
        public string AConstant { get; set; }
        public string OpticDia { get; set; }
        public string ModelNo { get; set; }
        public string Length { get; set; }
        public string DrugComposition { get; set; }
        public string Status { get; set; }

    }


    public class Strabismusimport
    {
        public int ID { get; set; }
        public string ParentDescription { get; set; }
        public string ParentTag { get; set; }
        public string Status { get; set; }

    }


    public class DrugStockExcelimport
    {
        public int DrugID { get; set; }
        public string Brand { get; set; }
        public string Generic { get; set; }
        public string BatchSerial { get; set; }
        public DateTime? Date { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }

    public class SurgeryDetailsExcelImport
    {
        public string ICDSpeciality { get; set; }
        public string ICDCode { get; set; }
        public string SurgeryDescription { get; set; }
        public string OTNotes { get; set; }
        public string UserInput { get; set; }
        public string InputValue { get; set; }
        public string Status { get; set; }
    }

    public class GetOTNotesValues
    {
        public string OTNotes { get; set; }
        public string UserInput { get; set; }
        public string InputValue { get; set; }
        public string Status { get; set; }
    }

}
