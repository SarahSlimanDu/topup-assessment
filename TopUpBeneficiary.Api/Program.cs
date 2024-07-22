using TopUpBeneficiary.Api;
using TopUpBeneficiary.Api.Commons.MappingConfig;
using TopUpBeneficiary.Api.Extension;
using TopUpBeneficiary.Application;
using TopUpBeneficiary.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<Seed>();
builder.Services.AddMappings();
builder.Services.AddApplication(builder.Configuration)
                .AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseExceptionHandler();
app.MapControllers();
app.Run();

