using System.Data.SQLite;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();

builder.Services.AddSingleton(new DbConfiguration(builder.Configuration["DbConnectionString"]!));

var app = builder.Build();

app.MapGet(
    "/politics/by/name", 
    (
        [FromServices]DbConfiguration dbConfiguration,
        [FromServices]ILogger<Program> logger,
        [FromQuery] string name) =>
    {
        var result = new List<PoliticDTO>();
        
        using var sqlConnection = new SQLiteConnection(dbConfiguration.ConnectionString);
        sqlConnection.Open();
        
        var query = $"SELECT * FROM Politicians WHERE Name='{name}'";
        using var command = new SQLiteCommand(query, sqlConnection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var currentPolitic = new PoliticDTO(reader.GetString(1), reader.GetString(2));
            result.Add(currentPolitic);
            logger.LogInformation($"Politic {currentPolitic} fetched");
        }

        return result;
    });

app.MapGet(
    "/all/politics", 
    (
        [FromServices]DbConfiguration dbConfiguration,
        [FromServices]ILogger<Program> logger) =>
    {
        var result = new List<PoliticDTO>();
        
        using var sqlConnection = new SQLiteConnection(dbConfiguration.ConnectionString);
        sqlConnection.Open();
        
        var query = $"SELECT * FROM Politicians";
        using var command = new SQLiteCommand(query, sqlConnection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var currentPolitic = new PoliticDTO(reader.GetString(1), reader.GetString(2));
            result.Add(currentPolitic);
            logger.LogInformation($"Politic {currentPolitic} fetched");
        }

        return result;
    });

app.Run();

public record DbConfiguration(string ConnectionString);

public record PoliticDTO(string Name, string Surname);
