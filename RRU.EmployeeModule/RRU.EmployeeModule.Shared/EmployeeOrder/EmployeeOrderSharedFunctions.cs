using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.EmployeeModule.EmployeeOrder;

namespace RRU.EmployeeModule.Shared
{
  partial class EmployeeOrderFunctions
  {
    public override void FillName() {
      _obj.Name = "Приказ " + _obj.RegistrationNumber + " " + _obj.Subject;
    }
    
  }
}