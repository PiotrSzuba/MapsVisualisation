using MapsVisualisation.Api.Controllers;
using MapsVisualisation.Database;
using MapsVisualisation.Api.Infrastructure.Extensions;
using MapsVisualisation.Service;
using MapsVisualisation.Service.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ConfigureLogging();

builder.Services.AddDbContext<MapsVisualisationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MapsVisualisationContext")));

builder.Services.AddSingleton<IPathProvider, PathProvider>(provider => new PathProvider(builder.Environment.ContentRootPath));
builder.Services.AddCors();
builder.Services.AddDirectoryBrowser();
builder.Services.AddServicesDependency();

var app = builder.Build();

app.ConfigureStaticFiles(builder.Environment.ContentRootPath, PathProvider.ThumbnailsFolder, PathProvider.MapsImagesFolder);

app.ConfigureExceptionHandler();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .WithOrigins("http://127.0.0.1:5173")
    .AllowCredentials());

app.UseHttpsRedirection();

app.AddControllers();

app.MapGet("/", () => "Hello world !");


app.Run();
