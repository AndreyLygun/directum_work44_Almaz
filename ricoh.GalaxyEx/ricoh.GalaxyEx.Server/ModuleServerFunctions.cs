using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Newtonsoft.Json.Linq;

namespace ricoh.GalaxyEx.Server
{
  public class ModuleFunctions
  {

    /// <summary>
    /// 
    /// </summary>
    /// 
    [Public(WebApiRequestType = RequestType.Post)]
    public string ImportJsonDocument()
    {
      string json;
      try {
        json = File.ReadAllText("c\temp\test.json");
      } catch {
        return "Не удалось прочитать файл"; 
      }
      var data = JObject.Parse(json);
      
          

      var filename = "c\temp\test.docx";
      GalaxyEx.OrderEmployees.CreateFrom(filename);
      // string json = File.ReadAllText("c:/temp/test.json");
      return "Ура!";
    }

  }
}