using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.BCRequests.OneTimeMovingRequest;

namespace RRU.BCRequests
{
  partial class OneTimeMovingRequestClientHandlers
  {

    public override void Showing(Sungero.Presentation.FormShowingEventArgs e)
    {
      base.Showing(e);
      _obj.State.Properties["Carrier"].IsVisible = false;
    }

  }
}