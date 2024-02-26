using CommonClass.BO;
using CommonClass.ITF.BL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Setup.BO.Customer;
using Setup.DTO.Customer;
using Setup.ITF.Customer;

namespace DSERP_API.Controllers.Customer
{
    [EnableCors("AllowAllHeaders")]
    [Route("ERP/Customer/[action]")]
    [ApiController]
    public class RemindersAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAppVariables _appVariables;
        private readonly IReminders _reminders;
        public RemindersAPIController(IReminders reminders, IAppVariables appVariables, IConfiguration configuration)
        {
            _reminders = reminders;
            _appVariables = appVariables;
            _configuration = configuration;
        }

        [HttpPost]
        public ResponseClass<RemindersResponse> BirthdayAndValidityReminders([FromBody] RemindersDTO ObjRequest)
        {
            return _reminders.BirthdayAndValidityReminders(ObjRequest);
        }

    }
}
