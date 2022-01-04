namespace Atc.Iot.DigitalTwin.Cli.Extensions;

public static class CommandAppExtensions
{
    public static void ConfigureCommands(this CommandApp app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.Configure(config =>
        {
            config.AddBranch("eventroute", ConfigureEventRouteCommands());
            config.AddBranch("model", ConfigureModelCommands());
            config.AddBranch("relationship", ConfigureRelationshipCommands());
            config.AddBranch("twin", ConfigureTwinCommands());
        });
    }

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

    private static void ConfigureEventRouteGetCommands(IConfigurator<CommandSettings> node)
        => node.AddBranch("get", get =>
        {
            get.SetDescription("Operations related to reading event routes.");

            get.AddCommand<EventRouteGetSingleCommand>("single")
                .WithDescription("Get single event route.");

            get.AddCommand<EventRouteGetAllCommand>("all")
                .WithDescription("Get all event routes.");
        });

    private static Action<IConfigurator<CommandSettings>> ConfigureModelCommands()
        => node =>
        {
            node.SetDescription("Operations related to models.");

            node.AddCommand<ModelDecommissionCommand>("decommission")
                .WithDescription("Decommission a single model.");

            ConfigureModelDeleteCommands(node);
            ConfigureModelGetCommands(node);
            ConfigureModelUploadCommands(node);

            node.AddCommand<ModelValidateCommand>("validate")
                .WithDescription("Validate models.");
        };

    private static void ConfigureModelGetCommands(IConfigurator<CommandSettings> node)
        => node.AddBranch("get", get =>
        {
            get.SetDescription("Operations related to reading models.");

            get.AddCommand<ModelGetSingleCommand>("single")
                .WithDescription("Get single model.");

            get.AddCommand<ModelGetAllCommand>("all")
                .WithDescription("Get all models.");
        });

    private static void ConfigureModelDeleteCommands(IConfigurator<CommandSettings> node)
        => node.AddBranch("delete", delete =>
        {
            delete.SetDescription("Operations related to deleting models.");

            delete.AddCommand<ModelDeleteSingleCommand>("single")
                .WithDescription("Delete single model.");

            delete.AddCommand<ModelDeleteAllCommand>("all")
                .WithDescription("Delete all models.");
        });

    private static void ConfigureModelUploadCommands(IConfigurator<CommandSettings> node)
        => node.AddBranch("upload", upload =>
        {
            upload.SetDescription("Operations related to uploading models.");

            upload.AddCommand<ModelUploadSingleCommand>("single")
                .WithDescription("Upload single model.");

            upload.AddCommand<ModelUploadAllCommand>("all")
                .WithDescription("Upload all models.");
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

            node.AddCommand<TwinUpdateCommand>("create")
                .WithDescription("Update single twin.");
        };

    private static void ConfigureTwinDeleteCommands(IConfigurator<CommandSettings> node)
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
            node.SetDescription("Operations related to relationships.");

            node.AddCommand<RelationshipCreateCommand>("create")
                .WithDescription("Create relationship.");

            node.AddCommand<RelationshipDeleteCommand>("delete")
                .WithDescription("Delete relationship.");

            ConfigureRelationshipGetCommands(node);
        };

    private static void ConfigureRelationshipGetCommands(IConfigurator<CommandSettings> node)
        => node.AddBranch("get", get =>
        {
            get.SetDescription("Operations related to reading relationships.");

            get.AddCommand<RelationshipGetSingleCommand>("single")
                .WithDescription("Get single relationship for twin.");

            get.AddCommand<RelationshipGetAllCommand>("all")
                .WithDescription("Get all relationships for twin.");

            get.AddCommand<RelationshipGetIncomingCommand>("incoming")
                .WithDescription("Get all incoming relationships for twin.");
        });
}