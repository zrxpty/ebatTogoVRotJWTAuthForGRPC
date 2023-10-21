using GrpcServer;
using GrpcServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
builder.Services.AddCors();
// Add services to the container.
builder.Services
  .AddAuthorization()
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
    options.SaveToken = true;
    options.RequireHttpsMetadata = true;

    options.TokenValidationParameters = new()
    {
      ValidateIssuer = false,
      ValidateAudience = false,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidIssuer = JwtAuthenticationManager.Issuer,
      ValidAudience = JwtAuthenticationManager.Audience,
      IssuerSigningKey = JwtAuthenticationManager.SymmetricSecurityKey,
    };
  });

builder.Services.AddGrpc();

var app = builder.Build();
app.UseCors(policy =>
{
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});
app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<AuthenticationService>();
app.MapGrpcService<CalculationService>();

app.MapGet("/", () => "Hello, World!");

await app.RunAsync();
