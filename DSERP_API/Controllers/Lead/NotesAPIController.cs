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
    public class NotesAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAppVariables _appVariables;
        private readonly INotes _notes;
        public NotesAPIController(INotes notes, IAppVariables appVariables, IConfiguration configuration)
        {
            _notes = notes;
            _appVariables = appVariables;
            _configuration = configuration;
        }

        [HttpPost]
        public ResponseClass<NotesResponse> GetNotesByID([FromBody] NotesDTO ObjRequest)
        {
            return _notes.GetNotesByID(ObjRequest);
        }

        [HttpPost]
        public ResponseClass<DocsResponse> GetDocsByID([FromBody] DocsDTO ObjRequest)
        {
            return _notes.GetDocsByID(ObjRequest);
        }
    }
}
