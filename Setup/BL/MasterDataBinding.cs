/*
****************************************************************************************************

Sl No      Developer Name         Date              Version              Description
1          Neha Jain         08/05/2023          1                    Initital development
****************************************************************************************************
*/
using CommonClass.BL;
using CommonClass.BO;
using CommonClass.ITF.BL;
using Dal;
using Setup.BO;
using Setup.DTO;
using Setup.ITF;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace Setup.BL
{
    public class MasterDataBinding : IMasterDataBinding
    {
        #region Common Data
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IAppVariables _appVariables;
        private static IConfiguration _configuration;
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        private string _ErrorMessage = string.Empty;
        public MasterDataBinding(IHostEnvironment hostEnvironment, IAppVariables appVariables, IHttpContextAccessor httpContextAccessor, IConfiguration con)
        {
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _appVariables = appVariables;
            _configuration  = con;
        }
        #endregion
        #region Method
        public ResponseClass<BO.MasterDataBinding> GetMasterDataBinding(MasterDataBindingDTO ObjMasterDataBinding)
        {
            ResponseClass<BO.MasterDataBinding> response = new ResponseClass<BO.MasterDataBinding>();
            #region Validation
            if (ObjMasterDataBinding == null)
            {
                response.responseCode = 0;
                response.responseMessage = "Data Binding request can not be null!";
                return response;
            }
            else if (string.IsNullOrEmpty(ObjMasterDataBinding.ObjCommon.InsertedUserID) || ObjMasterDataBinding.ObjCommon.InsertedUserID == "0" || ObjMasterDataBinding.ObjCommon.InsertedUserID == "string")
            {
                response.responseCode = 0;
                response.responseMessage = "InsertedUserID required!";
                return response;
            }
            else if (int.TryParse(ObjMasterDataBinding.ObjCommon.InsertedUserID, out int result) == false)
            {
                response.responseCode = 0;
                response.responseMessage = "InsertedUserID Not valid!";
                return response;
            }
            else if (string.IsNullOrEmpty(ObjMasterDataBinding.ObjCommon.InsertedIPAddress) || ObjMasterDataBinding.ObjCommon.InsertedIPAddress == "0")
            {
                response.responseCode = 0;
                response.responseMessage = "InsertedIPAddress Not valid!";
                return response;
            }
            #endregion
            #region MySQL Connection
            //DB Connection Info Get From AppSetting file
            //_appVariables.SetAppVariables();
            string Connection = _configuration.GetConnectionString("Conn");
            CommonMethods commonMethods = new CommonMethods(_hostEnvironment, _httpContextAccessor);
            CommandExecutor objCommandExecutor = new CommandExecutor(Connection, true);
            SpParameters objSpParameters = new SpParameters(RdbmsType.MySql);
            #endregion
            try
            {
                #region Parameters
                objSpParameters.Add("SPAction", DbType.String, ObjMasterDataBinding.Action, ParameterDirection.Input);
                objSpParameters.Add("SPSearchText", DbType.String, ObjMasterDataBinding.SearchText, ParameterDirection.Input);
                objSpParameters.Add("SPFilterID", DbType.Int32, ObjMasterDataBinding.FilterID, ParameterDirection.Input);
                objSpParameters.Add("SPFilterID1", DbType.Int32, ObjMasterDataBinding.FilterID1, ParameterDirection.Input);
                objSpParameters.Add("SPFilterID2", DbType.String, ObjMasterDataBinding.FilterID2, ParameterDirection.Input);
                objSpParameters.Add("SPFilterID3", DbType.String, ObjMasterDataBinding.FilterID3, ParameterDirection.Input);
                objSpParameters.Add("SPSearchCriteria", DbType.String, ObjMasterDataBinding.SearchCriteria, ParameterDirection.Input);
                objSpParameters.Add("SPInsertUserID", DbType.Int32, ObjMasterDataBinding.ObjCommon.InsertedUserID, ParameterDirection.Input);
                objSpParameters.Add("SPInsertIPAddress", DbType.String, ObjMasterDataBinding.ObjCommon.InsertedIPAddress, ParameterDirection.Input);
                #endregion
                #region Call DB
                DataSet DSMaster = new DataSet();
                string _Json = string.Empty;
                dynamic _responseDynamic;
                string[] TableName = {"MasterResponse", "MasterData" };
                objCommandExecutor.ExecuteDataSet(CommandType.StoredProcedure, "erp_master_data_binding", DSMaster, TableName, objSpParameters);
                if (DSMaster != null && DSMaster.Tables.Count > 0 && DSMaster.Tables[0].Rows.Count > 0)
                {
                    response.responseCode = Convert.ToInt32(DSMaster.Tables[0].Rows[0]["ResponseCode"]);
                    response.responseMessage = Convert.ToString(DSMaster.Tables[0].Rows[0]["ResponseMessage"]);
                    if (response.responseCode != 0)
                    {
                        if (DSMaster.Tables.Count > 1 && DSMaster.Tables[1].Rows.Count > 0)
                        {
                            string JSONString = JsonConvert.SerializeObject(DSMaster.Tables[1]);
                           _responseDynamic = Compress.ZipStringToByte(JSONString);
                            response.responseDynamic = _responseDynamic;
                        }
                        else
                        {
                            response.responseCode = 1;
                            response.responseMessage = "No Records to display!";
                        }
                    }

                }
                else
                {
                    response.responseCode = 0;
                    response.responseMessage = "Response not valid!";
                }


               
                #endregion
            }
            catch (Exception ex)
            {
                response.responseCode = 0;
                response.responseMessage = ex.Message;
            }
            return response;
        }
        #endregion

    }
}
