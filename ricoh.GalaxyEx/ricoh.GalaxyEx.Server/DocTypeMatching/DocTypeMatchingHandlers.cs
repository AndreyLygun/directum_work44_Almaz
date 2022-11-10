using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using ricoh.GalaxyEx.DocTypeMatching;

namespace ricoh.GalaxyEx
{
  partial class DocTypeMatchingDocKindPropertyFilteringServerHandler<T>
  {

    public virtual IQueryable<T> DocKindFiltering(IQueryable<T> query, Sungero.Domain.PropertyFilteringEventArgs e)
    {
      query = query.Where(d => d.DocumentType.Equals(_obj.DocType));
      return query;
    }
  }

}