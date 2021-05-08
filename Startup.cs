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
using Authentication;

namespace husarbeid
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _key = Encoding.ASCII.GetBytes(Configuration.GetSection("JwtConfig").GetSection("secret").Value);
        }
        public IConfiguration Configuration { get; }
        public byte[] _key { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPooledDbContextFactory<ApplicationDbContext>(
                  options => options.UseSqlite(Configuration.GetConnectionString("Sqlite")));


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddScoped<TokenService>();
            services.AddAuthorization();

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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
