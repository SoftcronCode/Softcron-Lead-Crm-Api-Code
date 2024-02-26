using CommonClass.BO;
using CommonClass.ITF.BL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Setup.BO;
using Setup.BO.Login;
using Setup.DTO;
using Setup.DTO.Login;
using Setup.ITF;
using Setup.ITF.Login;

namespace DSERP_API.Controllers.Login
{
    [EnableCors("AllowAllHeaders")]
    [Route("ERP/Login/[action]")]
    [ApiController]
    public class ResetPasswordAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAppVariables _appVariables;
        private readonly IResetPassword _resetPassword;
        public ResetPasswordAPIController(IResetPassword resetPassword, IAppVariables appVariables, IConfiguration configuration)
        {
            _resetPassword = resetPassword;
            _appVariables = appVariables;
            _configuration = configuration;
        }


        [HttpPost]
        public ResponseClass<ResetPasswordResponse> SendPasswordResetEmail([FromBody] ResetPasswordDTO ObjRequest)
        {
            return _resetPassword.SendPasswordResetEmail(ObjRequest);
        }
    }
}
