using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Sungero.Domain.Initialization;
using Sungero.Docflow;
using Sungero.Docflow.DocumentKind;

namespace RRU.EmployeeModule.Server
{
  public partial class ModuleInitializer
  {

    public override void Initializing(Sungero.Domain.ModuleInitializingEventArgs e)
    {
      CreateDocumentType();
      CreateDocumentKinds();
      GrantAccessRihts();
    }
    
    public void CreateDocumentType()
    {
      Sungero.Docflow.PublicInitializationFunctions.Module.CreateDocumentType("Приказы по персоналу", EmployeeOrder.ClassTypeGuid,
                                                                              Sungero.Docflow.DocumentType.DocumentFlow.Inner, true);
    }
    
    public void CreateDocumentKinds() {
      var kinds = new string[][] {
        new string[] {"5",	"О переводе работника на другую работу"},
        new string[] {"9",	"О назначении персональных надбавок работнику"},
        new string[] {"50",	"Об изменении окладов работнику"},
        new string[] {"9",	"Об изменении персональных надбавок работнику"},
        new string[] {"60",	"Об изменении режима работы работнику"},
        new string[] {"70",	"О замещении другого сотрудника с освобождением от обязанностей"},
        new string[] {"3",	"О замещении другого сотрудника без освобождения от обязанностей"},
        new string[] {"7",	"О досрочном завершении совмещения  профессий (должностей)"},
        new string[] {"44",	"Досрочное завершение временного замещения"},
      };
      foreach (string[] kind in kinds) {
        string id = kind[0];
        string shortName = kind[1];
        if (Sungero.Docflow.DocumentKinds.GetAll(el => el.ShortName.Equals(shortName)).FirstOrDefault() != null) continue;
        CreateDocumentKind(id, shortName);
      }
    }
    
    /// <summary>
    /// Создаёт вид документа. Используется данная функция вместо Sungero.DocFlow.CreateDocumentKind() , чтобы присвоить значение полю GalaxyID в перегруженном справочнике
    /// </summary>
    /// <param name="GalaxyId"></param>
    /// <param name="Name"></param>
    
    public void CreateDocumentKind(string GalaxyId, string shortName) {
      //    public static void CreateDocumentKind(string name, string shortName, Enumeration numerationType, Enumeration direction,
      //                                          bool autoFormattedName, bool autoNumerable, Guid typeGuid, Domain.Shared.IActionInfo[] actions,
      //                                          bool projectAccounting, bool grantRightsToProject, Guid entityId, bool isDefault)
      var entityId = Guid.NewGuid();
      var externalLink = Sungero.Docflow.PublicFunctions.Module.GetExternalLink(Sungero.Docflow.Server.DocumentKind.ClassTypeGuid, entityId);
      
      if (externalLink != null) return;
      
      var type = EmployeeOrder.ClassTypeGuid.ToString();
      var documentType = Sungero.Docflow.DocumentTypes.GetAll(t => t.DocumentTypeGuid == type).FirstOrDefault();
      
      var name = "Приказ " + shortName.ToLower();
      InitializationLogger.DebugFormat("Init: Create document kind {0}", name);
      var documentKind = RRU.Almaz.DocumentKinds.Create();
      documentKind.Name = name;
      documentKind.ShortName = shortName;
      documentKind.DocumentFlow =  Sungero.Docflow.DocumentKind.DocumentFlow.Inner;
      documentKind.NumberingType = Sungero.Docflow.DocumentKind.NumberingType.Registrable;
      documentKind.GenerateDocumentName = true;
      documentKind.AutoNumbering = true;
      documentKind.ProjectsAccounting = false;
      documentKind.GrantRightsToProject = false;
      documentKind.DocumentType = documentType;
      documentKind.IsDefault = false;
      documentKind.GalaxyKindRRU = GalaxyId;

      var actions = new Sungero.Domain.Shared.IActionInfo[] {
        Sungero.Docflow.OfficialDocuments.Info.Actions.SendForApproval,
        Sungero.Docflow.OfficialDocuments.Info.Actions.SendForFreeApproval,
        Sungero.Docflow.OfficialDocuments.Info.Actions.SendForReview };

      documentKind.AvailableActions.Clear();
      foreach (var action in actions)
        documentKind.AvailableActions.AddNew().Action =  Sungero.Docflow.Shared.ModuleFunctions.GetSendAction(action);

      documentKind.Save();
      
      Sungero.Docflow.PublicFunctions.Module.CreateExternalLink(documentKind, entityId);
    }
    public void GrantAccessRihts() {
      var users = Roles.AllUsers;
      EmployeeOrders.AccessRights.Grant(users, DefaultAccessRightsTypes.Create);
    }
    
  }  
}
