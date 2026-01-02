using ActiveLog.Web.Data;
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
    public void Add(Training training)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Trainings (Datum, Typ, DauerMinuten, Notizen, ZielId, Distanz, DurchschnittsGeschwindigkeit,
                                   GesamtGewicht, AnzahlSaetze, AnzahlTeilnehmer, Mannschaft)
            VALUES (@Datum, @Typ, @DauerMinuten, @Notizen, @ZielId, @Distanz, @Geschwindigkeit,
                    @Gewicht, @Saetze, @Teilnehmer, @Mannschaft)
        ";

        command.Parameters.AddWithValue("@Datum", training.Datum.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@Typ", training.Typ);
        command.Parameters.AddWithValue("@DauerMinuten", training.DauerMinuten);
        command.Parameters.AddWithValue("@Notizen", training.Notizen ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@ZielId", training.ZielId ?? (object)DBNull.Value);

        // Parameter-Mapping Logik (extrahiert aus dem alten Service)
        if (training is CardioTraining cardio)
        {
            command.Parameters.AddWithValue("@Distanz", cardio.Distanz);
            command.Parameters.AddWithValue("@Geschwindigkeit", cardio.DurchschnittsGeschwindigkeit);
            command.Parameters.AddWithValue("@Gewicht", DBNull.Value);
            command.Parameters.AddWithValue("@Saetze", DBNull.Value);
            command.Parameters.AddWithValue("@Teilnehmer", DBNull.Value);
            command.Parameters.AddWithValue("@Mannschaft", DBNull.Value);
        }
        else if (training is KraftTraining kraft)
        {
            command.Parameters.AddWithValue("@Distanz", DBNull.Value);
            command.Parameters.AddWithValue("@Geschwindigkeit", DBNull.Value);
            command.Parameters.AddWithValue("@Gewicht", kraft.GesamtGewicht);
            command.Parameters.AddWithValue("@Saetze", kraft.AnzahlSaetze);
            command.Parameters.AddWithValue("@Teilnehmer", DBNull.Value);
            command.Parameters.AddWithValue("@Mannschaft", DBNull.Value);
        }
        else if (training is TeamTraining team)
        {
            command.Parameters.AddWithValue("@Distanz", DBNull.Value);
            command.Parameters.AddWithValue("@Geschwindigkeit", DBNull.Value);
            command.Parameters.AddWithValue("@Gewicht", DBNull.Value);
            command.Parameters.AddWithValue("@Saetze", DBNull.Value);
            command.Parameters.AddWithValue("@Teilnehmer", team.AnzahlTeilnehmer);
            command.Parameters.AddWithValue("@Mannschaft", team.Mannschaft);
        }
        else
        {
            command.Parameters.AddWithValue("@Distanz", DBNull.Value);
            command.Parameters.AddWithValue("@Geschwindigkeit", DBNull.Value);
            command.Parameters.AddWithValue("@Gewicht", DBNull.Value);
            command.Parameters.AddWithValue("@Saetze", DBNull.Value);
            command.Parameters.AddWithValue("@Teilnehmer", DBNull.Value);
            command.Parameters.AddWithValue("@Mannschaft", DBNull.Value);
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

            // Mapping Logic (wie vorher im Service)
            switch (typ)
            {
                case "Cardio":
                    training = new CardioTraining
                    {
                        Id = reader.GetInt32(0),
                        Datum = DateTime.Parse(reader.GetString(1)),
                        Typ = typ,
                        DauerMinuten = reader.GetInt32(3),
                        Notizen = reader.IsDBNull(4) ? null : reader.GetString(4),
                        ZielId = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                        Distanz = reader.IsDBNull(6) ? 0 : reader.GetDouble(6),
                        DurchschnittsGeschwindigkeit = reader.IsDBNull(7) ? 0 : reader.GetDouble(7)
                    };
                    break;
                case "Kraft":
                    training = new KraftTraining
                    {
                        Id = reader.GetInt32(0),
                        Datum = DateTime.Parse(reader.GetString(1)),
                        Typ = typ,
                        DauerMinuten = reader.GetInt32(3),
                        Notizen = reader.IsDBNull(4) ? null : reader.GetString(4),
                        ZielId = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                        GesamtGewicht = reader.IsDBNull(8) ? 0 : reader.GetDouble(8),
                        AnzahlSaetze = reader.IsDBNull(9) ? 0 : reader.GetInt32(9)
                    };
                    break;
                case "Team":
                    training = new TeamTraining
                    {
                        Id = reader.GetInt32(0),
                        Datum = DateTime.Parse(reader.GetString(1)),
                        Typ = typ,
                        DauerMinuten = reader.GetInt32(3),
                        Notizen = reader.IsDBNull(4) ? null : reader.GetString(4),
                        ZielId = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                        AnzahlTeilnehmer = reader.IsDBNull(10) ? 0 : reader.GetInt32(10),
                        Mannschaft = reader.IsDBNull(11) ? "" : reader.GetString(11)
                    };
                    break;
                default:
                    training = new Training
                    {
                        Id = reader.GetInt32(0),
                        Datum = DateTime.Parse(reader.GetString(1)),
                        Typ = typ,
                        DauerMinuten = reader.GetInt32(3),
                        Notizen = reader.IsDBNull(4) ? null : reader.GetString(4),
                        ZielId = reader.IsDBNull(5) ? null : reader.GetInt32(5)
                    };
                    break;
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
    
    // Platzhalter für restliche Interface-Methoden (Throw NotImplementedException oder leer lassen für jetzt)
    public Training? GetById(int id) => throw new NotImplementedException();
    public void Update(Training training) => throw new NotImplementedException();
    public List<Training> GetByTyp(string typ) => throw new NotImplementedException();
    public List<Training> GetByDateRange(DateTime von, DateTime bis) => throw new NotImplementedException();
    public List<Training> GetByZielId(int zielId) => throw new NotImplementedException();
    public double GetGesamtDauer() => throw new NotImplementedException();
    public double GetDurchschnittlicheDauer() => throw new NotImplementedException();
    public Dictionary<string, int> GetTrainingsCountByTyp() => throw new NotImplementedException();
    public string ExportToCsv() => throw new NotImplementedException();
    public string ExportToJson() => throw new NotImplementedException();
    public bool ValidateTraining(Training training) => throw new NotImplementedException();
    public List<string> GetValidationErrors(Training training) => throw new NotImplementedException();
    public void AddUebung(Uebung uebung) => throw new NotImplementedException();
    public List<Uebung> GetUebungenByTrainingId(int trainingId) => throw new NotImplementedException();
}