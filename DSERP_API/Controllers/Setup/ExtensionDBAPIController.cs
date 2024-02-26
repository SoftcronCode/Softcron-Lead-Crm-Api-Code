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

namespace POS_WMS.Controllers.Setup
{
    [EnableCors("AllowAllHeaders")]
    [Route("ERP/Setup/[action]")]
    [ApiController]
    public class ExtensionDBAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAppVariables _appVariables;
        private readonly IExtensionDBMaster _ExtensionDB;
        public ExtensionDBAPIController(IExtensionDBMaster ExtensionDB, IAppVariables appVariables, IConfiguration configuration)
        {
            _ExtensionDB = ExtensionDB;
            _appVariables = appVariables;
            _configuration = configuration;
        }
        [HttpPost]
      
        public ResponseClass<TableResponse> AddTable([FromBody] AddTableDTO ObjRequest)
        {
            return _ExtensionDB.AddTable(ObjRequest);
        }
        [HttpPost]
        public ResponseClass<ColumnResponse> AddColumn([FromBody] AddColumnDTO ObjRequest)
        {
            return _ExtensionDB.AddColumn(ObjRequest);
        }
              
        
    }
}
