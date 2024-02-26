using CommonClass.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup.DTO.Lead
{
    public class FollowupPopupDTO
    {
        public InsertCommonBasicBN ObjCommon { get; set; }
    }

    public class FollowupNotificationDTO
    {
        public string Action { get; set; }
        public int id {  get; set; }
        public InsertCommonBasicBN ObjCommon { get; set; }
    }
}
