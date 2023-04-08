namespace Eventmi.Infrastructure.Data;

using Eventmi.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Контекст описващ базата данни
/// </summary>
public class EventmiDbContext : DbContext
{
    /// <summary>
    /// Създава контекст без настройки
    /// </summary>
    public EventmiDbContext()
    {
    }

    /// <summary>
    /// Създава контекст с избрани настройки
    /// </summary>
    /// <param name="options">настройки на контекст</param>
    public EventmiDbContext(DbContextOptions<EventmiDbContext> options) :
        base(options)
    {
    }

    /// <summary>
    /// таблица със събития
    /// </summary>
    public DbSet<Event> Events { get; set; } = null!;
}
