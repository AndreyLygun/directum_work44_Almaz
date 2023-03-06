using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.HelpDesk.InternalRequest;

namespace RRU.HelpDesk.Server
{
  partial class InternalRequestFunctions
  {

    /// <summary>
    /// 
    /// </summary>       
    /// 
    [Remote(IsPure=true)]
    public IQueryable<IInternalRequest> GetInternalRequestsByAuthor()
    {
      return InternalRequests.GetAll(r => r.Author.Equals(_obj.Author));
    }

  }
}