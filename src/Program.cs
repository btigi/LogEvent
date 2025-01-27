using System.Data.SQLite;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.UseHttpsRedirection();

var dbPath = app.Configuration.GetConnectionString("DbPath");
var connectionString = $"Data Source={dbPath};Version=3;";
var securityKey = app.Configuration["SecurityKey"];
await InitializeDatabase();

app.MapGet("/test", () =>
{
    return Results.Ok();
});

app.MapPost("/log", async (string eventname, string security) =>
{
    if (securityKey == security)
    {
        await SaveEvent(eventname);
        return Results.Created();
    }
    return Results.Unauthorized();
});

app.Run();


async Task InitializeDatabase()
{
    using var connection = new SQLiteConnection(connectionString);
    await connection.OpenAsync();

    var keysTableQuery = @"
                        CREATE TABLE IF NOT EXISTS LogEvents (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            EventName TEXT NOT NULL,
                            Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP
                        )";
    using var keysCommand = new SQLiteCommand(keysTableQuery, connection);
    await keysCommand.ExecuteNonQueryAsync();

    await connection.CloseAsync();
}

async Task SaveEvent(string eventName)
{
    using var connection = new SQLiteConnection(connectionString);
    await connection.OpenAsync();
    var insertQuery = "INSERT INTO LogEvents (EventName) VALUES (@eventName)";
    using var insertCmd = new SQLiteCommand(insertQuery, connection);
    insertCmd.Parameters.AddWithValue("@eventName", eventName);
    await insertCmd.ExecuteNonQueryAsync();
    await connection.CloseAsync();
}