using NServiceBus;

namespace SalesService
{
    public class EndpointConfig 
        : IConfigureThisEndpoint, AsA_Publisher
    {
    }
}
