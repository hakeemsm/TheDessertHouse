using NHibernate;

namespace TheDessertHouse.Infrastructure
{
    public interface IRepositoryProvider
    {
        IRepository GetRepository();
        ISessionFactory SessionFactory { get; set; }
    }
}