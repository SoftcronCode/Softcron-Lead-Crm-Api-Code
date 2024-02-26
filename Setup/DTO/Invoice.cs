using CommonClass.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup.DTO
{
    public class InvoiceSalesDataDTO
    {
        [Required (ErrorMessage ="sales ID Required")]
        public int id { get; set; }
        public InsertCommonBasicBN ObjCommon { get; set; }
    }
}
