using AtentoFramework2008.Infrastructure.Entity;
using LiveDev.Domain;

namespace LiveDev.Infrastructure.Repositories
{
    public class QuestionsRepository: Repository<Question>
    {
        public virtual Question GetById(int id)
        {
            return this[id];
        }
    }
}