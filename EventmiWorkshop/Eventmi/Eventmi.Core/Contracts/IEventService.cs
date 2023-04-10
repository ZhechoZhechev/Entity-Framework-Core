using Eventmi.Core.Models;

namespace Eventmi.Core.Contracts;
/// <summary>
/// Услуга за управление на събития
/// </summary>
public interface IEventService
{
    /// <summary>
    /// Добавяне на събитие
    /// </summary>
    /// <param name="entity">събитието, което ще е добавено</param>
    /// <returns></returns>
    Task AddAsync(EventDto entity);

    /// <summary>
    /// Премахване на събитие
    /// </summary>
    /// <param name="id">Идентификаторът на събитието, което ще е премахнато</param>
    /// <returns></returns>
    Task DeleteAsync(int id);

    /// <summary>
    /// Редактиране на събитие
    /// </summary>
    /// <param name="entity">Идентификаторът на събитието, което ще е редактирано</param>
    /// <returns></returns>
    Task UpdateAsync(EventDto entity);

    /// <summary>
    /// Получаване на колекция от всички събития
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<EventDto>> GetAllAsync();

    /// <summary>
    /// Получаване на едно събитие
    /// </summary>
    /// <param name="id">Идентификатор на събитието, което искаме да получим</param>
    /// <returns></returns>
    Task<EventDto> GetEventAsync(int id);
}
