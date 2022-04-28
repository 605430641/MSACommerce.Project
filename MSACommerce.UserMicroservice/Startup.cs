using AgileFramework.Common.IOCOptions;
using AgileFramework.WebCore.FilterExtend;
using AgileFramework.WebCore.MiddlewareExtend;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MSACommerce.Core;
using MSACommerce.Interface;
using MSACommerce.Model;
using MSACommerce.Service;
using System.Text;

namespace MSACommerce.UserMicroservice
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
            #region Filter
            services.AddControllers(o =>
            {
                
                o.Filters.Add(typeof(CustomExceptionFilterAttribute));
                o.Filters.Add(typeof(LogActionFilterAttribute));
            });
            #endregion

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MSACommerce.UserMicroservice", Version = "v1" });
            });

            #region 服务注入
            services.AddTransient<OrangeContext, OrangeContext>();
            services.AddTransient<CacheClientDB, CacheClientDB>();
            services.AddTransient<IUserService, UserService>();
            #endregion

            #region 配置文件注入
            services.Configure<MySqlConnOptions>(this.Configuration.GetSection("MysqlConn"));
            services.Configure<RedisConnOptions>(this.Configuration.GetSection("RedisConn"));
            #endregion

            #region jwt校验  HS
            JWTTokenOptions tokenOptions = new JWTTokenOptions();
            Configuration.Bind("JWTTokenOptions", tokenOptions);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//Scheme
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //JWT有一些默认的属性，就是给鉴权时就可以筛选了
                    ValidateIssuer = true,//是否验证Issuer
                    ValidateAudience = true,//是否验证Audience
                    ValidateLifetime = false,//是否验证失效时间
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    ValidAudience = tokenOptions.Audience,//
                    ValidIssuer = tokenOptions.Issuer,//Issuer，这两项和前面签发jwt的设置一致
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))
                };
            });
            #endregion
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("default", policy =>
            //    {
            //        //policy.WithOrigins(new string[] { "http://www.yitao.com", "http://api.yitao.com" })
            //        policy.AllowAnyOrigin()
            //            .AllowAnyHeader()
            //            .AllowAnyMethod();
            //        //.AllowCredentials();
            //    });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MSACommerce.UserMicroservice v1"));
            }
            app.UsePreOptionsRequest();

            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            //app.UseCors("default");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
