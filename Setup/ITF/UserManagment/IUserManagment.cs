using CommonClass.BO;
using Microsoft.Extensions.Caching.Distributed;
using Setup.BO;
using Setup.BO.Usermanagment;
using Setup.DTO;
using Setup.DTO.UserManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup.ITF.UserManagment
{
    public interface IUserManagment
    {
        IDistributedCache _cache { get; set; }

        ResponseClass<UserManagmentResponse> ManageUser(UserManagmentDTO ObjLogin);
    }
}
