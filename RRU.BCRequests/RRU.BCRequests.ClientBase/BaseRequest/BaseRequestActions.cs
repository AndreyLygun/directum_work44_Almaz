using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.BCRequests.BaseRequest;

namespace RRU.BCRequests.Client
{
  partial class BaseRequestActions
  {
    public virtual void CreateDocAndSend(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      var template = Sungero.Docflow.DocumentTemplates.Get(15);
      using (var body = template.LastVersion.Body.Read())
      {
        var newVersion = _obj.CreateVersionFrom(body, template.AssociatedApplication.Extension);
        var internalEntity = (Sungero.Domain.Shared.IExtendedEntity)_obj;
        internalEntity.Params[Sungero.Content.Shared.ElectronicDocumentUtils.FromTemplateIdKey] = template.Id;
        //        _obj.Save();
        Dialogs.NotifyMessage("Версия создана");
      }
      
      //      var task = Sungero.Docflow.ApprovalTasks.Create();
      //      task.ApprovalRule = Sungero.Docflow.ApprovalRuleBases.GetAll(r => r.DocumentKinds.In  .Any(  .Equals(_obj.DocumentKind)).FirstOrDefault();
      //      if (task.ApprovalRule == null) {
      //        e.AddWarning("Не найдено правило согласования для документа вида " + _obj.DocumentKind);
      //        return;
      //      }
      //      task.DocumentGroup.OfficialDocuments.Add(_obj);
      //      task.Start();
    }

    public virtual bool CanCreateDocAndSend(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

  }

}