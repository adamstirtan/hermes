using Hermes.Models;

namespace Hermes.Database.Repositories
{
    public interface IMessageRepository : IRepository<Message>, IRepositoryAsync<Message>
    { }
}