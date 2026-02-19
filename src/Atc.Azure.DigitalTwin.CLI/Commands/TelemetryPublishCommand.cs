namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class TelemetryPublishCommand : AsyncCommand<TelemetryPublishCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<TelemetryPublishCommand> logger;

    public TelemetryPublishCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<TelemetryPublishCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        TelemetryPublishCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings, cancellationToken);
    }

    private async Task<int> ExecuteInternalAsync(
        TelemetryPublishCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader();

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            (bool Succeeded, string? ErrorMessage) result;

            if (!string.IsNullOrEmpty(settings.ComponentName))
            {
                logger.LogInformation($"Publishing component telemetry for twin '{settings.TwinId}' component '{settings.ComponentName}'");
                result = await digitalTwinService.PublishComponentTelemetryAsync(
                    settings.TwinId,
                    settings.ComponentName,
                    settings.Payload,
                    cancellationToken: cancellationToken);
            }
            else
            {
                logger.LogInformation($"Publishing telemetry for twin '{settings.TwinId}'");
                result = await digitalTwinService.PublishTelemetryAsync(
                    settings.TwinId,
                    settings.Payload,
                    cancellationToken: cancellationToken);
            }

            if (!result.Succeeded)
            {
                logger.LogError($"Failed to publish telemetry: {result.ErrorMessage}");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation("Successfully published telemetry");
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"Error {ex.Status}: {ex.GetLastInnerMessage()}");
            return ConsoleExitStatusCodes.Failure;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.GetLastInnerMessage());
            return ConsoleExitStatusCodes.Failure;
        }

        return ConsoleExitStatusCodes.Success;
    }
}