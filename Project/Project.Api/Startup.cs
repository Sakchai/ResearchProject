
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Project.Entity.UnitofWork;
using Project.Entity.Context;
using AutoMapper;
using Project.Domain.Mapping;
using Project.Domain.Service;
using FluentMigrator.Runner;
using Project.Entity.Migrations;
using Swashbuckle.AspNetCore.Swagger;

/// <summary>
/// Designed by AnaSoft Inc. 2018
/// http://www.anasoft.net/apincore 
/// NOTE:
/// Must update database connection in appsettings.json - "Project.ApiDB"
/// </summary>

namespace Project.Api
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //db service
            //provides connection string for ProjectContext with Dapper implementation
            services.AddTransient<ProjectContext, ProjectContext>(sp =>
            {
                return new ProjectContext(Configuration["ConnectionStrings:Project.API"]);
            });

            //fluent migration services
            services.AddFluentMigratorCore().
                     ConfigureRunner(rb => rb
                    .WithGlobalConnectionString(Configuration["ConnectionStrings:Project.API.Migration"])
                    .AddSqlServer2012()
                    .WithMigrationsIn(new System.Reflection.Assembly[] { typeof(Migration_Initial).Assembly })
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(Migration_Initial).Assembly).For.Migrations())
                // Build the service provider
                .BuildServiceProvider(false);


            #region "Authentication"

            if (Configuration["Authentication:UseIndentityServer4"] == "False")
            {
                //API authentication service
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = Configuration["Jwt:Issuer"],
                            ValidAudience = Configuration["Jwt:Issuer"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                        };
                    }
                 );
            }
            else
            {
                //Indentity Server 4 API authentication service
                services.AddAuthorization();
                //.AddJsonFormatters();
                services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(option =>
                {
                    option.Authority = Configuration["Authentication:IndentityServer4IP"];
                    option.RequireHttpsMetadata = false;
                    //option.ApiSecret = "secret";
                    option.ApiName = "Project API";  //This is the resourceAPI that we defined in the Config.cs in the AuthServ project (apiresouces.json and clients.json). They have to be named equal."
                });

            }

            #endregion

            // include support for CORS
            // More often than not, we will want to specify that our API accepts requests coming from other origins (other domains). When issuing AJAX requests, browsers make preflights to check if a server accepts requests from the domain hosting the web app. If the response for these preflights don't contain at least the Access-Control-Allow-Origin header specifying that accepts requests from the original domain, browsers won't proceed with the real requests (to improve security).
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy-public",
                    builder => builder.AllowAnyOrigin()   //WithOrigins and define a specific origin to be allowed (e.g. https://mydomain.com)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                .Build());
            });

            //mvc service
            services.AddMvc();

            #region "DI code"

            //general unitofwork and repository injections
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            //services injections
            services.AddTransient(typeof(AccountService<,>), typeof(AccountService<,>));
            services.AddTransient(typeof(UserService<,>), typeof(UserService<,>));

            services.AddTransient(typeof(AccountServiceAsync<,>), typeof(AccountServiceAsync<,>));
            services.AddTransient(typeof(UserServiceAsync<,>), typeof(UserServiceAsync<,>));
            //services.AddTransient(typeof(RoleServiceAsync<,>), typeof(RoleServiceAsync<,>));
            //...add other services
            //
            services.AddTransient(typeof(IService<,>), typeof(GenericService<,>));
            services.AddTransient(typeof(IServiceAsync<,>), typeof(GenericServiceAsync<,>));

            services.AddTransient(typeof(RoleService), typeof(RoleService));

            #endregion

            //data mapper profiler setting
            Mapper.Initialize((config) =>
            {
                config.AddProfile<MappingProfile>();
            });

            //Swagger API documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Project.API", Version = "v1" });
            });

        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseMiddleware<ExceptionHandler>();

            app.UseAuthentication(); //needs to be up in the pipeline, before MVC
            app.UseCors("CorsPolicy-public");  //apply to every request
            app.UseMvc();

            //Swagger API documentation
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project.API V1");
            });

            //migration and seeding
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                if (Configuration["ConnectionStrings:UseMigrationService"] == "True")
                {
                    var runner = serviceScope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                    DatabaseHelper.CreateIfNotExists(Configuration["ConnectionStrings:Project.B"]);  //Create database process must be outside migration transaction
                    runner.MigrateUp();
                }
            }

        }

    }
}







