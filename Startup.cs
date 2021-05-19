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
            services.AddHttpContextAccessor();

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

            services.AddSingleton<ITokenService, TokenService>();
            //services.AddScoped<IUserRepository, UserRepository>();

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
                .AddHttpRequestInterceptor((context, executor, builder, ct) =>
                    {
                        string userId = "";
                        if (context.User.Identity.IsAuthenticated)
                        {
                            userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                        }
                        builder.AddProperty("currentUserId", string.IsNullOrEmpty(userId) ? null : Int32.Parse(userId));
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
