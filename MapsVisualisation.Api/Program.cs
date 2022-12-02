using MapsVisualisation.Api.Controllers;
using MapsVisualisation.Database;
using MapsVisualisation.Api.Infrastructure.Extensions;
using MapsVisualisation.Service;
using MapsVisualisation.Service.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Logging.ConfigureLogging();

builder.Services.AddDbContext<MapsVisualisationContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("MapsVisualisationContext")));

builder.Services.AddSingleton<IPathProvider, PathProvider>(provider => new PathProvider(builder.Environment.ContentRootPath, configuration));
builder.Services.AddSingleton<IDomainProvider, DomainProvider>(provider => new DomainProvider(configuration));

builder.Services.AddCors();
builder.Services.AddDirectoryBrowser();
builder.Services.AddServicesDependency();

var app = builder.Build();

app.ConfigureStaticFiles(builder.Environment.ContentRootPath, GetThumbnailsFolderName(configuration), GetMapImagesFolderName(configuration));

app.ConfigureExceptionHandler();

app.Urls.Add(configuration.GetSection("ApplicationUrl").Value);

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .WithOrigins(configuration.GetSection("FrontendAppUrl").Value)
    .AllowCredentials());

app.UseHttpsRedirection();

app.AddControllers();

app.MapGet("/", () => "Hello world !");


app.Run();


string GetThumbnailsFolderName(IConfiguration configuration)
{
    return configuration.GetSection("StaticFile").GetSection("ThumbnailsFolder").Value;
}

string GetMapImagesFolderName(IConfiguration configuration)
{
    return configuration.GetSection("StaticFile").GetSection("MapImagesFolder").Value;
}