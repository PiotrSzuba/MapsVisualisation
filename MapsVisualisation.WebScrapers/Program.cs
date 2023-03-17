using System.Text;
using MapsVisualisation.Domain.Entities;
using MapsVisualisation.WebScrapers.OtherSourcesScrapers;
using MapsVisualisation.WebScrapers.RegionScrapers;
using MapsVisualisation.WebScrapers.RegionScrapers.Mapster;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var test = await MapsterRunner.GetAusHunMonarch75Async();
Console.WriteLine(test);
//MapyAmzpScraper.Run();
//IgrekAmzpScraper.Run();
//SbcScrapper.Run(MockRegion());
//BcuwScrapper.Run(MockRegion());
//KpbcScraper.Run(MockRegion2());


static Region MockRegion()
{
    return Region.Create("Ostritz", "", "", "4955", RegionType.Messtischblatt);
}

static Region MockRegion2()
{
    return Region.Create("Przeróśl", "", "", "P31 S35", RegionType.Wig100);
}