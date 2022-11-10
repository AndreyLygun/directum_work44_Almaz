using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.HelpDesk.Request;

namespace RRU.HelpDesk
{
  partial class RequestClientHandlers
  {

    public override void Showing(Sungero.Presentation.FormShowingEventArgs e)
    {
      if (_obj.LifeCycle == Request.LifeCycle.Closed) {
        _obj.State.IsEnabled = true;
      }
      
    }

  }
}