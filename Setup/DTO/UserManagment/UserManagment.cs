using CommonClass.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup.DTO.UserManagment
{
    public class UserManagmentDTO
    {
        [Required(ErrorMessage = "Required Action Name")]
        public string action { get; set; }
        public int userid { get; set; }
        public int groupmasterid { get; set; }
        public InsertCommonBasicBN ObjCommon { get; set; }
    }
}
