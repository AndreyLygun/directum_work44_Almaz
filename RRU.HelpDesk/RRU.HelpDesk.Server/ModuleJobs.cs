using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace RRU.HelpDesk.Server
{
  public class ModuleJobs
  {

    /// <summary>
    /// 
    /// </summary>
    public virtual void SendRequestCountInfo()
    {
      var performer = Sungero.Company.Employees.GetAll(e => e.Name.Contains("Лыгун")).FirstOrDefault();
      if (performer != null) {
        var count = Requests.GetAll().Count();
        var todaycount = Requests.GetAll(r => r.CreatedDate.Equals(Calendar.Today)).Count();
        var info = Sungero.Workflow.SimpleTasks.CreateWithNotices("Уведомление о количество сообщений", performer);
        info.ActiveText = RRU.HelpDesk.Resources.RequestCountInfoFormat(count, todaycount);
        info.Start();
      }
    }
  }
}