using ActiveLog.Web.Data;
using ActiveLog.Web.Data.Mappers;            // NEU
using ActiveLog.Web.Data.Mappers.Strategies; // NEU
using ActiveLog.Web.Models;
using ActiveLog.Web.Services;
using ActiveLog.Web.Services.Strategies;
using Microsoft.Data.Sqlite;

namespace ActiveLog.Tests;

public class TrainingServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly TrainingService _service;

    public TrainingServiceTests()
    {
        // In-Memory DB Setup
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

        var mappers = new List<ITrainingDataMapper>
        {
            new CardioDataMapper(),
            new KraftDataMapper(),
            new TeamDataMapper()
        };

        var repository = new TrainingRepository(mappers); 
        // 👆 FIX ENDE

        var strategies = new List<ITrainingCreationStrategy>
        {
            new CardioCreationStrategy(),
            new KraftCreationStrategy(),
            new TeamCreationStrategy(),
            new YogaCreationStrategy()
        };

        var factory = new TrainingFactory(strategies);
        var validator = new TrainingValidator();

        _service = new TrainingService(repository, factory, validator);
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }

    // ... Rest der Tests bleibt gleich ...
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
        var cardio = (CardioTraining)training;
        Assert.Equal(10.5, cardio.Distanz);
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
        Assert.NotNull(kraft);
        Assert.Equal(100.0, kraft.GesamtGewicht);
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
        Assert.NotNull(team);
        Assert.Equal(11, team.AnzahlTeilnehmer);
        Assert.Equal("FC Test", team.Mannschaft);
    }

    [Fact]
    public void CreateTraining_YogaTyp_ErstelltYogaTraining()
    {
        var extraData = new Dictionary<string, object>
        {
            { "Stil", "Vinyasa" },
            { "Schwierigkeitsgrad", 5 }
        };

        var training = _service.CreateTraining("Yoga", DateTime.Now, 60, "Namaste", extraData);

        Assert.IsType<YogaTraining>(training);
        var yoga = training as YogaTraining;
        Assert.NotNull(yoga);
        Assert.Equal("Vinyasa", yoga.Stil);
        Assert.Equal(5, yoga.Schwierigkeitsgrad);
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
        var trainings = _service.GetAllTrainings();
        var exporter = new TrainingExporter();
        var result = exporter.Export(trainings, "csv");
        Assert.Contains("Id,Datum,Typ,Dauer (Min),Notizen", result);
    }

    [Fact]
    public void ExportTrainings_JsonFormat_ReturnsValidJson()
    {
        var trainings = _service.GetAllTrainings();
        var exporter = new TrainingExporter();
        var result = exporter.Export(trainings, "json");
        Assert.Contains("[", result);
        Assert.Contains("]", result);
    }

    [Fact]
    public void ExportTrainings_UngültigesFormat_WirftException()
    {
        var trainings = _service.GetAllTrainings();
        var exporter = new TrainingExporter();
        Assert.Throws<ArgumentException>(() => exporter.Export(trainings, "xml"));
    }
}