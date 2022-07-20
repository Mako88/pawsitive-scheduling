using Microsoft.AspNetCore.Builder;

namespace PawsitiveScheduling.API
{
    /// <summary>
    /// Interface for handlers
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// Map the handler to an endpoint
        /// </summary>
        void MapEndpoint(WebApplication app);
    }
}