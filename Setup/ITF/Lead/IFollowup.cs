using CommonClass.BO;
using Microsoft.Extensions.Caching.Distributed;
using Setup.BO.Lead;
using Setup.DTO.Lead;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup.ITF.Lead
{
    public interface IFollowup
    {
        IDistributedCache _cache { get; set; }
        ResponseClass<FollowupPopupResponse> FollowupReminderNotification(FollowupPopupDTO ObjLogin);
        ResponseClass<FollowupNotificationResponse> Followup_notification(FollowupNotificationDTO ObjLogin);
    }
}
