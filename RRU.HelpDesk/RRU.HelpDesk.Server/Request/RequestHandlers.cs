using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.HelpDesk.Request;

namespace RRU.HelpDesk
{
  partial class RequestServerHandlers
  {

    public override void BeforeSave(Sungero.Domain.BeforeSaveEventArgs e)
    {
      if ((_obj.LifeCycle == Request.LifeCycle.Closed) && string.IsNullOrWhiteSpace(_obj.Result)) {
        e.AddError(_obj.Info.Properties.Result, "Перед закрытием обращения заполните поле \"Результат\"");
        return;
      }
    }

    public override void Created(Sungero.Domain.CreatedEventArgs e)
    {
      _obj.Number = _obj.Id;
      _obj.Responsible = Sungero.Company.Employees.Current;
      _obj.LifeCycle = Request.LifeCycle.NewRequest;      
      _obj.CreatedDate = Calendar.Now;
      
    }
  }

}