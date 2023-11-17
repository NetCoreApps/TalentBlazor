using ServiceStack;
using TalentBlazor.ServiceModel;

namespace TalentBlazor.ServiceInterface;

public class MyServices : Service
{
    public object Any(Hello request)
    {
        return new HelloResponse { Result = $"Hello, {request.Name}!" };
    }
}