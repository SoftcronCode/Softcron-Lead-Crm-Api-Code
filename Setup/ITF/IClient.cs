﻿/*
**************************************************************************************************

Sl No      Developer Name         Date              Version              Description
1          Neha Jain         03/07/2023          1                    Initital development
****************************************************************************************************
*/
using CommonClass.BO;
using Setup.BO;
using Setup.DTO;

namespace Setup.ITF
{
    public interface IClient
    {
        ResponseClass<AddClientResponse> AddClient(AddClientDTO ObjMaster);
    }
}