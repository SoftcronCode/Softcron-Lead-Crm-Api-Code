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
    public class PaymentReportAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAppVariables _appVariables;
        private readonly IPaymentReport _paymentReport;
        public PaymentReportAPIController(IPaymentReport paymentReport, IAppVariables appVariables, IConfiguration configuration)
        {
            _paymentReport = paymentReport;
            _appVariables = appVariables;
            _configuration = configuration;
        }

        [HttpPost]
        public ResponseClass<PaymentReportResponse> PaymentReporting([FromBody] PaymentReportDTO ObjRequest)
        {
            return _paymentReport.PaymentReporting(ObjRequest);
        }
    }
}
