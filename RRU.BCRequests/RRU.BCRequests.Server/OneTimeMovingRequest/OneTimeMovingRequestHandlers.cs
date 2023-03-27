using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.BCRequests.OneTimeMovingRequest;

namespace RRU.BCRequests
{
  partial class OneTimeMovingRequestServerHandlers
  {

    public override void Created(Sungero.Domain.CreatedEventArgs e)
    {
      base.Created(e);
      _obj.ValidDate = Calendar.NextWorkingDay(Calendar.Today);
    }
  }

}