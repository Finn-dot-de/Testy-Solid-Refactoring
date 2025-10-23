using ActiveLog.Web.Models;

namespace ActiveLog.Web.Data;

// ISP-Verstoß: "Fat Interface" mit zu vielen Methoden
// Nicht alle Implementierungen brauchen alle diese Methoden
public interface ITrainingRepository
{
    // CRUD Operationen
    List<Training> GetAll();
    Training? GetById(int id);
    void Add(Training training);
    void Update(Training training);
    void Delete(int id);

    // Spezielle Abfragen
    List<Training> GetByTyp(string typ);
    List<Training> GetByDateRange(DateTime von, DateTime bis);
    List<Training> GetByZielId(int zielId);

    // Statistiken
    double GetGesamtDauer();
    double GetDurchschnittlicheDauer();
    Dictionary<string, int> GetTrainingsCountByTyp();

    // Export
    string ExportToCsv();
    string ExportToJson();

    // Validierung
    bool ValidateTraining(Training training);
    List<string> GetValidationErrors(Training training);

    // Übungen
    void AddUebung(Uebung uebung);
    List<Uebung> GetUebungenByTrainingId(int trainingId);
}
