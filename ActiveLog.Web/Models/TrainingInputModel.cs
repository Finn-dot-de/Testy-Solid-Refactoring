namespace ActiveLog.Web.Models;

public class TrainingInputModel
{
    public string Typ { get; set; } = string.Empty;
    public DateTime Datum { get; set; }
    public int DauerMinuten { get; set; }
    public string? Notizen { get; set; }
}
