using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace RRU.HelpDesk.Client
{
  public class ModuleFunctions
  {

    /// <summary>
    /// 
    /// </summary>
    public virtual void CreateInternalRequest()
    {
      Functions.Module.Remote.CreateInternalRequest().Show();
    }
    
    
    /// <summary>
    /// 
    /// </summary>
    public virtual void CreateExternalRequest()
    {
      Functions.Module.Remote.CreateExternalRequest().Show();
    }
    

    /// <summary>
    /// 
    /// </summary>
    public virtual void FindRequestByNumber()
    {
      var d = Dialogs.CreateInputDialog("Поиск обращения по номеру");
      var NumInput = d.AddInteger("Номер обращения", true);
      if (d.Show() != DialogButtons.Ok) return;
      var r = Functions.Module.Remote.FindRequestByNumber(NumInput.Value);
      if (r != null) {
        r.Show();
      } else {
        Dialogs.NotifyMessage($"Не найдено обращения с номером {NumInput.Value}");
      }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public virtual void FindRequestsByCompany()
    {
      var d = Dialogs.CreateInputDialog("Поиск внешних обращений по компании");
      var company = d.AddSelect("Компания", true, Sungero.Parties.CompanyBases.Null);
      if (d.Show()== DialogButtons.Cancel) return;
      Functions.Module.Remote.FindRequestsByCompany(company.Value).Show();
    }
    
  }
}