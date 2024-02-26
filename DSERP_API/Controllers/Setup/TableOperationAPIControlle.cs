using CommonClass.BO;
using CommonClass.Filter;
using CommonClass.ITF.BL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Setup.BO;
using Setup.DTO;
using Setup.ITF;
using System.Collections.Generic;

namespace POS_WMS.Controllers.Setup
{
   
    [EnableCors("AllowAllHeaders")]
    [Route("ERP/Setup/[action]")]
    [ApiController]
    public class TableOperationAPIControlle : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAppVariables _appVariables;
        private readonly ITableOperation _ITableOperation;
        public TableOperationAPIControlle(ITableOperation iTableOperation, IAppVariables appVariables, IConfiguration configuration)
        {
            _ITableOperation = iTableOperation;
            _appVariables = appVariables;
            _configuration = configuration;
        }
       

        [HttpPost]
        public ResponseClass<AddClientResponse> Execute([FromBody] TableOperationDTO ObjRequest)
        {
            return _ITableOperation.Execute(ObjRequest);
        }

        [HttpPost]
        public ResponseClass<TableOperationResponse> OLDExecute([FromBody] TableOperationDTO ObjRequest)
        {
            return _ITableOperation.OLDExecute(ObjRequest);
        }

    }
}
