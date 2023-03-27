using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.BCRequests.BaseRequest;

namespace RRU.BCRequests
{
  partial class BaseRequestClientHandlers
  {

    public override void Showing(Sungero.Presentation.FormShowingEventArgs e)
    {
      base.Showing(e);
      _obj.State.Properties.BusinessUnit.IsEnabled = false;
    }

  }
}