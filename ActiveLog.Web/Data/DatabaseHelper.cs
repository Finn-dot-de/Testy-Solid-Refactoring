using Microsoft.Data.Sqlite;

namespace ActiveLog.Web.Data;

public static class DatabaseHelper
{
    private static string _connectionString = "Data Source=activelog.db";

    public static void SetConnectionString(string connectionString)
    {
        _connectionString = connectionString;
    }

    public static SqliteConnection GetConnection()
    {
        return new SqliteConnection(_connectionString);
    }

    public static void InitializeDatabase()
    {
        using var connection = GetConnection();
        connection.Open();

        var createTablesCommand = connection.CreateCommand();
        createTablesCommand.CommandText = @"
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

            CREATE TABLE IF NOT EXISTS Uebungen (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                TrainingId INTEGER NOT NULL,
                Name TEXT NOT NULL,
                Saetze INTEGER NOT NULL,
                Wiederholungen INTEGER NOT NULL,
                Gewicht REAL,
                FOREIGN KEY (TrainingId) REFERENCES Trainings(Id) ON DELETE CASCADE
            );

            CREATE TABLE IF NOT EXISTS Ziele (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Beschreibung TEXT NOT NULL,
                ZielDatum TEXT NOT NULL,
                Erreicht INTEGER NOT NULL
            );
        ";
        createTablesCommand.ExecuteNonQuery();
    }
}
