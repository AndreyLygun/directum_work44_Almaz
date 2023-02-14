using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.HelpDesk.ExternalRequest;

namespace RRU.HelpDesk
{
  partial class ExternalRequestSharedHandlers
  {

    public virtual void ContactChanged(RRU.HelpDesk.Shared.ExternalRequestContactChangedEventArgs e)
    {
      if (_obj.Contact!=null) {
        _obj.Company = _obj.Contact.Company;
      } else {
        _obj.Company = null;
      }
    }
  }


}