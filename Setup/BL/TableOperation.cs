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
using System.IO;
using System.Text.Json;
using System.Globalization;
using System.Dynamic;
using Microsoft.Extensions.Primitives;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using System.Drawing;
using Microsoft.Extensions.Configuration;

namespace Setup.BL
{
    public class TableOperation : ITableOperation
    {
        #region Common Data
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IAppVariables _appVariables;
        private static IConfiguration _configuration;
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        private string _ErrorMessage = string.Empty;
        public TableOperation(IHostEnvironment hostEnvironment, IAppVariables appVariables, IHttpContextAccessor httpContextAccessor, IConfiguration con)
        {
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _appVariables = appVariables;
            _configuration = con;
        }
        #endregion
        #region Method

        //   public ResponseClass<IEnumerable<dynamic>> Execute(TableOperationDTO ObjMaster)
        public ResponseClass<BO.TableOperationResponse> OLDExecute(TableOperationDTO ObjMaster)
        {
            ResponseClass<BO.TableOperationResponse> response = new ResponseClass<BO.TableOperationResponse>();
            List<System.Dynamic.ExpandoObject> exObj = new List<System.Dynamic.ExpandoObject>();
            #region Validation
            /*  if (ObjMaster == null)
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
            */
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

                string customColumn = "";
                string customValue = "";
                string ExecQuery = "";

                StringBuilder Query = new StringBuilder();
                StringBuilder Column = new StringBuilder();
                string QueryToExecute;
                int count = 0;
                if (ObjMaster.Action == "INSERT")
                {
                    Column.Append("INSERT INTO " + ObjMaster.TableName + "(");
                    Query.Append("VALUES(");
                    foreach (var custom in ObjMaster.Columns)
                    {
                        Column.Append(custom.ColumnName + ",");
                        if (custom.ColumnDataType == 105001 || custom.ColumnDataType == 105006)
                        {
                            Query.Append("'" + custom.ColumnValue + "',");
                        }
                        else if (custom.ColumnDataType == 105002 || custom.ColumnDataType == 105005)
                        {
                            // if (long.TryParse(custom.ColumnValue, out long result) == false)
                            if (decimal.TryParse(custom.ColumnValue, out decimal result) == false)
                            {
                                response.responseCode = 0;
                                response.responseMessage = "Value of Field is not valid !" + custom.ColumnName;
                                return response;
                            }
                            else
                            {
                                Query.Append(custom.ColumnValue + ",");
                            }
                        }
                        else if (custom.ColumnDataType == 105003)
                        {
                            DateTime dateValue;
                            if (DateTime.TryParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateShort, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                            {
                                DateTime DT = DateTime.ParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateShort, CultureInfo.InvariantCulture);

                                Query.Append("'" + DT.ToString("yyyy-MM-dd") + "',");
                            }
                            else
                            {
                                response.responseCode = 0;
                                response.responseMessage = "Date of Field is not valid !" + custom.ColumnName;
                                //   return response;
                            }
                        }

                        else if (custom.ColumnDataType == 105004)
                        {
                            DateTime dateValue;
                            if (DateTime.TryParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateLong, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                            {
                                DateTime DT = DateTime.ParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateLong, CultureInfo.InvariantCulture);

                                Query.Append("'" + DT.ToString("yyyy-MM-dd - HH:mm:ss") + "',");
                            }
                            else
                            {
                                response.responseCode = 0;
                                response.responseMessage = "Date of Field is not valid !" + custom.ColumnName;
                                //    return response;
                            }
                        }
                        else if (custom.ColumnDataType == 105007)
                        {
                            Query.Append("'" + custom.ColumnValue + "',");
                        }

                    }
                    customValue = Query.ToString().TrimEnd(',');
                    customValue = customValue + ");";
                    customColumn = Column.ToString().TrimEnd(',');
                    customColumn = customColumn + ")";
                    ExecQuery = customColumn + customValue;
                }


                else if (ObjMaster.Action == "SELECT")
                {

                }

                else if (ObjMaster.Action == "GETBYID")
                {
                    // Query.Append("SELECT * FROM " + ObjMaster.TableName + " WHERE " + ObjMaster.TableName + "ID = " + ObjMaster.ID + ";") ;
                    Query.Append(" AND MAS." + ObjMaster.TableName + "ID = " + ObjMaster.ID + "; ");
                    ExecQuery = Query.ToString();
                }

                else if (ObjMaster.Action == "SELECTBYID")
                {
                    Query.Append(" AND MAS." + ObjMaster.primaryColumn + " = " + ObjMaster.ID + "; ");
                    ExecQuery = Query.ToString();
                }

                else if (ObjMaster.Action == "DELETE")
                {
                    Query.Append("UPDATE " + ObjMaster.TableName + " SET is_delete = 1 WHERE " + ObjMaster.TableName + "ID = " + ObjMaster.ID + ";");
                    ExecQuery = Query.ToString();
                }

                else if(ObjMaster.Action == "UPDATE_STATUS")
                {
                    Query.Append("UPDATE " + ObjMaster.TableName + " SET is_active = " + ObjMaster.primaryColumnValue + " WHERE " + ObjMaster.TableName + "ID = " + ObjMaster.ID + ";");
                    ExecQuery = Query.ToString();
                }
                

                if (ObjMaster.Action == "UPDATE")
                {
                    Query.Append("UPDATE " + ObjMaster.TableName + " SET ");
                    foreach (var custom in ObjMaster.Columns)
                    {
                        if (custom.ColumnDataType == 105001 || custom.ColumnDataType == 105006)
                        {
                            Query.Append(custom.ColumnName + " = '" + custom.ColumnValue + "', ");
                        }

                        else if (custom.ColumnDataType == 105002 || custom.ColumnDataType == 105005)
                        {
                            // if (long.TryParse(custom.ColumnValue, out long result) == false)
                            if (decimal.TryParse(custom.ColumnValue, out decimal result) == false)
                            {
                                response.responseCode = 0;
                                response.responseMessage = "Value of Field is not valid !" + custom.ColumnName;
                                //     return response;
                            }
                            else
                            {
                                Query.Append(custom.ColumnName + " = '" + custom.ColumnValue + "', ");
                            }
                        }

                        else if (custom.ColumnDataType == 105003)
                        {
                            DateTime dateValue;
                            if (DateTime.TryParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateShort, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                            {
                                DateTime DT = DateTime.ParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateShort, CultureInfo.InvariantCulture);

                                Query.Append(custom.ColumnName + " = '" + DT.ToString("yyyy-MM-dd") + "', ");
                            }
                            else
                            {
                                response.responseCode = 0;
                                response.responseMessage = "Date of Field is not valid !" + custom.ColumnName;
                                //       return response;
                            }
                        }
                        else if (custom.ColumnDataType == 105004)
                        {
                            DateTime dateValue;
                            if (DateTime.TryParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateLong, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                            {
                                DateTime DT = DateTime.ParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateLong, CultureInfo.InvariantCulture);

                                Query.Append(custom.ColumnName + " = '" + DT.ToString("yyyy-MM-dd - HH:mm:ss") + "', ");
                            }
                            else
                            {
                                response.responseCode = 0;
                                response.responseMessage = "Date of Field is not valid !" + custom.ColumnName;
                                //        return response;
                            }
                        }


                    }
                    Query.Length -= 2;

                    Query.Append(" WHERE " + ObjMaster.TableName + "ID = " + ObjMaster.ID + ";");

                    ExecQuery = Query.ToString();
                }


                QueryToExecute = ExecQuery;

                string ActionName = ObjMaster.Action;
                string Tablename = ObjMaster.TableName;
                #region Parameters
                objSpParameters.Add("SPQuery", DbType.String, QueryToExecute, ParameterDirection.Input);
                objSpParameters.Add("SPAction", DbType.String, ActionName, ParameterDirection.Input);
                objSpParameters.Add("TableName", DbType.String, Tablename, ParameterDirection.Input);

                #endregion  
                #region Call DB
                DataSet DSMaster = new DataSet();
                string _Json = string.Empty;
                dynamic _responseDynamic;
                var objRes = new BO.TableOperationResponse();
                string[] TableName = { "MasterResponse", "MasterData" };
                objCommandExecutor.ExecuteDataSet(CommandType.StoredProcedure, "table_operation", DSMaster, TableName, objSpParameters);

                response.responseObject = response.responseObject ?? new List<BO.TableOperationResponse>();

                if (DSMaster != null && DSMaster.Tables.Count > 0 && DSMaster.Tables[0].Rows.Count > 0)
                {
                    response.responseCode = Convert.ToInt32(DSMaster.Tables[0].Rows[0]["ResponseCode"]);
                    response.responseMessage = Convert.ToString(DSMaster.Tables[0].Rows[0]["ResponseMessage"]);
                    
                    var exObj1 = DSMaster.Tables[1].ToExpandoObjectList();
                    if (response.responseCode != 0)
                    {
                        if (DSMaster.Tables[0].Columns.Contains("insertedID"))
                        {
                            objRes.LastInsertedID = Convert.ToInt32(DSMaster.Tables[0].Rows[0]["insertedID"]);
                            response.responseObject.Add(objRes);
                        }

                        string JSONString = JsonConvert.SerializeObject(exObj1);
                        _responseDynamic = Compress.ZipStringToByte(JSONString);
                        response.responseDynamic = _responseDynamic;
                        //    exObj = testObj;
                        //     response.responseObject = testObj;


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
        public ResponseClass<BO.AddClientResponse> Execute(TableOperationDTO ObjMaster)
        {
            ResponseClass<AddClientResponse> response = new ResponseClass<AddClientResponse>();
            IEnumerable<dynamic> exObj;
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

                //string text = File.ReadAllText(@"column.json");
                //var playerList = JsonConvert.DeserializeObject<List<TableOperationDTO>>(text);

                string customColumn = "";
                string customValue = "";
                string ExecQuery = "";

                StringBuilder Query = new StringBuilder();
                StringBuilder Column = new StringBuilder();
                string QueryToExecute;
                int count = 0;
                if (ObjMaster.Action == "INSERT")
                {
                    Column.Append("INSERT INTO " + ObjMaster.TableName + "(");
                    Query.Append("VALUES(");
                    foreach (var custom in ObjMaster.Columns)
                    {
                        Column.Append(custom.ColumnName + ',');
                        if (custom.ColumnDataType == 105001 || custom.ColumnDataType == 105006)
                        {
                            Query.Append("'" + custom.ColumnValue + "',");
                        }
                        else if (custom.ColumnDataType == 105002 || custom.ColumnDataType == 105005)
                        {
                            // if (long.TryParse(custom.ColumnValue, out long result) == false)
                            if (decimal.TryParse(custom.ColumnValue, out decimal result) == false)
                            {
                                response.responseCode = 0;
                               response.responseMessage = "Value of Field is not valid !" + custom.ColumnName;
                               return response;
                            }
                            else
                            {
                                Query.Append(custom.ColumnValue + ',');
                            }
                        }
                        else if (custom.ColumnDataType == 105003)
                        {
                            DateTime dateValue;
                            if (DateTime.TryParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateShort, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                            {
                                DateTime DT = DateTime.ParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateShort, CultureInfo.InvariantCulture);

                                Query.Append("'" + DT.ToString("yyyy-MM-dd") + "',");
                            }
                            else
                            {
                                response.responseCode = 0;
                                response.responseMessage = "Date of Field is not valid !" + custom.ColumnName;
                               return response;
                            }
                        }

                        else if (custom.ColumnDataType == 105004)
                        {
                            DateTime dateValue;
                            if (DateTime.TryParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateLong, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                            {
                                DateTime DT = DateTime.ParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateLong, CultureInfo.InvariantCulture);

                                Query.Append("'" + DT.ToString("yyyy-MM-dd - HH:mm:ss") + "',");
                            }
                            else
                            {
                                response.responseCode = 0;
                                response.responseMessage = "Date of Field is not valid !" + custom.ColumnName;
                               return response;
                            }
                        }
                        else if (custom.ColumnDataType == 105007 )
                            {
                                Query.Append("'" + custom.ColumnValue + "',");
                            }

                    }
                    customValue = Query.ToString().TrimEnd(',');
                    customValue = customValue + ");";
                    customColumn = Column.ToString().TrimEnd(',');
                    customColumn = customColumn + ")";
                    ExecQuery = customColumn + customValue;
                }


                else if (ObjMaster.Action == "SELECT")
                {

                }

                else if (ObjMaster.Action == "GETBYID")
                {
                    // Query.Append("SELECT * FROM " + ObjMaster.TableName + " WHERE " + ObjMaster.TableName + "ID = " + ObjMaster.ID + ";") ;
                    Query.Append(" AND MAS." + ObjMaster.TableName + "ID = " + ObjMaster.ID + "; ");
                    ExecQuery = Query.ToString();
                }

                else if (ObjMaster.Action == "DELETE")
                {
                    Query.Append("UPDATE " + ObjMaster.TableName + " SET is_delete = 1 WHERE " + ObjMaster.TableName + "ID = " + ObjMaster.ID + ";");
                    ExecQuery = Query.ToString();
                }



                if (ObjMaster.Action == "UPDATE")
                {
                    Query.Append("UPDATE " + ObjMaster.TableName + " SET ");
                    foreach (var custom in ObjMaster.Columns)
                    {
                        if (custom.ColumnDataType == 105001 || custom.ColumnDataType == 105006)
                        {
                            Query.Append(custom.ColumnName + " = '" + custom.ColumnValue + "', ");
                        }

                        else if (custom.ColumnDataType == 105002 || custom.ColumnDataType == 105005)
                        {
                            // if (long.TryParse(custom.ColumnValue, out long result) == false)
                            if (decimal.TryParse(custom.ColumnValue, out decimal result) == false)
                            {
                                response.responseCode = 0;
                                response.responseMessage = "Value of Field is not valid !" + custom.ColumnName;
                                return response;
                            }
                            else
                            {
                                Query.Append(custom.ColumnName + " = '" + custom.ColumnValue + "', ");
                            }
                        }

                        else if (custom.ColumnDataType == 105003)
                        {
                            DateTime dateValue;
                            if (DateTime.TryParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateShort, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                            {
                                DateTime DT = DateTime.ParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateShort, CultureInfo.InvariantCulture);

                                Query.Append(custom.ColumnName + " = '" + DT.ToString("yyyy-MM-dd") + "',");
                            }
                            else
                            {
                                response.responseCode = 0;
                              response.responseMessage = "Date of Field is not valid !" + custom.ColumnName;
                               return response;
                            }
                        }
                        else if (custom.ColumnDataType == 105004)
                        {
                            DateTime dateValue;
                            if (DateTime.TryParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateLong, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                            {
                                DateTime DT = DateTime.ParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateLong, CultureInfo.InvariantCulture);

                                Query.Append(custom.ColumnName + " = '" + DT.ToString("yyyy-MM-dd - HH:mm:ss") + "',");
                            }
                            else
                            {
                                response.responseCode = 0;
                                response.responseMessage = "Date of Field is not valid !" + custom.ColumnName;
                                return response;
                            }
                        }


                    }
                    Query.Length -= 2;

                    Query.Append(" WHERE " + ObjMaster.TableName + "ID = " + ObjMaster.ID + ";");

                    ExecQuery = Query.ToString();
                }


                QueryToExecute = ExecQuery;

                string ActionName = ObjMaster.Action;
                string Tablename = ObjMaster.TableName;
                #region Parameters
                objSpParameters.Add("SPQuery", DbType.String, QueryToExecute, ParameterDirection.Input);
                objSpParameters.Add("SPAction", DbType.String, ActionName, ParameterDirection.Input);
                objSpParameters.Add("TableName", DbType.String, Tablename, ParameterDirection.Input);

                #endregion  
                #region Call DB
                DataSet DSMaster = new DataSet();
                string _Json = string.Empty;
                dynamic _responseDynamic;
                string[] TableName = { "MasterResponse", "MasterData" };
                objCommandExecutor.ExecuteDataSet(CommandType.StoredProcedure, "table_operation", DSMaster, TableName, objSpParameters);
                if (DSMaster != null && DSMaster.Tables.Count > 0 && DSMaster.Tables[0].Rows.Count > 0)
                {
                    response.responseCode = Convert.ToInt32(DSMaster.Tables[0].Rows[0]["ResponseCode"]);
                    response.responseMessage = Convert.ToString(DSMaster.Tables[0].Rows[0]["ResponseMessage"]);
                    List<IEnumerable<dynamic>> objRes =new List<IEnumerable<dynamic>>();
                    ResponseClass<IEnumerable<dynamic>> objResponse= new ResponseClass<IEnumerable<dynamic>> () ;
                    System.Data.DataColumn newColumn = new System.Data.DataColumn("BaseImage", typeof(System.String));

                    DSMaster.Tables[1].Columns.Add(newColumn);
                     var img = DSMaster.Tables[1].ToExpandoObjectList();
                    BO.TableOperationResponse b = new TableOperationResponse();
                        objRes.Add(img);
                    
                       if (response.responseCode != 0)
                    {
                        string JSONString = JsonConvert.SerializeObject(objRes);
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
        private byte[] ConvertDataSetToByteArray(DataTable dataTable)
        {
            byte[] binaryDataResult = null;
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter brFormatter = new BinaryFormatter();
                dataTable.RemotingFormat = SerializationFormat.Binary;
                brFormatter.Serialize(memStream, dataTable.Columns["Image"]);
                binaryDataResult = memStream.ToArray();
            }
            return binaryDataResult;
        }
        /*  byte[] ObjectToByteArray(object obj)
          {
              if (obj == null)
                  return null;
              BinaryFormatter bf = new BinaryFormatter();
              using (MemoryStream ms = new MemoryStream())
              {
                  bf.Serialize(ms, obj);
                  return ms.ToArray();
              }
          }
          */ 
        #region Method

        #endregion

        /*

                  public ResponseClass<IEnumerable<dynamic>> Execute(TableOperationDTO ObjMaster)
                {
                    ResponseClass<IEnumerable<dynamic>> response = new ResponseClass<IEnumerable<dynamic>>();
                    IEnumerable<dynamic> exObj;
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
                    Hashtable SettingInfo = new Hashtable();
                    SettingInfo = (Hashtable)_appVariables.AppSetting["Setting"];
                    string Connection = (string)SettingInfo["sZmX"];
                    CommonMethods commonMethods = new CommonMethods(_hostEnvironment, _httpContextAccessor);
                    CommandExecutor objCommandExecutor = new CommandExecutor(Connection, true);
                    SpParameters objSpParameters = new SpParameters(RdbmsType.MySql);
                    #endregion
                    try
                    {

                        //string text = File.ReadAllText(@"column.json");
                        //var playerList = JsonConvert.DeserializeObject<List<TableOperationDTO>>(text);

                        string customColumn = "";
                        string customValue = "";
                        string ExecQuery = "";

                        StringBuilder Query = new StringBuilder();
                        StringBuilder Column = new StringBuilder();
                        string QueryToExecute;
                        int count = 0;
                        if (ObjMaster.Action == "INSERT")
                        {
                            Column.Append("INSERT INTO " + ObjMaster.TableName + "(");
                            Query.Append("VALUES(");
                            foreach (var custom in ObjMaster.Columns)
                            {
                                Column.Append(custom.ColumnName + ',');
                                if (custom.ColumnDataType == 105001 || custom.ColumnDataType == 105006)
                                {
                                    Query.Append("'" + custom.ColumnValue + "',");
                                }
                                else if (custom.ColumnDataType == 105002 || custom.ColumnDataType == 105005)
                                {
                                    // if (long.TryParse(custom.ColumnValue, out long result) == false)
                                    if (decimal.TryParse(custom.ColumnValue, out decimal result) == false)
                                    {
                                        response.responseCode = 0;
                                       response.responseMessage = "Value of Field is not valid !" + custom.ColumnName;
                                       return response;
                                    }
                                    else
                                    {
                                        Query.Append(custom.ColumnValue + ',');
                                    }
                                }
                                else if (custom.ColumnDataType == 105003)
                                {
                                    DateTime dateValue;
                                    if (DateTime.TryParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateShort, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                                    {
                                        DateTime DT = DateTime.ParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateShort, CultureInfo.InvariantCulture);

                                        Query.Append("'" + DT.ToString("yyyy-MM-dd") + "',");
                                    }
                                    else
                                    {
                                        response.responseCode = 0;
                                        response.responseMessage = "Date of Field is not valid !" + custom.ColumnName;
                                       return response;
                                    }
                                }

                                else if (custom.ColumnDataType == 105004)
                                {
                                    DateTime dateValue;
                                    if (DateTime.TryParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateLong, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                                    {
                                        DateTime DT = DateTime.ParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateLong, CultureInfo.InvariantCulture);

                                        Query.Append("'" + DT.ToString("yyyy-MM-dd - HH:mm:ss") + "',");
                                    }
                                    else
                                    {
                                        response.responseCode = 0;
                                        response.responseMessage = "Date of Field is not valid !" + custom.ColumnName;
                                       return response;
                                    }
                                }
                                else if (custom.ColumnDataType == 105007 )
                                    {
                                        Query.Append("'" + custom.ColumnValue + "',");
                                    }

                            }
                            customValue = Query.ToString().TrimEnd(',');
                            customValue = customValue + ");";
                            customColumn = Column.ToString().TrimEnd(',');
                            customColumn = customColumn + ")";
                            ExecQuery = customColumn + customValue;
                        }


                        else if (ObjMaster.Action == "SELECT")
                        {

                        }

                        else if (ObjMaster.Action == "GETBYID")
                        {
                            // Query.Append("SELECT * FROM " + ObjMaster.TableName + " WHERE " + ObjMaster.TableName + "ID = " + ObjMaster.ID + ";") ;
                            Query.Append(" AND MAS." + ObjMaster.TableName + "ID = " + ObjMaster.ID + "; ");
                            ExecQuery = Query.ToString();
                        }

                        else if (ObjMaster.Action == "DELETE")
                        {
                            Query.Append("UPDATE " + ObjMaster.TableName + " SET is_delete = 1 WHERE " + ObjMaster.TableName + "ID = " + ObjMaster.ID + ";");
                            ExecQuery = Query.ToString();
                        }



                        if (ObjMaster.Action == "UPDATE")
                        {
                            Query.Append("UPDATE " + ObjMaster.TableName + " SET ");
                            foreach (var custom in ObjMaster.Columns)
                            {
                                if (custom.ColumnDataType == 105001 || custom.ColumnDataType == 105006)
                                {
                                    Query.Append(custom.ColumnName + " = '" + custom.ColumnValue + "', ");
                                }

                                else if (custom.ColumnDataType == 105002 || custom.ColumnDataType == 105005)
                                {
                                    // if (long.TryParse(custom.ColumnValue, out long result) == false)
                                    if (decimal.TryParse(custom.ColumnValue, out decimal result) == false)
                                    {
                                        response.responseCode = 0;
                                        response.responseMessage = "Value of Field is not valid !" + custom.ColumnName;
                                        return response;
                                    }
                                    else
                                    {
                                        Query.Append(custom.ColumnName + " = '" + custom.ColumnValue + "', ");
                                    }
                                }

                                else if (custom.ColumnDataType == 105003)
                                {
                                    DateTime dateValue;
                                    if (DateTime.TryParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateShort, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                                    {
                                        DateTime DT = DateTime.ParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateShort, CultureInfo.InvariantCulture);

                                        Query.Append(custom.ColumnName + " = '" + DT.ToString("yyyy-MM-dd") + "',");
                                    }
                                    else
                                    {
                                        response.responseCode = 0;
                                      response.responseMessage = "Date of Field is not valid !" + custom.ColumnName;
                                       return response;
                                    }
                                }
                                else if (custom.ColumnDataType == 105004)
                                {
                                    DateTime dateValue;
                                    if (DateTime.TryParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateLong, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                                    {
                                        DateTime DT = DateTime.ParseExact(custom.ColumnValue, ObjMaster.ObjCommon.DateLong, CultureInfo.InvariantCulture);

                                        Query.Append(custom.ColumnName + " = '" + DT.ToString("yyyy-MM-dd - HH:mm:ss") + "',");
                                    }
                                    else
                                    {
                                        response.responseCode = 0;
                                        response.responseMessage = "Date of Field is not valid !" + custom.ColumnName;
                                        return response;
                                    }
                                }


                            }
                            Query.Length -= 2;

                            Query.Append(" WHERE " + ObjMaster.TableName + "ID = " + ObjMaster.ID + ";");

                            ExecQuery = Query.ToString();
                        }


                        QueryToExecute = ExecQuery;

                        string ActionName = ObjMaster.Action;
                        string Tablename = ObjMaster.TableName;
                        #region Parameters
                        objSpParameters.Add("SPQuery", DbType.String, QueryToExecute, ParameterDirection.Input);
                        objSpParameters.Add("SPAction", DbType.String, ActionName, ParameterDirection.Input);
                        objSpParameters.Add("TableName", DbType.String, Tablename, ParameterDirection.Input);

                        #endregion  
                        #region Call DB
                        DataSet DSMaster = new DataSet();
                        string _Json = string.Empty;
                        dynamic _responseDynamic;
                        string[] TableName = { "MasterResponse", "MasterData" };
                        //string TableName = ObjMaster.TableName;
                        //objCommandExecutor.ExecuteDataSet(CommandType.StoredProcedure, "table_operation", DSMaster, TableName, objSpParameters);
                        objCommandExecutor.ExecuteDataSet(CommandType.StoredProcedure, "table_operation", DSMaster, TableName, objSpParameters);
                        if (DSMaster != null && DSMaster.Tables.Count > 0 && DSMaster.Tables[0].Rows.Count > 0)
                        {
                            response.responseCode = Convert.ToInt32(DSMaster.Tables[0].Rows[0]["ResponseCode"]);
                            response.responseMessage = Convert.ToString(DSMaster.Tables[0].Rows[0]["ResponseMessage"]);
                            List<IEnumerable<dynamic>> objRes =new List<IEnumerable<dynamic>>();
                            ResponseClass<IEnumerable<dynamic>> objResponse= new ResponseClass<IEnumerable<dynamic>> () ;
                            var img = DSMaster.Tables[1].ToExpandoObjectList();
                            byte[] image1 = ZipStringToByte(img.ToString());
                            objRes.Add(img);

                               if (response.responseCode != 0)
                            {
                                string JSONString = JsonConvert.SerializeObject(DSMaster.Tables[1]);

                                _responseDynamic = Compress.ZipStringToByte(JSONString);
                                response.responseDynamic = _responseDynamic;
                                response.responseObject = objRes.ToList<IEnumerable<dynamic>>();


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
 */

        public byte[] ZipStringToByte(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {

                    CopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }
        public void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

    }

}
#endregion
