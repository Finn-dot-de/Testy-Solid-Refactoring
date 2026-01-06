using ActiveLog.Web.Models;
using Microsoft.Data.Sqlite;

namespace ActiveLog.Web.Data.Mappers.Strategies;

public class TeamDataMapper : ITrainingDataMapper
{
    public string TrainingTyp => "Team";

    public void MapToDb(SqliteCommand command, Training training)
    {
        var team = (TeamTraining)training;
        command.Parameters.AddWithValue("@Teilnehmer", team.AnzahlTeilnehmer);
        command.Parameters.AddWithValue("@Mannschaft", team.Mannschaft);
    }

    public Training MapFromDb(SqliteDataReader reader)
    {
        return new TeamTraining
        {
            Id = reader.GetInt32(0),
            Datum = DateTime.Parse(reader.GetString(1)),
            Typ = TrainingTyp,
            DauerMinuten = reader.GetInt32(3),
            Notizen = reader.IsDBNull(4) ? null : reader.GetString(4),
            ZielId = reader.IsDBNull(5) ? null : reader.GetInt32(5),
            AnzahlTeilnehmer = reader.IsDBNull(10) ? 0 : reader.GetInt32(10),
            Mannschaft = reader.IsDBNull(11) ? "" : reader.GetString(11)
        };
    }
}