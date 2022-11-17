using System.Text;
using MapsVisualisation.WebScrapers;


Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

//MapyAmzpScrapper.Run();
//IgrekAmzpScraper.Run();

var thumbnail = await ThumbnailGenerator.GetThumbnailImage("http://amzpbig.com/maps/025_TK25/1956_Karnitz.jpg");