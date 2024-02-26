using CommonClass.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup.DTO.Lead
{
    public class NotesDTO
    {
        public int LeadID { get; set; }
        public int CustomerID { get; set; }
        public InsertCommonBasicBN ObjCommon { get; set; }
    }

    public class DocsDTO
    {
        public int LeadID { get; set; }
        public int CustomerID { get; set; }
        public InsertCommonBasicBN ObjCommon { get; set; }
    }

}
