/*
**************************************************************************************************

Sl No      Developer Name         Date              Version              Description
1          Neha Jain         03/07/2023          1                    Initital development
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
    public class User : IUser
    {
        #region Common Data
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IAppVariables _appVariables;
        private static IConfiguration _configuration;
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        private string _ErrorMessage = string.Empty;
        public User(IHostEnvironment hostEnvironment, IAppVariables appVariables, IHttpContextAccessor httpContextAccessor, IConfiguration con)
        {
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _appVariables = appVariables;
            _configuration = con;
        }
        #endregion
        #region Method
        public ResponseClass<BO.AddClientUserResponse> AddClientUser(AddClientUserDTO ObjMaster)
        {
            ResponseClass<BO.AddClientUserResponse> response = new ResponseClass<BO.AddClientUserResponse>();
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
                objSpParameters.Add("SPClientMasterID", DbType.Int32, ObjMaster.ClientMasterID, ParameterDirection.Input);
                objSpParameters.Add("SPUserDisplayName", DbType.String, ObjMaster.UserDisplayName, ParameterDirection.Input);
                objSpParameters.Add("SPAppAccessUserName", DbType.String, ObjMaster.AppAccessUserName, ParameterDirection.Input);
                objSpParameters.Add("SPUserEmailID", DbType.String, ObjMaster.UserEmailID, ParameterDirection.Input);
                objSpParameters.Add("SPAppAccessPWD", DbType.String, CommonClass.BL.EncodeDecode.Encode(ObjMaster.AppAccessPWD), ParameterDirection.Input);
                objSpParameters.Add("SPUserRole", DbType.String, ObjMaster.UserRole, ParameterDirection.Input);
                objSpParameters.Add("SPInsertUserID", DbType.String, ObjMaster.ObjCommon.InsertedUserID, ParameterDirection.Input);
                objSpParameters.Add("SPInsertIPAddress", DbType.String, ObjMaster.ObjCommon.InsertedIPAddress, ParameterDirection.Input);


                #endregion
                #region Call DB
                DataSet DSMaster = new DataSet();
                string _Json = string.Empty;
                dynamic _responseDynamic;
                string[] TableName = { "MasterResponse" };
                var objAddRes = new BO.AddClientUserResponse();
                objCommandExecutor.ExecuteDataSet(CommandType.StoredProcedure, "client_user_details_insert", DSMaster, TableName, objSpParameters);
                if (DSMaster != null && DSMaster.Tables.Count > 0 && DSMaster.Tables[0].Rows.Count > 0)
                {
                    response.responseCode = Convert.ToInt32(DSMaster.Tables[0].Rows[0]["ResponseCode"]);
                    response.responseMessage = Convert.ToString(DSMaster.Tables[0].Rows[0]["ResponseMessage"]);
                    if (response.responseCode != 0)
                    {
                        objAddRes.UserID = Convert.ToInt32(DSMaster.Tables[0].Rows[0]["UserID"]);

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


        #region Method Change Password
        public ResponseClass<BO.ChangePasswordResponse> ChangePassword(ChangePasswordDTO ObjMaster)
        {
            ResponseClass<BO.ChangePasswordResponse> response = new ResponseClass<BO.ChangePasswordResponse>();
          
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
                objSpParameters.Add("SPUserEmail", DbType.String, ObjMaster.UserEmail, ParameterDirection.Input);
                objSpParameters.Add("SPNewPassword", DbType.String, CommonClass.BL.EncodeDecode.Encode(ObjMaster.NewPassword), ParameterDirection.Input);


                #endregion
                #region Call DB
                DataSet DSMaster = new DataSet();
                string _Json = string.Empty;
                dynamic _responseDynamic;
                string[] TableName = { "MasterResponse" };
                var objAddRes = new BO.AddClientUserResponse();
                objCommandExecutor.ExecuteDataSet(CommandType.StoredProcedure, "user_password_change", DSMaster, TableName, objSpParameters);
                if (DSMaster != null && DSMaster.Tables.Count > 0 && DSMaster.Tables[0].Rows.Count > 0)
                {
                    response.responseCode = Convert.ToInt32(DSMaster.Tables[0].Rows[0]["ResponseCode"]);
                    response.responseMessage = Convert.ToString(DSMaster.Tables[0].Rows[0]["ResponseMessage"]);
                    if (response.responseCode != 0)
                    {
                        //objAddRes.UserID = Convert.ToInt32(DSMaster.Tables[0].Rows[0]["UserID"]);

                        //string JSONString = JsonConvert.SerializeObject(objAddRes);
                        //_responseDynamic = Compress.ZipStringToByte(JSONString);
                        //response.responseDynamic = _responseDynamic;
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
