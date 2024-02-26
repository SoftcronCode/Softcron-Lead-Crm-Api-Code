using CommonClass.BO;
using CommonClass.ITF.BL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Setup.BO.Customer;
using Setup.BO.Lead;
using Setup.DTO.Customer;
using Setup.DTO.Lead;
using Setup.ITF.Customer;
using Setup.ITF.Lead;

namespace DSERP_API.Controllers.Customer
{
    [EnableCors("AllowAllHeaders")]
    [Route("ERP/Customer/[action]")]
    [ApiController]
    public class ReportingAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAppVariables _appVariables;
        private readonly IReporting _reporting;
        public ReportingAPIController(IReporting reporting, IAppVariables appVariables, IConfiguration configuration)
        {
            _reporting = reporting;
            _appVariables = appVariables;
            _configuration = configuration;
        }

        [HttpPost]
        public ResponseClass<ReportingResponse> CustomerReporting([FromBody] ReporingDTO ObjRequest)
        {
            return _reporting.CustomerReporting(ObjRequest);
        }

        [HttpPost]
        public ResponseClass<SalesRecordResponse> CustomerSalesRecord([FromBody] SalesRecordDTO ObjRequest)
        {
            return _reporting.CustomerSalesRecord(ObjRequest);
        }
    }
}
