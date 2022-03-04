public static class Mappings
{
    public static void MapGetToTriggerLogEntryForMessage(this WebApplication webApplication)
    {
        webApplication.MapGet("/mess", (HttpContext ctx, Serilog.ILogger logger) =>
        {
            var response = $"Message {DateTime.Now}";
            logger.ForContext($"Message-{Guid.NewGuid()}", new MessageDto(Guid.NewGuid())).Information(response);
            return response;
        });
    }

    public static void MapGetToTriggerLogEntryForApplicationData(this WebApplication webApplication)
    {
        webApplication.MapGet("/ad", (HttpContext ctx, Serilog.ILogger logger) =>
        {
            var response = $"ApplicationData {DateTime.Now}";
            logger.ForContext("ApplicationData.Id", new MessageDto(Guid.NewGuid())).Information(response);
            return response;
        });
    }
}