using ContactManager.Endpoints;
using ContactManager.Validators;
using FastEndpoints;
using FastEndpoints.ApiExplorer;
using FastEndpoints.Security;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
var CorsAllowAllPolicy = "_corsAllowAllPolicy";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFastEndpoints();
builder.Services.AddScoped<IValidator<CreateContactRequest>, CreateContactRequestValidator>();

//if (!builder.Environment.IsDevelopment())
//{
//  builder.Services
//    .AddAuthenticationJwtBearer(s => s.SigningKey = "The secret used to sign tokens") //add this
//    .AddAuthorization()
//    .AddFastEndpoints()
//    .AddFastEndpointsApiExplorer() ;
//}

builder.Services.AddCors(options =>
{
  options.AddPolicy(name: CorsAllowAllPolicy,
                    policy =>
                    {
                      policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .WithExposedHeaders("*");
                    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
  app.UseAuthorization();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseFastEndpoints();
app.UseCors(CorsAllowAllPolicy);
app.Run();
