using FluentValidation;
using EntityApp.Api.Validators;
using EntityApp.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<CreateEntityValidator>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IEntityService, EntityService>();
builder.Services.AddScoped<IEntityFormatter, EntityFormatter>();
builder.Services.AddScoped<IReportWebhookSender, ReportWebhookSender>();
builder.Services.AddScoped<IExportService, ExportService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();