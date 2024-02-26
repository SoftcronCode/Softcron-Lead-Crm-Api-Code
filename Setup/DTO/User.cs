using CommonClass.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Setup.DTO
{
    public class AddClientUserDTO
    {

        [Required(ErrorMessage = "Client Masterid Required")]
        public int ClientMasterID { get; set; }

        [Required(ErrorMessage = "UserDisplayName Required")]
        public string UserDisplayName { get; set; }

        [Required(ErrorMessage = "UserName Required")]
        public string AppAccessUserName { get; set; }

        [Required(ErrorMessage = "User EmailId Required")]
        public string UserEmailID {  get; set; }

        [Required(ErrorMessage = "Password Required")]
        public string AppAccessPWD { get; set; }

        [Required(ErrorMessage = "Role Required")]
        public string UserRole { get; set; }

        public InsertCommonBasicBN ObjCommon { get; set; }

    }


    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "Password Required")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "EmailId Required")]
        public string UserEmail { get; set; }
    }
}
