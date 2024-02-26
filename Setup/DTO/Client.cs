using CommonClass.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Setup.DTO
{
    public class AddClientDTO
    {


        public string ClientName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailID { get; set; }

        public InsertCommonBasicBN ObjCommon { get; set; }

    }
}
