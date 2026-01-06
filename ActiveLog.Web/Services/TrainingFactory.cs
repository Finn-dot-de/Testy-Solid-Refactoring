using ActiveLog.Web.Models;
using ActiveLog.Web.Services.Strategies;

namespace ActiveLog.Web.Services;

public class TrainingFactory
{
    private readonly Dictionary<string, ITrainingCreationStrategy> _strategies;

    public TrainingFactory(IEnumerable<ITrainingCreationStrategy> strategies)
    {
        _strategies = strategies.ToDictionary(s => s.TrainingType, s => s);
    }

    public Training CreateTraining(string typ, DateTime datum, int dauerMinuten, string? notizen, Dictionary<string, object>? extraData = null)
    {
        if (_strategies.TryGetValue(typ, out var strategy))
        {
            return strategy.Create(datum, dauerMinuten, notizen, extraData);
        }

        return new Training
        {
            Typ = typ,
            Datum = datum,
            DauerMinuten = dauerMinuten,
            Notizen = notizen
        };
    }
}
