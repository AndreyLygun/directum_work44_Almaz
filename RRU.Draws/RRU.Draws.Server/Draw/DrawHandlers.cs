using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.Draws.Draw;

namespace RRU.Draws
{
  partial class DrawFilteringServerHandler<T>
  {

    public virtual IQueryable<Sungero.Parties.ICounterparty> NavigationFiltering(IQueryable<Sungero.Parties.ICounterparty> query, Sungero.Domain.FilteringEventArgs e)
    {
      return query;
    }
  }

  partial class DrawServerHandlers
  {

    public override void BeforeSave(Sungero.Domain.BeforeSaveEventArgs e)
    {

    }
  }

}