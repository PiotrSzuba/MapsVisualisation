using System.Text;
using MapsVisualisation.Database;
using MapsVisualisation.Domain.Entities;
using MapsVisualisation.WebScrapers.OtherSourcesScrapers;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

//MapyAmzpScrapper.Run();
//IgrekAmzpScraper.Run();
SbcScrapper.Run(MockRegion());



static Region MockRegion()
{
    return Region.Create("Breslau(Nord)", "", "", "4868");
}