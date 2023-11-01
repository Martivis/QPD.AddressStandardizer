using QPD.AddressStandardizer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddTransient<ICleanClient, CleanClient>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/clean-address", async (string address) =>
{
    var client = app.Services.GetRequiredService<ICleanClient>();
    var result = await client.CleanAddress(address);
    return result;
});

app.Run();
