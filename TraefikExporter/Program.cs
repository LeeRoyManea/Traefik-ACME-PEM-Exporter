using TraefikExporter;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<TraefikAcmeExporterService>();

if (string.IsNullOrWhiteSpace(builder.Configuration.GetValue<string>("AcmePath")))
    throw new Exception("AcmePath is empty!");

if (string.IsNullOrWhiteSpace(builder.Configuration.GetValue<string>("ExportPath")))
    throw new Exception("ExportPath is empty!");

if (builder.Configuration.GetValue<int?>("TimeInterval") is null or 0)
    throw new Exception("TimeInterval is empty!");

var app = builder.Build();
app.Run();