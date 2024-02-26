using CommonClass.BO;
using CommonClass.Filter;
using CommonClass.ITF.BL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Setup.BO;
using Setup.DTO;
using Setup.ITF;

namespace POS_WMS.Controllers.Setup
{
   
    [EnableCors("AllowAllHeaders")]
    [Route("ERP/Setup/[action]")]
    [ApiController]
    public class MasterDataBindingAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAppVariables _appVariables;
        private readonly IMasterDataBinding _MasterDataBinding;
        public MasterDataBindingAPIController(IMasterDataBinding MasterDataBinding, IAppVariables appVariables, IConfiguration configuration)
        {
            _MasterDataBinding = MasterDataBinding;
            _appVariables = appVariables;
            _configuration = configuration;
        }
       

        [HttpPost]
        public ResponseClass<MasterDataBinding> GetMasterDataBinding([FromBody] MasterDataBindingDTO ObjRequest)
        {
            return _MasterDataBinding.GetMasterDataBinding(ObjRequest);
        }
        
    }
}
