using System.Text;
using System.Text.Json;
using ActiveLog.Web.Data;
using ActiveLog.Web.Models;
using Microsoft.Data.Sqlite;

namespace ActiveLog.Web.Services;

// SRP-Verstoß: Diese Klasse macht zu viel!
public class TrainingService : ITrainingService
{
    public Training CreateTraining(string typ, DateTime datum, int dauerMinuten, string? notizen, Dictionary<string, object>? extraData = null)
    {
        Training training;

        switch (typ)
        {
            case "Cardio":
                training = new CardioTraining
                {
                    Typ = typ,
                    Datum = datum,
                    DauerMinuten = dauerMinuten,
                    Notizen = notizen,
                    Distanz = extraData?.ContainsKey("Distanz") == true ? Convert.ToDouble(extraData["Distanz"]) : 0,
                    DurchschnittsGeschwindigkeit = extraData?.ContainsKey("Geschwindigkeit") == true ? Convert.ToDouble(extraData["Geschwindigkeit"]) : 0
                };
                break;
            case "Kraft":
                training = new KraftTraining
                {
                    Typ = typ,
                    Datum = datum,
                    DauerMinuten = dauerMinuten,
                    Notizen = notizen,
                    GesamtGewicht = extraData?.ContainsKey("Gewicht") == true ? Convert.ToDouble(extraData["Gewicht"]) : 0,
                    AnzahlSaetze = extraData?.ContainsKey("Saetze") == true ? Convert.ToInt32(extraData["Saetze"]) : 0
                };
                break;
            case "Team":
                training = new TeamTraining
                {
                    Typ = typ,
                    Datum = datum,
                    DauerMinuten = dauerMinuten,
                    Notizen = notizen,
                    AnzahlTeilnehmer = extraData?.ContainsKey("Teilnehmer") == true ? Convert.ToInt32(extraData["Teilnehmer"]) : 0,
                    Mannschaft = extraData?.ContainsKey("Mannschaft") == true ? extraData["Mannschaft"].ToString() ?? "" : ""
                };
                break;
            default:
                training = new Training
                {
                    Typ = typ,
                    Datum = datum,
                    DauerMinuten = dauerMinuten,
                    Notizen = notizen
                };
                break;
        }

        return training;
    }

    public void SaveTraining(Training training)
    {
        if (!ValidateTraining(training))
        {
            throw new ArgumentException("Training ist ungültig");
        }

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

    public List<Training> GetAllTrainings()
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

    public bool ValidateTraining(Training training)
    {
        if (training.DauerMinuten <= 0)
            return false;

        if (string.IsNullOrWhiteSpace(training.Typ))
            return false;

        if (training.Datum > DateTime.Now.AddDays(1))
            return false;

        return true;
    }

    public string ExportTrainings(string format)
    {
        var trainings = GetAllTrainings();

        switch (format.ToLower())
        {
            case "csv":
                return ExportToCsv(trainings);
            case "json":
                return ExportToJson(trainings);
            default:
                throw new ArgumentException($"Format {format} wird nicht unterstützt");
        }
    }

    private string ExportToCsv(List<Training> trainings)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Id,Datum,Typ,Dauer (Min),Notizen");

        foreach (var training in trainings)
        {
            sb.AppendLine($"{training.Id},{training.Datum:yyyy-MM-dd},{training.Typ},{training.DauerMinuten},\"{training.Notizen}\"");
        }

        return sb.ToString();
    }

    private string ExportToJson(List<Training> trainings)
    {
        return JsonSerializer.Serialize(trainings, new JsonSerializerOptions { WriteIndented = true });
    }

    public Dictionary<string, object> GetStatistiken()
    {
        var trainings = GetAllTrainings();

        return new Dictionary<string, object>
        {
            { "GesamtAnzahl", trainings.Count },
            { "GesamtDauer", trainings.Sum(t => t.DauerMinuten) },
            { "DurchschnittsDauer", trainings.Any() ? trainings.Average(t => t.DauerMinuten) : 0 },
            { "GesamtKalorien", trainings.Sum(t => t.BerechneKalorien()) },
            { "TrainingsProTyp", trainings.GroupBy(t => t.Typ).ToDictionary(g => g.Key, g => g.Count()) }
        };
    }

    public void DeleteTraining(int id)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Trainings WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", id);
        command.ExecuteNonQuery();
    }
}
