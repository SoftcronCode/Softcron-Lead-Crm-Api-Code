/*
**************************************************************************************************

Sl No      Developer Name         Date              Version              Description
1          Neha Jain         03/07/2023          1                    Initital development
****************************************************************************************************
*/
using CommonClass.BO;
using Setup.BO;
using Setup.DTO;
using System.Collections.Generic;
using System.Dynamic;

namespace Setup.ITF
{
    public interface ITableOperation
    {
        ResponseClass<BO.AddClientResponse> Execute(TableOperationDTO ObjMaster);
         ResponseClass<BO.TableOperationResponse> OLDExecute(TableOperationDTO ObjMaster);
    }
}