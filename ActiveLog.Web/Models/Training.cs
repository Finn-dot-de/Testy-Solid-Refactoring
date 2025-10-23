namespace ActiveLog.Web.Models;

public class Training
{
    public int Id { get; set; }
    public DateTime Datum { get; set; }
    public string Typ { get; set; } = string.Empty;
    public int DauerMinuten { get; set; }
    public string? Notizen { get; set; }
    public int? ZielId { get; set; }

    public virtual double BerechneKalorien()
    {
        return DauerMinuten * 5.0;
    }

    public virtual string GetTrainingInfo()
    {
        return $"{Typ} - {DauerMinuten} Min am {Datum:dd.MM.yyyy}";
    }
}
