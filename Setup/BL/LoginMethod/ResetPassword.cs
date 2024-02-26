using CommonClass.BL;
using CommonClass.BO;
using CommonClass.ITF.BL;
using Dal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Setup.BO.Login;
using Setup.DTO.Login;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using Setup.ITF.Login;
using Newtonsoft.Json;

namespace Setup.BL.LoginMethod
{
    public class ResetPassword : IResetPassword
    {
        #region Common Data
        public IDistributedCache _cache { get; set; }
        private static IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IAppVariables _appVariables;
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        private string _ErrorMessage = string.Empty;
        public ResetPassword(IDistributedCache cache, IHostEnvironment hostEnvironment, IAppVariables appVariables, IHttpContextAccessor httpContextAccessor, IConfiguration con)
        {
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _appVariables = appVariables;
            _configuration = con;
            _cache = cache;
        }
        #endregion


        #region Reset Password Email Send Method
        public ResponseClass<ResetPasswordResponse> SendPasswordResetEmail(ResetPasswordDTO ObjRequest)
        {
            ResponseClass<ResetPasswordResponse> response = new ResponseClass<ResetPasswordResponse>();
            dynamic _responseDynamic;
            try
            {
                Random random = new Random();
                int otpValue = random.Next(100000, 999999);
                string generatedOTP = otpValue.ToString();


                string Email = ObjRequest.Email;
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.Credentials = new NetworkCredential("softcrontechnology@gmail.com", "liii lvmw yosm cgbo");
                    smtpClient.EnableSsl = true;

                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress("softcrontechnology@gmail.com", "Softcron Technology"),
                        Subject = "Password Reset OTP",
                        Body = $"We have received a request to reset the password for your Account.\n" +
                          $"To proceed with this request, we have generated a one-time password (OTP) for you. Please use the following OTP to complete the password reset process:\n\n" +
                         $"OTP: {generatedOTP}\n\n" +
                          $"Please note that this OTP is valid for the next 10 minutes. If you do not use it within this timeframe, you will need to submit another password reset request.\n\n" +
                          $"\n\nBest Regards,\nSoftcron Technology."
                    };

                    // Add recipient
                    mailMessage.To.Add(Email);
                    try
                    {
                        // Send the email
                        smtpClient.Send(mailMessage);

                        string JSONString = JsonConvert.SerializeObject("otp : " +generatedOTP);
                        _responseDynamic = Compress.ZipStringToByte(JSONString);
                        response.responseDynamic = _responseDynamic;
                        response.responseCode = 1;
                        response.responseMessage = "Otp Send Successfully";
                    }
                    catch (SmtpException ex)
                    {
                        // Handle SMTP exceptions
                        response.responseCode = -1;
                        response.responseMessage = $"SMTP error: {ex.Message}";
                    }
                    catch (Exception ex)
                    {
                        // Handle other exceptions
                        response.responseCode = -1;
                        response.responseMessage = $"Error: {ex.Message}";
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                response.responseCode = -1;
                response.responseMessage = $"Error: {ex.Message}";
            }

            return response;
        }
        #endregion
    }
}
