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
using CommonClass.BL;
using System.IdentityModel.Tokens.Jwt;
using System.Dynamic;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Setup.BL
{
    public class Login : ILogin
    {
        #region Common Datas
        public IDistributedCache _cache { get; set; }
        private static IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IAppVariables _appVariables;
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        private string _ErrorMessage = string.Empty;
        public Login(IDistributedCache cache, IHostEnvironment hostEnvironment, IAppVariables appVariables, IHttpContextAccessor httpContextAccessor, IConfiguration con)
        {
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _appVariables = appVariables;
            _configuration = con;
            _cache = cache;
        }
        #endregion
        #region Login Method
        public ResponseClass<ERPLoginResponse> ERPLogin(ERPLoginDTO ObjLogin)
        {
            ResponseClass<ERPLoginResponse> response = new ResponseClass<ERPLoginResponse>();

            #region MySQL Connection
            //DB Connection Info Get From AppSetting file
           // _appVariables.SetAppVariables();
            //  Hashtable SettingInfo = new Hashtable();
            // SettingInfo = (Hashtable)_appVariables.AppSetting["Setting"];
            // string Connection = (string)SettingInfo["sZmX"];
            string Connection = _configuration.GetConnectionString("Conn");
            CommonMethods commonMethods = new CommonMethods(_hostEnvironment, _httpContextAccessor);
            CommandExecutor objCommandExecutor = new CommandExecutor(Connection, true);
            SpParameters objSpParameters = new SpParameters(RdbmsType.MySql);

            #endregion
            try
            {
                #region Parameters
                objSpParameters.Add("SPLoginUID", DbType.String, ObjLogin.LoginUID, ParameterDirection.Input);
                objSpParameters.Add("SPLoginPWD", DbType.String, CommonClass.BL.EncodeDecode.Encode(ObjLogin.LoginPWD), ParameterDirection.Input);
                //objSpParameters.Add("SPCompanyCode", DbType.String, ObjLogin.CompanyCode, ParameterDirection.Input);
                #endregion
                #region Call DB
                DataSet dsLogin = new DataSet();
                string _Json = string.Empty;
                string[] TableName = { "LoginResponse", "User" };
                objCommandExecutor.ExecuteDataSet(CommandType.StoredProcedure, "erp_login", dsLogin, TableName, objSpParameters);

                if (dsLogin != null && dsLogin.Tables.Count > 0 && dsLogin.Tables[0].Rows.Count > 0)
                {
                    response.responseCode = Convert.ToInt32(dsLogin.Tables[0].Rows[0]["ResponseCode"]);
                    response.responseMessage = Convert.ToString(dsLogin.Tables[0].Rows[0]["ResponseMessage"]);

                    if (response.responseCode != 0)
                    {

                        if (dsLogin.Tables.Count > 1 && dsLogin.Tables[1].Rows.Count > 0)
                        {
                            var objAddRes = new ERPLoginResponse();

                            string JSONString = JsonConvert.SerializeObject(dsLogin.Tables[1]);
                            dynamic _responseDynamic = Compress.ZipStringToByte(JSONString);
                            response.responseDynamic = _responseDynamic;


                        }
                        else
                        {
                            response.responseCode = 1;
                            response.responseMessage = "No Records to display!";
                        }

                    }
                    else
                    {
                        response.responseCode = 0;
                        response.responseMessage = "Response not valid!";
                    }

                }
                else
                {
                    response.responseCode = 0;
                    response.responseMessage = "Response not valid!";
                }
            }
            catch (Exception ex)
            {
                response.responseCode = 0;
                response.responseMessage = ex.Message;
            }


            return response;
        }
        #endregion
        #region Login Method
        public ResponseClass<ERPClientLoginResponse> ClientERPLogin(ClientERPLoginDTO ObjLogin)
        {
            ResponseClass<ERPClientLoginResponse> response = new ResponseClass<ERPClientLoginResponse>();

            #region MySQL Connection
            //DB Connection Info Get From AppSetting file
          //  _appVariables.SetAppVariables();
            string Connection = _configuration.GetConnectionString("Conn");
            CommonMethods commonMethods = new CommonMethods(_hostEnvironment, _httpContextAccessor);
            CommandExecutor objCommandExecutor = new CommandExecutor(Connection, true);
            SpParameters objSpParameters = new SpParameters(RdbmsType.MySql);
            #endregion
            try
            {
                #region Parameters
                objSpParameters.Add("SPLoginUID", DbType.String, ObjLogin.LoginUID, ParameterDirection.Input);
                objSpParameters.Add("SPLoginPWD", DbType.String, CommonClass.BL.EncodeDecode.Encode(ObjLogin.LoginPWD), ParameterDirection.Input);
                //objSpParameters.Add("SPClientMasterID", DbType.Int32, ObjLogin.ClientMasterID, ParameterDirection.Input);
                #endregion
                #region Call DB
                DataSet dsLogin = new DataSet();
                string _Json = string.Empty;
                string[] TableName = { "LoginResponse", "User" };
                objCommandExecutor.ExecuteDataSet(CommandType.StoredProcedure, "client_erp_login", dsLogin, TableName, objSpParameters);

                if (dsLogin != null && dsLogin.Tables.Count > 0 && dsLogin.Tables[0].Rows.Count > 0)
                {
                    response.responseCode = Convert.ToInt32(dsLogin.Tables[0].Rows[0]["ResponseCode"]);
                    response.responseMessage = Convert.ToString(dsLogin.Tables[0].Rows[0]["ResponseMessage"]);

                    if (response.responseCode != 0)
                    {

                        if (dsLogin.Tables.Count > 1 && dsLogin.Tables[1].Rows.Count > 0)
                        {
                            var objAddRes = new ERPClientLoginResponse();
                            var user = dsLogin.Tables[1].Rows[0].ToExpandoObject();
                            objAddRes.User = user;
                            string JSONString = JsonConvert.SerializeObject(objAddRes);
                            dynamic _responseDynamic = Compress.ZipStringToByte(JSONString);
                            response.responseDynamic = _responseDynamic;


                        }
                        else
                        {
                            response.responseCode = 1;
                            response.responseMessage = Convert.ToString(dsLogin.Tables[0].Rows[0]["ResponseMessage"]);
                        }

                    }
                    else
                    {
                        response.responseCode = 0;
                        response.responseMessage = Convert.ToString(dsLogin.Tables[0].Rows[0]["ResponseMessage"]);
                    }

                }
                else
                {
                    response.responseCode = 0;
                    response.responseMessage = "Response not valid!";
                }
            }
            catch (Exception ex)
            {
                response.responseCode = 0;
                response.responseMessage = ex.Message;
            }


            return response;
        }
        #endregion
        #endregion


    }
    #endregion
}
