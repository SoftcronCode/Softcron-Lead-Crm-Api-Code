using CommonClass.BO;
using Setup.BL.Customer;
using Setup.BO.Customer;
using Setup.DTO.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup.ITF.Customer
{
    public interface IReporting
    {
         ResponseClass<ReportingResponse> CustomerReporting(ReporingDTO ObjRequest);
         ResponseClass<SalesRecordResponse> CustomerSalesRecord(SalesRecordDTO ObjRequest);
    }
}
