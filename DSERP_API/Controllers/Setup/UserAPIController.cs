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
    public class UserAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAppVariables _appVariables;
        private readonly IUser _User;
        public UserAPIController(IUser User, IAppVariables appVariables, IConfiguration configuration)
        {
            _User = User;
            _appVariables = appVariables;
            _configuration = configuration;
        }
        [HttpPost]
     
        public ResponseClass<AddClientUserResponse> AddClientUser([FromBody] AddClientUserDTO ObjRequest)
        {
            return _User.AddClientUser(ObjRequest);
        }

        [HttpPost]
        public ResponseClass<ChangePasswordResponse> ChangePassword(ChangePasswordDTO ObjMaster)
        {
            return _User.ChangePassword(ObjMaster);
        }


    }
}
