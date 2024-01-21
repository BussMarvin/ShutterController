using System.Reflection;
using Autofac;
using Autofac.Core;
using Bootstrapper.Contract.Attributes;
using Database.Contract.Interfaces;
using Microsoft.EntityFrameworkCore;
using Module = Autofac.Module;

namespace Database.Mapping;

[BuildRegister]
public class DatabaseMapping : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        DatabaseConfiguration<ControlDatabaseContext>(builder);
        RegisterDbContext<ControlDatabaseContext>(builder);
        RegisterRepository<ControlDatabaseContext>(builder);
        RegisterServiceDatabase(builder);
    }

    private void DatabaseConfiguration<TDatabaseContext>(ContainerBuilder builder) where TDatabaseContext : DbContext
    {
        builder.RegisterType<DatabaseConfiguration>()
            .As<IDatabaseConfiguration<TDatabaseContext>>()
            .InstancePerLifetimeScope();
    }

    private void RegisterDbContext<TDatabaseContext>(ContainerBuilder builder) where TDatabaseContext : DbContext
    {
        builder.RegisterType<TDatabaseContext>()
            .WithParameter(new ResolvedParameter(
                (pi, _) => pi.ParameterType == typeof(DbContextOptions<TDatabaseContext>),
                (_, ctx) => ctx.Resolve<IDatabaseConfiguration<TDatabaseContext>>().GetDatabaseOptions()))
            .AsSelf()
            .SingleInstance();
    }


    private void RegisterRepository<TDatabaseContext>(ContainerBuilder builder)
    {
        Type dbContextType = typeof(TDatabaseContext);
        Type dbSetType = typeof(DbSet<>);

        foreach (PropertyInfo property in dbContextType.GetProperties())
        {
            if (property.PropertyType.IsGenericType &&
                property.PropertyType.GetGenericTypeDefinition() == dbSetType)
            {
                Type entityType = property.PropertyType.GetGenericArguments()[0];
                object? propertyInfo = dbContextType.GetProperty(property.Name);


                MethodInfo? method = typeof(DatabaseMapping).GetMethods()
                    .FirstOrDefault(m => m.Name == "RegisterGenericRepository");

                if (method != null)
                {
                    MethodInfo genericMethod = method.MakeGenericMethod(entityType);
                    genericMethod.Invoke(null, new[] { builder, propertyInfo });
                }
            }
        }
    }


    public static void RegisterGenericRepository<T>(ContainerBuilder builder, PropertyInfo propertyInfo) where T : class, new()
    {
        builder.RegisterType(typeof(GenericRepository<T>))
            .As(typeof(IGenericRepository<T>))
            .WithParameter(new ResolvedParameter(
                (pi, ctx) => pi.ParameterType == typeof(DbSet<T>),
                (pi, ctx) => propertyInfo.GetValue(ctx.Resolve<ControlDatabaseContext>())))
            .SingleInstance();
    }


    private void RegisterServiceDatabase(ContainerBuilder builder)
    {
        builder.RegisterType<ControlDatabaseUnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();
    }
}