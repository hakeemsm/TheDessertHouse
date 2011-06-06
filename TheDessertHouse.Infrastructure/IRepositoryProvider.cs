using NHibernate;

namespace TheDessertHouse.Infrastructure
{
    public interface IRepositoryProvider
    {
        IRepository GetRepository();
        ISessionFactory SessionFactory { get; set; }
    }

    public class RepositoryProvider : IRepositoryProvider
    {
        public IRepository GetRepository()
        {
            return new Repository(SessionFactory);
        }

        public ISessionFactory SessionFactory
        {
            get; set;
        }
    }

    
}