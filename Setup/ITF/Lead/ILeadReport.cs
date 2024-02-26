using CommonClass.BO;
using Microsoft.Extensions.Caching.Distributed;
using Setup.BO.Usermanagment;
using Setup.DTO.UserManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Setup.BO.Lead;
using Setup.DTO.Lead;

namespace Setup.ITF.Lead
{
    public interface ILeadReport
    {
        IDistributedCache _cache { get; set; }
        ResponseClass<LeadReportResponse> LeadReport(LeadReportDTO ObjLogin);

    }
}
