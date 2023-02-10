
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
      Response response = new Response() {Status = -1, Message = "", URL = ""};

      try {
        // Читаем данные из JSON
        Document docObject;
        jsonFileName = Uri.UnescapeDataString(jsonFileName);
        if (!File.Exists(jsonFileName))
          throw new Exception($"Не найден JSON файл {jsonFileName}");
        string json = File.ReadAllText(jsonFileName);
        try {
          docObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Document>(json);
        }
        catch {
          throw new Exception($"Не удалось преобразовать JSON-данные в файле {jsonFileName}");
        }
        // Получаем из полученной структуры объекты Directum (вид документа, ответственный пользователь и пр.)
        var kind = RRU.Almaz.DocumentKinds.GetAll(d => (d.DocumentType.Name.Equals("Приказы по персоналу")) && (d.GalaxyKindRRU==docObject.DocKind)).FirstOrDefault();
        //        var kind = RRU.Almaz.DocumentKinds.GetAll(d => (d.GalaxyKindRRU==docObject.DocKind)).FirstOrDefault();
        if (kind==null)
          throw new Exception($"В Directum не найден вид документа с типом 'Приказы по персоналу' и значением поля 'ID вида в Галактике' = {docObject.DocKind}");

        var date = new DateTime();
        date = DateTime.ParseExact(docObject.DocDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        var responsible = Sungero.Company.Employees.GetAll(e => e.PersonnelNumber == docObject.ResponsibleID).FirstOrDefault();
        if (responsible==null)
          throw new Exception($"Не удалось найти сотрудника c табельным номером {docObject.ResponsibleID}, ответственного за приказ.");
        
        var doc = EmployeeOrders.GetAll(d => d.GalaxyIDRRU.Equals(docObject.GalaxyID)).FirstOrDefault();
        if (doc == null) {
          doc = EmployeeOrders.Create();
          doc.Department = Sungero.Company.Departments.Null;
        } else {
          doc.Employees.Clear(); // Если документ уже существовал, удаляем всех сотрудников, чтобы добавить их заново
          response.Status = 2;
          response.Message = "Свойства документа обновлены. Текст приказа и приложений внесены в новые версии.";
          string URL = Sungero.Core.Hyperlinks.Get(doc);
          var regexResult = Regex.Matches(URL, @"https?:\/\/.*\/(.*)", RegexOptions.IgnoreCase);
          response.URL = regexResult[0].Groups[1].Value; // извлекаем из URI путь (убираем имя домена)
          return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        }

        if (!File.Exists(docObject.FileName))
          throw new Exception($"Не найден файл с приказом {docObject.FileName}");
        doc.CreateVersionFrom(docObject.FileName);

        doc.DocumentKind = kind;
        doc.GalaxyIDRRU = docObject.GalaxyID;
        doc.Subject = docObject.Description;
        doc.RegistrationNumber = docObject.DocNum;
        doc.Author = responsible;
        doc.PreparedBy = responsible;
        doc.DocumentDate = date;
        doc.RegistrationDate = date;
        
        // Заполняем таблицу с сотрудниками.
        foreach(Employee row in docObject.Employees){
          if (string.IsNullOrEmpty(row.FIO) | string.IsNullOrEmpty(row.ID) | string.IsNullOrEmpty(row.DepartmentID))
            throw new Exception("Некоторые JSON-данные в списке сотрудников имеют неправильный формат");
          var employee = Sungero.Company.Employees.GetAll(e => e.PersonnelNumber.Equals(row.ID)).FirstOrDefault();
          if (employee == null) {
            // Создаём Person с указанными ФИО
            var person = Sungero.Parties.People.Create();
            string[] names = row.FIO.Split(' ');
            if (names.Count()<2)
              throw new Exception($"ФИО сотрудника в передаваемых данных должно содержать как фамилию, так и имя (табельный номер {row.ID})");
            person.LastName = names[0];
            person.FirstName = names[1];
            if (names.Count()>=2)
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
            if (department==null)
              throw new Exception($"В Directum отсутствует подразделение с кодом {row.DepartmentID}. Попросите админстратора добавить его.");
            employee.Department = department;
          }
          doc.Employees.AddNew().Employee = employee;
          // Заполняем поле "Отдел" документа сведениями по первому сотруднику в списке сотрудников, чтобы приказ мог попасть на согласование начальнику отдела
          // Если в приказе есть сотрудники нескольких отделов, согласующему нужно вручную добавить в согласование дополнительного согласующего
          if (doc.Department==null) {
            if (employee.Department.BusinessUnit==null)
              throw new Exception($"У отдела '{employee.Department.Name}' не указана организация.");
            doc.Department = employee.Department;
            doc.BusinessUnit = employee.Department.BusinessUnit;
          }
        }
        doc.Save();
        doc.AccessRights.Grant(responsible, DefaultAccessRightsTypes.FullAccess);
        doc.AccessRights.Save();        
        
        // Импортирум приложения
        foreach(Attachment attachInfo in docObject.Attachments) {
          if (!File.Exists(docObject.FileName))
            throw new Exception($"Не найден файл с приложением {docObject.FileName}");
          var attach =  Sungero.Docflow.OfficialDocuments.As(doc.Relations.GetRelated("Addendum").Where(d => Sungero.Docflow.OfficialDocuments.As(d).Subject == attachInfo.Description).FirstOrDefault());
          if (attach == null) {
            attach = Sungero.Docflow.Addendums.CreateFrom(attachInfo.FileName);
            attach.Subject = attachInfo.Description;
            attach.LeadingDocument = doc;
            attach.Relations.Add("Addendum", doc);
          } else {
            attach.CreateVersionFrom(attachInfo.FileName);
          }
          attach.Save();
        }
        if (response.Status==-1) {
          response.Status = 1;
          response.Message = "Документ успешно отправлен в Directum";
          string docURL = Sungero.Core.Hyperlinks.Get(doc);
          var regexRes = Regex.Matches(docURL, @"https?:\/\/.*\/(.*)", RegexOptions.IgnoreCase);
          response.URL = regexRes[0].Groups[1].Value; // извлекаем из URI путь (убираем имя домена)
        }
      }
      catch(Exception ex) {
        response.Status = -1;
        response.Message = ex.Message;
        Logger.Error(ex.Message, ex);
      }
      return Newtonsoft.Json.JsonConvert.SerializeObject(response);
      //      return Newtonsoft.Json.JsonConvert.SerializeObject(response);
    }
    
    
    /// <summary>
    /// Функция интеграции для импорта приказа по персоналу:
    /// считывает из указанного JSON файла свойства приказа по персоналу
    /// и создаёт его вместе с приложениями к приказу
    /// </summary>
    /// <param name="jsonFileName">сетевое имя JSON-файла</param>
    /// <returns></returns>
