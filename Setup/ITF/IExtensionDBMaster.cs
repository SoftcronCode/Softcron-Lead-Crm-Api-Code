/*
**************************************************************************************************

Sl No      Developer Name         Date              Version              Description
1          Neha Jain         08/05/2023          1                    Initital development
****************************************************************************************************
*/
using CommonClass.BO;
using Setup.BO;
using Setup.DTO;

namespace Setup.ITF
{
    public interface IExtensionDBMaster
    {
        ResponseClass<ColumnResponse> AddColumn(AddColumnDTO ObjMaster);
        ResponseClass<TableResponse> AddTable(AddTableDTO ObjMaster);
    }
}