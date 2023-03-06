using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.HelpDesk.InternalRequest;

namespace RRU.HelpDesk.Client
{
  partial class InternalRequestActions
  {
    public virtual void ShowAuthorsRequestWrong(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      var requests = InternalRequests.GetAll(r => r.Author.Equals(_obj.Author));
      requests.Show();            
    }

    public virtual bool CanShowAuthorsRequestWrong(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

    public virtual void ShowAuthorsRequest(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      var requests = Functions.InternalRequest.Remote.GetInternalRequestsByAuthor(_obj);
      requests.Show();      
    }

    public virtual bool CanShowAuthorsRequest(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

  }

}