using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace RRU.EmployeeModule.Server
{
  
 
  public class ModuleFunctions
  {

      
    [Public(WebApiRequestType = RequestType.Get)]
    public string ImportEmployeeOrderV1(string jsonName) {
      return jsonName;
    }
    [Public(WebApiRequestType = RequestType.Get)]
    public string ImportEmployeeOrderV2(int jsonName) {
      return "Всё получилось";
    }
    
  }
}