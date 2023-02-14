using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.HelpDesk.ExternalRequest;

namespace RRU.HelpDesk
{
  partial class ExternalRequestClientHandlers
  {

    public virtual void CompanyValueInput(RRU.HelpDesk.Client.ExternalRequestCompanyValueInputEventArgs e)
    {
      if (!Equals(e.NewValue, e.OldValue)) {
        _obj.Contact = null;
      }      
    }
  }

}