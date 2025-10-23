namespace ActiveLog.Web.Models;

public class Uebung
{
    public int Id { get; set; }
    public int TrainingId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Saetze { get; set; }
    public int Wiederholungen { get; set; }
    public double? Gewicht { get; set; }
}
