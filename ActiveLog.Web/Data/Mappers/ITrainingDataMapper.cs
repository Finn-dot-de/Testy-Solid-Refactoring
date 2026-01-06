using ActiveLog.Web.Models;
using Microsoft.Data.Sqlite;

namespace ActiveLog.Web.Data.Mappers;

public interface ITrainingDataMapper
{
    string TrainingTyp { get; }
    void MapToDb(SqliteCommand command, Training training);
    Training MapFromDb(SqliteDataReader reader);
}