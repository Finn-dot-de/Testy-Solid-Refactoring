using ActiveLog.Web.Models;
using ActiveLog.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace ActiveLog.Web.Controllers;

public class TrainingController : Controller
{
    private readonly ITrainingService _service;
    private readonly TrainingExporter _exporter;
    private readonly TrainingStatisticsService _statsService;

    public TrainingController(
        ITrainingService service, 
        TrainingExporter exporter, 
        TrainingStatisticsService statsService)
    {
        _service = service;
        _exporter = exporter;
        _statsService = statsService;
    }

    public IActionResult Index()
    {
        var trainings = _service.GetAllTrainings();
        return View(trainings);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(TrainingInputModel model, IFormCollection form)
    {
        var extraData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        foreach (var key in form.Keys)
        {
            if (key != nameof(model.Typ) && 
                key != nameof(model.Datum) && 
                key != nameof(model.DauerMinuten) && 
                key != nameof(model.Notizen) && 
                key != "__RequestVerificationToken")
            {
                extraData[key] = form[key];
            }
        }

        if (extraData.ContainsKey("Distanz") && model.DauerMinuten > 0)
        {
             double dist = Convert.ToDouble(extraData["Distanz"]);
             extraData["Geschwindigkeit"] = dist / (model.DauerMinuten / 60.0);
        }

        var training = _service.CreateTraining(model.Typ, model.Datum, model.DauerMinuten, model.Notizen, extraData);
        _service.SaveTraining(training);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Statistics()
    {
        var trainings = _service.GetAllTrainings();
        var stats = _statsService.Calculate(trainings);
        return View(stats);
    }

    public IActionResult Export(string format)
    {
        var trainings = _service.GetAllTrainings();
        var content = _exporter.Export(trainings, format);
        var fileName = $"trainings_{DateTime.Now:yyyyMMdd}.{format}";
        var contentType = format.ToLower() == "csv" ? "text/csv" : "application/json";
        return File(System.Text.Encoding.UTF8.GetBytes(content), contentType, fileName);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        _service.DeleteTraining(id);
        return RedirectToAction(nameof(Index));
    }
}
