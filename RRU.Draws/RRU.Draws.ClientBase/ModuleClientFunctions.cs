using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace RRU.Draws.Client
{
  public class ModuleFunctions
  {

    /// <summary>
    /// 
    /// </summary>
    public virtual void AddDraw()
    {
      var d = RRU.Draws.Draws.Create();
      d.Show();
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void RequestScan()
    {
      Dialogs.ShowMessage("Scan Request");
    }

  }
}