using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace RRU.BCRequests.Client
{
  public class ModuleFunctions
  {

    /// <summary>
    /// 
    /// </summary>
    public virtual void CreateOneTimeMovingRequest()
    {
      var r = OneTimeMovingRequests.Create();
      r.Show();
    }

  }
}