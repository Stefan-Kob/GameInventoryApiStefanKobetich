using GameInventoryApiStefanKobetich.Models;
using GameInventoryApiStefanKobetich.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

// Add swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

// In-memory list to store games
List<Game> games = new()
{
    new Game { Id = 1, Name = "Minecraft", Quantity = 10 },
    new Game { Id = 2, Name = "Far Cry 4", Quantity = 5 },
    new Game { Id = 3, Name = "Red Dead Redemption 2", Quantity = 8 },
    new Game { Id = 4, Name = "VTOL VR", Quantity = 1 }
};

// Use custom middleware to check for API key
app.UseApiKey();
    
// Get endpoint to retrieve all games
app.MapGet("/games", () => games);

// Post endpoint to add a new game
app.MapPost("/games", (Game game) =>
{
    // Validate the input (id is auto-generated, so we don't check it here)
    if (string.IsNullOrWhiteSpace(game.Name))
    {
        return Results.BadRequest(new { error = "InvalidParameter", message = "Name must not be empty" });
    }
    if (game.Quantity <= 0)
    {
        return Results.BadRequest(new { error = "InvalidParameter", message = "Quantity must be greater than zero" });
    }

    game.Id = games.Max(g => g.Id) + 1;
    games.Add(game);

    // Return the created game with a 201 status code
    return Results.Created($"/games/{game.Id}", game);
});

// Only using swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();