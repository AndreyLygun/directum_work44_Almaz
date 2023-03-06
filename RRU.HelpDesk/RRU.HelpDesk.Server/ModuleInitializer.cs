using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Sungero.Domain.Initialization;

namespace RRU.HelpDesk.Server
{
  public partial class ModuleInitializer
  {

    public override void Initializing(Sungero.Domain.ModuleInitializingEventArgs e)
    {
      InitializationLogger.Debug("Инициализация модуля \"Обращения\"");
      Sungero.Docflow.PublicInitializationFunctions.Module.CreateDocumentType(
        "Приложение к обращению", 
        AddendumRequest.ClassTypeGuid,
        Sungero.Docflow.DocumentType.DocumentFlow.Inner, 
        true);
      var allUsers = Roles.AllUsers;
      if (allUsers != null) {
        Requests.AccessRights.Grant(allUsers, DefaultAccessRightsTypes.Read);
        InitializationLogger.Debug("Выдача всем пользователям права на чтение справочника Обращение");
        RequestKinds.AccessRights.Grant(allUsers, DefaultAccessRightsTypes.Read);
        InitializationLogger.Debug("Выдача всем пользователям права на чтение справочника Типы обращений");
        AddendumRequests.AccessRights.Grant(allUsers, DefaultAccessRightsTypes.Create);        
        Requests.AccessRights.Grant(allUsers, DefaultAccessRightsTypes.Create);
        InitializationLogger.Debug("Выдача всем пользователям права на создание обращений и приложений к ним");
        Requests.AccessRights.Save();
        AddendumRequests.AccessRights.Save();
        
      }
    }
  }
}
