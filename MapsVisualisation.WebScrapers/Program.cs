using System.Text;
using MapsVisualisation.Database;
using MapsVisualisation.Domain.Entities;
using MapsVisualisation.WebScrapers.OtherSourcesScrapers;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

//MapyAmzpScrapper.Run();
//IgrekAmzpScraper.Run();
//SbcScrapper.Run(MockRegion());
BcuwScrapper.Run(MockRegion());
//KpbcScraper.Run(MockRegion2());


static Region MockRegion()
{
    return Region.Create("Ostritz", "", "", "4955");
}

static Region MockRegion2()
{
    return Region.Create("Przeróśl", "", "", "P31 S35");
}