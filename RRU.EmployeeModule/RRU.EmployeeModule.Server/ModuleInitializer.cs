using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Sungero.Domain.Initialization;

namespace RRU.EmployeeModule.Server
{
  public partial class ModuleInitializer
  {

    /// <summary>
    /// 
    /// </summary>
    public void CreateDocumentType()      
    {
      Sungero.Docflow.PublicInitializationFunctions.Module.CreateDocumentType("Приказы по персоналу", EmployeeOrder.ClassTypeGuid, 
                                                                              Sungero.Docflow.DocumentType.DocumentFlow.Inner, true);
    }
    public void CreateDocumentKind() {
      string[,] docKinds = {
        ["2", "Приказ о предоставлении отпуска"],
        ["2", "Приказ о предоставлении отпуска"],
      };
//      Sungero.Docflow.PublicInitializationFunctions.Module.CreateDocumentKind();
      
    }
       
  }
}
