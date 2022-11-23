using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Company;
using Sungero.Core;
using Sungero.CoreEntities;
using Sungero.Docflow;

namespace RRU.EmployeeModule
{
  partial class ApprovalSheetReportWoDateServerHandlers
  {
    public override void AfterExecute(Sungero.Reporting.Server.AfterExecuteEventArgs e)
    {
      Sungero.Docflow.PublicFunctions.Module.DeleteReportData(Constants.ApprovalSheetReportWoDate.SourceTableName, ApprovalSheetReportWoDate.ReportSessionId);
    }

    public override void BeforeExecute(Sungero.Reporting.Server.BeforeExecuteEventArgs e)
    {
      ApprovalSheetReportWoDate.ReportSessionId = Guid.NewGuid().ToString();
      Sungero.Docflow.Server.ApprovalTaskFunctions.UpdateApprovalSheetReportTable(ApprovalSheetReportWoDate.Document, ApprovalSheetReportWoDate.ReportSessionId);
      //=Functions.ApprovalTask.UpdateApprovalSheetReportTable(ApprovalSheetReportWoDate.Document, ApprovalSheetReportWoDate.ReportSessionId);
      ApprovalSheetReportWoDate.HasRespEmployee = false;
      
      var document = ApprovalSheetReportWoDate.Document;
      if (document == null)
        return;
      
      // Наименование отчета.
      ApprovalSheetReportWoDate.DocumentName = Sungero.Docflow.PublicFunctions.Module.FormatDocumentNameForReport(document, false);
      
      // НОР.
      var ourOrg = document.BusinessUnit;
      if (ourOrg != null)
        ApprovalSheetReportWoDate.OurOrgName = ourOrg.Name;
      
      // Дата отчета.
      ApprovalSheetReportWoDate.CurrentDate = Calendar.Now;
      
      // Ответственный.
      var responsibleEmployee =  Employees.As(document.Author); //Sungero.Docflow.Shared.OfficialDocumentFunctions.GetDocumentResponsibleEmployee(document);
      
      if (responsibleEmployee != null &&
          responsibleEmployee.IsSystem != true)
      {
        var respEmployee = string.Format("{0}: {1}", Reports.Resources.ApprovalSheetReportWoDate.ResponsibleEmployee,
                                         responsibleEmployee.Person.ShortName);
        
        if (responsibleEmployee.JobTitle != null)
          respEmployee = string.Format("{0} ({1})", respEmployee, responsibleEmployee.JobTitle.DisplayValue.Trim());
        
        ApprovalSheetReportWoDate.RespEmployee = respEmployee;
        
        ApprovalSheetReportWoDate.HasRespEmployee = true;
      }
      
      // Распечатал.
      if (Employees.Current == null)
      {
        ApprovalSheetReportWoDate.Clerk = Users.Current.Name;
      }
      else
      {
        var clerkPerson = Employees.Current.Person;
        ApprovalSheetReportWoDate.Clerk = clerkPerson.ShortName;
      }
    }
  }
}