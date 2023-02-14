using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.BankRecords.BankRecord;

namespace RRU.BankRecords.Shared
{
  partial class BankRecordFunctions
  {
    public override void SetRequiredProperties()
    {
      base.SetRequiredProperties();
      _obj.State.Properties.Counterparty.IsRequired = false;
    }    
    
  }
}