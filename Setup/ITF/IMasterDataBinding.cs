using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Setup.DTO;
using Setup.BO;
using CommonClass.BO;

namespace Setup.ITF
{
    public interface IMasterDataBinding
    {
        ResponseClass<BO.MasterDataBinding> GetMasterDataBinding(MasterDataBindingDTO ObjMasterDataBinding);
    }
}