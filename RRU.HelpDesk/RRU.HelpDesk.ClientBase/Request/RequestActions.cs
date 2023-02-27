using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.HelpDesk.Request;

namespace RRU.HelpDesk.Client
{
  partial class RequestActions
  {
    public virtual void Open(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      _obj.LifeCycle = LifeCycle.InWork;
      _obj.ClosedDate = null;
      _obj.State.IsEnabled = true;
    }

    public virtual bool CanOpen(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return _obj.LifeCycle == LifeCycle.Closed;
    }

  }

}