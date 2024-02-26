using CommonClass.BO;
using CommonClass.ITF.BL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Setup.ITF.Lead;
using Setup.BO.Lead;
using Setup.DTO.Lead;

namespace DSERP_API.Controllers.Lead
{
    [EnableCors("AllowAllHeaders")]
    [Route("ERP/Lead/[action]")]
    [ApiController]
    public class LeadReportingAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAppVariables _appVariables;
        private readonly ILeadReport _leadReport;
        public LeadReportingAPIController(ILeadReport leadReport, IAppVariables appVariables, IConfiguration configuration)
        {
            _leadReport = leadReport;
            _appVariables = appVariables;
            _configuration = configuration;
        }

        [HttpPost]
        public ResponseClass<LeadReportResponse> LeadReport([FromBody] LeadReportDTO ObjRequest)
        {
            return _leadReport.LeadReport(ObjRequest);
        }
    }
}
