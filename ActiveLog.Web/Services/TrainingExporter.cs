using System.Text;
using System.Text.Json;
using ActiveLog.Web.Models;

namespace ActiveLog.Web.Services;

public class TrainingExporter
{
    public string Export(List<Training> trainings, string format)
    {
        return format.ToLower() switch
        {
            "csv" => ExportToCsv(trainings),
            "json" => JsonSerializer.Serialize(trainings, new JsonSerializerOptions { WriteIndented = true }),
            _ => throw new ArgumentException($"Format {format} wird nicht unterstützt")
        };
    }

    private string ExportToCsv(List<Training> trainings)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Id,Datum,Typ,Dauer (Min),Notizen");
        foreach (var t in trainings)
        {
            sb.AppendLine($"{t.Id},{t.Datum:yyyy-MM-dd},{t.Typ},{t.DauerMinuten},\"{t.Notizen}\"");
        }
        return sb.ToString();
    }
}