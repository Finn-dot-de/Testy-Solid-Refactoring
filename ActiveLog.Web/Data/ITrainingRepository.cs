// uploaded:AEuP12_OOP_SOLID_team1/ActiveLog.Web/Data/ITrainingRepository.cs
using ActiveLog.Web.Models;

namespace ActiveLog.Web.Data;

// Core CRUD Operationen
public interface ITrainingRepository
{
    List<Training> GetAll();
    Training? GetById(int id);
    void Add(Training training);
    void Update(Training training);
    void Delete(int id);
}

// Spezielle Abfragen (Queries)
public interface ITrainingSearchRepository
{
    List<Training> GetByTyp(string typ);
    List<Training> GetByDateRange(DateTime von, DateTime bis);
    List<Training> GetByZielId(int zielId);
}

// Datenbank-seitige Statistiken
public interface ITrainingStatsRepository
{
    double GetGesamtDauer();
    double GetDurchschnittlicheDauer();
    Dictionary<string, int> GetTrainingsCountByTyp();
}

// Export-Logik
public interface ITrainingExportRepository
{
    string ExportToCsv();
    string ExportToJson();
}

// Übungs-Management
public interface IUebungRepository
{
    void AddUebung(Uebung uebung);
    List<Uebung> GetUebungenByTrainingId(int trainingId);
}