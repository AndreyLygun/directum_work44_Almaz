using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
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
    public string ResponsibleID { get; set; }
    public string ResponsibleFIO { get; set; }
    public Employee[] Employees { get; set; }
    public Attachment[] Attachments { get; set; }
  }

  struct Employee
  {
    public string ID { get; set; }
    public string FIO { get; set; }
    public string DepartmentID { get; set; }
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
    /// <summary>
    /// Функция интеграции для импорта приказа по персоналу:
    /// считывает из указанного JSON файла свойства приказа по персоналу
    /// и создаёт его вместе с приложениями к приказу
    /// </summary>
    /// <param name="jsonFileName">сетевое имя JSON-файла</param>
    /// <returns></returns>
    [Public(WebApiRequestType = RequestType.Get)]
    public string ImportEmployeeOrderV1(string jsonFileName) {
      Response response = new Response() {Status = 1, Message = "", URL = ""};
      try{
        // Читаем данные из JSON
        Document docObject;
        string json="";
        try {
          jsonFileName = Uri.UnescapeDataString(jsonFileName);
          json = File.ReadAllText(jsonFileName);
        } catch(Exception ex) {
          Logger.Error($"Ошибка при чтении файла '{jsonFileName}' в функции ImportEmployeeOrderV1", ex);
          response.Message = $"Ошибка при чтении файла '{jsonFileName}'. Подробное описание см. в лог-файле";
          throw ex;
        }
        
        docObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Document>(json);

        // Получаем из полученной структуры объекты Directum
        var kind = RRU.Almaz.DocumentKinds.GetAll(d => d.GalaxyKindRRU==docObject.DocKind).FirstOrDefault();
        if (kind==null) throw new Exception("Ошибка в функции 'ImportEmployeeOrderV1': в Directum не найден вид документа с полем 'ID вида в Галактике' = {docObject.DocKind}");

        var date = new DateTime();
        date = DateTime.ParseExact(docObject.DocDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        //        Calendar.TryParseDate(docObject.DocDate, out date);

        var responsible = Sungero.Company.Employees.GetAll(e => e.PersonnelNumber == docObject.ResponsibleID).FirstOrDefault();
        if (responsible==null) {
          response.Message = $"Не удалось найти сотрудника c табельным номером {docObject.ResponsibleID}";
          Logger.Error("Ошибка в в функции 'ImportEmployeeOrderV1': " + response.Message);
          return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        }
        
        var org = Sungero.Company.BusinessUnits.GetAll().FirstOrDefault();
        if (org==null) {
          response.Message = $"В Directum не создано ни одной организации.";
          Logger.Error("Ошибка в в функции 'ImportEmployeeOrderV1': " + response.Message);
          return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        }

        IEmployeeOrder doc = null;
        string path = "";
        try {
          path = System.IO.Path.GetDirectoryName(jsonFileName);
          string fileName = path+"/" + docObject.FileName;
          doc = EmployeeOrders.CreateFrom(fileName);
          doc.Department = Sungero.Company.Departments.Null;
        } catch {
          response.Message = $"В Directum не создано ни одной организации.";
        }


        doc.DocumentKind = kind;
        doc.GalaxyIDRRU = docObject.GalaxyID;
        doc.Subject = docObject.Description;
        doc.RegistrationNumber = docObject.DocNum;
        doc.BusinessUnit = org;
        doc.Author = responsible;
        doc.PreparedBy = responsible;
        doc.DocumentDate = date;
        doc.RegistrationDate = date;
        
        // Заполняем таблицу с сотрудниками
        foreach(Employee row in docObject.Employees){
          var employee = Sungero.Company.Employees.GetAll(e => e.PersonnelNumber.Equals(row.ID)).FirstOrDefault();
          if (employee == null) {
            // Создаём Person с указанными ФИО
            var person = Sungero.Parties.People.Create();            
            string[] names = row.FIO.Split(' ');
            person.LastName = names[0];
            person.FirstName = names[1];
            person.MiddleName = names[2];
            person.Save();
            // Создаём сотрудника
            employee = Sungero.Company.Employees.Create();
            employee.NeedNotifyAssignmentsSummary = false;
            employee.NeedNotifyExpiredAssignments = false;
            employee.NeedNotifyNewAssignments = false;              
            employee.Person = person;
            employee.PersonnelNumber = row.ID;
            var department = Sungero.Company.Departments.GetAll(d => d.Code==row.DepartmentID).FirstOrDefault();
            if (department==null) {
              response.Message = $"Не удалось найти отдел с кодом {row.DepartmentID} ({row.FIO}) ";
              Logger.Error("Ошибка в в функции 'ImportEmployeeOrderV1': " + response.Message);
              return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            employee.Department = department;
          }
          doc.Employees.AddNew().Employee = employee;
          // Заполняем поле "Отдел" документа сведениями по первому сотруднику в списке сотрудников, чтобы приказ мог попасть на согласование начальнику отдела
          // Если в приказе есть сотрудники нескольких отделов, согласующему нужно вручную добавить в согласование дополнительного согласующего 
          if (doc.Department.Equals(Sungero.Company.Departments.Null)) 
            doc.Department = employee.Department;
        }
        doc.Save();
        
        // Импортирум приложения
        foreach(Attachment attachInfo in docObject.Attachments) {
          var attach = Sungero.Docflow.Addendums.CreateFrom(path + "/" + attachInfo.FileName);
          attach.Subject = attachInfo.Description;
          //        attach.Name = attachInfo.Description;
          attach.LeadingDocument = doc;
          attach.Relations.Add("Addendum", doc);
          attach.Save();
        }
        response.Status = 0;
        response.Message = "Документ успешно отправлен в Directum";
        string typeid = EmployeeOrder.ClassTypeGuid.ToString();
        response.URL = @"/Client/#/card/"+typeid+"/"+ doc.Id.ToString();
      }
      catch(Exception ex) {
        response.Message = ex.Message;
        Logger.Error(ex.Message, ex);
      }
      return Newtonsoft.Json.JsonConvert.SerializeObject(response);
    }
  }
}