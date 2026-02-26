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

// In-memory list to store wallets
List<Wallet> wallets = new()
{
    new Wallet { Id = 1, Balance = 100 },
    new Wallet { Id = 2, Balance = 50 }
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

// Post endpoint to transfer funds between wallets
app.MapPost("/transfer", (TransferReq req) =>
{
    int fromId = req.FromId;
    int toId = req.ToId;
    decimal amount = req.Amount;

    if (amount <= 0)
    {
        return Results.BadRequest(new { status = "Failed", error = "InvalidAmount" });
    }

    // Validate wallets and funds before making any changes
    var fromWallet = wallets.FirstOrDefault(w => w.Id == fromId);
    var toWallet = wallets.FirstOrDefault(w => w.Id == toId);

    if (fromWallet == null || toWallet == null)
    {
        return Results.BadRequest(new { status = "Failed", error = "AccountNotFound" });
    }

    if (fromWallet.Balance < amount)
    {
        return Results.BadRequest(new { status = "Failed", error = "InsufficientFunds" });
    }

    // Atomic updating -> update after validation to prevent partial updates
    fromWallet.Balance -= amount;
    toWallet.Balance += amount;

    // This is just a simulation, but after this point we would also change the inventory, 
    // and any other factors related to the wallet, possible account, and game

    return Results.Ok(new { status = "Success" });
});

// Only using swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();