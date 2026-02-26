using GameInventoryApiStefanKobetich.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

// Add swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

List<Game> games = new()
{
    new Game { Id = 1, Name = "Minecraft", Quantity = 10 },
    new Game { Id = 2, Name = "Tetris", Quantity = 5 }
};

app.MapGet("/games", () => games);

// Get a game by ID
app.MapPost("/games", (Game game) => {
    game.Id = games.Max(g => g.Id) + 1;
    games.Add(game);

    return Results.Created($"/games/{game.Id}", game);
});

// Only using swagger in development
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
