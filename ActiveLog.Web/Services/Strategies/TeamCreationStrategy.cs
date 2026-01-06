using ActiveLog.Web.Models;

namespace ActiveLog.Web.Services.Strategies;

public class TeamCreationStrategy : ITrainingCreationStrategy
{
    public string TrainingType => "Team";

    public Training Create(DateTime datum, int dauerMinuten, string? notizen, Dictionary<string, object>? extraData)
    {
        return new TeamTraining
        {
            Typ = TrainingType,
            Datum = datum,
            DauerMinuten = dauerMinuten,
            Notizen = notizen,
            AnzahlTeilnehmer = extraData?.ContainsKey("Teilnehmer") == true ? Convert.ToInt32(extraData["Teilnehmer"]) : 0,
            Mannschaft = extraData?.ContainsKey("Mannschaft") == true ? extraData["Mannschaft"].ToString() ?? "" : ""
        };
    }
}
