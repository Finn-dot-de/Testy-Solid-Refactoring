using ActiveLog.Web.Data;
using ActiveLog.Web.Models;

namespace ActiveLog.Web.Services;

public class TrainingService : ITrainingService
{
    private readonly ITrainingRepository _repository;
    private readonly TrainingFactory _factory;
    private readonly TrainingValidator _validator;

    public TrainingService(ITrainingRepository repository, TrainingFactory factory, TrainingValidator validator)
    {
        _repository = repository;
        _factory = factory;
        _validator = validator;
    }

    public Training CreateTraining(string typ, DateTime datum, int dauerMinuten, string? notizen, Dictionary<string, object>? extraData = null)
    {
        return _factory.CreateTraining(typ, datum, dauerMinuten, notizen, extraData);
    }

    public void SaveTraining(Training training)
    {
        if (!_validator.Validate(training))
        {
            throw new ArgumentException("Training ist ungültig!");
        }

        _repository.Add(training);
    }

    public List<Training> GetAllTrainings()
    {
        return _repository.GetAll();
    }

    public void DeleteTraining(int id)
    {
        _repository.Delete(id);
    }
    
    public bool ValidateTraining(Training training) 
    {
        return _validator.Validate(training);
    }

    public Dictionary<string, object> GetStatistiken() => throw new NotSupportedException("Use StatisticsService directly");
    public string ExportTrainings(string format) => throw new NotSupportedException("Use TrainingExporter directly");
}