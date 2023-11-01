using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.Drawings.DrawingSize;

namespace RRU.Drawings.Client
{
  partial class DrawingSizeActions
  {
    public virtual void RescanRequest(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      var dlg = Dialogs.CreateInputDialog("Запрос на пересканирование");
      dlg.AddMultilineString("Опишите дефект на скане", true);
      dlg.Show();
      Dialogs.NotifyMessage("Запрос на пересканирование отправлен");                
    }

    public virtual bool CanRescanRequest(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

    public virtual void CopyRequest(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      Dialogs.NotifyMessage("Запрос на изготовление бумажной копии отправлен");
    }

    public virtual bool CanCopyRequest(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

    public virtual void PaperRequest(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      Dialogs.NotifyMessage("Запрос на предоставление бумажного оригинала отправлен");
    }

    public virtual bool CanPaperRequest(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

  }

}