/*
**************************************************************************************************

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
    public class ExtensionDBMaster : IExtensionDBMaster
    {
        #region Common Data
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IAppVariables _appVariables;
        private static IConfiguration _configuration;
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        private string _ErrorMessage = string.Empty;
        public ExtensionDBMaster(IHostEnvironment hostEnvironment, IAppVariables appVariables, IHttpContextAccessor httpContextAccessor, IConfiguration con)
        {
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _appVariables = appVariables;
            _configuration = con;
        }
        #endregion
        #region Method
        public ResponseClass<BO.TableResponse> AddTable(AddTableDTO ObjMaster)
        {
            ResponseClass<BO.TableResponse> response = new ResponseClass<BO.TableResponse>();
            #region Validation
            if (ObjMaster == null)
            {
                response.responseCode = 0;
                response.responseMessage = "Data Binding request can not be null!";
                return response;
            }
            else if (string.IsNullOrEmpty(ObjMaster.ObjCommon.InsertedUserID) || ObjMaster.ObjCommon.InsertedUserID == "0" || ObjMaster.ObjCommon.InsertedUserID == "string")
            {
                response.responseCode = 0;
                response.responseMessage = "InsertedUserID required!";
                return response;
            }
            else if (int.TryParse(ObjMaster.ObjCommon.InsertedUserID, out int result) == false)
            {
                response.responseCode = 0;
                response.responseMessage = "InsertedUserID Not valid!";
                return response;
            }
            else if (string.IsNullOrEmpty(ObjMaster.ObjCommon.InsertedIPAddress) || ObjMaster.ObjCommon.InsertedIPAddress == "0")
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
                objSpParameters.Add("SPTableName", DbType.String, ObjMaster.TableName, ParameterDirection.Input);
                objSpParameters.Add("SPClientMasterID", DbType.Int32, ObjMaster.ClientMasterID, ParameterDirection.Input);
                objSpParameters.Add("SPTableAliasName", DbType.String, ObjMaster.TableAliasName, ParameterDirection.Input);
                objSpParameters.Add("SPTableUrl", DbType.String, ObjMaster.TableUrl, ParameterDirection.Input);
                #endregion
                #region Call DB
                DataSet DSMaster = new DataSet();
                string _Json = string.Empty;
                dynamic _responseDynamic;
                string[] TableName = { "MasterResponse" };
                objCommandExecutor.ExecuteDataSet(CommandType.StoredProcedure, "add_table", DSMaster, TableName, objSpParameters);
                if (DSMaster != null && DSMaster.Tables.Count > 0 && DSMaster.Tables[0].Rows.Count > 0)
                {
                    response.responseCode = Convert.ToInt32(DSMaster.Tables[0].Rows[0]["ResponseCode"]);
                    response.responseMessage = Convert.ToString(DSMaster.Tables[0].Rows[0]["ResponseMessage"]);
                    if (response.responseCode != 0)
                    {
                        
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
        #region Method
        public ResponseClass<BO.ColumnResponse> AddColumn(AddColumnDTO ObjMaster)
        {
            ResponseClass<BO.ColumnResponse> response = new ResponseClass<BO.ColumnResponse>();
            #region Validation
            if (ObjMaster == null)
            {
                response.responseCode = 0;
                response.responseMessage = "Data Binding request can not be null!";
                return response;
            }
            else if (string.IsNullOrEmpty(ObjMaster.ObjCommon.InsertedUserID) || ObjMaster.ObjCommon.InsertedUserID == "0" || ObjMaster.ObjCommon.InsertedUserID == "string")
            {
                response.responseCode = 0;
                response.responseMessage = "InsertedUserID required!";
                return response;
            }
            else if (int.TryParse(ObjMaster.ObjCommon.InsertedUserID, out int result) == false)
            {
                response.responseCode = 0;
                response.responseMessage = "InsertedUserID Not valid!";
                return response;
            }
            else if (string.IsNullOrEmpty(ObjMaster.ObjCommon.InsertedIPAddress) || ObjMaster.ObjCommon.InsertedIPAddress == "0")
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
                objSpParameters.Add("SPExtensionTableCode", DbType.Int32, ObjMaster.ExtensionTableCode, ParameterDirection.Input);
                objSpParameters.Add("SPExtensionTableName", DbType.String, ObjMaster.ExtensionTableName, ParameterDirection.Input);
                objSpParameters.Add("SPFieldName", DbType.String, ObjMaster.FieldName, ParameterDirection.Input);
                objSpParameters.Add("SPDisplayName", DbType.String, ObjMaster.DisplayName, ParameterDirection.Input);
                objSpParameters.Add("SPControlType", DbType.Int32, ObjMaster.ControlType, ParameterDirection.Input);
                objSpParameters.Add("SPInputDataType", DbType.Int32, ObjMaster.InputDataType, ParameterDirection.Input);
                objSpParameters.Add("SPValidate_MaxLength", DbType.Int32, ObjMaster.Validate_MaxLength, ParameterDirection.Input);
                objSpParameters.Add("SPValidate_isRequired", DbType.Int32, ObjMaster.Validate_isRequired, ParameterDirection.Input);
                objSpParameters.Add("SPValidate_isUnique", DbType.Int32, ObjMaster.Validate_isUnique, ParameterDirection.Input);
                objSpParameters.Add("SPValidate_isReference", DbType.Int32, ObjMaster.Validate_isReference, ParameterDirection.Input);
                objSpParameters.Add("SPReferenceTableName", DbType.String, ObjMaster.ReferenceTableName, ParameterDirection.Input);
                objSpParameters.Add("SPReferenceFieldName", DbType.String, ObjMaster.ReferenceFieldName, ParameterDirection.Input);
                objSpParameters.Add("SPInsertUserID", DbType.String, ObjMaster.ObjCommon.InsertedUserID, ParameterDirection.Input);
                objSpParameters.Add("SPInsertIPAddress", DbType.String, ObjMaster.ObjCommon.InsertedIPAddress, ParameterDirection.Input);


                #endregion
                #region Call DB
                DataSet DSMaster = new DataSet();
                string _Json = string.Empty;
                dynamic _responseDynamic;
                string[] TableName = { "MasterResponse" };
                var objAddRes = new BO.ColumnResponse();
                objCommandExecutor.ExecuteDataSet(CommandType.StoredProcedure, "field_master_extension_config_insert", DSMaster, TableName, objSpParameters);
                if (DSMaster != null && DSMaster.Tables.Count > 0 && DSMaster.Tables[0].Rows.Count > 0)
                {
                    response.responseCode = Convert.ToInt32(DSMaster.Tables[0].Rows[0]["ResponseCode"]);
                    response.responseMessage = Convert.ToString(DSMaster.Tables[0].Rows[0]["ResponseMessage"]);
                    if (response.responseCode != 0)
                    {
                        objAddRes.ExtensionFieldID = Convert.ToInt32(DSMaster.Tables[0].Rows[0]["ExtensionFieldID"]);
                      
                            string JSONString = JsonConvert.SerializeObject(objAddRes);
                            _responseDynamic = Compress.ZipStringToByte(JSONString);
                            response.responseDynamic = _responseDynamic;                       
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
