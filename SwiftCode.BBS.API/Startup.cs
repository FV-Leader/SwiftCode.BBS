using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;

namespace SwiftCode.BBS.API
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

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v0.1.0",
                    Title = "SwiftCode.BBS.API",
                    Description = "���˵���ĵ�",
                    Contact = new OpenApiContact
                    {
                        Name = "SwiftCode",
                        Email = "SwiftCode@xxx.com",
                    }
                });

                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "SwiftCode.BBS.API.xml");//������Ǹո����õ�xml�ļ���
                c.IncludeXmlComments(xmlPath, true);


                var xmlModelPath = Path.Combine(basePath, "SwiftCode.BBS.Model.xml");//�������Model���xml�ļ���
                c.IncludeXmlComments(xmlModelPath);

            });
           
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp  v1");
                c.RoutePrefix = ""; //·�����ã�����Ϊ�գ���ʾֱ���ڸ�������localhost:8001�����ʸ��ļ�,ע��localhost:8001/swagger�Ƿ��ʲ����ģ�ȥlaunchSettings.json��launchUrlȥ��
            });
            #endregion

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
