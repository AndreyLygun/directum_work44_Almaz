using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using ricoh.GalaxyEx.OrderEmployee;

namespace ricoh.GalaxyEx
{
  partial class OrderEmployeeServerHandlers
  {

    public override void Created(Sungero.Domain.CreatedEventArgs e)
    {
      base.Created(e);
      _obj.Department = Sungero.Company.Departments.Null;
    }
  }

}