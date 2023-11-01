using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.Drawings.Drawing;

namespace RRU.Drawings.Client
{
  partial class DrawingActions
  {
    public virtual void DrawRescanRequest(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      var d = Dialogs.CreateInputDialog("Запрос на пересканирование");
      d.AddMultilineString("Укажите дефект скана, который нужно исправить", true);
      d.Show();
      Dialogs.NotifyMessage("Запрос на пересканирование отправлен");
    }

    public virtual bool CanDrawRescanRequest(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

    public virtual void DrawCopyRequest(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      Dialogs.NotifyMessage("Запрос на изготовление бумажной копии отправлен");
    }

    public virtual bool CanDrawCopyRequest(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

    public virtual void DrawPaperRequest(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      Dialogs.NotifyMessage("Запрос на выдачу бумажного экземпляра отправлен");
    }

    public virtual bool CanDrawPaperRequest(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

  }

}