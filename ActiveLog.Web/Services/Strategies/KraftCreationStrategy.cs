using ActiveLog.Web.Models;

namespace ActiveLog.Web.Services.Strategies;

public class KraftCreationStrategy : ITrainingCreationStrategy
{
    public string TrainingType => "Kraft";

    public Training Create(DateTime datum, int dauerMinuten, string? notizen, Dictionary<string, object>? extraData)
    {
        return new KraftTraining
        {
            Typ = TrainingType,
            Datum = datum,
            DauerMinuten = dauerMinuten,
            Notizen = notizen,
            GesamtGewicht = extraData?.ContainsKey("Gewicht") == true ? Convert.ToDouble(extraData["Gewicht"]) : 0,
            AnzahlSaetze = extraData?.ContainsKey("Saetze") == true ? Convert.ToInt32(extraData["Saetze"]) : 0
        };
    }
}
