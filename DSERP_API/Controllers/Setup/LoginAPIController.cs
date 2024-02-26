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
    public class LoginAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAppVariables _appVariables;
        private readonly ILogin _Login;
        public LoginAPIController(ILogin login, IAppVariables appVariables, IConfiguration configuration)
        {
            _Login = login;
            _appVariables = appVariables;
            _configuration = configuration;
        }
        [HttpPost]
       
        public ResponseClass<ERPLoginResponse> ERPLogin([FromBody] ERPLoginDTO ObjRequest)
        {
            return _Login.ERPLogin(ObjRequest);
        }
        [HttpPost]

        public ResponseClass<ERPClientLoginResponse> ClientERPLogin([FromBody] ClientERPLoginDTO ObjRequest)
        {
            return _Login.ClientERPLogin(ObjRequest);
        }


    }
}
