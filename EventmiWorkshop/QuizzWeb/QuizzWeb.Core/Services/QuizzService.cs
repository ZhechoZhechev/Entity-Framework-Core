namespace QuizzWeb.Core.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using QuizzWeb.Core.Contracts;
using QuizzWeb.Core.Models;
using QuizzWeb.Infrastructure.Data.Common;
using QuizzWeb.Infrastructure.Data.Models;


public class QuizzService : IQuizzService
{
    private readonly IRepository repository;

    public QuizzService(IRepository repository)
    {
        this.repository = repository;
    }

    public async Task CreateQuizzAsync(QuizzModel model)
    {
        Quizz quizz = new Quizz() 
        {
            Name = model.Name
        };

        await repository.AddAsync(quizz);
        await repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var quizzToDelete = await repository.GetByIdAsync<Quizz>(id);

        if (quizzToDelete == null) 
        {
            throw new ArgumentException("Quizz with such Id does not exist", nameof(id));
        }

        quizzToDelete.isActive = false;
        await repository.SaveChangesAsync();
    }

    public async Task<IEnumerable<QuizzModel>> GetAllAsync()
    {
        return await repository.AllReadonly<Quizz>()
            .Where(q => q.isActive == true)
            .Select(q => new QuizzModel() 
            {
                Id = q.Id,
                Name = q.Name
            })
            .OrderBy(q => q.Name)
            .ToListAsync();
    }

    public async Task<QuizzModel> GetQuizzAsync(int id)
    {
        var model = await repository.GetByIdAsync<Quizz>(id);

        if (model == null)
        {
            throw new ArgumentException("Invalid Id!");
        }

        return new QuizzModel() 
        {
            Id = model.Id,
            Name = model.Name
        };
    }

    public async Task UpdateAsync(QuizzModel model)
    {
        var entityToUpdate = await repository.GetByIdAsync<Quizz>(model.Id);

        if (entityToUpdate == null) 
        {
            throw new ArgumentException("Entity with such Id does not exist", nameof(model.Id));
        }

        entityToUpdate.Name = model.Name;

        await repository.SaveChangesAsync();
    }
}
