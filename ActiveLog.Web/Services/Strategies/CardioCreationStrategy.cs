using ActiveLog.Web.Models;

namespace ActiveLog.Web.Services.Strategies;

public class CardioCreationStrategy : ITrainingCreationStrategy
{
    public string TrainingType => "Cardio";

    public Training Create(DateTime datum, int dauerMinuten, string? notizen, Dictionary<string, object>? extraData)
    {
        return new CardioTraining
        {
            Typ = TrainingType,
            Datum = datum,
            DauerMinuten = dauerMinuten,
            Notizen = notizen,
            Distanz = extraData?.ContainsKey("Distanz") == true ? Convert.ToDouble(extraData["Distanz"]) : 0,
            DurchschnittsGeschwindigkeit = extraData?.ContainsKey("Geschwindigkeit") == true ? Convert.ToDouble(extraData["Geschwindigkeit"]) : 0
        };
    }
}
