using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using husarbeid.Data;
using husarbeid.DataLoader;
using husarbeid.Families;
using husarbeid.FamilyTasks;
using husarbeid.Types;
using husarbeid.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace husarbeid
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
            services.AddPooledDbContextFactory<ApplicationDbContext>(
                  options => options.UseSqlite(Configuration.GetConnectionString("Sqlite")));

            var secret = Configuration.GetSection("JwtConfig").GetSection("secret").Value;
            var key = Encoding.ASCII.GetBytes(secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidIssuer = Configuration.GetSection("JwtConfig").GetSection("Issuer").Value,
                    ValidAudience = Configuration.GetSection("JwtConfig").GetSection("Audience").Value,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services
                .AddGraphQLServer()
                .AddQueryType(d => d.Name("Query"))
                   .AddTypeExtension<UserQueries>()
                   .AddTypeExtension<FamilyQueries>()
                   .AddTypeExtension<FamilyTaskQueries>()
                .AddMutationType(d => d.Name("Mutation"))
                   .AddTypeExtension<UserMutations>()
                   .AddTypeExtension<FamilyMutations>()
                   .AddTypeExtension<FamilyTaskMutations>()
                .AddType<UserType>()
                .AddType<FamilyType>()
                .AddType<FamilyTaskType>()
                .EnableRelaySupport()
                .AddDataLoader<UserByIdDataLoader>()
                .AddDataLoader<FamilyByIdDataLoader>()
                .AddDataLoader<FamilyTaskByIdDataLoader>()
                .AddHttpRequestInterceptor((ctx, executor, builder, ct) =>
                    {
                        var identity = new ClaimsIdentity();
                        ctx.User.AddIdentity(identity);
                        return ValueTask.CompletedTask;
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
