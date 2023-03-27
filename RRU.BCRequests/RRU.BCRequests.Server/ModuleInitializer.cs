using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Sungero.Domain.Initialization;

namespace RRU.BCRequests.Server
{
  public partial class ModuleInitializer
  {
    public override void Initializing(Sungero.Domain.ModuleInitializingEventArgs e)
    {
      InitializationLogger.Debug("Инициализация модуля \"Заявка по бизнес-центру\"");
      InitializationLogger.Debug("Создание типа \"Заявка на разовый ввоз-вывоз\"");
      Sungero.Docflow.PublicInitializationFunctions.Module.CreateDocumentType("Заявка на разовый ввоз-вывоз", OneTimeMovingRequest.ClassTypeGuid,
                                                                              Sungero.Docflow.DocumentType.DocumentFlow.Incoming, true);      
    }
  }
}
