using CommonClass.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup.DTO.Login
{
    public class ResetPasswordDTO
    {
        public string Email { get; set; }
        public InsertCommonBasicBN ObjCommon { get; set; }
    }
}
