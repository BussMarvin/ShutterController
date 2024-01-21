using Autofac;
using Configuration.Contract.Interfaces;
using Configuration.Contract.Interfaces.Mapping;
using Configuration.Contract.MappingKeys;
using Controller.Contract.DataModels;
using Database.Contract.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database;

internal class DatabaseConfiguration : IDatabaseConfiguration<ControlDatabaseContext>
{
    private readonly ILifetimeScope _lifetimeScope;

    public DatabaseConfiguration(ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
    }


    public DbContextOptions<ControlDatabaseContext> GetDatabaseOptions()
    {
        return new DbContextOptionsBuilder<ControlDatabaseContext>()
            .UseSqlite(GetConnectionString())
            .Options;
    }


    private string GetConnectionString()
    {
        IConfigurationPathMappingKeyLocationConfigurator mappingKeyCreator = new ConfigurationPathMappingKeyType();
        IConfigurationPathMappingKey key = mappingKeyCreator.SetApplicationPath().SetConfigurationType(ConfigurationTypeMappingKeys.SecureConfiguration);

        string databasePathWithFileInformation =
            _lifetimeScope.ResolveKeyed<IConfigurationManager<Configuration_DataModel>>(key.GetConfigurationPath()).Load().DatabaseConnectionString;
        return databasePathWithFileInformation;
    }
}