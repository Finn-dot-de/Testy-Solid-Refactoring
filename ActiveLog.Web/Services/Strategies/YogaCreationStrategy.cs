using ActiveLog.Web.Models;

namespace ActiveLog.Web.Services.Strategies;

public class YogaCreationStrategy : ITrainingCreationStrategy
{
    public string TrainingType => "Yoga";

    public Training Create(DateTime datum, int dauerMinuten, string? notizen, Dictionary<string, object>? extraData)
    {
        return new YogaTraining
        {
            Typ = TrainingType,
            Datum = datum,
            DauerMinuten = dauerMinuten,
            Notizen = notizen,
            Stil = extraData?.ContainsKey("Stil") == true ? extraData["Stil"].ToString() ?? "" : "Standard",
            Schwierigkeitsgrad = extraData?.ContainsKey("Schwierigkeitsgrad") == true ? Convert.ToInt32(extraData["Schwierigkeitsgrad"]) : 1
        };
    }
}
