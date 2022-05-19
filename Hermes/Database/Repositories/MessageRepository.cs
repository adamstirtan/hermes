using Hermes.Models;

namespace Hermes.Database.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(ApplicationDbContext context)
            : base(context)
        { }
    }
}