namespace ActiveLog.Web.Models;

public class CardioTraining : Training
{
    public double Distanz { get; set; }
    public double DurchschnittsGeschwindigkeit { get; set; }

    public override double BerechneKalorien()
    {
        return Distanz * 60.0;
    }

    public override string GetTrainingInfo()
    {
        return $"{Typ} - {Distanz} km in {DauerMinuten} Min am {Datum:dd.MM.yyyy}";
    }
}
