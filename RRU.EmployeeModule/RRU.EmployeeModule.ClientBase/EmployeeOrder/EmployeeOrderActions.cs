using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.EmployeeModule.EmployeeOrder;

namespace RRU.EmployeeModule.Client
{
  partial class EmployeeOrderActions
  {
    public virtual void ApprovalFormWoDate(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      Functions.Module.RunApprovalSheetReportWoDates(_obj);
    }

    public virtual bool CanApprovalFormWoDate(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

  }

}