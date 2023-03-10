using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.HelpDesk.Request;

namespace RRU.HelpDesk
{
  partial class RequestFilteringServerHandler<T>
  {

    public override IQueryable<T> Filtering(IQueryable<T> query, Sungero.Domain.FilteringEventArgs e)
    {
      if (_filter == null) return query;
      if (_filter.FlagInWork || _filter.FlagInControl || _filter.FlagClosed) {
        query = query.Where(r => (_filter.FlagInWork && r.LifeCycle.Equals(Request.LifeCycle.InWork)) ||
                                 (_filter.FlagInControl && r.LifeCycle.Equals(Request.LifeCycle.InControl)) ||
                                 (_filter.FlagClosed && r.LifeCycle.Equals(Request.LifeCycle.Closed)));
      }
      if (_filter.TypeExternal)
        query = query.Where(r => ExternalRequests.Is(r));
      if (_filter.TypeInternal)
        query = query.Where(r => InternalRequests.Is(r));       
      if (_filter.ResponsibleMe) 
        query = query.Where(r => r.Responsible.Equals(Sungero.Company.Employees.Current));
      if (_filter.ResponsibleSelected && _filter.ResponsibleEmployee != Sungero.Company.Employees.Null) 
        query = query.Where(r => r.Responsible.Equals(_filter.ResponsibleEmployee));
      return query;
    }
  }

  partial class RequestServerHandlers
  {

    public override void BeforeSave(Sungero.Domain.BeforeSaveEventArgs e)
    {
      if (_obj.LifeCycle.Equals(LifeCycle.Closed)) {
        if (string.IsNullOrWhiteSpace(_obj.Result)) {
          e.AddError(_obj.Info.Properties.Result , "Перед закрытием обращения заполните результат обработки");
          return;
        }
        _obj.ClosedDate = Calendar.Today;
        _obj.State.IsEnabled = false;
      }
    }

    public override void Created(Sungero.Domain.CreatedEventArgs e)
    {
      _obj.Number = _obj.Id;
      _obj.Responsible = Sungero.Company.Employees.Current;
      _obj.LifeCycle = LifeCycle.InWork;
      _obj.CreatedDate = Calendar.Today;
      _obj.Name = "Тема обращения будет заполнена автоматически";
    }
  }

}