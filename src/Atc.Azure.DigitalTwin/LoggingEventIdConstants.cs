namespace Atc.Azure.DigitalTwin;

internal static class LoggingEventIdConstants
{
    internal static class DigitalTwinService
    {
        public const int RequestFailed = 10_000;
        public const int Failure = 10_010;

        public const int Retrieving = 10_100;
        public const int NotFound = 10_101;
        public const int Retrieved = 10_102;
        public const int Querying = 10_103;
        public const int QueryingFailed = 10_104;
        public const int Queried = 10_105;

        public const int RetrievingModels = 10_200;
        public const int ModelsNotFound = 10_201;
        public const int RetrievedModels = 10_202;
        public const int CreatingModels = 10_203;
        public const int CreateModelsFailed = 10_204;
        public const int CreatedModels = 10_205;
        public const int DecommissioningModel = 10_206;
        public const int DecommissionModelFailed = 10_207;
        public const int DecommissionedModel = 10_208;
        public const int DeletingModel = 10_209;
        public const int DeleteModelFailed = 10_210;
        public const int DeletedModel = 10_211;

        public const int CreatingOrReplacingTwin = 10_300;
        public const int CreateOrReplaceTwinFailed = 10_301;
        public const int CreatedOrReplacedTwin = 10_302;
        public const int RetrievingTwinIds = 10_303;
        public const int RetrievedTwinIds = 10_304;
        public const int RetrievingTwins = 10_305;
        public const int RetrievedTwins = 10_306;
        public const int DeletingTwin = 10_307;
        public const int DeleteTwinFailed = 10_308;
        public const int DeletedTwin = 10_309;
        public const int UpdatingTwin = 10_310;
        public const int UpdateTwinFailed = 10_311;
        public const int UpdatedTwin = 10_312;

        public const int RetrievingRelationship = 10_400;
        public const int RelationshipNotFound = 10_401;
        public const int RetrievedRelationship = 10_402;
        public const int RetrievingRelationships = 10_403;
        public const int RelationshipsNotFound = 10_404;
        public const int RetrievedRelationships = 10_405;
        public const int CreatingOrUpdatingRelationship = 10_406;
        public const int CreateOrUpdatingRelationshipFailed = 10_407;
        public const int CreatedOrUpdatedRelationship = 10_408;
        public const int DeletingRelationship = 10_409;
        public const int DeleteRelationshipFailed = 10_410;
        public const int DeletedRelationship = 10_411;
        public const int UpdatingRelationship = 10_412;
        public const int UpdateRelationshipFailed = 10_413;
        public const int UpdatedRelationship = 10_414;
    }

    internal static class Telemetry
    {
        public const int PublishingTelemetry = 10_500;
        public const int PublishedTelemetry = 10_501;
        public const int PublishTelemetryFailed = 10_502;
        public const int PublishingComponentTelemetry = 10_503;
        public const int PublishedComponentTelemetry = 10_504;
        public const int PublishComponentTelemetryFailed = 10_505;
    }

    internal static class ImportJobs
    {
        public const int ImportingGraph = 10_600;
        public const int ImportedGraph = 10_601;
        public const int ImportGraphFailed = 10_602;
        public const int RetrievingImportJob = 10_603;
        public const int RetrievedImportJob = 10_604;
        public const int RetrievingImportJobs = 10_605;
        public const int RetrievedImportJobs = 10_606;
        public const int DeletingImportJob = 10_607;
        public const int DeletedImportJob = 10_608;
        public const int DeleteImportJobFailed = 10_609;
        public const int CancellingImportJob = 10_610;
        public const int CancelledImportJob = 10_611;
        public const int CancelImportJobFailed = 10_612;
    }

    internal static class Component
    {
        public const int RetrievingComponent = 10_700;
        public const int RetrievedComponent = 10_701;
        public const int ComponentNotFound = 10_702;
        public const int UpdatingComponent = 10_703;
        public const int UpdatedComponent = 10_704;
        public const int UpdateComponentFailed = 10_705;
    }

    internal static class ModelRepositoryService
    {
        public const int UnknownDirectoryPath = 11_000;
        public const int ModelsLoaded = 11_001;
        public const int ParseFailed = 11_002;
        public const int ParseError = 11_003;
        public const int DependencyOrderingFailed = 11_004;
    }

    internal static class DigitalTwinParser
    {
        public const int ParseFailed = 12_000;
        public const int ParseError = 12_001;
    }
}