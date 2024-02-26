using CommonClass.BO;
using Setup.BO;
using Setup.BO.Login;
using Setup.DTO;
using Setup.DTO.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup.ITF.Login
{
    public interface IResetPassword
    {
        ResponseClass<ResetPasswordResponse> SendPasswordResetEmail(ResetPasswordDTO ObjRequest);
    }
}
