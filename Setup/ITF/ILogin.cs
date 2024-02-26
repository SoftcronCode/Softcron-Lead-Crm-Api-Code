using CommonClass.BO;
using Microsoft.Extensions.Caching.Distributed;
using Setup.BO;
using Setup.DTO;

namespace Setup.ITF
{
    public interface ILogin
    {
        IDistributedCache _cache { get; set; }

        ResponseClass<ERPLoginResponse> ERPLogin(ERPLoginDTO ObjLogin);
        ResponseClass<ERPClientLoginResponse> ClientERPLogin(ClientERPLoginDTO ObjLogin);
    }
}