//    [Public(WebApiRequestType = RequestType.Get)]
//    public string ImportEmployeeOrderV0(string jsonFileName) {
//      Response response = new Response() {Status = -1, Message = "", URL = ""};
//
//      try {
//        // Читаем данные из JSON
//        Document docObject;
//        jsonFileName = Uri.UnescapeDataString(jsonFileName);
//        if (!File.Exists(jsonFileName))
//          throw new Exception($"Не найден JSON файл {jsonFileName}");
//        string json = File.ReadAllText(jsonFileName);
//        try {
//          docObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Document>(json);
//        }
//        catch {
//          throw new Exception($"Не удалось преобразовать JSON-данные в файле {jsonFileName}");
//        }
//        // Получаем из полученной структуры объекты Directum
//        var kind = RRU.Almaz.DocumentKinds.GetAll(d => (d.DocumentType.Name=="Приказы по персоналу") && (d.GalaxyKindRRU==docObject.DocKind)).FirstOrDefault();
//
//        if (kind==null)
//          throw new Exception($"В Directum не найден вид документа с типом 'Приказы по персоналу' и значением поля 'ID вида в Галактике' = {docObject.DocKind}");
//
//        var date = new DateTime();
//        date = DateTime.ParseExact(docObject.DocDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
//
//        var responsible = Sungero.Company.Employees.GetAll(e => e.PersonnelNumber == docObject.ResponsibleID).FirstOrDefault();
//        if (responsible==null)
//          throw new Exception($"Не удалось найти сотрудника c табельным номером {docObject.ResponsibleID}, ответственного за приказ.");
//        
//        var doc = EmployeeOrders.GetAll(d => d.GalaxyIDRRU.Equals(docObject.GalaxyID)).FirstOrDefault();
//        
//        if (doc != null) {
//          response.Status = 2;
//          response.Message = "Свойства документа обновлены. Текст приказа и приложений внесены в новые версии.";
//          string URL = Sungero.Core.Hyperlinks.Get(doc);
//          var regexResult = Regex.Matches(URL, @"https?:\/\/.*\/(.*)", RegexOptions.IgnoreCase);
//          response.URL = regexResult[0].Groups[1].Value; // извлекаем из URI путь (убираем имя домена)
//          return Newtonsoft.Json.JsonConvert.SerializeObject(response);
//        }
//        if (!File.Exists(docObject.FileName))
//          throw new Exception($"Не найден файл с приказом {docObject.FileName}");
//        doc = EmployeeOrders.CreateFrom(docObject.FileName);
//        doc.Department = Sungero.Company.Departments.Null;
//
//        doc.DocumentKind = kind;
//        doc.GalaxyIDRRU = docObject.GalaxyID;
//        doc.Subject = docObject.Description;
//        doc.RegistrationNumber = docObject.DocNum;
//        doc.Author = responsible;
//        doc.PreparedBy = responsible;
//        doc.DocumentDate = date;
//        doc.RegistrationDate = date;
//        
//        // Заполняем таблицу с сотрудниками
//        foreach(Employee row in docObject.Employees){
//          if (string.IsNullOrEmpty(row.FIO) | string.IsNullOrEmpty(row.ID) | string.IsNullOrEmpty(row.DepartmentID))
//            throw new Exception("Некоторые JSON-данные в списке сотрудников имеют неправильный формат");
//          var employee = Sungero.Company.Employees.GetAll(e => e.PersonnelNumber.Equals(row.ID)).FirstOrDefault();
//          if (employee == null) {
//            // Создаём Person с указанными ФИО
//            var person = Sungero.Parties.People.Create();
//            string[] names = row.FIO.Split(' ');
//            if (names.Count()<2)
//              throw new Exception($"ФИО сотрудника в передаваемых данных должно содержать как фамилию, так и имя (табельный номер {row.ID})");
//            person.LastName = names[0];
//            person.FirstName = names[1];
//            if (names.Count()>=2)
//              person.MiddleName = names[2];
//            person.Save();
//            // Создаём сотрудника
//            employee = Sungero.Company.Employees.Create();
//            employee.NeedNotifyAssignmentsSummary = false;
//            employee.NeedNotifyExpiredAssignments = false;
//            employee.NeedNotifyNewAssignments = false;
//            employee.Person = person;
//            employee.PersonnelNumber = row.ID;
//            var department = Sungero.Company.Departments.GetAll(d => d.Code==row.DepartmentID).FirstOrDefault();
//            if (department==null)
//              throw new Exception($"В Directum отсутствует подразделение с кодом {row.DepartmentID}. Попросите админстратора добавить его.");
//            employee.Department = department;
//          }
//          doc.Employees.AddNew().Employee = employee;
//          // Заполняем поле "Отдел" документа сведениями по первому сотруднику в списке сотрудников, чтобы приказ мог попасть на согласование начальнику отдела
//          // Если в приказе есть сотрудники нескольких отделов, согласующему нужно вручную добавить в согласование дополнительного согласующего
//          if (doc.Department==null) {
//            if (employee.Department.BusinessUnit==null)
//              throw new Exception($"У отдела '{employee.Department.Name}' не указана организация.");
//            doc.Department = employee.Department;
//            doc.BusinessUnit = employee.Department.BusinessUnit;
//          }
//        }
//        doc.Save();
//        doc.AccessRights.Grant(responsible, DefaultAccessRightsTypes.FullAccess);
//        doc.AccessRights.Save();
//
//        
//        // Импортирум приложения
//        foreach(Attachment attachInfo in docObject.Attachments) {
//          if (!File.Exists(docObject.FileName))
//            throw new Exception($"Не найден файл с приложением {docObject.FileName}");
//          var attach = Sungero.Docflow.Addendums.CreateFrom(attachInfo.FileName);
//          attach.Subject = attachInfo.Description;
//          attach.LeadingDocument = doc;
//          attach.Relations.Add("Addendum", doc);
//          attach.Save();
//        }
//        if (response.Status==-1) {
//          response.Status = 1;
//          response.Message = "Документ успешно отправлен в Directum";
//          string docURL = Sungero.Core.Hyperlinks.Get(doc);
//          var regexRes = Regex.Matches(docURL, @"https?:\/\/.*\/(.*)", RegexOptions.IgnoreCase);
//          response.URL = regexRes[0].Groups[1].Value; // извлекаем из URI путь (убираем имя домена)
//        }
//      }
//      catch(Exception ex) {
//        response.Message = ex.Message;
//        Logger.Error(ex.Message, ex);
//      }
//      return Newtonsoft.Json.JsonConvert.SerializeObject(response);
//      //      return Newtonsoft.Json.JsonConvert.SerializeObject(response);
//    }
  }
}