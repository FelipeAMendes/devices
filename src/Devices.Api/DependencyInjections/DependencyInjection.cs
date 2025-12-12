using Devices.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Devices.Api.DependencyInjections;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.ConfigureDefaultErrors();

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.ConfigureSwaggerEndpoint();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.MapControllers();

        return app;
    }

    private static void ConfigureDefaultErrors(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var response = new ApiErrorResponse
                {
                    Message = "Validation error",
                    Errors = [.. context.ModelState
                        .Where(ms => ms.Value?.Errors.Count > 0)
                        .Select(ms => new ApiErrorDetail
                        {
                            Field = ms.Key,
                            Message = ms.Value?.Errors.FirstOrDefault()?.ErrorMessage
                        })]
                };

                return new BadRequestObjectResult(response);
            };
        });
    }

    private static void ConfigureSwaggerEndpoint(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/")
            {
                context.Response.Redirect("/swagger");
                return;
            }

            await next();
        });
    }
}
