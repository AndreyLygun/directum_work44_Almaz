using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.BankRecords.BankRecord;

namespace RRU.BankRecords
{
  partial class BankRecordClientHandlers
  {

    public override void Refresh(Sungero.Presentation.FormRefreshEventArgs e)
    {
      base.Refresh(e);
      var n = _obj.Name;
      var n1 = _obj.State.Properties.Name.OriginalValue;
      var n2 = _obj.State.Properties.Name.PreviousValue;
      Dialogs.NotifyMessage(n + " -# " + n1 + " -#- " + n2);      
    }

    public override void Showing(Sungero.Presentation.FormShowingEventArgs e)
    {
      

    }

  }
}