using ActiveLog.Web.Models;
using Microsoft.Data.Sqlite;

namespace ActiveLog.Web.Data.Mappers.Strategies;

public class CardioDataMapper : ITrainingDataMapper
{
    public string TrainingTyp => "Cardio";

    public void MapToDb(SqliteCommand command, Training training)
    {
        var cardio = (CardioTraining)training;
        command.Parameters.AddWithValue("@Distanz", cardio.Distanz);
        command.Parameters.AddWithValue("@Geschwindigkeit", cardio.DurchschnittsGeschwindigkeit);
    }

    public Training MapFromDb(SqliteDataReader reader)
    {
        return new CardioTraining
        {
            Id = reader.GetInt32(0),
            Datum = DateTime.Parse(reader.GetString(1)),
            Typ = TrainingTyp,
            DauerMinuten = reader.GetInt32(3),
            Notizen = reader.IsDBNull(4) ? null : reader.GetString(4),
            ZielId = reader.IsDBNull(5) ? null : reader.GetInt32(5),
            Distanz = reader.IsDBNull(6) ? 0 : reader.GetDouble(6),
            DurchschnittsGeschwindigkeit = reader.IsDBNull(7) ? 0 : reader.GetDouble(7)
        };
    }
}