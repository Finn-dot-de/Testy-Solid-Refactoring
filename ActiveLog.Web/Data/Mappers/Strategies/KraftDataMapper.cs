using ActiveLog.Web.Models;
using Microsoft.Data.Sqlite;

namespace ActiveLog.Web.Data.Mappers.Strategies;

public class KraftDataMapper : ITrainingDataMapper
{
    public string TrainingTyp => "Kraft";

    public void MapToDb(SqliteCommand command, Training training)
    {
        var kraft = (KraftTraining)training;
        command.Parameters.AddWithValue("@Gewicht", kraft.GesamtGewicht);
        command.Parameters.AddWithValue("@Saetze", kraft.AnzahlSaetze);
    }

    public Training MapFromDb(SqliteDataReader reader)
    {
        return new KraftTraining
        {
            Id = reader.GetInt32(0),
            Datum = DateTime.Parse(reader.GetString(1)),
            Typ = TrainingTyp,
            DauerMinuten = reader.GetInt32(3),
            Notizen = reader.IsDBNull(4) ? null : reader.GetString(4),
            ZielId = reader.IsDBNull(5) ? null : reader.GetInt32(5),
            GesamtGewicht = reader.IsDBNull(8) ? 0 : reader.GetDouble(8),
            AnzahlSaetze = reader.IsDBNull(9) ? 0 : reader.GetInt32(9)
        };
    }
}