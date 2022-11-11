using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.EmployeeModule.EmployeeOrder;

namespace RRU.EmployeeModule
{
  partial class EmployeeOrderServerHandlers
  {

    public override void Created(Sungero.Domain.CreatedEventArgs e)
    {
      base.Created(e);
      _obj.Department = Sungero.Company.Departments.Null;
    }
  }

}