using CommonClass.BL;
using CommonClass.BO;
using CommonClass.ExceptionHandling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using CommonClass.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using DSERP_API.Extension;

namespace DSERP_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ExceptionHandler));

            }
            )
      .SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DSERP_API_CONTROLLER", Version = "v1" });
            });
           
            services.AddSingleton<IConfiguration>(Configuration);
            services.Configure<AppVariables>(Configuration);
            services.RegisterService();
            //services.AddScoped<ActionFilterExample>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
               builder =>
               {
                   builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
               });
            });
            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = actionContext =>
                {
                    return CustomErrorResponse(actionContext);

                };
            });
            services.AddMvc();
            services.AddControllers();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DSERP_API_CONTROLLER v1"));
            app.UseRouting();
            app.UseCors("AllowAllHeaders");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private BadRequestObjectResult CustomErrorResponse(ActionContext actionContext)
        {
            ResponseClass<Error> response = new ResponseClass<Error>();
            //BadRequestObjectResult is class found Microsoft.AspNetCore.Mvc and is inherited from ObjectResult.    
            //Rest code is linq.    
            response.responseCode = 0;

            response.responseObject = (actionContext.ModelState
             .Where(modelError => modelError.Value.Errors.Count > 0)
             .Select(modelError => new Error
             {
                 ErrorField = modelError.Key,
                 ErrorDescription = modelError.Value.Errors.FirstOrDefault().ErrorMessage

             }).ToList());
            response.responseMessage = "Validation -- " + response.responseObject[0].ErrorDescription;
            return new BadRequestObjectResult(response);
        }
        public class Error
        {
            public string ErrorField { get; set; }
            public string ErrorDescription { get; set; }
        }
    }
}
