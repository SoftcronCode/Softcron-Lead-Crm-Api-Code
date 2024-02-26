using CommonClass.BL;
using CommonClass.ITF.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Setup.ITF;
using Setup.BL;
using Setup.ITF.UserManagment;
using Setup.BL.UserManagment;
using Setup.ITF.Lead;
using Setup.BL.Lead;
using Setup.ITF.Customer;
using Setup.BL.Customer;
using Setup.ITF.Login;
using Setup.BL.LoginMethod;
using Setup.ITF.Invoice;
using Setup.BL.Invoice;

namespace DSERP_API.Extension
{
    public static class ServiceExtension
    {

        public static IServiceCollection RegisterService(this IServiceCollection services)
        {
            #region AddScoped
            #endregion
            #region Transient
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAppVariables, AppVariables>();
       
            #endregion

            #region Setup
            services.AddScoped<ILogin, Login>();
            services.AddScoped<IExtensionDBMaster, ExtensionDBMaster>();
            services.AddScoped<IMasterDataBinding, MasterDataBinding>();
            services.AddScoped<IClient, Client>();
            services.AddScoped<IUser, User>();
            services.AddScoped<ITableOperation, TableOperation>();
            services.AddScoped<IUserManagment, UserManagment>();
            services.AddScoped<IResetPassword, ResetPassword>();
            #endregion

            #region Lead
            services.AddScoped<ILeadReport, LeadReporting>();
            services.AddScoped<IFollowup, Followup>();
            services.AddScoped<IQuotation, Quotation>();
            services.AddScoped<INotes, Notes>();

            #endregion

            #region Customer
            services.AddScoped<IReporting, Reporting>();
            services.AddScoped<IPaymentReport, PaymentReport>();
            services.AddScoped<ISalesReport, SalesReport>();
            services.AddScoped<IReminders, Reminder>();
            services.AddScoped<IInvoice, Invoice>();
            #endregion


            return services;
        }
    }
}
