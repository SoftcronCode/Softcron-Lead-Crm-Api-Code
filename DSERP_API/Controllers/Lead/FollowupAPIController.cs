using CommonClass.BO;
using CommonClass.ITF.BL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Setup.BO.Lead;
using Setup.DTO.Lead;
using Setup.ITF.Lead;

namespace DSERP_API.Controllers.Lead
{
    [EnableCors("AllowAllHeaders")]
    [Route("ERP/Lead/[action]")]
    [ApiController]

    public class FollowupAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAppVariables _appVariables;
        private readonly IFollowup _followup;
        public FollowupAPIController(IFollowup followup, IAppVariables appVariables, IConfiguration configuration)
        {
            _followup = followup;
            _appVariables = appVariables;
            _configuration = configuration;
        }

        [HttpPost]
        public ResponseClass<FollowupPopupResponse> FollowupReminderNotification([FromBody] FollowupPopupDTO ObjRequest)
        {
            return _followup.FollowupReminderNotification(ObjRequest);
        }

        [HttpPost]
        public ResponseClass<FollowupNotificationResponse> Followup_notification([FromBody] FollowupNotificationDTO ObjRequest)
        {
            return _followup.Followup_notification(ObjRequest);
        }
    }
}
