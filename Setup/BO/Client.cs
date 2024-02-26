using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup.BO
{
    public class Client
    {
    }
   
    public class AddClientResponse
    {
        public int ClientMasterID { get; set; }
    }

    public class TableOperationResponse2
    {
        
        public int responseCode { get; set; }
        public string responseMessage { get; set; }
        public string Base64String { get; set; }
        public IEnumerable<dynamic> Images { get; set; }
        public dynamic responseDynamic { get; set; }
        public  byte[] Image { get; set; }

    }

    public class TableOperationResponse1
    {
        public TableOperationResponse objTableOperationResponse1 { get; set; }
}
}
