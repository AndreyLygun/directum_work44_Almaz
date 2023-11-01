using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Sungero.Domain.Initialization;

namespace RRU.Drawings.Server
{
  public partial class ModuleInitializer
  {
    
    public override void Initializing(Sungero.Domain.ModuleInitializingEventArgs e)
    {
      InitializationLogger.Debug("Инициализация модуля \"Архив чертежей\"");
      InitializationLogger.Debug("Создание типа \"Чертёж\"");
      Sungero.Docflow.PublicInitializationFunctions.Module.CreateDocumentType("Чертёж", Drawing.ClassTypeGuid,
                                                                              Sungero.Docflow.DocumentType.DocumentFlow.Inner, true);      
    }
  }

}
