using Database.Data;
using Database;
using Backend.Controllers;
using Microsoft.EntityFrameworkCore;
using Backend;

using System.Diagnostics;
using System.Drawing;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MapsVisualisationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MapsVisualisationContext")));

var app = builder.Build();

app.AddControllers();

app.MapGet("/",() => "Hello world !");

app.MapGet("/test", () =>
{
    var imagePath = @"C:\Users\lgszu\Downloads\Maps\3567_Posen_Nord_1944.jpg";
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();
    var sampleData = new MapDetection.ModelInput()
    {
        ImageSource = imagePath,
    };

    var result = MapDetection.Predict(sampleData);


    Image image = Image.FromFile(imagePath);
    Pen blackPen = new Pen(Color.Cyan, 10);
    var cords = result.BoundingBoxes[0];

    var top = image.Height * cords.Top / 600;
    var bottom = image.Height * cords.Bottom / 600;
    var left = image.Width * cords.Left / 800;
    var right = image.Width * cords.Right / 800;

    float width = Math.Abs(left - right);
    float height = Math.Abs(bottom - top);
    Graphics g = Graphics.FromImage(image);

    g.DrawRectangle(blackPen, left, top, width,height);

    MemoryStream ms = new MemoryStream();
    image.Save(ms, image.RawFormat);

    string base64 = "data:image/jpeg;base64," + Convert.ToBase64String(ms.ToArray());
    //base64 = "data:image/jpeg;base64," + Convert.ToBase64String(File.ReadAllBytes(imagePath));

    stopWatch.Stop();

    var ts = stopWatch.Elapsed;
    string elapsedTime = String.Format("{0:00}h:{1:00}m:{2:00}s.{3:00}ms",
        ts.Hours, ts.Minutes, ts.Seconds,
        ts.Milliseconds / 10);

    return base64 + "!!!" + result.ToString() + " Time: " + elapsedTime + " Boxes: " + width;
});

app.Run();
