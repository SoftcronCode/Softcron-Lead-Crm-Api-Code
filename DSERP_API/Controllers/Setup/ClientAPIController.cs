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
    public class ClientAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAppVariables _appVariables;
        private readonly IClient _Client;
        public ClientAPIController(IClient Client, IAppVariables appVariables, IConfiguration configuration)
        {
            _Client = Client;
            _appVariables = appVariables;
            _configuration = configuration;
        }
        [HttpPost]
       
        public ResponseClass<AddClientResponse> AddClient([FromBody] AddClientDTO ObjRequest)
        {
            return _Client.AddClient(ObjRequest);
        }
              
        
    }
}
