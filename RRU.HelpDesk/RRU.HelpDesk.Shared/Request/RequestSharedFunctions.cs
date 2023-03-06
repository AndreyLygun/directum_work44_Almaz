using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.HelpDesk.Request;

namespace RRU.HelpDesk.Shared
{
  partial class RequestFunctions
  {

    /// <summary>
    /// 
    /// </summary>       
    /// 
    public void FillName()
    {      
      var Name = Requests.Resources.NameFormat(_obj.RequestKind, _obj.Number, _obj.CreatedDate.ToString());
      _obj.Name = Name;      
    }

  }
}