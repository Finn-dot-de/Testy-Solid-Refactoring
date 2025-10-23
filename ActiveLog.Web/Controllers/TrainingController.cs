using ActiveLog.Web.Models;
using ActiveLog.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace ActiveLog.Web.Controllers;

public class TrainingController : Controller
{
    private readonly TrainingService _service = new TrainingService();

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
    public IActionResult Create(string typ, DateTime datum, int dauerMinuten, string? notizen,
        double? distanz, double? gewicht, int? saetze,
        int? teilnehmer, string? mannschaft)
    {
        var extraData = new Dictionary<string, object>();

        if (distanz.HasValue)
        {
            extraData["Distanz"] = distanz.Value;
            // Berechne Durchschnittsgeschwindigkeit: km / (min / 60) = km/h
            if (dauerMinuten > 0)
            {
                var geschwindigkeit = distanz.Value / (dauerMinuten / 60.0);
                extraData["Geschwindigkeit"] = geschwindigkeit;
            }
        }
        if (gewicht.HasValue) extraData["Gewicht"] = gewicht.Value;
        if (saetze.HasValue) extraData["Saetze"] = saetze.Value;
        if (teilnehmer.HasValue) extraData["Teilnehmer"] = teilnehmer.Value;
        if (!string.IsNullOrEmpty(mannschaft)) extraData["Mannschaft"] = mannschaft;

        var training = _service.CreateTraining(typ, datum, dauerMinuten, notizen, extraData);
        _service.SaveTraining(training);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Statistics()
    {
        var stats = _service.GetStatistiken();
        return View(stats);
    }

    public IActionResult Export(string format)
    {
        var content = _service.ExportTrainings(format);
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
