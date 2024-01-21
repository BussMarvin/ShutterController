using Microsoft.EntityFrameworkCore;

namespace Database.Contract.Interfaces;

/// <summary>
///     Interface for database configuration.
/// </summary>
public interface IDatabaseConfiguration<T> where T : DbContext
{
    /// <summary>
    ///     Gets the database options.
    /// </summary>
    /// <returns>
    ///     <see cref="DbContextOptions" />
    /// </returns>
    DbContextOptions<T> GetDatabaseOptions();
}