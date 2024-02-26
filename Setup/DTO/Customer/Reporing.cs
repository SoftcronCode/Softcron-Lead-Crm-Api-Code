using CommonClass.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup.DTO.Customer
{
    public class ReporingDTO
    {
        public InsertCommonBasicBN ObjCommon { get; set; }
    }

    public class SalesRecordDTO
    {
        public int Customer_id {get; set;}
        public InsertCommonBasicBN ObjCommon { get; set; }
    }
}
