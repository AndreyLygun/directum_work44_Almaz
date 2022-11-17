using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Sungero.Core;
using Sungero.CoreEntities;
using Newtonsoft.Json;

namespace RRU.EmployeeModule.Server
{
  
  struct Document
  {
    public string DocKind { get; set; }
    public string GalaxyID { get; set; }
    public string DocDate { get; set; }
    public string Description { get; set; }
    public string FileName { get; set; }
    public string DocNum { get; set; }
    public string DepartmentID { get; set; }
    public string ResponsibleID { get; set; }
    public string ResponsibleFIO { get; set; }
    public Employee[] Employees { get; set; }
    public Attachment[] Attachments { get; set; }
  }

  struct Employee
  {
    public string ID { get; set; }
    public string FIO { get; set; }
  }

  struct Attachment
  {
    public string FileName { get; set; }
    public string Description { get; set; }
  }

  struct Response
  {
    public int Status;
    public string Message;
    public string URL;
  }

  
  public class ModuleFunctions
  {
    
    [Public(WebApiRequestType = RequestType.Get)]
    public string ImportEmployeeOrderV1(string jsonFileName) {

      Response response = new Response() {Status = 1, Message = "", URL = ""};
      // Получаем данные из JSON
      jsonFileName = @"c:\temp\DRX\data.json";
      string path = System.IO.Path.GetDirectoryName(jsonFileName);
      string json = File.ReadAllText(jsonFileName);
      Document docObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Document>(json);

      // Получаем из полученной структуры объекты Directum
      var kind = RRU.Almaz.DocumentKinds.GetAll(d => d.GalaxyKindRRU==docObject.DocKind).FirstOrDefault();
      if (kind==null) {
        response.Message = $"Не найден вид документа со значением 'ID вида в Галактике' равным {docObject.DocKind}";
        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
      }

      var date = new DateTime();
      Calendar.TryParseDate(docObject.DocDate, out date);

      var department = Sungero.Company.Departments.GetAll(d => d.Code==docObject.DepartmentID).FirstOrDefault();
      if (department==null) {
        response.Message = $"Не удалось найти отдел с кодом {docObject.DepartmentID}";
        return Newtonsoft.Json.JsonConvert.SerializeObject(response);        
      }

      var responsible = Sungero.Company.Employees.GetAll(e => e.PersonnelNumber == docObject.ResponsibleID).FirstOrDefault();
      if (responsible==null) {
        response.Message = $"Не удалось найти сотрудника c табельным номером {docObject.ResponsibleID}";
        return Newtonsoft.Json.JsonConvert.SerializeObject(response);        
      }
      
      var org = Sungero.Company.BusinessUnits.GetAll().FirstOrDefault();
      if (org==null) {
        response.Message = $"В Directum не создано ни одной организации";
        return Newtonsoft.Json.JsonConvert.SerializeObject(response);        
      }

      var doc = EmployeeOrders.CreateFrom(path+"/" + docObject.FileName);

      doc.DocumentKind = kind;
      doc.GalaxyIDRRU = docObject.GalaxyID;      
      doc.Subject = docObject.Description;
      doc.RegistrationNumber = docObject.DocNum;
      doc.BusinessUnit = org;
      doc.Department = department;
      doc.Author = responsible;
      doc.PreparedBy = responsible;
//      doc.Name = "Приказ " + doc.RegistrationNumber + " " + doc.Note;
      foreach(Employee emp in docObject.Employees){
        var e = doc.Employees.AddNew();
        e.Name = emp.FIO;
        e.EmployeeID = emp.ID;
//        e.Save();
      }
      foreach(Attachment attachInfo in docObject.Attachments) {
        var attach = Sungero.Docflow.Addendums.CreateFrom(path + "/" + attachInfo.FileName);
        attach.Subject = attachInfo.Description;
//        attach.Name = attachInfo.Description;
        attach.LeadingDocument = doc;
        attach.Relations.Add("Addendum", doc);
        attach.Save();
//        doc.Relations.Add("Addendum", attach);        
      }      
      doc.Save();

      response.Status = 0;
      response.Message = "Документ успешно отправлен в Directum";
      response.URL = "http://www.ricoh.ru";
      return Newtonsoft.Json.JsonConvert.SerializeObject(response);
    }
  }
}