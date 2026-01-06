using ActiveLog.Web.Data.Mappers;
using ActiveLog.Web.Models;
using Microsoft.Data.Sqlite;

namespace ActiveLog.Web.Data;

public class TrainingRepository : 
    ITrainingRepository, 
    ITrainingSearchRepository, 
    ITrainingStatsRepository, 
    ITrainingExportRepository, 
    IUebungRepository
{
    private readonly Dictionary<string, ITrainingDataMapper> _mappers;

    public TrainingRepository(IEnumerable<ITrainingDataMapper> mappers)
    {
        _mappers = mappers.ToDictionary(m => m.TrainingTyp);
    }

    public void Add(Training training)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Trainings (Datum, Typ, DauerMinuten, Notizen, ZielId, Distanz, DurchschnittsGeschwindigkeit,
                                   GesamtGewicht, AnzahlSaetze, AnzahlTeilnehmer, Mannschaft)
            VALUES (@Datum, @Typ, @DauerMinuten, @Notizen, @ZielId, @Distanz, @Geschwindigkeit,
                    @Gewicht, @Saetze, @Teilnehmer, @Mannschaft)";

        AddBaseParameters(command, training);
        AddDefaultNullParameters(command);

        // Dynamischer Mapper-Aufruf statt if/else
        if (_mappers.TryGetValue(training.Typ, out var mapper))
        {
            mapper.MapToDb(command, training);
        }

        command.ExecuteNonQuery();
    }

    public void Update(Training training)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Trainings 
            SET Datum = @Datum, 
                DauerMinuten = @DauerMinuten, 
                Notizen = @Notizen, 
                ZielId = @ZielId,
                Distanz = @Distanz,
                DurchschnittsGeschwindigkeit = @Geschwindigkeit,
                GesamtGewicht = @Gewicht,
                AnzahlSaetze = @Saetze,
                AnzahlTeilnehmer = @Teilnehmer,
                Mannschaft = @Mannschaft
            WHERE Id = @Id";

        command.Parameters.AddWithValue("@Id", training.Id);
        AddBaseParameters(command, training);
        AddDefaultNullParameters(command);

        if (_mappers.TryGetValue(training.Typ, out var mapper))
        {
            mapper.MapToDb(command, training);
        }

        command.ExecuteNonQuery();
    }

    public List<Training> GetAll()
    {
        var trainings = new List<Training>();

        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Trainings ORDER BY Datum DESC";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var typ = reader.GetString(2);
            Training training;

            if (_mappers.TryGetValue(typ, out var mapper))
            {
                training = mapper.MapFromDb(reader);
            }
            else
            {
                // Fallback für unbekannte Typen (Basis-Klasse)
                training = new Training
                {
                    Id = reader.GetInt32(0),
                    Datum = DateTime.Parse(reader.GetString(1)),
                    Typ = typ,
                    DauerMinuten = reader.GetInt32(3),
                    Notizen = reader.IsDBNull(4) ? null : reader.GetString(4),
                    ZielId = reader.IsDBNull(5) ? null : reader.GetInt32(5)
                };
            }
            trainings.Add(training);
        }
        return trainings;
    }

    public void Delete(int id)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Trainings WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", id);
        command.ExecuteNonQuery();
    }

    private void AddBaseParameters(SqliteCommand command, Training training)
    {
        command.Parameters.AddWithValue("@Datum", training.Datum.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@Typ", training.Typ);
        command.Parameters.AddWithValue("@DauerMinuten", training.DauerMinuten);
        command.Parameters.AddWithValue("@Notizen", training.Notizen ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@ZielId", training.ZielId ?? (object)DBNull.Value);
    }

    private void AddDefaultNullParameters(SqliteCommand command)
    {
        var paramsToNull = new[] { "@Distanz", "@Geschwindigkeit", "@Gewicht", "@Saetze", "@Teilnehmer", "@Mannschaft" };
        foreach(var p in paramsToNull) {
             if(!command.Parameters.Contains(p)) command.Parameters.AddWithValue(p, DBNull.Value);
        }
    }

    // Dummy Implementationen (ISP Compliance)
    public Training? GetById(int id) => throw new NotImplementedException();
    public List<Training> GetByTyp(string typ) => throw new NotImplementedException();
    public List<Training> GetByDateRange(DateTime von, DateTime bis) => throw new NotImplementedException();
    public List<Training> GetByZielId(int zielId) => throw new NotImplementedException();
    public double GetGesamtDauer() => throw new NotImplementedException();
    public double GetDurchschnittlicheDauer() => throw new NotImplementedException();
    public Dictionary<string, int> GetTrainingsCountByTyp() => throw new NotImplementedException();
    public string ExportToCsv() => throw new NotImplementedException();
    public string ExportToJson() => throw new NotImplementedException();
    public void AddUebung(Uebung uebung) => throw new NotImplementedException();
    public List<Uebung> GetUebungenByTrainingId(int trainingId) => throw new NotImplementedException();
}