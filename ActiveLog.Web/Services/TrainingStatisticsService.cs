using ActiveLog.Web.Models;

namespace ActiveLog.Web.Services;

public class TrainingStatisticsService
{
    public Dictionary<string, object> Calculate(List<Training> trainings)
    {
        return new Dictionary<string, object>
        {
            { "GesamtAnzahl", trainings.Count },
            { "GesamtDauer", trainings.Sum(t => t.DauerMinuten) },
            { "DurchschnittsDauer", trainings.Any() ? trainings.Average(t => t.DauerMinuten) : 0 },
            { "GesamtKalorien", trainings.Sum(t => t.BerechneKalorien()) },
            { "TrainingsProTyp", trainings.GroupBy(t => t.Typ).ToDictionary(g => g.Key, g => g.Count()) }
        };
    }
}