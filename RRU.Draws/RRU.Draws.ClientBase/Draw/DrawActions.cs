using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.Draws.Draw;

namespace RRU.Draws.Client
{
  partial class DrawActions
  {
    public virtual void RequestOriginal(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      Dialogs.ShowMessage("Здесь будет запрос на выдачу оригинала.");
    }

    public virtual bool CanRequestOriginal(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

    public virtual void RequestRescan(Sungero.Domain.Client.ExecuteActionArgs e)
    {
        Dialogs.ShowMessage("Запрос на пересканирование.");
//      var performer = Sungero.Company.Employees.Get(16);
//      var r = Sungero.Workflow.SimpleTasks.Create("Прошу выполнить повторное сканирование бумажного чертежа", performer);
//      r.Attachments.Add(_obj);
//      r.ActiveText = "Укажите, почему требуется пересканирование (например, характер и места расположения дефектов).";
//      r.Show();
    }

    public virtual bool CanRequestRescan(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

    public virtual void RequestChange(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      Dialogs.ShowMessage("Запрос на исправление ошибки");
    }

    public virtual bool CanRequestChange(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

  }

}