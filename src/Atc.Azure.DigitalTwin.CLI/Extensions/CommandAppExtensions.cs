// ReSharper disable StringLiteralTypo
namespace Atc.Azure.DigitalTwin.CLI.Extensions;

public static class CommandAppExtensions
{
    public static void ConfigureCommands(
        this CommandApp app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.Configure(config =>
        {
            config.AddBranch("model", ConfigureModelCommands());
            config.AddBranch("route", ConfigureEventRouteCommands());
            config.AddBranch("twin", ConfigureTwinCommands());
        });
    }

    private static Action<IConfigurator<CommandSettings>> ConfigureModelCommands()
        => node =>
        {
            node.SetDescription("Operations related to models.");

            ConfigureModelCreateCommands(node);

            node.AddCommand<ModelDecommissionCommand>("decommission")
                .WithDescription("Decommission a single model.")
                .WithExample("model decommission --tenantId -a <adt-instance-url> -m <model-id>");

            ConfigureModelDeleteCommands(node);

            ConfigureModelGetCommands(node);

            node.AddCommand<ModelValidateCommand>("validate")
                .WithDescription("Validate models.")
                .WithExample("model validate -d <directory-path>");
        };

    private static void ConfigureModelCreateCommands(
        IConfigurator<CommandSettings> node)
        => node.AddBranch("create", upload =>
        {
            upload.SetDescription("Operations related to creating models.");

            upload.AddCommand<ModelCreateSingleCommand>("single")
                .WithDescription("Create single model.")
                .WithExample("model create single --tenantId -a <adt-instance-url> -d <directory-path> -m <model-id>");

            upload.AddCommand<ModelCreateAllCommand>("all")
                .WithDescription("Create all models.")
                .WithExample("model create all --tenantId -a <adt-instance-url> -d <directory-path>");
        });

    private static void ConfigureModelDeleteCommands(
        IConfigurator<CommandSettings> node)
        => node.AddBranch("delete", delete =>
        {
            delete.SetDescription("Operations related to deleting models.");

            delete.AddCommand<ModelDeleteSingleCommand>("single")
                .WithDescription("Delete single model.")
                .WithExample("model delete single --tenantId -a <adt-instance-url> -m <model-id>");

            delete.AddCommand<ModelDeleteAllCommand>("all")
                .WithDescription("Delete all models.")
                .WithExample("model delete all --tenantId -a <adt-instance-url>");
        });

    private static void ConfigureModelGetCommands(
        IConfigurator<CommandSettings> node)
        => node.AddBranch("get", get =>
        {
            get.SetDescription("Operations related to reading models.");

            get.AddCommand<ModelGetSingleCommand>("single")
                .WithDescription("Get single model.")
                .WithExample("model get single --tenantId -a <adt-instance-url> -m <model-id>");

            get.AddCommand<ModelGetAllCommand>("all")
                .WithDescription("Get all models.")
                .WithExample("model get all --tenantId -a <adt-instance-url>");
        });

    private static Action<IConfigurator<CommandSettings>> ConfigureEventRouteCommands()
        => node =>
        {
            node.SetDescription("Operations related to event routes.");

            node.AddCommand<EventRouteCreateCommand>("create")
                .WithDescription("Create event route.")
                .WithExample("route create");

            node.AddCommand<EventRouteDeleteCommand>("delete")
                .WithDescription("Delete event route.")
                .WithExample("route delete");

            ConfigureEventRouteGetCommands(node);
        };

    private static void ConfigureEventRouteGetCommands(
        IConfigurator<CommandSettings> node)
        => node.AddBranch("get", get =>
        {
            get.SetDescription("Operations related to reading event routes.");

            get.AddCommand<EventRouteGetSingleCommand>("single")
                .WithDescription("Get single event route.")
                .WithExample("route get single");

            get.AddCommand<EventRouteGetAllCommand>("all")
                .WithDescription("Get all event routes.")
                .WithExample("route get all");
        });

    private static Action<IConfigurator<CommandSettings>> ConfigureTwinCommands()
        => node =>
        {
            node.SetDescription("Operations related to twins.");

            node.AddCommand<TwinCountCommand>("count")
                .WithDescription("Count the different types of twins.")
                .WithExample("twin count --tenantId -a <adt-instance-url>");

            node.AddCommand<TwinCreateCommand>("create")
                .WithDescription("Create single twin.")
                .WithExample("twin create");

            ConfigureTwinDeleteCommands(node);

            node.AddCommand<TwinGetCommand>("get")
                .WithDescription("Get single twin.")
                .WithExample("twin get --tenantId -a <adt-instance-url> -t <twin-id>");

            node.AddCommand<TwinUpdateCommand>("update")
                .WithDescription("Update single twin.")
                .WithExample("twin update");

            node.AddBranch("relationship", ConfigureRelationshipCommands());
        };

    private static void ConfigureTwinDeleteCommands(
        IConfigurator<CommandSettings> node)
        => node.AddBranch("delete", delete =>
        {
            delete.SetDescription("Operations related to deleting twins.");

            delete.AddCommand<TwinDeleteSingleCommand>("single")
                .WithDescription("Delete single twin.")
                .WithExample("twin delete single --tenantId -a <adt-instance-url> -t <twin-id>");

            delete.AddCommand<TwinDeleteAllCommand>("all")
                .WithDescription("Delete all twins.")
                .WithExample("twin delete all --tenantId -a <adt-instance-url>");

            delete.AddCommand<TwinDeleteAllByModelCommand>("allbymodel")
                .WithDescription("Delete all twins by a modelId.")
                .WithExample("twin delete allbymodel --tenantId -a <adt-instance-url> -m <model-id>");
        });

    private static Action<IConfigurator<CommandSettings>> ConfigureRelationshipCommands()
        => node =>
        {
            node.SetDescription("Operations related to twin relationships.");

            node.AddCommand<RelationshipCreateCommand>("create")
                .WithDescription("Create relationship.")
                .WithExample("twin relationship create --tenantId -a <adt-instance-url> --source-twinId <source-twin-id> --target-twinId <target-twin-id> --relationshipName <relationship-name>");

            node.AddCommand<RelationshipDeleteCommand>("delete")
                .WithDescription("Delete relationship.")
                .WithExample("twin relationship delete --tenantId -a <adt-instance-url> -t <twin-id> --relationshipId <relationship-id>");

            ConfigureRelationshipGetCommands(node);
        };

    private static void ConfigureRelationshipGetCommands(
        IConfigurator<CommandSettings> node)
        => node.AddBranch("get", get =>
        {
            get.SetDescription("Operations related to reading twin relationships.");

            get.AddCommand<RelationshipGetSingleCommand>("single")
                .WithDescription("Get single relationship for twin.")
                .WithExample("twin relationship get single --tenantId -a <adt-instance-url> -t <twin-id> -r <relationship-id>");

            get.AddCommand<RelationshipGetAllCommand>("all")
                .WithDescription("Get all relationships for twin.")
                .WithExample("twin relationship get all --tenantId -a <adt-instance-url> -t <twin-id>");

            get.AddCommand<RelationshipGetIncomingCommand>("incoming")
                .WithDescription("Get all incoming relationships for twin.")
                .WithExample("twin relationship get incoming --tenantId -a <adt-instance-url> -t <twin-id>");
        });
}