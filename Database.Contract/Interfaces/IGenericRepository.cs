namespace Database.Contract.Interfaces;

/// <summary>
///     Interface for generic repository.
/// </summary>
public interface IGenericRepository<T> where T : class, new()
{
    /// <summary>
    ///     Gets the entity by id.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns>A T.</returns>
    T GetById(int id);

    /// <summary>
    ///     Gets all entities.
    /// </summary>
    /// <returns>A list of TS.</returns>
    IEnumerable<T> GetAll();

    /// <summary>
    ///     Adds the entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    void Add(T entity);

    /// <summary>
    ///     Deletes the entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    void Delete(T entity);

    /// <summary>
    ///     Updates the entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    void Update(T entity);
}