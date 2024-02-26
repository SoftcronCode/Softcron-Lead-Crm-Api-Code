using CommonClass.BL;
using CommonClass.BO;
using CommonClass.ITF.BL;
using Dal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Setup.BO.Usermanagment;
using Setup.BO;
using Setup.DTO.UserManagment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Setup.BO.Lead;
using Setup.DTO.Lead;
using Setup.ITF.Lead;

namespace Setup.BL.Lead
{
    public class LeadReporting : ILeadReport
    {
        #region Common Data
        public IDistributedCache _cache { get; set; }
        private static IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IAppVariables _appVariables;
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        private string _ErrorMessage = string.Empty;
        public LeadReporting(IDistributedCache cache, IHostEnvironment hostEnvironment, IAppVariables appVariables, IHttpContextAccessor httpContextAccessor, IConfiguration con)
        {
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _appVariables = appVariables;
            _configuration = con;
            _cache = cache;
        }
        #endregion

        #region Reporting Method
        public ResponseClass<LeadReportResponse> LeadReport(LeadReportDTO ObjRequest)
        {
            ResponseClass<LeadReportResponse> response = new ResponseClass<LeadReportResponse>();

            #region MySQL Connection
            string Connection = _configuration.GetConnectionString("Conn");
            CommonMethods commonMethods = new CommonMethods(_hostEnvironment, _httpContextAccessor);
            CommandExecutor objCommandExecutor = new CommandExecutor(Connection, true);
            SpParameters objSpParameters = new SpParameters(RdbmsType.MySql);
            #endregion

            try
            {
                #region Parameters
                //objSpParameters.Add("SPInsertUserID", DbType.String, ObjRequest.ObjCommon.InsertedUserID, ParameterDirection.Input);
                //objSpParameters.Add("SPInsertIPAddress", DbType.String, ObjRequest.ObjCommon.InsertedIPAddress, ParameterDirection.Input);
                #endregion


                #region Call DB
                DataSet dsLogin = new DataSet();
                string _Json = string.Empty;
                string[] TableName = { "Response Table", "Data" };
                objCommandExecutor.ExecuteDataSet(CommandType.StoredProcedure, "new_lead_report", dsLogin, TableName, objSpParameters);

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
                        response.responseMessage = Convert.ToString(dsLogin.Tables[0].Rows[0]["ResponseMessage"]);
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
