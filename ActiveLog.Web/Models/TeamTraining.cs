namespace ActiveLog.Web.Models;

public class TeamTraining : Training
{
    public int AnzahlTeilnehmer { get; set; }
    public string Mannschaft { get; set; } = string.Empty;

    public override double BerechneKalorien()
    {
        return 0.0;
    }

    public override string GetTrainingInfo()
    {
        return $"{Typ} - {Mannschaft} mit {AnzahlTeilnehmer} Teilnehmern am {Datum:dd.MM.yyyy}";
    }
}
