using Eventmi.Core.Contracts;
using Eventmi.Core.Models;
using Eventmi.Infrastructure.Data.Common;
using Eventmi.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventmi.Core.Services;

public class EventService : IEventService
{
    private readonly IRepository repository;

    public EventService(IRepository repository)
    {
        this.repository = repository;
    }
    public async Task AddAsync(EventDto entity)
    {
        var eventToAdd = new Event()
        {
            Name = entity.Name,
            Start = entity.Start,
            End = entity.End,
            Place = entity.Place
        };

        await repository.AddAsync(eventToAdd);
        await repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await repository.DeleteAsync<Event>(id);
        await repository.SaveChangesAsync();
    }

    public async Task<IEnumerable<EventDto>> GetAllAsync()
    {
        return await repository.AllReadonly<Event>()
            .Select(e => new EventDto()
            {
                Id = e.Id,
                Name = e.Name,
                Start = e.Start,
                End = e.End,
                Place = e.Place

            })
            .OrderBy(e => e.Start)
            .ThenBy(e => e.Name)
            .ToListAsync();
    }

    public async Task<EventDto> GetEventAsync(int id)
    {
        var entity = await repository.GetByIdAsync<Event>(id);

        if (entity == null) 
            throw new ArgumentException("Невалиден идентификатор", nameof(id));

        return new EventDto() 
        {
            Name= entity.Name,
            Start= entity.Start,
            End= entity.End,
            Place= entity.Place
        };
    }

    public async Task UpdateAsync(EventDto entity)
    {
        var entityToUpdate = await repository.GetByIdAsync<Event>(entity.Id);

        if (entity == null)
            throw new ArgumentException("Невалиден идентификатор", nameof(entity.Id));

        entityToUpdate.Name = entity.Name;
        entityToUpdate.Start = entity.Start;
        entityToUpdate.End = entity.End;
        entityToUpdate.Place = entity.Place;

        await repository.SaveChangesAsync();
    }
}
