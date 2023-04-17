using QuizzWeb.Core.Models;

namespace QuizzWeb.Core.Contracts
{
    public interface IQuizzService
    {
        Task<IEnumerable<QuizzModel>> GetAllAsync();

        Task<QuizzModel> GetQuizzAsync(int id);

        Task CreateQuizzAsync(QuizzModel model);

        Task UpdateAsync(QuizzModel model);

        Task DeleteAsync(int id);
    }
}
