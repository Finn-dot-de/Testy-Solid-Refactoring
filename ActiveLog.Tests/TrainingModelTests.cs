using ActiveLog.Web.Models;

namespace ActiveLog.Tests;

public class TrainingModelTests
{
    [Fact]
    public void Training_BerechneKalorien_StandardFormel()
    {
        var training = new Training
        {
            DauerMinuten = 60
        };

        var kalorien = training.BerechneKalorien();

        Assert.Equal(300.0, kalorien);
    }

    [Fact]
    public void CardioTraining_BerechneKalorien_DistanzBasiert()
    {
        var cardio = new CardioTraining
        {
            DauerMinuten = 60,
            Distanz = 10.0
        };

        var kalorien = cardio.BerechneKalorien();

        Assert.Equal(600.0, kalorien);
    }

    [Fact]
    public void KraftTraining_BerechneKalorien_SaetzeBasiert()
    {
        var kraft = new KraftTraining
        {
            DauerMinuten = 45,
            AnzahlSaetze = 10
        };

        var kalorien = kraft.BerechneKalorien();

        Assert.Equal(300.0, kalorien);
    }

    [Fact]
    public void TeamTraining_BerechneKalorien_ReturnsZero()
    {
        var team = new TeamTraining
        {
            DauerMinuten = 90,
            AnzahlTeilnehmer = 11
        };

        var kalorien = team.BerechneKalorien();
        Assert.Equal(0.0, kalorien);
    }

    [Fact]
    public void Training_GetTrainingInfo_ReturnsKorrekteInfo()
    {
        var training = new Training
        {
            Typ = "Test",
            Datum = new DateTime(2025, 10, 4),
            DauerMinuten = 30
        };

        var info = training.GetTrainingInfo();

        Assert.Contains("Test", info);
        Assert.Contains("30 Min", info);
        Assert.Contains("04.10.2025", info);
    }

    [Fact]
    public void CardioTraining_GetTrainingInfo_EnthältDistanz()
    {
        var cardio = new CardioTraining
        {
            Typ = "Cardio",
            Datum = new DateTime(2025, 10, 4),
            DauerMinuten = 60,
            Distanz = 10.5
        };

        var info = cardio.GetTrainingInfo();

        Assert.Contains("10,5 km", info);
    }
}
