namespace Atc.Azure.DigitalTwin.Extensions;

public static class DigitalTwinOptionsExtensions
{
    public static TokenCredential GetTokenCredential(
        this DigitalTwinOptions options)
        => new DefaultAzureCredential(
            GetCredentialOptions(options));

    public static DefaultAzureCredentialOptions GetCredentialOptions(
        DigitalTwinOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        return new DefaultAzureCredentialOptions
        {
            // We need to explicit specify the tenant id for this to work locally.
            // Ref: https://github.com/Azure/azure-sdk-for-net/issues/8957
            TenantId = options.TenantId,
            SharedTokenCacheTenantId = options.TenantId,
            VisualStudioTenantId = options.TenantId,
            VisualStudioCodeTenantId = options.TenantId,
#if DEBUG
            ExcludeManagedIdentityCredential = true,
#endif
            ExcludeAzurePowerShellCredential = true,
            ExcludeInteractiveBrowserCredential = true,
            ExcludeSharedTokenCacheCredential = true,
            ExcludeEnvironmentCredential = true,
            ExcludeVisualStudioCodeCredential = true,
        };
    }
}
