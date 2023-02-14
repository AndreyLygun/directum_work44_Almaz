using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.BankRecords.BankRecord;

namespace RRU.BankRecords.Client
{
  partial class BankRecordActions
  {
    public virtual void TestAction(Sungero.Domain.Client.ExecuteActionArgs e)
    {            
      var msg = _obj.Name;
      Dialogs.ShowMessage(msg);      
      _obj.Save();
    }

    public virtual bool CanTestAction(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

  }

}