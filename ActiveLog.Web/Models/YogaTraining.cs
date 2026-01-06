namespace ActiveLog.Web.Models;

public class YogaTraining : Training
{
    public string Stil { get; set; } = string.Empty;
    public int Schwierigkeitsgrad { get; set; }

    public override double BerechneKalorien()
    {
        return DauerMinuten * 3.5;
    }

    public override string GetTrainingInfo()
    {
        return $"{Typ} ({Stil}) - Level {Schwierigkeitsgrad} - {DauerMinuten} Min am {Datum:dd.MM.yyyy}";
    }
}
