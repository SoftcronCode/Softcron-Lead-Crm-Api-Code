using CommonClass.BO;
using Setup.BO.Customer;
using Setup.DTO.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup.ITF.Customer
{
    public interface IPaymentReport
    {
        ResponseClass<PaymentReportResponse> PaymentReporting(PaymentReportDTO ObjRequest);
    }
}
