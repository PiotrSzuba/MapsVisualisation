using Microsoft.Extensions.Configuration;

namespace MapsVisualisation.Service.Services;

public interface IDomainProvider
{
    string GetDomainName();
}

public class DomainProvider : IDomainProvider
{
    private string BaseUrl { get; set; }

    public DomainProvider(IConfiguration configuration)
    {
        BaseUrl = configuration.GetSection("ApplicationUrl").Value;
    }

    public string GetDomainName() => BaseUrl;
}
