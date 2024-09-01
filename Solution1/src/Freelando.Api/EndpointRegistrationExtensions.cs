using Freelando.Dados;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api
{
    public static class EndpointRegistrationExtensions
    {
    
        public static void AddAllEndpoints(this WebApplication app)
        {
            var endpointMethods = typeof(EndpointRegistrationExtensions).Assembly
                .GetTypes()
                .SelectMany(type => type.GetMethods())
                .Where(method => method.IsStatic && method.IsPublic && method.Name.StartsWith("AddEndPoint"));
            foreach (var endpointMethod in endpointMethods)
            {
                endpointMethod.Invoke(null, new object[] { app });
            }
        }
    }
}
