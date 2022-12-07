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
        new string[] {"4", "Внутреннее совместительство, совмещение"},
        new string[] {"5", "Перемещение"},
        new string[] {"6", "Оформление очередного отпуска"},
        new string[] {"7", "Закрытие совмещения (снятие надбавки за совмещение)"},
        new string[] {"8", "Увольнение"},
        new string[] {"9", "Назначение персональных надбавок"},
        new string[] {"10", "Снятие персональных надбавок"},
        new string[] {"11", "Частичное снятие надбавки"},
        new string[] {"12", "Изменение надбавки"},
        new string[] {"13", "Назначение персональных надбавок с днями и часами"},
        new string[] {"14", "Досрочное завершение отпуска по уходу за ребенком"},
        new string[] {"20", "Назначение единовременных выплат"},
        new string[] {"21", "Назначение единовременных выплат с днями и часами"},
        new string[] {"30", "Награждение/поощрение, премирование"},
        new string[] {"31", "Взыскание"},
        new string[] {"32", "Снятие взыскания"},
        new string[] {"35", "Направление на обучение"},
        new string[] {"36", "Перенос сроков обучения"},
        new string[] {"37", "Отмена направления на обучение"},
        new string[] {"38", "Изменение план-графика отпусков"},
        new string[] {"39", "Перенос ежегодного оплачиваемого отпуска"},
        new string[] {"40", "Отзыв из отпуска / Перенос отпуска"},
        new string[] {"41", "Предоставление отпуска"},
        new string[] {"42", "Перенос отпуска"},
        new string[] {"43", "Проведение аттестации и создание аттестационной комиссии"},
        new string[] {"44", "Досрочное завершение временного замещения"},
        new string[] {"45", "Изменение характера работы"},
        new string[] {"46", "Изменение условий труда"},
        new string[] {"47", "Изменение дополнительной характеристики"},
        new string[] {"50", "Изменение оклада"},
        new string[] {"60", "Изменение режима работы"},
        new string[] {"61", "Изменение условий работы"},
        new string[] {"62", "Изменение разряда (категории)"},
        new string[] {"65", "Заключение/продление контракта"},
        new string[] {"70", "Временное замещение с освобождением от своих обязанностей"},
        new string[] {"71", "Дни отдыха и оплачиваемые неявки"},
        new string[] {"72", "Привлечение сотрудника к сверхурочной работе"},
        new string[] {"90", "Работа в выходные и праздничные"},
        new string[] {"91", "Назначение почасовых табельных отклонений"},
        new string[] {"92", "Отражение работы во время отпуска по уходу за ребенком"},
        new string[] {"93", "Отмена почасовых табельных отклонений"},
        new string[] {"95", "Конкурс на занятие вакантной ставки"},
        new string[] {"99", "Отмена распорядительного действия"},
        new string[] {"100", "Прочие действия"},
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
