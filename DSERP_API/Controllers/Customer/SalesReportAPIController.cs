using CommonClass.BO;
using CommonClass.ITF.BL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Setup.BL.Customer;
using Setup.BO.Customer;
using Setup.DTO.Customer;
using Setup.ITF.Customer;

namespace DSERP_API.Controllers.Customer
{
    [EnableCors("AllowAllHeaders")]
    [Route("ERP/Customer/[action]")]
    [ApiController]
    public class SalesReportAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAppVariables _appVariables;
        private readonly ISalesReport _salesReport;
        public SalesReportAPIController(ISalesReport salesReport, IAppVariables appVariables, IConfiguration configuration)
        {
            _salesReport = salesReport;
            _appVariables = appVariables;
            _configuration = configuration;
        }

        [HttpPost]
        public ResponseClass<SalesReportResponse> SalesReporting([FromBody] SalesReportDTO ObjRequest)
        {
            return _salesReport.SalesReporting(ObjRequest);
        }
    }
}
