using ActiveLog.Web.Models;

namespace ActiveLog.Web.Services.Strategies;

public interface ITrainingCreationStrategy
{
    string TrainingType { get; }
    Training Create(DateTime datum, int dauerMinuten, string? notizen, Dictionary<string, object>? extraData);
}
