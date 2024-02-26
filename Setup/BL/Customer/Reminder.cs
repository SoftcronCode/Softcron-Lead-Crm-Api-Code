using CommonClass.BL;
using CommonClass.BO;
using CommonClass.ITF.BL;
using Dal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Setup.BO.Customer;
using Setup.DTO.Customer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Setup.ITF.Customer;
using Org.BouncyCastle.Crypto.Tls;
using System.Security.Claims;

namespace Setup.BL.Customer
{
    public class Reminder : IReminders
    {
        #region Common Data
        public IDistributedCache _cache { get; set; }
        private static IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IAppVariables _appVariables;
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        private string _ErrorMessage = string.Empty;
        public Reminder(IDistributedCache cache, IHostEnvironment hostEnvironment, IAppVariables appVariables, IHttpContextAccessor httpContextAccessor, IConfiguration con)
        {
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _appVariables = appVariables;
            _configuration = con;
            _cache = cache;
        }
        #endregion

        #region Sales Reporting Method
        public ResponseClass<RemindersResponse> BirthdayAndValidityReminders(RemindersDTO ObjRequest)
        {
            ResponseClass<RemindersResponse> response = new ResponseClass<RemindersResponse>();

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
                string[] TableName = { "Response Table", "Birthday Data", "Validity Data" };
                objCommandExecutor.ExecuteDataSet(CommandType.StoredProcedure, "birthday_and_validity_reminders", dsLogin, TableName, objSpParameters);

                if (dsLogin != null && dsLogin.Tables.Count > 0 && dsLogin.Tables[0].Rows.Count > 0)
                {
                    response.responseCode = Convert.ToInt32(dsLogin.Tables[0].Rows[0]["ResponseCode"]);
                    response.responseMessage = Convert.ToString(dsLogin.Tables[0].Rows[0]["ResponseMessage"]);

                    if (response.responseCode != 0)
                    {
                        if (dsLogin.Tables.Count > 1 && dsLogin.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow row in dsLogin.Tables[1].Rows)
                            {
                                string customerName = row["Customer_name"].ToString();
                                string customerEmail = row["Customer_email"].ToString();

                                // Call a method to send email
                                SendBirthdayEmail(customerName, customerEmail);
                            }
                        }
                        if (dsLogin.Tables.Count > 2 && dsLogin.Tables[2].Rows.Count > 0)
                        {
                            foreach (DataRow row in dsLogin.Tables[2].Rows)
                            {
                                string customerName = row["Customer_name"].ToString();
                                string customerEmail = row["Customer_email"].ToString();
                                string productName = row["product_name"].ToString();
                                string productClass = row["product_class"].ToString();
                                string productValidity = row["product_validity"].ToString();
                                string expiryDateStr = row["expiry_date"].ToString();
                                if (DateTime.TryParse(expiryDateStr, out DateTime expiryDate))
                                {
                                    DateTime currentDate = DateTime.Now.Date;  // Use Date property to ignore the time portion
                                    TimeSpan dateDifference = expiryDate.Date - currentDate;

                                    string expiryTime;

                                    if (dateDifference.Days == 7)
                                    {
                                        expiryTime = "in next 7 days";
                                    }
                                    else if (dateDifference.Days == 15)
                                    {
                                        expiryTime = "in next 15 days";
                                    }
                                    else if (dateDifference.Days == 0)
                                    {
                                        expiryTime = "today";
                                    }
                                    else
                                    {
                                        // If the difference is not 7, 15, or 0 days, set time to a default value or handle it accordingly
                                        expiryTime = "in some days";
                                    }

                                    // Call a method to send email with the calculated time
                                    SendValidityEmail(customerName, customerEmail, productName, productClass, productValidity, expiryTime);
                                }
                            }
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


        #region Method to send birthday email

        private void SendBirthdayEmail(string customerName, string customerEmail)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.Credentials = new NetworkCredential("softcrontechnology@gmail.com", "liii lvmw yosm cgbo");
                    smtpClient.EnableSsl = true;

                    // Create the email message
                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress("softcrontechnology@gmail.com", "Softcron Technology"),
                        Subject = "Birthday Greetings",
                        Body = $"Dear {customerName},\n\nHappy Birthday!\n\nBest Regards,\nSoftcron Technology."
                    };

                    // Add recipient
                    mailMessage.To.Add(customerEmail);
                    try
                    {
                        // Send the email
                        smtpClient.Send(mailMessage);
                    }
                    catch (Exception)
                    {
                        throw;
                    }                   
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region Method to send birthday email

        private void SendValidityEmail(string customerName, string customerEmail, string productName, string productClass, string productValidity, string expiryTime)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.Credentials = new NetworkCredential("softcrontechnology@gmail.com", "liii lvmw yosm cgbo");
                    smtpClient.EnableSsl = true;

                    // Create the email message
                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress("softcrontechnology@gmail.com", "Softcron Technology"),
                        Subject = "Product Validity Expiry",
                        Body = $"Dear {customerName},\n\n your product {productName} {productClass} with a validity of {productValidity} has expired {expiryTime}. To renew the product validity, please contact us.\n\nBest Regards,\nSoftcron Technology."
                    }; 

                    // Add recipient
                    mailMessage.To.Add(customerEmail);
                    try
                    {
                        // Send the email
                        smtpClient.Send(mailMessage);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

    }
}
