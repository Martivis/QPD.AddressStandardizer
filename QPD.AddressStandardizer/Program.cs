using Microsoft.AspNetCore.Mvc;
using QPD.AddressStandardizer.Configuration;
using QPD.AddressStandardizer.Exceptions;
using QPD.AddressStandardizer.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddHttpClient();
services.AddTransient<ICleanClient, CleanClient>();
services.AddControllers();
services.AddAppCors();
services.AddAppAutoMapper();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAppCors();
app.MapControllers();

app.Run();
