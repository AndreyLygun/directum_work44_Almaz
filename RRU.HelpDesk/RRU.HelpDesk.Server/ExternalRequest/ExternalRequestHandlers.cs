using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.HelpDesk.ExternalRequest;

namespace RRU.HelpDesk
{
  partial class ExternalRequestContactPropertyFilteringServerHandler<T>
  {

    public virtual IQueryable<T> ContactFiltering(IQueryable<T> query, Sungero.Domain.PropertyFilteringEventArgs e)
    {
      if (_obj.Company!=null) {
        query = query.Where(c => Equals(c.Company, _obj.Company));
      }
      return query;
    }
  }



}