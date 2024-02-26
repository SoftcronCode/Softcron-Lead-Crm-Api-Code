using CommonClass.BO;
using Microsoft.Extensions.Caching.Distributed;
using Setup.BL.Invoice;
using Setup.BO;
using Setup.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup.ITF.Invoice
{
    public interface IInvoice
    {
        IDistributedCache _cache { get; set; }

        ResponseClass<InvoiceSalesDataResponse> GetSalesDataForInvoice( InvoiceSalesDataDTO ObjRequest);
    }
}
