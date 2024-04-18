using TraefikAcmeExport;

namespace TraefikExporter;

public class TraefikAcmeExporterService : BackgroundService
{
    private readonly PeriodicTimer _pdfImportTimer;
    private string acmePath { get; set; }
    private string exportPath { get; set; }
    private DateTime Modified { get; set; }

    public TraefikAcmeExporterService(IConfiguration configuration)
    {
        acmePath = configuration.GetValue<string>("AcmePath");
        exportPath = configuration.GetValue<string>("ExportPath");
        _pdfImportTimer = new(TimeSpan.FromMinutes(configuration.GetValue<int>("TimeInterval")));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        TraefikAcmeExporter exporter = new TraefikAcmeExporter();
        while (await _pdfImportTimer.WaitForNextTickAsync(cancellationToken: stoppingToken) &&
               !stoppingToken.IsCancellationRequested)
        {
            if (!File.Exists(acmePath))
            {
                Console.Error.WriteLine($"File does not exists {acmePath}");
                continue;
            }
            
            if (File.GetLastWriteTime(acmePath) is { } lastModified && lastModified.Ticks > Modified.Ticks)
            {
                Console.WriteLine($"Starting new export");
                Modified = lastModified;
                var jsonContent = await File.ReadAllTextAsync(acmePath, stoppingToken);
                exporter.ExportAcmeFile(jsonContent, exportPath);
            }
            else
            {
                Console.WriteLine($"No new changes since {Modified:hh:mm:ss - dd.MM.yyyy}");
            }
        }
    }
}