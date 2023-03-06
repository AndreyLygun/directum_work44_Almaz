using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.HelpDesk.Request;

namespace RRU.HelpDesk
{
  partial class RequestJobsSharedCollectionHandlers
  {

    public virtual void JobsAdded(Sungero.Domain.Shared.CollectionPropertyAddedEventArgs e)
    {
      _added.Date = Calendar.Today;
      _added.Employee = Sungero.Company.Employees.Current;
    }
  }

  partial class RequestSharedHandlers
  {

    public virtual void CreatedDateChanged(Sungero.Domain.Shared.DateTimePropertyChangedEventArgs e)
    {
      Functions.Request.FillName(_obj);
    }

    public virtual void NumberChanged(Sungero.Domain.Shared.IntegerPropertyChangedEventArgs e)
    {
      Functions.Request.FillName(_obj);
    }

    public virtual void RequestKindChanged(RRU.HelpDesk.Shared.RequestRequestKindChangedEventArgs e)
    {
      Functions.Request.FillName(_obj);
    }

    public virtual void JobsChanged(Sungero.Domain.Shared.CollectionPropertyChangedEventArgs e)
    {
      _obj.TotalHours = _obj.Jobs.Sum(j => j.Hours);
    }

  }
}