using ActiveLog.Web.Models;

namespace ActiveLog.Web.Services;

public interface ITrainingService
{
    Training CreateTraining(string typ, DateTime datum, int dauerMinuten, string? notizen, Dictionary<string, object>? extraData = null);
    void SaveTraining(Training training);
    List<Training> GetAllTrainings();
    Dictionary<string, object> GetStatistiken();
    string ExportTrainings(string format);
    void DeleteTraining(int id);
    bool ValidateTraining(Training training);
}