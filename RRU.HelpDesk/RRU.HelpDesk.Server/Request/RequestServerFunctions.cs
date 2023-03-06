using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.HelpDesk.Request;

namespace RRU.HelpDesk.Server
{
  partial class RequestFunctions
  {

    /// <summary>
    /// 
    /// </summary>       
    [Remote]
    public IQueryable<IAddendumRequest> GetAddendums()
    {
      return AddendumRequests.GetAll(d => d.Request.Equals(_obj));
    }

    /// <summary>
    /// 
    /// </summary>       
    /// 
    [Remote]
    public IAddendumRequest CreateAddendum()
    {
      var doc = AddendumRequests.Create();
      doc.Request = _obj;
      doc.Name = AddendumRequests.Resources.NameFormat(_obj.Number);
      doc.AccessRights.Grant(_obj.Responsible, DefaultAccessRightsTypes.Read);
      return doc;
    }

  }
}