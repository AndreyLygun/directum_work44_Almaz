using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Sungero.Parties;

namespace RRU.HelpDesk.Server
{
  public class ModuleFunctions
  {

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Public, Remote]
    public IInternalRequest CreateInternalRequest()
    {
      return InternalRequests.Create();
    }


    /// <summary>
    /// 
    /// </summary>
    /// 
    [Public, Remote]
    public IExternalRequest CreateExternalRequest()
    {
      return ExternalRequests.Create();
    }

    [Remote]
    public IRequest FindRequestByNumber(int? Number)
    {
      if (Number == null) return null;
      return Requests.GetAll( r => r.Number == Number).FirstOrDefault();
    }    
    
    /// <summary>
    /// 
    /// </summary>
    /// 
    [Public, Remote]
    public IQueryable<IExternalRequest> FindRequestsByCompany(ICompanyBase company)
    {
      return ExternalRequests.GetAll(r => r.Company.Equals(company));
    }    
    
  }
}