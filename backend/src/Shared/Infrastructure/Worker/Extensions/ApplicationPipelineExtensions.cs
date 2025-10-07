namespace Worker.Extensions;

public static class ApplicationPipelineExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // Configure CORS
        app.UseCors();

        // Map SignalR hubs
        app.MapSignalRHubs();

        return app;
    }
}
