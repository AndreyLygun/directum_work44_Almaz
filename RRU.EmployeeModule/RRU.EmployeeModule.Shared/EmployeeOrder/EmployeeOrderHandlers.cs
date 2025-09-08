using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.EmployeeModule.EmployeeOrder;

namespace RRU.EmployeeModule
{
  partial class EmployeeOrderSharedHandlers
  {

    public override void RegistrationNumberChanged(Sungero.Domain.Shared.StringPropertyChangedEventArgs e)
    {
      base.RegistrationNumberChanged(e);
      FillName();
    }

    public override void SubjectChanged(Sungero.Domain.Shared.StringPropertyChangedEventArgs e)
    {
      base.SubjectChanged(e);
      FillName();
    }

  }
}