namespace Atc.Azure.DigitalTwin.CLI.Extensions;

public static class CommandAppExtensions
{
    public static void ConfigureCommands(
        this CommandApp app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.Configure(config =>
        {
            //// TODO: Implement
            //// data-history    : Manage and configure data history.
            //// job             : Manage and configure jobs for a digital twin instance.
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
                .WithDescription("Decommission a single model.");

            ConfigureModelDeleteCommands(node);

            ConfigureModelGetCommands(node);

            node.AddCommand<ModelValidateCommand>("validate")
                .WithDescription("Validate models.");
        };

    private static void ConfigureModelCreateCommands(
        IConfigurator<CommandSettings> node)
        => node.AddBranch("create", upload =>
        {
            upload.SetDescription("Operations related to creating models.");

            upload.AddCommand<ModelCreateSingleCommand>("single")
                .WithDescription("Create single model.");

            upload.AddCommand<ModelCreateAllCommand>("all")
                .WithDescription("Create all models.");
        });

    private static void ConfigureModelDeleteCommands(
        IConfigurator<CommandSettings> node)
        => node.AddBranch("delete", delete =>
        {
            delete.SetDescription("Operations related to deleting models.");

            delete.AddCommand<ModelDeleteSingleCommand>("single")
                .WithDescription("Delete single model.");

            delete.AddCommand<ModelDeleteAllCommand>("all")
                .WithDescription("Delete all models.");
        });

    private static void ConfigureModelGetCommands(
        IConfigurator<CommandSettings> node)
        => node.AddBranch("get", get =>
        {
            get.SetDescription("Operations related to reading models.");

            get.AddCommand<ModelGetSingleCommand>("single")
                .WithDescription("Get single model.");

            get.AddCommand<ModelGetAllCommand>("all")
                .WithDescription("Get all models.");
        });

    private static Action<IConfigurator<CommandSettings>> ConfigureEventRouteCommands()
        => node =>
        {
            node.SetDescription("Operations related to event routes.");

            node.AddCommand<EventRouteCreateCommand>("create")
                .WithDescription("Create event route.");

            node.AddCommand<EventRouteDeleteCommand>("delete")
                .WithDescription("Delete event route.");

            ConfigureEventRouteGetCommands(node);
        };

    private static void ConfigureEventRouteGetCommands(
        IConfigurator<CommandSettings> node)
        => node.AddBranch("get", get =>
        {
            get.SetDescription("Operations related to reading event routes.");

            get.AddCommand<EventRouteGetSingleCommand>("single")
                .WithDescription("Get single event route.");

            get.AddCommand<EventRouteGetAllCommand>("all")
                .WithDescription("Get all event routes.");
        });

    private static Action<IConfigurator<CommandSettings>> ConfigureTwinCommands()
        => node =>
        {
            node.SetDescription("Operations related to twins.");

            node.AddCommand<TwinCountCommand>("count")
                .WithDescription("Count the different types of twins.");

            node.AddCommand<TwinCreateCommand>("create")
                .WithDescription("Create single twin.");

            ConfigureTwinDeleteCommands(node);

            node.AddCommand<TwinGetCommand>("get")
                .WithDescription("Get single twin.");

            node.AddCommand<TwinUpdateCommand>("update")
                .WithDescription("Update single twin.");

            node.AddBranch("relationship", ConfigureRelationshipCommands());
        };

    private static void ConfigureTwinDeleteCommands(
        IConfigurator<CommandSettings> node)
        => node.AddBranch("delete", delete =>
        {
            delete.SetDescription("Operations related to deleting twins.");

            delete.AddCommand<TwinDeleteSingleCommand>("single")
                .WithDescription("Delete single twin.");

            delete.AddCommand<TwinDeleteAllCommand>("all")
                .WithDescription("Delete all twins.");

            delete.AddCommand<TwinDeleteAllByModelCommand>("allByModel")
                .WithDescription("Delete all twins by a modelId.");
        });

    private static Action<IConfigurator<CommandSettings>> ConfigureRelationshipCommands()
        => node =>
        {
            node.SetDescription("Operations related to twin relationships.");

            node.AddCommand<RelationshipCreateCommand>("create")
                .WithDescription("Create relationship.");

            node.AddCommand<RelationshipDeleteCommand>("delete")
                .WithDescription("Delete relationship.");

            ConfigureRelationshipGetCommands(node);
        };

    private static void ConfigureRelationshipGetCommands(
        IConfigurator<CommandSettings> node)
        => node.AddBranch("get", get =>
        {
            get.SetDescription("Operations related to reading twin relationships.");

            get.AddCommand<RelationshipGetSingleCommand>("single")
                .WithDescription("Get single relationship for twin.");

            get.AddCommand<RelationshipGetAllCommand>("all")
                .WithDescription("Get all relationships for twin.");

            get.AddCommand<RelationshipGetIncomingCommand>("incoming")
                .WithDescription("Get all incoming relationships for twin.");
        });
}