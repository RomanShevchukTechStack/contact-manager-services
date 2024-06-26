using ContactManager.Endpoints;
using ContactManager.Validators;
using FastEndpoints;
using FluentValidation;
var CorsAllowAllPolicy = "_corsAllowAllPolicy";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFastEndpoints();
builder.Services.AddScoped<IValidator<CreateContactRequest>, CreateContactRequestValidator>();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseFastEndpoints();
app.UseCors(CorsAllowAllPolicy);
app.Run();
