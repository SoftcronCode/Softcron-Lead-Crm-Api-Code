using CommonClass.BO;
using CommonClass.ITF.BL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Setup.BO;
using Setup.BO.Lead;
using Setup.DTO;
using Setup.DTO.Lead;
using Setup.ITF.Invoice;

namespace DSERP_API.Controllers.Invoice
{
    [EnableCors("AllowAllHeaders")]
    [Route("ERP/Invoice/[action]")]
    [ApiController]
    public class InvoiceAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAppVariables _appVariables;
        private readonly IInvoice _invoice;
        public InvoiceAPIController(IInvoice invoice, IAppVariables appVariables, IConfiguration configuration)
        {
            _invoice = invoice;
            _appVariables = appVariables;
            _configuration = configuration;
        }

        [HttpPost]
        public ResponseClass<InvoiceSalesDataResponse> GetSalesDataForInvoice([FromBody] InvoiceSalesDataDTO ObjRequest)
        {
            return _invoice.GetSalesDataForInvoice(ObjRequest);
        }
    }
}
