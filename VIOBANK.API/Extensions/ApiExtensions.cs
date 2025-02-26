using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VIOBANK.Application.Services;
using VIOBANK.Domain.Enums;
using VIOBANK.Infrastructure;

namespace VIOBANK.API.Extensions
{
    public static class ApiExtensions
    {
        public static void AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtOptions");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    //ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = false,
                    //ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["tasty-cookies"];
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddScoped<PermissionService>();
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddAuthorization(options =>
            {
                foreach (PermissionEnum permission in Enum.GetValues(typeof(PermissionEnum)))
                {
                    options.AddPolicy($"Permissions:{permission}", policy =>
                        policy.Requirements.Add(new PermissionRequirement(new[] { permission })));
                }
            });
        }

        public static IEndpointConventionBuilder RequirePermissions<TBuilder>(
            this TBuilder builder, params PermissionEnum[] permissions)
                where TBuilder : IEndpointConventionBuilder
        {
            return builder.RequireAuthorization(policy => policy.AddRequirements(new PermissionRequirement(permissions)));
        }
            
    }

    

}
