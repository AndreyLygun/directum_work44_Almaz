using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace RRU.EmployeeModule.Client
{
  public class ModuleFunctions
  {

    /// <summary>
    /// 
    /// </summary>
    public virtual void RunApprovalSheetReportWoDates(Sungero.Docflow.IOfficialDocument document)
    {

//      var hasSignatures =  Functions.OfficialDocument.Remote.HasSignatureForApprovalSheetReport(document);
//      if (!hasSignatures)
//      {
//        Dialogs.NotifyMessage(EmployeeOrders.Resources.DocumentIsNotSigned);
//        return;
//      }
      
      var report = Reports.GetApprovalSheetReportWoDate();
      report.Document = document;
      report.Open();
    }

  }
}