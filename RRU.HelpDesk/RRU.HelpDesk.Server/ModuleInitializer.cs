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
      var allUsers = Roles.AllUsers;
      if (allUsers != null) {
        Requests.AccessRights.Grant(allUsers, DefaultAccessRightsTypes.Read);
        InitializationLogger.Debug("Выдача всем пользователям права на чтение справочника Обращение");
        RequestKinds.AccessRights.Grant(allUsers, DefaultAccessRightsTypes.Read);
        InitializationLogger.Debug("Выдача всем пользователям права на чтение справочника Типы обращений");
      }
    }
  }
}
