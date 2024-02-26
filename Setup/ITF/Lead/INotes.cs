using CommonClass.BO;
using Setup.BL.Lead;
using Setup.BO.Lead;
using Setup.DTO.Lead;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup.ITF.Lead
{
    public interface INotes
    {
        ResponseClass<NotesResponse> GetNotesByID( NotesDTO ObjRequest);

        ResponseClass<DocsResponse> GetDocsByID(DocsDTO ObjRequest);
    }
}
