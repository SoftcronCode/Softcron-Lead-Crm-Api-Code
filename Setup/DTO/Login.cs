using CommonClass.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup.DTO
{
    public class ERPLoginDTO
    {
        [Required(ErrorMessage = "Required User Name")]
        [MaxLength(50, ErrorMessage = "Max User Name Write Length  is 50")]
        public string LoginUID { get; set; }
        [Required(ErrorMessage = "Required Password")]
        [MaxLength(50, ErrorMessage = "Max User Name Write Length  is 50")]
        public string LoginPWD { get; set; }

        //[Required(ErrorMessage = "Required Company")]
        //[MaxLength(6, ErrorMessage = "Company Length  Max is 6")]
        //public string CompanyCode { get; set; }
        
    }
    public class ClientERPLoginDTO
    {
        [Required(ErrorMessage = "Required User Name")]
        [MaxLength(50, ErrorMessage = "Max User Name Write Length  is 50")]
        public string LoginUID { get; set; }
        [Required(ErrorMessage = "Required Password")]
        [MaxLength(50, ErrorMessage = "Max User Name Write Length  is 50")]
        public string LoginPWD { get; set; }

        //[Required(ErrorMessage = "Required Cient")]
       
        //public int ClientMasterID { get; set; }

    }

}
