namespace ActiveLog.Web.Models;

public class KraftTraining : Training
{
    public double GesamtGewicht { get; set; }
    public int AnzahlSaetze { get; set; }

    public override double BerechneKalorien()
    {
        return AnzahlSaetze * 30.0;
    }

    public override string GetTrainingInfo()
    {
        return $"{Typ} - {AnzahlSaetze} Sätze, {GesamtGewicht} kg am {Datum:dd.MM.yyyy}";
    }
}
