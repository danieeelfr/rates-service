using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Swashbuckle.AspNetCore.Swagger;
using Services;
using Microsoft.OpenApi.Models;
using AutoMapper;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace RatesAPI
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
            services.AddTransient<ILoginService, LoginService>();

            services.AddCors();
            services.AddControllers();

            var key = Configuration.GetValue<string>("JwtToken:SecretKey");
            var secret = Encoding.ASCII.GetBytes(key);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllParametersInCamelCase();
                options.SwaggerDoc("v0", new OpenApiInfo
                {
                    Title = "RatesAPI",
                    Version = "v0",
                    Description = "RatesAPI - Softplan challenge",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Daniel Fonseca Reis",
                        Email = "danielfonsecareis@gmail.com",
                        Url = new Uri("https://example.com/terms"),
                    }
                });

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {

                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/v0/swagger.json", "RatesAPI");
            });
        }
    }
}
