using ActiveLog.Web.Data;
using ActiveLog.Web.Models;
using ActiveLog.Web.Services;
using Microsoft.Data.Sqlite;

namespace ActiveLog.Tests;

public class TrainingServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly TrainingService _service;

    public TrainingServiceTests()
    {
        _connection = new SqliteConnection("Data Source=TestDb;Mode=Memory;Cache=Shared");
        _connection.Open();

        var createCommand = _connection.CreateCommand();
        createCommand.CommandText = @"
            CREATE TABLE IF NOT EXISTS Trainings (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Datum TEXT NOT NULL,
                Typ TEXT NOT NULL,
                DauerMinuten INTEGER NOT NULL,
                Notizen TEXT,
                ZielId INTEGER,
                Distanz REAL,
                DurchschnittsGeschwindigkeit REAL,
                GesamtGewicht REAL,
                AnzahlSaetze INTEGER,
                AnzahlTeilnehmer INTEGER,
                Mannschaft TEXT
            );
        ";
        createCommand.ExecuteNonQuery();

        DatabaseHelper.SetConnectionString("Data Source=TestDb;Mode=Memory;Cache=Shared");

        _service = new TrainingService();
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }

    [Fact]
    public void CreateTraining_CardioTyp_ErstelltCardioTraining()
    {
        var extraData = new Dictionary<string, object>
        {
            { "Distanz", 10.5 },
            { "Geschwindigkeit", 12.0 }
        };

        var training = _service.CreateTraining("Cardio", DateTime.Now, 60, "Test", extraData);

        Assert.IsType<CardioTraining>(training);
        var cardio = training as CardioTraining;
        Assert.Equal(10.5, cardio!.Distanz);
        Assert.Equal(12.0, cardio.DurchschnittsGeschwindigkeit);
    }

    [Fact]
    public void CreateTraining_KraftTyp_ErstelltKraftTraining()
    {
        var extraData = new Dictionary<string, object>
        {
            { "Gewicht", 100.0 },
            { "Saetze", 5 }
        };

        var training = _service.CreateTraining("Kraft", DateTime.Now, 45, "Test", extraData);

        Assert.IsType<KraftTraining>(training);
        var kraft = training as KraftTraining;
        Assert.Equal(100.0, kraft!.GesamtGewicht);
        Assert.Equal(5, kraft.AnzahlSaetze);
    }

    [Fact]
    public void CreateTraining_TeamTyp_ErstelltTeamTraining()
    {
        var extraData = new Dictionary<string, object>
        {
            { "Teilnehmer", 11 },
            { "Mannschaft", "FC Test" }
        };

        var training = _service.CreateTraining("Team", DateTime.Now, 90, "Test", extraData);

        Assert.IsType<TeamTraining>(training);
        var team = training as TeamTraining;
        Assert.Equal(11, team!.AnzahlTeilnehmer);
        Assert.Equal("FC Test", team.Mannschaft);
    }

    [Fact]
    public void ValidateTraining_GültigesTraining_ReturnsTrue()
    {
        var training = new Training
        {
            Typ = "Cardio",
            Datum = DateTime.Now,
            DauerMinuten = 30
        };

        var result = _service.ValidateTraining(training);

        Assert.True(result);
    }

    [Fact]
    public void ValidateTraining_DauerNull_ReturnsFalse()
    {
        var training = new Training
        {
            Typ = "Cardio",
            Datum = DateTime.Now,
            DauerMinuten = 0
        };

        var result = _service.ValidateTraining(training);

        Assert.False(result);
    }

    [Fact]
    public void ValidateTraining_TypLeer_ReturnsFalse()
    {
        var training = new Training
        {
            Typ = "",
            Datum = DateTime.Now,
            DauerMinuten = 30
        };

        var result = _service.ValidateTraining(training);

        Assert.False(result);
    }

    [Fact]
    public void ValidateTraining_DatumInZukunft_ReturnsFalse()
    {
        var training = new Training
        {
            Typ = "Cardio",
            Datum = DateTime.Now.AddDays(2),
            DauerMinuten = 30
        };

        var result = _service.ValidateTraining(training);

        Assert.False(result);
    }

    [Fact]
    public void ExportTrainings_CsvFormat_ReturnsValidCsv()
    {
        var result = _service.ExportTrainings("csv");

        Assert.Contains("Id,Datum,Typ,Dauer (Min),Notizen", result);
    }

    [Fact]
    public void ExportTrainings_JsonFormat_ReturnsValidJson()
    {
        var result = _service.ExportTrainings("json");

        Assert.Contains("[", result);
        Assert.Contains("]", result);
    }

    [Fact]
    public void ExportTrainings_UngültigesFormat_WirftException()
    {
        Assert.Throws<ArgumentException>(() => _service.ExportTrainings("xml"));
    }
}
