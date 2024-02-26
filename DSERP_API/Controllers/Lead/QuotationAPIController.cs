using CommonClass.BO;
using CommonClass.ITF.BL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Setup.BL.Lead;
using Setup.BO.Lead;
using Setup.DTO.Lead;
using Setup.ITF.Lead;

namespace DSERP_API.Controllers.Lead
{
    [EnableCors("AllowAllHeaders")]
    [Route("ERP/Lead/[action]")]
    [ApiController]

    public class QuotationAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAppVariables _appVariables;
        private readonly IQuotation _quotation;
        public QuotationAPIController(IQuotation quotation, IAppVariables appVariables, IConfiguration configuration)
        {
            _quotation = quotation;
            _appVariables = appVariables;
            _configuration = configuration;
        }

        [HttpPost]
        public ResponseClass<QuotationResponse> GetAllQuotationData([FromBody] QuotationDTO ObjRequest)
        {
            return _quotation.GetAllQuotationData(ObjRequest);
        }
    }
}
