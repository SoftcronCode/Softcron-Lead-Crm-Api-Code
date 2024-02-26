using CommonClass.BO;
using CommonClass.ITF.BL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Setup.BL;
using Setup.BO;
using Setup.BO.Usermanagment;
using Setup.DTO;
using Setup.DTO.UserManagment;
using Setup.ITF;
using Setup.ITF.UserManagment;

namespace DSERP_API.Controllers.UserManagment
{
    [EnableCors("AllowAllHeaders")]
    [Route("ERP/Setup/[action]")]
    [ApiController]
    public class UserManagementAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAppVariables _appVariables;
        private readonly IUserManagment _UserManagment;
        public UserManagementAPIController(IUserManagment userManagment, IAppVariables appVariables, IConfiguration configuration)
        {
            _UserManagment = userManagment;
            _appVariables = appVariables;
            _configuration = configuration;
        }

        [HttpPost]

        public ResponseClass<UserManagmentResponse> ManageUser([FromBody] UserManagmentDTO ObjRequest)
        {
            return _UserManagment.ManageUser(ObjRequest);
        }
    }
}
