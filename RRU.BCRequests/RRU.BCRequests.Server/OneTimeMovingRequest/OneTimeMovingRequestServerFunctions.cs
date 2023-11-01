using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using RRU.BCRequests.OneTimeMovingRequest;

namespace RRU.BCRequests.Server
{
  partial class OneTimeMovingRequestFunctions
  {

    /// <summary>
    /// 
    /// </summary>       
    /// 
    [Converter("Cars")]
    public string CarsCollectionToString(IOneTimeMovingRequest doc)
    {
      var s = "Табличка 2"+'\n';
      foreach(var row in doc.Cars) {
        
        s = s + row.Model + '	' + '\t' + row.Number + '\u0009' + ';' + '\n';
      }
      return s;
    }

  }
}